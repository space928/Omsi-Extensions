using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// All recoginised Globals in OMSI
    /// </summary>
    public class OmsiGlobals : OmsiObject
    {
        internal OmsiGlobals(Memory omsiMemory, int baseAddress, OmsiHook hook) : base(omsiMemory, baseAddress) { this.hook = hook; }
        public OmsiGlobals(OmsiHook hook) : base() { this.hook = hook; }

        private OmsiRemoteMethods remoteMethods;
        private OmsiHook hook;

        /// <summary>
        /// Gets the vehicle instance being driven by the player.
        /// </summary>
        public OmsiRoadVehicleInst PlayerVehicle => hook.GetRoadVehicleInst(PlayerVehicleIndex);
        public int PlayerVehicleIndex => Memory.ReadMemory<int>(0x00861740);

        /// <summary>
        /// Current Weather
        /// </summary>
        public OmsiWeather Weather => new(Memory, Memory.ReadMemory<int>(0x008617D0));

        /// <summary>
        /// Current Map
        /// </summary>
        public OmsiMap Map => new(Memory, Memory.ReadMemory<int>(0x861588));

        /// <summary>
        /// In game TicketPack List
        /// </summary>
        public OmsiTicketPack TicketPack => Memory.MarshalStruct<OmsiTicketPack, OmsiTicketPackInternal>(
            Memory.ReadMemory<OmsiTicketPackInternal>(0x008611fc));

        /// <summary>
        /// Access to RemoteMethods
        /// </summary>
        public OmsiRemoteMethods RemoteMethods => remoteMethods ??= new(Memory, 0);

        /// <summary>
        /// Current in game Date / Time
        /// </summary>
        public OmsiTime Time => new(Memory, 0);

        /// <summary>
        /// In game Driver List
        /// </summary>
        public OmsiDriver[] Drivers => Memory.MarshalStructs<OmsiDriver, OmsiDriverInternal>(Memory.ReadMemoryStructArray<OmsiDriverInternal>(0x008614F8));
        public int SelectedDriver => Memory.ReadMemory<int>(0x008614FC);

        /// <summary>
        /// Current Service logs
        /// </summary>
        public OmsiTTLogDetailed[] OmsiTTLogs => Memory.MarshalStructs<OmsiTTLogDetailed, OmsiTTLogDetailedInternal>(
            Memory.ReadMemoryStructArray<OmsiTTLogDetailedInternal>(0x00861750));

        /// <summary>
        /// Current real weather config
        /// </summary>
        public OmsiActuWeather ActuWeather => new(Memory, Memory.ReadMemory<int>(0x00861278));
        public OmsiHumanBeingInst[] Humans => Memory.ReadMemoryObjArray<OmsiHumanBeingInst>(0x0086172c);

        /// <summary>
        /// Timetable manager for current session
        /// </summary>
        public OmsiTimeTableMan TimeTableManager => new(Memory, Memory.ReadMemory<int>(0x008614e8));
    }
}
