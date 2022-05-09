using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Class that decodes the date and time in game in OMSI
    /// </summary>
    public class OmsiTime : OmsiObject
    {

        public OmsiTime() : base() { }

        internal OmsiTime(Memory memory, int address) : base(memory, address) { }

        public byte Hour
        {
            get => Memory.ReadMemory<byte>(0x0086176c);
            set => Memory.WriteMemory(0x0086176c, value);
        }
        public byte Minute
        {
            get => Memory.ReadMemory<byte>(0x0086176d);
            set => Memory.WriteMemory(0x0086176d, value);
        }
        public float Second
        {
            get => Memory.ReadMemory<float>(0x00861770);
            set => Memory.WriteMemory(0x00861770, value);
        }
        public int Day
        {
            get => Memory.ReadMemory<int>(0x00861778);
            set => Memory.WriteMemory(0x00861778, value);
        }
        public byte Month
        {
            get => Memory.ReadMemory<byte>(0x0086178c);
            set => Memory.WriteMemory(0x0086178c, value);
        }
        public int Year
        {
            get => Memory.ReadMemory<int>(0x00861790);
            set => Memory.WriteMemory(0x00861790, value);
        }
    }
}
