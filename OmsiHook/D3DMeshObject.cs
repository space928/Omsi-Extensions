using System;

namespace OmsiHook
{
    /// <summary>
    /// Direct 3D Mesh object
    /// </summary>
    public class D3DMeshObject : D3DTransformObject
    {
        internal D3DMeshObject(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public D3DMeshObject() : base() { }
        
        public D3DMatrix OriginMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x60);
            set => Memory.WriteMemory(Address + 0x60, value);
        }
        /// <summary>
        /// ! Warn ! Listed Data type: ID3DxMesh
        /// </summary>
        public IntPtr Mesh // ID3DxMesh I guess read in as a ptr?
        {
            get => new IntPtr(Memory.ReadMemory<int>(Address + 0xa0));
            set => Memory.WriteMemory(Address + 0xa0, value.ToInt32());
        }
        public D3DOBB OBB
        {
            get => Memory.ReadMemory<D3DOBB>(Address + 0xa4);
            set => Memory.WriteMemory(Address + 0xa4, value);
        }
        /// <summary>
        /// ! Warn ! Listed Data type: ID3DXBuffer
        /// </summary>
        public IntPtr D3DXBuffer_Adja // ID3DXBuffer I guess read in as a ptr?
        {
            get => new IntPtr(Memory.ReadMemory<int>(Address + 0xa8));
            set => Memory.WriteMemory(Address + 0xa8, value.ToInt32());
        }
        /// <summary>
        /// ! Warn ! Listed Data type: ID3DXBuffer
        /// </summary>
        public IntPtr D3DXBuffer_Mtrl // ID3DXBuffer I guess read in as a ptr?
        {
            get => new IntPtr(Memory.ReadMemory<int>(Address + 0xac));
            set => Memory.WriteMemory(Address + 0xac, value.ToInt32());
        }
        /// <summary>
        /// ! Warn ! Listed Data type: ID3DXBuffer
        /// </summary>
        public IntPtr D3DXBuffer_Eff // ID3DXBuffer I guess read in as a ptr?
        {
            get => new IntPtr(Memory.ReadMemory<int>(Address + 0xb0));
            set => Memory.WriteMemory(Address + 0xb0, value.ToInt32());
        }
        public OmsiMaterialProp[] Materials
        {
            get => Memory.MarshalStructs<OmsiMaterialProp, OmsiMaterialPropInternal>(
                Memory.ReadMemoryStructArray<OmsiMaterialPropInternal>(Address + 0xec));
        }
        
        public string[] MaterialNames
        {
            get => Memory.ReadMemoryStringArray(Address + 0xf0);
        }
        public bool UseCharMatrix
        {
            get => Memory.ReadMemory<bool>(Address + 0xf4);
            set => Memory.WriteMemory(Address + 0xf4, value);
        }
        public D3DMatrix CharMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0xf5);
            set => Memory.WriteMemory(Address + 0xf5, value);
        }
        public D3DMatrix CharMatrixInv
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x135);
            set => Memory.WriteMemory(Address + 0x135, value);
        }
    }
}