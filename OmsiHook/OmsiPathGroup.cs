namespace OmsiHook
{
    /// <summary>
    /// A Group of Paths - used in <seealso cref="OmsiMapKachel">Map Tiles</seealso>
    /// </summary>
    public class OmsiPathGroup :OmsiObject
    {
        public OmsiPathGroup() : base() { }

        internal OmsiPathGroup(Memory memory, int address) : base(memory, address) { }

        public MemArray<OmsiPathID> Paths =>
            new(Memory, Address + 0x4);
        public MemArray<int> PathBlockingLinks =>
            new(Memory, Address + 0x8);
        public OmsiPathBelegung[] PathBelegung => Memory.MarshalStructs<OmsiPathBelegung, OmsiPathBelegungInternal>(
            Memory.ReadMemoryStructArray<OmsiPathBelegungInternal>(Address + 0xc));
        /// <summary>
        /// Space test
        /// </summary>
        public byte PlatzTest
        {
            get => Memory.ReadMemory<byte>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        /// <summary>
        /// Reservations
        /// </summary>
        public OmsiPathGroupReserv[] Reservvierungen => Memory.MarshalStructs<OmsiPathGroupReserv, OmsiPathGroupReservInternal>(
            Memory.ReadMemoryStructArray<OmsiPathGroupReservInternal>(Address + 0x14));
        public MemArray<OmsiPathGroupBlocking> PathBlockings =>
            new(Memory, Address + 0x18);
        /// <summary>
        /// Traffic light
        /// </summary>
        public int Ampel
        {
            get => Memory.ReadMemory<int>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
    }
}