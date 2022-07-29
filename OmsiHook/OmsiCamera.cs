namespace OmsiHook
{
    /// <summary>
    /// In game camera object
    /// </summary>
    public class OmsiCamera : OmsiObject
    {
        internal OmsiCamera(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiCamera() : base() { }

        public string Name
        {
            get => Memory.ReadMemoryString(Address + 0x4, true);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public float Ratio
        {
            get => Memory.ReadMemory<float>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public float NearZ
        {
            get => Memory.ReadMemory<float>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public float FarZ
        {
            get => Memory.ReadMemory<float>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public float Z_Offset
        {
            get => Memory.ReadMemory<float>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public bool Mirror
        {
            get => Memory.ReadMemory<bool>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        public float Angle_HDG
        {
            get => Memory.ReadMemory<float>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public float Angle_HGT
        {
            get => Memory.ReadMemory<float>(Address + 0x20);
            set => Memory.WriteMemory(Address + 0x20, value);
        }
        public float Dist
        {
            get => Memory.ReadMemory<float>(Address + 0x24);
            set => Memory.WriteMemory(Address + 0x24, value);
        }
        public float Angle_HDG_Render
        {
            get => Memory.ReadMemory<float>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public float Angle_HGT_Render
        {
            get => Memory.ReadMemory<float>(Address + 0x2c);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }
        public float Dist_Render
        {
            get => Memory.ReadMemory<float>(Address + 0x30);
            set => Memory.WriteMemory(Address + 0x30, value);
        }

        /// <summary>
        /// Field Of View
        /// </summary>
        public float Sichtwinkel_Render
        {
            get => Memory.ReadMemory<float>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }
        public float Sichtwinkel
        {
            get => Memory.ReadMemory<float>(Address + 0x38);
            set => Memory.WriteMemory(Address + 0x38, value);
        }
        public D3DMatrix MatView
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x3c);
            set => Memory.WriteMemory(Address + 0x3c, value);
        }
        public D3DMatrix MatProj
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x7c);
            set => Memory.WriteMemory(Address + 0x7c, value);
        }
        public D3DMatrix MatView_Calc
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0xbc);
            set => Memory.WriteMemory(Address + 0xbc, value);
        }
        public D3DMatrix MatProj_Calc
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0xfc);
            set => Memory.WriteMemory(Address + 0xfc, value);
        }
        public D3DVector Cam_CenterPoint
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x13c);
            set => Memory.WriteMemory(Address + 0x13c, value);
        }
        public D3DVector Cam_Pos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x148);
            set => Memory.WriteMemory(Address + 0x148, value);
        }
        public D3DVector Cam_Dir
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x154);
            set => Memory.WriteMemory(Address + 0x154, value);
        }
        public D3DVector Cam_Up
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x160);
            set => Memory.WriteMemory(Address + 0x160, value);
        }
        public D3DVector Cam_CenterPoint_Calc
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x16c);
            set => Memory.WriteMemory(Address + 0x16c, value);
        }
        public D3DVector Cam_Pos_Calc
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x178);
            set => Memory.WriteMemory(Address + 0x178, value);
        }
        public D3DVector Cam_Dir_Calc
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x184);
            set => Memory.WriteMemory(Address + 0x184, value);
        }
        public D3DVector Cam_Up_Calc
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x190);
            set => Memory.WriteMemory(Address + 0x190, value);
        }
        public bool CenterPoint_Global_OK
        {
            get => Memory.ReadMemory<bool>(Address + 0x19c);
            set => Memory.WriteMemory(Address + 0x19c, value);
        }
        public D3DVector CenterPoint_Global
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x19d);
            set => Memory.WriteMemory(Address + 0x19d, value);
        }
        public D3DMatrix CamPntMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x1a9);
            set => Memory.WriteMemory(Address + 0x1a9, value);
        }
        public D3DMatrix CamPntMatrix_Calc
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x1e9);
            set => Memory.WriteMemory(Address + 0x1e9, value);
        }


        // TODO: VFDPlane_Render - struct D3DXPlane[6] @ 0x229
        /*public D3DXPlane[] VFDPlane_Render
        {
            get => omsiMemory.ReadMemory<D3DXPlane[6]>(baseAddress + 0x229);
            set => omsiMemory.WriteMemory(baseAddress + 0x229, value);
        }*/
        // TODO: VFDPlane_Calc - struct D3DXPlane[6] @ 0x289
        /*public D3DXPlane[] VFDPlane_Calc
        {
            get => omsiMemory.ReadMemory<D3DXPlane[6]>(baseAddress + 0x289);
            set => omsiMemory.WriteMemory(baseAddress + 0x289, value);
        }*/


        public D3DVector Pos
        {
            get => Memory.ReadMemory <D3DVector> (Address + 0x2e9);
            set => Memory.WriteMemory(Address + 0x2e9, value);
        }
        public D3DVector Pos_Render
        {
            get => Memory.ReadMemory <D3DVector> (Address + 0x2f5);
            set => Memory.WriteMemory(Address + 0x2f5, value);
        }
        public float Angle_HDG_Norm
        {
            get => Memory.ReadMemory<float>(Address + 0x304);
            set => Memory.WriteMemory(Address + 0x304, value);
        }
        public float Angle_HGT_Norm
        {
            get => Memory.ReadMemory<float>(Address + 0x308);
            set => Memory.WriteMemory(Address + 0x308, value);
        }
        public D3DVector Pos_Norm
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x30c);
            set => Memory.WriteMemory(Address + 0x30c, value);
        }
        public bool ConstDist
        {
            get => Memory.ReadMemory<bool>(Address + 0x318);
            set => Memory.WriteMemory(Address + 0x318, value);
        }
        /// <summary>
        /// Field Of View
        /// </summary>
        public float Sichtwinkel_Norm
        {
            get => Memory.ReadMemory<float>(Address + 0x31c);
            set => Memory.WriteMemory(Address + 0x31c, value);
        }
        public D3DVector Dir_Vec_Norm
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x320);
            set => Memory.WriteMemory(Address + 0x320, value);
        }
        public D3DMatrix Local_Coord
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x32c);
            set => Memory.WriteMemory(Address + 0x32c, value);
        }
        public D3DMatrix Transform
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x330);
            set => Memory.WriteMemory(Address + 0x330, value);
        }
        public D3DMatrix Transform_Render
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x334);
            set => Memory.WriteMemory(Address + 0x334, value);
        }
        public D3DMatrix Local_Coord_Render
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x374);
            set => Memory.WriteMemory(Address + 0x374, value);
        }
        public D3DMatrix NullMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x3b4);
            set => Memory.WriteMemory(Address + 0x3b4, value);
        }
        /// <summary>
        /// Floor Height
        /// </summary>
        public float Bodenhoehe
        {
            get => Memory.ReadMemory<float>(Address + 0x3f4);
            set => Memory.WriteMemory(Address + 0x3f4, value);
        }
    }
}