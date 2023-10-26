using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// More advanced data for complex map object instances, such as string vars
    /// </summary>
    public class OmsiComplObjInst : OmsiObject
    {
        internal OmsiComplObjInst(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiComplObjInst() : base() { }

        public bool RefreshSkins_Calc
        {
            get => Memory.ReadMemory<bool>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public int ActLOD
        {
            get => Memory.ReadMemory<int>(Address + 0x8);
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public float Dist
        {
            get => Memory.ReadMemory<int>(Address + 0xc);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public float RelDist
        {
            get => Memory.ReadMemory<int>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public float ScreenSize
        {
            get => Memory.ReadMemory<int>(Address + 0x14);
            set => Memory.WriteMemory(Address + 0x14, value);
        }
        public bool TextureReduction
        {
            get => Memory.ReadMemory<bool>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        public bool Visible
        {
            get => Memory.ReadMemory<bool>(Address + 0x19);
            set => Memory.WriteMemory(Address + 0x19, value);
        }
        public bool RefreshAllFreeTexs
        {
            get => Memory.ReadMemory<bool>(Address + 0x1a);
            set => Memory.WriteMemory(Address + 0x1a, value);
        }
        public float Refresh_OFT
        {
            get => Memory.ReadMemory<float>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public OmsiComplObj ComplObj
        {
            get => new(Memory, Address + 0x20);
        }
        public byte ViewPoint_Opt
        {
            get => Memory.ReadMemory<byte>(Address + 0x24);
            set => Memory.WriteMemory(Address + 0x24, value);
        }
        public byte ViewPoint_Snd
        {
            get => Memory.ReadMemory<byte>(Address + 0x25);
            set => Memory.WriteMemory(Address + 0x25, value);
        }
        public MemArrayPtr<float> PublicVars
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x28));
        }
        public MemArray<OmsiWStringInternal, OmsiWString> StringVars
        {
            get => new(Memory, Address + 0x2c, false);
        }
        /* TODO:
        public OmsiChangeTex[] ChangeTexs
        {
            get => Memory.ReadMemoryObjArray<OmsiChangeTex>(Address + 0x30);
        }
        public OmsiChangeMatl[] ChangeMatls
        {
            get => Memory.ReadMemoryObjArray<OmsiChangeMatl>(Address + 0x34);
        }*/
        /*public OmsiFreeTexInst[] FreeTexInsts
        {
            get => Memory.ReadMemoryObjArray<OmsiFreeTexInst>(Address + 0x38);
        }*/
        /* TODO:
        /// <summary>
        /// Array of Smoke Instances
        /// </summary>
        public OmsiRauchInst[] RauchInsts
        {
            get => Memory.ReadMemoryObjArray<OmsiRauchInst>(Address + 0x3c);
        }*/
        /// <summary>
        /// ! Warning Data Type Mismatch! - Official Data Type: PIDirect3DTexture9
        /// </summary>
        /*public int[] ReflectionTexs
        {
            get => Memory.ReadMemoryStructArray<int>(Address + 0x40);
        }*/
        public bool OFTTexturesLoaded
        {
            get => Memory.ReadMemory<bool>(Address + 0x44);
            set => Memory.WriteMemory(Address + 0x44, value);
        }
        /* TODO:
        public iDirect3DTexture9[] OFTTextures
        {
            get => Memory.ReadMemoryObjArray<iDirect3DTexture9>(Address + 0x48);
        }*/
        /*public string[] OFT_LastString
        {
            get => Memory.ReadMemoryStringArray(Address + 0x4c);
        }*/
        public MemArray<OmsiScriptTextureInternal, OmsiScriptTexture> ScriptTextures
        {
            get => new(Memory, Address + 0x50, true);//Memory.MarshalStructs<OmsiScriptTexture, OmsiScriptTextureInternal>(Memory.ReadMemoryStructArray<OmsiScriptTextureInternal>(Address + 0x50));
        }
        public OmsiScriptTexture[] UseScriptTextures
        {
            get => Memory.MarshalStructs<OmsiScriptTexture, OmsiScriptTextureInternal>(Memory.ReadMemoryStructArray<OmsiScriptTextureInternal>(Memory.ReadMemory<int>(Address + 0x54)));
        }
        public float Spot_Select
        {
            get => Memory.ReadMemory<float>(Address + 0x58);
            set => Memory.WriteMemory(Address + 0x58, value);
        }
        public D3DMatrix Position
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x5c);
            set => Memory.WriteMemory(Address + 0x5c, value);
        }
        public bool RenderMe
        {
            get => Memory.ReadMemory<bool>(Address + 0x9c);
            set => Memory.WriteMemory(Address + 0x9c, value);
        }
        public D3DMatrix Shadow_Matrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x9d);
            set => Memory.WriteMemory(Address + 0x9d, value);
        }
        /* TODO:
        public OmsiAnimSubMeshInst[] AnimSubMeshInsts
        {
            get => Memory.ReadMemoryObjArray<OmsiAnimSubMeshInst>(Address + 0xe0);
        }*/
        /* TODO:
        public OmsiInteriorLightInst[] AnimSubMeshInsts
        {
            get => Memory.ReadMemoryObjArray<OmsiInteriorLightInst>(Address + 0xe4);
        }*/
        public uint LastSkinCalc
        {
            get => Memory.ReadMemory<uint>(Address + 0xe8);
            set => Memory.WriteMemory(Address + 0xe8, value);
        }
        /* TODO:
        public D3DMatrix[] AnimMatrixes
        {
            get => Memory.ReadMemoryStructArray<D3DMatrix>(Address + 0xec);
        }*/
        public bool Simple_Translated
        {
            get => Memory.ReadMemory<bool>(Address + 0xf0);
            set => Memory.WriteMemory(Address + 0xf0, value);
        }

    }
}
