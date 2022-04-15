namespace OmsiHook
{
    public class OmsiCamera : OmsiObject
    {
        internal OmsiCamera(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress)
        {
        }

        public string Name
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x4);
            set => omsiMemory.WriteMemory(baseAddress + 0x4, value);
        }
        public float Ratio
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x8);
            set => omsiMemory.WriteMemory(baseAddress + 0x8, value);
        }
        public float NearZ
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0xc);
            set => omsiMemory.WriteMemory(baseAddress + 0xc, value);
        }
        public float FarZ
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x10);
            set => omsiMemory.WriteMemory(baseAddress + 0x10, value);
        }
        public float Z_Offset
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x14);
            set => omsiMemory.WriteMemory(baseAddress + 0x14, value);
        }
        public bool Mirror
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x18);
            set => omsiMemory.WriteMemory(baseAddress + 0x18, value);
        }
        public float Angle_HDG
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x1c);
            set => omsiMemory.WriteMemory(baseAddress + 0x1c, value);
        }
        public float Angle_HGT
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x20);
            set => omsiMemory.WriteMemory(baseAddress + 0x20, value);
        }
        public float Dist
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x24);
            set => omsiMemory.WriteMemory(baseAddress + 0x24, value);
        }
        public float Angle_HDG_Render
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x28);
            set => omsiMemory.WriteMemory(baseAddress + 0x28, value);
        }
        public float Angle_HGT_Render
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x2c);
            set => omsiMemory.WriteMemory(baseAddress + 0x2c, value);
        }
        public float Dist_Render
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x30);
            set => omsiMemory.WriteMemory(baseAddress + 0x30, value);
        }

        /// <summary>
        /// Field Of View
        /// </summary>
        public float Sichtwinkel_Render
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x34);
            set => omsiMemory.WriteMemory(baseAddress + 0x34, value);
        }
        public float Sichtwinkel
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x38);
            set => omsiMemory.WriteMemory(baseAddress + 0x38, value);
        }
        public D3DMatrix MatView
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x3c);
            set => omsiMemory.WriteMemory(baseAddress + 0x3c, value);
        }
        public D3DMatrix MatProj
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x7c);
            set => omsiMemory.WriteMemory(baseAddress + 0x7c, value);
        }
        public D3DMatrix MatView_Calc
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0xbc);
            set => omsiMemory.WriteMemory(baseAddress + 0xbc, value);
        }
        public D3DMatrix MatProj_Calc
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0xfc);
            set => omsiMemory.WriteMemory(baseAddress + 0xfc, value);
        }
        public D3DVector Cam_CenterPoint
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x13c);
            set => omsiMemory.WriteMemory(baseAddress + 0x13c, value);
        }
        public D3DVector Cam_Pos
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x148);
            set => omsiMemory.WriteMemory(baseAddress + 0x148, value);
        }
        public D3DVector Cam_Dir
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x154);
            set => omsiMemory.WriteMemory(baseAddress + 0x154, value);
        }
        public D3DVector Cam_Up
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x160);
            set => omsiMemory.WriteMemory(baseAddress + 0x160, value);
        }
        public D3DVector Cam_CenterPoint_Calc
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x16c);
            set => omsiMemory.WriteMemory(baseAddress + 0x16c, value);
        }
        public D3DVector Cam_Pos_Calc
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x178);
            set => omsiMemory.WriteMemory(baseAddress + 0x178, value);
        }
        public D3DVector Cam_Dir_Calc
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x184);
            set => omsiMemory.WriteMemory(baseAddress + 0x184, value);
        }
        public D3DVector Cam_Up_Calc
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x190);
            set => omsiMemory.WriteMemory(baseAddress + 0x190, value);
        }
        public bool CenterPoint_Global_OK
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x19c);
            set => omsiMemory.WriteMemory(baseAddress + 0x19c, value);
        }
        public D3DVector CenterPoint_Global
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x19d);
            set => omsiMemory.WriteMemory(baseAddress + 0x19d, value);
        }
        public D3DMatrix CamPntMatrix
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x1a9);
            set => omsiMemory.WriteMemory(baseAddress + 0x1a9, value);
        }
        public D3DMatrix CamPntMatrix_Calc
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x1e9);
            set => omsiMemory.WriteMemory(baseAddress + 0x1e9, value);
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
            get => omsiMemory.ReadMemory <D3DVector> (baseAddress + 0x2e9);
            set => omsiMemory.WriteMemory(baseAddress + 0x2e9, value);
        }
        public D3DVector Pos_Render
        {
            get => omsiMemory.ReadMemory <D3DVector> (baseAddress + 0x2f5);
            set => omsiMemory.WriteMemory(baseAddress + 0x2f5, value);
        }
        public float Angle_HDG_Norm
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x304);
            set => omsiMemory.WriteMemory(baseAddress + 0x304, value);
        }
        public float Angle_HGT_Norm
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x308);
            set => omsiMemory.WriteMemory(baseAddress + 0x308, value);
        }
        public D3DVector Pos_Norm
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x30c);
            set => omsiMemory.WriteMemory(baseAddress + 0x30c, value);
        }
        public bool ConstDist
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x318);
            set => omsiMemory.WriteMemory(baseAddress + 0x318, value);
        }
        /// <summary>
        /// Field Of View
        /// </summary>
        public float Sichtwinkel_Norm
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x31c);
            set => omsiMemory.WriteMemory(baseAddress + 0x31c, value);
        }
        public D3DVector Dir_Vec_Norm
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x320);
            set => omsiMemory.WriteMemory(baseAddress + 0x320, value);
        }
        public D3DMatrix Local_Coord
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x32c);
            set => omsiMemory.WriteMemory(baseAddress + 0x32c, value);
        }
        public D3DMatrix Transform
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x330);
            set => omsiMemory.WriteMemory(baseAddress + 0x330, value);
        }
        public D3DMatrix Transform_Render
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x334);
            set => omsiMemory.WriteMemory(baseAddress + 0x334, value);
        }
        public D3DMatrix Local_Coord_Render
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x374);
            set => omsiMemory.WriteMemory(baseAddress + 0x374, value);
        }
        public D3DMatrix NullMatrix
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x3b4);
            set => omsiMemory.WriteMemory(baseAddress + 0x3b4, value);
        }
        /// <summary>
        /// Floor Height
        /// </summary>
        public float Bodenhoehe
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x3f4);
            set => omsiMemory.WriteMemory(baseAddress + 0x3f4, value);
        }
    }
}