using System;
using System.Collections.Generic;

namespace OmsiHook
{
    /// <summary>
    /// Base class for complex map object instances - such as vehicle instances and human instances
    /// </summary>
    public class OmsiComplMapObjInst : OmsiPhysObjInst
    {
        internal OmsiComplMapObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiComplMapObjInst() : base() { }

        public uint IDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x1e4);
            set => Memory.WriteMemory(Address + 0x1e4, value);
        }
        public OmsiObject MyFileObject
        {
            get => new(Memory, Address + 0x1e8);
        }
        public float EinsVar
        {
            get => Memory.ReadMemory<float>(Address + 0x1ec);
            set => Memory.WriteMemory(Address + 0x1ec, value);
        }
        public float Debug // TODO: Check Data Type
        {
            get => Memory.ReadMemory<float>(Address + 0x1f0);
            set => Memory.WriteMemory(Address + 0x1f0, value);
        }
        public float Unknown_OCMOI_A // TODO: Check Data Name
        {
            get => Memory.ReadMemory<float>(Address + 0x1f4);
            set => Memory.WriteMemory(Address + 0x1f4, value);
        }
        public float Unknown_OCMOI_B // TODO: Check Data Name
        {
            get => Memory.ReadMemory<float>(Address + 0x1f8);
            set => Memory.WriteMemory(Address + 0x1f8, value);
        }
        public float Unknown_OCMOI_C // TODO: Check Data Name
        {
            get => Memory.ReadMemory<float>(Address + 0x1fc);
            set => Memory.WriteMemory(Address + 0x1fc, value);
        }
        public float UUnknown_OCMOI_D // TODO: Check Data Name
        {
            get => Memory.ReadMemory<float>(Address + 0x200);
            set => Memory.WriteMemory(Address + 0x200, value);
        }
        public float Unknown_OCMOI_E // TODO: Check Data Name
        {
            get => Memory.ReadMemory<float>(Address + 0x204);
            set => Memory.WriteMemory(Address + 0x204, value);
        }
        public string SoundItent
        {
            get => Memory.ReadMemoryString(Memory.ReadMemory<int>(Address + 0x208), true);
        }
        public float FarbSchema
        {
            get => Memory.ReadMemory<float>(Address + 0x20c);
            set => Memory.WriteMemory(Address + 0x20c, value);
        }
        public OmsiComplMapObj ComplMapObj
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x210));
        }

        public OmsiComplObjInst ComplObjInst
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x214));
        }
        public bool UseSound
        {
            get => Memory.ReadMemory<bool>(Address + 0x218);
            set => Memory.WriteMemory(Address + 0x218, value);
        }
        public OmsiSoundPack SoundPack
        {
            get => new(Memory, Address + 0x21c);
        }
        public OmsiCamera[] ReflCameras
        {
            get => Memory.ReadMemoryObjArray<OmsiCamera>(Address + 0x220);
        }
        public byte Selected
        {
            get => Memory.ReadMemory<byte>(Address + 0x224);
            set => Memory.WriteMemory(Address + 0x224, value);
        }
        public D3DColorValue AmbLight
        {
            get => Memory.ReadMemory<D3DColorValue>(Address + 0x225);
            set => Memory.WriteMemory(Address + 0x225, value);
        }
        public MemArray<OmsiFloatPtrInternal, OmsiFloatPtr> PublicVars_Int
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x238), false);
        }
        public MemArray<OmsiFloatPtrInternal, OmsiFloatPtr> PublicVars
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x23c), false);
        }
        public OmsiComplMapObjInst ScriptShareParent
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x240));
        }
        public float[] UserVars
        {
            get => Memory.ReadMemoryStructArray<float>(Address + 0x244);
        }
        public int MyPathGroup
        {
            get => Memory.ReadMemory<int>(Address + 0x248);
            set => Memory.WriteMemory(Address + 0x248, value);
        }
        public float OutsideVol
        {
            get => Memory.ReadMemory<float>(Address + 0x24c);
            set => Memory.WriteMemory(Address + 0x24c, value);
        }
        public bool CollFeedback_Activ
        {
            get => Memory.ReadMemory<bool>(Address + 0x250);
            set => Memory.WriteMemory(Address + 0x250, value);
        }
        public OmsiCollFeedback[] CollFeedbacks
        {
            get => Memory.ReadMemoryStructArray<OmsiCollFeedback>(Address + 0x254);
        }

        /// <summary>
        /// Get a float variable for an object from its name.
        /// </summary>
        /// <param name="varName">Variable Name</param>
        /// <returns>requested float value</returns>
        /// <exception cref="KeyNotFoundException"/>
        public float GetVariable(string varName)
        {
            int index = ComplMapObj.VarStrings[varName];
            if (index < PublicVars.Count && index >= 0)
                return this.PublicVars[index].Float;
            else
                throw new KeyNotFoundException($"Variable '{varName}' not found in object. - Index Out Of Bounds");
        }

        /// <summary>
        /// Set a float variable for an object to a value from its name.
        /// </summary>
        /// <param name="varName">Variable Name</param>
        /// <param name="value">Desired Value</param>
        /// <exception cref="KeyNotFoundException"/>
        public void SetVariable(string varName, float value)
        {
            int index = ComplMapObj.VarStrings[varName];
            if (index < PublicVars.Count && index >= 0)
                this.PublicVars[index] = new(value);
            else
                throw new KeyNotFoundException($"Variable '{varName}' not found in object. - Index Out Of Bounds");
        }

        /// <summary>
        /// Get a string variable for an object from its name.
        /// </summary>
        /// <param name="varName">Variable Name</param>
        /// <returns>requested float value</returns>
        /// <exception cref="KeyNotFoundException"/>
        public string GetStringVariable(string varName)
        {
            int index = ComplMapObj.SVarStrings[varName];
            if (index < ComplObjInst.StringVars.Count && index >= 0)
                return ComplObjInst.StringVars[index].String;
            else
                throw new KeyNotFoundException($"String Variable '{varName}' not found in object. - Index Out Of Bounds");
        }

        /// <summary>
        /// Set a string variable for an object to a value from its name.
        /// </summary>
        /// <param name="varName">Variable Name</param>
        /// <param name="value">Desired Value</param>
        /// <exception cref="KeyNotFoundException"/>
        public void SetStringVariable(string varName, string value)
        {
            int index = ComplMapObj.SVarStrings[varName];
            if (index < ComplObjInst.StringVars.Count && index >= 0)
                ComplObjInst.StringVars[index] = new(value);
            else
                throw new KeyNotFoundException($"String Variable '{varName}' not found in object. - Index Out Of Bounds");
        }
    }
}