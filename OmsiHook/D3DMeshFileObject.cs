namespace OmsiHook
{
    /// <summary>
    /// Direct 3D Mesh File, all the metadata relating to a loaded mesh file
    /// </summary>
    public class D3DMeshFileObject : D3DMeshObject
    {
        internal D3DMeshFileObject(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public D3DMeshFileObject() : base() { }

        public bool Loaded_MeshFileObject
        {
            get => Memory.ReadMemory<bool>(Address + 0x178);
            set => Memory.WriteMemory(Address + 0x178, value);
        }
        public string Filename
        {
            get => Memory.ReadMemoryString(Address + 0x178);
            set => Memory.WriteMemory(Address + 0x178, value);
        }
        public OmsiWeightData[] WeightData
        {
            get => Memory.MarshalStructs<OmsiWeightData, OmsiWeightDataInternal>(
                Memory.ReadMemoryStructArray<OmsiWeightDataInternal>(Address + 0x180));
        }
        
        public sbyte[] WeightData_Easy
        {
            get => Memory.ReadMemoryStructArray<sbyte>(Address + 0x184);
        }
        public string[] BoneNames
        {
            get => Memory.ReadMemoryStringArray(Address + 0x188);
        }
    }
}