namespace OmsiHook
{
    public class OmsiPassengerCabin : OmsiObject
    {
        internal OmsiPassengerCabin(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiPassengerCabin() : base() { }
        
        public OmsiSeat[] Seats
        {
            get => Memory.ReadMemoryStructArray<OmsiSeat>(Address + 0x4);
        }
        public OmsiPathPointBasic[] Entries => Memory.ReadMemoryObjArray<OmsiPathPointBasic>(Address + 0x8);
        public OmsiEntryProp[] EntriesProp
        {
            get => Memory.ReadMemoryStructArray<OmsiEntryProp>(Address + 0xc);
        }
        public OmsiPathPointBasic[] Exits => Memory.ReadMemoryObjArray<OmsiPathPointBasic>(Address + 0x10);
        public OmsiPassCabinStamper Stamper
        {
            get => Memory.ReadMemory<OmsiPassCabinStamper>(Address + 0x14);
        }
        public OmsiPassCabinTicketSale TicketSale
        {
            get => Memory.MarshalStruct<OmsiPassCabinTicketSale, OmsiPassCabinTicketSaleInternal>(
                Memory.ReadMemory<OmsiPassCabinTicketSaleInternal>(Address + 0x28));
        }
        public OmsiPathPointBasic[] LinkToOtherVehicle => Memory.ReadMemoryObjArray<OmsiPathPointBasic>(Address + 0x7c);
    }
}