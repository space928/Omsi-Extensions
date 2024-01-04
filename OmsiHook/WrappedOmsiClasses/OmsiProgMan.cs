using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Main Program Manager
    /// </summary>
    public class OmsiProgMan : OmsiObject
    {
        public OmsiProgMan() : base() { }

        internal OmsiProgMan(Memory memory, int address) : base(memory, address) { }

        public bool LastStay
        {
            get => Memory.ReadMemory<bool>(Address + 0x4);
            set => Memory.WriteMemory(Address + 0x4, value);
        }
        public bool HoverObj
        {
            get => Memory.ReadMemory<bool>(Address + 0x5);
            set => Memory.WriteMemory(Address + 0x5, value);
        }
        public bool HoverSpl
        {
            get => Memory.ReadMemory<bool>(Address + 0x6);
            set => Memory.WriteMemory(Address + 0x6, value);
        }
        public bool HoverPaths
        {
            get => Memory.ReadMemory<bool>(Address + 0x7);
            set => Memory.WriteMemory(Address + 0x7, value);
        }
        /* TODO:
        public OmsiAudioMixer AudioMixer
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x8));
        }*/
        /* TODO:
        public D3DText Text2D_Hinweise_V
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0xc));
        }
        public D3DText Text2D_Hinweise_H
        {
            get => new(Memory, Memory.ReadMemory<int>(Address + 0x10));
        }*/
        public OmsiCamera MapCam
        {
            get => Memory.ReadMemoryObject<OmsiCamera>(Address, 0x14, false);
        }
        public D3DVector MapCamTargetPos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x18);
            set => Memory.WriteMemory(Address + 0x18, value);
        }
        /* TODO:
        public OmsiThreadCheckMissingScheduledAIVehicles ThreadCheckMissingScheduledAIVehicles
        {
            get => new(Memory, Memory.ReadMemory<int>(0x24));
        }*/
        public D3DMatrix GlobalMatrix
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0x28);
            set => Memory.WriteMemory(Address + 0x28, value);
        }
        public int Width
        {
            get => Memory.ReadMemory<int>(Address + 0x68);
            set => Memory.WriteMemory(Address + 0x68, value);
        }
        public int Height
        {
            get => Memory.ReadMemory<int>(Address + 0x6c);
            set => Memory.WriteMemory(Address + 0x6c, value);
        }
        public byte InformationDisplay
        {
            get => Memory.ReadMemory<byte>(Address + 0x70);
            set => Memory.WriteMemory(Address + 0x70, value);
        }
        public bool Nacht
        {
            get => Memory.ReadMemory<bool>(Address + 0x71);
            set => Memory.WriteMemory(Address + 0x71, value);
        }
        public int Costs
        {
            get => Memory.ReadMemory<int>(Address + 0x74);
            set => Memory.WriteMemory(Address + 0x74, value);
        }
        public bool IsSaved
        {
            get => Memory.ReadMemory<bool>(Address + 0x78);
            set => Memory.WriteMemory(Address + 0x78, value);
        }
        public bool Dsgn_Rast_Shift
        {
            get => Memory.ReadMemory<bool>(Address + 0x79);
            set => Memory.WriteMemory(Address + 0x79, value);
        }
        public bool Dsgn_Rast_Strg
        {
            get => Memory.ReadMemory<bool>(Address + 0x7a);
            set => Memory.WriteMemory(Address + 0x7a, value);
        }
        public float MoveSpeed
        {
            get => Memory.ReadMemory<float>(Address + 0x7c);
            set => Memory.WriteMemory(Address + 0x7c, value);
        }
        public int Chrono_Scen
        {
            get => Memory.ReadMemory<int>(Address + 0x80);
            set => Memory.WriteMemory(Address + 0x80, value);
        }
        public D3DVector MausPos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x84);
            set => Memory.WriteMemory(Address + 0x84, value);
        }
        public D3DVector LastMausPos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x90);
            set => Memory.WriteMemory(Address + 0x90, value);
        }
        public D3DVector MausDiff
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x9c);
            set => Memory.WriteMemory(Address + 0x9c, value);
        }
        public bool Maus_Drag
        {
            get => Memory.ReadMemory<bool>(Address + 0xa8);
            set => Memory.WriteMemory(Address + 0xa8, value);
        }
        public bool Maus_Clicked
        {
            get => Memory.ReadMemory<bool>(Address + 0xa9);
            set => Memory.WriteMemory(Address + 0xa9, value);
        }
        public OmsiMouseButton Maus_Button
        {
            get => (OmsiMouseButton)Memory.ReadMemory<byte>(Address + 0xaa);
            set => Memory.WriteMemory(Address + 0xaa, value);
        }
        public D3DVector MausLine3DPos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0xab);
            set => Memory.WriteMemory(Address + 0xab, value);
        }
        public D3DVector MausLine3DDir
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0xb7);
            set => Memory.WriteMemory(Address + 0xb7, value);
        }
        public D3DVector MausLine3DOnEarth
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0xc3);
            set => Memory.WriteMemory(Address + 0xc3, value);
        }
        public bool MausCrossEarth
        {
            get => Memory.ReadMemory<bool>(Address + 0xcf);
            set => Memory.WriteMemory(Address + 0xcf, value);
        }
        public D3DVector MausLine3DObjFlat
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0xd0);
            set => Memory.WriteMemory(Address + 0xd0, value);
        }
        public bool MausCrossObjFlat
        {
            get => Memory.ReadMemory<bool>(Address + 0xdc);
            set => Memory.WriteMemory(Address + 0xdc, value);
        }
        public float MausCrossObjFlat_ObjHeight
        {
            get => Memory.ReadMemory<float>(Address + 0xe0);
            set => Memory.WriteMemory(Address + 0xe0, value);
        }
        public int MausCrossKachel
        {
            get => Memory.ReadMemory<int>(Address + 0xe4);
            set => Memory.WriteMemory(Address + 0xe4, value);
        }
        public string Maus_MeshEvent
        {
            get => Memory.ReadMemoryString(Address + 0xe8);
            set => Memory.WriteMemory(Address + 0xe8, value);
        }
        public OmsiMouseButton Maus_Last_Button
        {
            get => (OmsiMouseButton)Memory.ReadMemory<byte>(Address + 0xec);
            set => Memory.WriteMemory(Address + 0xec, value);
        }
        public bool Maus_Last_Clicked
        {
            get => Memory.ReadMemory<bool>(Address + 0xed);
            set => Memory.WriteMemory(Address + 0xed, value);
        }
        public OmsiFileObject SelObjPri
        {
            get => Memory.ReadMemoryObject<OmsiFileObject>(Address, 0xf0, false);
        }
        public OmsiFileObject SelObjSec
        {
            get => Memory.ReadMemoryObject<OmsiFileObject>(Address, 0xf4, false);
        }
        public OmsiFileObject OrgObj
        {
            get => Memory.ReadMemoryObject<OmsiFileObject>(Address, 0xf8, false);
        }
        public int SelObjEscInst
        {
            get => Memory.ReadMemory<int>(Address + 0xfc);
            set => Memory.WriteMemory(Address + 0xfc, value);
        }
        public OmsiFileSpline SelSplPri
        {
            get => Memory.ReadMemoryObject<OmsiFileSpline>(Address, 0x100, false);
        }
        public OmsiFileSpline SelSplSec
        {
            get => Memory.ReadMemoryObject<OmsiFileSpline>(Address, 0x104, false);
        }
        public OmsiFileSpline OrgSpl
        {
            get => Memory.ReadMemoryObject<OmsiFileSpline>(Address, 0x108, false);
        }
        public OmsiFileSpline[] SelSplExp
        {
            get => Memory.ReadMemoryObjArray<OmsiFileSpline>(Address + 0x10c);
        }
        public OmsiSnapPosition SelSnpA
        {
            get => Memory.ReadMemory<OmsiSnapPosition>(Memory.ReadMemory<int>(Address + 0x110));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x110), value);
        }
        public OmsiSnapPosition SelSnpB
        {
            get => Memory.ReadMemory<OmsiSnapPosition>(Memory.ReadMemory<int>(Address + 0x114));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x114), value);
        }
        public OmsiSnapPosition Snap_Nrst
        {
            get => Memory.ReadMemory<OmsiSnapPosition>(Memory.ReadMemory<int>(Address + 0x118));
            set => Memory.WriteMemory(Memory.ReadMemory<int>(Address + 0x118), value);
        }
        public int Snap_SelPnt
        {
            get => Memory.ReadMemory<int>(Address + 0x11c);
            set => Memory.WriteMemory(Address + 0x11c, value);
        }
        public float SplitPos
        {
            get => Memory.ReadMemory<float>(Address + 0x120);
            set => Memory.WriteMemory(Address + 0x120, value);
        }
        public MemArray<OmsiPathID> HovPath
        {
            get => new(Memory, Address + 0x124);
        }
        public OmsiPathRuleIdents Path_ActiveRule
        {
            get => (OmsiPathRuleIdents)Memory.ReadMemory<byte>(Address + 0x128);
            set => Memory.WriteMemory(Address + 0x128, value);
        }
        public float Path_ActiveRuleValue
        {
            get => Memory.ReadMemory<float>(Address + 0x12c);
            set => Memory.WriteMemory(Address + 0x12c, value);
        }
        public bool Path_Desel
        {
            get => Memory.ReadMemory<bool>(Address + 0x130);
            set => Memory.WriteMemory(Address + 0x130, value);
        }
        public bool Path_Deact
        {
            get => Memory.ReadMemory<bool>(Address + 0x131);
            set => Memory.WriteMemory(Address + 0x131, value);
        }
        public D3DMeshFileObject SplHelper
        {
            get => Memory.ReadMemoryObject<D3DMeshFileObject>(Address, 0x134, false);
        }
        public D3DOBB SplHelperBounding
        {
            get => Memory.ReadMemory<D3DOBB>(Address + 0x138);
            set => Memory.WriteMemory(Address + 0x138, value);
        }
        public OmsiDynHelperMan DynHelperMan
        {
            get => Memory.ReadMemoryObject<OmsiDynHelperMan>(Address, 0x174, false);
        }
        /* TODO:
        public OmsiSplineObject PathHelperSplines
        {
            get => new(Memory, Memory.ReadMemory<int>(0x178));
        }*/
        /// <summary>
        /// Pointers to IDirect3DTexture9
        /// </summary>
        public IntPtr[] PathHelperTextures
        {
            get => new[] {
                new IntPtr(Memory.ReadMemory<int>(Address + 0x17c)),
                new IntPtr(Memory.ReadMemory<int>(Address + 0x180)),
                new IntPtr(Memory.ReadMemory<int>(Address + 0x184)),
                new IntPtr(Memory.ReadMemory<int>(Address + 0x188)),
                new IntPtr(Memory.ReadMemory<int>(Address + 0x18c)),
                new IntPtr(Memory.ReadMemory<int>(Address + 0x190)),
                new IntPtr(Memory.ReadMemory<int>(Address + 0x194)),
                new IntPtr(Memory.ReadMemory<int>(Address + 0x198))
                };
        }
        public OmsiVisu_OBB Visu_OBB
        {
            get => Memory.ReadMemoryObject<OmsiVisu_OBB>(Address, 0x19c, false);
        }
        public float Coll_Pos_X_Var
        {
            get => Memory.ReadMemory<float>(Address + 0x1a0);
            set => Memory.WriteMemory(Address + 0x1a0, value);
        }
        public float Coll_Pos_Y_Var
        {
            get => Memory.ReadMemory<float>(Address + 0x1a4);
            set => Memory.WriteMemory(Address + 0x1a4, value);
        }
        public float Coll_Pos_Z_Var
        {
            get => Memory.ReadMemory<float>(Address + 0x1a8);
            set => Memory.WriteMemory(Address + 0x1a8, value);
        }
        public float Coll_Energy_Var
        {
            get => Memory.ReadMemory<float>(Address + 0x1ac);
            set => Memory.WriteMemory(Address + 0x1ac, value);
        }
        public OmsiObjBlackRect ScreenRect
        {
            get => Memory.ReadMemoryObject<OmsiObjBlackRect>(Address, 0x1b0, false);
        }
        // TODO: Write internal structs for these so that they can be marhsalled
        /*public OmsiCriticalSection CS_MakeVehicle
        {
            get => Memory.ReadMemory<OmsiCriticalSection>(Address + 0x1b4);
            set => Memory.WriteMemory(Address + 0x1b4, value);
        }
        public OmsiCriticalSection CS_TexUse
        {
            get => Memory.ReadMemory<OmsiCriticalSection>(Address + 0x1d4);
            set => Memory.WriteMemory(Address + 0x1d4, value);
        }
        public OmsiCriticalSection CS_ODE
        {
            get => Memory.ReadMemory<OmsiCriticalSection>(Address + 0x1f4);
            set => Memory.WriteMemory(Address + 0x1f4, value);
        }*/
        public bool FPS_ShowedAllReady
        {
            get => Memory.ReadMemory<bool>(Address + 0x214);
            set => Memory.WriteMemory(Address + 0x214, value);
        }
        public float FPS_Time_All
        {
            get => Memory.ReadMemory<float>(Address + 0x218);
            set => Memory.WriteMemory(Address + 0x218, value);
        }
        public float FPS_Time_BelowLimit
        {
            get => Memory.ReadMemory<float>(Address + 0x21c);
            set => Memory.WriteMemory(Address + 0x21c, value);
        }
    }
}
