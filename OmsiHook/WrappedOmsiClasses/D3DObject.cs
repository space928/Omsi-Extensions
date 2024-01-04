namespace OmsiHook
{
    /// <summary>
    /// Base class for Direct 3D objects
    /// </summary>
    public class D3DObject : OmsiObject
    {
        internal D3DObject(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public D3DObject() : base() { }

        public bool Loaded
        {
            get => Memory.ReadMemory<bool>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
    }
}