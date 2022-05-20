using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Texture Manager Object
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
        public OmsiTextureItem[] TextureItems
        {
            get => Memory.MarshalStructs<OmsiTextureItem, OmsiTextureItemInternal>(Memory.ReadMemoryStructArray<OmsiTextureItemInternal>(Address + 0x8));
        }
    }
}
