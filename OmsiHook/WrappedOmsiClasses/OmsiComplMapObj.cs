﻿using System;
using System.Collections.Generic;

namespace OmsiHook
{
    /// <summary>
    /// Base class for complex map objects - used by vehicles and humans
    /// </summary>
    public class OmsiComplMapObj : OmsiPhysObj
    {
        internal OmsiComplMapObj(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiComplMapObj() : base() { }

        public string[] Scripts_Int => Memory.ReadMemoryStringArray(Address + 0x17c);
        public string[] Vars_Int => Memory.ReadMemoryStringArray(Address + 0x180);
        public string[] SVars_Int => Memory.ReadMemoryStringArray(Address + 0x184);
        public string[] ConstFiles_Int => Memory.ReadMemoryStringArray(Address + 0x188);
        public string Model_Int
        {
            get => Memory.ReadMemoryString(Address + 0x18c);
            set => Memory.WriteMemory(Address + 0x18c, value);
        }
        public OmsiPassengerCabin PassengerCabin => Memory.ReadMemoryObject<OmsiPassengerCabin>(Address, 0x190, false);
        public bool AbsHeight
        {
            get => Memory.ReadMemory<bool>(Address + 0x194);
            set => Memory.WriteMemory(Address + 0x194, value);
        }
        public bool FoundInProtokoll
        {
            get => Memory.ReadMemory<bool>(Address + 0x195);
            set => Memory.WriteMemory(Address + 0x195, value);
        }
        public bool Exists
        {
            get => Memory.ReadMemory<bool>(Address + 0x196);
            set => Memory.WriteMemory(Address + 0x196, value);
        }
        public string[] Groups => Memory.ReadMemoryStringArray(Address + 0x198);
        public string FriendlyName
        {
            get => Memory.ReadMemoryString(Address + 0x19c);
            set => Memory.WriteMemory(Address + 0x19c, value);
        }
        public string StdColorScheme
        {
            get => Memory.ReadMemoryString(Address + 0x1a0);
            set => Memory.WriteMemory(Address + 0x1a0, value);
        }
        public string Model
        {
            get => Memory.ReadMemoryString(Address + 0x1a4);
            set => Memory.WriteMemory(Address + 0x1a4, value);
        }
        public string MyPath
        {
            get => Memory.ReadMemoryString(Address + 0x1a8);
            set => Memory.WriteMemory(Address + 0x1a8, value);
        }
        public string Sound
        {
            get => Memory.ReadMemoryString(Address + 0x1ac);
            set => Memory.WriteMemory(Address + 0x1ac, value);
        }
        public string Sound_AI
        {
            get => Memory.ReadMemoryString(Address + 0x1b0);
            set => Memory.WriteMemory(Address + 0x1b0, value);
        }
        public float Pause
        {
            get => Memory.ReadMemory<float>(Memory.ReadMemory<int>(Address + 0x1b4));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x1b4), value);
        }
        public float NoSound
        {
            get => Memory.ReadMemory<float>(Memory.ReadMemory<int>(Address + 0x1b8));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x1b8), value);
        }
        public byte Complexity // Byte maybe?
        {
            get => Memory.ReadMemory<byte>(Address + 0x1bc);
            set => Memory.WriteMemory(Address + 0x1bc, value);
        }
        public bool Joinable
        {
            get => Memory.ReadMemory<bool>(Address + 0x1bd);
            set => Memory.WriteMemory(Address + 0x1bd, value);
        }
        public float NullVar
        {
            get => Memory.ReadMemory<float>(Memory.ReadMemory<int>(Address + 0x1c0));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x1c0), value);
        }
        public OmsiFileObjectSpecial Special
        {
            get => Memory.ReadMemory<OmsiFileObjectSpecial>(Address + 0x1c4);
            set => Memory.WriteMemory(Address + 0x1c4, value);
        }
        public bool Depot
        {
            get => Memory.ReadMemory<bool>(Address + 0x1c5);
            set => Memory.WriteMemory(Address + 0x1c5, value);
        }
        public D3DMeshFileObject HeightDeform => Memory.ReadMemoryObject<D3DMeshFileObject>(Address, 0x1c8);
        public string HeightDeform_FileName
        {
            get => Memory.ReadMemoryString(Address + 0x1cc);
            set => Memory.WriteMemory(Address + 0x1cc, value);
        }
        public int Switch_State_Count
        {
            get => Memory.ReadMemory<int>(Address + 0x1d0);
            set => Memory.WriteMemory(Address + 0x1d0, value);
        }
        // TODO: Implement internal struct for OmsiTreeInfo
        /*public OmsiTreeInfo TreeInfo
        {
            get => Memory.ReadMemory<OmsiTreeInfo>(Address + 0x1d4);
            set => Memory.WriteMemory(Address + 0x1d4, value);
        }*/
        public bool OnlyEditor
        {
            get => Memory.ReadMemory<bool>(Address + 0x1e8);
            set => Memory.WriteMemory(Address + 0x1e8, value);
        }
        public OmsiMapRenderPriority Priority
        {
            get => Memory.ReadMemory<OmsiMapRenderPriority>(Address + 0x1e9);
            set => Memory.WriteMemory(Address + 0x1e9, value);
        }
        private MemArrayStringDict varStrings;
        /// <summary>
        /// Array of names of float variables.
        /// </summary>
        /// <remarks>
        /// The value of this property is automatically cached for performance.
        /// </remarks>
        public MemArrayStringDict VarStrings => varStrings ??= new(Memory, Address + 0x1ec, true);
        private MemArrayStringDict sVarStrings;
        /// <summary>
        /// Array of names of string variables.
        /// </summary>
        /// <remarks>
        /// The value of this property is automatically cached for performance.
        /// </remarks>
        public MemArrayStringDict SVarStrings => sVarStrings ??= new(Memory, Address + 0x1f0, true);
        private MemArrayStringDict sysVarStrings;
        /// <summary>
        /// Array of names of system variables.
        /// </summary>
        /// <remarks>
        /// The value of this property is automatically cached for performance.
        /// </remarks>
        public MemArrayStringDict SysVarStrings => sysVarStrings ??= new(Memory, Address + 0x1f4, true);
        private MemArrayStringDict callBackStrings;
        /// <summary>
        /// Array of names of callbacks.
        /// </summary>
        /// <remarks>
        /// The value of this property is automatically cached for performance.
        /// </remarks>
        public MemArrayStringDict CallBackStrings => callBackStrings ??= new(Memory, Address + 0x1f8, true);
        private MemArrayPtr<float> sysVars;
        /// <summary>
        /// Array of values of system variables.
        /// </summary>
        /// <remarks>
        /// The value of this property is automatically cached for performance.
        /// </remarks>
        public MemArrayPtr<float> SysVars => sysVars ??= new(Memory, Address + 0x1fc);
        public bool ScriptShare
        {
            get => Memory.ReadMemory<bool>(Address + 0x200);
            set => Memory.WriteMemory(Address + 0x200, value);
        }
        public bool VB_Couple_Front_Open_For_Sound
        {
            get => Memory.ReadMemory<bool>(Address + 0x201);
            set => Memory.WriteMemory(Address + 0x201, value);
        }
        public int FirstUser
        {
            get => Memory.ReadMemory<int>(Address + 0x204);
            set => Memory.WriteMemory(Address + 0x204, value);
        }
        /* TODO:
        public OmsiXPC_CreateReturn Script_Return
        {
            get => Memory.ReadMemory<OmsiXPC_CreateReturn>(Address + 0x208);
            set => Memory.WriteMemory(Address + 0x208, value);
        } */

        public OmsiConstBlock Script_ConstBlock
        {
            get => Memory.ReadMemoryObject<OmsiConstBlock>(Address, 0x220, false);
        }
        public D3DMatrix[] AttatchmentPnts
        {
            get => Memory.ReadMemoryStructArray<D3DMatrix>(Address + 0x224);
        }
        public OmsiMapLight[] MapLights
        {
            get => Memory.ReadMemoryStructArray<OmsiMapLight>(Address + 0x228);
        }
        public bool MapLighting
        {
            get => Memory.ReadMemory<bool>(Address + 0x22c);
            set => Memory.WriteMemory(Address + 0x22c, value);
        }
        public bool LightMapMapping
        {
            get => Memory.ReadMemory<bool>(Address + 0x22d);
            set => Memory.WriteMemory(Address + 0x22d, value);
        }
        public int NightMapMode
        {
            get => Memory.ReadMemory<int>(Address + 0x22e);
            set => Memory.WriteMemory(Address + 0x22e, value);
        }
        public OmsiAmpelGroup AmpelGroup
        {
            get => Memory.MarshalStruct<OmsiAmpelGroup, OmsiAmpelGroupInternal>(
                Memory.ReadMemory<OmsiAmpelGroupInternal>(Address + 0x230));
            //TODO:
            //set => Memory.WriteMemory(Address + 0x230, value);
        }
        public int CTC_FarbSchema
        {
            get => Memory.ReadMemory<int>(Address + 0x244);
            set => Memory.WriteMemory(Address + 0x244, value);
        }
        public OmsiTriggerBox[] TriggerBoxes => Memory.ReadMemoryStructArray<OmsiTriggerBox>(Address + 0x248);
        
        /* TODO:
        public OmsiComplObjPtr ComplObj
        {
            get => Memory.ReadMemory<OmsiComplObjPtr>(Address + 0x24c);
            set => Memory.WriteMemory(Address + 0x24c, value);
        }*/
        public OmsiPathManager ComplObj
        {
            get => Memory.ReadMemoryObject<OmsiPathManager>(Address, 0x250, false);
        }
        public MemArray<OmsiObjectPathInfoInternal, OmsiObjectPathInfo> Paths
        {
            get => new(Memory, Address + 0x254);
        }
        public OmsiSnapPosition[] SnapPoints
        {
            get => Memory.ReadMemoryStructArray<OmsiSnapPosition>(Address + 0x258);
        }
        public OmsiCameraSettings[] ReflCameraSettings
        {
            get => Memory.ReadMemoryStructArray<OmsiCameraSettings>(Address + 0x25c);
        }
        public MemArray<OmsiSplineHelperInternal, OmsiSplineHelper> OmsiSplineHelpers
        {
            get => new(Memory, Address + 0x260);
        }
    }
}
