namespace OmsiHook
{
    /// <summary>
    /// Complex scenery object instace data - relating to nightlights, active use...
    /// </summary>
    /// TComplMapScenObjInst
    public class OmsiComplMapSceneObjInst : OmsiComplMapObjInst
    {
        public OmsiComplMapSceneObjInst() : base() { }

        internal OmsiComplMapSceneObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }


        public float NightLightA
        {
            get => Memory.ReadMemory<float>(Address + 0x258);
            set => Memory.WriteMemory(Address + 0x258, value);
        }
        public float InUse
        {
            get => Memory.ReadMemory<float>(Address + 0x25c);
            set => Memory.WriteMemory(Address + 0x25c, value);
        }
        public float SwitchPos
        {
            get => Memory.ReadMemory<float>(Address + 0x260);
            set => Memory.WriteMemory(Address + 0x260, value);
        }
        public float NightLightStart
        {
            get => Memory.ReadMemory<float>(Address + 0x264);
            set => Memory.WriteMemory(Address + 0x264, value);
        }
        public float NightLightEnd
        {
            get => Memory.ReadMemory<float>(Address + 0x268);
            set => Memory.WriteMemory(Address + 0x268, value);
        }
        public float NightLightBrightnessLimit
        {
            get => Memory.ReadMemory<float>(Address + 0x26c);
            set => Memory.WriteMemory(Address + 0x26c, value);
        }
        public bool AlignTerrain
        {
            get => Memory.ReadMemory<bool>(Address + 0x270);
            set => Memory.WriteMemory(Address + 0x270, value);
        }
        public int Ampel
        {
            get => Memory.ReadMemory<int>(Address + 0x274);
            set => Memory.WriteMemory(Address + 0x274, value);
        }
    }
}