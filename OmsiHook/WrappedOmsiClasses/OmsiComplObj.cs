using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// More advanced data for complex map objects, such as terrain holes and freetextures
    /// </summary>
    public class OmsiComplObj : OmsiObject
    {
        internal OmsiComplObj(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        public OmsiComplObj() : base() { }

        public bool Lighting_Human
        {
            get => Memory.ReadMemory<bool>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        // TODO: Check this data type, should be a TStringList
        // https://github.com/space928/Omsi-Extensions/issues/129
        /*public string[] FileText
        {
            get => Memory.ReadMemoryStringArray(Address + 0x8);
            //set => Memory.WriteMemory(Address + 0x4, value);
        }*/
        public string Code
        {
            get => Memory.ReadMemoryString(Address + 0xc, StrPtrType.DelphiAnsiString);
            set => Memory.WriteMemory(Address + 0xc, value);
        }
        public D3DVector VFDMax
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x10);
            set => Memory.WriteMemory(Address + 0x10, value);
        }
        public D3DVector VFDMin
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x1c);
            set => Memory.WriteMemory(Address + 0x1c, value);
        }
        public string FileName
        {
            get => Memory.ReadMemoryString(Address + 0x28, StrPtrType.DelphiAnsiString);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public string MyPath
        {
            get => Memory.ReadMemoryString(Address + 0x2c, StrPtrType.DelphiAnsiString);
            set => Memory.WriteMemory(Address + 0x2c, value);
        }
        public int FileVersion
        {
            get => Memory.ReadMemory<int>(Address + 0x30);
            set => Memory.WriteMemory(Address + 0x30, value);
        }
        public int FileVersion_Hole
        {
            get => Memory.ReadMemory<int>(Address + 0x34);
            set => Memory.WriteMemory(Address + 0x34, value);
        }
        public MemArrayList<OmsiAnimSubMesh> Meshes
        {
            get => new(Memory, Address + 0x38);
        }
        /* TODO:
         * https://github.com/space928/Omsi-Extensions/issues/129
        public OmsiTexChangeMaster[] TexChangeMasters
        {
            get => Memory.ReadMemoryObjArray<OmsiTexChangeMaster>(Address + 0x3c);
        }*/
        /* TODO:
         * https://github.com/space928/Omsi-Extensions/issues/129
        public OmsiMatlChangeMaster[] TexChangeMasters
        {
            get => Memory.ReadMemoryObjArray<OmsiMatlChangeMaster>(Address + 0x40);
        }*/
        public MemArrayOList<OmsiFreeTex, OmsiFreeTex> FreeTexs
        {
            get => new(Memory, Address + 0x44);
        }
        public int Refls
        {
            get => Memory.ReadMemory<int>(Address + 0x48);
            set => Memory.WriteMemory(Address + 0x48, value);
        }
        public MemArrayOList<OmsiLOD, OmsiLOD> LODs
        {
            get => new(Memory, Address + 0x4c);
        }
        public MemArrayOList<OmsiSpot, OmsiSpot> Spots
        {
            get => new(Memory, Address + 0x50);
        }
        /* TODO:
         * https://github.com/space928/Omsi-Extensions/issues/129
        public MemArrayList<OmsiCTCSet, OmsiCTCSet> CTCs
        {
            get => new(Memory, Address + 0x54);
        }*/
        public MemArrayOList<OmsiOFTTexInternal, OmsiOFTTex> OFTTex
        {
            get => new(Memory, Address + 0x58);
        }
        public MemArrayOList<OmsiScriptTex, OmsiScriptTex> ScriptTex
        {
            get => new(Memory, Address + 0x5c);
        }
        public int LabelCount
        {
            get => Memory.ReadMemory<int>(Address + 0x60);
            set => Memory.WriteMemory(Address + 0x60, value);
        }
        public D3DOBB VisualBox
        {
            get => Memory.ReadMemory<D3DOBB>(Address + 0x64);
            set => Memory.WriteMemory(Address + 0x64, value);
        }
        public float Visual_Radius
        {
            get => Memory.ReadMemory<float>(Address + 0xa0);
            set => Memory.WriteMemory(Address + 0xa0, value);
        }
        public float Tex_Detail
        {
            get => Memory.ReadMemory<float>(Address + 0xa4);
            set => Memory.WriteMemory(Address + 0xa4, value);
        }
        public float Detail_Factor
        {
            get => Memory.ReadMemory<float>(Address + 0xa8);
            set => Memory.WriteMemory(Address + 0xa8, value);
        }
        public bool OtherMapRendering
        {
            get => Memory.ReadMemory<bool>(Address + 0xac);
            set => Memory.WriteMemory(Address + 0xac, value);
        }
        public bool NoDistanceCheck
        {
            get => Memory.ReadMemory<bool>(Address + 0xad);
            set => Memory.WriteMemory(Address + 0xad, value);
        }
        public bool Inc_SkinMesh
        {
            get => Memory.ReadMemory<bool>(Address + 0xae);
            set => Memory.WriteMemory(Address + 0xae, value);
        }
        public string TerrainHole_FileName
        {
            get => Memory.ReadMemoryString(Address + 0xb0, StrPtrType.DelphiAnsiString);
            set => Memory.WriteMemory(Address + 0xb0, value);
        }
        public bool GotAllTerrainHoles
        {
            get => Memory.ReadMemory<bool>(Address + 0xb4);
            set => Memory.WriteMemory(Address + 0xb4, value);
        }
        /* TODO:
         * https://github.com/space928/Omsi-Extensions/issues/129
        public OmsiTessPolygon[] TerrainHoles
        {
            get => Memory.ReadMemoryStructArray<OmsiTessPolygon>(Address + 0xb8);
        }*/
        public bool SimpleObject
        {
            get => Memory.ReadMemory<bool>(Address + 0xbc);
            set => Memory.WriteMemory(Address + 0xbc, value);
        }
    }
}
