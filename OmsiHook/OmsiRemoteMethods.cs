using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using OmsiHookRPCPlugin;

namespace OmsiHook
{
    /// <summary>
    /// All of these methods will only work if called from a native Omsi plugin or if the OmsiHookRPCPlugin is installed.
    /// These methods also rely on OmsiHookInvoker.dll which must be in the Omsi plugins folder.
    /// </summary>
	public static class OmsiRemoteMethods
	{
        private static NamedPipeClientStream pipe;
        private static Memory memory;

        public static bool IsInitialised => pipe?.IsConnected ?? false;

        // TODO: Okay maybe this shouldn't be static... Singleton?
        internal static void InitRemoteMethods(Memory omsiMemory, bool inifiniteTimeout = false)
        {
            memory = omsiMemory;

#if !OMSI_PLUGIN
            pipe = new(".", OmsiHookRPCMethods.PIPE_NAME, PipeDirection.InOut);
            //pipe.ReadMode = PipeTransmissionMode.Message;
            try
            {
                if (inifiniteTimeout)
                    pipe.Connect();
                else
                    pipe.Connect(20000);
            }
            catch(TimeoutException)
            {
                pipe = null;
                throw new TimeoutException("Couldn't manage to connect to OmsiHookRPCPlugin within 20 seconds! Check that it is loaded correctly.");
            }
#endif
        }

        [Obsolete]
        public static int MakeVehicle()
        {
            int vehList = TTempRVListCreate(0x0074802C, 1);
            string path = @"Vehicles\GPM_MAN_LionsCity_M\MAN_A47.bus";
            int mem = memory.AllocateString(path, false);

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
            int argPos = 0;
            Span<byte> writeBuffer = stackalloc byte[39];
            Span<byte> readBuffer = stackalloc byte[4];
            BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)OmsiHookRPCMethods.RemoteMethod.TProgManPlaceRandomBus);
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
            pipe.Write(writeBuffer);
            pipe.Read(readBuffer);
            return BitConverter.ToInt32(readBuffer);
#endif
        }

        /// <summary>
        /// Allocates memory in Omsi using it's own memory allocator.
        /// EXPERIMENTAL: A lot of messy stuff has to work for this to not crash.
        /// </summary>
        /// <param name="length">How many bytes to allocate</param>
        /// <returns>A pointer to the newly allocated memory (note that you made need to
        /// <c>VirtualProtect</c> it to access it).</returns>
        public static int OmsiGetMem(int length)
        {
#if OMSI_PLUGIN
            return GetMem(length);
#else
            int argPos = 0;
            Span<byte> writeBuffer = stackalloc byte[8];
            Span<byte> readBuffer = stackalloc byte[4];
            BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)OmsiHookRPCMethods.RemoteMethod.GetMem);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], length);
            pipe.Write(writeBuffer);
            pipe.Read(readBuffer);
            return BitConverter.ToInt32(readBuffer);
#endif
        }

        /// <summary>
        /// Frees memory in Omsi using it's own memory allocator.
        /// EXPERIMENTAL: A lot of messy stuff has to work for this to not crash.
        /// </summary>
        /// <param name="addr">The pointer to the object to deallocate</param>
        public static void OmsiFreeMem(int addr)
        {
#if OMSI_PLUGIN
            FreeMem(addr);
#else
            int argPos = 0;
            Span<byte> writeBuffer = stackalloc byte[8];
            Span<byte> readBuffer = stackalloc byte[4];
            BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)OmsiHookRPCMethods.RemoteMethod.FreeMem);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], addr);
            pipe.Write(writeBuffer);
            pipe.Read(readBuffer);
#endif
        }

        /// <summary>
        /// Attempts to get the current D3D context from Omsi, required before any of the graphics 
        /// </summary>
        public static bool OmsiHookD3D()
        {
#if OMSI_PLUGIN
            return HookD3D() != 0;
#else
            int argPos = 0;
            Span<byte> writeBuffer = stackalloc byte[4];
            Span<byte> readBuffer = stackalloc byte[4];
            BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)OmsiHookRPCMethods.RemoteMethod.HookD3D);
            pipe.Write(writeBuffer);
            return pipe.Read(readBuffer) != 0;
#endif
        }

        /// <summary>
        /// Attempts to get the current D3D context from Omsi, required before any of the graphics 
        /// </summary>
        public static bool OmsiCreateTexture(uint width, uint height, DXGI_FORMAT format, uint ppTexture, uint pSharedHandle)
        {
#if OMSI_PLUGIN
            return CreateTexture(width, height, (uint)format, ppTexture, pSharedHandle) != 0;
#else
            int argPos = 0;
            Span<byte> writeBuffer = stackalloc byte[20];
            Span<byte> readBuffer = stackalloc byte[4];
            BitConverter.TryWriteBytes(writeBuffer[(argPos)..], (int)OmsiHookRPCMethods.RemoteMethod.HookD3D);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], width);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], height);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], (uint)format); 
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], ppTexture);
            BitConverter.TryWriteBytes(writeBuffer[(argPos += 4)..], pSharedHandle);
            pipe.Write(writeBuffer);
            return pipe.Read(readBuffer) != 0;
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
        internal static extern int CreateTexture(uint Width, uint Height, uint Format, uint ppTexture, uint pSharedHandle);

        public enum DXGI_FORMAT :uint
        {
            R16G16B16A16_FLOAT = 10,
            R10G10B10A2_UNORM = 24,
            R8G8B8A8_UNORM = 28
        }
    }
}
