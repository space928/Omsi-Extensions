namespace OmsiHook
{
    /// <summary>
    /// A Tile on a Map
    /// </summary>
    public class OmsiMapKachel : OmsiObject
    {
        internal OmsiMapKachel(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiMapKachel() : base() { }

        /*TODO: public OmsiCSChecker FCSCNearObjects
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x4));
        }
        public OmsiCriticalSection FCSPathGroups
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x8));
        }
        public OmsiCriticalSection CS_NearObjectsSplines
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0xc));
        }
         */
        public string Filename
        {
            get => Memory.ReadMemoryString(Address + 0x2c);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }
        public int Version
        {
            get => Memory.ReadMemory<int>(Address + 0x30);
            set => Memory.WriteMemory(Address + 0x30, value);
        }
        public bool Unsaved
        {
            get => Memory.ReadMemory<bool>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }
        public bool Unsaved_Terrain
        {
            get => Memory.ReadMemory<bool>(Address + 0x35);
            set => Memory.WriteMemory(Address + 0x35, value);
        }
        public bool Save_add
        {
            get => Memory.ReadMemory<bool>(Address + 0x36);
            set => Memory.WriteMemory(Address + 0x36, value);
        }
        public bool Save_Add_Terrain
        {
            get => Memory.ReadMemory<bool>(Address + 0x36);
            set => Memory.WriteMemory(Address + 0x36, value);
        }
        public bool Loaded
        {
            get => Memory.ReadMemory<bool>(Address + 0x38);
            set => Memory.WriteMemory(Address + 0x38, value);
        }
        public bool Load_Request
        {
            get => Memory.ReadMemory<bool>(Address + 0x39);
            set => Memory.WriteMemory(Address + 0x39, value);
        }
        public bool Failed
        {
            get => Memory.ReadMemory<bool>(Address + 0x3a);
            set => Memory.WriteMemory(Address + 0x3a, value);
        }
        public bool ThreadLoading
        {
            get => Memory.ReadMemory<bool>(Address + 0x3b);
            set => Memory.WriteMemory(Address + 0x3b, value);
        }
        public bool ThreadLoading_Real
        {
            get => Memory.ReadMemory<bool>(Address + 0x3c);
            set => Memory.WriteMemory(Address + 0x3c, value);
        }
        public bool ThreadLoading_Render
        {
            get => Memory.ReadMemory<bool>(Address + 0x3d);
            set => Memory.WriteMemory(Address + 0x3d, value);
        }
        public bool Prepared_For_Ode
        {
            get => Memory.ReadMemory<bool>(Address + 0x3e);
            set => Memory.WriteMemory(Address + 0x3e, value);
        }
        public float Width_S
        {
            get => Memory.ReadMemory<float>(Address + 0x40);
            set => Memory.WriteMemory(Address + 0x40, value);
        }
        public float Width_N
        {
            get => Memory.ReadMemory<float>(Address + 0x44);
            set => Memory.WriteMemory(Address + 0x44, value);
        }
        public OmsiFileObject[] ObjectsFromFile => 
            Memory.ReadMemoryObjArray<OmsiFileObject>(Address + 0x48);
        public OmsiFileTerrain TerrainFromFile => new(Memory, Memory.ReadMemory<int>(Address + 0x4c));
        public OmsiFileWater WaterFromFile => new(Memory, Memory.ReadMemory<int>(Address + 0x50));
        public OmsiFileSpline[] SplinesFromFile =>
            Memory.ReadMemoryObjArray<OmsiFileSpline>(Address + 0x54);
        public bool Splines_Refreshed
        {
            get => Memory.ReadMemory<bool>(Address + 0x58);
            set => Memory.WriteMemory(Address + 0x58, value);
        }
        public bool Objects_Refreshed
        {
            get => Memory.ReadMemory<bool>(Address + 0x59);
            set => Memory.WriteMemory(Address + 0x59, value);
        }
        public bool Paths_Refreshed
        {
            get => Memory.ReadMemory<bool>(Address + 0x5a);
            set => Memory.WriteMemory(Address + 0x5a, value);
        }
        public OmsiComplMapObjInst[] Objects => 
            Memory.ReadMemoryObjArray<OmsiComplMapObjInst>(Address + 0x5c);
        public OmsiPathGroup[] PathGroups => 
            Memory.ReadMemoryObjArray<OmsiPathGroup>(Address + 0x60);
        public OmsiPathSegment[] PathSegments =>
            Memory.ReadMemoryObjArray<OmsiPathSegment>(Address + 0x64);
        public bool PathsLinkedWithFstrn
        {
            get => Memory.ReadMemory<bool>(Address + 0x68);
            set => Memory.WriteMemory(Address + 0x68, value);
        }
        public OmsiAmpelGroup[] AmpelGroups => Memory.MarshalStructs<OmsiAmpelGroup, OmsiAmpelGroupInternal>(
            Memory.ReadMemoryStructArray<OmsiAmpelGroupInternal>(Address + 0x6c));
        public D3DMeshFileObject TileMesh => new(Memory, Memory.ReadMemory<int>(Address + 0x70));
        public string TexPath
        {
            get => Memory.ReadMemoryString(Address + 0x74);
            set => Memory.WriteMemory(Address + 0x74, value);
        }
        public OmsiComplMapSceneObjInst[] Objects_Surface =>
            Memory.ReadMemoryObjArray<OmsiComplMapSceneObjInst>(Address + 0x78);
        public OmsiSplineSegment[] SplineSegments =>
            Memory.ReadMemoryObjArray<OmsiSplineSegment>(Address + 0x7c);
        public bool Trees_Refreshed
        {
            get => Memory.ReadMemory<bool>(Address + 0x80);
            set => Memory.WriteMemory(Address + 0x80, value);
        }
        public OmsiKachelForest MyForest => new(Memory, Memory.ReadMemory<int>(Address + 0x84));
        public OmsiKachelForest MyScrubs => new(Memory, Memory.ReadMemory<int>(Address + 0x88));
        //TODO: Many more...
    }
}