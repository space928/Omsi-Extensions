namespace OmsiHook
{
    public class OmsiPhysObjInst : OmsiMapObjInst
    {
        internal OmsiPhysObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }

        /*
         * Unimplemented fields:
         * PdxBody PH_Body; 0x138
         * PdxGeom PH_MainGeomTrafo; 0x13c
         * PdxGeom PH_MainGeom; 0x140
         * Cardinal mast_joint; 0x144
         */

        public D3DVector Mast_BasePoint
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x148);
            set => omsiMemory.WriteMemory(baseAddress + 0x148, value);
        }

        public float F_Long
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x154);
            set => omsiMemory.WriteMemory(baseAddress + 0x154, value);
        }

        public float F_Vert
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x158);
            set => omsiMemory.WriteMemory(baseAddress + 0x158, value);
        }

        public float F_Lat
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x15c);
            set => omsiMemory.WriteMemory(baseAddress + 0x15c, value);
        }

        public float M_Long
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x160);
            set => omsiMemory.WriteMemory(baseAddress + 0x160, value);
        }

        public float M_Lat
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x164);
            set => omsiMemory.WriteMemory(baseAddress + 0x164, value);
        }

        public float M_Vert
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x168);
            set => omsiMemory.WriteMemory(baseAddress + 0x168, value);
        }

        public float CrashMode_Mast_GES
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x16c);
            set => omsiMemory.WriteMemory(baseAddress + 0x16c, value);
        }

        /*public OmsiPhysObj PhysObj
        {
            get => omsiMemory.ReadMemory<OmsiPhysObj>(omsiMemory.ReadMemory<int>(baseAddress + 0x170));
            set => omsiMemory.WriteMemory(baseAddress + 0x170, value);
        }*/

        public D3DVector Velocity
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x174);
            set => omsiMemory.WriteMemory(baseAddress + 0x174, value);
        }

        public D3DVector LastForce
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x180);
            set => omsiMemory.WriteMemory(baseAddress + 0x180, value);
        }

        public D3DVector LastMoment
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x18c);
            set => omsiMemory.WriteMemory(baseAddress + 0x18c, value);
        }

        public D3DVector Long
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x198);
            set => omsiMemory.WriteMemory(baseAddress + 0x198, value);
        }

        public D3DVector Lat
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x1a4);
            set => omsiMemory.WriteMemory(baseAddress + 0x1a4, value);
        }

        public D3DVector Vert
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x1b0);
            set => omsiMemory.WriteMemory(baseAddress + 0x174, value);
        }

        public float Hdg
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x1bc);
            set => omsiMemory.WriteMemory(baseAddress + 0x1bc, value);
        }

        public D3DVector Turn_Velocity
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x1c0);
            set => omsiMemory.WriteMemory(baseAddress + 0x1c0, value);
        }

        public D3DVector Veloc_Lokal
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x1cc);
            set => omsiMemory.WriteMemory(baseAddress + 0x1cc, value);
        }

        public byte Contact
        {
            get => omsiMemory.ReadMemory<byte>(baseAddress + 0x1d8);
            set => omsiMemory.WriteMemory(baseAddress + 0x1d8, value);
        }

        public OmsiCamera Camera => new(omsiMemory, omsiMemory.ReadMemory<int>(baseAddress + 0x1dc));
    }
}