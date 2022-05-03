namespace OmsiHook
{
    public class OmsiPhysObj : OmsiObject
    {
        internal OmsiPhysObj(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiPhysObj() : base() { }

        public string FileName // ANSI String
        {
            get => Memory.ReadMemoryString(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public int FileVersion
        {
            get => Memory.ReadMemory<int>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public float Mass
        {
            get => Memory.ReadMemory<float>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public float J_X
        {
            get => Memory.ReadMemory<float>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public float J_Y
        {
            get => Memory.ReadMemory<float>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public float J_Z
        {
            get => Memory.ReadMemory<float>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        public D3DMatrix PH_I
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public D3DMatrix PH_I_Inv
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x5c);
            set => Memory.WriteMemory(Address + 0x5c, value);
        }
        public D3DVector CoG
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x9c);
            set => Memory.WriteMemory(Address + 0x9c, value);
        }
        public D3DOBB BoundingBox
        {
            get => Memory.ReadMemory<D3DOBB>(Address + 0xa8);
            set => Memory.WriteMemory(Address + 0xa8, value);
        }
        public float Z_B
        {
            get => Memory.ReadMemory<float>(Address + 0xe4);
            set => Memory.WriteMemory(Address + 0xe4, value);
        }
        public float Z_F
        {
            get => Memory.ReadMemory<float>(Address + 0xe8);
            set => Memory.WriteMemory(Address + 0xe8, value);
        }
        public float X_L
        {
            get => Memory.ReadMemory<float>(Address + 0xec);
            set => Memory.WriteMemory(Address + 0xec, value);
        }
        public float X_R
        {
            get => Memory.ReadMemory<float>(Address + 0xf0);
            set => Memory.WriteMemory(Address + 0xf0, value);
        }

        /*
         *  █▀▄▀█ █ █▀ █▀ █ █▄░█ █▀▀   █▀▄ ▄▀█ ▀█▀ ▄▀█
         *  █░▀░█ █ ▄█ ▄█ █ █░▀█ █▄█   █▄▀ █▀█ ░█░ █▀█ 
         * 0xF4 - 0x153 UNKNOWN DATA
         * 
         */

        public bool Fest
        {
            get => Memory.ReadMemory<bool>(Address + 0x154);
            set => Memory.WriteMemory(Address + 0x154, value);
        }
        public bool No_Collision
        {
            get => Memory.ReadMemory<bool>(Address + 0x155);
            set => Memory.WriteMemory(Address + 0x155, value);
        }
        public bool Active
        {
            get => Memory.ReadMemory<bool>(Address + 0x156);
            set => Memory.WriteMemory(Address + 0x156, value);
        }
        public bool Surface
        {
            get => Memory.ReadMemory<bool>(Address + 0x157);
            set => Memory.WriteMemory(Address + 0x157, value);
        }
        public bool CrashMode_Mast
        {
            get => Memory.ReadMemory<bool>(Address + 0x158);
            set => Memory.WriteMemory(Address + 0x158, value);
        }
        public float CrashMode_Mast_M
        {
            get => Memory.ReadMemory<float>(Address + 0x15c);
            set => Memory.WriteMemory(Address + 0x15c, value);
        }
        public float CrashMode_Mast_Max
        {
            get => Memory.ReadMemory<float>(Address + 0x160);
            set => Memory.WriteMemory(Address + 0x160, value);
        }
        public bool CollMesh_Valid
        {
            get => Memory.ReadMemory<bool>(Address + 0x164);
            set => Memory.WriteMemory(Address + 0x164, value);
        }
        public string CollMesh_FileName
        {
            get => Memory.ReadMemoryString(Address + 0x168); // ANSI STRING
            set => Memory.WriteMemory(Address + 0x168, value);
        }
        /* TODO:
        public DVector3Ptr CollMesh_Vertices
        {
            get => Memory.ReadMemory<int>(Address + 0x16c);
            set => Memory.WriteMemory(Address + 0x16c, value);
        }*/
        /* TODO:
        public IntPtr CollMesh_Indices
        {
            get => Memory.ReadMemory<IntPtr>(Address + 0x170);
            set => Memory.WriteMemory(Address + 0x170, value);
        }*/
        public short CollMesh_Vertex_Cnt
        {
            get => Memory.ReadMemory<short>(Address + 0x174);
            set => Memory.WriteMemory(Address + 0x174, value);
        }
        public int CollMesh_Index_Cnt
        {
            get => Memory.ReadMemory<int>(Address + 0x176);
            set => Memory.WriteMemory(Address + 0x176, value);
        }

        /* TODO:
        public DXTriMeshDataPtr CollMesh_TriMeshData
        {
            get => Memory.ReadMemory<DXTriMeshDataPtr>(Address + 0x178);
            set => Memory.WriteMemory(Address + 0x178, value);
        }*/

    }
}