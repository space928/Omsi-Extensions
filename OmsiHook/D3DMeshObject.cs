namespace OmsiHook
{
    public class D3DMeshObject : D3DTransformObject
    {
        internal D3DMeshObject(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public D3DMeshObject() : base() { }
    }
}