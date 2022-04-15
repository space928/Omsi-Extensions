namespace OmsiHook
{
    public class D3DTransformObject : D3DObject
    {
        internal D3DTransformObject(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal D3DTransformObject() : base() { }
    }
}