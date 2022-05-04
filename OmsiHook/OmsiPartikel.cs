using System;

namespace OmsiHook
{
    public class OmsiPartikel : OmsiObject
    {
        internal OmsiPartikel(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiPartikel() : base() { }

        public D3DVector Position
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public D3DVector Veloc
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        /// <summary>
        /// Birthday?
        /// </summary>
        public uint Geburtsdatum
        {
            get => Memory.ReadMemory<uint>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        /// <summary>
        /// Death Day?
        /// </summary>
        public uint Sterbedatum
        {
            get => Memory.ReadMemory<uint>(Address + 0x20);
            set => Memory.WriteMemory(Address + 0x20, value);
        }
        public float Rotation
        {
            get => Memory.ReadMemory<float>(Address + 0x24);
            set => Memory.WriteMemory(Address + 0x24, value);
        }
        /// <summary>
        /// Braking Factor?
        /// </summary>
        public float BremsFaktor
        {
            get => Memory.ReadMemory<float>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public float FallKoeffizent
        {
            get => Memory.ReadMemory<float>(Address + 0x2c);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }
        public float StartSize
        {
            get => Memory.ReadMemory<float>(Address + 0x30);
            set => Memory.WriteMemory(Address + 0x30, value);
        }
        public float SizeGrow
        {
            get => Memory.ReadMemory<float>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }
        public float Alpha_Initial
        {
            get => Memory.ReadMemory<float>(Address + 0x38);
            set => Memory.WriteMemory(Address + 0x38, value);
        }
        public float Alpha_End
        {
            get => Memory.ReadMemory<float>(Address + 0x3c);
            set => Memory.WriteMemory(Address + 0x3c, value);
        }
        /// <summary>
         /// Color
         /// </summary>
        public int Farbe
        {
            get => Memory.ReadMemory<int>(Address + 0x40);
            set => Memory.WriteMemory(Address + 0x40, value);
        }
        public bool Attached
        {
            get => Memory.ReadMemory<bool>(Address + 0x44);
            set => Memory.WriteMemory(Address + 0x44, value);
        }
        public bool Illuminated
        {
            get => Memory.ReadMemory<bool>(Address + 0x45);
            set => Memory.WriteMemory(Address + 0x45, value);
        }
        public float VisFactor
        {
            get => Memory.ReadMemory<float>(Address + 0x48);
            set => Memory.WriteMemory(Address + 0x48, value);
        }
        public float Z_Offset
        {
            get => Memory.ReadMemory<float>(Address + 0x4c);
            set => Memory.WriteMemory(Address + 0x4c, value);
        }
        public bool Spotlight_Calc
        {
            get => Memory.ReadMemory<bool>(Address + 0x50);
            set => Memory.WriteMemory(Address + 0x50, value);
        }
        public float Visibility
        {
            get => Memory.ReadMemory<float>(Address + 0x54);
            set => Memory.WriteMemory(Address + 0x54, value);
        }
        public float TempAlpha
        {
            get => Memory.ReadMemory<float>(Address + 0x58);
            set => Memory.WriteMemory(Address + 0x58, value);
        }

    }
}
