namespace OmsiHook
{
    /// <summary>
    /// Defines a forest on a tile
    /// </summary>
    public class OmsiKachelForest : OmsiObject
    {
        public OmsiKachelForest() : base() { }

        internal OmsiKachelForest(Memory memory, int address) : base(memory, address) { }

        public int FractTrees_TestTexture
        {
            get => Memory.ReadMemory<int>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public float Dist
        {
            get => Memory.ReadMemory<float>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public float RelDist
        {
            get => Memory.ReadMemory<float>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public float Visual_Radius
        {
            get => Memory.ReadMemory<float>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public bool Generated
        {
            get => Memory.ReadMemory<bool>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public bool Visible
        {
            get => Memory.ReadMemory<bool>(Address + 0x15);
            set => Memory.WriteMemory(Address + 0x15, value);
        }
        public OmsiTree[] Trees =>
            Memory.ReadMemoryStructArray<OmsiTree>(Address + 0x18);
        public D3DMeshObject MeshObject => Memory.ReadMemoryObject<D3DMeshObject>(Address, 0x1c, false);
    }
}