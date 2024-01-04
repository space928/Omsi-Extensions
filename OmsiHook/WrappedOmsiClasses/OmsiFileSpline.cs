namespace OmsiHook
{
    /// <summary>
    /// Splines read from a tile file
    /// </summary>
    public class OmsiFileSpline : OmsiObject
    {
        public OmsiFileSpline() : base() { }

        internal OmsiFileSpline(Memory memory, int address) : base(memory, address) { }

        public uint IDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public string Obj
        {
            get => Memory.ReadMemoryString(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public D3DVector StartPnt
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public float StartHdg
        {
            get => Memory.ReadMemory<float>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        public float SplLength
        {
            get => Memory.ReadMemory<float>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public float Radius
        {
            get => Memory.ReadMemory<float>(Address + 0x20);
            set => Memory.WriteMemory(Address + 0x20, value);
        }
        public float StartZ
        {
            get => Memory.ReadMemory<float>(Address + 0x24);
            set => Memory.WriteMemory(Address + 0x24, value);
        }
        public uint NextIDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public uint PrevIDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x2c);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }
        public float EndRadius
        {
            get => Memory.ReadMemory<float>(Address + 0x30);
            set => Memory.WriteMemory(Address + 0x30, value);
        }
        /// <summary>
        /// Track transition curve
        /// </summary>
        public bool Klothoide
        {
            get => Memory.ReadMemory<bool>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }
        public byte AlignTerrain
        {
            get => Memory.ReadMemory<byte>(Address + 0x35);
            set => Memory.WriteMemory(Address + 0x35, value);
        }
        /// <summary>
        /// Slope Start
        /// </summary>
        public float Steig_Start
        {
            get => Memory.ReadMemory<float>(Address + 0x38);
            set => Memory.WriteMemory(Address + 0x38, value);
        }
        /// <summary>
        /// Slope End
        /// </summary>
        public float Steig_End
        {
            get => Memory.ReadMemory<float>(Address + 0x3c);
            set => Memory.WriteMemory(Address + 0x3c, value);
        }
        public float Cant_Start
        {
            get => Memory.ReadMemory<float>(Address + 0x40);
            set => Memory.WriteMemory(Address + 0x40, value);
        }
        public float Cant_End
        {
            get => Memory.ReadMemory<float>(Address + 0x44);
            set => Memory.WriteMemory(Address + 0x44, value);
        }
        public float Skew_Start
        {
            get => Memory.ReadMemory<float>(Address + 0x48);
            set => Memory.WriteMemory(Address + 0x48, value);
        }
        public float Skew_End
        {
            get => Memory.ReadMemory<float>(Address + 0x4c);
            set => Memory.WriteMemory(Address + 0x4c, value);
        }
        public bool UseDeltaHeight
        {
            get => Memory.ReadMemory<bool>(Address + 0x50);
            set => Memory.WriteMemory(Address + 0x50, value);
        }
        public float DeltaHeight
        {
            get => Memory.ReadMemory<float>(Address + 0x54);
            set => Memory.WriteMemory(Address + 0x54, value);
        }
        public bool Mirror
        {
            get => Memory.ReadMemory<bool>(Address + 0x58);
            set => Memory.WriteMemory(Address + 0x58, value);
        }
        public OmsiSplineSegment MySegment => Memory.ReadMemoryObject<OmsiSplineSegment>(Address, 0x5c, false);
        public MemArray<OmsiFileSplinePathInfoInternal, OmsiFileSplinePathInfo> Paths => new(Memory, Memory.ReadMemory<int>(Address + 0x60), true);
        public int Chrono_Origin
        {
            get => Memory.ReadMemory<int>(Address + 0x64);
            set => Memory.WriteMemory(Address + 0x64, value);
        }
        public OmsiChronoChange[] Chrono_Changes => Memory.ReadMemoryObjArray<OmsiChronoChange>(Address + 0x68);
        public OmsiObjChronoVars Chrono_Active => Memory.ReadMemoryObject<OmsiObjChronoVars>(Address, 0x6c, false);
    }
}