using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Main Progrma Manager
    /// </summary>
    public class OmsiProgMan : OmsiObject
    {
        public OmsiProgMan() : base() { }

        internal OmsiProgMan(Memory memory, int address) : base(memory, address) { }
        public bool LastStay
        {
            get => Memory.ReadMemory<bool>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public bool HoverObj
        {
            get => Memory.ReadMemory<bool>(Address + 0x5);
            set => Memory.WriteMemory(Address + 0x5, value);
        }
        public bool HoverSpl
        {
            get => Memory.ReadMemory<bool>(Address + 0x6);
            set => Memory.WriteMemory(Address + 0x6, value);
        }
        public bool HoverPaths
        {
            get => Memory.ReadMemory<bool>(Address + 0x7);
            set => Memory.WriteMemory(Address + 0x7, value);
        }
        /* TODO:
        public OmsiAudioMixer AudioMixer
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x8));
        }*/
        /* TODO:
        public D3DText Text2D_Hinweise_V
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0xc));
        }
        public D3DText Text2D_Hinweise_H
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x10));
        }*/
        public OmsiCamera MapCam
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x14));
        }
        public D3DVector MapCamTargetPos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        /* TODO:
        public OmsiThreadCheckMissingScheduledAIVehicles ThreadCheckMissingScheduledAIVehicles
        {
            get => new(Memory, Memory.ReadMemory<int>(0x24));
        }*/
        public D3DMatrix GlobalMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public int Width
        {
            get => Memory.ReadMemory<int>(Address + 0x68);
            set => Memory.WriteMemory(Address + 0x68, value);
        }
        public int Height
        {
            get => Memory.ReadMemory<int>(Address + 0x6c);
            set => Memory.WriteMemory(Address + 0x6c, value);
        }
        public byte InformationDisplay
        {
            get => Memory.ReadMemory<byte>(Address + 0x70);
            set => Memory.WriteMemory(Address + 0x70, value);
        }
        public bool Nacht
        {
            get => Memory.ReadMemory<bool>(Address + 0x71);
            set => Memory.WriteMemory(Address + 0x71, value);
        }
        public int Costs
        {
            get => Memory.ReadMemory<int>(Address + 0x74);
            set => Memory.WriteMemory(Address + 0x74, value);
        }
        public bool IsSaved
        {
            get => Memory.ReadMemory<bool>(Address + 0x78);
            set => Memory.WriteMemory(Address + 0x78, value);
        }
        public bool Dsgn_Rast_Shift
        {
            get => Memory.ReadMemory<bool>(Address + 0x79);
            set => Memory.WriteMemory(Address + 0x79, value);
        }
        public bool Dsgn_Rast_Strg
        {
            get => Memory.ReadMemory<bool>(Address + 0x7a);
            set => Memory.WriteMemory(Address + 0x7a, value);
        }
    }
}
