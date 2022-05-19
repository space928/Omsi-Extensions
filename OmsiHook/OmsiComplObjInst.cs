using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// More advanced data for complex map object instances, such as string vars
    /// </summary>
    public class OmsiComplObjInst : OmsiObject
    {
        internal OmsiComplObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiComplObjInst() : base() { }

        public bool RefreshSkins_Calc
        {
            get => Memory.ReadMemory<bool>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public int ActLOD
        {
            get => Memory.ReadMemory<int>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public float Dist
        {
            get => Memory.ReadMemory<int>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public float RelDist
        {
            get => Memory.ReadMemory<int>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public float ScreenSize
        {
            get => Memory.ReadMemory<int>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public bool TextureRedunction
        {
            get => Memory.ReadMemory<bool>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        public bool Visible
        {
            get => Memory.ReadMemory<bool>(Address + 0x19);
            set => Memory.WriteMemory(Address + 0x19, value);
        }
        public bool RefreshAllFreeTexs
        {
            get => Memory.ReadMemory<bool>(Address + 0x1a);
            set => Memory.WriteMemory(Address + 0x1a, value);
        }
        public float Refresh_OFT
        {
            get => Memory.ReadMemory<float>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public OmsiComplObj ComplObj
        {
            get => new(Memory, Address + 0x20);
        }
    }
}
