namespace OmsiHook
{
    /// <summary>
    /// Instance of <seealso cref="OmsiVehicle"/>
    /// </summary>
    public class OmsiVehicleInst : OmsiMovingMapObjInst
    {
        internal OmsiVehicleInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiVehicleInst() : base() { }

        //TODO:
        /*public OmsiCriticalSection CS_AI_BusStopData
        {
            get => Memory.ReadMemory<int>(Address + 0x4f0);
            set => Memory.WriteMemory(Address + 0x4f0, value);
        }*/

        /// <summary>
        /// Last seat height
        /// </summary>
        public float LastSitzHgt
        {
            get => Memory.ReadMemory<float>(Address + 0x510);
            set => Memory.WriteMemory(Address + 0x510, value);
        }

        /// <summary>
        /// Last seat velocity
        /// </summary>
        public float LastSitzVeloc
        {
            get => Memory.ReadMemory<float>(Address + 0x514);
            set => Memory.WriteMemory(Address + 0x514, value);
        }

        /// <summary>
        /// Seat matrix
        /// </summary>
        public D3DMatrix SitzMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x518);
            set => Memory.WriteMemory(Address + 0x518, value);
        }

        public D3DMatrix DriverMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x558);
            set => Memory.WriteMemory(Address + 0x558, value);
        }

        public D3DVector HeadPos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x598);
            set => Memory.WriteMemory(Address + 0x598, value);
        }

        public D3DVector HeadVeloc
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x5a4);
            set => Memory.WriteMemory(Address + 0x5a4, value);
        }

        public float CS_AI_BusStopData
        {
            get => Memory.ReadMemory<float>(Address + 0x510);
            set => Memory.WriteMemory(Address + 0x510, value);
        }

        public OmsiCamera[] Cameras_Driver => Memory.ReadMemoryObjArray<OmsiCamera>(Address + 0x5b0);

        public OmsiCamera[] Cameras_Pax => Memory.ReadMemoryObjArray<OmsiCamera>(Address + 0x5b4);

        public OmsiCamera Camera_Driver => new(Memory, Memory.ReadMemory<int>(Address + 0x5b8));

        public OmsiCamera Camera_Pax => new(Memory, Memory.ReadMemory<int>(Address + 0x5bc));

        public int Act_Camera_Driver
        {
            get => Memory.ReadMemory<int>(Address + 0x5c0);
            set => Memory.WriteMemory(Address + 0x5c0, value);
        }

        public int Act_Camera_Pax
        {
            get => Memory.ReadMemory<int>(Address + 0x5c4);
            set => Memory.WriteMemory(Address + 0x5c4, value);
        }

        public float Rad_M_Ab
        {
            get => Memory.ReadMemory<float>(Address + 0x5c8);
            set => Memory.WriteMemory(Address + 0x5c8, value);
        }

        public float Rad_N_Ab
        {
            get => Memory.ReadMemory<float>(Address + 0x5cc);
            set => Memory.WriteMemory(Address + 0x5cc, value);
        }

        /// <summary>
        /// Braking power
        /// </summary>
        public float Bremskraft
        {
            get => Memory.ReadMemory<float>(Address + 0x5d0);
            set => Memory.WriteMemory(Address + 0x5d0, value);
        }

        public bool Amplify
        {
            get => Memory.ReadMemory<bool>(Address + 0x5d4);
            set => Memory.WriteMemory(Address + 0x5d4, value);
        }

        public bool Throttle_Pressed
        {
            get => Memory.ReadMemory<bool>(Address + 0x5d5);
            set => Memory.WriteMemory(Address + 0x5d5, value);
        }

        public bool WheelBackRunning
        {
            get => Memory.ReadMemory<bool>(Address + 0x5d6);
            set => Memory.WriteMemory(Address + 0x5d6, value);
        }

        public float MaxWert_Gas_O_Amplify
        {
            get => Memory.ReadMemory<float>(Address + 0x5d8);
            set => Memory.WriteMemory(Address + 0x5d8, value);
        }

        public float Throttle
        {
            get => Memory.ReadMemory<float>(Address + 0x5dc);
            set => Memory.WriteMemory(Address + 0x5dc, value);
        }

        /// <summary>
        /// Brake pedal
        /// </summary>
        public float Bremspedal
        {
            get => Memory.ReadMemory<float>(Address + 0x5e0);
            set => Memory.WriteMemory(Address + 0x5e0, value);
        }

        /// <summary>
        /// Clutch
        /// </summary>
        public float Kupplung
        {
            get => Memory.ReadMemory<float>(Address + 0x5e4);
            set => Memory.WriteMemory(Address + 0x5e4, value);
        }

        public float Microphone
        {
            get => Memory.ReadMemory<float>(Address + 0x5e8);
            set => Memory.WriteMemory(Address + 0x5e8, value);
        }

        public float Radio
        {
            get => Memory.ReadMemory<float>(Address + 0x5ec);
            set => Memory.WriteMemory(Address + 0x5ec, value);
        }

        public float PrecipRate
        {
            get => Memory.ReadMemory<float>(Address + 0x5f0);
            set => Memory.WriteMemory(Address + 0x5f0, value);
        }

        public float DirtRate
        {
            get => Memory.ReadMemory<float>(Address + 0x5f4);
            set => Memory.WriteMemory(Address + 0x5f4, value);
        }

        public float Dirt
        {
            get => Memory.ReadMemory<float>(Address + 0x5f8);
            set => Memory.WriteMemory(Address + 0x5f8, value);
        }

        public float CabinAir_Temp
        {
            get => Memory.ReadMemory<float>(Address + 0x5fc);
            set => Memory.WriteMemory(Address + 0x5fc, value);
        }

        public float CabinAir_RelHum
        {
            get => Memory.ReadMemory<float>(Address + 0x600);
            set => Memory.WriteMemory(Address + 0x600, value);
        }

        public float CabinAir_AbsHum
        {
            get => Memory.ReadMemory<float>(Address + 0x604);
            set => Memory.WriteMemory(Address + 0x604, value);
        }

        public D3DVector A_Trans
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x608);
            set => Memory.WriteMemory(Address + 0x608, value);
        }

        public D3DVector A_Rot
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x614);
            set => Memory.WriteMemory(Address + 0x614, value);
        }

        public OmsiCamera OutsideCamera => new(Memory, Memory.ReadMemory<int>(Address + 0x620));

        public bool PAI
        {
            get => Memory.ReadMemory<bool>(Address + 0x624);
            set => Memory.WriteMemory(Address + 0x624, value);
        }

        public OmsiPAIM PAI_Mode
        {
            get => (OmsiPAIM)Memory.ReadMemory<byte>(Address + 0x625);
            set => Memory.WriteMemory(Address + 0x625, (byte)value);
        }
        public uint PAI_Timer
        {
            get => Memory.ReadMemory<uint>(Address + 0x628);
            set => Memory.WriteMemory(Address + 0x628, value);
        }
        public float AI_var
        {
            get => Memory.ReadMemory<float>(Address + 0x62c);
            set => Memory.WriteMemory(Address + 0x62c, value);
        }
        public float AI_Engine
        {
            get => Memory.ReadMemory<float>(Address + 0x630);
            set => Memory.WriteMemory(Address + 0x630, value);
        }
        /// <summary>
        /// Light
        /// </summary>
        public float AI_Licht
        {
            get => Memory.ReadMemory<float>(Address + 0x634);
            set => Memory.WriteMemory(Address + 0x634, value);
        }
        public float AI_InteriorLight
        {
            get => Memory.ReadMemory<float>(Address + 0x638);
            set => Memory.WriteMemory(Address + 0x638, value);
        }
        public float AI_Blinker_L
        {
            get => Memory.ReadMemory<float>(Address + 0x63c);
            set => Memory.WriteMemory(Address + 0x63c, value);
        }
        public float AI_Blinker_R
        {
            get => Memory.ReadMemory<float>(Address + 0x640);
            set => Memory.WriteMemory(Address + 0x640, value);
        }
        public float AI_BrakeLight
        {
            get => Memory.ReadMemory<float>(Address + 0x644);
            set => Memory.WriteMemory(Address + 0x644, value);
        }
        public bool Prev_AI_Licht
        {
            get => Memory.ReadMemory<bool>(Address + 0x648);
            set => Memory.WriteMemory(Address + 0x648, value);
        }
        public float AI_Licht_Change_Timer
        {
            get => Memory.ReadMemory<float>(Address + 0x64c);
            set => Memory.WriteMemory(Address + 0x64c, value);
        }
        public byte AI_Licht_Change_Counter
        {
            get => Memory.ReadMemory<byte>(Address + 0x650);
            set => Memory.WriteMemory(Address + 0x650, value);
        }
        public bool AI_Lichthupe
        {
            get => Memory.ReadMemory<bool>(Address + 0x651);
            set => Memory.WriteMemory(Address + 0x651, value);
        }
        public float AI_Lichthupe_Timer
        {
            get => Memory.ReadMemory<float>(Address + 0x654);
            set => Memory.WriteMemory(Address + 0x654, value);
        }
        public float AI_Lichthupe_LastSpeed
        {
            get => Memory.ReadMemory<float>(Address + 0x658);
            set => Memory.WriteMemory(Address + 0x658, value);
        }
        public bool AI_Scheduled_Info_Valid
        {
            get => Memory.ReadMemory<bool>(Address + 0x65c);
            set => Memory.WriteMemory(Address + 0x65c, value);
        }
        public bool AI_Scheduled_InvalidDataWarning
        {
            get => Memory.ReadMemory<bool>(Address + 0x65d);
            set => Memory.WriteMemory(Address + 0x65d, value);
        }
        public int AI_Scheduled_Line
        {
            get => Memory.ReadMemory<int>(Address + 0x660);
            set => Memory.WriteMemory(Address + 0x660, value);
        }
        public int AI_Scheduled_Tour
        {
            get => Memory.ReadMemory<int>(Address + 0x664);
            set => Memory.WriteMemory(Address + 0x664, value);
        }
        public int AI_Scheduled_TourEntry
        {
            get => Memory.ReadMemory<int>(Address + 0x668);
            set => Memory.WriteMemory(Address + 0x668, value);
        }
        public int AI_Scheduled_Trip
        {
            get => Memory.ReadMemory<int>(Address + 0x66c);
            set => Memory.WriteMemory(Address + 0x66c, value);
        }
        public bool AI_Scheduled_Amplifyer
        {
            get => Memory.ReadMemory<bool>(Address + 0x670);
            set => Memory.WriteMemory(Address + 0x670, value);
        }
        public float AI_Scheduled_Target_Index
        {
            get => Memory.ReadMemory<float>(Address + 0x674);
            set => Memory.WriteMemory(Address + 0x674, value);
        }
        public int AI_Scheduled_StartDay
        {
            get => Memory.ReadMemory<int>(Address + 0x678);
            set => Memory.WriteMemory(Address + 0x678, value);
        }
        public int AI_Scheduled_Profile
        {
            get => Memory.ReadMemory<int>(Address + 0x67c);
            set => Memory.WriteMemory(Address + 0x67c, value);
        }
        public int AI_Scheduled_NextBusstop
        {
            get => Memory.ReadMemory<int>(Address + 0x680);
            set => Memory.WriteMemory(Address + 0x680, value);
        }
        public int AI_Scheduled_PrevBusstop
        {
            get => Memory.ReadMemory<int>(Address + 0x684);
            set => Memory.WriteMemory(Address + 0x684, value);
        }
        public float AI_Scheduled_NextBusstopDist
        {
            get => Memory.ReadMemory<float>(Address + 0x688);
            set => Memory.WriteMemory(Address + 0x688, value);
        }
        public float AI_Scheduled_NextBusstopDist_Min
        {
            get => Memory.ReadMemory<float>(Address + 0x68c);
            set => Memory.WriteMemory(Address + 0x68c, value);
        }
        public float AI_Scheduled_NextBusstopRelHDG
        {
            get => Memory.ReadMemory<float>(Address + 0x690);
            set => Memory.WriteMemory(Address + 0x690, value);
        }
        public float AI_Scheduled_PrevBusstopDist
        {
            get => Memory.ReadMemory<float>(Address + 0x694);
            set => Memory.WriteMemory(Address + 0x694, value);
        }
        public float AI_Scheduled_NextBusstopTime
        {
            get => Memory.ReadMemory<float>(Address + 0x698);
            set => Memory.WriteMemory(Address + 0x698, value);
        }
        public float AI_Scheduled_NextBusstopArrTime
        {
            get => Memory.ReadMemory<float>(Address + 0x69c);
            set => Memory.WriteMemory(Address + 0x69c, value);
        }
        public float AI_Scheduled_PrevBusstopDepTime
        {
            get => Memory.ReadMemory<float>(Address + 0x6a0);
            set => Memory.WriteMemory(Address + 0x6a0, value);
        }
        public float AI_Scheduled_NextBusstop_TimeToDepart
        {
            get => Memory.ReadMemory<float>(Address + 0x6a4);
            set => Memory.WriteMemory(Address + 0x6a4, value);
        }
        public int AI_Scheduled_NextBusstopIndex
        {
            get => Memory.ReadMemory<int>(Address + 0x6a8);
            set => Memory.WriteMemory(Address + 0x6a8, value);
        }
        public string AI_Scheduled_NextBusstopName
        {
            get => Memory.ReadMemoryString(Address + 0x6ac, true);
            set => Memory.WriteMemory(Address + 0x6ac, value, true);
        }
        public uint AI_Scheduled_NextBusstopIDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x6b0);
            set => Memory.WriteMemory(Address + 0x6b0, value);
        }
        public int AI_Scheduled_Aussetzen_before_tourentry
        {
            get => Memory.ReadMemory<int>(Address + 0x6b4);
            set => Memory.WriteMemory(Address + 0x6b4, value);
        }
        public bool AI_Scheduled_Aussetzen_Betriebsfahrt
        {
            get => Memory.ReadMemory<bool>(Address + 0x6b8);
            set => Memory.WriteMemory(Address + 0x6b8, value);
        }
        public int AI_Scheduled_Delay
        {
            get => Memory.ReadMemory<int>(Address + 0x6bc);
            set => Memory.WriteMemory(Address + 0x6bc, value);
        }
        public bool AI_Scheduled_NewTerminusSet
        {
            get => Memory.ReadMemory<bool>(Address + 0x6c0);
            set => Memory.WriteMemory(Address + 0x6c0, value);
        }
        public byte AI_Scheduled_NextBusstop_state
        {
            get => Memory.ReadMemory<byte>(Address + 0x6c1);
            set => Memory.WriteMemory(Address + 0x6c1, value);
        }
        public float AI_Scheduled_AtStation
        {
            get => Memory.ReadMemory<float>(Address + 0x6c4);
            set => Memory.WriteMemory(Address + 0x6c4, value);
        }
        public float AI_Scheduled_SetBusstopDisplayTimer
        {
            get => Memory.ReadMemory<float>(Address + 0x6c8);
            set => Memory.WriteMemory(Address + 0x6c8, value);
        }
        public float Schedule_Active_var
        {
            get => Memory.ReadMemory<float>(Address + 0x6cc);
            set => Memory.WriteMemory(Address + 0x6cc, value);
        }
        public MemArray<bool> SeatOccupancy
        {
            get => new(Memory, Address + 0x6d0);
        }
        public bool Pass_Cabin_useAlsoNextVeh
        {
            get => Memory.ReadMemory<bool>(Address + 0x6d4);
            set => Memory.WriteMemory(Address + 0x6d4, value);
        }
        public bool Pass_Cabin_useAlsoPrevVeh
        {
            get => Memory.ReadMemory<bool>(Address + 0x6d5);
            set => Memory.WriteMemory(Address + 0x6d5, value);
        }
        public float Humans_Count
        {
            get => Memory.ReadMemory<float>(Address + 0x6d8);
            set => Memory.WriteMemory(Address + 0x6d8, value);
        }
        public MemArray<float> EntriesOpen
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x6dc));
        }
        public MemArray<float> ExitsOpen
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x6e0));
        }
        public MemArray<float> EntriesReq
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x6e4));
        }
        public MemArray<float> ExitsReq
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x6e8));
        }
    }
}