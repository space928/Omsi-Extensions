using System;

namespace OmsiHook
{
    /// <summary>
    /// Instance of <seealso cref="OmsiRoadVehicle"/>
    /// </summary>
    public class OmsiRoadVehicleInst : OmsiVehicleInst
    {
        internal OmsiRoadVehicleInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiRoadVehicleInst() : base() { }
        /* TODO:
        public OmsiCriticalSection CS_Ticket_Items
        {
            get => Memory.ReadMemory<OmsiCriticalSection>(Address + 0x6f0);
            set => Memory.WriteMemory(Address + 0x6f0, value);
        }*/
        public OmsiRoadVehicle RoadVehicle
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x710));
        }
        public bool OnLoadedKachel
        {
            get => Memory.ReadMemory<bool>(Address + 0x714);
            set => Memory.WriteMemory(Address + 0x714, value);
        }
        public bool WasCalculated
        {
            get => Memory.ReadMemory<bool>(Address + 0x715);
            set => Memory.WriteMemory(Address + 0x715, value);
        }
        public MemArray<OmsiAchseInstance> Achsen => new(Memory, Address + 0x718);
        public byte Achsen_Angetriben
        {
            get => Memory.ReadMemory<byte>(Address + 0x71c);
            set => Memory.WriteMemory(Address + 0x71c, value);
        }
        public byte AchsOffset_Script
        {
            get => Memory.ReadMemory<byte>(Address + 0x71d);
            set => Memory.WriteMemory(Address + 0x71d, value);
        }
        public byte EntryOffset_Script
        {
            get => Memory.ReadMemory<byte>(Address + 0x71e);
            set => Memory.WriteMemory(Address + 0x71e, value);
        }
        public byte ExitOffset_Script
        {
            get => Memory.ReadMemory<byte>(Address + 0x71f);
            set => Memory.WriteMemory(Address + 0x71f, value);
        }
        public byte Offset_Gelenke
        {
            get => Memory.ReadMemory<byte>(Address + 0x720);
            set => Memory.WriteMemory(Address + 0x720, value);
        }
        public D3DVector Last_Velocity
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x721);
            set => Memory.WriteMemory(Address + 0x721, value);
        }
        public D3DVector Acc_Local
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x72d);
            set => Memory.WriteMemory(Address + 0x72d, value);
        }
        public float Inv_LenkRadius
        {
            get => Memory.ReadMemory<float>(Address + 0x73c);
            set => Memory.WriteMemory(Address + 0x73c, value);
        }
        public float FF_Zentr_Max
        {
            get => Memory.ReadMemory<float>(Address + 0x740);
            set => Memory.WriteMemory(Address + 0x740, value);
        }
        public float FF_Zentr_MaxSpeed
        {
            get => Memory.ReadMemory<float>(Address + 0x744);
            set => Memory.WriteMemory(Address + 0x744, value);
        }
        public float FF_Drag_Max
        {
            get => Memory.ReadMemory<float>(Address + 0x748);
            set => Memory.WriteMemory(Address + 0x748, value);
        }
        public float FF_Drag_MaxSpeed
        {
            get => Memory.ReadMemory<float>(Address + 0x74C);
            set => Memory.WriteMemory(Address + 0x74C, value);
        }
        public float FF_Vib_Period
        {
            get => Memory.ReadMemory<float>(Address + 0x750);
            set => Memory.WriteMemory(Address + 0x750, value);
        }
        public float FF_Vib_Amp
        {
            get => Memory.ReadMemory<float>(Address + 0x754);
            set => Memory.WriteMemory(Address + 0x754, value);
        }
        public float FF_Stoss
        {
            get => Memory.ReadMemory<float>(Address + 0x758);
            set => Memory.WriteMemory(Address + 0x758, value);
        }
        public bool PH_NeedPreCalc
        {
            get => Memory.ReadMemory<bool>(Address + 0x75c);
            set => Memory.WriteMemory(Address + 0x75c, value);
        }
        public float StreetCond
        {
            get => Memory.ReadMemory<float>(Address + 0x760);
            set => Memory.WriteMemory(Address + 0x760, value);
        }
        public float EinsatzFahrzeug
        {
            get => Memory.ReadMemory<float>(Address + 0x764);
            set => Memory.WriteMemory(Address + 0x764, value);
        }
        public float WearLifespan_Var
        {
            get => Memory.ReadMemory<float>(Address + 0x768);
            set => Memory.WriteMemory(Address + 0x768, value);
        }
        public float Brightness
        {
            get => Memory.ReadMemory<float>(Address + 0x76c);
            set => Memory.WriteMemory(Address + 0x76c, value);
        }
        public float Bew_A_Sld_Faktor
        {
            get => Memory.ReadMemory<float>(Address + 0x770);
            set => Memory.WriteMemory(Address + 0x770, value);
        }
        public float Bew_A_Sld_Faktor_2
        {
            get => Memory.ReadMemory<float>(Address + 0x774);
            set => Memory.WriteMemory(Address + 0x774, value);
        }
        public D3DVector Bew_A_Sld
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x778);
            set => Memory.WriteMemory(Address + 0x778, value);
        }
        public D3DVector Bew_A_Sld_2
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x784);
            set => Memory.WriteMemory(Address + 0x784, value);
        }
        public uint Bew_A_Event_Time
        {
            get => Memory.ReadMemory<uint>(Address + 0x790);
            set => Memory.WriteMemory(Address + 0x790, value);
        }
        public uint Bew_Ruppig_LastTime
        {
            get => Memory.ReadMemory<uint>(Address + 0x794);
            set => Memory.WriteMemory(Address + 0x794, value);
        }
        public int Bew_Ruppig_Counter
        {
            get => Memory.ReadMemory<int>(Address + 0x798);
            set => Memory.WriteMemory(Address + 0x798, value);
        }
        public bool Bew_Ruppig_Beschl
        {
            get => Memory.ReadMemory<bool>(Address + 0x79c);
            set => Memory.WriteMemory(Address + 0x79c, value);
        }
        public bool Bew_Crash
        {
            get => Memory.ReadMemory<bool>(Address + 0x79d);
            set => Memory.WriteMemory(Address + 0x79d, value);
        }
        public int MyStation
        {
            get => Memory.ReadMemory<int>(Address + 0x7a0);
            set => Memory.WriteMemory(Address + 0x7a0, value);
        }
        public MemArray<int> MyStations => new(Memory, Address + 0x7a4);
        public int Ticket_Passenger
        {
            get => Memory.ReadMemory<int>(Address + 0x7a8);
            set => Memory.WriteMemory(Address + 0x7a8, value);
        }
        public float Ticket_Give
        {
            get => Memory.ReadMemory<int>(Address + 0x7ac);
            set => Memory.WriteMemory(Address + 0x7ac, value);
        }
        public OmsiTicket[] Ticket_Items => Memory.MarshalStructs<OmsiTicket, OmsiTicketInternal>(
            Memory.ReadMemoryStructArray<OmsiTicketInternal>(Address + 0x7b0));
        public float Target_Index_Int_Var
        {
            get => Memory.ReadMemory<float>(Address + 0x7b4);
            set => Memory.WriteMemory(Address + 0x7b4, value);
        }
        public int Target_Index_Int
        {
            get => Memory.ReadMemory<int>(Address + 0x7b8);
            set => Memory.WriteMemory(Address + 0x7b8, value);
        }
        public string Target_Int_String
        {
            get => Memory.ReadMemoryString(Address + 0x7bc); // ANSI String
            set => Memory.WriteMemory(Address + 0x7bc, value);
        }
        public string Target_Int_String_PassOff
        {
            get => Memory.ReadMemoryString(Address + 0x7c0); // ANSI String
            set => Memory.WriteMemory(Address + 0x7c0, value);
        }
        public bool Betriebsfahrt
        {
            get => Memory.ReadMemory<bool>(Address + 0x7c4);
            set => Memory.WriteMemory(Address + 0x7c4, value);
        }
        public bool All_Get_Off
        {
            get => Memory.ReadMemory<bool>(Address + 0x7c5);
            set => Memory.WriteMemory(Address + 0x7c5, value);
        }
        public int MyHOF
        {
            get => Memory.ReadMemory<int>(Address + 0x7c8);
            set => Memory.WriteMemory(Address + 0x7c8, value);
        }
        public float Fuel_Percent
        {
            get => Memory.ReadMemory<float>(Address + 0x7cc);
            set => Memory.WriteMemory(Address + 0x7cc, value);
        }
        public bool OnDepot
        {
            get => Memory.ReadMemory<bool>(Address + 0x7d0);
            set => Memory.WriteMemory(Address + 0x7d0, value);
        }
        public int Last_Target_Code
        {
            get => Memory.ReadMemory<int>(Address + 0x7d4);
            set => Memory.WriteMemory(Address + 0x7d4, value);
        }
        public int Last_Route_Code
        {
            get => Memory.ReadMemory<int>(Address + 0x7d8);
            set => Memory.WriteMemory(Address + 0x7d8, value);
        }
        public int Route_Index_Result
        {
            get => Memory.ReadMemory<int>(Address + 0x7dc);
            set => Memory.WriteMemory(Address + 0x7dc, value);
        }
        public int Target_Index_Result
        {
            get => Memory.ReadMemory<int>(Address + 0x7e0);
            set => Memory.WriteMemory(Address + 0x7e0, value);
        }
        public D3DOBB AI_OBB_Large
        {
            get => Memory.ReadMemory<D3DOBB>(Address + 0x7e4);
            set => Memory.WriteMemory(Address + 0x7e4, value);
        }
        public D3DOBB AI_OBB_Small
        {
            get => Memory.ReadMemory<D3DOBB>(Address + 0x820);
            set => Memory.WriteMemory(Address + 0x820, value);
        }
        public bool PAI_Stop
        {
            get => Memory.ReadMemory<bool>(Address + 0x85c);
            set => Memory.WriteMemory(Address + 0x85c, value);
        }
        public float PAI_InvLenkR_Soll
        {
            get => Memory.ReadMemory<float>(Address + 0x860);
            set => Memory.WriteMemory(Address + 0x860, value);
        }
        public float PAI_X_Diff
        {
            get => Memory.ReadMemory<float>(Address + 0x864);
            set => Memory.WriteMemory(Address + 0x864, value);
        }
        public float PAI_MaxMue
        {
            get => Memory.ReadMemory<float>(Address + 0x868);
            set => Memory.WriteMemory(Address + 0x868, value);
        }
        public uint PAI_LastHorn
        {
            get => Memory.ReadMemory<uint>(Address + 0x86c);
            set => Memory.WriteMemory(Address + 0x86c, value);
        }
        public float PAI_WaitTime
        {
            get => Memory.ReadMemory<float>(Address + 0x870);
            set => Memory.WriteMemory(Address + 0x870, value);
        }
        public uint PAI_LastBrake
        {
            get => Memory.ReadMemory<uint>(Address + 0x874);
            set => Memory.WriteMemory(Address + 0x874, value);
        }
        public bool PAI_Approaching_Busstop
        {
            get => Memory.ReadMemory<bool>(Address + 0x878);
            set => Memory.WriteMemory(Address + 0x878, value);
        }
        public float PAI_CrashSum
        {
            get => Memory.ReadMemory<float>(Address + 0x87c);
            set => Memory.WriteMemory(Address + 0x87c, value);
        }
        public float FreeDist
        {
            get => Memory.ReadMemory<float>(Address + 0x880);
            set => Memory.WriteMemory(Address + 0x880, value);
        }
        public int KollType
        {
            get => Memory.ReadMemory<int>(Address + 0x884);
            set => Memory.WriteMemory(Address + 0x884, value);
        }
        public float CoupleForce // TODO: Check DataType
        {
            get => Memory.ReadMemory<float>(Address + 0x888);
            set => Memory.WriteMemory(Address + 0x888, value);
        }
        public int Unknown_A // TODO: Check Value
        {
            get => Memory.ReadMemory<int>(Address + 0x88c);
            set => Memory.WriteMemory(Address + 0x88c, value);
        }
        public int Unknown_B // TODO: Check Value
        {
            get => Memory.ReadMemory<int>(Address + 0x890);
            set => Memory.WriteMemory(Address + 0x890, value);
        }
        public int Unknown_C // TODO: Check Value
        {
            get => Memory.ReadMemory<int>(Address + 0x894);
            set => Memory.WriteMemory(Address + 0x894, value);
        }
        public int Unknown_D // TODO: Check Value
        {
            get => Memory.ReadMemory<int>(Address + 0x898);
            set => Memory.WriteMemory(Address + 0x898, value);
        }
        public int Unknown_E // TODO: Check Value
        {
            get => Memory.ReadMemory<int>(Address + 0x89c);
            set => Memory.WriteMemory(Address + 0x89c, value);
        }
        public float CoupleMomentTorsion // TODO: Check DataType
        {
            get => Memory.ReadMemory<float>(Address + 0x8a0);
            set => Memory.WriteMemory(Address + 0x8a0, value);
        }
        public float Unknown_F // TODO: Check Value
        {
            get => Memory.ReadMemory<float>(Address + 0x8a4);
            set => Memory.WriteMemory(Address + 0x8a4, value);
        }
        public float Couple_Alpha
        {
            get => Memory.ReadMemory<float>(Address + 0x8a8);
            set => Memory.WriteMemory(Address + 0x8a8, value);
        }
        public float Couple_Beta
        {
            get => Memory.ReadMemory<float>(Address + 0x8ac);
            set => Memory.WriteMemory(Address + 0x8ac, value);
        }
        public bool SetAlphaBeta
        {
            get => Memory.ReadMemory<bool>(Address + 0x8b0);
            set => Memory.WriteMemory(Address + 0x8b0, value);
        }
        public float VB_Var_FrontCoupling
        {
            get => Memory.ReadMemory<float>(Address + 0x8b4);
            set => Memory.WriteMemory(Address + 0x8b4, value);
        }
        public float VB_Var_BackCoupling
        {
            get => Memory.ReadMemory<float>(Address + 0x8b8);
            set => Memory.WriteMemory(Address + 0x8b8, value);
        }
        public float VB_Var_Me_Reverse
        {
            get => Memory.ReadMemory<float>(Address + 0x8bc);
            set => Memory.WriteMemory(Address + 0x8bc, value);
        }
        public OmsiRoadVehicleInst ScriptParent => new(Memory, Memory.ReadMemory<int>(Address + 0x8c0));
        public D3DXVector2 Wagenkasten_RotZPhys
        {
            get => Memory.ReadMemory<D3DXVector2>(Address + 0x8c4);
            set => Memory.WriteMemory(Address + 0x8c4, value);
        }
        public D3DXVector2 Wagenkasten_RotXPhys
        {
            get => Memory.ReadMemory<D3DXVector2>(Address + 0x8cc);
            set => Memory.WriteMemory(Address + 0x8cc, value);
        }
        public D3DXVector2 Wagenkasten_TransZPhys
        {
            get => Memory.ReadMemory<D3DXVector2>(Address + 0x8d4);
            set => Memory.WriteMemory(Address + 0x8d4, value);
        }
        public D3DVector Euler
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x8dc);
            set => Memory.WriteMemory(Address + 0x8dc, value);
        }
        public float DistToCam_Render
        {
            get => Memory.ReadMemory<float>(Address + 0x8e8);
            set => Memory.WriteMemory(Address + 0x8e8, value);
        }
        public bool Debug_B
        {
            get => Memory.ReadMemory<bool>(Address + 0x8ec);
            set => Memory.WriteMemory(Address + 0x8ec, value);
        }
    }
}