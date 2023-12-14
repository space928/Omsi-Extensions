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
        private static bool localPlugin;

        private static readonly ThreadLocal<byte[]> asyncWriteBuff = new(() => new byte[256]);

        public static bool IsInitialised => (pipeRX?.IsConnected ?? false) && (pipeTX?.IsConnected ?? false);

        // TODO: Okay maybe this shouldn't be static... Singleton?
        internal static async Task InitRemoteMethods(Memory omsiMemory, bool inifiniteTimeout = false, bool isLocalPlugin = false)
        {
            memory = omsiMemory;
            localPlugin = isLocalPlugin;

            if (localPlugin)
                return;

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

        public static void CloseRPCSession(bool killAllConnections)
        {
            if (!localPlugin)
                return;
            if (!IsInitialised)
                throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

            int argPos = 0;
            var method = OmsiHookRPCMethods.RemoteMethod.CloseRPCConnection;
            Span<byte> writeBuffer = stackalloc byte[OmsiHookRPCMethods.RemoteMethodsArgsSizes[method] + 8];
            //Span<byte> readBuffer = stackalloc byte[4];
            //(int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
            BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)method);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], 0);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], killAllConnections?1:0);
            lock (pipeTX)
                pipeTX.Write(writeBuffer);
            // promise.Task.Wait();
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
            if (localPlugin)
            {
                return TProgManPlaceRandomBus(memory.ReadMemory<int>(0x00862f28), aiType, group, 0, false, true, type, scheduled, 0, tour, line);
            }
            else
            {
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
            }
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
            if (localPlugin)
                return (uint)GetMem(length);
            else
            {
                if (!IsInitialised)
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
            }
        }

        /// <summary>
        /// Frees memory in Omsi using it's own memory allocator. This method might return before the memory has been freed.
        /// EXPERIMENTAL: A lot of messy stuff has to work for this to not crash.
        /// </summary>
        /// <param name="addr">The pointer to the object to deallocate</param>
        public static void OmsiFreeMemAsync(int addr)
        {
            if (localPlugin)
                FreeMem(addr);
            else
            {
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
            }
        }

        /// <summary>
        /// Attempts to get the current D3D context from Omsi, required before any of the graphics methods can be called.
        /// </summary>
        public static bool OmsiHookD3D()
        {
            if (localPlugin)
                return HookD3D() != 0;
            else
            {
                if (!IsInitialised)
                    throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

                int argPos = 0;
                Span<byte> writeBuffer = stackalloc byte[8];
                (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
                BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)OmsiHookRPCMethods.RemoteMethod.HookD3D);
                BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], resultPromise);
                lock (pipeTX)
                    pipeTX.Write(writeBuffer);
                var res = promise.Task.Result;
                return res != 0;
            }
        }

        /// <summary>
        /// Attempts to create a new d3d texture which can be shared with an external D3D context.
        /// </summary>
        public static async Task<(HRESULT hresult, uint pTexture)> OmsiCreateTextureAsync(uint width, uint height, D3DFORMAT format)
        {
            if (localPlugin)
            {
                uint ppTexture = OmsiGetMem(4).Result;
                HRESULT hresult = (HRESULT)CreateTexture(width, height, (uint)format, ppTexture);
                return (hresult, ppTexture);
            }
            else
            {
                if (!IsInitialised)
                    throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

                // Allocate the pointers
                int ppTexture = memory.AllocRemoteMemory(4, true).Result;//OmsiGetMem(4).Result;
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
                HRESULT result = (HRESULT)await promise.Task;
                uint pTexture = memory.ReadMemory<uint>(ppTexture);
                return (result, pTexture);
            }
        }

        /// <summary>
        /// Attempts to update the contents a d3d texture.
        /// </summary>
        /// <param name="texturePtr">the pointer to the IDirect3DTexture9 object</param>
        /// <param name="textureDataPtr">the pointer to the block of texture data to copy into the texture (must belong to the remote process)</param>
        /// <param name="width">the width of the texture data to copy</param>
        /// <param name="height">the height of the texture data to copy</param>
        /// <param name="updateRect">optionally, a rectangle specifying the area to copy into</param>
        /// <returns>an HRESULT indicating the result of the operation.</returns>
        /// <exception cref="NotInitialisedException"></exception>
        public static async Task<HRESULT> OmsiUpdateTextureAsync(uint texturePtr, uint textureDataPtr, uint width, uint height, Rectangle? updateRect = null)
        {
            if (localPlugin)
            {
                return (HRESULT)UpdateSubresource(texturePtr, textureDataPtr, width, height,
                    updateRect.HasValue ? 1 : 0, updateRect?.left ?? 0, updateRect?.top ?? 0, updateRect?.right ?? 0, updateRect?.bottom ?? 0);
            } 
            else 
            {
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
                return (HRESULT)await promise.Task;
            }
        }

        /// <summary>
        /// Releases an IDirect3DTexture9 object.
        /// </summary>
        /// <param name="texturePtr">the pointer to the IDirect3DTexture9</param>
        /// <returns>the status of the operation.</returns>
        /// <exception cref="NotInitialisedException"></exception>
        public static async Task<HRESULT> OmsiReleaseTextureAsync(uint texturePtr)
        {
            if (localPlugin)
                return (HRESULT)ReleaseTexture(texturePtr);
            else
            {
                if (!IsInitialised)
                    throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

                int argPos = 0;
                var method = OmsiHookRPCMethods.RemoteMethod.ReleaseTexture;
                // This should be thread safe as the asyncWriteBuff is thread local
                int writeBufferSize = OmsiHookRPCMethods.RemoteMethodsArgsSizes[method] + 8;
                byte[] writeBuffer = asyncWriteBuff.Value;
                (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos)..], (int)method);
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], resultPromise);
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], texturePtr);
                lock (pipeTX)
                    pipeTX.Write(writeBuffer.AsSpan()[..writeBufferSize]);
                return (HRESULT)await promise.Task;
            }
        }

        /// <summary>
        /// Gets the description of an IDirect3DTexture9 object.
        /// </summary>
        /// <param name="texturePtr"></param>
        /// <returns></returns>
        /// <exception cref="NotInitialisedException"></exception>
        public static async Task<(HRESULT hresult, uint width, uint height, D3DFORMAT format)> OmsiGetTextureDescAsync(uint texturePtr)
        {
            uint descPtr = unchecked((uint)await memory.AllocRemoteMemory(4 * 3, true));
            if (localPlugin)
            {
                HRESULT res = (HRESULT)GetTextureDesc(texturePtr, descPtr, descPtr + 4, descPtr + 8);

                uint width = memory.ReadMemory<uint>(descPtr);
                uint height = memory.ReadMemory<uint>(descPtr + 4);
                D3DFORMAT format = (D3DFORMAT)memory.ReadMemory<uint>(descPtr + 8);
                memory.FreeRemoteMemory(descPtr, true);

                return (res, width, height, format);
            }
            else
            {
                if (!IsInitialised)
                    throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

                int argPos = 0;
                var method = OmsiHookRPCMethods.RemoteMethod.GetTextureDesc;
                // This should be thread safe as the asyncWriteBuff is thread local
                int writeBufferSize = OmsiHookRPCMethods.RemoteMethodsArgsSizes[method] + 8;
                byte[] writeBuffer = asyncWriteBuff.Value;
                (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos)..], (int)method);
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], resultPromise);
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], texturePtr);
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], descPtr);
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], descPtr + 4);
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], descPtr + 8);
                lock (pipeTX)
                    pipeTX.Write(writeBuffer.AsSpan()[..writeBufferSize]);

                HRESULT res = (HRESULT)await promise.Task;
                uint width = memory.ReadMemory<uint>(descPtr);
                uint height = memory.ReadMemory<uint>(descPtr + 4);
                D3DFORMAT format = (D3DFORMAT)memory.ReadMemory<uint>(descPtr + 8);
                memory.FreeRemoteMemory(descPtr, true);

                return (res, width, height, format);
            }
        }

        /// <summary>
        /// Checks if a given pointer is an IDirect3DTexture9.
        /// 
        /// WARNING: If the pointer is non-null and the object is not a COM object, this method can cause crashes.
        /// </summary>
        /// <param name="texturePtr"></param>
        /// <returns></returns>
        /// <exception cref="NotInitialisedException"></exception>
        public static async Task<bool> OmsiIsTextureAsync(uint texturePtr)
        {
            if (localPlugin)
                return !HRESULTFailed((HRESULT)IsTexture(texturePtr));
            else
            {
                if (!IsInitialised)
                    throw new NotInitialisedException("OmsiHook RPC plugin is not connected! Did you make sure to call OmsiRemoteMethods.InitRemoteMethods() before this call?");

                int argPos = 0;
                var method = OmsiHookRPCMethods.RemoteMethod.IsTexture;
                // This should be thread safe as the asyncWriteBuff is thread local
                int writeBufferSize = OmsiHookRPCMethods.RemoteMethodsArgsSizes[method] + 8;
                byte[] writeBuffer = asyncWriteBuff.Value;
                (int resultPromise, TaskCompletionSource<int> promise) = CreateResultPromise();
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos)..], (int)method);
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], resultPromise);
                BitConverter.TryWriteBytes(writeBuffer.AsSpan()[(argPos += 4)..], texturePtr);
                lock (pipeTX)
                    pipeTX.Write(writeBuffer.AsSpan()[..writeBufferSize]);
                return !HRESULTFailed((HRESULT)await promise.Task);
            }
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
        private static extern int GetMem(int length);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern void FreeMem(int addr);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int HookD3D();
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int CreateTexture(uint Width, uint Height, uint Format, uint ppTexture);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int UpdateSubresource(uint Texture, uint TextureData, uint Width, uint Height, int UseRect, uint Left, uint Top, uint Right, uint Bottom);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int ReleaseTexture(uint Texture);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int GetTextureDesc(uint Texture, uint pWidth, uint pHeight, uint pFormat);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int IsTexture(uint Texture);

        public enum D3DFORMAT : uint
        {
            D3DFMT_UNKNOWN = 0,

            D3DFMT_R8G8B8 = 20,
            D3DFMT_A8R8G8B8 = 21,
            D3DFMT_X8R8G8B8 = 22,
            D3DFMT_R5G6B5 = 23,
            D3DFMT_X1R5G5B5 = 24,
            D3DFMT_A1R5G5B5 = 25,
            D3DFMT_A4R4G4B4 = 26,
            D3DFMT_R3G3B2 = 27,
            D3DFMT_A8 = 28,
            D3DFMT_A8R3G3B2 = 29,
            D3DFMT_X4R4G4B4 = 30,
            D3DFMT_A2B10G10R10 = 31,
            D3DFMT_A8B8G8R8 = 32,
            D3DFMT_X8B8G8R8 = 33,
            D3DFMT_G16R16 = 34,
            D3DFMT_A2R10G10B10 = 35,
            D3DFMT_A16B16G16R16 = 36,

            D3DFMT_A8P8 = 40,
            D3DFMT_P8 = 41,

            D3DFMT_L8 = 50,
            D3DFMT_A8L8 = 51,
            D3DFMT_A4L4 = 52,

            D3DFMT_V8U8 = 60,
            D3DFMT_L6V5U5 = 61,
            D3DFMT_X8L8V8U8 = 62,
            D3DFMT_Q8W8V8U8 = 63,
            D3DFMT_V16U16 = 64,
            D3DFMT_A2W10V10U10 = 67,

            D3DFMT_UYVY = ((byte)'U') | ((byte)'Y' << 8) | ((byte)'V' << 16) | ((byte)'Y' << 24),//MAKEFOURCC('U', 'Y', 'V', 'Y'),
            D3DFMT_R8G8_B8G8 = ((byte)'R') | ((byte)'G' << 8) | ((byte)'B' << 16) | ((byte)'G' << 24),
            D3DFMT_YUY2 = ((byte)'Y') | ((byte)'U' << 8) | ((byte)'Y' << 16) | ((byte)'2' << 24),
            D3DFMT_G8R8_G8B8 = ((byte)'G') | ((byte)'R' << 8) | ((byte)'G' << 16) | ((byte)'B' << 24),
            D3DFMT_DXT1 = ((byte)'D') | ((byte)'X' << 8) | ((byte)'T' << 16) | ((byte)'1' << 24),
            D3DFMT_DXT2 = ((byte)'D') | ((byte)'X' << 8) | ((byte)'T' << 16) | ((byte)'2' << 24),
            D3DFMT_DXT3 = ((byte)'D') | ((byte)'X' << 8) | ((byte)'T' << 16) | ((byte)'3' << 24),
            D3DFMT_DXT4 = ((byte)'D') | ((byte)'X' << 8) | ((byte)'T' << 16) | ((byte)'4' << 24),
            D3DFMT_DXT5 = ((byte)'D') | ((byte)'X' << 8) | ((byte)'T' << 16) | ((byte)'5' << 24),

            D3DFMT_D16_LOCKABLE = 70,
            D3DFMT_D32 = 71,
            D3DFMT_D15S1 = 73,
            D3DFMT_D24S8 = 75,
            D3DFMT_D24X8 = 77,
            D3DFMT_D24X4S4 = 79,
            D3DFMT_D16 = 80,

            D3DFMT_D32F_LOCKABLE = 82,
            D3DFMT_D24FS8 = 83,

            D3DFMT_L16 = 81,

            D3DFMT_VERTEXDATA = 100,
            D3DFMT_INDEX16 = 101,
            D3DFMT_INDEX32 = 102,

            D3DFMT_Q16W16V16U16 = 110,

            D3DFMT_MULTI2_ARGB8 = ((byte)'M') | ((byte)'E' << 8) | ((byte)'T' << 16) | ((byte)'1' << 24),

            // Floating point surface formats

            // s10e5 formats (16-bits per channel)
            D3DFMT_R16F = 111,
            D3DFMT_G16R16F = 112,
            D3DFMT_A16B16G16R16F = 113,

            // IEEE s23e8 formats (32-bits per channel)
            D3DFMT_R32F = 114,
            D3DFMT_G32R32F = 115,
            D3DFMT_A32B32G32R32F = 116,

            D3DFMT_CxV8U8 = 117,

            D3DFMT_FORCE_DWORD = 0x7fffffff
        }

        private const int D3D_FAC = 0x876;
        private const int OH_FAC = 0x554;

        public static bool HRESULTFailed(HRESULT hr) => (int)hr < 0;

        public enum HRESULT : int
        {
            S_OK = 0,
            S_FALSE = 1,
            E_FAIL =                                 (1 << 31) | 0x00004005,
            E_ABORT =                                (1 << 31) | 0x00004004,
            E_INVALIDARG =                           (1 << 31) | 0x00070057,

            D3DERR_WRONGTEXTUREFORMAT =              (1 << 31) | (D3D_FAC << 16) | (2072),
            D3DERR_UNSUPPORTEDCOLOROPERATION =       (1 << 31) | (D3D_FAC << 16) | (2073),
            D3DERR_UNSUPPORTEDCOLORARG =             (1 << 31) | (D3D_FAC << 16) | (2074),
            D3DERR_UNSUPPORTEDALPHAOPERATION =       (1 << 31) | (D3D_FAC << 16) | (2075),
            D3DERR_UNSUPPORTEDALPHAARG =             (1 << 31) | (D3D_FAC << 16) | (2076),
            D3DERR_TOOMANYOPERATIONS =               (1 << 31) | (D3D_FAC << 16) | (2077),
            D3DERR_CONFLICTINGTEXTUREFILTER =        (1 << 31) | (D3D_FAC << 16) | (2078),
            D3DERR_UNSUPPORTEDFACTORVALUE =          (1 << 31) | (D3D_FAC << 16) | (2079),
            D3DERR_CONFLICTINGRENDERSTATE =          (1 << 31) | (D3D_FAC << 16) | (2081),
            D3DERR_UNSUPPORTEDTEXTUREFILTER =        (1 << 31) | (D3D_FAC << 16) | (2082),
            D3DERR_CONFLICTINGTEXTUREPALETTE =       (1 << 31) | (D3D_FAC << 16) | (2086),
            D3DERR_DRIVERINTERNALERROR =             (1 << 31) | (D3D_FAC << 16) | (2087),                                                
            D3DERR_NOTFOUND =                        (1 << 31) | (D3D_FAC << 16) | (2150),
            D3DERR_MOREDATA =                        (1 << 31) | (D3D_FAC << 16) | (2151),
            D3DERR_DEVICELOST =                      (1 << 31) | (D3D_FAC << 16) | (2152),
            D3DERR_DEVICENOTRESET =                  (1 << 31) | (D3D_FAC << 16) | (2153),
            D3DERR_NOTAVAILABLE =                    (1 << 31) | (D3D_FAC << 16) | (2154),
            D3DERR_OUTOFVIDEOMEMORY =                (1 << 31) | (D3D_FAC << 16) | (380),
            D3DERR_INVALIDDEVICE =                   (1 << 31) | (D3D_FAC << 16) | (2155),
            D3DERR_INVALIDCALL =                     (1 << 31) | (D3D_FAC << 16) | (2156),
            D3DERR_DRIVERINVALIDCALL =               (1 << 31) | (D3D_FAC << 16) | (2157),
            D3DERR_WASSTILLDRAWING =                 (1 << 31) | (D3D_FAC << 16) | (540),
            D3DOK_NOAUTOGEN =                        (0 << 31) | (D3D_FAC << 16) | (2159),

            OHERR_NOD3DDEVICE =                      (1 << 31) | (OH_FAC << 16) | (10),
            OHERR_D3DDEVICEQUERYFAILED =             (1 << 31) | (OH_FAC << 16) | (11),
            OHERR_TEXTURENULL =                      (1 << 31) | (OH_FAC << 16) | (20),
            OHERR_UPDATESUBRES_DSTTEXTURETOOSMALL =  (1 << 31) | (OH_FAC << 16) | (21),
            OHERR_UPDATESUBRES_INVALIDRECT =         (1 << 31) | (OH_FAC << 16) | (22),
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
