namespace OmsiHook
{
    public class OmsiCamera : OmsiObject
    {
        internal OmsiCamera(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress)
        {
        }

        public string Name
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x4);
            set => omsiMemory.WriteMemory(baseAddress + 0x4, value);
        }
    }
}