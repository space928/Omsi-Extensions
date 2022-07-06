using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    public class OmsiObjBlackRect : D3DTransformObject
    {
        public OmsiObjBlackRect() : base() { }

        internal OmsiObjBlackRect(Memory memory, int address) : base(memory, address) { }

        /// <summary>
        /// Pointer to IDirect3DVertexBuffer9
        /// </summary>
        public IntPtr VB
        {
            get => new(Memory.ReadMemory<int>(Address + 0x60));
        }
    }
}
