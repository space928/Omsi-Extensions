namespace OmsiHook
{
    public class OmsiObject
    {
        internal readonly Memory omsiMemory;
        internal readonly int baseAddress;

        internal OmsiObject(Memory omsiMemory, int baseAddress)
        {
            this.omsiMemory = omsiMemory;
            this.baseAddress = baseAddress;
        }
    }
}