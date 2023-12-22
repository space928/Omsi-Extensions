namespace OmsiHook
{
    /// <summary>
    /// Base class for all OmsiHook objects - handles some of the memory managment for objects.
    /// </summary>
    /// <remarks>
    /// Wraps a native pointer in the remote application. Orphaned <seealso cref="OmsiObject"/>s with no 
    /// <seealso cref="Address"/> bound will not behave correctly and should never be assigned to remote 
    /// variables! As such, an <seealso cref="OmsiObject"/> cannot be created in C# without first allocating
    /// an object in the remote application to bind to.
    /// </remarks>
    public class OmsiObject
    {
        internal Memory Memory { get; private set; }
        internal int Address { get; set; }

        public bool IsNull => Address == 0;

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
        internal virtual void InitObject(Memory memory, int address)
        {
            this.Memory = memory;
            this.Address = address;
        }

        public override bool Equals(object obj)
        {
            if (obj is not OmsiObject item)
            {
                return (Address == 0);
            }

            return (item.Address == Address);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}