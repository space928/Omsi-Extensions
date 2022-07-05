namespace OmsiHook
{
    /// <summary>
    /// Simple Path Point
    /// </summary>
    public class OmsiPathPointBasic : OmsiObject
    {
        internal OmsiPathPointBasic(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiPathPointBasic() : base() { }
        
        /*
        public OmsiPathPointBasic[][] Links
        {
            get => 0x4
        }*/

        public MemArray<bool> Links_Avl
        {
            get => new(Memory, Address + 0x8);
        }
        public MemArray<short> StepSound
        {
            get => new(Memory, Address + 0xc);
        }
        public MemArray<float> Height
        {
            get => new(Memory, Address + 0x10);
        }
        public MemArray<int> LinkIndex
        {
            get => new(Memory, Address + 0x14);
        }
    }
}