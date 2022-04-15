using System;

namespace OmsiHook
{
    public class OmsiPartikelemitter : OmsiObject
    {
        internal OmsiPartikelemitter(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }

        public uint LastEmitTime
        {
            get => omsiMemory.ReadMemory<uint>(baseAddress + 0x4);
            set => omsiMemory.WriteMemory(baseAddress + 0x4, value);
        }
        public float Frequency
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x8);
            set => omsiMemory.WriteMemory(baseAddress + 0x8, value);
        }
        /// <summary>
        /// Lifespan
        /// </summary>
        public float Lebensdauer
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0xc);
            set => omsiMemory.WriteMemory(baseAddress + 0xc, value);
        }
        public float Lebensdauer_Variation
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x10);
            set => omsiMemory.WriteMemory(baseAddress + 0x10, value);
        }
        public D3DVector VeLoc
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x14);
            set => omsiMemory.WriteMemory(baseAddress + 0x14, value);
        }
        public float V_Variation
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x20);
            set => omsiMemory.WriteMemory(baseAddress + 0x20, value);
        }
        /// <summary>
        /// Braking Factor?
        /// </summary>
        public float BremsFaktor
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x24);
            set => omsiMemory.WriteMemory(baseAddress + 0x24, value);
        }
        public float FallKoeffizent
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x28);
            set => omsiMemory.WriteMemory(baseAddress + 0x28, value);
        }
        public float StartSize
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x2c);
            set => omsiMemory.WriteMemory(baseAddress + 0x2c, value);
        }
        public float SizeGrow
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x30);
            set => omsiMemory.WriteMemory(baseAddress + 0x30, value);
        }
        public D3DVector X_Var
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x34);
            set => omsiMemory.WriteMemory(baseAddress + 0x34, value);
        }
        public D3DVector Y_Var
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x40);
            set => omsiMemory.WriteMemory(baseAddress + 0x40, value);
        }
        public float Alpha_Initial
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x4c);
            set => omsiMemory.WriteMemory(baseAddress + 0x4c, value);
        }
        public float Alpha_End
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x50);
            set => omsiMemory.WriteMemory(baseAddress + 0x50, value);
        }
        public float Alpha_Variation
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x54);
            set => omsiMemory.WriteMemory(baseAddress + 0x54, value);
        }
        /// <summary>
        /// Color
        /// </summary>
        public int Farbe
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x58);
            set => omsiMemory.WriteMemory(baseAddress + 0x58, value);
        }
        public float MaxCnt
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x5c);
            set => omsiMemory.WriteMemory(baseAddress + 0x5c, value);
        }
        public bool Spotlight_Calc
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x60);
            set => omsiMemory.WriteMemory(baseAddress + 0x60, value);
        }
        /* TODO:
        public OmsiPartikel[] Partikel
        {
            get => omsiMemory.ReadMemory<OmsiPartikel[]>(baseAddress + 0x64);
            //set => omsiMemory.WriteMemory(baseAddress + 0x64, value);
        }*/
        public int Textur
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x68);
            set => omsiMemory.WriteMemory(baseAddress + 0x68, value);
        }
        public D3DVector Position
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x6c);
            set => omsiMemory.WriteMemory(baseAddress + 0x6c, value);
        }

    }
}
