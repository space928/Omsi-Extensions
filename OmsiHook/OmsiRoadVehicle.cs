namespace OmsiHook
{
    public class OmsiRoadVehicle : OmsiVehicle
    {
        internal OmsiRoadVehicle(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiRoadVehicle() : base() { }
        /*public OMSIScriptVarIndizes ScriptVarIndizes
        {
            get => Memory.ReadMemory<OMSIScriptVarIndizes>(Address + 0x2b8);
            set => Memory.WriteMemory(Address + 0x2b8, value);
        }*/

        public bool Show_Dialog
        {
            get => Memory.ReadMemory<bool>(Address + 0x500);
            set => Memory.WriteMemory(Address + 0x500, value);
        }
        public OmsiAchse[] Achse
        {
            get => Memory.ReadMemoryStructArray<OmsiAchse>(Address + 0x504);
            //set => Memory.WriteMemoryStructArray<OmsiAchse>(Address + 0x504, value);
        }
        /// <summary>
        /// Pivot
        /// </summary>
        public float Drehpunk
        {
            get => Memory.ReadMemory<float>(Address + 0x508);
            set => Memory.WriteMemory(Address + 0x508, value);
        }
        public float Inv_Min_Radius
        {
            get => Memory.ReadMemory<float>(Address + 0x50c);
            set => Memory.WriteMemory(Address + 0x50c, value);
        }
        public float AI_DeltaHeight
        {
            get => Memory.ReadMemory<float>(Address + 0x510);
            set => Memory.WriteMemory(Address + 0x510, value);
        }
        public float Rowdy_Factor_Min
        {
            get => Memory.ReadMemory<float>(Address + 0x514);
            set => Memory.WriteMemory(Address + 0x514, value);
        }
        public float Rowdy_Factor_Max
        {
            get => Memory.ReadMemory<float>(Address + 0x518);
            set => Memory.WriteMemory(Address + 0x518, value);
        }
        public int Var_AI_Station_Side
        {
            get => Memory.ReadMemory<int>(Address + 0x51c);
            set => Memory.WriteMemory(Address + 0x51c, value);
        }
        public float AI_Brake_A_Std
        {
            get => Memory.ReadMemory<float>(Address + 0x520);
            set => Memory.WriteMemory(Address + 0x520, value);
        }
        public float AI_Brake_A_RowVar
        {
            get => Memory.ReadMemory<float>(Address + 0x524);
            set => Memory.WriteMemory(Address + 0x524, value);
        }
        public float AI_Brake_MueExploit
        {
            get => Memory.ReadMemory<float>(Address + 0x528);
            set => Memory.WriteMemory(Address + 0x528, value);
        }
        public float AI_Brake_Final
        {
            get => Memory.ReadMemory<float>(Address + 0x52c);
            set => Memory.WriteMemory(Address + 0x52c, value);
        }
        public float AI_Brake_StopPointOffset
        {
            get => Memory.ReadMemory<float>(Address + 0x530);
            set => Memory.WriteMemory(Address + 0x530, value);
        }
        public int VehType
        {
            get => Memory.ReadMemory<int>(Address + 0x534);
            set => Memory.WriteMemory(Address + 0x534, value);
        }
        public OmsiCoupling[] Coupling
        {
            get => Memory.ReadMemoryStructArray<OmsiCoupling>(Address + 0x538);
            //set => Memory.WriteMemory(Address + 0x538, value);
        }
        public OmsiVehicleCouple[] Couple
        {
            get => Memory.MarshalStructs<OmsiVehicleCouple, OmsiVehicleCoupleInternal>(
                Memory.ReadMemoryStructArray<OmsiVehicleCoupleInternal>(Address + 0x568));
        //set => Memory.WriteMemory(Address + 0x538, value);
        }
        public OmsiVehicleCoupleChar Couple_Char
        {
            get => Memory.ReadMemory<OmsiVehicleCoupleChar>(Address + 0x534);
            set => Memory.WriteMemory(Address + 0x534, value);
        }
        public bool Boogies_AVL
        {
            get => Memory.ReadMemory<bool>(Address + 0x588);
            set => Memory.WriteMemory(Address + 0x588, value);
        }
        public float Bookies_Y
        {
            get => Memory.ReadMemory<float>(Address + 0x58c);
            set => Memory.WriteMemory(Address + 0x58c, value);
        }
        /// <summary>
        /// ! WARN ! Data type un-known, assumed int due to size
        /// </summary>
        public int Var_Boogie_Wheel_At_Limit
        {
            get => Memory.ReadMemory<int>(Address + 0x590);
            set => Memory.WriteMemory(Address + 0x590, value);
        }
        /// <summary>
        /// ! WARN ! Unkown name!
        /// </summary>
        public int Unknown_A
        {
            get => Memory.ReadMemory<int>(Address + 0x594);
            set => Memory.WriteMemory(Address + 0x594, value);
        }
        /// <summary>
        /// ! WARN ! Data type un-known, assumed int due to size
        /// </summary>
        public int Var_Boogie_InvRadius
        {
            get => Memory.ReadMemory<int>(Address + 0x598);
            set => Memory.WriteMemory(Address + 0x598, value);
        }
        /// <summary>
        /// ! WARN ! Unkown name!
        /// </summary>
        public int Unknown_B
        {
            get => Memory.ReadMemory<int>(Address + 0x59c);
            set => Memory.WriteMemory(Address + 0x59c, value);
        }
        /// <summary>
        /// Sine Run Omega ?
        /// </summary>
        public float Sinuslauf_Omega
        {
            get => Memory.ReadMemory<float>(Address + 0x5a0);
            set => Memory.WriteMemory(Address + 0x5a0, value);
        }
        public float Sinuslauf_D
        {
            get => Memory.ReadMemory<float>(Address + 0x5a4);
            set => Memory.WriteMemory(Address + 0x5a4, value);
        }
        public float Sinuslauf_YSoll_Faktor
        {
            get => Memory.ReadMemory<float>(Address + 0x5a8);
            set => Memory.WriteMemory(Address + 0x5a8, value);
        }
        /// <summary>
        /// Body Lever Arm Z?
        /// </summary>
        public float Wagenkasten_Hebelarm_Z
        {
            get => Memory.ReadMemory<float>(Address + 0x5ac);
            set => Memory.WriteMemory(Address + 0x5ac, value);
        }
        public float Wagenkasten_rotZPhys_Omega
        {
            get => Memory.ReadMemory<float>(Address + 0x5b0);
            set => Memory.WriteMemory(Address + 0x5b0, value);
        }
        public float Wagenkasten_rotZPhys_d
        {
            get => Memory.ReadMemory<float>(Address + 0x5b4);
            set => Memory.WriteMemory(Address + 0x5b4, value);
        }
        public float Wagenkasten_rotXPhys_Omega
        {
            get => Memory.ReadMemory<float>(Address + 0x5b8);
            set => Memory.WriteMemory(Address + 0x5b8, value);
        }
        public float Wagenkasten_rotXPhys_d
        {
            get => Memory.ReadMemory<float>(Address + 0x5bc);
            set => Memory.WriteMemory(Address + 0x5bc, value);
        }
        public float Wagenkasten_TransZPhys_Omega
        {
            get => Memory.ReadMemory<float>(Address + 0x5c0);
            set => Memory.WriteMemory(Address + 0x5c0, value);
        }
        public float Wagenkasten_TransZPhys_d
        {
            get => Memory.ReadMemory<float>(Address + 0x5c4);
            set => Memory.WriteMemory(Address + 0x5c4, value);
        }
        public OmsiContactShoe[] ContactShoes
        {
            get => Memory.ReadMemoryStructArray<OmsiContactShoe>(Address + 0x5c8);
            //set => Memory.ReadMemoryStructArray<OmsiContactShoe>(Address + 0x5c8);
        }
    }
}