using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// String refering to the ChronoChange type
    /// </summary>
    public class OmsiChronoChangeTyp : OmsiChronoChange
    {
        public OmsiChronoChangeTyp() : base() { }

        internal OmsiChronoChangeTyp(Memory memory, int address) : base(memory, address) { }

        public string NewTyp
        {
            get => Memory.ReadMemoryString(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }

    }
}
