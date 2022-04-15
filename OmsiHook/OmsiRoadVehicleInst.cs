namespace OmsiHook
{
    public class OmsiRoadVehicleInst : OmsiVehicleInst
    {
        internal OmsiRoadVehicleInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        /* TODO:
        public OmsiCriticalSection CS_Ticket_Items
        {
            get => omsiMemory.ReadMemory<OmsiCriticalSection>(baseAddress + 0x6f0);
            set => omsiMemory.WriteMemory(baseAddress + 0x6f0, value);
        }*/
        /* TODO:
        public OmsiRoadVehiclePtr RoadVehicle
        {
            get => omsiMemory.ReadMemory<OmsiRoadVehiclePtr>(baseAddress + 0x710);
            set => omsiMemory.WriteMemory(baseAddress + 0x710, value);
        }*/
        public bool OnLoadedKachel
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x714);
            set => omsiMemory.WriteMemory(baseAddress + 0x714, value);
        }
        public bool WasCalculated
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x715);
            set => omsiMemory.WriteMemory(baseAddress + 0x715, value);
        }
        /* TODO:
        public OmsiAchseInstance[] Achsen
        {
            get => omsiMemory.ReadMemory<OmsiAchseInstance[]>(baseAddress + 0x718);
            set => omsiMemory.WriteMemory(baseAddress + 0x718, value);
        }*/
        public byte Achsen_Angetriben
        {
            get => omsiMemory.ReadMemory<byte>(baseAddress + 0x71c);
            set => omsiMemory.WriteMemory(baseAddress + 0x71c, value);
        }
        public byte AchsOffset_Script
        {
            get => omsiMemory.ReadMemory<byte>(baseAddress + 0x71d);
            set => omsiMemory.WriteMemory(baseAddress + 0x71d, value);
        }
        public byte EntryOffset_Script
        {
            get => omsiMemory.ReadMemory<byte>(baseAddress + 0x71e);
            set => omsiMemory.WriteMemory(baseAddress + 0x71e, value);
        }
        public byte ExitOffset_Script
        {
            get => omsiMemory.ReadMemory<byte>(baseAddress + 0x71f);
            set => omsiMemory.WriteMemory(baseAddress + 0x71f, value);
        }
        public byte Offset_Gelenke
        {
            get => omsiMemory.ReadMemory<byte>(baseAddress + 0x720);
            set => omsiMemory.WriteMemory(baseAddress + 0x720, value);
        }
        public D3DVector Last_Velocity
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x721);
            set => omsiMemory.WriteMemory(baseAddress + 0x721, value);
        }
        public D3DVector Acc_Local
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x72d);
            set => omsiMemory.WriteMemory(baseAddress + 0x72d, value);
        }
        public float Inv_LenkRadius
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x73c);
            set => omsiMemory.WriteMemory(baseAddress + 0x73c, value);
        }
        public float FF_Zentr_Max
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x740);
            set => omsiMemory.WriteMemory(baseAddress + 0x740, value);
        }
        public float FF_Zentr_MaxSpeed
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x744);
            set => omsiMemory.WriteMemory(baseAddress + 0x744, value);
        }
        public float FF_Drag_Max
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x748);
            set => omsiMemory.WriteMemory(baseAddress + 0x748, value);
        }
        public float FF_Drag_MaxSpeed
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x74C);
            set => omsiMemory.WriteMemory(baseAddress + 0x74C, value);
        }
        public float FF_Vib_Period
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x750);
            set => omsiMemory.WriteMemory(baseAddress + 0x750, value);
        }
        public float FF_Vib_Amp
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x754);
            set => omsiMemory.WriteMemory(baseAddress + 0x754, value);
        }
        public float FF_Stoss
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x758);
            set => omsiMemory.WriteMemory(baseAddress + 0x758, value);
        }
        public bool PH_NeedPreCalc
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x75c);
            set => omsiMemory.WriteMemory(baseAddress + 0x75c, value);
        }
        public float StreetCond
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x760);
            set => omsiMemory.WriteMemory(baseAddress + 0x760, value);
        }
        public float EinsatzFahrzeug
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x764);
            set => omsiMemory.WriteMemory(baseAddress + 0x764, value);
        }
        public float WearLifespan_Var
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x768);
            set => omsiMemory.WriteMemory(baseAddress + 0x768, value);
        }
        public float Brightness
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x76c);
            set => omsiMemory.WriteMemory(baseAddress + 0x76c, value);
        }
        public float Bew_A_Sld_Faktor
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x770);
            set => omsiMemory.WriteMemory(baseAddress + 0x770, value);
        }
        public float Bew_A_Sld_Faktor_2
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x774);
            set => omsiMemory.WriteMemory(baseAddress + 0x774, value);
        }
        public D3DVector Bew_A_Sld
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x778);
            set => omsiMemory.WriteMemory(baseAddress + 0x778, value);
        }
        public D3DVector Bew_A_Sld_2
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x784);
            set => omsiMemory.WriteMemory(baseAddress + 0x784, value);
        }
        public uint Bew_A_Event_Time
        {
            get => omsiMemory.ReadMemory<uint>(baseAddress + 0x790);
            set => omsiMemory.WriteMemory(baseAddress + 0x790, value);
        }
        public uint Bew_Ruppig_LastTime
        {
            get => omsiMemory.ReadMemory<uint>(baseAddress + 0x794);
            set => omsiMemory.WriteMemory(baseAddress + 0x794, value);
        }
        public int Bew_Ruppig_Counter
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x798);
            set => omsiMemory.WriteMemory(baseAddress + 0x798, value);
        }
        public bool Bew_Ruppig_Beschl
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x79c);
            set => omsiMemory.WriteMemory(baseAddress + 0x79c, value);
        }
        public bool Bew_Crash
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x79d);
            set => omsiMemory.WriteMemory(baseAddress + 0x79d, value);
        }
        public int MyStation
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x7a0);
            set => omsiMemory.WriteMemory(baseAddress + 0x7a0, value);
        }
        /* TODO:
        public int[] MyStations
        {
            get => omsiMemory.ReadMemory<int[]>(baseAddress + 0x7a4);
            set => omsiMemory.WriteMemory(baseAddress + 0x7a4, value);
        }*/
        public int Ticket_Passenger
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x7a8);
            set => omsiMemory.WriteMemory(baseAddress + 0x7a8, value);
        }
        public float Ticket_Give
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x7ac);
            set => omsiMemory.WriteMemory(baseAddress + 0x7ac, value);
        }
        /* TODO:
        public Ticket[] Ticket_Items
        {
            get => omsiMemory.ReadMemory<Ticket[]>(baseAddress + 0x7b0);
            set => omsiMemory.WriteMemory(baseAddress + 0x7b0, value);
        }*/
        public float Target_Index_Int_Var
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x7b4);
            set => omsiMemory.WriteMemory(baseAddress + 0x7b4, value);
        }
        public int Target_Index_Int
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x7b8);
            set => omsiMemory.WriteMemory(baseAddress + 0x7b8, value);
        }
        public string Target_Int_String
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x7bc); // ANSI String
            set => omsiMemory.WriteMemory(baseAddress + 0x7bc, value);
        }
        public string Target_Int_String_PassOff
        {
            get => omsiMemory.ReadMemoryString(baseAddress + 0x7c0); // ANSI String
            set => omsiMemory.WriteMemory(baseAddress + 0x7c0, value);
        }
        public bool Betriebsfahrt
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x7c4);
            set => omsiMemory.WriteMemory(baseAddress + 0x7c4, value);
        }
        public bool All_Get_Off
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x7c5);
            set => omsiMemory.WriteMemory(baseAddress + 0x7c5, value);
        }
        public int MyHOF
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x7c8);
            set => omsiMemory.WriteMemory(baseAddress + 0x7c8, value);
        }
        public float Fuel_Percent
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x7cc);
            set => omsiMemory.WriteMemory(baseAddress + 0x7cc, value);
        }
        public bool OnDepot
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x7d0);
            set => omsiMemory.WriteMemory(baseAddress + 0x7d0, value);
        }
        public int Last_Target_Code
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x7d4);
            set => omsiMemory.WriteMemory(baseAddress + 0x7d4, value);
        }
        public int Last_Route_Code
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x7d8);
            set => omsiMemory.WriteMemory(baseAddress + 0x7d8, value);
        }
        public int Route_Index_Result
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x7dc);
            set => omsiMemory.WriteMemory(baseAddress + 0x7dc, value);
        }
        public int Target_Index_Result
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x7e0);
            set => omsiMemory.WriteMemory(baseAddress + 0x7e0, value);
        }
        public D3DOBB AI_OBB_Large
        {
            get => omsiMemory.ReadMemory<D3DOBB>(baseAddress + 0x7e4);
            set => omsiMemory.WriteMemory(baseAddress + 0x7e4, value);
        }
        public D3DOBB AI_OBB_Small
        {
            get => omsiMemory.ReadMemory<D3DOBB>(baseAddress + 0x820);
            set => omsiMemory.WriteMemory(baseAddress + 0x820, value);
        }
        public bool PAI_Stop
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x85c);
            set => omsiMemory.WriteMemory(baseAddress + 0x85c, value);
        }
        public float PAI_InvLenkR_Soll
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x860);
            set => omsiMemory.WriteMemory(baseAddress + 0x860, value);
        }
        public float PAI_X_Diff
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x864);
            set => omsiMemory.WriteMemory(baseAddress + 0x864, value);
        }
        public float PAI_MaxMue
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x868);
            set => omsiMemory.WriteMemory(baseAddress + 0x868, value);
        }
        public uint PAI_LastHorn
        {
            get => omsiMemory.ReadMemory<uint>(baseAddress + 0x868);
            set => omsiMemory.WriteMemory(baseAddress + 0x868, value);
        }
    }
}