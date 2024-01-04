namespace OmsiHook
{
    /// <summary>
    /// Path Manager - used to keep track of paths mostly used by <seealso cref="OmsiHumanBeingInst">Humans</seealso>
    /// </summary>
    public class OmsiPathManager : OmsiObject
    {
        internal OmsiPathManager(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiPathManager() : base() { }

        public int StepSoundPacks
        {
            get => Memory.ReadMemory<int>(Address + 0x4);
        }
        public OmsiPathPointBasic MyClass
        {
            get => Memory.ReadMemoryObject<OmsiPathPointBasic>(Address, 0x8, false);
        }
        /* TODO: 
        public OmsiPathPoint[] PathPoints
        {
            get => new OmsiPathPoint(Memory, Memory.ReadMemory<int>(Address + 0xc));
        }*/
        public int LinkCounter
        {
            get => Memory.ReadMemory<int>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
    }
}