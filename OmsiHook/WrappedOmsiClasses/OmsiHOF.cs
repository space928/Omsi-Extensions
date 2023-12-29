using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Representation of a HOF / yard / depot File
    /// </summary>
    public class OmsiHOF : OmsiObject
    {
        internal OmsiHOF(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiHOF() : base() { }

        public string Name
        {
            get => Memory.ReadMemoryString(Address + 0x4, true);
            set => Memory.WriteMemory(Address + 0x4, value, true);
        }
        public string[] GlobalStrings
        {
            get => Memory.ReadMemoryStringArray(Address + 0x8, true);
            //set => Memory.WriteMemory(Address + 0x8, value);
        }
        public string ServiceTrip
        {
            get => Memory.ReadMemoryString(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public byte TargetStringCount
        {
            get => Memory.ReadMemory<byte>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public byte BusStopStringCount
        {
            get => Memory.ReadMemory<byte>(Address + 0x11);
            set => Memory.WriteMemory(Address + 0x11, value);
        }
        public OmsiHOFTarget[] Targets
        {
            get => Memory.MarshalStructs<OmsiHOFTarget, OmsiHOFTargetInternal>(
                            Memory.ReadMemoryStructArray<OmsiHOFTargetInternal>(Address + 0x14));
            //set => Memory.WriteMemory(Address + 0x14, value);
        }
        public OmsiHofFISBusstop[] Busstops
        {
            get => Memory.MarshalStructs<OmsiHofFISBusstop, OmsiHofFISBusstopInternal>(
                            Memory.ReadMemoryStructArray<OmsiHofFISBusstopInternal>(Address + 0x18));
            //set => Memory.WriteMemory(Address + 0x18, value);
        }
        public OmsiHofFISTrip[] HofFISTrips
        {
            get => Memory.MarshalStructs<OmsiHofFISTrip, OmsiHofFISTripInternal>(
                            Memory.ReadMemoryStructArray<OmsiHofFISTripInternal>(Address + 0x1c));
            //set => Memory.WriteMemory(Address + 0x1c, value);
        }

    }
}
