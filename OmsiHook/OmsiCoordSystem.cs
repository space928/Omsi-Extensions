using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    public class OmsiCoordSystem : OmsiObject
    {
        public OmsiCoordSystem() : base() { }

        internal OmsiCoordSystem(Memory memory, int address) : base(memory, address) { }

        public float InvStartRadius
        {
            get => Memory.ReadMemory<float>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public float InvEndRadius
        {
            get => Memory.ReadMemory<float>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        /// <summary>
        /// Something to do with Euler Spiral?
        /// </summary>
        public bool Klothoide
        {
            get => Memory.ReadMemory<bool>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public float A2
        {
            get => Memory.ReadMemory<float>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        /// <summary>
        /// Slope
        /// </summary>
        public float SteigungA
        {
            get => Memory.ReadMemory<float>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public float SteigungB
        {
            get => Memory.ReadMemory<float>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        public float SteigungC
        {
            get => Memory.ReadMemory<float>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        /// <summary>
        /// Tilt ?
        /// </summary>
        public float NeigungStart
        {
            get => Memory.ReadMemory<float>(Address + 0x20);
            set => Memory.WriteMemory(Address + 0x20, value);
        }
        public float NeigungD
        {
            get => Memory.ReadMemory<float>(Address + 0x24);
            set => Memory.WriteMemory(Address + 0x24, value);
        }
        /// <summary>
        /// Slope half width?
        /// </summary>
        public float NeigungShalbBreite
        {
            get => Memory.ReadMemory<float>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
    }
}
