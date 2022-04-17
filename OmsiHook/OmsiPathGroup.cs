namespace OmsiHook
{
    public class OmsiPathGroup :OmsiObject
    {
        public OmsiPathGroup() : base() { }

        internal OmsiPathGroup(Memory memory, int address) : base(memory, address) { }

        public OmsiPathID[] Paths =>
            Memory.ReadMemoryStructArray<OmsiPathID>(Address + 0x4);
        public int[] PathBlockingLinks =>
            Memory.ReadMemoryStructArray<int>(Address + 0x8);
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
        public OmsiPathGroupBlocking[] PathBlockings =>
            Memory.ReadMemoryStructArray<OmsiPathGroupBlocking>(Address + 0x18);
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