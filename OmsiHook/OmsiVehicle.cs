namespace OmsiHook
{
    public class OmsiVehicle : OmsiComplMapObj
    {
        internal OmsiVehicle(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiVehicle() : base() { }
    }
}