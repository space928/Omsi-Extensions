namespace OmsiHook
{
    /// <summary>
    /// Vehicle base class - used for road and rail
    /// </summary>
    public class OmsiVehicle : OmsiComplMapObj
    {
        internal OmsiVehicle(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiVehicle() : base() { }
        public int StdCamera
        {
            get => Memory.ReadMemory<int>(Address + 0x264);
            set => Memory.WriteMemory(Address + 0x264, value);
        }
        public OmsiCameraSettings[] CameraSettingsDriver
        {
            get => Memory.ReadMemoryStructArray<OmsiCameraSettings>(Address + 0x268);
            //set => Memory.WriteMemory(Address + 0x264, value);
        }
        public OmsiCameraSettings[] CameraSettingsPax
        {
            get => Memory.ReadMemoryStructArray<OmsiCameraSettings>(Address + 0x26c);
            //set => Memory.WriteMemory(Address + 0x264, value);
        }
        public D3DVector OutsideCameraCenter
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x270);
            set => Memory.WriteMemory(Address + 0x270, value);
        }
        public int View_Schedule
        {
            get => Memory.ReadMemory<int>(Address + 0x27c);
            set => Memory.WriteMemory(Address + 0x27c, value);
        }
        public int View_TicketSelling
        {
            get => Memory.ReadMemory<int>(Address + 0x280);
            set => Memory.WriteMemory(Address + 0x280, value);
        }
        public byte Station_Side
        {
            get => Memory.ReadMemory<byte>(Address + 0x284);
            set => Memory.WriteMemory(Address + 0x284, value);
        }
        /// <summary>
        /// Livery index?
        /// </summary>
        public int Num_Werbung
        {
            get => Memory.ReadMemory<int>(Address + 0x288);
            set => Memory.WriteMemory(Address + 0x288, value);
        }
        /// <summary>
        /// ! Warn ! Listed as int in decompilation, but treated as byte due to following struct item
        /// </summary>
        public byte AI_Veh_Type
        {
            get => Memory.ReadMemory<byte>(Address + 0x28c);
            set => Memory.WriteMemory(Address + 0x28c, value);
        }
        public byte REG_Type
        {
            get => Memory.ReadMemory<byte>(Address + 0x28d);
            set => Memory.WriteMemory(Address + 0x28d, value);
        }
        public float Wider_Roll
        {
            get => Memory.ReadMemory<float>(Address + 0x290);
            set => Memory.WriteMemory(Address + 0x290, value);
        }
        /// <summary>
        /// Center of Gravity Height
        /// </summary>
        public float Schwerpunkt_Height
        {
            get => Memory.ReadMemory<float>(Address + 0x294);
            set => Memory.WriteMemory(Address + 0x294, value);
        }
        public bool REG_NumberActive
        {
            get => Memory.ReadMemory<bool>(Address + 0x298);
            set => Memory.WriteMemory(Address + 0x298, value);
        }
        public string REG_List // ANSI String
        {
            get => Memory.ReadMemoryString(Address + 0x29c);
            set => Memory.WriteMemory(Address + 0x29c, value);
        }
        public string REG_NumberFile // ANSI String
        {
            get => Memory.ReadMemoryString(Address + 0x2a0);
            set => Memory.WriteMemory(Address + 0x20a, value);
        }
        public string REG_Prefix // ANSI String
        {
            get => Memory.ReadMemoryString(Address + 0x2a4);
            set => Memory.WriteMemory(Address + 0x2a4, value);
        }
        public string REG_Postfix // ANSI String
        {
            get => Memory.ReadMemoryString(Address + 0x2a8);
            set => Memory.WriteMemory(Address + 0x2a8, value);
        }
        public int Ident_Var
        {
            get => Memory.ReadMemory<int>(Address + 0x2ac);
            set => Memory.WriteMemory(Address + 0x2ac, value);
        }
        public int KMCounter_Init_StartYear
        {
            get => Memory.ReadMemory<int>(Address + 0x2b0);
            set => Memory.WriteMemory(Address + 0x2b0, value);
        }
        public int KMCounter_Init_KMPerYear
        {
            get => Memory.ReadMemory<int>(Address + 0x2b4);
            set => Memory.WriteMemory(Address + 0x2b4, value);
        }
    }
}