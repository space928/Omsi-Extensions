namespace OmsiHook
{
    /// <summary>
    /// In material manager
    /// </summary>
    public class OmsiMaterialMan : OmsiObject
    {
        internal OmsiMaterialMan(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiMaterialMan() : base() { }

        /// <summary>
        /// Note that this field seems to be mostly unused by Omsi, hence it's always empty...
        /// </summary>
        public MemArray<OmsiMaterialItemInternal, OmsiMaterialItem> MaterialItems => new(Memory, Address + 0x4);
        public D3DMaterial9 StdMaterial
        {
            get => Memory.ReadMemory<D3DMaterial9>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public D3DMaterial9 ReflMaterial
        {
            get => Memory.ReadMemory<D3DMaterial9>(Address + 0x4c);
            set => Memory.WriteMemory(Address + 0x4c, value);
        }
    }
}