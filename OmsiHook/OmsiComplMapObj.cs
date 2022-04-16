namespace OmsiHook
{
    public class OmsiComplMapObj : OmsiPhysObj
    {
        internal OmsiComplMapObj(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiComplMapObj() : base() { }
        /* TODO:
        public string[] Scripts_Int
        {
            get => Memory.ReadMemoryStructArray<string>(Address + 0x17c);
        }*/
        /* TODO:
        public string[] Vars_Int
        {
            get => Memory.ReadMemoryStructArray<string>(Address + 0x180);
        }*/
        /* TODO:
        public string[] SVars_Int
        {
            get => Memory.ReadMemoryStructArray<string>(Address + 0x184);
        }*/
        /* TODO:
        public string[] ConstFiles_Int
        {
            get => Memory.ReadMemoryStructArray<string>(Address + 0x188);
        }*/
        public string Model_Int
        {
            get => Memory.ReadMemoryString(Address + 0x188);
            set => Memory.WriteMemory(Address + 0x188, value);
        }
        public OmsiPassengerCabin PassengerCabin
        {
            get => new OmsiPassengerCabin(Memory, Address + 0x190);
        }
    }
}