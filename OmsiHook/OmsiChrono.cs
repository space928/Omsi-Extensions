namespace OmsiHook
{
    /// <summary>
    /// Data store for a map's Chrono Events
    /// </summary>
    public class OmsiChrono : OmsiObject
    {
        internal OmsiChrono(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiChrono() : base() { }

        /*
        TODO: public Array? Timeline
        {
            get => omsiMemory.ReadMemory<Array?>(baseAddress + 0x4);
            set => omsiMemory.WriteMemory(baseAddress + 0x4, value);
        }*/

        public int Curr_TimelinePos
        {
            get => Memory.ReadMemory<int>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }

        public bool Scenario_Changed
        {
            get => Memory.ReadMemory<bool>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }

        /*
        TODO: public Array? Scenarios
        {
            get => omsiMemory.ReadMemory<Array?>(baseAddress + 0x10);
            set => omsiMemory.WriteMemory(baseAddress + 0x10, value);
        }*/

        public bool Prev_Timeline_StillValid
        {
            get => Memory.ReadMemory<bool>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
    }
}