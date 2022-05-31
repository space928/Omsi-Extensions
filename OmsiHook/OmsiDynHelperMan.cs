using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    public class OmsiDynHelperMan : OmsiObject
    {
        public OmsiDynHelperMan() : base() { }

        internal OmsiDynHelperMan(Memory memory, int address) : base(memory, address) { }
        
        public int HelperPoses_Cnt
        {
            get => Memory.ReadMemory<int>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public OmsiComplMapSceneObjInst[] HelperPoses
        {
            get => Memory.ReadMemoryObjArray<OmsiComplMapSceneObjInst>(Address + 0x8);
        }
        public OmsiCriticalSection CriticalSection
        {
            get => Memory.ReadMemory<OmsiCriticalSection>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public OmsiPathSegment Prev_PathSegment
        {
            get => new(Memory, Memory.ReadMemory<int>(0x2c));
        }
    }
}
