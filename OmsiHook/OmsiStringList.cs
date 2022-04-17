namespace OmsiHook
{
    public class OmsiStringList : OmsiObject
    {
        public OmsiStringList() : base() { }
        internal OmsiStringList(Memory memory, int address) : base(memory, address) { }
    }
}