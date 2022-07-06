using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Defines a set of conditions based on various data types.
    /// </summary>
    public class OmsiBoolClass : OmsiObject
    { 
        internal OmsiBoolClass(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiBoolClass() : base() { }
        
        public OmsiBoolClassCondiBool[] CondisBool
        {
            get => Memory.ReadMemoryStructArray<OmsiBoolClassCondiBool>(Address + 0x4);
        }
        public OmsiBoolClassCondiInt[] CondisInt
        {
            get => Memory.ReadMemoryStructArray<OmsiBoolClassCondiInt>(Address + 0x8);
        }
        public OmsiBoolClassCondiFloat[] CondisFloat
        {
            get => Memory.ReadMemoryStructArray<OmsiBoolClassCondiFloat>(Address + 0xc);
        }
    }
}
