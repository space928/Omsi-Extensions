namespace OmsiHook
{
    public class OmsiPhysObjInst : OmsiMapObjInst
    {
        internal OmsiPhysObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiPhysObjInst() : base() { }

        /*
         * Unimplemented fields:
         * PdxBody PH_Body; 0x138
         * PdxGeom PH_MainGeomTrafo; 0x13c
         * PdxGeom PH_MainGeom; 0x140
         * Cardinal mast_joint; 0x144
         */

        public D3DVector Mast_BasePoint
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x148);
            set => Memory.WriteMemory(Address + 0x148, value);
        }

        public float F_Long
        {
            get => Memory.ReadMemory<float>(Address + 0x154);
            set => Memory.WriteMemory(Address + 0x154, value);
        }

        public float F_Vert
        {
            get => Memory.ReadMemory<float>(Address + 0x158);
            set => Memory.WriteMemory(Address + 0x158, value);
        }

        public float F_Lat
        {
            get => Memory.ReadMemory<float>(Address + 0x15c);
            set => Memory.WriteMemory(Address + 0x15c, value);
        }

        public float M_Long
        {
            get => Memory.ReadMemory<float>(Address + 0x160);
            set => Memory.WriteMemory(Address + 0x160, value);
        }

        public float M_Lat
        {
            get => Memory.ReadMemory<float>(Address + 0x164);
            set => Memory.WriteMemory(Address + 0x164, value);
        }

        public float M_Vert
        {
            get => Memory.ReadMemory<float>(Address + 0x168);
            set => Memory.WriteMemory(Address + 0x168, value);
        }

        public float CrashMode_Mast_GES
        {
            get => Memory.ReadMemory<float>(Address + 0x16c);
            set => Memory.WriteMemory(Address + 0x16c, value);
        }

        /*public OmsiPhysObj PhysObj
        {
            get => omsiMemory.ReadMemory<OmsiPhysObj>(omsiMemory.ReadMemory<int>(baseAddress + 0x170));
            set => omsiMemory.WriteMemory(baseAddress + 0x170, value);
        }*/

        public D3DVector Velocity
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x174);
            set => Memory.WriteMemory(Address + 0x174, value);
        }

        public D3DVector LastForce
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x180);
            set => Memory.WriteMemory(Address + 0x180, value);
        }

        public D3DVector LastMoment
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x18c);
            set => Memory.WriteMemory(Address + 0x18c, value);
        }

        public D3DVector Long
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x198);
            set => Memory.WriteMemory(Address + 0x198, value);
        }

        public D3DVector Lat
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x1a4);
            set => Memory.WriteMemory(Address + 0x1a4, value);
        }

        public D3DVector Vert
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x1b0);
            set => Memory.WriteMemory(Address + 0x174, value);
        }

        public float Hdg
        {
            get => Memory.ReadMemory<float>(Address + 0x1bc);
            set => Memory.WriteMemory(Address + 0x1bc, value);
        }

        public D3DVector Turn_Velocity
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x1c0);
            set => Memory.WriteMemory(Address + 0x1c0, value);
        }

        public D3DVector Veloc_Lokal
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x1cc);
            set => Memory.WriteMemory(Address + 0x1cc, value);
        }

        public byte Contact
        {
            get => Memory.ReadMemory<byte>(Address + 0x1d8);
            set => Memory.WriteMemory(Address + 0x1d8, value);
        }

        public OmsiCamera Camera => new(Memory, Memory.ReadMemory<int>(Address + 0x1dc));
    }
}