using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// List of stringvars relating to a chronochange
    /// </summary>
    public class OmsiChronoChangeLabels : OmsiChronoChange
    {
        public OmsiChronoChangeLabels() : base() { }

        internal OmsiChronoChangeLabels(Memory memory, int address) : base(memory, address) { }

        public string[] StringVars
        {
            get => Memory.ReadMemoryStringArray(Address + 0x8);
        }

    }
}
