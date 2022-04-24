using System;
using System.Runtime.InteropServices;

namespace OmsiHook
{
	public class OmsiRemoteMethods : OmsiObject
	{
        public OmsiRemoteMethods() : base() { }

        internal OmsiRemoteMethods(Memory memory, int address) : base(memory, address) { }

        public int MakeVehicle()
        {
            int vehList = TTempRVListCreate(0x0074802C, 1);
            string path = @"Vehicles\GPM_MAN_LionsCity_M\MAN_A47.bus";
            int mem = Memory.AllocateString(path, false);

            return TProgManMakeVehicle(Memory.ReadMemory<int>(0x00862f28), vehList,
                Memory.ReadMemory<int>(0x008615A8), false, false,
              0, false, false, false, false,
              -1, true, 0, (byte)3, false,
              0, 0, 0, 0, 0, false,
              false, true, true, mem);
        }

        /// <summary>
        /// Spawns a random bus in the map at one of the entry points.
        /// </summary>
        /// <returns>Honestly who knows... TODO: Find out</returns>
        public int PlaceRandomBus()
        {
            return TProgManPlaceRandomBus(Memory.ReadMemory<int>(0x00862f28), 0, 1, 0, false, true, -1, false, 0, 0, 0);
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
    }
}
