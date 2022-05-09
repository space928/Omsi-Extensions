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

        public bool[] Links_Avl
        {
            get => Memory.ReadMemoryStructArray<bool>(Address + 0x8);
        }
        public short[] StepSound
        {
            get => Memory.ReadMemoryStructArray<short>(Address + 0xc);
        }
        public float[] Height
        {
            get => Memory.ReadMemoryStructArray<float>(Address + 0x10);
        }
        public int[] LinkIndex
        {
            get => Memory.ReadMemoryStructArray<int>(Address + 0x14);
        }
    }
}