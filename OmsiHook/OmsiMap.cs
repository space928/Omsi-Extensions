using System;

namespace OmsiHook
{
    public class OmsiMap : OmsiObject
    {
        internal OmsiMap(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }

        public OmsiPoint NW_Corner
        {
            get => omsiMemory.ReadMemory<OmsiPoint>(baseAddress + 0x4);
            set => omsiMemory.WriteMemory(baseAddress + 0x4, value);
        }

        public OmsiPoint SE_Corner
        {
            get => omsiMemory.ReadMemory<OmsiPoint>(baseAddress + 0xc);
            set => omsiMemory.WriteMemory(baseAddress + 0xc, value);
        }

        public bool LoadAllKacheln
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x14);
            set => omsiMemory.WriteMemory(baseAddress + 0x14, value);
        }

        public bool NextTime_CheckKachelUnloading
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x15);
            set => omsiMemory.WriteMemory(baseAddress + 0x15, value);
        }

        public uint DynTile_RedTimer
        {
            get => omsiMemory.ReadMemory<uint>(baseAddress + 0x18);
            set => omsiMemory.WriteMemory(baseAddress + 0x18, value);
        }

        public byte FDynTileIst
        {
            get => omsiMemory.ReadMemory<byte>(baseAddress + 0x1c);
            set => omsiMemory.WriteMemory(baseAddress + 0x1c, value);
        }

        public bool NotUnloaded
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x1d);
            set => omsiMemory.WriteMemory(baseAddress + 0x1d, value);
        }

        public int Version
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x20);
            set => omsiMemory.WriteMemory(baseAddress + 0x20, value);
        }

        public bool Ignore_TruckFlags
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x24);
            set => omsiMemory.WriteMemory(baseAddress + 0x24, value);
        }

        public OmsiChrono Chrono => new(omsiMemory, omsiMemory.ReadMemory<int>(baseAddress + 0x28));

        public bool Splines_Refreshed
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x2c);
            set => omsiMemory.WriteMemory(baseAddress + 0x2c, value);
        }

        public bool NewNearKachelnExist
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x2d);
            set => omsiMemory.WriteMemory(baseAddress + 0x2d, value);
        }

        public OmsiMapSeason[] Seasons
            => omsiMemory.ReadMemoryStructArray<OmsiMapSeason>(baseAddress + 0x30);

        public bool RealSwitches
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x34);
            set => omsiMemory.WriteMemory(baseAddress + 0x34, value);
        }

        public bool LHT
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x35);
            set => omsiMemory.WriteMemory(baseAddress + 0x35, value);
        }

        public OmsiGroundType[] GroundTypes
            => omsiMemory.ReadMemoryStructArray<OmsiGroundType>(baseAddress + 0x38);

        public OmsiMaterialProp Water_Matl
        {
            get => omsiMemory.ReadMemory<OmsiMaterialProp>(baseAddress + 0x3c);
            set => omsiMemory.WriteMemory(baseAddress + 0x3c, value);
        }

        public int LoadedKacheln
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0xf8);
            set => omsiMemory.WriteMemory(baseAddress + 0xf8, value);
        }

        public bool WCoord
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0xfc);
            set => omsiMemory.WriteMemory(baseAddress + 0xfc, value);
        }

        public bool DynHelperActive
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0xfd);
            set => omsiMemory.WriteMemory(baseAddress + 0xfd, value);
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
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x10c);
            set => omsiMemory.WriteMemory(baseAddress + 0x10c, value);
        }

        public float Acct_TrafficDensity_Road
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x110);
            set => omsiMemory.WriteMemory(baseAddress + 0x110, value);
        }

        /*TODO: public OmsiThreadTileLoadAndRefresh ThreadTileLoadAndRefresh 
            => omsiMemory.ReadMemory<OmsiThreadTileLoadAndRefresh>(baseAddress + 0x114);*/

        public OmsiKacheln[] Kacheln => omsiMemory.ReadMemoryObjArray<OmsiKacheln>(baseAddress + 0x118);

        public OmsiMapKachelInfo[] KachelInfos 
            => omsiMemory.ReadMemoryStructArray<OmsiMapKachelInfo>(baseAddress + 0x11c);

        public bool Loaded
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x120);
            set => omsiMemory.WriteMemory(baseAddress + 0x120, value);
        }

        public OmsiStandardMapCam StandardMapCam
        {
            get => omsiMemory.ReadMemory<OmsiStandardMapCam>(baseAddress + 0x124);
            set => omsiMemory.WriteMemory(baseAddress + 0x124, value);
        }

        public OmsiPoint CenterKachel
        {
            get => omsiMemory.ReadMemory<OmsiPoint>(baseAddress + 0x144);
            set => omsiMemory.WriteMemory(baseAddress + 0x144, value);
        }

        public OmsiPoint CenterKachelNum
        {
            get => omsiMemory.ReadMemory<OmsiPoint>(baseAddress + 0x14c);
            set => omsiMemory.WriteMemory(baseAddress + 0x14c, value);
        }

        public string Name
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x150);
            set => omsiMemory.WriteMemory(baseAddress + 0x150, value);
        }

        public string Filename
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x154);
            set => omsiMemory.WriteMemory(baseAddress + 0x154, value);
        }
        
        public string FriendlyName
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x158);
            set => omsiMemory.WriteMemory(baseAddress + 0x158, value);
        }
        
        public string Description
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x15c);
            set => omsiMemory.WriteMemory(baseAddress + 0x15c, value);
        }

        public string StandardAIGroup
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x160);
            set => omsiMemory.WriteMemory(baseAddress + 0x160, value);
        }

        public uint NextFileObjectCode
        {
            get => omsiMemory.ReadMemory<uint>(baseAddress + 0x164);
            set => omsiMemory.WriteMemory(baseAddress + 0x164, value);
        }

        public int BG_Picture
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x168);
            set => omsiMemory.WriteMemory(baseAddress + 0x168, value);
        }

        public string BG_Picture_Filename
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x16c);
            set => omsiMemory.WriteMemory(baseAddress + 0x16c, value);
        }

        public bool BG_Picture_Visible
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x170);
            set => omsiMemory.WriteMemory(baseAddress + 0x170, value);
        }

        public float BG_Picture_Faktor_X
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x174);
            set => omsiMemory.WriteMemory(baseAddress + 0x174, value);
        }

        public float BG_Picture_Faktor_Y
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x178);
            set => omsiMemory.WriteMemory(baseAddress + 0x178, value);
        }

        public float BG_Picture_Midpoint_X
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x17c);
            set => omsiMemory.WriteMemory(baseAddress + 0x17c, value);
        }

        public float BG_Picture_Midpoint_Y
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x180);
            set => omsiMemory.WriteMemory(baseAddress + 0x180, value);
        }

        public bool Tile_Aerial_Visible
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x184);
            set => omsiMemory.WriteMemory(baseAddress + 0x184, value);
        }

        //TODO: Many more...
    }
}
