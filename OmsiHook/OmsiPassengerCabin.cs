namespace OmsiHook
{
    public class OmsiPassengerCabin : OmsiObject
    {
        internal OmsiPassengerCabin(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiPassengerCabin() : base() { }
        
        public OmsiSeat[] Seats
        {
            get => Memory.ReadMemoryStructArray<OmsiSeat>(Address + 0x4);
        }
        /* TODO:
        public OmsiPathPoint[] Entries
        {
            get => Memory.ReadMemoryStructArray<OmsiPathPoint>(Address + 0x8);
        }*/
        public OmsiEntryProp[] EntriesProp
        {
            get => Memory.ReadMemoryStructArray<OmsiEntryProp>(Address + 0xc);
        }
        /* TODO:
        public OmsiPathPoint[] Exits
        {
            get => Memory.ReadMemoryStructArray<OmsiPathPoint>(Address + 0x10);
        }*/
        public OmsiPassCabinStamper Stamper
        {
            get => Memory.ReadMemory<OmsiPassCabinStamper>(Address + 0x14);
        }
        public OmsiPassCabinStamper TicketSale // TODO: Make Strings work
        {
            get => Memory.ReadMemory<OmsiPassCabinStamper>(Address + 0x28);
        }
        /* TODO:
        public OmsiPathPoint[] LinkToOtherVehicle
        {
            get => Memory.ReadMemoryStructArray<OmsiPathPoint>(Address + 0x7c);
        }*/
    }
}