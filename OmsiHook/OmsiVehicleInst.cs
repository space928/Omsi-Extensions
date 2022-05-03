namespace OmsiHook
{
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

        //TODO: A whole buch of AI fields and some other fields
    }
}