namespace OmsiHook
{
    /// <summary>
    /// Base Class for all instances of moving map objects - vehicles / humans
    /// </summary>
    public class OmsiMovingMapObjInst : OmsiComplMapObjInst
    {
        internal OmsiMovingMapObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiMovingMapObjInst() : base() { }

        public int Index
        {
            get => Memory.ReadMemory<int>(Address + 0x258);
            set => Memory.WriteMemory(Address + 0x258, value);
        }
        public bool MarkedForKilling
        {
            get => Memory.ReadMemory<bool>(Address + 0x25c);
            set => Memory.WriteMemory(Address + 0x25c, value);
        }
        public MemArray<OmsiBoogieInternal, OmsiBoogie> Boogies
        {
            get => new(Memory, Address + 0x260);
        }
        /// <summary>
        /// Boogies Init Virbrations
        /// </summary>
        public bool Boogies_InitSchwingungen
        {
            get => Memory.ReadMemory<bool>(Address + 0x264);
            set => Memory.WriteMemory(Address + 0x264, value);
        }
        /// <summary>
        /// Steering arm
        /// </summary>
        public float Lenkhebel
        {
            get => Memory.ReadMemory<float>(Address + 0x268);
            set => Memory.WriteMemory(Address + 0x268, value);
        }
        public bool UserTrain
        {
            get => Memory.ReadMemory<bool>(Address + 0x26c);
            set => Memory.WriteMemory(Address + 0x26c, value);
        }
        public bool UserTrain_Render
        {
            get => Memory.ReadMemory<bool>(Address + 0x26c);
            set => Memory.WriteMemory(Address + 0x26c, value);
        }
        public D3DVector Last_Position
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x26e);
            set => Memory.WriteMemory(Address + 0x26e, value);
        }
        public D3DXQuaternion Last_Rotation
        {
            get => Memory.ReadMemory<D3DXQuaternion>(Address + 0x27a);
            set => Memory.WriteMemory(Address + 0x27a, value);
        }
        public D3DMatrix RelMatrixVar
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x28a);
            set => Memory.WriteMemory(Address + 0x28a, value);
        }
        public OmsiPoint MyKachelPnt
        {
            get => Memory.ReadMemory<OmsiPoint>(Address + 0x2cc);
            set => Memory.WriteMemory(Address + 0x2cc, value);
        }
        public OmsiPoint CenterKachel
        {
            get => Memory.ReadMemory<OmsiPoint>(Memory.ReadMemory<int>(Address + 0x2d44));
        }
        public uint CalcTimer
        {
            get => Memory.ReadMemory<uint>(Address + 0x2d8);
            set => Memory.WriteMemory(Address + 0x2d8, value);
        }
        public bool Visible_Logical
        {
            get => Memory.ReadMemory<bool>(Address + 0x2dc);
            set => Memory.WriteMemory(Address + 0x2dc, value);
        }
        public bool Visible_Logical_RenderThread
        {
            get => Memory.ReadMemory<bool>(Address + 0x2dd);
            set => Memory.WriteMemory(Address + 0x2dd, value);
        }
        public bool PathFixed
        {
            get => Memory.ReadMemory<bool>(Address + 0x2de);
            set => Memory.WriteMemory(Address + 0x2de, value);
        }
        public OmsiPathInfo PathInfo
        {
            get => Memory.MarshalStruct<OmsiPathInfo, OmsiPathInfoInternal>(Memory.ReadMemory<OmsiPathInfoInternal>(Address + 0x2e0));
            set => Memory.WriteMemory(Address + 0x2e0, Memory.UnMarshalStruct<OmsiPathInfoInternal, OmsiPathInfo>(value));
        }
        public float PathVOffset
        {
            get => Memory.ReadMemory<float>(Address + 0x3f0);
            set => Memory.WriteMemory(Address + 0x3f0, value);
        }
        public byte TrafficType
        {
            get => Memory.ReadMemory<byte>(Address + 0x3f4);
            set => Memory.WriteMemory(Address + 0x3f4, value);
        }
        public float PAI_Hdg_Soll
        {
            get => Memory.ReadMemory<float>(Address + 0x3f8);
            set => Memory.WriteMemory(Address + 0x3f8, value);
        }
        public float PAI_Hdg_Ist
        {
            get => Memory.ReadMemory<float>(Address + 0x3fc);
            set => Memory.WriteMemory(Address + 0x3fc, value);
        }
        public float PAI_Wagenwinkel
        {
            get => Memory.ReadMemory<float>(Address + 0x400);
            set => Memory.WriteMemory(Address + 0x400, value);
        }
        public float PAI_DeltaBeta
        {
            get => Memory.ReadMemory<float>(Address + 0x404);
            set => Memory.WriteMemory(Address + 0x404, value);
        }
        public float PAI_DeltaAlpha
        {
            get => Memory.ReadMemory<float>(Address + 0x408);
            set => Memory.WriteMemory(Address + 0x408, value);
        }
        public float PAI_InvLenk_Ist
        {
            get => Memory.ReadMemory<float>(Address + 0x40c);
            set => Memory.WriteMemory(Address + 0x40c, value);
        }
        public float PAI_MovingDistance
        {
            get => Memory.ReadMemory<float>(Address + 0x410);
            set => Memory.WriteMemory(Address + 0x410, value);
        }
        public OmsiPathSollwerte PAI_Soll
        {
            get => Memory.ReadMemory<OmsiPathSollwerte>(Address + 0x414);
            set => Memory.WriteMemory(Address + 0x414, value);
        }
        public float Tacho
        {
            get => Memory.ReadMemory<float>(Address + 0x424);
            set => Memory.WriteMemory(Address + 0x424, value);
        }
        public float Groundspeed
        {
            get => Memory.ReadMemory<float>(Address + 0x428);
            set => Memory.WriteMemory(Address + 0x428, value);
        }
        public double KMCounter
        {
            get => Memory.ReadMemory <double>(Address + 0x430);
            set => Memory.WriteMemory(Address + 0x430, value);
        }
        public double KMCounter_Init
        {
            get => Memory.ReadMemory<double>(Address + 0x438);
            set => Memory.WriteMemory(Address + 0x438, value);
        }
        public float KMCounter_KM
        {
            get => Memory.ReadMemory<float>(Address + 0x440);
            set => Memory.WriteMemory(Address + 0x440, value);
        }
        public float KMCounter_M
        {
            get => Memory.ReadMemory<float>(Address + 0x444);
            set => Memory.WriteMemory(Address + 0x444, value);
        }
        public float RelRange
        {
            get => Memory.ReadMemory<float>(Address + 0x448);
            set => Memory.WriteMemory(Address + 0x448, value);
        }
        public D3DMatrix OutsideMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x44c);
            set => Memory.WriteMemory(Address + 0x44c, value);
        }
        public D3DMatrix OutsideMatrix_ThreadFree
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x48c);
            set => Memory.WriteMemory(Address + 0x48c, value);
        }
        public float Sound_Reverb_Dist
        {
            get => Memory.ReadMemory<float>(Address + 0x4cc);
            set => Memory.WriteMemory(Address + 0x4cc, value);
        }
        public OmsiMovingMapObjInst VB_First
        {
            get => new (Memory, Memory.ReadMemory<int>(Address + 0x4d0));
        }
        public OmsiMovingMapObjInst VB_Last
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x4d4));
        }
        public OmsiMovingMapObjInst VB_Next
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x4d8));
        }
        public OmsiMovingMapObjInst VB_Prev
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x4dc));
        }
        public bool VB_Me_Reverse
        {
            get => Memory.ReadMemory<bool>(Address + 0x4e0);
            set => Memory.WriteMemory(Address + 0x4e0, value);
        }
        public bool VB_SchedUnit_Reverse
        {
            get => Memory.ReadMemory<bool>(Address + 0x4e1);
            set => Memory.WriteMemory(Address + 0x4e1, value);
        }
        public bool VB_Leading
        {
            get => Memory.ReadMemory<bool>(Address + 0x4e2);
            set => Memory.WriteMemory(Address + 0x4e2, value);
        }
        public OmsiMovingMapObjInst VB_SoundRef
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x4e4));
        }
        public bool RL_ExitReq
        {
            get => Memory.ReadMemory<bool>(Address + 0x4e8);
            set => Memory.WriteMemory(Address + 0x4e8, value);
        }
    }
}