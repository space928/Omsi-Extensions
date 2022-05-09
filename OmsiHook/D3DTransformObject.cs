namespace OmsiHook
{
    /// <summary>
    /// Base class for transformable Direct 3D objects
    /// </summary>
    public class D3DTransformObject : D3DObject
    {
        internal D3DTransformObject(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public D3DTransformObject() : base() { }

        public D3DVector VFDmax
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public D3DVector VFDmin
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public D3DMatrix Matrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x20);
            set => Memory.WriteMemory(Address + 0x20, value);
        }
    }
}