using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Segment between Path Points
    /// </summary>
    public class OmsiPathSegment : OmsiMapObjInst
    {
        public OmsiPathSegment() : base() { }

        internal OmsiPathSegment(Memory memory, int address) : base(memory, address) { }

        public uint Parent_IDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x138);
            set => Memory.WriteMemory(Address + 0x138, value);
        }
        public int Parent_PathIndex
        {
            get => Memory.ReadMemory<int>(Address + 0x13c);
            set => Memory.WriteMemory(Address + 0x13c, value);
        }
        public OmsiObject Parent
        {
            get => Memory.ReadMemoryObject<OmsiObject>(Address, 0x140, false);
        }
        public byte Type
        {
            get => Memory.ReadMemory<byte>(Address + 0x144);
            set => Memory.WriteMemory(Address + 0x144, value);
        }
        public OmsiCoordSystem CoordSystem
        {
            get => Memory.ReadMemoryObject<OmsiCoordSystem>(Address, 0x148, false);
        }
        public bool CoordSystemIsMine
        {
            get => Memory.ReadMemory<bool>(Address + 0x14c);
            set => Memory.WriteMemory(Address + 0x14c, value);
        }
        public float StartOffsetX
        {
            get => Memory.ReadMemory<float>(Address + 0x150);
            set => Memory.WriteMemory(Address + 0x150, value);
        }
        public float EndOffsetX
        {
            get => Memory.ReadMemory<float>(Address + 0x154);
            set => Memory.WriteMemory(Address + 0x154, value);
        }
        public float StartOffsetY
        {
            get => Memory.ReadMemory<float>(Address + 0x158);
            set => Memory.WriteMemory(Address + 0x158, value);
        }
        public float EndOffsetY
        {
            get => Memory.ReadMemory<float>(Address + 0x15c);
            set => Memory.WriteMemory(Address + 0x15c, value);
        }
        public float CantOffset
        {
            get => Memory.ReadMemory<float>(Address + 0x160);
            set => Memory.WriteMemory(Address + 0x160, value);
        }
        public float Width
        {
            get => Memory.ReadMemory<float>(Address + 0x164);
            set => Memory.WriteMemory(Address + 0x164, value);
        }
        /// <summary>
        /// Length
        /// </summary>
        public float Laenge
        {
            get => Memory.ReadMemory<float>(Address + 0x168);
            set => Memory.WriteMemory(Address + 0x168, value);
        }
        public byte Reverse
        {
            get => Memory.ReadMemory<byte>(Address + 0x16c);
            set => Memory.WriteMemory(Address + 0x16c, value);
        }
        public bool Mirror
        {
            get => Memory.ReadMemory<bool>(Address + 0x16d);
            set => Memory.WriteMemory(Address + 0x16d, value);
        }
        public OmsiPathLink[] Next
        {
            get => Memory.MarshalStructs<OmsiPathLink, OmsiPathLinkInternal>(
                Memory.ReadMemoryStructArray<OmsiPathLinkInternal>(Address + 0x170));
        }
        public OmsiPathLink[] Previous
        {
            get => Memory.MarshalStructs<OmsiPathLink, OmsiPathLinkInternal>(
                Memory.ReadMemoryStructArray<OmsiPathLinkInternal>(Address + 0x174));
        }
        public OmsiPathID NextL
        {
            get => Memory.ReadMemory<OmsiPathID>(Address + 0x178);
            set => Memory.WriteMemory(Address + 0x178, value);
        }
        public OmsiPathID NextR
        {
            get => Memory.ReadMemory<OmsiPathID>(Address + 0x180);
            set => Memory.WriteMemory(Address + 0x180, value);
        }
        public OmsiPathGroupID Group
        {
            get => Memory.ReadMemory<OmsiPathGroupID>(Address + 0x188);
            set => Memory.WriteMemory(Address + 0x188, value);
        }
        public int GroupPath
        {
            get => Memory.ReadMemory<int>(Address + 0x190);
            set => Memory.WriteMemory(Address + 0x190, value);
        }
        public int PathLine
        {
            get => Memory.ReadMemory<int>(Address + 0x194);
            set => Memory.WriteMemory(Address + 0x194, value);
        }
        public float V_Phys_Max_Factor
        {
            get => Memory.ReadMemory<float>(Address + 0x198);
            set => Memory.WriteMemory(Address + 0x198, value);
        }
        public float V_Temp_Max
        {
            get => Memory.ReadMemory<float>(Address + 0x19c);
            set => Memory.WriteMemory(Address + 0x19c, value);
        }
        public short Blinker
        {
            get => Memory.ReadMemory<short>(Address + 0x1a0);
            set => Memory.WriteMemory(Address + 0x1a0, value);
        }
        public short SwitchDir
        {
            get => Memory.ReadMemory<short>(Address + 0x1a1);
            set => Memory.WriteMemory(Address + 0x1a1, value);
        }
        public bool CrossingProblem
        {
            get => Memory.ReadMemory<bool>(Address + 0x1a2);
            set => Memory.WriteMemory(Address + 0x1a2, value);
        }
        public byte Blockiert
        {
            get => Memory.ReadMemory<byte>(Address + 0x1a3);
            set => Memory.WriteMemory(Address + 0x1a3, value);
        }
        public byte AmpelBlocking // pByte
        {
            get => Memory.ReadMemory<byte>(Memory.ReadMemory<int>(Address + 0x1a4));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x1a4), value);
        }
        /// <summary>
        /// Request?
        /// </summary>
        public bool Anforderung
        {
            get => Memory.ReadMemory<bool>(Address + 0x1a8);
            set => Memory.WriteMemory(Address + 0x1a8, value);
        }
        public float AnforderungPnt // pSingle
        {
            get => Memory.ReadMemory<float>(Memory.ReadMemory<int>(Address + 0x1ac));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x1ac), value);
        }
        public uint Anforderung_Frame_Pnt // pCardinal
        {
            get => Memory.ReadMemory<uint>(Memory.ReadMemory<int>(Address + 0x1b0));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x1b0), value);
        }
        public float ApproachDist // pSingle
        {
            get => Memory.ReadMemory<float>(Memory.ReadMemory<int>(Address + 0x1b4));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x1b4), value);
        }
        /*
        public float[] AIDens
        {
            get => Memory.ReadMemoryObjArray<float>(Address + 0x1b8);
        }
        public int[] AIDensAct
        {
            get => Memory.ReadMemoryObjArray<int>(Address + 0x1bc);
        }
        public float[] AIDensRel
        {
            get => Memory.ReadMemoryObjArray<float>(Address + 0x1c0);
        }*/
        public OmsiSplineSegment Helper
        {
            get => Memory.ReadMemoryObject<OmsiSplineSegment>(Address, 0x1c4, false);
        }
        /* WAITING FOR PR#25
        public OmsiPathRule[] Rules
        {
            get => Memory.MarshalStructs<OmsiPathRule, OmsiPathRuleInternal>(Memory.ReadMemoryStructArray<OmsiPathRuleInternal>(Address + 0x1c8));
        }*/
        public MemArray<OmsiPathID> Parallel
        {
            get => new(Memory, Address + 0x1cc);
        }
        public MemArray<OmsiPathCrossing> Crossings
        {
            get => new(Memory, Address + 0x1d0);
        }
        public float Crossing_Min
        {
            get => Memory.ReadMemory<float>(Address + 0x1d4);
            set => Memory.WriteMemory(Address + 0x1d4, value);
        }
        public float Crossing_Max
        {
            get => Memory.ReadMemory<float>(Address + 0x1d8);
            set => Memory.WriteMemory(Address + 0x1d8, value);
        }
        public OmsiPathReservation[] Rules
        {
            get => Memory.MarshalStructs<OmsiPathReservation, OmsiPathReservationInternal>(Memory.ReadMemoryStructArray<OmsiPathReservationInternal>(Address + 0x1dc));
        }
        public MemArray<OmsiPathSegmentFStr> FStrn
        {
            get => new(Memory, Address + 0x1e0);
        }
        public OmsiPathInfoRailEnh Rail_Enh // Ptr
        {
            get => Memory.ReadMemory<OmsiPathInfoRailEnh>(Memory.ReadMemory<int>(Address + 0x1e4));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x1e4), value);
        }
        public OmsiThirdRail[] Third_Rails // Ptr
        {
            get => Memory.ReadMemoryStructArray<OmsiThirdRail>(Memory.ReadMemory<int>(Address + 0x1e8));
        }
        /*
        public uint[] NextEmitTime
        {
            get => Memory.ReadMemoryObjArray<uint>(Address + 0x1ec);
        }*/
    }
}
