using System;

namespace OmsiHook
{
    /// <summary>
    /// Sound that is playable by OMSI
    /// </summary>
    public class OmsiSound : OmsiObject
    {
        internal OmsiSound(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiSound() : base() { }
        public string FileName
        {
            get => Memory.ReadMemoryString(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        /// <summary>
        /// Pointer to an IDirectSound8
        /// </summary>
        public IntPtr Device
        {
            get => (IntPtr)Memory.ReadMemory<int>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, (int)value);
        }
        /// <summary>
        /// Pointer to an IDirectSoundBuffer8
        /// </summary>
        public IntPtr SoundBuffer
        {
            get => (IntPtr)Memory.ReadMemory<int>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, (int)value);
        }
        /// <summary>
        /// Pointer to an IDirectSoundFXWavesReverb8
        /// </summary>
        public IntPtr FX_Hall
        {
            get => (IntPtr)Memory.ReadMemory<int>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, (int)value);
        }
        public float FX_Hall_Gain
        {
            get => Memory.ReadMemory<float>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public float FX_Hall_Time
        {
            get => Memory.ReadMemory<float>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        public bool Failed
        {
            get => Memory.ReadMemory<bool>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public float[] InternVars
        {
            get => Memory.ReadMemoryStructArray<float>(Address + 0x20);
        }
        public uint StartTime
        {
            get => Memory.ReadMemory<uint>(Address + 0x24);
            set => Memory.WriteMemory(Address + 0x24, value);
        }
        public int BufferSize
        {
            get => Memory.ReadMemory<int>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public float Prev_Volume
        {
            get => Memory.ReadMemory<float>(Address + 0x2c);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }
        public int FreqVar
        {
            get => Memory.ReadMemory<int>(Address + 0x30);
            set => Memory.WriteMemory(Address + 0x30, value);
        }
        public int SampleRate
        {
            get => Memory.ReadMemory<int>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }
        public float RefValue
        {
            get => Memory.ReadMemory<float>(Address + 0x38);
            set => Memory.WriteMemory(Address + 0x38, value);
        }
        public bool Loop
        {
            get => Memory.ReadMemory<bool>(Address + 0x3c);
            set => Memory.WriteMemory(Address + 0x3c, value);
        }
        public bool Pitch
        {
            get => Memory.ReadMemory<bool>(Address + 0x3d);
            set => Memory.WriteMemory(Address + 0x3d, value);
        }
        public bool Triggered
        {
            get => Memory.ReadMemory<bool>(Address + 0x3e);
            set => Memory.WriteMemory(Address + 0x3e, value);
        }
        public bool CheckLoading
        {
            get => Memory.ReadMemory<bool>(Address + 0x3f);
            set => Memory.WriteMemory(Address + 0x3f, value);
        }
        public bool OnlyOne
        {
            get => Memory.ReadMemory<bool>(Address + 0x40);
            set => Memory.WriteMemory(Address + 0x40, value);
        }
        /// <summary>
        /// Only One _ Prohibition?
        /// </summary>
        public bool OnlyOne_Verbot
        {
            get => Memory.ReadMemory<bool>(Address + 0x41);
            set => Memory.WriteMemory(Address + 0x41, value);
        }
        public bool Important
        {
            get => Memory.ReadMemory<bool>(Address + 0x42);
            set => Memory.WriteMemory(Address + 0x42, value);
        }
        public string[] TriggerList
        {
            get => Memory.ReadMemoryStringArray(Address + 0x44);
        }
        public bool StartTrigSnd
        {
            get => Memory.ReadMemory<bool>(Address + 0x48);
            set => Memory.WriteMemory(Address + 0x48, value);
        }
        public byte Flag_Viewpoint
        {
            get => Memory.ReadMemory<byte>(Address + 0x49);
            set => Memory.WriteMemory(Address + 0x49, value);
        }
        public bool Is3D
        {
            get => Memory.ReadMemory<bool>(Address + 0x4a);
            set => Memory.WriteMemory(Address + 0x4a, value);
        }
        public D3DVector SndPos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x4b);
            set => Memory.WriteMemory(Address + 0x4b, value);
        }
        public float VolDist
        {
            get => Memory.ReadMemory<float>(Address + 0x58);
            set => Memory.WriteMemory(Address + 0x58, value);
        }
        public bool HasDir
        {
            get => Memory.ReadMemory<bool>(Address + 0x5c);
            set => Memory.WriteMemory(Address + 0x5c, value);
        }
        public D3DVector Dir
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x5d);
            set => Memory.WriteMemory(Address + 0x5d, value);
        }
        public float VolFaktor
        {
            get => Memory.ReadMemory<float>(Address + 0x6c);
            set => Memory.WriteMemory(Address + 0x6c, value);
        }
        public int[] VolVars
        {
            get => Memory.ReadMemoryStructArray<int>(Address + 0x70);
        }
        public OmsiFuncClass[] VolCurves
        {
            get => Memory.ReadMemoryObjArray<OmsiFuncClass>(Address + 0x74);
        }
        public OmsiBoolClass BoolClass
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x78));
        }
        public bool Playing
        {
            get => Memory.ReadMemory<bool>(Address + 0x7c);
            set => Memory.WriteMemory(Address + 0x7c, value);
        }
        public bool MayPlay
        {
            get => Memory.ReadMemory<bool>(Address + 0x7d);
            set => Memory.WriteMemory(Address + 0x7d, value);
        }
        public bool Stopped_TooFar
        {
            get => Memory.ReadMemory<bool>(Address + 0x7e);
            set => Memory.WriteMemory(Address + 0x7e, value);
        }
        /// <summary>
        /// Pointer to an IDirectSound3DBuffer
        /// </summary>
        public IntPtr Int3D
        {
            get => (IntPtr)Memory.ReadMemory<int>(Address + 0x80);
            set => Memory.WriteMemory(Address + 0x80, (int)value);
        }




    }
}