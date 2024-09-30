using System;

namespace OmsiHook
{
    /// <summary>
    /// Map object - many global settings can be found here relating to the currently loaded map.
    /// </summary>
    public class OmsiMap : OmsiObject
    {
        internal OmsiMap(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiMap() : base() { }

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

        public OmsiChrono Chrono => Memory.ReadMemoryObject<OmsiChrono>(Address, 0x28, false);

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

        public MemArray<OmsiMapSeason> Seasons
            => new(Memory, Address + 0x30, true);

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
            //TODO: set => Memory.WriteMemory(Address + 0x3c, value);
            //https://github.com/space928/Omsi-Extensions/issues/134
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

        public OmsiUnschedVehGroup[] UnschedVehGroups => Memory.MarshalStructs<OmsiUnschedVehGroup, OmsiUnschedVehGroupInternal>(
            Memory.ReadMemoryStructArray<OmsiUnschedVehGroupInternal>(Address + 0x100));

        public OmsiFuncClass Func_TrafficDensity_Passenger
        {
            get => Memory.ReadMemoryObject<OmsiFuncClass>(Address, 0x104, false);
        }

        public OmsiFuncClass Func_TrafficDensity_Road
        {
            get => Memory.ReadMemoryObject<OmsiFuncClass>(Address, 0x108, false);
        }

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
        //https://github.com/space928/Omsi-Extensions/issues/134

        public OmsiMapKachel[] Kacheln => Memory.ReadMemoryObjArray<OmsiMapKachel>(Address + 0x118);

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

        public int CenterKachelNum
        {
            get => Memory.ReadMemory<int>(Address + 0x14c);
            set => Memory.WriteMemory(Address + 0x14c, value);
        }

        public string Name
        {
            get => Memory.ReadMemoryString(Address + 0x150, true);
            set => Memory.WriteMemory(Address + 0x150, value);
        }

        public string Filename
        {
            get => Memory.ReadMemoryString(Address + 0x154);
            set => Memory.WriteMemory(Address + 0x154, value);
        }
        
        public string FriendlyName
        {
            get => Memory.ReadMemoryString(Address + 0x158, true);
            set => Memory.WriteMemory(Address + 0x158, value);
        }
        
        public string Description
        {
            get => Memory.ReadMemoryString(Address + 0x15c, true);
            set => Memory.WriteMemory(Address + 0x15c, value);
        }

        public string StandardAIGroup
        {
            get => Memory.ReadMemoryString(Address + 0x160, true);
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

        public int GridTexture
        {
            get => Memory.ReadMemory<int>(Address + 0x188);
            set => Memory.WriteMemory(Address + 0x188, value);
        }

        public float WellenAnimation
        {
            get => Memory.ReadMemory<float>(Address + 0x18c);
            set => Memory.WriteMemory(Address + 0x18c, value);
        }

        //TODO: This is meant to be an array of pointers to floats, which can't be marshalled yet
        //public float[] WellenAnimation_P => Memory.ReadMemoryStructArray<float>(Address + 0x190);
        //https://github.com/space928/Omsi-Extensions/issues/134

        public OmsiStringList Registers => Memory.ReadMemoryObject<OmsiStringList>(Address, 0x194, false);

        public OmsiStringList[] CarsParked_Array => Memory.ReadMemoryObjArray<OmsiStringList>(Address + 0x198);

        public OmsiStringList Humans => Memory.ReadMemoryObject<OmsiStringList>(Address, 0x19c, false);

        public OmsiStringList Drivers => Memory.ReadMemoryObject<OmsiStringList>(Address, 0x1a0, false);

        /*TODO: There's a bug when dereferencing this 
        public OmsiAIList AIList
        {
            get => Memory.MarshalStruct<OmsiAIList, OmsiAIListInternal>(
                Memory.ReadMemory<OmsiAIListInternal>(Address + 0x1a4));
            //TODO: set => Memory.WriteMemory(Address + 0x1a4, value);
        }*/
        //https://github.com/space928/Omsi-Extensions/issues/134

        public float MaxSpeed
        {
            get => Memory.ReadMemory<float>(Address + 0x1b0);
            set => Memory.WriteMemory(Address + 0x1b0, value);
        }

        public string Currency_Path
        {
            get => Memory.ReadMemoryString(Address + 0x1b4);
            set => Memory.WriteMemory(Address + 0x1b4, value);
        }

        public string TicketPack_Path
        {
            get => Memory.ReadMemoryString(Address + 0x1b8);
            set => Memory.WriteMemory(Address + 0x1b8, value);
        }

        public int Year_Start
        {
            get => Memory.ReadMemory<int>(Address + 0x1bc);
            set => Memory.WriteMemory(Address + 0x1bc, value);
        }

        public int Year_Ende
        {
            get => Memory.ReadMemory<int>(Address + 0x1c0);
            set => Memory.WriteMemory(Address + 0x1c0, value);
        }

        public int RealYearOffset
        {
            get => Memory.ReadMemory<int>(Address + 0x1c4);
            set => Memory.WriteMemory(Address + 0x1c4, value);
        }

        public OmsiHoliday[] Holiday => Memory.MarshalStructs<OmsiHoliday, OmsiHolidayInternal>(
            Memory.ReadMemoryStructArray<OmsiHolidayInternal>(Address + 0x1c8));

        public OmsiHolidays[] Holidays => Memory.MarshalStructs<OmsiHolidays, OmsiHolidaysInternal>(
            Memory.ReadMemoryStructArray<OmsiHolidaysInternal>(Address + 0x1cc));

        public MemArray<OmsiDST> DaylightSavingTimes => new(Memory, Address + 0x1d0, true);

        public D3DMatrix TexMatrix_Main
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x1d4);
            set => Memory.WriteMemory(Address + 0x1d4, value);
        }

        public D3DMatrix TexMatrix_LM
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x214);
            set => Memory.WriteMemory(Address + 0x214, value);
        }

        /*TODO: public OmsiCriticalSection CriticalSection_SetKachelMatrizen
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x254);
            set => Memory.WriteMemory(Address + 0x254, value);
        }*/
        //https://github.com/space928/Omsi-Extensions/issues/134
    }
}
