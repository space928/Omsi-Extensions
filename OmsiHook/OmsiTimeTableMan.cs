using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    public class OmsiTimeTableMan : OmsiObject
    {
        public OmsiTimeTableMan() : base() { }

        internal OmsiTimeTableMan(Memory memory, int address) : base(memory, address) { }

        public bool Invalid
        {
            get => Memory.ReadMemory<bool>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public OmsiTTTrack[] Tracks
        {
            get => Memory.MarshalStructs<OmsiTTTrack, OmsiTTTrackInternal>(Memory.ReadMemoryStructArray<OmsiTTTrackInternal>(Address + 0x8));
        }
        public OmsiTTTrip[] Trips
        {
            get => Memory.MarshalStructs<OmsiTTTrip, OmsiTTTripInternal>(Memory.ReadMemoryStructArray<OmsiTTTripInternal>(Address + 0xc));
        }
        public OmsiTTBusstopListEntry[] BusstopList
        {
            get => Memory.MarshalStructs<OmsiTTBusstopListEntry, OmsiTTBusstopListEntryInternal>(Memory.ReadMemoryStructArray<OmsiTTBusstopListEntryInternal>(Address + 0x10));
        }
        public OmsiTTStnLink[] StnLinks
        {
            get => Memory.MarshalStructs<OmsiTTStnLink, OmsiTTStnLinkInternal>(Memory.ReadMemoryStructArray<OmsiTTStnLinkInternal>(Address + 0x14));
        }
        public OmsiTTLine[] Lines
        {
            get => Memory.MarshalStructs<OmsiTTLine, OmsiTTLineInternal>(Memory.ReadMemoryStructArray<OmsiTTLineInternal>(Address + 0x18));
        }
        public bool AllStationIndicesResetted
        {
            get => Memory.ReadMemory<bool>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public OmsiRVFile[] RVFiles
        {
            get => Memory.MarshalStructs<OmsiRVFile, OmsiRVFileInternal>(Memory.ReadMemoryStructArray<OmsiRVFileInternal>(Address + 0x20));
        }
        public string[] NoRVNumbers
        {
            get => Memory.ReadMemoryStringArray(Address + 0x24, raw: true);
        }
    }
}
