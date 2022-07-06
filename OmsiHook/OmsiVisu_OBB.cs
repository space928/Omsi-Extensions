using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    public class OmsiVisu_OBB : D3DTransformObject
    {
        public OmsiVisu_OBB() : base() { }

        internal OmsiVisu_OBB(Memory memory, int address) : base(memory, address) { }

        public IntPtr Mesh
        {
            get => new(Memory.ReadMemory<int>(Address + 0x60));
        }
        public D3DMaterial9 Material
        {
            get => Memory.ReadMemory<D3DMaterial9>(Address + 0x64);
        }

    }
}
