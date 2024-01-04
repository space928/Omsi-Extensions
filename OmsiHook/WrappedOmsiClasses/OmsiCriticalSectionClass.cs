using System.Net;

namespace OmsiHook
{
    /// <summary>
    /// Base class for all lockable objects.
    /// </summary>
    public class OmsiCriticalSectionClass : OmsiObject
    {
        internal OmsiCriticalSectionClass(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiCriticalSectionClass() : base() { }

        public OmsiCriticalSection CS
        {
            get => Memory.MarshalStruct<OmsiCriticalSection, OmsiCriticalSectionInternal>(Memory.ReadMemory<OmsiCriticalSectionInternal>(Address + 0x4));
            set => Memory.WriteMemory(Address + 0x4, Memory.UnMarshalStruct<OmsiCriticalSectionInternal, OmsiCriticalSection>(value));
        }
    }
}