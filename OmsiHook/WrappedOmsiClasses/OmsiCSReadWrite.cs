namespace OmsiHook
{
    /// <summary>
    /// Base class for all lockable objects supporting read/write.
    /// </summary>
    public class OmsiCSReadWrite : OmsiCriticalSectionClass
    {
        internal OmsiCSReadWrite(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiCSReadWrite() : base() { }

        public OmsiCriticalSection[] LockCounter
        {
            get => Memory.MarshalStructs<OmsiCriticalSection, OmsiCriticalSectionInternal>(Memory.ReadMemoryStructArray<OmsiCriticalSectionInternal>(Address + 0x24));
        }
    }
}