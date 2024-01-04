using System;
using System.Numerics;
using System.Runtime.InteropServices;

#pragma warning disable CS0649

namespace OmsiHook
{
    /// <summary>
    /// Defines a simple 3 dimensional float vector.
    /// </summary>
    public struct D3DVector
    {
        public float x, y, z;

        public D3DVector() 
        { 
            x = y = z = 0;
        }
        public D3DVector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override readonly string ToString() => $"[{x,8:F3}, {y,8:F3}, {z,8:F3}]";

        public static implicit operator Vector3(D3DVector v) => new(v.x, v.y, v.z);
        public static implicit operator D3DVector(Vector3 v) => new() { x = v.X, y = v.Y, z = v.Z };
    }
    public struct D3DXVector2
    {
        public float x, y;

        public override readonly string ToString() => $"[{x,8:F3}, {y,8:F3}]";
    }

    /// <summary>
    /// Defines a 4x4 float matrix.
    /// </summary>
    public struct D3DMatrix
    {
        public float _00, _01, _02, _03,
                     _10, _11, _12, _13,
                     _20, _21, _22, _23,
                     _30, _31, _32, _33;
        public override readonly string ToString() => $"[ [{_00,8:F3}, {_01,8:F3}, {_02,8:F3}, {_03,8:F3}],\n" +
            $"[{_10,8:F3}, {_11,8:F3}, {_12,8:F3}, {_13,8:F3}],\n" +
            $"[{_20,8:F3}, {_21,8:F3}, {_22,8:F3}, {_23,8:F3}],\n" +
            $"[{_30,8:F3}, {_31,8:F3}, {_32,8:F3}, {_33,8:F3}] ]";

        public readonly D3DVector Position => new(_30, _31, _32);

        public static implicit operator Matrix4x4(D3DMatrix m)
        {
            return new Matrix4x4()
            {
                M11 = m._00,
                M12 = m._01,
                M13 = m._02,
                M14 = m._03,
                M21 = m._10,
                M22 = m._11,
                M23 = m._12,
                M24 = m._13,
                M31 = m._20,
                M32 = m._21,
                M33 = m._22,
                M34 = m._23,
                M41 = m._30,
                M42 = m._31,
                M43 = m._32,
                M44 = m._33
            };
        }

        public static implicit operator D3DMatrix(Matrix4x4 m)
        {
            return new()
            {
                _00 = m.M11,
                _01 = m.M12,
                _02 = m.M13,
                _03 = m.M14,
                _10 = m.M21,
                _11 = m.M22,
                _12 = m.M23,
                _13 = m.M24,
                _20 = m.M31,
                _21 = m.M32,
                _22 = m.M33,
                _23 = m.M34,
                _30 = m.M41,
                _31 = m.M42,
                _32 = m.M43,
                _33 = m.M44
            };
        }
    }

    /// <summary>
    /// Defines a 4 dimensional float vector.
    /// </summary>
    public struct D3DXQuaternion
    {
        public float x, y, z, w;

        public override readonly string ToString() => $"[{x,8:F3}, {y,8:F3}, {z,8:F3}, {w,8:F3}]";
    }

    /// <summary>
    /// Defines a plane with a normal vector and a distance.
    /// </summary>
    public struct D3DXPlane
    {
        public float a, b, c, d;

        public override readonly string ToString() => $"[{a,8:F3}, {b,8:F3}, {c,8:F3}, {d,8:F3}]";
    }

    /// <summary>
    /// Defines a colour in terms of r,g,b,a as floats.
    /// </summary>
    public struct D3DColorValue
    {
        public float r, g, b, a;

        public override readonly string ToString() => $"[{r,8:F3}, {g,8:F3}, {b,8:F3}, {a,8:F3}]";
    }

    /// <summary>
    /// Defines a standard DirectX9 material.
    /// </summary>
    public struct D3DMaterial9
    {
        public D3DColorValue diffuse, ambient, specular, emissive;
        public float power;
    }

    public struct D3DOBB
    {
        // TODO: Check Axis Data Type
        public int Axis;
        public D3DVector Depth;
        public D3DVector Center;
    }

    /// <summary>
    /// Defines a 2 dimensional int vector
    /// </summary>
    public struct OmsiPoint
    {
        public int x, y;
    }

    public struct OmsiMaterialPropInternal
    {
        public int mainTexture;
        public bool standard;
        public bool useTexCoordTransX, useTexCoordTransY;
        public bool dirtScale, alphaScale;
        public bool useRealTimeReflx, useNightMap, useLightMap, useTerrainLightMap, useReflxMap;
        public bool horizontal;
        public bool water;
        public bool useEnvirReflx, useEnvirMask, useBumpMap, useRainDropAreaMap,
            useTransMap;
        public int useTextTexture, useScriptTexture;
        public int texCoordTransX, texCoordTransY;
        public int raindropAreaMapVar;
        public int alphaScale_Var;
        public bool reflxMapUsesAlpha;
        public byte reflxMap;
        public int nightMap;
        public int nightMapScale;
        [OmsiPtr] public int lightMap;
        [OmsiPtr] public int lightMapVars;
        public int envMap;
        public int envMaskMap;
        public float envFactor;
        public int bumpMap;
        public float bumpFactor;
        public int alphaMap;
        public byte alpha;
        public bool noZWrite;
        public bool noZCheck;
        public int zBias;
        public byte address;
        public uint borderColor;
        public D3DMaterial9 d3dMaterial;
        [OmsiPtr] public int freeTexs;
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int shaderBlock;
        public bool splineScaleByLength;
    }

    /// <summary>
    /// Stores all the properties of an Omsi material
    /// </summary>
    public struct OmsiMaterialProp
    {
        public int mainTexture;
        public bool standard;
        public bool useTexCoordTransX, useTexCoordTransY;
        public bool dirtScale, alphaScale;
        public bool useRealTimeReflx, useNightMap, useLightMap, useTerrainLightMap, useReflxMap;
        public bool horizontal;
        public bool water;
        public bool useEnvirReflx, useEnvirMask, useBumpMap, useRainDropAreaMap,
            useTransMap;
        public int useTextTexture, useScriptTexture;
        public int texCoordTransX, texCoordTransY;
        public int raindropAreaMapVar;
        public int alphaScale_Var;
        public bool reflxMapUsesAlpha;
        public byte reflxMap;
        public int nightMap;
        public int nightMapScale;
        /// <summary>
        /// Pointer to an array of lightmaps. 
        /// The array is of type int.
        /// </summary>
        public IntPtr lightMap;
        /// <summary>
        /// Pointer to an array of lightmap vars. 
        /// The array is of type int.
        /// </summary>
        public IntPtr lightMapVars;
        public int envMap;
        public int envMaskMap;
        public float envFactor;
        public int bumpMap;
        public float bumpFactor;
        public int alphaMap;
        public byte alpha;
        public bool noZWrite;
        public bool noZCheck;
        public int zBias;
        public byte address;
        public uint borderColor;
        public D3DMaterial9 d3dMaterial;
        /// <summary>
        /// Pointer to an array of freeTexs. 
        /// The array is of type int.
        /// </summary>
        public IntPtr freeTexs;
        public string shaderBlock;
        public bool splineScaleByLength;
    }

    internal struct OmsiMapKachelInfoInternal
    {
        public OmsiPoint position;
        [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int filename;
        public int mapKachel;
    }

    public struct OmsiMapKachelInfo
    {
        public OmsiPoint position;
        public string filename;
        public int mapKachel;
    }

    public struct OmsiMapSeason
    {
        public OmsiSeason season;
        public short start_day;
        public short end_day;
    }

    public enum OmsiSeason : short
    {
        SsnSummer,
        SsnSpring,
        SsnAutumn,
        SsnWinter,
        SsnWinterSnow,
        SsnSummerDry
    }

    public enum OmsiFileObjectSpecial : byte
    {
        FOS_Normal,
        FOS_Busstop,
        FOS_PeopleStandingRandom,
        FOS_EntryPoint,
        FOS_TrafficLight,
        FOS_Signal,
        FOS_Tree,
        FOS_CarPark_P,
        FOS_HelpArrow,
        FOS_Switch,
        FOS_Invalid
    }

    public enum OmsiMapRenderPriority : byte
    {
        TMRP_PreSurfaces,
        TMRP_Surfaces,
        TMRP_OnSurfaces,
        TMRP_1,
        TMRP_2,
        TMRP_3,
        TMRP_4
    }

    public enum OmsiDuplicates : byte
    {
        dupIgnore,
        dupAccept,
        dupError
    }

    internal struct OmsiGroundTypeInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int texture;
        [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int det_texture;
        public int tex_id;
        public int det_tex_id;
        public byte resolution;
        public byte repeating, repeating_det;
        public D3DMatrix matrix, detMatrix;
    }

    public struct OmsiGroundType
    {
        public string texture;
        public string det_texture;
        public int tex_id;
        public int det_tex_id;
        public byte resolution;
        public byte repeating, repeating_det;
        public D3DMatrix matrix, detMatrix;
    }

    public struct OmsiStandardMapCam
    {
        public OmsiPoint centerKachel;
        public D3DVector mapCamTargetPos;
        public float angle_hdg;
        public float angle_hgt;
        public float dist;
    }

    internal struct OmsiWeatherPropInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name; // STRING
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int description; // STRING
        public float fogDensity;
        public float lightness;
        public float windSpeed;
        public float windDirection;
        public float temperature;
        public float dewPoint;
        public float pressure;
        public byte percipitation;
        public byte percipRate;
        public D3DVector percipVec;
        [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int cloudType; // ANSI STRING
        public float cloudHeight;
        public int cloudTexture;
        public float bodenfeuchte;
        public float bodenfeuchte_bedeckung;
        public float bodennaesse;
        public bool schnee;
        public bool schneeAufStrassen;
    }

    public struct OmsiWeatherProp
    {
        public string name; // STRING
        public string description; // STRING
        public float fogDensity;
        public float lightness;
        public float windSpeed;
        public float windDirection;
        public float temperature;
        public float dewPoint;
        public float pressure;
        public byte percipitation;
        public byte percipRate;
        public D3DVector percipVec;
        public string cloudType; // ANSI STRING
        public float cloudHeight;
        public int cloudTexture;
        /// <summary>
        /// Ground Moisture
        /// </summary>
        public float bodenfeuchte;
        /// <summary>
        /// Ground Moisture Coverage
        /// </summary>
        public float bodenfeuchte_bedeckung;
        /// <summary>
        /// Ground Wet?
        /// </summary>
        public float bodennaesse;
        /// <summary>
        /// Snow
        /// </summary>
        public bool schnee;
        /// <summary>
        /// Snow On Roads
        /// </summary>
        public bool schneeAufStrassen;
    }

    internal struct OmsiCloudTypeInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int name; // ANSI String
        [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int texFile; // ANSI String
        public float texSize;
        public bool ovc;
    }

    public struct OmsiCloudType
    {
        public string name; // ANSI String
        public string texFile; // ANSI String
        public float texSize;
        /// <summary>
        /// Overcast?
        /// </summary>
        public bool ovc;
    }

    internal struct OmsiTicketPackInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int filename;
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int voicepath;
        [OmsiStructArrayPtr(typeof(OmsiTicket), typeof(OmsiTicketInternal), true)]
        public int tickets;
        public float stamper_prop;
        public float ticketBuy_prop;
        public float chattiness;
        public float whinge_prop;
    }

    public struct OmsiTicketPack
    {
        public string filename;
        public string voicepath;
        public OmsiTicket[] tickets;
        public float stamper_prop;
        public float ticketBuy_prop;
        public float chattiness;
        public float whinge_prop;
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x2c)]
    internal struct OmsiTicketInternal
    {
        //TODO: I don't think these are decoding correctly (I saw nothing when I looked), check this actually works.
        [FieldOffset(0x0)][OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int name;
        [FieldOffset(0x4)][OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int name_english;
        [FieldOffset(0x8)][OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int name_display;
        [FieldOffset(0xc)] public int max_stations;
        [FieldOffset(0x10)] public int age_min;
        [FieldOffset(0x14)] public int age_max;
        [FieldOffset(0x18)] public float value;
        [FieldOffset(0x1c)] public bool dayTicket;
        [FieldOffset(0x20)] public float propability;
        [FieldOffset(0x24)][OmsiObjPtr(typeof(D3DMeshFileObject))] public int mesh_block;
        [FieldOffset(0x28)][OmsiObjPtr(typeof(D3DMeshFileObject))] public int mesh_single;
    }

    public struct OmsiTicket
    {
        public string name, name_english, name_display;
        public int max_stations;
        public int age_min, age_max;
        public float value;
        public bool dayTicket;
        public float propability;
        public D3DMeshFileObject mesh_block;
        public D3DMeshFileObject mesh_single;
    }
    public struct OmsiSeat
    {
        public D3DVector position;
        public D3DXQuaternion rotation;
        public byte flag;
        public float height;
        public int getInteriorLights; // TODO: Check Data Type
    }
    public struct OmsiEntryProp
    {
        public bool noTicketSale;
        public bool withButton;
    }
    public struct OmsiPassCabinStamper
    {
        public OmsiPathPointBasic point;
        public D3DVector stamperPos;
        public bool valid;
    }
    internal struct OmsiPassCabinStamperInternal
    {
        [OmsiObjPtr(typeof(OmsiPathPointBasic))] public int point;
        public D3DVector stamperPos;
        public bool valid;
    }
    public struct OmsiPassCabinTicketSale
    {
        public OmsiPathPointBasic point;
        public D3DVector ticketPos;
        public D3DVector moneyPos;
        public D3DVector moneyPos_var;
        public int moneyPos_parent_idx;
        public string moneyPos_parent_str; // ANSIString
        public D3DVector changePos;
        public D3DVector changePos_var;
        public int changePos_parent_idx;
        public string changePos_parent_str; // ANSIString
        public bool valid;
    }
    internal struct OmsiPassCabinTicketSaleInternal
    {
        [OmsiObjPtr(typeof(OmsiPathPointBasic))] public int point;
        public D3DVector ticketPos;
        public D3DVector moneyPos;
        public D3DVector moneyPos_var;
        public int moneyPos_parent_idx;
        [OmsiStrPtr(StrPtrType.DelphiAnsiString)] public int moneyPos_parent_str; // ANSIString
        public D3DVector changePos;
        public D3DVector changePos_var;
        public int changePos_parent_idx;
        [OmsiStrPtr(StrPtrType.DelphiAnsiString)] public int changePos_parent_str; // ANSIString
        public bool valid;
    }
    public struct OmsiTreeInfo
    {
        public string texture;
        public float min_height;
        public float max_height;
        public float min_ratio;
        public float max_ratio;
    }
    public struct OmsiMapLight
    {
        public D3DVector position;
        public float r, g, b, radius;

        public double x, y, z;
    }
    public struct OmsiVector3Double
    {
        public double x, y, z;
    }

    /// <summary>
    /// Omsi axel instance
    /// </summary>
    public struct OmsiAchseInstance
    {
        //TODO: Translate field names
        public double phi; //TODO: Verify all double types
        public float alpha;
        public double drehzahl;
        public double federung;
        public OmsiVector3Double anpress; //TODO: Verify type
        public float contact;
        public float antrieb;
        public double brems;
        public double schlupf;
        public float bremShaft;
        public double mue_str_gl;
        public double mue_str_hft;
        public double puddle_depth;
        public double surface;
        public double surface_var;
        public double springFactor;
        public double brakeForce;
        public double ph_Wheel;
        public double ph_wheel_geom;
        public double ph_joint;
        public int lastSchiene;
    }

    internal struct OmsiUnschedVehGroupInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int aiList;
        public int aiList_index;
        public int defaultDensity;
        public float densityFactor;
        public float act_trafficDensity;
        [OmsiPtr] public int func_trafficDensity_weekday;
        [OmsiPtr] public int func_trafficDensity_saturday;
        [OmsiPtr] public int func_trafficDensity_sunday;
    }

    public struct OmsiUnschedVehGroup
    {
        public string aiList;
        public int aiList_index;
        public int defaultDensity;
        public float densityFactor;
        public float act_trafficDensity;
        public IntPtr func_trafficDensity_weekday;
        public IntPtr func_trafficDensity_saturday;
        public IntPtr func_trafficDensity_sunday;
    }

    internal struct OmsiAIListInternal
    {
        [OmsiStrArrayPtr] public int allVeh;
        public int randomGroup;
        [OmsiStructArrayPtr(typeof(OmsiAIGroup), typeof(OmsiAIGroupInternal))]
        public int groups;
    }

    public struct OmsiAIList
    {
        public string[] allVeh;
        public int randomGroup;
        public OmsiAIGroup[] groups;
    }

    internal struct OmsiAIGroupInternal
    {
        [OmsiStrPtr] public int ident;
        [OmsiStructArrayPtr(typeof(OmsiAIGroupType), typeof(OmsiAIGroupTypeInternal))]
        public int typeEntries;
        [OmsiStrPtr] public int hof;
        public bool fixedList;
        public int nonUsed_count;
    }

    public struct OmsiAIGroup
    {
        public string ident;
        public OmsiAIGroupType[] typeEntries;
        public string hof;
        public bool fixedList;
        public int nonUsed_count;
    }

    internal struct OmsiAIGroupTypeInternal
    {
        public int veh;
        public int nonUsed_count;
        [OmsiStructArrayPtr(typeof(OmsiAIGroupTypeNumber), typeof(OmsiAIGroupTypeNumberInternal))]
        public int numbers;
    }

    public struct OmsiAIGroupType
    {
        public int veh;
        public int nonUsed_count;
        public OmsiAIGroupTypeNumber[] numbers;
    }

    internal struct OmsiAIGroupTypeNumberInternal
    {
        [OmsiStrPtr] public int num, reg, werbung;
        public bool used;
    }

    public struct OmsiAIGroupTypeNumber
    {
        public string num, reg, werbung;
        public bool used;
    }

    internal struct OmsiHolidayInternal
    {
        public int date;
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name;
    }

    public struct OmsiHoliday
    {
        public int date;
        public string name;
    }

    internal struct OmsiHolidaysInternal
    {
        public int start, ende;
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name;
    }

    public struct OmsiHolidays
    {
        public int start, ende;
        public string name;
    }

    public struct OmsiDST
    {
        public int start_day, ende_day;
        public float start_time, ende_time, diff;
    }

    public struct OmsiPathID
    {
        public int kachel, path;
    }

    internal struct OmsiPathBelegungInternal
    {
        public float xmin, xmax, zmin, zmax;
        [OmsiStructPtr(typeof(OmsiPathInfo), typeof(OmsiPathInfoInternal))]
        public int pathInfoPntr;
        public uint idCode;
    }

    /// <summary>
    /// Path assignment?
    /// </summary>
    public struct OmsiPathBelegung
    {
        public float xmin, xmax, zmin, zmax;
        public OmsiPathInfo pathInfoPntr;
        public uint idCode;
    }

    //TODO: Bug when dereferencing this
    [StructLayout(LayoutKind.Explicit, Size = 0x110)]
    internal struct OmsiPathInfoInternal
    {
        [FieldOffset(0x0)] public byte veh_type;
        [FieldOffset(0x1)] public byte uvg;
        [FieldOffset(0x2)] public bool railroad;
        [FieldOffset(0x3)] public bool freeOrLarge;
        [FieldOffset(0x4)] public D3DVector pathPos;
        [FieldOffset(0x10)] public OmsiPathID path;
        [FieldOffset(0x18)] public int subPath;
        [FieldOffset(0x1c)] public bool reverse;
        [FieldOffset(0x20)] public float hdg;
        [FieldOffset(0x24)][OmsiStructPtr(typeof(uint))] public int idCode;
        [FieldOffset(0x28)][OmsiObjPtr(typeof(OmsiObject))] public int vehicle;
        [FieldOffset(0x2c)] public float veloc;
        [FieldOffset(0x30)] public float x_L;
        [FieldOffset(0x34)] public float x_R;
        [FieldOffset(0x38)] public float z_B;
        [FieldOffset(0x3c)] public float z_F;
        [FieldOffset(0x40)] public float radius;
        [FieldOffset(0x44)] public float drehpunkt;
        [FieldOffset(0x48)][OmsiStructPtr(typeof(D3DMatrix))] public int absPosition;
        [FieldOffset(0x4c)] public byte waitMode;
        [FieldOffset(0x50)] public int reserveGroup;
        [FieldOffset(0x54)] public byte blinker;
        [FieldOffset(0x55)] public byte einOrdnen;//TODO: Check data type
        [FieldOffset(0x56)] public bool prevVisible_logical;
        //TODO: Determine actual type
        [FieldOffset(0x58)] public int nextPath_a;
        [FieldOffset(0x5c)] public int nextPath_b;
        [FieldOffset(0x60)] public int nextPath_c;
        [FieldOffset(0x64)] public int nextPath_d;
        [FieldOffset(0x68)] public int nextPath_e;
        [FieldOffset(0x6c)] public OmsiNextPathSegment nextPathSeg;
        [FieldOffset(0x94)] public OmsiNextPathID nextPathID;
        [FieldOffset(0xbc)][OmsiStructArrayPtr(typeof(OmsiPathID), raw: true)] public int reservePaths;
        [FieldOffset(0xc0)] public int spurwechsel; //TODO: Check data type
        [FieldOffset(0xc4)] public uint spurwechselTime;
        [FieldOffset(0xc8)] public bool spurwechselAuto;
        [FieldOffset(0xc9)] public bool noScheduledSpurwechsel;
        [FieldOffset(0xca)] public bool on_crossing;
        [FieldOffset(0xcc)] public float leftFree;
        [FieldOffset(0xd0)] public float rightFree;
        [FieldOffset(0xd4)] public byte relPosError; //TODO: Check data type
        [FieldOffset(0xd5)] public bool setPosOnNearTile;
        [FieldOffset(0xd8)] public float rowdy_factor;
        [FieldOffset(0xdc)] public bool eilig;
        [FieldOffset(0xdd)] public bool einSatzFahrzeug;
        [FieldOffset(0xe0)] public float martinshorn;
        [FieldOffset(0xe4)] public float getrieben_von_einSatz;
        [FieldOffset(0xe8)] public uint getrieben_von_einSatz_time;
        [FieldOffset(0xec)] public byte setSoll;
        [FieldOffset(0xf0)] public int track;
        [FieldOffset(0xf4)] public int trackEntry;
        [FieldOffset(0xf8)] public int stnLink;
        [FieldOffset(0xfc)] public int next_stnLink;
        [FieldOffset(0x100)] public bool zwangsEinsetzen;
        [FieldOffset(0x104)] public float normBrakedDist;
        [FieldOffset(0x108)] public bool allowHupen;
        [FieldOffset(0x10c)][OmsiStrPtr(StrPtrType.RawDelphiString)] public int debug_aiData_limit;
    }

    public struct OmsiPathInfo
    {
        public byte veh_type;
        public byte uvg;
        public bool railroad;
        public bool freeOrLarge;
        public D3DVector pathPos;
        public OmsiPathID path;
        public int subPath;
        public bool reverse;
        public float hdg;
        public uint idCode;
        public OmsiObject vehicle;
        /// <summary>
        /// Vehicle location
        /// </summary>
        public float veloc;
        public float x_L, x_R, z_B, z_F;
        public float radius;
        /// <summary>
        /// Pivot point
        /// </summary>
        public float drehpunkt;
        public D3DMatrix absPosition;
        public byte waitMode;
        public int reserveGroup;
        public byte blinker;
        /// <summary>
        /// Order
        /// </summary>
        public byte einOrdnen;//TODO: Check data type
        public bool prevVisible_logical;
        //TODO: Determine actual type
        public int nextPath_a, nextPath_b, nextPath_c, nextPath_d, nextPath_e;
        public OmsiNextPathSegment nextPathSeg;
        public OmsiNextPathID nextPathID;
        public OmsiPathID[] reservePaths;
        /// <summary>
        /// Lane change
        /// </summary>
        public int spurwechsel; //TODO: Check data type
        /// <summary>
        /// Lane change time
        /// </summary>
        public uint spurwechselTime;
        /// <summary>
        /// Lane change auto
        /// </summary>
        public bool spurwechselAuto;
        /// <summary>
        /// No scheduled lane change
        /// </summary>
        public bool noScheduledSpurwechsel;
        public bool on_crossing;
        public float leftFree, rightFree;
        public byte relPosError; //TODO: Check data type
        public bool setPosOnNearTile;
        public float rowdy_factor;
        /// <summary>
        /// Hurried
        /// </summary>
        public bool eilig;
        /// <summary>
        /// Emergency vehicles
        /// </summary>
        public bool einSatzFahrzeug;
        /// <summary>
        /// Siren
        /// </summary>
        public float martinshorn;
        /// <summary>
        /// Driven by emergency?
        /// </summary>
        public float getrieben_von_einSatz;
        /// <summary>
        /// Driven by emergency time?
        /// </summary>
        public uint getrieben_von_einSatz_time;
        /// <summary>
        /// Set target
        /// </summary>
        public byte setSoll;
        public int track;
        public int trackEntry;
        public int stnLink;
        public int next_stnLink;
        /// <summary>
        /// Forced insertion?
        /// </summary>
        public bool zwangsEinsetzen;
        public float normBrakedDist;
        /// <summary>
        /// Allow horns
        /// </summary>
        public bool allowHupen;
        public string debug_aiData_limit;
    }

    /// <summary>
    /// Temporary struct to store unknown data type in OmsiPathInfo.nextPathSeg
    /// </summary>
    public struct OmsiNextPathSegment
    {
        public int a, b, c, d, e, f, g, h, i, j;
    }

    /// <summary>
    /// Temporary struct to store unkown data type in OmsiPathInfo.nextPathID
    /// </summary>
    public struct OmsiNextPathID
    {
        public OmsiPathID a, b, c, d, e;
    }

    internal struct OmsiPathGroupReservInternal
    {
        [OmsiStruct(typeof(OmsiPathInfo), typeof(OmsiPathInfoInternal))]
        public OmsiPathInfoInternal pathInfo;
    }

    public struct OmsiPathGroupReserv
    {
        public OmsiPathInfo pathInfo;
    }

    public struct OmsiPathGroupBlocking
    {
        public OmsiPathID blockingPath;
        public int blockedPathRel;
        public byte param;
        public byte active;
    }

    internal struct OmsiAmpelGroupInternal
    {
        [OmsiStructArrayPtr(typeof(OmsiAmpel))]
        public int ampeln;
        [OmsiStructArrayPtr(typeof(OmsiAmpelStop))]
        public int stops;
        public float zyklus;
        public bool running;
        public float ampelTime;
    }

    /// <summary>
    /// Traffic light group
    /// </summary>
    public struct OmsiAmpelGroup
    {
        /// <summary>
        /// Traffic lights
        /// </summary>
        public OmsiAmpel[] ampeln;
        public OmsiAmpelStop[] stops;
        /// <summary>
        /// Cycle
        /// </summary>
        public float zyklus;
        public bool running;
        public float ampelTime;
    }

    internal struct OmsiAmpelInternal
    {
        [OmsiStrPtr] public int name;
        [OmsiStructArrayPtr(typeof(OmsiAmpelPhase))]
        public int phasen;
        public float actPhase;
        public byte blockingPhase;
        public float anforderung;
        public uint anforderung_frame;
        public float approachDist;
    }

    /// <summary>
    /// Traffic light
    /// </summary>
    public struct OmsiAmpel
    {
        public string name;
        /// <summary>
        /// Phases
        /// </summary>
        public OmsiAmpelPhase[] phasen;
        public float actPhase;
        public byte blockingPhase;
        /// <summary>
        /// Requirement?
        /// </summary>
        public float anforderung;
        /// <summary>
        /// Requirement frame?
        /// </summary>
        public uint anforderung_frame;
        public float approachDist;
    }

    /// <summary>
    /// Traffic light phase
    /// </summary>
    public struct OmsiAmpelPhase
    {
        public float time;
        public byte phase;
    }

    public struct OmsiAmpelStop
    {
        public int ampel;
        public float time;
        public float jumpTime;
        public bool ifAnf;
    }

    public struct OmsiTree
    {
        public D3DVector position;
        public float rotation;
        public OmsiTreeTextureSet texture;
        public D3DMatrix matrix;
        public float width, height;
    }

    public struct OmsiTreeTextureSet
    {
        public int main, winterSnow;
    }

    public struct OmsiObjectPathInfo
    {
        public D3DVector position;
        public float hdg;
        public float invradius;
        public float laenge;
        public float grdnt_strt;
        public float grdnt_end;
        public float neigung_strt;
        public float neigung_end;
        public float deltaHeight;
        public bool use_deltaHeight;
        public byte typ;
        public float width;
        public byte reverse;
        public byte blinker;
        public int ampel;
        public OmsiObjectPathInfo[] blockings;
        public bool crossingProblem;
        public short switchDir;
        public OmsiPathInfoRailEnh[] rail_enh;
        public OmsiThirdRail[] third_rails;
    }

    public struct OmsiObjectPathInfoInternal
    {
        public D3DVector position;
        public float hdg;
        public float invradius;
        public float laenge;
        public float grdnt_strt;
        public float grdnt_end;
        public float neigung_strt;
        public float neigung_end;
        public float deltaHeight;
        public bool use_deltaHeight;
        public byte typ;
        public float width;
        public byte reverse;
        public byte blinker;
        public int ampel;
        [OmsiStructArrayPtr(typeof(OmsiObjectPathInfo))] public int blockings;
        public bool crossingProblem;
        public short switchDir;
        [OmsiStructArrayPtr(typeof(OmsiPathInfoRailEnh))] public int rail_enh;
        [OmsiStructArrayPtr(typeof(OmsiThirdRail))] public int third_rails;
    }

    public struct OmsiThirdRail
    {
        public float pos_x;
        public float pos_z;
        public byte flags;
        public float volt;
        public float freq;
        public float sigA;
    }

    public struct OmsiPathInfoRailEnh
    {
        public float schienenlaenge;
        public bool stoesse;
        public float Gleislage_Wellenlaenge;
        public float Gleislage_Amp;
        public int Gleislage_Pot;
        public float Gleislage_Wellenlaenge_Z;
        public float Gleislage_Amp_Z;
        public int Gleislage_Pot_Z;
    }

    public struct OmsiSnapPosition
    {
        public D3DVector position;
        public float hdg;
        public float steigung;
        public float cant;
        public int parent_obj; // Pointer
        public int parent_spl; // Pointer
        public int kachel;
        public float offsetSpline;
        public float offsetSplineY;
    }

    public struct OmsiCameraSettings
    {
        public float angle_hdg_norm;
        public float angle_hgt_norm;
        public bool constDist;
        public D3DVector pos;
        public float dist;
        public float sichtwinkel;
        public float radius;
    }

    public struct OmsiSplineHelper
    {
        public OmsiSnapPosition pos;
        public string splineType;
    }

    public struct OmsiSplineHelperInternal
    {
        public OmsiSnapPosition pos;
        [OmsiStrPtr] public int splineType;
    }

    public struct OmsiTriggerBox
    {
        public D3DOBB box;
        public bool reverb;
        public float reverb_time;
        public float reverb_dist;
    }

    public struct OmsiAchse
    {
        public float _long;
        public float maxWidth;
        public float minWidth;
        public float feder;
        public float maxForce;
        public float daempfer;
        public float rad_dia;
        /// <summary>
        /// Driven? Powered?
        /// </summary>
        public bool angetrieben;
        public float omega_F;
    }

    public struct OmsiCoupling
    {
        public D3DVector position;
        public OmsiCoupleCC[] cc;
    }
    internal struct OmsiCouplingInternal
    {
        public D3DVector position;
        [OmsiStructArrayPtr(typeof(OmsiCoupleCC))] public int cc;
    }

    public struct OmsiCoupleCC
    {
        public int read_var;
        public int write_var;
        public int couple_var;
    }
    internal struct OmsiVehicleCoupleInternal
    {
        [OmsiStrPtr] public int filename;
        public bool reverse;
    }
    public struct OmsiVehicleCouple
    {
        public string filename;
        public bool reverse;
    }

    public struct OmsiVehicleCoupleChar
    {
        public float alpha_max;
        public float beta_min;
        public float beta_max;
        public int type;
    }

    public struct OmsiContactShoe
    {
        public byte boogie;
        public float x_min;
        public float x_max;
        public float z_min;
        public float z_max;
        public byte flags;
        public int var_volt_veh;
        public int var_volt_rail;
        public int var_volt_freq;
        public int var_x;
        public int var_z;
        public int var_index;
    }

    public struct OmsiHOFTarget
    {
        public int nummer;
        public string name; // ANSI String
        /// <summary>
        /// Terminus
        /// </summary>
        public string endstelle; // ANSI String
        public int texNummer;
        public string[] strings;
        public byte allExit;
    }
    internal struct OmsiHOFTargetInternal
    {
        public int nummer;
        [OmsiStrPtr(raw: true)] public int name; // ANSI String
        /// <summary>
        /// Terminus
        /// </summary>
        [OmsiStrPtr(raw: true)] public int endstelle; // ANSI String
        public int texNummer;
        [OmsiStrArrayPtr(wide: true, raw: true)] public int strings;
        public byte allExit;
    }

    public struct OmsiHofFISBusstop
    {
        public string ident; // ANSI String
        public string[] strings;
    }
    internal struct OmsiHofFISBusstopInternal
    {
        [OmsiStrPtr(raw: true)] public int ident; // ANSI String
        [OmsiStrArrayPtr(wide: true, raw: true)] public int strings;
    }

    public struct OmsiHofFISTrip
    {
        public int code;
        public string name; // ANSI String
        public int target;
        public string line; // ANSI String
        public string[] busstops;
    }
    internal struct OmsiHofFISTripInternal
    {
        public int code;
        [OmsiStrPtr(raw: true)] public int name; // ANSI String
        public int target;
        [OmsiStrPtr(raw: true)] public int line; // ANSI String
        [OmsiStrArrayPtr(raw: true)] public int busstops;
    }
    public struct OmsiCollFeedback
    {
        public D3DVector position;
        public float energie;
    }

    public struct OmsiPointPair
    {
        public float x;
        public float y;
    }

    public struct OmsiPathLink
    {
        public OmsiPathID nextPath;
        public bool nextRev;
    }
    internal struct OmsiPathLinkInternal
    {
        [OmsiStructPtr(typeof(OmsiPathID))] public int nextPath;
        public bool nextRev;
    }

    public struct OmsiPathGroupID
    {
        public int kachel;
        public int pathGroup;
    }

    public struct OmsiPathCrossing
    {
        public OmsiPathID other;
        public bool parallel;
    }
    public struct OmsiPathReservation
    {
        public OmsiPathInfo pathInfo;
        public byte typ;
    }
    internal struct OmsiPathReservationInternal
    {
        [OmsiStructPtr(typeof(OmsiPathInfo))] public int pathInfo;
        public byte typ;
    }

    public struct OmsiWeightData
    {
        public float[] influences;
    }

    internal struct OmsiWeightDataInternal
    {
        [OmsiStructArrayPtrAttribute(typeof(float))] public int influences;
    }

    public struct OmsiFileObjectPathInfo
    {
        public OmsiPathRule[] rules;
    }
    internal struct OmsiFileObjectPathInfoInternal
    {
        [OmsiStructArrayPtr(typeof(OmsiPathRule), typeof(OmsiPathRuleInternal))] public int rules;
    }

    public struct OmsiPathRule
    {
        public short[] trafficDensity_act;
        public float[] trafficDensity;
        public short priority_act;
        public byte priority;
    }

    internal struct OmsiPathRuleInternal
    {
        [OmsiObjArrayPtr(typeof(short))] public int trafficDensity_act;
        [OmsiObjArrayPtr(typeof(float))] public int trafficDensity;
        public short priority_act;
        public byte priority;
    }

    public struct OmsiPathSegmentFStr
    {
        public int fstr;
        public int fstrentry;
    }

    public struct OmsiStringItem
    {
        public string fstring;
        public OmsiObject fobject;
    }

    internal struct OmsiStringItemInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int fstring;
        [OmsiObjPtr(typeof(OmsiObject))] public int fobject;
    }

    public struct OmsiScriptTexture
    {
        /// <summary>
        /// IDirect3DTexture9 - No Marshaling implemented
        /// </summary>
        public IntPtr tex;
        public uint[] TexPn;
        public uint color;
    }

    public struct OmsiScriptTextureInternal
    {
        [OmsiPtr] public int tex; // IDirect3DTexture9 - No Marshaling implemented
        [OmsiStructArrayPtr(typeof(uint))] public int TexPn;
        public uint color;
    }

    public struct OmsiFreeTex
    {
        public int orgTex;
        public int stringVar;
    }

    public struct OmsiPerbus
    {
        public string busname;
        public uint hektometer;
    }
    internal struct OmsiPerbusInternal
    {
        [OmsiStrPtr(StrPtrType.Raw)] public int busname;
        public uint hektometer;
    }

    public struct OmsiDriver
    {
        public string filename;
        public string name;
        public bool gender;
        public double birthday;
        public double dateOfHire;
        public uint cnt_busstop_all;
        public uint cnt_busstop_late;
        public uint cnt_busstop_early;
        public uint hektometer_all;
        public uint cnt_crashs;
        public uint cnt_hitandrun;
        public uint cnt_hitandrun_heavy;
        public uint crashes_ped;
        public double bew_fahrstill; // These look like driver ratings
        public uint bew_passcomfort;
        public uint bew_ticket_count;
        public uint bew_ticket_points;
        public uint passCount;
        public uint ticket_cnt;
        public float tickets_cash;
        public OmsiPerbus[] perbus;
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x68)]
    internal struct OmsiDriverInternal
    {
        [FieldOffset(0x0)][OmsiStrPtr(StrPtrType.RawDelphiString)] public int filename;
        [FieldOffset(0x4)][OmsiStrPtr(StrPtrType.RawDelphiString)] public int name;
        [FieldOffset(0x8)] public bool gender;
        [FieldOffset(0x10)] public double birthday;
        [FieldOffset(0x18)] public double dateOfHire;
        [FieldOffset(0x20)] public uint cnt_busstop_all;
        [FieldOffset(0x24)] public uint cnt_busstop_late;
        [FieldOffset(0x28)] public uint cnt_busstop_early;
        [FieldOffset(0x2c)] public uint hektometer_all;
        [FieldOffset(0x30)] public uint cnt_crashs;
        [FieldOffset(0x34)] public uint cnt_hitandrun;
        [FieldOffset(0x38)] public uint cnt_hitandrun_heavy;
        [FieldOffset(0x3c)] public uint crashes_ped;
        [FieldOffset(0x40)] public double bew_fahrstill; // These look like driver raitings
        [FieldOffset(0x48)] public uint bew_passcomfort;
        [FieldOffset(0x4c)] public uint bew_ticket_count;
        [FieldOffset(0x50)] public uint bew_ticket_points;
        [FieldOffset(0x54)] public uint passCount;
        [FieldOffset(0x58)] public uint ticket_cnt;
        [FieldOffset(0x5c)] public float tickets_cash;
        [FieldOffset(0x60)][OmsiStructArrayPtr(typeof(OmsiPerbus), typeof(OmsiPerbusInternal), true)] public int perbus;
    }
    public struct OmsiTTLogDetailed
    {
        /// <summary>
        /// Busstop
        /// </summary>
        public string busstop_name;
        /// <summary>
        /// estimated arrival
        /// </summary>
        public int eta;
        /// <summary>
        /// estimated departure
        /// </summary>
        public int etd;
        /// <summary>
        /// actual arrival
        /// </summary>
        public int ata;
        /// <summary>
        /// actual departure
        /// </summary>
        public int atd;
        /// <summary>
        /// arrived on time
        /// </summary>
        public byte arr_ok;
        /// <summary>
        /// departed on time
        /// </summary>
        public byte dep_ok;
    }
    public struct OmsiTTLogDetailedInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int busstop_name;
        public int eta;
        public int etd;
        public int ata;
        public int atd;
        public byte arr_ok;
        public byte dep_ok;
    }

    public enum OmsiHumanAIMode : byte
    {
        THAM_Stop, THAM_WalkToTarget, THAM_WaitBeforeTarget, THAM_AtTarget, THAM_TooFar, THAM_WalkToPathTarget, THAM_WaitOnPath, THAM_AtPathTarget, THAM_WalkStreet, THAM_Stand
    }
    public enum OmsiHumanAIModeEX : byte
    {
        THAME_DoNothing, THAME_WaitingForBus, THAME_WalkingToBusPre, THAME_WalkingToBus, THAME_WalkingInBusToPlace, THAME_WalkingInBusToExit, THAME_WalkingToBusstop, THAME_SittingInBus, THAME_WalkStreet, THAME_DrivingBus
    }
    public enum OmsiHumanAISubMode : byte
    {
        None, WaitForStamper, Stamp, WaitForTicketBuy, WaitForGeldabwurf, WaitForTicketAndChange, WaitForTakingTicket, WaitForChange, FinishedBuyingTicket
    }
    public enum OmsiHumanKollision : byte
    {
        THK_Free, THK_TooClose, THK_TooCloseFrontFront, THK_TooCloseFront, THK_TooCloseInside
    }

    public enum OmsiPAIM : byte
    {
        PAIM_Parked, PAIM_Busstop, PAIM_Drive
    }

    public struct OmsiHumanKollisionFreeSide
    {
        public bool left;
        public bool right;
    }

    public struct OmsiTTTrackEntry
    {
        public uint idCode;
        public int pathIndexOnObject;
        public OmsiPathID pathIndex;
        public float relDist;
        public float dist;
        public bool valid;
        public byte pathOrderCheck;
        public int[] fstrn_allowed;
        public int chronon_origin;
        public string[] chronos_bad;
    }
    [StructLayout(LayoutKind.Explicit, Size = 0x28)]
    internal struct OmsiTTTrackEntryInternal
    {
        [FieldOffset(0x0)] public uint idCode;
        [FieldOffset(0x4)] public int pathIndexOnObject;
        [FieldOffset(0x8)] public OmsiPathID pathIndex;
        [FieldOffset(0x10)] public float relDist;
        [FieldOffset(0x14)] public float dist;
        [FieldOffset(0x18)] public bool valid;
        [FieldOffset(0x19)] public byte pathOrderCheck;
        [FieldOffset(0x1c)][OmsiStructArrayPtr(typeof(int), raw: true)] public int fstrn_allowed;
        [FieldOffset(0x20)] public int chronon_origin;
        [FieldOffset(0x24)][OmsiStrArrayPtr(raw: true)] public int chronos_bad;
    }

    public struct OmsiTTTrack
    {
        public string filename;
        public string filepath;
        public OmsiTTTrackEntry[] TrackEntrys;
        /// <summary>
        /// Length
        /// </summary>
        public float laenge;
    }
    internal struct OmsiTTTrackInternal
    {
        [OmsiStrPtr(raw: true)] public int filename;
        [OmsiStrPtr(raw: true)] public int filepath;
        [OmsiStructArrayPtr(typeof(OmsiTTTrackEntry), typeof(OmsiTTTrackEntryInternal), raw: true)] public int TrackEntrys;
        public float laenge; // Length
    }

    public struct OmsiTTBusstop
    {
        public string name;
        /// <summary>
        /// Supliment / Addition
        /// </summary>
        public string name_zusatz;
        public int kachel;
        public uint IDCode_formal;
        public uint IDCode_real;
        public int index;
        public int index_ownList;
        public int[] index_alternatives;
        /// <summary>
        /// Dropouts?
        /// </summary>
        public float preset_Aussteiger;
        public OmsiPathID pathIndex;
        public int trackEntry;
        public byte invalid;
        public float x_path;
        public float dist_relPath;
        public float dist;
    }
    internal struct OmsiTTBusstopInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name;
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name_zusatz; // Supliment / Addition
        public int kachel;
        public uint IDCode_formal;
        public uint IDCode_real;
        public int index;
        public int index_ownList;
        [OmsiStructArrayPtr(typeof(int), raw: true)] public int index_alternatives;
        public float preset_Aussteiger; // Dropouts?
        [OmsiStruct(typeof(OmsiPathID))] public OmsiPathID pathIndex;
        public int trackEntry;
        public byte invalid;
        public float x_path;
        public float dist_relPath;
        public float dist;
    }

    public struct OmsiTTStopTime
    {
        public float arr_time;
        public float dep_time;
        public byte arr_time_man;
        public byte dep_time_man;
        public byte stopping;
    }

    public struct OmsiTTProfile
    {
        public string name;
        public float time_all;
        public OmsiTTStopTime[] stop_times;
        public float[] TrackEntryTime;
        public bool serviceTrip;
    }
    internal struct OmsiTTProfileInternal
    {
        [OmsiStrPtr(raw: true)] public int name;
        public float time_all;
        [OmsiStructArrayPtr(typeof(OmsiTTStopTime), raw: true)] public int stop_times;
        [OmsiStructArrayPtr(typeof(float), raw: true)] public int TrackEntryTime;
        public bool serviceTrip;
    }

    public struct OmsiTTTrip
    {
        public string filename;
        public int chrono_origin;
        public string target;
        public string linie;
        public byte trainRev;
        public byte invalide;
        public OmsiTTProfile[] profiles;
        public OmsiTTBusstop[] busstops;
        public string trackName;
        public int trackIndex;
        public int stnLinkList;
    }
    internal struct OmsiTTTripInternal
    {
        [OmsiStrPtr(raw: true)] public int filename;
        public int chrono_origin;
        [OmsiStrPtr(raw: true)] public int target;
        [OmsiStrPtr(raw: true)] public int linie;
        public byte trainRev;
        public byte invalide;
        [OmsiStructArrayPtr(typeof(OmsiTTProfile), typeof(OmsiTTProfileInternal), raw: true)] public int profiles;
        [OmsiStructArrayPtr(typeof(OmsiTTBusstop), typeof(OmsiTTBusstopInternal), raw: true)] public int busstops;
        [OmsiStrPtr(raw: true)] public int trackName;
        public int trackIndex;
        public int stnLinkList;
    }

    public struct OmsiTTBusstopListEntryChronoRename
    {
        public string name;
        /// <summary>
        /// Supliment / Addition
        /// </summary>
        public string name_zustatz;
        /// <summary>
        /// Dropouts?
        /// </summary>
        public float aussteiger;
        public int chrono_origin;
    }
    internal struct OmsiTTBusstopListEntryChronoRenameInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name;
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name_zustatz; // Supliment / Addition
        public float aussteiger; // Dropouts?
        public int chrono_origin;
    }
    public struct OmsiTTBusstopListEntry
    {
        /// <summary>
        /// Supliment / Addition
        /// </summary>
        public string name_zustatz;
        public string name;
        public int kachel;
        public uint IDCode;
        public uint parent_IDCode;
        /// <summary>
        /// Dropouts?
        /// </summary>
        public float preset_aussteiger;
        public string[] chronos_bad;
        public int index;
        public int[] stnlinks_starting;
        public int[] stnlinks_ending;
        public int chrono_origin;
        public OmsiTTBusstopListEntryChronoRename[] chrono_rename;
    }
    internal struct OmsiTTBusstopListEntryInternal
    {
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name_zustatz; // Supliment / Addition
        [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name;
        public int kachel;
        public uint IDCode;
        public uint parent_IDCode;
        public float preset_aussteiger; // Dropouts?
        [OmsiStrArrayPtr(StrPtrType.RawDelphiString)] public int chronos_bad;
        public int index;
        [OmsiStructArrayPtr(typeof(int), raw: true)] public int stnlinks_starting;
        [OmsiStructArrayPtr(typeof(int), raw: true)] public int stnlinks_ending;
        public int chrono_origin;
        [OmsiStructArrayPtr(typeof(OmsiTTBusstopListEntryChronoRename), typeof(OmsiTTBusstopListEntryChronoRenameInternal))] public int chrono_rename;
    }

    public struct OmsiTTStnLink
    {
        public OmsiTTTrackEntry[] trackEntrys;
        /// <summary>
        /// Length
        /// </summary>
        public float laenge;
        public uint busstop_start_IDCode;
        public uint busstop_end_IDCode;
        public float x_path_Start;
        public float x_path_end;
        public int[] chronos_needed;
        public int[] chronos_bad;
        public int busstop_start;
        public int busstop_end;
        public int chrono_origin;
        public byte valid;
        public int busstop_start_trackentry;
        public int busstop_end_trackentry;
        public float dist_relPath_start;
        public float dist_relPath_end;
        public bool visible;
    }
    internal struct OmsiTTStnLinkInternal
    {
        [OmsiStructArrayPtr(typeof(OmsiTTTrackEntry), typeof(OmsiTTTrackEntryInternal), raw: true)] public int trackEntrys;
        public float laenge; // Length
        public uint busstop_start_IDCode;
        public uint busstop_end_IDCode;
        public float x_path_Start;
        public float x_path_end;
        [OmsiStructArrayPtr(typeof(int), raw: true)] public int chronos_needed;
        [OmsiStructArrayPtr(typeof(int), raw: true)] public int chronos_bad;
        public int busstop_start;
        public int busstop_end;
        public int chrono_origin;
        public byte valid;
        public int busstop_start_trackentry;
        public int busstop_end_trackentry;
        public float dist_relPath_start;
        public float dist_relPath_end;
        public bool visible;
    }

    public struct OmsiTTTourEntry
    {
        public string trip;
        public int tripIndex;
        public int profile;
        public float startTime;
        public float endTime;
        public bool smoothTrans;
    }
    internal struct OmsiTTTourEntryInternal
    {
        [OmsiStrPtr(raw: true)] public int trip;
        public int tripIndex;
        public int profile;
        public float startTime;
        public float endTime;
        public bool smoothTrans;
    }

    public struct OmsiTTTourValid
    {
        public byte mon;
        public byte tue;
        public byte wed;
        public byte thu;
        public byte fri;
        public byte sat;
        public byte sun;
        public byte hol;
        public byte hols;
        public byte noHols;
    }

    public struct OmsiTTTour
    {
        public string name;
        public string aiGroup;
        public int aiType;
        public int aiGroupIndex;
        public string vehicle_nr_reservations;
        public int[] vehical_indizes;
        public byte hasNormalVeh;
        /// <summary>
        /// day done? (Day of week maybe? - OmsiTTTourValid?)
        /// </summary>
        public int TagErledigt;
        public OmsiTTTourValid validOn;
        public byte invalide;
        public OmsiTTTourEntry[] entrys;
    }
    [StructLayout(LayoutKind.Explicit)]
    internal struct OmsiTTTourInternal
    {
        [FieldOffset(0x0)][OmsiStrPtr(raw: true)] public int name;
        [FieldOffset(0x4)][OmsiStrPtr(raw: true)] public int aiGroup;
        [FieldOffset(0x8)] public int aiType;
        [FieldOffset(0xc)] public int aiGroupIndex;
        [FieldOffset(0x10)][OmsiStrPtr(raw: true)] public int vehicle_nr_reservations;
        [FieldOffset(0x14)][OmsiStructArrayPtr(typeof(int), raw: true)] public int vehical_indizes;
        [FieldOffset(0x18)] public byte hasNormalVeh;
        [FieldOffset(0x1c)] public int TagErledigt; // day done? (Day of week maybe? - OmsiTTTourValid?)
        [FieldOffset(0x20)][OmsiStruct(typeof(OmsiTTTourValid))] public OmsiTTTourValid validOn;
        [FieldOffset(0x2a)] public byte invalide;
        [FieldOffset(0x2c)][OmsiStructArrayPtr(typeof(OmsiTTTourEntry), typeof(OmsiTTTourEntryInternal), raw: true)] public int entrys;
    }

    public struct OmsiTTLine
    {
        public string name;
        public byte userAllowed;
        public byte priority;
        public OmsiTTTour[] tours;
        public int chrono_origin;
    }
    [StructLayout(LayoutKind.Explicit)]
    internal struct OmsiTTLineInternal
    {
        [FieldOffset(0x0)][OmsiStrPtr(raw: true)] public int name;
        [FieldOffset(0x4)] public byte userAllowed;
        [FieldOffset(0x5)] public byte priority;
        [FieldOffset(0x8)][OmsiStructArrayPtr(typeof(OmsiTTTour), typeof(OmsiTTTourInternal), raw: true)] public int tours;
        [FieldOffset(0xc)] public int chrono_origin;
    }

    public struct OmsiRVNumTour
    {
        public string number;
        public string tourName;
    }
    internal struct OmsiRVNumTourInternal
    {
        [OmsiStrPtr] public int number;
        [OmsiStrPtr] public int tourName;
    }

    public struct OmsiRVTypeTour
    {
        public string filename;
        public string tourName;
    }
    internal struct OmsiRVTypeTourInternal
    {
        [OmsiStrPtr] public int filename;
        [OmsiStrPtr] public int tourName;
    }

    public struct OmsiRVTypesLine
    {
        public string[] filenames;
        public float probability;
    }
    internal struct OmsiRVTypesLineInternal
    {
        [OmsiStrArrayPtr] public int filnames;
        public float probability;
    }

    public struct OmsiRVFile
    {
        public int startDateRel2000;
        public int endDateRel2000;
        public string line;
        public OmsiRVNumTour[] list_number_tour;
        public OmsiRVTypeTour[] list_type_tour;
        public OmsiRVTypesLine list_types_line;
    }
    internal struct OmsiRVFileInternal
    {
        public int startDateRel2000;
        public int endDateRel2000;
        [OmsiStrPtr] public int line;
        [OmsiStructArrayPtr(typeof(OmsiRVNumTour), typeof(OmsiRVNumTourInternal))] public int list_number_tour;
        [OmsiStructArrayPtr(typeof(OmsiRVTypeTour), typeof(OmsiRVTypeTourInternal))] public int list_type_tour;
        [OmsiStruct(typeof(OmsiRVTypesLine), typeof(OmsiRVTypesLineInternal))] public int list_types_line;
    }
    /* TODO: Parse the pointers better to get the variables */
    public struct OmsiBoolClassCondiBool
    {
        /// <summary>
        /// Pointer to array of float var pointers (OSC accessible variables)
        /// </summary>
        public int vars;
        public int varNum;
        public bool negative;
    }
    public struct OmsiBoolClassCondiInt
    {
        /// <summary>
        /// Pointer to array of float var pointers (OSC accessible variables)
        /// </summary>
        public int vars;
        public int varNum;
        public int refValue;
        public int comparison;
    }
    public struct OmsiBoolClassCondiFloat
    {
        /// <summary>
        /// Pointer to array of float var pointers (OSC accessible variables)
        /// </summary>
        public int vars;
        public int varNum;
        public float refValue;
        public int comparison;
    }
    public enum OmsiMouseButton : byte
    {
        MBLeft, MBRight, MBMiddle
    }
    public struct OmsiLOD
    {
        public float minSize;
    }

    public struct OmsiSpot
    {
        public D3DVector pos;
        public D3DVector dir;
        public float r;
        public float g;
        public float b;
        public float range;
        public float innerCone;
        public float outerCoune;
    }

    public struct OmsiOFTTex
    {
        public int source;
        public string font;
        public OmsiPoint size;
        public byte fullColor;
        public uint color;
        public byte orientation;
        public char grid;
    }
    internal struct OmsiOFTTexInternal
    {
        public int source;
        [OmsiStrPtr] public int font;
        public OmsiPoint size;
        public byte fullColor;
        public uint color;
        public byte orientation;
        public char grid;
    }
    public enum OmsiPathRuleIdents : int
    {
        TPRISpeed, TPRIOvertake, TPRInocars, TPRIbuses, TPRItrucks, TPRITrafficDens, TPRIPriority
    }

    public struct OmsiScriptTex
    {
        public OmsiPoint size;
    }
    [StructLayout(LayoutKind.Explicit, Size = 0x20)]
    public struct OmsiCriticalSectionInternal
    {
        [FieldOffset(0x00)] public RTL_CRITICAL_SECTION cs;
        [FieldOffset(0x18)] [OmsiStrPtr(StrPtrType.RawDelphiString)] public int name;
        [FieldOffset(0x1c)] public uint ident;
    }
    public struct OmsiCriticalSection
    {
        public RTL_CRITICAL_SECTION cs;
        public string name;
        public uint ident;
    }
    /// <summary>
    /// This is a place holder struct, confirmation of exact struct data TBC
    /// </summary>
    public struct RTL_CRITICAL_SECTION
    {
        public IntPtr DebugInfo;
        public long LockCount;
        public long RecursionCount;
        public IntPtr OwningThread;
        public IntPtr LockSemaphore;
        public uint SpinCount;
    }

    /// <summary>
    /// Struct used for Strings to floats - especially used in the OMSI OSC StringVariables
    /// </summary>
    public struct OmsiString
    {
        public string String;
        public OmsiString(string s)
        {
            String = s;
        }
    }
    public struct OmsiStringInternal
    {
        [OmsiStrPtr(false, true, false)] public int String;
    }
    /// <summary>
    /// Struct used for WStrings to strings - especially used in the OMSI OSC StringVariables
    /// </summary>
    public struct OmsiWString
    {
        public string String;
        public OmsiWString(string s)
        {
            String = s;
        }
    }
    public struct OmsiWStringInternal
    {
        [OmsiStrPtr(true, true, false)] public int String;
    }
    /// <summary>
    /// Struct used for Pointers to floats - especially used in the OMSI OSC variables
    /// </summary>
    public struct OmsiFloatPtr
    {
        public float Float;
        public OmsiFloatPtr(float f)
        {
            Float = f;
        }
    }
    public struct OmsiFloatPtrInternal
    {
        [OmsiStructPtr(typeof(float))] public int Float;
    }

    public struct OmsiFileSplinePathInfo
    {
        public OmsiPathRule[] Rules;
    }

    public struct OmsiFileSplinePathInfoInternal
    {
        [OmsiStructArrayPtr(typeof(OmsiPathRule), typeof(OmsiPathRuleInternal))] public int Rules;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct OmsiBoogieInternal
    {
        [OmsiStruct(typeof(OmsiPathInfo), typeof(OmsiPathInfoInternal))]
        [FieldOffset(0x0)] internal OmsiPathInfoInternal pathInfo;
        [FieldOffset(0x110)] internal D3DVector pos;
        [FieldOffset(0x11c)] internal D3DXVector2 y_soll;
        [FieldOffset(0x124)] internal D3DXVector2 y_harmon;
        [FieldOffset(0x12c)] internal float y_gleisfehler;
        [FieldOffset(0x130)] internal float z_gleisfehler;
    }

    public struct OmsiBoogie
    {
        public OmsiPathInfo pathInfo;
        public D3DVector pos;
        public D3DXVector2 y_soll;
        public D3DXVector2 y_harmon;
        public float y_gleisfehler;
        public float z_gleisfehler;
    }

    /// <summary>
    /// Omsi Path Setpoints
    /// </summary>
    public struct OmsiPathSollwerte
    {
        public float v, x, curve_x_offset, ai_stdbrems;
    }

    public struct OmsiMaterialItemInternal
    {
        [OmsiStruct(typeof(OmsiMaterialProp), typeof(OmsiMaterialPropInternal))]
        public OmsiMaterialPropInternal Properties;
    }

    public struct OmsiMaterialItem
    {
        public OmsiMaterialProp Properties;
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x68)]
    public struct OmsiTextureItemInternal
    {
        [FieldOffset(0x0)] public ushort size_x;
        [FieldOffset(0x2)] public ushort size_y;
        [FieldOffset(0x8)] public double mem;
        [FieldOffset(0x10)] public int datasize;
        [FieldOffset(0x14)] public bool dataready;
        [FieldOffset(0x18)] [OmsiObjPtr(typeof(D3DTexture))] public int Texture;
        [FieldOffset(0x1c)] [OmsiObjPtr(typeof(D3DTexture))] public int oldTexture;
        [FieldOffset(0x20)] [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int path;
        [FieldOffset(0x24)] [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int justfilename;
        [FieldOffset(0x28)] [OmsiStrPtr(StrPtrType.RawDelphiAnsiString)] public int loadpath;
        [FieldOffset(0x2c)] public byte loaded;
        [FieldOffset(0x2d)] public byte load_request;
        [FieldOffset(0x2e)] public bool managed;
        [FieldOffset(0x30)] public uint failed;
        [FieldOffset(0x34)] public ushort used;
        [FieldOffset(0x36)] public ushort used_highres;
        [FieldOffset(0x38)] public bool threadloading;
        [FieldOffset(0x39)] public bool hasspecials;
        [FieldOffset(0x3a)] public bool no_unload;
        [FieldOffset(0x3b)] public bool onlyalpha;
        [FieldOffset(0x3c)] public int NightMap;
        [FieldOffset(0x40)] public int WinterSnowMap;
        [FieldOffset(0x44)] public int WinterSnowfallMap;
        [FieldOffset(0x48)] public int FallMap;
        [FieldOffset(0x4c)] public int SpringMap;
        [FieldOffset(0x50)] public int WinterMap;
        [FieldOffset(0x54)] public int SummerDryMap;
        [FieldOffset(0x58)] public int SurfMap;
        [FieldOffset(0x5c)] public bool moisture;
        [FieldOffset(0x5d)] public bool puddles;
        [FieldOffset(0x5e)] public bool moisture_ic;
        [FieldOffset(0x5f)] public bool puddles_ic;
        [FieldOffset(0x60)] public byte surface;
        [FieldOffset(0x61)] public byte surface_ic;
        [FieldOffset(0x62)] public bool terrainmapping;
        [FieldOffset(0x63)] public bool terrainmapping_alpha;
    }

    public struct OmsiTextureItem
    {
        public ushort size_x;
        public ushort size_y;
        public double mem;
        public int datasize;
        public bool dataready;
        public D3DTexture Texture;
        public D3DTexture oldTexture;
        public string path;
        public string justfilename;
        public string loadpath;
        public byte loaded;
        public byte load_request;
        public bool managed;
        public uint failed;
        public ushort used;
        public ushort used_highres;
        public bool threadloading;
        public bool hasspecials;
        public bool no_unload;
        public bool onlyalpha;
        public int NightMap;
        public int WinterSnowMap;
        public int WinterSnowfallMap;
        public int FallMap;
        public int SpringMap;
        public int WinterMap;
        public int SummerDryMap;
        public int SurfMap;
        public bool moisture;
        public bool puddles;
        public bool moisture_ic;
        public bool puddles_ic;
        public byte surface;
        public byte surface_ic;
        public bool terrainmapping;
        public bool terrainmapping_alpha;
    }
}

