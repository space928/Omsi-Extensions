namespace OmsiHook
{
    public class OmsiChrono : OmsiObject
    {
        internal OmsiChrono(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }

        /*
        TODO: public Array? Timeline
        {
            get => omsiMemory.ReadMemory<Array?>(baseAddress + 0x4);
            set => omsiMemory.WriteMemory(baseAddress + 0x4, value);
        }*/

        public int Curr_TimelinePos
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x8);
            set => omsiMemory.WriteMemory(baseAddress + 0x8, value);
        }

        public bool Scenario_Changed
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0xc);
            set => omsiMemory.WriteMemory(baseAddress + 0xc, value);
        }

        /*
        TODO: public Array? Scenarios
        {
            get => omsiMemory.ReadMemory<Array?>(baseAddress + 0x10);
            set => omsiMemory.WriteMemory(baseAddress + 0x10, value);
        }*/

        public bool Prev_Timeline_StillValid
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x14);
            set => omsiMemory.WriteMemory(baseAddress + 0x14, value);
        }
    }
}