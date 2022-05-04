using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    public class OmsiObjChronoVars : OmsiObject
    {
        public OmsiObjChronoVars() : base() { }

        internal OmsiObjChronoVars(Memory memory, int address) : base(memory, address) { }

        public byte Deleted
        {
            get => Memory.ReadMemory<byte>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public string ActivFilename
        {
            get => Memory.ReadMemoryString(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public string[] StringsPnt
        {
            get => Memory.ReadMemoryStringArray(Memory.ReadMemory<int>(Address + 0xc));
        }


    }
}
