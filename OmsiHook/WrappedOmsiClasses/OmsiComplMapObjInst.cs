using System;
using System.Collections.Generic;

namespace OmsiHook
{
    /// <summary>
    /// Base class for complex map object instances - such as vehicle instances and human instances
    /// </summary>
    public class OmsiComplMapObjInst : OmsiPhysObjInst
    {
        private MemArrayStringDict varStrings;
        private MemArrayPtr<float> publicVars;
        private MemArrayStringDict sVarStrings;
        private MemArrayStringDict constStrings;
        private MemArrayStringDict funcsStrings;
        private OmsiFuncClass[] funcs;
        private float[] consts;
        private MemArray<OmsiWStringInternal, OmsiWString> stringVars;

        internal OmsiComplMapObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiComplMapObjInst() : base() { }

        internal override void InitObject(Memory memory, int address)
        {
            base.InitObject(memory, address);

            varStrings = ComplMapObj.VarStrings;
            publicVars = PublicVars;
            sVarStrings = ComplMapObj.SVarStrings;
            stringVars = ComplObjInst.StringVars;
            constStrings = ComplMapObj.Script_ConstBlock.Consts_str;
            funcsStrings = ComplMapObj.Script_ConstBlock.Funcs_str;
            consts = ComplMapObj.Script_ConstBlock.Consts;
            funcs = ComplMapObj.Script_ConstBlock.Funcs;
        }

        public uint IDCode
        {
            get => Memory.ReadMemory<uint>(Address + 0x1e4);
            set => Memory.WriteMemory(Address + 0x1e4, value);
        }
        public OmsiObject MyFileObject
        {
            get => Memory.ReadMemoryObject<OmsiObject>(Address, 0x1e8);
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
            get => Memory.ReadMemoryObject<OmsiComplMapObj>(Address, 0x210, false);
        }

        public OmsiComplObjInst ComplObjInst
        {
            get => Memory.ReadMemoryObject<OmsiComplObjInst>(Address, 0x214, false);
        }
        public bool UseSound
        {
            get => Memory.ReadMemory<bool>(Address + 0x218);
            set => Memory.WriteMemory(Address + 0x218, value);
        }
        public OmsiSoundPack SoundPack
        {
            get => Memory.ReadMemoryObject<OmsiSoundPack>(Address, 0x21c, false);
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
        public MemArrayPtr<float> PublicVars_Int
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x238), false);
        }
        public MemArrayPtr<float> PublicVars
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x23c), false);
        }
        public OmsiComplMapObjInst ScriptShareParent
        {
            get => Memory.ReadMemoryObject<OmsiComplMapObjInst>(Address, 0x240, false);
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
            int index = varStrings[varName];
            if (index >= publicVars.Count || index < 0)
                throw new KeyNotFoundException($"Variable '{varName}' not found in object. - Index Out Of Bounds");
            publicVars.UpdateFromHook(index);
            return publicVars[index];
        }

        /// <summary>
        /// Set a float variable for an object to a value from its name.
        /// </summary>
        /// <param name="varName">Variable Name</param>
        /// <param name="value">Desired Value</param>
        /// <exception cref="KeyNotFoundException"/>
        public void SetVariable(string varName, float value)
        {
            int index = varStrings[varName];
            if (index >= publicVars.Count || index < 0)
                throw new KeyNotFoundException($"Variable '{varName}' not found in object. - Index Out Of Bounds");
            publicVars[index] = value; 
        }

        /// <summary>
        /// Get a string variable for an object from its name.
        /// </summary>
        /// <param name="varName">Variable Name</param>
        /// <returns>requested float value</returns>
        /// <exception cref="KeyNotFoundException"/>
        public string GetStringVariable(string varName)
        {
            int index = sVarStrings[varName];
            if (index >= stringVars.Count || index < 0)
                throw new KeyNotFoundException($"String Variable '{varName}' not found in object. - Index Out Of Bounds");
            stringVars.UpdateFromHook(index);
            return stringVars[index].String;
        }

        /// <summary>
        /// Set a string variable for an object to a value from its name.
        /// </summary>
        /// <param name="varName">Variable Name</param>
        /// <param name="value">Desired Value</param>
        /// <exception cref="KeyNotFoundException"/>
        public void SetStringVariable(string varName, string value)
        {
            int index = sVarStrings[varName];
            if (index >= stringVars.Count || index < 0)
                throw new KeyNotFoundException($"String Variable '{varName}' not found in object. - Index Out Of Bounds");
            stringVars[index] = new(value);
        }

        /// <summary>
        /// Get a constant for an object from its name.
        /// </summary>
        /// <param name="varName">Const Name</param>
        /// <returns>requested float value</returns>
        /// <exception cref="KeyNotFoundException"/>
        public float GetConst(string varName)
        {
            int index = constStrings[varName];
            if (index >= consts.Length || index < 0)
                throw new KeyNotFoundException($"Const Variable '{varName}' not found in object. - Index Out Of Bounds");
            return consts[index];
        }

        /// <summary>
        /// Get a value at a point for a predifined curve for an object from its name.
        /// </summary>
        /// <param name="varName">Const Name</param>
        /// <param name="x">X Coordinate to read on curve</param>
        /// <returns>requested float value</returns>
        /// <exception cref="KeyNotFoundException"/>
        public float GetCurve(string varName, float x)
        {
            int index = funcsStrings[varName];
            if (index >= consts.Length || index < 0)
                throw new KeyNotFoundException($"Curve Variable '{varName}' not found in object. - Index Out Of Bounds");

            var curve = funcs[index].Pnts;
            for (int i = 0; i < curve.Length - 1; i++)
            {
                float x1 = curve[i].x;
                float y1 = curve[i].y;

                float x2 = curve[i + 1].x;
                float y2 = curve[i + 1].y;

                if (x >= x1 && x <= x2)
                {
                    // Perform linear interpolation
                    float y = y1 + (x - x1) * (y2 - y1) / (x2 - x1);
                    return y;
                }
            }
            if (x <= curve[0].x)
                return curve[0].y;
            if (x >= curve[curve.Length-1].x)
                return curve[curve.Length - 1].y;
            return float.NaN;
        }

        /// <inheritdoc cref="OmsiRemoteMethods.OmsiSoundTrigger(OmsiComplMapObjInst, string, string)"/>
        public async void SoundTrigger(string trigger, string filename)
        {
            await Memory.RemoteMethods.OmsiSoundTrigger(this, trigger, filename);
        }
    }
}