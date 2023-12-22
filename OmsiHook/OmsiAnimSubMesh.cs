using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// A mesh within a complex mmap object. This represents a single [mesh] tag in a cfg file.
    /// </summary>
    public class OmsiAnimSubMesh : D3DMeshFileObject
    {
        internal OmsiAnimSubMesh(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiAnimSubMesh() : base() { }

        public string Filename_Int
        {
            get => Memory.ReadMemoryString(Address + 0x18c, StrPtrType.DelphiAnsiString);
            set => Memory.WriteMemory(Address + 0x18c, value, StrPtrType.DelphiAnsiString);
        }
        public string ident
        {
            get => Memory.ReadMemoryString(Address + 0x190, StrPtrType.DelphiAnsiString);
            set => Memory.WriteMemory(Address + 0x190, value, StrPtrType.DelphiAnsiString);
        }
        public int AnimParent
        {
            get => Memory.ReadMemory<int>(Address + 0x194);
            set => Memory.WriteMemory(Address + 0x194, value);
        }
        //public MemArrayList<D3DMatrix> origin
        //public MemArrayList<D3DMatrix> originI
        //public MemArrayList<OmsiAnimation> anim
        public int Visible
        {
            get => Memory.ReadMemory<int>(Address + 0x1a4);
            set => Memory.WriteMemory(Address + 0x1a4, value);
        }
        public int VisibleValue
        {
            get => Memory.ReadMemory<int>(Address + 0x1a8);
            set => Memory.WriteMemory(Address + 0x1a8, value);
        }
        //public MemArrayList<OmsiRauch> Rauchs
        //public MemArrayList<OmsiLampenSetting> LampenSettings
        //public MemArrayList<OmsiInteriorLight> InteriorLights
        //public MemArrayList<OmsiBoneProp> BoneProps
        public bool SmoothSkin
        {
            get => Memory.ReadMemory<bool>(Address + 0x1bc);
            set => Memory.WriteMemory(Address + 0x1bc, value);
        }
        public float Skin_MaxRelDist
        {
            get => Memory.ReadMemory<float>(Address + 0x1c0);
            set => Memory.WriteMemory(Address + 0x1c0, value);
        }
        public string MausEvent
        {
            get => Memory.ReadMemoryString(Address + 0x1c4, StrPtrType.DelphiAnsiString);
            set => Memory.WriteMemory(Address + 0x1c4, value, StrPtrType.DelphiAnsiString);
        }
        public int MyLOD
        {
            get => Memory.ReadMemory<int>(Address + 0x1c8);
            set => Memory.WriteMemory(Address + 0x1c8, value);
        }
        public byte Flag_GetLights
        {
            get => Memory.ReadMemory<byte>(Address + 0x1cc);
            set => Memory.WriteMemory(Address + 0x1cc, value);
        }
        /* TODO:
        public int GetInteriorLights
        {
            get => Memory.ReadMemory<int>(Address + 0x1cd);
            set => Memory.WriteMemory(Address + 0x1cd, value);
        }*/
        public byte Flag_ViewPoint
        {
            get => Memory.ReadMemory<byte>(Address + 0x1d1);
            set => Memory.WriteMemory(Address + 0x1d1, value);
        }
        public bool HasShadow
        {
            get => Memory.ReadMemory<bool>(Address + 0x1d2);
            set => Memory.WriteMemory(Address + 0x1d2, value);
        }
        public bool Shadow
        {
            get => Memory.ReadMemory<bool>(Address + 0x1d2);
            set => Memory.WriteMemory(Address + 0x1d2, value);
        }
    }
}
