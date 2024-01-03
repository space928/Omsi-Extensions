using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Settings relating to the real world weather in OMSI
    /// </summary>
    public class OmsiConstBlock : OmsiObject
    {
        internal OmsiConstBlock(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiConstBlock() : base() { }
        public float[] Consts
        {
            get => Memory.ReadMemoryStructArray<float>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }

        private MemArrayStringDict consts_str;
        public MemArrayStringDict Consts_str => consts_str ??= new (Memory, Address + 0x8, true);

        public OmsiFuncClass[] Funcs
        {
            get => Memory.ReadMemoryObjArray<OmsiFuncClass>(Address + 0xc);
        }

        private MemArrayStringDict funcs_str;
        public MemArrayStringDict Funcs_str => funcs_str ??= new(Memory, Address + 0x10, true);
    }
}
