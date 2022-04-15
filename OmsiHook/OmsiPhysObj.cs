namespace OmsiHook
{
    public class OmsiPhysObj : OmsiObject
    {
        internal OmsiPhysObj(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiPhysObj() : base() { }
    }
}