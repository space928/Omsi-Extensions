namespace OmsiHook
{
    public class OmsiSound : OmsiObject
    {
        internal OmsiSound(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiSound() : base() { }

        /*public DirectSound8 Device
        {
            get => Memory.ReadMemory<DirectSound8>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }*/
        public string FileName
        {
            get => Memory.ReadMemoryString(Memory.ReadMemory<int>(Address + 0x8));
            set => Memory.WriteMemory(Address + 0x8, value);
        }

    }
}