using System;

namespace OmsiHook
{
    public class OmsiMap : OmsiObject
    {
        internal OmsiMap(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }

        public OmsiPoint NW_Corner
        {
            get => omsiMemory.ReadMemory<OmsiPoint>(baseAddress + 0x4);
            set => omsiMemory.WriteMemory(baseAddress + 0x4, value);
        }

        public OmsiPoint SE_Corner
        {
            get => omsiMemory.ReadMemory<OmsiPoint>(baseAddress + 0xc);
            set => omsiMemory.WriteMemory(baseAddress + 0xc, value);
        }

        public bool LoadAllKacheln
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x14);
            set => omsiMemory.WriteMemory(baseAddress + 0x14, value);
        }

        public bool NextTime_CheckKachelUnloading
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x15);
            set => omsiMemory.WriteMemory(baseAddress + 0x15, value);
        }

        public uint DynTile_RedTimer
        {
            get => omsiMemory.ReadMemory<uint>(baseAddress + 0x18);
            set => omsiMemory.WriteMemory(baseAddress + 0x18, value);
        }

        public byte FDynTileIst
        {
            get => omsiMemory.ReadMemory<byte>(baseAddress + 0x1c);
            set => omsiMemory.WriteMemory(baseAddress + 0x1c, value);
        }

        public bool NotUnloaded
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x1d);
            set => omsiMemory.WriteMemory(baseAddress + 0x1d, value);
        }

        public int Version
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0x20);
            set => omsiMemory.WriteMemory(baseAddress + 0x20, value);
        }
    }
}
