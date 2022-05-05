using System;

namespace OmsiHook
{
    public class OmsiPartikelEmitter : OmsiObject
    {
        internal OmsiPartikelEmitter(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiPartikelEmitter() : base() { }

        public uint LastEmitTime
        {
            get => Memory.ReadMemory<uint>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public float Frequency
        {
            get => Memory.ReadMemory<float>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        /// <summary>
        /// Lifespan
        /// </summary>
        public float Lebensdauer
        {
            get => Memory.ReadMemory<float>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public float Lebensdauer_Variation
        {
            get => Memory.ReadMemory<float>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public D3DVector VeLoc
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public float V_Variation
        {
            get => Memory.ReadMemory<float>(Address + 0x20);
            set => Memory.WriteMemory(Address + 0x20, value);
        }
        /// <summary>
        /// Braking Factor?
        /// </summary>
        public float BremsFaktor
        {
            get => Memory.ReadMemory<float>(Address + 0x24);
            set => Memory.WriteMemory(Address + 0x24, value);
        }
        public float FallKoeffizent
        {
            get => Memory.ReadMemory<float>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public float StartSize
        {
            get => Memory.ReadMemory<float>(Address + 0x2c);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }
        public float SizeGrow
        {
            get => Memory.ReadMemory<float>(Address + 0x30);
            set => Memory.WriteMemory(Address + 0x30, value);
        }
        public D3DVector X_Var
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }
        public D3DVector Y_Var
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x40);
            set => Memory.WriteMemory(Address + 0x40, value);
        }
        public float Alpha_Initial
        {
            get => Memory.ReadMemory<float>(Address + 0x4c);
            set => Memory.WriteMemory(Address + 0x4c, value);
        }
        public float Alpha_End
        {
            get => Memory.ReadMemory<float>(Address + 0x50);
            set => Memory.WriteMemory(Address + 0x50, value);
        }
        public float Alpha_Variation
        {
            get => Memory.ReadMemory<float>(Address + 0x54);
            set => Memory.WriteMemory(Address + 0x54, value);
        }
        /// <summary>
        /// Color
        /// </summary>
        public int Farbe
        {
            get => Memory.ReadMemory<int>(Address + 0x58);
            set => Memory.WriteMemory(Address + 0x58, value);
        }
        public float MaxCnt
        {
            get => Memory.ReadMemory<float>(Address + 0x5c);
            set => Memory.WriteMemory(Address + 0x5c, value);
        }
        public bool Spotlight_Calc
        {
            get => Memory.ReadMemory<bool>(Address + 0x60);
            set => Memory.WriteMemory(Address + 0x60, value);
        }
        public OmsiPartikel[] Partikel => Memory.ReadMemoryObjArray<OmsiPartikel>(Address + 0x64);
        public int Textur
        {
            get => Memory.ReadMemory<int>(Address + 0x68);
            set => Memory.WriteMemory(Address + 0x68, value);
        }
        public D3DVector Position
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x6c);
            set => Memory.WriteMemory(Address + 0x6c, value);
        }
    }
}
