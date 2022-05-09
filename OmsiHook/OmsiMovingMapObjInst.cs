namespace OmsiHook
{
    /// <summary>
    /// Base Class for all instances of moving map objects - vehicles / humans
    /// </summary>
    public class OmsiMovingMapObjInst : OmsiComplMapObjInst
    {
        internal OmsiMovingMapObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiMovingMapObjInst() : base() { }
        // TODO: this one
    }
}