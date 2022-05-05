using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    public class OmsiChronoChange : OmsiObject
    {
        public OmsiChronoChange() : base() { }

        internal OmsiChronoChange(Memory memory, int address) : base(memory, address) { }

        public int Scenario
        {
            get => Memory.ReadMemory<int>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
    }
}
