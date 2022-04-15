namespace OmsiHook
{
    public class D3DMeshObject : D3DTransformObject
    {
        internal D3DMeshObject(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal D3DMeshObject() : base() { }
    }
}