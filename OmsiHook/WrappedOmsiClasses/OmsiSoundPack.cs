namespace OmsiHook
{
    /// <summary>
    /// Pack of sounds used by both humans and vehicles
    /// </summary>
    public class OmsiSoundPack : OmsiObject
    {
        internal OmsiSoundPack(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiSoundPack() : base() { }

        /*public DirectSound8 Device
        {
            get => Memory.ReadMemory<DirectSound8>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }*/
        public string FileName
        {
            get => Memory.ReadMemoryString(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public bool AI
        {
            get => Memory.ReadMemory<bool>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public string Path
        {
            get => Memory.ReadMemoryString(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public int SoundCount
        {
            get => Memory.ReadMemory<int>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public OmsiSound[] Sounds
        {
            get => Memory.ReadMemoryObjArray<OmsiSound>(Address + 0x18);
        }
        public float RefRange
        {
            get => Memory.ReadMemory<float>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        /* TODO:
        public floatptr[] Vars
        {
            get => Memory.ReadMemoryStructArray<floatptr>(Address + 0x20);
        }*/
        public D3DMatrix KoordSystem
        {
            get => Memory.ReadMemory<D3DMatrix>(Memory.ReadMemory<int>(Address + 0x24));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x24), value);
        }
        public bool Loaded
        {
            get => Memory.ReadMemory<bool>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public float Dist
        {
            get => Memory.ReadMemory<float>(Address + 0x2c);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }
        public string SoundIdent
        {
            get => Memory.ReadMemoryString(Memory.ReadMemory<int>(Address + 0x30), true);
            set => Memory.WriteMemory(Address + 0x30, value);
        }
        public bool TooFar
        {
            get => Memory.ReadMemory<bool>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }
        public bool Stopped_TooFar
        {
            get => Memory.ReadMemory<bool>(Address + 0x35);
            set => Memory.WriteMemory(Address + 0x35, value);
        }

    }
}