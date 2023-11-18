using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OmsiHookRPCPlugin;

namespace OmsiHook
{
    /// <summary>
    /// All of these methods will only work if called from a native Omsi plugin or if the OmsiHookRPCPlugin is installed.
    /// These methods also rely on OmsiHookInvoker.dll which must be in the Omsi plugins folder.
    /// </summary>
	public static class OmsiRemoteMethods
	{
        private static NamedPipeClientStream pipeRX;
        private static NamedPipeClientStream pipeTX;
        private static ConcurrentDictionary<int, TaskCompletionSource<int>> resultPromises;
        private static Task resultReaderThread;
        private static Memory memory;

        private static readonly ThreadLocal<byte[]> asyncWriteBuff = new(() => new byte[256]);

        public static bool IsInitialised => (pipeRX?.IsConnected ?? false) && (pipeTX?.IsConnected ?? false);

        // TODO: Okay maybe this shouldn't be static... Singleton?
        internal static async Task InitRemoteMethods(Memory omsiMemory, bool inifiniteTimeout = false)
        {
            memory = omsiMemory;

#if !OMSI_PLUGIN
            resultPromises = new();

            // We swap rx and tx here so that it makes semantic sense (since the tx of the client goes to the rx of the server)
            pipeTX = new(".", OmsiHookRPCMethods.PIPE_NAME_RX, PipeDirection.Out);
            pipeRX = new(".", OmsiHookRPCMethods.PIPE_NAME_TX, PipeDirection.In);
            //pipe.ReadMode = PipeTransmissionMode.Message;
            try
            {
                await pipeTX.ConnectAsync(inifiniteTimeout ? Timeout.Infinite : 20000);
                Console.WriteLine("pipeTX Connected!");
                await pipeRX.ConnectAsync(inifiniteTimeout ? Timeout.Infinite : 20000);
                Console.WriteLine("pipeRX Connected!");
            }
            catch(TimeoutException)
            {
                pipeRX = null;
                pipeTX = null;
                throw new TimeoutException("Couldn't manage to connect to OmsiHookRPCPlugin within 20 seconds! Check that it is loaded correctly.");
            }

            resultReaderThread = new Task(ResultReaderTask);
            resultReaderThread.Start();
#endif
        }

        /// <summary>
        /// Creates a new result promise and adds it to the promises dictionary.
        /// </summary>
        /// <returns>The hashcode of the new promise.</returns>
        private static (int promiseHash, TaskCompletionSource<int> promise)CreateResultPromise()
        {
            TaskCompletionSource<int> resultPromise = new();
            int resultHash;
            do
            {
                resultHash = Random.Shared.Next();
            } while (!resultPromises.TryAdd(resultHash, resultPromise));
            return (resultHash, resultPromise);
        }

        private static void ResultReaderTask()
        {
            using BinaryReader reader = new(pipeRX);
            while(pipeRX.IsConnected)
            {
                int promiseHash = reader.ReadInt32();
                int value = reader.ReadInt32();

                resultPromises.TryRemove(promiseHash, out var promise);
                promise.SetResult(value);
            }
        }

        [Obsolete]
        public static int MakeVehicle()
        {
            int vehList = TTempRVListCreate(0x0074802C, 1);
            string path = @"Vehicles\GPM_MAN_LionsCity_M\MAN_A47.bus";
            int mem = memory.AllocateString(path, false).Result;

            return TProgManMakeVehicle(memory.ReadMemory<int>(0x00862f28), vehList,
                memory.ReadMemory<int>(0x008615A8), false, false,
              0, false, false, false, false,
              -1, true, 0, (byte)3, false,
              0, 0, 0, 0, 0, false,
              false, true, true, mem);
        }

        /// <summary>
        /// Spawns a random bus in the map at one of the entry points.
        /// EXPERIMENTAL: The parameters of this method default to known working values, changing them may result in game crashes.
        /// </summary>
        /// <param name="aiType"></param>
        /// <param name="group"></param>
        /// <param name="type"></param>
        /// <param name="scheduled"></param>
        /// <param name="tour"></param>
        /// <param name="line"></param>
        /// <returns>The index of the vehicle that was placed.</returns>
        public static int PlaceRandomBus(int aiType = 0, int group = 1, int type = -1, bool scheduled = false, int tour = 0, int line = 0)
        {
#if OMSI_PLUGIN
            return TProgManPlaceRandomBus(memory.ReadMemory<int>(0x00862f28), aiType, group, 0, false, true, type, scheduled, 0, tour, line);
#else
            if (!IsInitialised)
                throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

            int argPos = 0;
            var method = OmsiHookRPCMethods.RemoteMethod.TProgManPlaceRandomBus;
            Span<byte> writeBuffer = stackalloc byte[OmsiHookRPCMethods.RemoteMethodsArgsSizes[method] + 8];
            //Span<byte> readBuffer = stackalloc byte[4];
            (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
            BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)method);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], resultPromise);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], aiType);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], group);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], 0);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 1)..], false);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 1)..], true);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], type);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 1)..], scheduled);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], 0);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], aiType);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], tour);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], line);
            lock (pipeTX)
                pipeTX.Write(writeBuffer);
            return promise.Task.Result;
            //lock (pipeRX)
            //    pipeRX.Read(readBuffer);
            //return BitConverter.ToInt32(readBuffer);
#endif
        }

        /// <summary>
        /// Allocates memory in Omsi using it's own memory allocator.
        /// EXPERIMENTAL: A lot of messy stuff has to work for this to not crash.
        /// </summary>
        /// <param name="length">How many bytes to allocate</param>
        /// <returns>A pointer to the newly allocated memory (note that you made need to
        /// <c>VirtualProtect</c> it to access it).</returns>
        public static async Task<uint> OmsiGetMem(int length)
        {
#if OMSI_PLUGIN
            return (uint)GetMem(length);
#else
            if(!IsInitialised) 
                return 0;

            int argPos = 0;
            var method = OmsiHookRPCMethods.RemoteMethod.GetMem;
            int writeBufferSize = OmsiHookRPCMethods.RemoteMethodsArgsSizes[method] + 8;
            // This should be thread safe as the asyncWriteBuff is thread local
            byte[] writeBuffer = asyncWriteBuff.Value;
            (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos)..], (int)method);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], resultPromise);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], length);
            lock (pipeTX)
                pipeTX.Write(writeBuffer.AsSpan()[..writeBufferSize]);
            return (uint)await promise.Task;
            /*lock (pipeRX)
                pipeRX.Read(readBuffer);
            return BitConverter.ToUInt32(readBuffer);*/
#endif
        }

        /// <summary>
        /// Frees memory in Omsi using it's own memory allocator. This method might return before the memory has been freed.
        /// EXPERIMENTAL: A lot of messy stuff has to work for this to not crash.
        /// </summary>
        /// <param name="addr">The pointer to the object to deallocate</param>
        public static void OmsiFreeMemAsync(int addr)
        {
#if OMSI_PLUGIN
            FreeMem(addr);
#else
            if (!IsInitialised)
                throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

            int argPos = 0;
            var method = OmsiHookRPCMethods.RemoteMethod.FreeMem;
            Span<byte> writeBuffer = stackalloc byte[OmsiHookRPCMethods.RemoteMethodsArgsSizes[method] + 8];
            (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
            BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)method);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], resultPromise);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], addr);
            lock (pipeTX)
                pipeTX.Write(writeBuffer);
            //promise.AsTask().Wait();
#endif
        }

        /// <summary>
        /// Attempts to get the current D3D context from Omsi, required before any of the graphics methods can be called.
        /// </summary>
        public static bool OmsiHookD3D()
        {
#if OMSI_PLUGIN
            return HookD3D() != 0;
#else
            if (!IsInitialised)
                throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

            int argPos = 0;
            Span<byte> writeBuffer = stackalloc byte[8];
            (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
            BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)OmsiHookRPCMethods.RemoteMethod.HookD3D);
            BitConverter.TryWriteBytes(writeBuffer[(argPos+=4)..], resultPromise);
            lock (pipeTX)
                pipeTX.Write(writeBuffer);
            return promise.Task.Result != 0;
#endif
        }

        /// <summary>
        /// Attempts to create a new d3d texture which can be shared with an external D3D context.
        /// </summary>
        public static async Task<(uint hresult, uint ppTexture)> OmsiCreateTextureAsync(uint width, uint height, DXGI_FORMAT format)
        {
#if OMSI_PLUGIN
            uint ppTexture = OmsiGetMem(4).Result;
            uint hresult = unchecked((uint)CreateTexture(width, height, (uint)format, ppTexture));
            return (hresult, ppTexture);
#else
            if (!IsInitialised)
                throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

            // Allocate the pointers
            uint ppTexture = OmsiGetMem(4).Result;
            memory.WriteMemory(ppTexture, 0);

            int argPos = 0;
            var method = OmsiHookRPCMethods.RemoteMethod.CreateTexture;
            // This should be thread safe as the asyncWriteBuff is thread local
            int writeBufferSize = OmsiHookRPCMethods.RemoteMethodsArgsSizes[method] + 8;
            byte[] writeBuffer = asyncWriteBuff.Value;
            (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos)..], (int)method);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], resultPromise);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], width);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], height);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], (uint)format); 
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], ppTexture);
            lock (pipeTX)
                pipeTX.Write(writeBuffer.AsSpan()[..writeBufferSize]);
            return (unchecked((uint)await promise.Task), ppTexture);
#endif
        }

        /// <summary>
        /// Attempts to create a new d3d texture which can be shared with an external D3D context.
        /// </summary>
        public static async Task<uint> OmsiUpdateTextureAsync(uint texturePtr, uint textureDataPtr, uint width, uint height, Rectangle? updateRect = null)
        {
#if OMSI_PLUGIN
            return unchecked((uint)UpdateSubresource(texturePtr, textureDataPtr, width, height, 
                updateRect.HasValue ? 1 : 0, updateRect?.left??0, updateRect?.top??0, updateRect?.right??0, updateRect?.bottom??0));
#else
            if (!IsInitialised)
                throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

            int argPos = 0;
            var method = OmsiHookRPCMethods.RemoteMethod.UpdateSubresource;
            // This should be thread safe as the asyncWriteBuff is thread local
            int writeBufferSize = OmsiHookRPCMethods.RemoteMethodsArgsSizes[method] + 8;
            byte[] writeBuffer = asyncWriteBuff.Value;
            (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos)..], (int)method);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], resultPromise);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], texturePtr);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], textureDataPtr);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], width);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], height);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], updateRect.HasValue ? 1 : 0);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], updateRect?.left ?? 0);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], updateRect?.top ?? 0);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], updateRect?.right ?? 0);
            BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], updateRect?.bottom ?? 0);
            lock (pipeTX)
                pipeTX.Write(writeBuffer.AsSpan()[..writeBufferSize]);
            return unchecked((uint)await promise.Task);
#endif
        }

        [DllImport("OmsiHookInvoker.dll")]
        private static extern int TProgManMakeVehicle(int progMan, int vehList, int _RoadVehicleTypes, bool onlyvehlist, bool CS,
            float TTtime, bool situationload, bool dialog, bool setdriver, bool thread,
            int kennzeichen_index, bool initcall, int startday, byte trainbuilddir, bool reverse,
            int grouphof, int typ, int tour, int line, int farbschema, bool Scheduled,
            bool AIRoadVehicle, bool kennzeichen_random, bool farbschema_random, int filename);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int TTempRVListCreate(int classAddr, int capacity);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int TProgManPlaceRandomBus(int progMan, int aityp,
            int group, float TTtime, bool thread, bool instantCopy, int _typ,
            bool scheduled, int startDay, int tour, int line);
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int GetMem(int length);
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern void FreeMem(int addr);
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int HookD3D();
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int CreateTexture(uint Width, uint Height, uint Format, uint ppTexture);
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int UpdateSubresource(uint Texture, uint TextureData, uint Width, uint Height, int UseRect, uint Left, uint Top, uint Right, uint Bottom);

        public enum DXGI_FORMAT : uint
        {
            R16G16B16A16_FLOAT = 10,
            R10G10B10A2_UNORM = 24,
            R8G8B8A8_UNORM = 28
        }

        public struct Rectangle
        {
            public uint left;
            public uint top;
            public uint right;
            public uint bottom;
        }
    }
}
