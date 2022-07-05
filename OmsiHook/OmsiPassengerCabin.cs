namespace OmsiHook
{
    /// <summary>
    /// Defines a passenger cabin (as per the passenger cabin file)
    /// </summary>
    public class OmsiPassengerCabin : OmsiObject
    {
        internal OmsiPassengerCabin(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiPassengerCabin() : base() { }
        
        public MemArray<OmsiSeat> Seats
        {
            get => new(Memory, Address + 0x4);
        }
        public OmsiPathPointBasic[] Entries => Memory.ReadMemoryObjArray<OmsiPathPointBasic>(Address + 0x8);
        public MemArray<OmsiEntryProp> EntriesProp
        {
            get => new(Memory, Address + 0xc);
        }
        public OmsiPathPointBasic[] Exits => Memory.ReadMemoryObjArray<OmsiPathPointBasic>(Address + 0x10);
        public OmsiPassCabinStamper Stamper
        {
            get => Memory.MarshalStruct<OmsiPassCabinStamper, OmsiPassCabinStamperInternal>(
                Memory.ReadMemory<OmsiPassCabinStamperInternal>(Address + 0x14));
        }
        public OmsiPassCabinTicketSale TicketSale
        {
            get => Memory.MarshalStruct<OmsiPassCabinTicketSale, OmsiPassCabinTicketSaleInternal>(
                Memory.ReadMemory<OmsiPassCabinTicketSaleInternal>(Address + 0x28));
        }
        public OmsiPathPointBasic[] LinkToOtherVehicle => Memory.ReadMemoryObjArray<OmsiPathPointBasic>(Address + 0x7c);
    }
}