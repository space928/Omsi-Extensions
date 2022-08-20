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

namespace OmsiHookRPCPlugin
{
    public class OmsiHookRPCPlugin
    {
        public const int MAX_CLIENTS = 16;
        /// <summary>
        /// Forces all RPC to occur in the main thread. Has a slight performance impact but is usually much more stable.
        /// </summary>
        public const bool SINGLE_THREADED_EXECTION = true;

        private static NamedPipeServerStream pipe;
        private static List<Thread> threadPool;
        private static ConcurrentQueue<MethodData> callQueue;
        private static ArrayPool<byte> argumentArrayPool;
        private static List<ReturnData> returnPool;

        private class ReturnData
        {
            public readonly ManualResetEventSlim isReady;
            public int val;

            public ReturnData(ManualResetEventSlim isReady, int val)
            {
                this.isReady = isReady;
                this.val = val;
            }
        }

        private struct MethodData
        {
            public readonly RemoteMethod method;
            public readonly byte[] args;
            public readonly int threadId;

            public MethodData(RemoteMethod method, byte[] args, int threadId)
            {
                this.method = method;
                this.args = args;
                this.threadId = threadId;
            }
        }

        private static readonly ReadOnlyDictionary<RemoteMethod, int> RemoteMethodsArgsSizes = new(new Dictionary<RemoteMethod, int>()
        {
            { RemoteMethod.TProgManMakeVehicle,         -1 },
            { RemoteMethod.TTempRVListCreate,           -1 },
            { RemoteMethod.TProgManPlaceRandomBus,      -1 },
            { RemoteMethod.GetMem,                      -1 },
            { RemoteMethod.FreeMem,                     -1 },
        });

        private static void Log(object msg) => File.AppendAllText("omsiHookRPCPluginLog.txt", $"[{DateTime.Now:dd/MM/yy HH:mm:ss:ff}] {msg}\n");

        /// <summary>
        /// Plugin entry point. 
        /// </summary>
        /// <param name="aOwner"></param>
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginStart))]
        // ReSharper disable once UnusedMember.Global
        public static void PluginStart(IntPtr aOwner)
        {
            File.Delete("omsiHookRPCPluginLog.txt");
            Log("PluginStart()");
            Log("Opening name pipe...");

            argumentArrayPool = ArrayPool<byte>.Create(256,8);

            if (SINGLE_THREADED_EXECTION)
            {
                callQueue = new();
                returnPool = new(1);
            }

            // We expect server threads to have a relatively long lifetime so for now, no need to have a complicated thread pool implementation.
            threadPool = new() { new(ServerThreadStart) };
        }

        private static void ServerThreadStart()
        {
            pipe = new(PIPE_NAME, PipeDirection.InOut, MAX_CLIENTS, PipeTransmissionMode.Byte);
            pipe.WaitForConnection();

            Log($"Client has connected to server thread {threadPool.Count}.");

            // Now that this thread is occupied open a new one with a new pipe server
            // TODO: Potential race condition here.
            lock(threadPool)
                threadPool.Add(new(ServerThreadStart));

            // Add a spot for this thread in the return pool
            // TODO: Race condition?
            int threadId;
            lock (returnPool)
            {
                threadId = returnPool.Count;
                returnPool.Add(new(new(false), 0));
            }

            using BinaryReader reader = new(pipe);
            using BinaryWriter writer = new(pipe);
            while (pipe.IsConnected)
            {
                // Read the message type from the pipe
                RemoteMethod method = (RemoteMethod) reader.ReadInt32();

                int argBytes = RemoteMethodsArgsSizes[method];
                byte[] args = argumentArrayPool.Rent(argBytes);

                // Read all the arguments into a byte array
                int read = reader.Read(args, 0, argBytes);
                if (read < argBytes)
                {
                    Log($"Only read {read} out of {argBytes} bytes of arguments for {method} call!");
                    continue;
                }
                callQueue.Enqueue(new(method, args, threadId));

                // Wait for method to be executed and write back it's return value
                returnPool[threadId].isReady.Wait();
                writer.Write(returnPool[threadId].val);
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
                        BitConverter.ToInt32(methodData.args, argInd+=4),
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
                        BitConverter.ToInt32(methodData.args, argInd += 4),
                        BitConverter.ToInt32(methodData.args, argInd += 4)
                    );
                    break;
                case RemoteMethod.TProgManPlaceRandomBus:
                    ret = NativeImports.TProgManPlaceRandomBus(
                        BitConverter.ToInt32(methodData.args, argInd += 4),
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
                    ret = NativeImports.GetMem(
                        BitConverter.ToInt32(methodData.args, argInd += 4)
                    );
                    break;
                case RemoteMethod.FreeMem:
                    NativeImports.FreeMem(
                        BitConverter.ToInt32(methodData.args, argInd += 4)
                    );
                    break;
                default:
                    Log($"Unknown message type: {methodData.method} encountered!");
                    break;
            }

            returnPool[methodData.threadId].val = ret;
            returnPool[methodData.threadId].isReady.Set();
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
