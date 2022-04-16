namespace OmsiHook
{
    public class OmsiPathPoint : OmsiObject
    {
        internal OmsiPathPoint(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiPathPoint() : base() { }
    }
}