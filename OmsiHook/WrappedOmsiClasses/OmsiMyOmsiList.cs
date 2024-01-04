using System.Net;

namespace OmsiHook
{
    /// <summary>
    /// A somewhat specialised version of a generic list; used to store road vehicles.
    /// </summary>
    public class OmsiMyOmsiList<T> : OmsiCSReadWrite where T : OmsiObject, new()
    {
        internal OmsiMyOmsiList(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiMyOmsiList() : base() { }

        public MemArrayList<T> FList => new(Memory, Address + 0x28, false);
        public int FCount => Memory.ReadMemory<int>(Address + 0x2c);
        public string Name => Memory.ReadMemoryString(Address + 0x30, StrPtrType.DelphiString);
        public bool FAI => Memory.ReadMemory<bool>(Address + 0x34);
    }
}