using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Defines a FreeTex instance
    /// </summary>
    public class OmsiFreeTexInst : OmsiObject
    {
        public OmsiFreeTexInst() : base() { }

        internal OmsiFreeTexInst(Memory memory, int address) : base(memory, address) { }

        public int Tex
        {
            get => Memory.ReadMemory<int>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public string LastString
        {
            get => Memory.ReadMemoryString(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
    }
}
