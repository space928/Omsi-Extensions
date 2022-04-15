namespace OmsiHook
{
    public class D3DMeshFileObject : D3DMeshObject
    {
        internal D3DMeshFileObject(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal D3DMeshFileObject() : base() { }
    }
}