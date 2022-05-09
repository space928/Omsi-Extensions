using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Settings relating to the real world weather in OMSI
    /// </summary>
    public class OmsiActuWeather : OmsiObject
    {
        internal OmsiActuWeather(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiActuWeather() : base() { }
        /*
        public OmsiWebBrowser WebBrowser1
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x4));
        }*/
        public bool Active
        {
            get => Memory.ReadMemory<bool>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public string ICAO
        {
            get => Memory.ReadMemoryString(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        /// <summary>
        /// Delphi DateTime - integer part = days since 30/12/1899, fraction part = time of day
        /// </summary>
        public double LastDownloaded
        {
            get => Memory.ReadMemory<double>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public bool Invalid_ICAO
        {
            get => Memory.ReadMemory<bool>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        public uint Counter
        {
            get => Memory.ReadMemory<uint>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public byte Prozess
        {
            get => Memory.ReadMemory<byte>(Address + 0x20);
            set => Memory.WriteMemory(Address + 0x20, value);
        }
    }
}
