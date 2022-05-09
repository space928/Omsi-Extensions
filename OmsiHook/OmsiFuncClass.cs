using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Defines a curve - used in const files and similar configuration files
    /// </summary>
    public class OmsiFuncClass : OmsiObject
    {
        public OmsiFuncClass() : base() { }

        internal OmsiFuncClass(Memory memory, int address) : base(memory, address) { }

        public OmsiPointPair[] Pnts
        {
            get => Memory.ReadMemoryStructArray<OmsiPointPair>(Address + 0x4);
        }

        public bool PreNullIsNull
        {
            get => Memory.ReadMemory<bool>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public bool AftNullIsNull
        {
            get => Memory.ReadMemory<bool>(Address + 0x9);
            set => Memory.WriteMemory(Address + 0x9, value);
        }

    }
}
