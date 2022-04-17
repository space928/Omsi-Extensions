namespace OmsiHook
{
    public class OmsiRoadVehicle : OmsiVehicle
    {
        internal OmsiRoadVehicle(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiRoadVehicle() : base() { }

        /* TODO:
        public OmsiScriptVarIndizes ScriptVarIndizes
        {
            get => Memory.ReadMemory<OmsiScriptVarIndizes>(Address + 0x2b8);
            set => Memory.WriteMemory(Address + 0x2b8, value);
        }*/
        public bool Show_Dialogue
        {
            get => Memory.ReadMemory<bool>(Address + 0x500);
            set => Memory.WriteMemory(Address + 0x500, value);
        }
    }
}