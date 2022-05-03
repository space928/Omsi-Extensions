namespace OmsiHook
{
    public class OmsiRoadVehicle : OmsiVehicle
    {
        internal OmsiRoadVehicle(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiRoadVehicle() : base() { }
    }
}