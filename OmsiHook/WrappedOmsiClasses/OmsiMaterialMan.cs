namespace OmsiHook
{
    /// <summary>
    /// In game camera object
    /// </summary>
    public class OmsiMaterialMan : OmsiObject
    {
        internal OmsiMaterialMan(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiMaterialMan() : base() { }

        public OmsiMaterialItem[] MaterialItems => Memory.MarshalStructs<OmsiMaterialItem, OmsiMaterialItemInternal>(Memory.ReadMemoryStructPtrArray<OmsiMaterialItemInternal>(Address + 0x4));

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