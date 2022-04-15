using System;

namespace OmsiHook
{
    public class OmsiMap : OmsiObject
    {
        internal OmsiMap(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiMap() : base() { }

        public OmsiPoint NW_Corner
        {
            get => Memory.ReadMemory<OmsiPoint>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }

        public OmsiPoint SE_Corner
        {
            get => Memory.ReadMemory<OmsiPoint>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }

        public bool LoadAllKacheln
        {
            get => Memory.ReadMemory<bool>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }

        public bool NextTime_CheckKachelUnloading
        {
            get => Memory.ReadMemory<bool>(Address + 0x15);
            set => Memory.WriteMemory(Address + 0x15, value);
        }

        public uint DynTile_RedTimer
        {
            get => Memory.ReadMemory<uint>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }

        public byte FDynTileIst
        {
            get => Memory.ReadMemory<byte>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }

        public bool NotUnloaded
        {
            get => Memory.ReadMemory<bool>(Address + 0x1d);
            set => Memory.WriteMemory(Address + 0x1d, value);
        }

        public int Version
        {
            get => Memory.ReadMemory<int>(Address + 0x20);
            set => Memory.WriteMemory(Address + 0x20, value);
        }

        public bool Ignore_TruckFlags
        {
            get => Memory.ReadMemory<bool>(Address + 0x24);
            set => Memory.WriteMemory(Address + 0x24, value);
        }

        public OmsiChrono Chrono => new(Memory, Memory.ReadMemory<int>(Address + 0x28));

        public bool Splines_Refreshed
        {
            get => Memory.ReadMemory<bool>(Address + 0x2c);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }

        public bool NewNearKachelnExist
        {
            get => Memory.ReadMemory<bool>(Address + 0x2d);
            set => Memory.WriteMemory(Address + 0x2d, value);
        }

        public OmsiMapSeason[] Seasons
            => Memory.ReadMemoryStructArray<OmsiMapSeason>(Address + 0x30);

        public bool RealSwitches
        {
            get => Memory.ReadMemory<bool>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }

        public bool LHT
        {
            get => Memory.ReadMemory<bool>(Address + 0x35);
            set => Memory.WriteMemory(Address + 0x35, value);
        }

        public OmsiGroundType[] GroundTypes
            => Memory.MarshalStructs<OmsiGroundType, OmsiGroundTypeInternal>(
                Memory.ReadMemoryStructArray<OmsiGroundTypeInternal>(Address + 0x38));

        public OmsiMaterialProp Water_Matl
        {
            get => Memory.MarshalStruct<OmsiMaterialProp, OmsiMaterialPropInternal>(
                Memory.ReadMemory<OmsiMaterialPropInternal>(Address + 0x3c));
            set => Memory.WriteMemory(Address + 0x3c, value);
        }

        public int LoadedKacheln
        {
            get => Memory.ReadMemory<int>(Address + 0xf8);
            set => Memory.WriteMemory(Address + 0xf8, value);
        }

        public bool WCoord
        {
            get => Memory.ReadMemory<bool>(Address + 0xfc);
            set => Memory.WriteMemory(Address + 0xfc, value);
        }

        public bool DynHelperActive
        {
            get => Memory.ReadMemory<bool>(Address + 0xfd);
            set => Memory.WriteMemory(Address + 0xfd, value);
        }

        /*
        TODO:public OmsiUnsechdVehGroup[] UnschedVehGroups
        {
            get => omsiMemory.ReadMemory<OmsiUnsechdVehGroup[]>(baseAddress + 0x100);
            set => omsiMemory.WriteMemory(baseAddress + 0x100, value);
        }*/

        /*public void Func_TrafficDensity_Passenger //TFuncClass
        {
            get => omsiMemory.ReadMemory<void>(baseAddress + 0x104);
            set => omsiMemory.WriteMemory(baseAddress + 0x104, value);
        }*/

        /*public void Func_TrafficDensity_Road //TFuncClass
        {
            get => omsiMemory.ReadMemory<void>(baseAddress + 0x108);
            set => omsiMemory.WriteMemory(baseAddress + 0x108, value);
        }*/

        public float Acct_TrafficDensity_Passenger
        {
            get => Memory.ReadMemory<float>(Address + 0x10c);
            set => Memory.WriteMemory(Address + 0x10c, value);
        }

        public float Acct_TrafficDensity_Road
        {
            get => Memory.ReadMemory<float>(Address + 0x110);
            set => Memory.WriteMemory(Address + 0x110, value);
        }

        /*TODO: public OmsiThreadTileLoadAndRefresh ThreadTileLoadAndRefresh 
            => omsiMemory.ReadMemory<OmsiThreadTileLoadAndRefresh>(baseAddress + 0x114);*/

        public OmsiKacheln[] Kacheln => Memory.ReadMemoryObjArray<OmsiKacheln>(Address + 0x118);

        public OmsiMapKachelInfo[] KachelInfos 
            => Memory.MarshalStructs<OmsiMapKachelInfo, OmsiMapKachelInfoInternal>(
                Memory.ReadMemoryStructArray<OmsiMapKachelInfoInternal>(Address + 0x11c));

        public bool Loaded
        {
            get => Memory.ReadMemory<bool>(Address + 0x120);
            set => Memory.WriteMemory(Address + 0x120, value);
        }

        public OmsiStandardMapCam StandardMapCam
        {
            get => Memory.ReadMemory<OmsiStandardMapCam>(Address + 0x124);
            set => Memory.WriteMemory(Address + 0x124, value);
        }

        public OmsiPoint CenterKachel
        {
            get => Memory.ReadMemory<OmsiPoint>(Address + 0x144);
            set => Memory.WriteMemory(Address + 0x144, value);
        }

        public OmsiPoint CenterKachelNum
        {
            get => Memory.ReadMemory<OmsiPoint>(Address + 0x14c);
            set => Memory.WriteMemory(Address + 0x14c, value);
        }

        public string Name
        {
            get => Memory.ReadMemoryString(Memory.ReadMemory<int>(Address + 0x150), true);
            set => Memory.WriteMemory(Address + 0x150, value);
        }

        public string Filename
        {
            get => Memory.ReadMemoryString(Memory.ReadMemory<int>(Address + 0x154));
            set => Memory.WriteMemory(Address + 0x154, value);
        }
        
        public string FriendlyName
        {
            get => Memory.ReadMemoryString(Memory.ReadMemory<int>(Address + 0x158), true);
            set => Memory.WriteMemory(Address + 0x158, value);
        }
        
        public string Description
        {
            get => Memory.ReadMemoryString(Memory.ReadMemory<int>(Address + 0x15c), true);
            set => Memory.WriteMemory(Address + 0x15c, value);
        }

        public string StandardAIGroup
        {
            get => Memory.ReadMemoryString(Address + 0x160);
            set => Memory.WriteMemory(Address + 0x160, value);
        }

        public uint NextFileObjectCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x164);
            set => Memory.WriteMemory(Address + 0x164, value);
        }

        public int BG_Picture
        {
            get => Memory.ReadMemory<int>(Address + 0x168);
            set => Memory.WriteMemory(Address + 0x168, value);
        }

        public string BG_Picture_Filename
        {
            get => Memory.ReadMemoryString(Address + 0x16c);
            set => Memory.WriteMemory(Address + 0x16c, value);
        }

        public bool BG_Picture_Visible
        {
            get => Memory.ReadMemory<bool>(Address + 0x170);
            set => Memory.WriteMemory(Address + 0x170, value);
        }

        public float BG_Picture_Faktor_X
        {
            get => Memory.ReadMemory<float>(Address + 0x174);
            set => Memory.WriteMemory(Address + 0x174, value);
        }

        public float BG_Picture_Faktor_Y
        {
            get => Memory.ReadMemory<float>(Address + 0x178);
            set => Memory.WriteMemory(Address + 0x178, value);
        }

        public float BG_Picture_Midpoint_X
        {
            get => Memory.ReadMemory<float>(Address + 0x17c);
            set => Memory.WriteMemory(Address + 0x17c, value);
        }

        public float BG_Picture_Midpoint_Y
        {
            get => Memory.ReadMemory<float>(Address + 0x180);
            set => Memory.WriteMemory(Address + 0x180, value);
        }

        public bool Tile_Aerial_Visible
        {
            get => Memory.ReadMemory<bool>(Address + 0x184);
            set => Memory.WriteMemory(Address + 0x184, value);
        }

        //TODO: Many more...
    }
}
