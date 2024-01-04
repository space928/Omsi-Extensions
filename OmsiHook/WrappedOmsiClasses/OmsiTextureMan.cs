namespace OmsiHook
{
    /// <summary>
    /// The global texture manager
    /// </summary>
    public class OmsiTextureMan : OmsiObject
    {
        internal OmsiTextureMan(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiTextureMan() : base() { }

        public byte LoadFlag
        {
            get => Memory.ReadMemory<byte>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public MemArray<OmsiTextureItemInternal, OmsiTextureItem> TextureItems => new(Memory, Address + 0x8, false);
    }
}