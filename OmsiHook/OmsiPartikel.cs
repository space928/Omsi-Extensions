using System;

namespace OmsiHook
{
    public class OmsiPartikel : OmsiObject
    {
        internal OmsiPartikel(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }

        public D3DVector Position
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x4);
            set => omsiMemory.WriteMemory(baseAddress + 0x4, value);
        }
        public D3DVector Veloc
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x10);
            set => omsiMemory.WriteMemory(baseAddress + 0x10, value);
        }
        /// <summary>
        /// Birthday?
        /// </summary>
        public uint Geburtsdatum
        {
            get => omsiMemory.ReadMemory<uint>(baseAddress + 0x1c);
            set => omsiMemory.WriteMemory(baseAddress + 0x1c, value);
        }
        /// <summary>
        /// Death Day?
        /// </summary>
        public uint Sterbedatum
        {
            get => omsiMemory.ReadMemory<uint>(baseAddress + 0x20);
            set => omsiMemory.WriteMemory(baseAddress + 0x20, value);
        }
        public float Rotation
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x24);
            set => omsiMemory.WriteMemory(baseAddress + 0x24, value);
        }
        /// <summary>
        /// Braking Factor?
        /// </summary>
        public float BremsFaktor
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x28);
            set => omsiMemory.WriteMemory(baseAddress + 0x28, value);
        }
        public float FallKoeffizent
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x2c);
            set => omsiMemory.WriteMemory(baseAddress + 0x2c, value);
        }
        public float StartSize
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x30);
            set => omsiMemory.WriteMemory(baseAddress + 0x30, value);
        }
        public float SizeGrow
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x34);
            set => omsiMemory.WriteMemory(baseAddress + 0x34, value);
        }
        public float Alpha_Initial
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x38);
            set => omsiMemory.WriteMemory(baseAddress + 0x38, value);
        }
        public float Alpha_End
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x3c);
            set => omsiMemory.WriteMemory(baseAddress + 0x3c, value);
        }
        /// <summary>
         /// Color
         /// </summary>
        public int Farbe
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x40);
            set => omsiMemory.WriteMemory(baseAddress + 0x40, value);
        }
        public bool Attached
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x44);
            set => omsiMemory.WriteMemory(baseAddress + 0x44, value);
        }
        public bool Illuminated
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x45);
            set => omsiMemory.WriteMemory(baseAddress + 0x45, value);
        }
        public float VisFactor
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x48);
            set => omsiMemory.WriteMemory(baseAddress + 0x48, value);
        }
        public float Z_Offset
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x4c);
            set => omsiMemory.WriteMemory(baseAddress + 0x4c, value);
        }
        public bool Spotlight_Calc
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x50);
            set => omsiMemory.WriteMemory(baseAddress + 0x50, value);
        }
        public float Visibility
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x54);
            set => omsiMemory.WriteMemory(baseAddress + 0x54, value);
        }
        public float TempAlpha
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x58);
            set => omsiMemory.WriteMemory(baseAddress + 0x58, value);
        }

    }
}
