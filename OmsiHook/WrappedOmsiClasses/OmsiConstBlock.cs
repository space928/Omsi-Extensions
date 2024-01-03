using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// An object's ConstFile Structure
    /// </summary>
    public class OmsiConstBlock : OmsiObject
    {
        internal OmsiConstBlock(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiConstBlock() : base() { }
        public float[] Consts => Memory.ReadMemoryStructArray<float>(Address + 0x4);

        private MemArrayStringDict consts_str;
        public MemArrayStringDict Consts_str => consts_str ??= new (Memory, Address + 0x8, true);

        public OmsiFuncClass[] Funcs => Memory.ReadMemoryObjArray<OmsiFuncClass>(Address + 0xc);

        private MemArrayStringDict funcs_str;
        public MemArrayStringDict Funcs_str => funcs_str ??= new(Memory, Address + 0x10, true);
    }
}
