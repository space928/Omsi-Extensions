namespace OmsiHook
{
    public class OmsiObject
    {
        internal Memory Memory { get; private set; }
        internal int Address { get; private set; }

        public OmsiObject() { }
        internal OmsiObject(Memory memory, int address) 
        {
            InitObject(memory, address);
        }

        /// <summary>
        /// Call this method to initialise an OmsiObject if the two-parameter constructor wasn't used.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="address"></param>
        internal void InitObject(Memory memory, int address)
        {
            this.Memory = memory;
            this.Address = address;
        }
    }
}