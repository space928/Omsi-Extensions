namespace OmsiHook
{
    /// <summary>
    /// List of strings
    /// </summary>
    public class OmsiStringList : OmsiObject
    {
        public OmsiStringList() : base() { }
        internal OmsiStringList(Memory memory, int address) : base(memory, address) { }
        public OmsiStringItem[] FList
        {
            get => Memory.MarshalStructs<OmsiStringItem, OmsiStringItemInternal>(Memory.ReadMemoryStructArray<OmsiStringItemInternal>(Address + 0x2c));
        }
        public int FCount
        {
            get => Memory.ReadMemory<int>(Address + 0x30);
            set => Memory.WriteMemory(Address + 0x30, value);
        }
        public int FCapacity
        {
            get => Memory.ReadMemory<int>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }
        public bool FSorted
        {
            get => Memory.ReadMemory<bool>(Address + 0x38);
            set => Memory.WriteMemory(Address + 0x38, value);
        }
        public OmsiDuplicates FDuplicates
        {
            get => Memory.ReadMemory<OmsiDuplicates>(Address + 0x39);
            set => Memory.WriteMemory(Address + 0x39, value);
        }
        public bool FCaseSensitive
        {
            get => Memory.ReadMemory<bool>(Address + 0x3a);
            set => Memory.WriteMemory(Address + 0x3a, value);
        }
        /*
         * NotifyEvent params (FOnChange and FOnChanging - 0x40, 0x48) doubt are marshalable.
         */
        public bool FOwnsObject
        {
            get => Memory.ReadMemory<bool>(Address + 0x50);
            set => Memory.WriteMemory(Address + 0x50, value);
        }
    }
}