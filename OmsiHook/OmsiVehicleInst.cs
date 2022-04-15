namespace OmsiHook
{
    public class OmsiVehicleInst : OmsiMovingMapObjInst
    {
        internal OmsiVehicleInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiVehicleInst() : base() { }
    }
}