using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// All recognised Globals in OMSI.
    /// </summary>
    public class OmsiGlobals : OmsiObject
    {
        internal OmsiGlobals(Memory omsiMemory, int baseAddress, OmsiHook hook) : base(omsiMemory, baseAddress) { this.hook = hook; }
        public OmsiGlobals(OmsiHook hook) : base() { this.hook = hook; }

        private OmsiHook hook;

        /// <summary>
        /// Gets the vehicle instance being driven by the player.
        /// </summary>
        public OmsiRoadVehicleInst PlayerVehicle => hook.GetRoadVehicleInst(PlayerVehicleIndex);
        /// <summary>
        /// Gets the current vehicle index driven by the player.
        /// </summary>
        public int PlayerVehicleIndex => Memory.ReadMemory<int>(0x00861740);

        /// <summary>
        /// Current Weather
        /// </summary>
        public OmsiWeather Weather => Memory.ReadMemoryObject<OmsiWeather>(0x008617D0);

        /// <summary>
        /// Current Map
        /// </summary>
        public OmsiMap Map => Memory.ReadMemoryObject<OmsiMap>(0x861588);

        /// <summary>
        /// In game TicketPack List
        /// </summary>
        public OmsiTicketPack TicketPack => Memory.MarshalStruct<OmsiTicketPack, OmsiTicketPackInternal>(
            Memory.ReadMemory<OmsiTicketPackInternal>(0x008611fc));

        /// <summary>
        /// Current in game Date / Time
        /// </summary>
        public OmsiTime Time => new(Memory, 0);

        /// <summary>
        /// In game Driver List
        /// </summary>
        public OmsiDriver[] Drivers => Memory.MarshalStructs<OmsiDriver, OmsiDriverInternal>(Memory.ReadMemoryStructArray<OmsiDriverInternal>(0x008614F8));
        /// <summary>
        /// Currently active driver profile index
        /// </summary>
        public int SelectedDriver => Memory.ReadMemory<int>(0x008614FC);

        /// <summary>
        /// Current Service logs
        /// </summary>
        public MemArray<OmsiTTLogDetailedInternal, OmsiTTLogDetailed> OmsiTTLogs => new(Memory, 0x00861750, false);

        /// <summary>
        /// Current real weather config
        /// </summary>
        public OmsiActuWeather ActuWeather => Memory.ReadMemoryObject<OmsiActuWeather>(0x00861278);

        /// <summary>
        /// List of Humans present in the scene
        /// </summary>
        public OmsiHumanBeingInst[] Humans => Memory.ReadMemoryObjArray<OmsiHumanBeingInst>(0x0086172c);

        /// <summary>
        /// Timetable manager for current session
        /// </summary>
        public OmsiTimeTableMan TimeTableManager => Memory.ReadMemoryObject<OmsiTimeTableMan>(0x008614e8);

        /// <summary>
        /// Current Program Manager
        /// </summary>
        public OmsiProgMan ProgamManager => Memory.ReadMemoryObject<OmsiProgMan>(0x00862f28);

        /// <summary>
        /// Main Camera Object
        /// </summary>
        public OmsiCamera Camera => Memory.ReadMemoryObject<OmsiCamera>(0x008616e0);

        /// <summary>
        /// Central Material Manager
        /// </summary>
        public OmsiMaterialMan MaterialMan => Memory.ReadMemoryObject<OmsiMaterialMan>(0x00861ca8);
    }
}
