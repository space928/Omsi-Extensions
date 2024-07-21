using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook.WrappedOmsiClasses
{
    public class OmsiCriticalSectionObject : OmsiObject
    {
        internal OmsiCriticalSectionObject(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiCriticalSectionObject() : base() { }

        public RTL_CRITICAL_SECTION cs
        {
            get => Memory.ReadMemory<RTL_CRITICAL_SECTION>(Address + 0x0);
        }
        public string name
        {
            get => Memory.ReadMemoryString(Address + 0x18);
        }
        public uint ident
        {
            get => Memory.ReadMemory<uint>(Address + 0x1c);
        }
    }
}
