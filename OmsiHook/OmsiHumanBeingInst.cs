using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Instance of a <seealso cref="OmsiHumanBeing">Human Being</seealso>
    /// </summary>
    public class OmsiHumanBeingInst : OmsiMovingMapObjInst
    {
        internal OmsiHumanBeingInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiHumanBeingInst() : base() { }

        public D3DMatrix Human_Main_Matrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x4f0);
            set => Memory.WriteMemory(Address + 0x4f0, value);
        }
        /* Unknown data type - Trad: Left is?
        public ? LinksIst
        {
            get => Memory.ReadMemory<?>(Address + 0x530);
            set => Memory.WriteMemory(Address + 0x530, value);
        }*/
        public bool Tutorial_Created
        {
            get => Memory.ReadMemory<bool>(Address + 0x5a8);
            set => Memory.WriteMemory(Address + 0x5a8, value);
        }
        public int Index
        {
            get => Memory.ReadMemory<int>(Address + 0x5ac);
            set => Memory.WriteMemory(Address + 0x5ac, value);
        }
        public OmsiHumanBeing Human
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x5b0));
        }
        public float FreeDist
        {
            get => Memory.ReadMemory<float>(Address + 0x5b4);
            set => Memory.WriteMemory(Address + 0x5b4, value);
        }
        public int KollType
        {
            get => Memory.ReadMemory<int>(Address + 0x5b8);
            set => Memory.WriteMemory(Address + 0x5b8, value);
        }
        public bool RenderMe
        {
            get => Memory.ReadMemory<bool>(Address + 0x5bc);
            set => Memory.WriteMemory(Address + 0x5bc, value);
        }
        public D3DVector Target
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x5bd);
            set => Memory.WriteMemory(Address + 0x5bd, value);
        }
        public float Target_x_min
        {
            get => Memory.ReadMemory<float>(Address + 0x5cc);
            set => Memory.WriteMemory(Address + 0x5cc, value);
        }
        public bool Target_x_min_Active
        {
            get => Memory.ReadMemory<bool>(Address + 0x5d0);
            set => Memory.WriteMemory(Address + 0x5d0, value);
        }
        public bool Target_x_min_On_Z
        {
            get => Memory.ReadMemory<bool>(Address + 0x5d1);
            set => Memory.WriteMemory(Address + 0x5d1, value);
        }
        public bool Target_x_min_Inv
        {
            get => Memory.ReadMemory<bool>(Address + 0x5d2);
            set => Memory.WriteMemory(Address + 0x5d2, value);
        }
        public float TargetHDG
        {
            get => Memory.ReadMemory<float>(Address + 0x5d4);
            set => Memory.WriteMemory(Address + 0x5d4, value);
        }
        public D3DMatrix TargetMat
        {
            get => Memory.ReadMemory<D3DMatrix>(Memory.ReadMemory<int>(Address + 0x5d8));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x5d8), value);
        }
        public OmsiPathManager PathManager
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x5dc));
        }
        public OmsiPathPointBasic PathPointTarget
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x5e0));
        }
        public OmsiPathPointBasic PathPointNext
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x5e4));
        }
        public OmsiPathPointBasic ActPathPoint
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x5e8));
        }
        public bool WaitBeforeEnd
        {
            get => Memory.ReadMemory<bool>(Address + 0x5ec);
            set => Memory.WriteMemory(Address + 0x5ec, value);
        }
        public bool Anim_Cont
        {
            get => Memory.ReadMemory<bool>(Address + 0x5ed);
            set => Memory.WriteMemory(Address + 0x5ed, value);
        }
        public bool Anim_Refresh
        {
            get => Memory.ReadMemory<bool>(Address + 0x5ee);
            set => Memory.WriteMemory(Address + 0x5ee, value);
        }
        public bool InWorld
        {
            get => Memory.ReadMemory<bool>(Address + 0x5ef);
            set => Memory.WriteMemory(Address + 0x5ef, value);
        }
        public float WayToGo
        {
            get => Memory.ReadMemory<float>(Address + 0x5f0);
            set => Memory.WriteMemory(Address + 0x5f0, value);
        }
        public string Target_Station
        {
            get => Memory.ReadMemoryString(Address + 0x5f4);
            set => Memory.WriteMemory(Address + 0x5f4, value);
        }
        public int Target_Station_Termini_Index
        {
            get => Memory.ReadMemory<int>(Address + 0x5f8);
            set => Memory.WriteMemory(Address + 0x5f8, value);
        }
        public string Pre_Target_Station
        {
            get => Memory.ReadMemoryString(Address + 0x5fc);
            set => Memory.WriteMemory(Address + 0x5fc, value);
        }
        public bool Pre_Target_Station_Reached
        {
            get => Memory.ReadMemory<bool>(Address + 0x600);
            set => Memory.WriteMemory(Address + 0x600, value);
        }
        public float Dist_PreTarget_Stn
        {
            get => Memory.ReadMemory<float>(Address + 0x604);
            set => Memory.WriteMemory(Address + 0x604, value);
        }
        public float SollDeparture
        {
            get => Memory.ReadMemory<float>(Address + 0x608);
            set => Memory.WriteMemory(Address + 0x608, value);
        }
        public float EnterBusAt
        {
            get => Memory.ReadMemory<float>(Address + 0x60c);
            set => Memory.WriteMemory(Address + 0x60c, value);
        }
        public int SeatNrBus
        {
            get => Memory.ReadMemory<int>(Address + 0x610);
            set => Memory.WriteMemory(Address + 0x610, value);
        }
        public int BusPartOffset
        {
            get => Memory.ReadMemory<int>(Address + 0x614);
            set => Memory.WriteMemory(Address + 0x614, value);
        }
        public int SeatNrStn
        {
            get => Memory.ReadMemory<int>(Address + 0x618);
            set => Memory.WriteMemory(Address + 0x618, value);
        }
        public byte TicketType
        {
            get => Memory.ReadMemory<byte>(Address + 0x61c);
            set => Memory.WriteMemory(Address + 0x61c, value);
        }
        public byte TicketIndex
        {
            get => Memory.ReadMemory<byte>(Address + 0x61d);
            set => Memory.WriteMemory(Address + 0x61d, value);
        }
        public float Ticket_Soll
        {
            get => Memory.ReadMemory<float>(Address + 0x620);
            set => Memory.WriteMemory(Address + 0x620, value);
        }
        public float Ticket_Gegeben
        {
            get => Memory.ReadMemory<float>(Address + 0x624);
            set => Memory.WriteMemory(Address + 0x624, value);
        }
        public bool Ticket_BadChange
        {
            get => Memory.ReadMemory<bool>(Address + 0x628);
            set => Memory.WriteMemory(Address + 0x628, value);
        }
        public bool Ticket_Ready
        {
            get => Memory.ReadMemory<bool>(Address + 0x629);
            set => Memory.WriteMemory(Address + 0x629, value);
        }
        public float Fahrt_Bew
        {
            get => Memory.ReadMemory<float>(Address + 0x62c);
            set => Memory.WriteMemory(Address + 0x62c, value);
        }
        public int Fahrt_Mecker
        {
            get => Memory.ReadMemory<int>(Address + 0x630);
            set => Memory.WriteMemory(Address + 0x630, value);
        }
        /*
        public ? Fahrt_Mecker_Grenzwerte
        {
            get => Memory.ReadMemory<?>(Address + 0x634);
            set => Memory.WriteMemory(Address + 0x634, value);
        }*/
        /// <summary>
        /// ! Warn ! Unrecognised value
        /// </summary>
        public float Unkown_A
        {
            get => Memory.ReadMemory<float>(Address + 0x638);
            set => Memory.WriteMemory(Address + 0x638, value);
        }
        /// <summary>
        /// ! Warn ! Unrecognised value
        /// </summary>
        public float Unkown_B
        {
            get => Memory.ReadMemory<float>(Address + 0x63c);
            set => Memory.WriteMemory(Address + 0x63c, value);
        }
        public int MyEntryExit
        {
            get => Memory.ReadMemory<int>(Address + 0x640);
            set => Memory.WriteMemory(Address + 0x640, value);
        }
        public float LastMovedDist
        {
            get => Memory.ReadMemory<float>(Address + 0x644);
            set => Memory.WriteMemory(Address + 0x644, value);
        }
        public float HeightOfSeat
        {
            get => Memory.ReadMemory<float>(Address + 0x648);
            set => Memory.WriteMemory(Address + 0x648, value);
        }
        public float State
        {
            get => Memory.ReadMemory<float>(Address + 0x64c);
            set => Memory.WriteMemory(Address + 0x64c, value);
        }
        public float Timer
        {
            get => Memory.ReadMemory<float>(Address + 0x650);
            set => Memory.WriteMemory(Address + 0x650, value);
        }
        public float Dist_Timer
        {
            get => Memory.ReadMemory<float>(Address + 0x654);
            set => Memory.WriteMemory(Address + 0x654, value);
        }
        public OmsiPathID LastPath
        {
            get => Memory.ReadMemory<OmsiPathID>(Address + 0x658);
            set => Memory.WriteMemory(Address + 0x658, value);
        }
        public bool Smooth
        {
            get => Memory.ReadMemory<bool>(Address + 0x660);
            set => Memory.WriteMemory(Address + 0x660, value);
        }
        public bool Righty
        {
            get => Memory.ReadMemory<bool>(Address + 0x661);
            set => Memory.WriteMemory(Address + 0x661, value);
        }
        public bool Activity_FixDriver
        {
            get => Memory.ReadMemory<bool>(Address + 0x662);
            set => Memory.WriteMemory(Address + 0x662, value);
        }
        public byte Activity_Leg
        {
            get => Memory.ReadMemory<byte>(Address + 0x663);
            set => Memory.WriteMemory(Address + 0x663, value);
        }
        public bool Activity_Arm_Umbrella
        {
            get => Memory.ReadMemory<bool>(Address + 0x664);
            set => Memory.WriteMemory(Address + 0x664, value);
        }
        public bool Activity_Arm_KI
        {
            get => Memory.ReadMemory<bool>(Address + 0x665);
            set => Memory.WriteMemory(Address + 0x665, value);
        }
        public bool Activity_Head_KI
        {
            get => Memory.ReadMemory<bool>(Address + 0x666);
            set => Memory.WriteMemory(Address + 0x666, value);
        }
        public float RoomHeight
        {
            get => Memory.ReadMemory<float>(Address + 0x668);
            set => Memory.WriteMemory(Address + 0x668, value);
        }
        public float Steps
        {
            get => Memory.ReadMemory<float>(Address + 0x66c);
            set => Memory.WriteMemory(Address + 0x66c, value);
        }
        public D3DVector KI_Target_Pos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x670);
            set => Memory.WriteMemory(Address + 0x670, value);
        }
        public D3DVector KI_Target_Pos_Global
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x67c);
            set => Memory.WriteMemory(Address + 0x67c, value);
        }
        public D3DVector KI_Target_Head
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x688);
            set => Memory.WriteMemory(Address + 0x688, value);
        }
        public short StepSoundIndex
        {
            get => Memory.ReadMemory<short>(Address + 0x694);
            set => Memory.WriteMemory(Address + 0x694, value);
        }
        public int LinkIndex
        {
            get => Memory.ReadMemory<int>(Address + 0x698);
            set => Memory.WriteMemory(Address + 0x698, value);
        }
        public float Soll_HDG
        {
            get => Memory.ReadMemory<float>(Address + 0x69c);
            set => Memory.WriteMemory(Address + 0x69c, value);
        }
        public float Soll_Speed
        {
            get => Memory.ReadMemory<float>(Address + 0x6a0);
            set => Memory.WriteMemory(Address + 0x6a0, value);
        }
        public float Act_Speed
        {
            get => Memory.ReadMemory<float>(Address + 0x6a4);
            set => Memory.WriteMemory(Address + 0x6a4, value);
        }
        public float Act_HDG
        {
            get => Memory.ReadMemory<float>(Address + 0x6a8);
            set => Memory.WriteMemory(Address + 0x6a8, value);
        }
        public float MyMaxSpeed
        {
            get => Memory.ReadMemory<float>(Address + 0x6ac);
            set => Memory.WriteMemory(Address + 0x6ac, value);
        }
        public int MyBusIndex_Temp
        {
            get => Memory.ReadMemory<int>(Address + 0x6b0);
            set => Memory.WriteMemory(Address + 0x6b0, value);
        }
        public OmsiRoadVehicleInst MyBus
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x6b4));
        }
        public uint MyBusCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x6b8);
            set => Memory.WriteMemory(Address + 0x6b8, value);
        }
        public int MyStation
        {
            get => Memory.ReadMemory<int>(Address + 0x6bc);
            set => Memory.WriteMemory(Address + 0x6bc, value);
        }
        public uint MyStationIDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x6c0);
            set => Memory.WriteMemory(Address + 0x6c0, value);
        }
        public OmsiHumanAIMode AIMode
        {
            get => (OmsiHumanAIMode) Memory.ReadMemory<byte>(Address + 0x6c4);
            set => Memory.WriteMemory(Address + 0x6c4, value);
        }
        public OmsiHumanAIModeEX AIModeEx
        {
            get => (OmsiHumanAIModeEX) Memory.ReadMemory<byte>(Address + 0x6c5);
            set => Memory.WriteMemory(Address + 0x6c5, value);
        }
        public OmsiHumanAISubMode AISubMode
        {
            get => (OmsiHumanAISubMode) Memory.ReadMemory<byte>(Address + 0x6c6);
            set => Memory.WriteMemory(Address + 0x6c6, value);
        }
        public OmsiHumanKollision KollisionState
        {
            get => (OmsiHumanKollision) Memory.ReadMemory<byte>(Address + 0x6c7);
            set => Memory.WriteMemory(Address + 0x6c7, value);
        }
        public OmsiHumanKollisionFreeSide KollisionFreeSide
        {
            get => Memory.ReadMemory<OmsiHumanKollisionFreeSide>(Address + 0x6c8);
            set => Memory.WriteMemory(Address + 0x6c8, value);
        }

    }
}
