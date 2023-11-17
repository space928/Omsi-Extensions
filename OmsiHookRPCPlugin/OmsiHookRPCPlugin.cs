using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.IO.Pipes;
using System.Threading;
using DNNE;
using static OmsiHookRPCPlugin.OmsiHookRPCMethods;
using System.Collections.Concurrent;
using System.Buffers;
using System.Threading.Tasks;
using System.Reflection;

namespace OmsiHookRPCPlugin
{
    public class OmsiHookRPCPlugin
    {
        public const int MAX_CLIENTS = 8;
        /// <summary>
        /// Forces all RPC to occur in the main thread. Has a slight performance impact but is usually much more stable.
        /// </summary>
        public const bool SINGLE_THREADED_EXECTION = true;

        private static List<Thread> threadPool;
        private static ConcurrentQueue<MethodData> callQueue;
        private static ArrayPool<byte> argumentArrayPool;
        private static List<ReturnData> returnPool;
        private static readonly object logLock = new();
        private static readonly object writeMutex = new();

        internal record struct ReturnPromise(int Val, int Promise);

        private struct ReturnData
        {
            public readonly SemaphoreSlim isReady;
            public ConcurrentQueue<ReturnPromise> values;

            public ReturnData()
            {
                this.isReady = new(0);
                this.values = new();
            }
        }

        private readonly struct MethodData
        {
            public readonly RemoteMethod method;
            public readonly byte[] args;
            public readonly int threadId;
            public readonly int returnPromise;

            public MethodData(RemoteMethod method, byte[] args, int threadId, int returnPromise)
            {
                this.method = method;
                this.args = args;
                this.threadId = threadId;
                this.returnPromise = returnPromise;
            }
        }

        private static void Log(object msg)
        {
            lock (logLock)
            {
                File.AppendAllText("omsiHookRPCPluginLog.txt", $"[{DateTime.Now:dd/MM/yy HH:mm:ss:ff}] {msg}\n");
            }
        }

        /// <summary>
        /// Plugin entry point. 
        /// </summary>
        /// <param name="aOwner"></param>
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginStart))]
        // ReSharper disable once UnusedMember.Global
        public static void PluginStart(IntPtr aOwner)
        {
            File.Delete("omsiHookRPCPluginLog.txt");
            Log("############## Omsi Hook RPC Plugin ##############");
            Log($"    version: {Assembly.GetExecutingAssembly().GetName().Version}");
            Log($@"Starting RPC server on named pipe: \\.\pipe\{PIPE_NAME_RX} and \\.\pipe\{PIPE_NAME_TX} with {MAX_CLIENTS} threads...");

            argumentArrayPool = ArrayPool<byte>.Create(256,8);

            if (SINGLE_THREADED_EXECTION)
            {
                callQueue = new();
                returnPool = new(MAX_CLIENTS);
            }

            // We expect server threads to have a relatively long lifetime so for now, no need to have a complicated thread pool implementation.
            threadPool = new();
            for(int i = 0; i < MAX_CLIENTS; i++)
            {
                Thread thread = new(() => ServerThreadStart(i-1));
                threadPool.Add(thread);
                lock(returnPool)
                    returnPool.Add(new());
                thread.Start();
            }
        }

        private static void ServerThreadStart(int threadId)
        {
            while (true)
            {
                try
                {
                    // Log($"[RPC Server {threadId}] Init.");
                    using NamedPipeServerStream pipeRX = new(PIPE_NAME_RX, PipeDirection.In, MAX_CLIENTS, PipeTransmissionMode.Byte);
                    pipeRX.WaitForConnection();
                    Log($"[RPC Server {threadId}] Client has connected to rx.");
                    using NamedPipeServerStream pipeTX = new(PIPE_NAME_TX, PipeDirection.Out, MAX_CLIENTS, PipeTransmissionMode.Byte);
                    pipeTX.WaitForConnection();
                    Log($"[RPC Server {threadId}] Client has connected to tx.");

                    var readTask = new Task(() => ReadTask(threadId, pipeRX));
                    var writeTask = new Task(() => WriteTask(threadId, pipeTX));
                    readTask.Start();
                    writeTask.Start();

                    Task.WaitAll(readTask, writeTask);
                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    Log(ex);
                }
                Log($"[RPC Server {threadId}] Client has disconnected.");
            }
        }

        private static void WriteTask(int threadId, NamedPipeServerStream pipeTX)
        {
            using BinaryWriter writer = new(pipeTX);
            while (pipeTX.IsConnected)
            {
                returnPool[threadId].isReady.Wait();
                lock (writeMutex)
                {
                    if (returnPool[threadId].values.TryDequeue(out ReturnPromise ret))
                    {
                        Log($"[RPC Server {threadId}]    writing: {ret.Val:X} for promise: 0x{ret.Promise:X8}");
                        writer.Write(ret.Promise);
                        writer.Write(ret.Val);
                    }
                    else
                        Log($"[RPC Server {threadId}]    tried to return a result, but no result was available!");
                }
                Log($"[RPC Server {threadId}]    Done!");
            }
        }

        private static void ReadTask(int threadId, NamedPipeServerStream pipeRX)
        {
            using BinaryReader reader = new(pipeRX);
            while (pipeRX.IsConnected)
            {
                // Read the message type from the pipe
                RemoteMethod method = (RemoteMethod)reader.ReadInt32();
                int returnPromise = reader.ReadInt32();

                int argBytes = RemoteMethodsArgsSizes[method];
                byte[] args = argumentArrayPool.Rent(argBytes);
                Log($"[RPC Server {threadId}] Remote method execute: '{method}' promise: 0x{returnPromise:X8}; reading {argBytes} bytes of arguments...");

                // Read all the arguments into a byte array
                int read = reader.Read(args, 0, argBytes);
                if (read < argBytes)
                {
                    Log($"Only read {read} out of {argBytes} bytes of arguments for {method} call!");
                    continue;
                }
                callQueue.Enqueue(new(method, args, threadId, returnPromise));
                Log($"[RPC Server {threadId}]    method enqueued...");
            }
        }

        /// <summary>
        /// Parses the method arguments and executes the native method.
        /// </summary>
        /// <param name="methodData"></param>
        private static void ProcessCall(MethodData methodData)
        {
            int ret = -1;
            int argInd = 0;
            switch (methodData.method)
            {
                case RemoteMethod.TProgManMakeVehicle:
                    ret = NativeImports.TProgManMakeVehicle(
                        BitConverter.ToInt32(methodData.args, argInd),
                        BitConverter.ToInt32(methodData.args, argInd+=4),
                        BitConverter.ToInt32(methodData.args, argInd+=4),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToSingle(methodData.args, argInd+=4),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToInt32(methodData.args, argInd+=4),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToInt32(methodData.args, argInd+=4),
                        methodData.args[argInd+=1],
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToInt32(methodData.args, argInd+=4),
                        BitConverter.ToInt32(methodData.args, argInd+=4),
                        BitConverter.ToInt32(methodData.args, argInd+=4),
                        BitConverter.ToInt32(methodData.args, argInd+=4),
                        BitConverter.ToInt32(methodData.args, argInd+=4),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToBoolean(methodData.args, argInd+=1),
                        BitConverter.ToInt32(methodData.args, argInd+=4)
                    );
                    break;
                case RemoteMethod.TTempRVListCreate:
                    ret = NativeImports.TTempRVListCreate(
                        BitConverter.ToInt32(methodData.args, argInd),
                        BitConverter.ToInt32(methodData.args, argInd += 4)
                    );
                    break;
                case RemoteMethod.TProgManPlaceRandomBus:
                    ret = NativeImports.TProgManPlaceRandomBus(
                        BitConverter.ToInt32(methodData.args, argInd),
                        BitConverter.ToInt32(methodData.args, argInd += 4),
                        BitConverter.ToInt32(methodData.args, argInd += 4),
                        BitConverter.ToSingle(methodData.args, argInd += 4),
                        BitConverter.ToBoolean(methodData.args, argInd += 1),
                        BitConverter.ToBoolean(methodData.args, argInd += 1),
                        BitConverter.ToInt32(methodData.args, argInd += 4),
                        BitConverter.ToBoolean(methodData.args, argInd += 1),
                        BitConverter.ToInt32(methodData.args, argInd += 4),
                        BitConverter.ToInt32(methodData.args, argInd += 4),
                        BitConverter.ToInt32(methodData.args, argInd += 4)
                    );
                    break;
                case RemoteMethod.GetMem:
                    //Log($"[RPC Main Thread]   Executing GetMem(size={BitConverter.ToInt32(methodData.args, argInd)})");
                    ret = NativeImports.GetMem(
                        BitConverter.ToInt32(methodData.args, argInd)
                    );
                    break;
                case RemoteMethod.FreeMem:
                    ret = NativeImports.FreeMem(
                        BitConverter.ToInt32(methodData.args, argInd)
                    );
                    break;
                case RemoteMethod.HookD3D:
                    ret = NativeImports.HookD3D();
                    break;
                case RemoteMethod.CreateTexture:
                    ret = NativeImports.CreateTexture(
                        BitConverter.ToUInt32(methodData.args, argInd),
                        BitConverter.ToUInt32(methodData.args, argInd += 4),
                        BitConverter.ToUInt32(methodData.args, argInd += 4),
                        BitConverter.ToUInt32(methodData.args, argInd += 4),
                        BitConverter.ToUInt32(methodData.args, argInd += 4)
                    );
                    break;
                default:
                    Log($"Unknown message type: {methodData.method} encountered!");
                    break;
            }

            returnPool[methodData.threadId].values.Enqueue(new (ret, methodData.returnPromise));
            returnPool[methodData.threadId].isReady.Release();
            argumentArrayPool.Return(methodData.args);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginFinalize))]
        // ReSharper disable once UnusedMember.Global
        public static void PluginFinalize()
        {
            Log("Exiting OmsiHookRPCPlugin...");
        }

        [UnmanagedCallersOnly(CallConvs = new[] {typeof(CallConvCdecl)}, EntryPoint = nameof(AccessVariable))]
        public static void AccessVariable(ushort variableIndex, [C99Type("float*")] IntPtr value,
            [C99Type("__crt_bool*")] IntPtr writeValue)
        {
            if (SINGLE_THREADED_EXECTION)
            {
                while (!callQueue.IsEmpty)
                {
                    if (callQueue.TryDequeue(out MethodData call))
                    {
                        ProcessCall(call);
                    }
                }
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessTrigger))]
        public static void AccessTrigger(ushort variableIndex, [C99Type("__crt_bool*")] IntPtr triggerScript) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessStringVariable(ushort variableIndex, [C99Type("char*")] IntPtr firstCharacterAddress, [C99Type("__crt_bool*")] IntPtr writeValue) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessSystemVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue) { }
    }
}
