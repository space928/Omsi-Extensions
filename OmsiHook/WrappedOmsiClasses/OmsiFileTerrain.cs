namespace OmsiHook
{
    /// <summary>
    /// Defines a tile from a file - including heightmap and sizing
    /// </summary>
    public class OmsiFileTerrain : OmsiObject
    {
        public OmsiFileTerrain() : base() { }

        internal OmsiFileTerrain(Memory memory, int address) : base(memory, address) { }

        /*TODO: This looks like a 61x61 array of floats representing height + an additional float/int
         * It's not correctly defined in IDR so dissassembly is required to work out how to use it.
         * https://github.com/space928/Omsi-Extensions/issues/132
        public float[] Elevs
        {
            get => Memory.ReadMemoryStruct<float[]>(Address + 0x4);
        }*/
        /*public ? Inactive // Size = 3600 Bytes ~ 60x60 bytes?
        {
            get => Memory.ReadMemory<?>(Address + 0x3a2c);
            set => Memory.WriteMemory(Address + 0x3a2c, value);
        }*/
        public short Count_Inactive
        {
            get => Memory.ReadMemory<short>(Address + 0x483c);
            set => Memory.WriteMemory(Address + 0x483c, value);
        }
        public float Width_S
        {
            get => Memory.ReadMemory<float>(Address + 0x4840);
            set => Memory.WriteMemory(Address + 0x4840, value);
        }
        public float Width_N
        {
            get => Memory.ReadMemory<float>(Address + 0x4844);
            set => Memory.WriteMemory(Address + 0x4844, value);
        }
        public int MyKachel
        {
            get => Memory.ReadMemory<int>(Address + 0x4848);
            set => Memory.WriteMemory(Address + 0x4848, value);
        }
    }
}