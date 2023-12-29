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
    public class OmsiAnimSubMeshInst : OmsiObject
    {
        internal OmsiAnimSubMeshInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiAnimSubMeshInst() : base() { }

        public D3DTransformObject ShadowObject // TODO: Should be a D3DShadowObject
        {
            get => new(Memory, Address + 0x4);
        }
        public D3DMatrix Matrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        /// <summary>
        /// Normalized matrix
        /// </summary>
        public D3DMatrix EntzerrteMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x48);
            set => Memory.WriteMemory(Address + 0x48, value);
        }
        public D3DMatrix LocalMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x88);
            set => Memory.WriteMemory(Address + 0x88, value);
        }
        /* TODO:
        public MemArrayList<OmsiMaterialProp> Matl
        {
            get => new(Memory, Address + 0xc8);
        }*/
        public int Tex
        {
            get => Memory.ReadMemory<int>(Address + 0xcc);
            set => Memory.WriteMemory(Address + 0xcc, value);
        }
        public bool Visible
        {
            get => Memory.ReadMemory<bool>(Address + 0xd0);
            set => Memory.WriteMemory(Address + 0xd0, value);
        }
        // TODO: MemArrayList<OmsiLampe> Lampen @ 0xd4
        // TODO: MemArrayList<OmsiInteriorLight> InteriorLights @ 0xd8
        // TODO: MemArrayList<float> Anim_LastValue @ 0xdc
        public IntPtr SkinMesh // ID3DXMesh
        {
            get => Memory.ReadMemory<IntPtr>(Address + 0xe0);
            set => Memory.WriteMemory(Address + 0xe0, value);
        }
        public bool SkinMesh_Use
        {
            get => Memory.ReadMemory<bool>(Address + 0xe4);
            set => Memory.WriteMemory(Address + 0xe4, value);
        }
        public bool RefreshSkinMesh_Render
        {
            get => Memory.ReadMemory<bool>(Address + 0xe5);
            set => Memory.WriteMemory(Address + 0xe5, value);
        }
    }
}
