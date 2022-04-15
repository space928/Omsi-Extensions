namespace OmsiHook
{
    public class OmsiRoadVehicle : OmsiMovingMapObjInst
    {
        internal OmsiRoadVehicle(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiRoadVehicle() : base() { }
    }
}