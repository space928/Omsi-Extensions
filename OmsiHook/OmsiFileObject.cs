namespace OmsiHook
{
    public class OmsiFileObject : OmsiObject
    {
        public OmsiFileObject() : base() { }

        internal OmsiFileObject(Memory memory, int address) : base(memory, address) { }
    }
}