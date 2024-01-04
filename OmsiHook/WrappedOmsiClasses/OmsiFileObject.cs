namespace OmsiHook
{
    /// <summary>
    /// Base class for objects created from files
    /// </summary>
    public class OmsiFileObject : OmsiObject
    {
        public OmsiFileObject() : base() { }

        internal OmsiFileObject(Memory memory, int address) : base(memory, address) { }
        
        public OmsiComplMapObj MyComplMapObj
        {
            get => Memory.ReadMemoryObject<OmsiComplMapObj>(Address, 0x4, false);
        }
        public uint IDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public bool Refreshed
        {
            get => Memory.ReadMemory<bool>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public bool AttachedToObj
        {
            get => Memory.ReadMemory<bool>(Address + 0xd);
            set => Memory.WriteMemory(Address + 0xd, value);
        }
        public bool AttachedToSpl
        {
            get => Memory.ReadMemory<bool>(Address + 0xe);
            set => Memory.WriteMemory(Address + 0xe, value);
        }
        public int IsRepeaterOfTile
        {
            get => Memory.ReadMemory<int>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public int RepeaterOffset
        {
            get => Memory.ReadMemory<int>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public string Obj
        {
            get => Memory.ReadMemoryString(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        public D3DVector Pos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public float Hdg
        {
            get => Memory.ReadMemory<float>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public float Pitch
        {
            get => Memory.ReadMemory<float>(Address + 0x2c);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }
        public float Bank
        {
            get => Memory.ReadMemory<float>(Address + 0x30);
            set => Memory.WriteMemory(Address + 0x30, value);
        }
        public OmsiFileObject AttObj
        {
            get => Memory.ReadMemoryObject<OmsiFileObject>(Address, 0x34, false);
        }
        public OmsiFileSpline AttSpln
        {
            get => Memory.ReadMemoryObject<OmsiFileSpline>(Address, 0x38, false);
        }
        public int AttPnt
        {
            get => Memory.ReadMemory<int>(Address + 0x3c);
            set => Memory.WriteMemory(Address + 0x3c, value);
        }
        /// <summary>
        /// Spacing ?
        /// </summary>
        public float Abstand
        {
            get => Memory.ReadMemory<float>(Address + 0x40);
            set => Memory.WriteMemory(Address + 0x40, value);
        }
        public float Distanz
        {
            get => Memory.ReadMemory<float>(Address + 0x44);
            set => Memory.WriteMemory(Address + 0x44, value);
        }
        public bool Tangential
        {
            get => Memory.ReadMemory<bool>(Address + 0x48);
            set => Memory.WriteMemory(Address + 0x48, value);
        }
        public byte Priority
        {
            get => Memory.ReadMemory<byte>(Address + 0x49);
            set => Memory.WriteMemory(Address + 0x49, value);
        }
        public string[] StringVars
        {
            get => Memory.ReadMemoryStringArray(Address + 0x4c);
            //set => Memory.WriteMemory(Address + 0x49, value);
        }
        public OmsiComplMapObjInst[] MyInstance
        {
            get => Memory.ReadMemoryObjArray<OmsiComplMapObjInst>(Address + 0x50);
        }
        public bool AlignTerrain
        {
            get => Memory.ReadMemory<bool>(Address + 0x54);
            set => Memory.WriteMemory(Address + 0x54, value);
        }
        public uint MyVarParent_IDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x58);
            set => Memory.WriteMemory(Address + 0x58, value);
        }
        public int MyVarParent_Katchel
        {
            get => Memory.ReadMemory<int>(Address + 0x5c);
            set => Memory.WriteMemory(Address + 0x5c, value);
        }
        public int MyVarParent_Obj
        {
            get => Memory.ReadMemory<int>(Address + 0x60);
            set => Memory.WriteMemory(Address + 0x60, value);
        }
        public OmsiFileObjectPathInfo[] Paths
        {
            get => Memory.MarshalStructs<OmsiFileObjectPathInfo, OmsiFileObjectPathInfoInternal>(
                Memory.ReadMemoryStructArray<OmsiFileObjectPathInfoInternal>(Address + 0x64));
        }
        public int Chrono_Origin
        {
            get => Memory.ReadMemory<int>(Address + 0x68);
            set => Memory.WriteMemory(Address + 0x68, value);
        }
        public OmsiChronoChange[] Chrono_Changes
        {
            get => Memory.ReadMemoryObjArray<OmsiChronoChange>(Address + 0x6c);
        }
        public OmsiObjChronoVars Chrono_Active
        {
            get => Memory.ReadMemoryObject<OmsiObjChronoVars>(Address, 0x70, false);
        }
        public bool Chrono_Own
        {
            get => Memory.ReadMemory<bool>(Address + 0x74);
            set => Memory.WriteMemory(Address + 0x74, value);
        }
        public int TempStation
        {
            get => Memory.ReadMemory<int>(Address + 0x78);
            set => Memory.WriteMemory(Address + 0x78, value);
        }

    }
}