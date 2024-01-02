using System;

namespace OmsiHook
{
    /// <summary>
    /// Defines a human being.
    /// </summary>
    public class OmsiHumanBeing : OmsiComplMapObj
    {
        internal OmsiHumanBeing(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiHumanBeing() : base() { }

        public string Voice
        {
            get => Memory.ReadMemoryString(Address + 0x264);
            set => Memory.WriteMemory(Address + 0x264, value);
        }
        public float Age
        {
            get => Memory.ReadMemory<float>(Address + 0x268);
            set => Memory.WriteMemory(Address + 0x268, value);
        }
        public float AssHeight
        {
            get => Memory.ReadMemory<float>(Address + 0x26c);
            set => Memory.WriteMemory(Address + 0x26c, value);
        }
        public float Height
        {
            get => Memory.ReadMemory<float>(Address + 0x270);
            set => Memory.WriteMemory(Address + 0x270, value);
        }
        public float FeetDist
        {
            get => Memory.ReadMemory<float>(Address + 0x274);
            set => Memory.WriteMemory(Address + 0x274, value);
        }
        public D3DVector Shoulder
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x278);
            set => Memory.WriteMemory(Address + 0x278, value);
        }
        public D3DVector Elbow
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x284);
            set => Memory.WriteMemory(Address + 0x284, value);
        }
        public D3DVector Carpus
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x290);
            set => Memory.WriteMemory(Address + 0x290, value);
        }
        public D3DVector Finger
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x29c);
            set => Memory.WriteMemory(Address + 0x29c, value);
        }
        public D3DVector Neck
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x2a8);
            set => Memory.WriteMemory(Address + 0x2a8, value);
        }
        public D3DVector Waist
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x2b4);
            set => Memory.WriteMemory(Address + 0x2b4, value);
        }
        public D3DVector Hip
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x2c0);
            set => Memory.WriteMemory(Address + 0x2c0, value);
        }
        public D3DVector Knee
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x2cc);
            set => Memory.WriteMemory(Address + 0x2cc, value);
        }
        /// <summary>
        /// Incriment
        /// </summary>
        public float Anim_Walk_Schritteweite
        {
            get => Memory.ReadMemory<float>(Address + 0x2d8);
            set => Memory.WriteMemory(Address + 0x2d8, value);
        }
        /// <summary>
        /// Arm Swing?
        /// </summary>
        public float Anim_Walk_ArmSchwing
        {
            get => Memory.ReadMemory<float>(Address + 0x2dc);
            set => Memory.WriteMemory(Address + 0x2dc, value);
        }
        /// <summary>
        /// Hips?
        /// </summary>
        public float Anim_Walk_Huefte
        {
            get => Memory.ReadMemory<float>(Address + 0x2e0);
            set => Memory.WriteMemory(Address + 0x2e0, value);
        }
        /// <summary>
        /// Waist?
        /// </summary>
        public float Anim_Walk_Taille
        {
            get => Memory.ReadMemory<float>(Address + 0x2e4);
            set => Memory.WriteMemory(Address + 0x2e4, value);
        }
        public float Anim_Walk_Upper_Arm_Beta
        {
            get => Memory.ReadMemory<float>(Address + 0x2e8);
            set => Memory.WriteMemory(Address + 0x2e8, value);
        }
        public D3DVector UA_L_Vec
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x2ec);
            set => Memory.WriteMemory(Address + 0x2cc, value);
        }
        public D3DVector UA_R_Vec
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x2f8);
            set => Memory.WriteMemory(Address + 0x2cc, value);
        }
        public D3DVector OA_L_Vec
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x304);
            set => Memory.WriteMemory(Address + 0x2cc, value);
        }
        public D3DVector OA_R_Vec
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x310);
            set => Memory.WriteMemory(Address + 0x2cc, value);
        }
        public D3DVector OS_L_Vec
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x31c);
            set => Memory.WriteMemory(Address + 0x2cc, value);
        }
        public D3DVector OS_R_Vec
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x328);
            set => Memory.WriteMemory(Address + 0x2cc, value);
        }
    }
}
