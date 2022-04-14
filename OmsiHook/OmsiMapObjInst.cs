namespace OmsiHook
{
    public class OmsiMapObjInst : OmsiObject
    {
        internal OmsiMapObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }

        public D3DVector Position
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x4);
            set => omsiMemory.WriteMemory(baseAddress + 0x4, value);
        }

        public D3DMatrix Pos_Mat
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x10);
            set => omsiMemory.WriteMemory(baseAddress + 0x10, value);
        }

        public D3DXQuaternion Rotation => omsiMemory.ReadMemory<D3DXQuaternion>(baseAddress + 0x50);
        public float Scale
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x60);
            set => omsiMemory.WriteMemory(baseAddress + 0x60, value);
        }

        public D3DMatrix RelMatrix => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x64);
        public D3DMatrix Used_RelVec => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x68);
        public int Kachel
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x74); 
            set => omsiMemory.WriteMemory(baseAddress + 0x74, value);
        }

        public D3DMatrix AbsPosition
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0x78);
            set => omsiMemory.WriteMemory(baseAddress + 0x78, value);
        }

        public D3DMatrix AbsPosition_Inv => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0xb8);
        public D3DMatrix AbsPosition_ThreadFree => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0xf8);
    }
}