namespace OmsiHook
{
    public class OmsiObject
    {
        internal Memory Memory { get; private set; }
        internal int Address { get; private set; }

        internal OmsiObject() { }
        internal OmsiObject(Memory memory, int address) 
        {
            InitObject(memory, address);
        }

        internal void InitObject(Memory memory, int address)
        {
            this.Memory = memory;
            this.Address = address;
        }
    }
}