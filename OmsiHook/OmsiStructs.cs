using System;
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
    }
    public struct D3DXVector2
    {
        public float x, y;
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
    }

    /// <summary>
    /// Defines a 4 dimensional float vector.
    /// </summary>
    public struct D3DXQuaternion
    {
        public float x, y, z, w;
    }

    /// <summary>
    /// Defines a plane with a normal vector and a distance.
    /// </summary>
    public struct D3DXPlane
    {
        public float a, b, c, d;
    }

    /// <summary>
    /// Defines a colour in terms of r,g,b,a as floats.
    /// </summary>
    public struct D3DColorValue
    {
        public float r, g, b, a;
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

    internal struct OmsiMaterialPropInternal
    {
        public int mainTexture;
        public bool standard;
        public bool useTexCoordTransX, useTexCoordTransY;
        public bool dirtScale, alphaScale;
        public bool useRealTimeReflx, useNightMap, useLightMap, useTerrainLightMap, useReflxMap;
        public bool horizontal;
        public bool water;
        public bool useEnvirReflx, useEnvirMask, useBumpMap, useRainDropAreaMap,
            useTransMap, useTextTexture, useScriptTexture;
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
        [OmsiStrPtr] public int shaderBlock;
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
            useTransMap, useTextTexture, useScriptTexture;
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
        [OmsiStrPtr] public int filename;
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
        [OmsiStrPtr] public int texture;
        [OmsiStrPtr] public int det_texture;
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
        [OmsiStrPtr(true)] public int name; // STRING
        [OmsiStrPtr(true)] public int description; // STRING
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
        [OmsiStrPtr] public int cloudType; // ANSI STRING
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
        [OmsiStrPtr] public int name; // ANSI String
        [OmsiStrPtr] public int texFile; // ANSI String
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
        [OmsiStrPtr] public int filename;
        [OmsiStrPtr(true)] public int voicepath;
        [OmsiStructArrayPtr(typeof(OmsiTicket), typeof(OmsiTicketInternal))]
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

    internal struct OmsiTicketInternal
    {
        //TODO: I don't think these are decoding correctly (I saw nothing when I looked), check this actually works.
        [OmsiStrPtr] public int name, name_english, name_display;
        public int max_stations;
        public int age_min, age_max;
        public float value;
        public bool dayTicket;
        public float propability;
        [OmsiObjPtr(typeof(D3DMeshFileObject))] public int mesh_block;
        [OmsiObjPtr(typeof(D3DMeshFileObject))] public int mesh_single;
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
        [OmsiStrPtr] public int moneyPos_parent_str; // ANSIString
        public D3DVector changePos;
        public D3DVector changePos_var;
        public int changePos_parent_idx;
        [OmsiStrPtr] public int changePos_parent_str; // ANSIString
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
        [OmsiStrPtr] public int aiList;
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
        [OmsiStrPtr] public int name;
    }

    public struct OmsiHoliday
    {
        public int date;
        public string name;
    }

    internal struct OmsiHolidaysInternal
    {
        public int start, ende;
        [OmsiStrPtr] public int name;
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
    internal struct OmsiPathInfoInternal
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
        [OmsiStructPtr(typeof(uint))] public int idCode;
        [OmsiObjPtr(typeof(OmsiObject))] public int vehicle;
        public float veloc;
        public float x_L, x_R, z_B, z_F;
        public float radius;
        public float drehpunkt;
        [OmsiStructPtr(typeof(D3DMatrix))] public int absPosition;
        public byte waitMode;
        public int reserveGroup;
        public byte blinker;
        public byte einOrdnen;//TODO: Check data type
        public bool prevVisible_logical;
        //TODO: Determine actual type
        public int nextPath_a, nextPath_b, nextPath_c, nextPath_d, nextPath_e;
        public OmsiNextPathSegment nextPathSeg;
        public OmsiNextPathID nextPathID;
        [OmsiStructArrayPtr(typeof(OmsiPathID))]
        public int reservePaths;
        public int spurwechsel; //TODO: Check data type
        public uint spurwechselTime;
        public bool spurwechselAuto;
        public bool noScheduledSpurwechsel;
        public bool on_crossing;
        public float leftFree, rightFree;
        public byte relPosError; //TODO: Check data type
        public bool setPosOnNearTile;
        public float rowdy_factor;
        public bool eilig;
        public bool einSatzFahrzeug;
        public float martinshorn;
        public float getrieben_von_einSatz;
        public uint getrieben_von_einSatz_time;
        public byte setSoll;
        public int track;
        public int trackEntry;
        public int stnLink;
        public int next_stnLink;
        public bool zwangsEinsetzen;
        public bool allowHupen;
        [OmsiStrPtr] public int debug_aiData_limit;
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
        public byte einordnen;//TODO: Check data type
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
        public bool einsatzfahrzeug;
        /// <summary>
        /// Siren
        /// </summary>
        public float martinshorn;
        /// <summary>
        /// Driven by emergency?
        /// </summary>
        public float getrieben_von_einsatz;
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

    internal struct OmsiObjectPathInfoInternal
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
        public bool angetrieben; // Driven? Powered?
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
        public string endstelle; // Terminus - ANSI String
        public int texNummer;
        public string[] strings;
        public bool allExit;
    }
    internal struct OmsiHOFTargetInternal
    {
        public int nummer;
        [OmsiStrPtr] public int name; // ANSI String
        [OmsiStrPtr] public int endstelle; // Terminus - ANSI String
        public int texNummer;
        [OmsiStrArrayPtr] public int strings;
        public bool allExit;
    }

    public struct OmsiHofFISBusstop
    {
        public string ident; // ANSI String
        public string[] strings;
    }
    internal struct OmsiHofFISBusstopInternal
    {
        [OmsiStrPtr] public int ident; // ANSI String
        [OmsiStrArrayPtr] public int strings;
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
        [OmsiStrPtr] public int name; // ANSI String
        public int target;
        [OmsiStrPtr] public int line; // ANSI String
        [OmsiStrArrayPtr] public int busstops;
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
		[OmsiStrPtr] public int fstring;
		[OmsiObjPtr(typeof(OmsiObject))] public int fobject;
    }

    public struct OmsiScriptTexture
    {
        public IntPtr tex; // IDirect3DTexture9 - No Marshaling implemented
        public uint[] TexPn;
        public uint color;
    }

    internal struct OmsiScriptTextureInternal
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
        [OmsiStrPtr] public int busname;
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
        public double bew_fahrstill; // These look like driver raitings
        public uint bew_passcomfort;
        public uint bew_ticket_count;
        public uint bew_ticket_points;
        public uint passCount;
        public uint ticket_cnt;
        public float tickets_cash;
        public OmsiPerbus[] perbus;
    }

    internal struct OmsiDriverInternal
    {
        [OmsiStrPtr] public int filename;
        [OmsiStrPtr] public int name;
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
        public double bew_fahrstill; // These look like driver raitings
        public uint bew_passcomfort;
        public uint bew_ticket_count;
        public uint bew_ticket_points;
        public uint passCount;
        public uint ticket_cnt;
        public float tickets_cash;
        [OmsiStructPtr(typeof(OmsiPerbus),typeof(OmsiPerbusInternal))] public int perbus;

    }
	///<summary>
	///Busstop, estimated arrival and departure, actual arrival and departure, arrived on time, departed on time
	///</summary>
    public struct OmsiTTLogDetailed
    {
        public string busstop_name;
        public int eta;
        public int etd;
        public int ata;
        public int atd;
        public byte arr_ok;
        public byte dep_ok;
    }
    internal struct OmsiTTLogDetailedInternal
    {
        [OmsiStrPtr] public int busstop_name;
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
    internal struct OmsiTTTrackEntryInternal
    {
        public uint idCode;
        public int pathIndexOnObject;
        [OmsiStructPtr(typeof(OmsiPathID))] public int pathIndex;
        public float relDist;
        public float dist;
        public bool valid;
        public byte pathOrderCheck;
        [OmsiStructArrayPtr(typeof(int))] public int fstrn_allowed;
        public int chronon_origin;
        [OmsiStrArrayPtr] public int chronos_bad;
    }

    public struct OmsiTTTrack
    {
        public string filename;
        public string filepath;
        public OmsiTTTrackEntry[] TrackEntrys;
        public float laenge; // Length
    }
    internal struct OmsiTTTrackInternal
    {
        [OmsiStrPtr] public int filename;
        [OmsiStrPtr] public int filepath;
        [OmsiStructArrayPtr(typeof(OmsiTTTrackEntry), typeof(OmsiTTTrackEntryInternal))] public int TrackEntrys;
        public float laenge; // Length
    }
    
    public struct OmsiTTBusstop
    {
        public string name;
        public string name_zusatz; // Supliment / Addition
        public int kachel;
        public uint IDCode_formal;
        public uint IDCode_real;
        public int index;
        public int index_ownList;
        public int[] index_alternatives;
        public float preset_Aussteiger; // Dropouts?
        public OmsiPathID pathIndex;
        public int trackEntry;
        public bool invalid;
        public float x_path;
        public float dist_relPath;
        public float dist;
    }
    internal struct OmsiTTBusstopInternal
    {
        [OmsiStrPtr] public int name;
        [OmsiStrPtr] public int name_zusatz; // Supliment / Addition
        public int kachel;
        public uint IDCode_formal;
        public uint IDCode_real;
        public int index;
        public int index_ownList;
        [OmsiStructArrayPtr(typeof(int))] public int index_alternatives;
        public float preset_Aussteiger; // Dropouts?
        [OmsiStructPtr(typeof(OmsiPathID))] public int pathIndex;
        public int trackEntry;
        public bool invalid;
        public float x_path;
        public float dist_relPath;
        public float dist;
    }

    public struct OmsiTTStopTime
    {
        public float arr_time;
        public float dep_time;
        public bool arr_time_man;
        public bool dep_time_man;
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
        [OmsiStrPtr] public int name;
        public float time_all;
        [OmsiStructArrayPtr(typeof(OmsiTTStopTime))] public int stop_times;
        [OmsiStructArrayPtr(typeof(float))] public int TrackEntryTime;
        public bool serviceTrip;
    }

    public struct OmsiTTTrip
    {
        public string filename;
        public int chrono_origin;
        public string target;
        public string linie;
        public bool trainRev;
        public byte invalide;
        public OmsiTTProfile[] profiles;
        public OmsiTTBusstop[] busstops;
        public string trackName;
        public int trackIndex;
        public int[][] stnLinkList;
    }
    internal struct OmsiTTTripInternal
    {
        [OmsiStrPtr] public int filename;
        public int chrono_origin;
        [OmsiStrPtr] public int target;
        [OmsiStrPtr] public int linie;
        public bool trainRev;
        public byte invalide;
        [OmsiStructArrayPtr(typeof(OmsiTTProfile), typeof(OmsiTTProfileInternal))] public int profiles;
        [OmsiStructArrayPtr(typeof(OmsiTTBusstop), typeof(OmsiTTBusstopInternal))] public int busstops;
        [OmsiStrPtr] public int trackName;
        public int trackIndex;
        [OmsiStructArrayPtr(typeof(int[]))] public int stnLinkList;
    }

    public struct OmsiTTBusstopListEntryChronoRename
    {
        public string name;
        public string name_zustatz; // Supliment / Addition
        public float aussteiger; // Dropouts?
        public int chrono_origin;
    }
    internal struct OmsiTTBusstopListEntryChronoRenameInternal
    {
        [OmsiStrPtr] public int name;
        [OmsiStrPtr] public int name_zustatz; // Supliment / Addition
        public float aussteiger; // Dropouts?
        public int chrono_origin;
    }
    public struct OmsiTTBusstopListEntry
    {
        public string name_zustatz; // Supliment / Addition
        public string name;
        public int kachel;
        public uint IDCode;
        public uint parent_IDCode;
        public float preset_aussteiger; // Dropouts?
        public string[] chronos_bad;
        public int index;
        public int[] stnlinks_starting;
        public int[] stnlinks_ending;
        public int chrono_origin;
        public OmsiTTBusstopListEntryChronoRename[] chrono_rename;
    }
    internal struct OmsiTTBusstopListEntryInternal
    {
        [OmsiStrPtr] public int name_zustatz; // Supliment / Addition
        [OmsiStrPtr] public int name;
        public int kachel;
        public uint IDCode;
        public uint parent_IDCode;
        public float preset_aussteiger; // Dropouts?
        [OmsiStrArrayPtr] public int chronos_bad;
        public int index;
        [OmsiStructArrayPtr(typeof(int))] public int stnlinks_starting;
        [OmsiStructArrayPtr(typeof(int))] public int stnlinks_ending;
        public int chrono_origin;
        [OmsiStructArrayPtr(typeof(OmsiTTBusstopListEntryChronoRename), typeof(OmsiTTBusstopListEntryChronoRenameInternal))] public int chrono_rename;
    }

    public struct OmsiTTStnLink
    {
        public OmsiTTTrackEntry[] trackEntrys;
        public float laenge; // Length
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
        [OmsiStructArrayPtr(typeof(OmsiTTTrackEntry), typeof(OmsiTTTrackEntryInternal))] public int trackEntrys;
        public float laenge; // Length
        public uint busstop_start_IDCode;
        public uint busstop_end_IDCode;
        public float x_path_Start;
        public float x_path_end;
        [OmsiStructArrayPtr(typeof(int))] public int chronos_needed;
        [OmsiStructArrayPtr(typeof(int))] public int chronos_bad;
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
        [OmsiStrPtr] public int trip;
        public int tripIndex;
        public int profile;
        public float startTime;
        public float endTime;
        public bool smoothTrans;
    }

    public struct OmsiTTTourValid
    {
        public bool mon;
        public bool tue;
        public bool wed;
        public bool thu;
        public bool fri;
        public bool sat;
        public bool sun;
        public bool hol;
        public bool hols;
        public bool noHols;
    }

    public struct OmsiTTTour
    {
        public string name;
        public string aiGroup;
        public int aiType;
        public int aiGroupIndex;
        public string vehicle_nr_reservations;
        public int[] vehical_indizes;
        public bool hasNormalVeh;
        public int TagErledigt; // day done? (Day of week maybe? - OmsiTTTourValid?)
        public OmsiTTTourValid validOn;
        public bool invalide;
        public OmsiTTTourEntry[] entrys;
    }
    internal struct OmsiTTTourInternal
    {
        [OmsiStrPtr] public int name;
        [OmsiStrPtr] public int aiGroup;
        public int aiType;
        public int aiGroupIndex;
        [OmsiStrPtr] public int vehicle_nr_reservations;
        [OmsiStructArrayPtr(typeof(int))] public int vehical_indizes;
        public bool hasNormalVeh;
        public int TagErledigt; // day done? (Day of week maybe? - OmsiTTTourValid?)
        [OmsiStruct(typeof(OmsiTTTourValid), typeof(OmsiTTTourValid))] public int validOn;
        public bool invalide;
        [OmsiStructArrayPtr(typeof(OmsiTTTourEntry), typeof(OmsiTTTourEntryInternal))] public int entrys;
    }

    public struct OmsiTTLine
    {
        public string name;
        public bool userAllowed;
        public byte priority;
        public OmsiTTTour[] tours;
        public int chrono_origin;
    }
    internal struct OmsiTTLineInternal
    {
        [OmsiStrPtr] public int name;
        public bool userAllowed;
        public byte priority;
        [OmsiStructArrayPtr(typeof(OmsiTTTour), typeof(OmsiTTTourInternal))] public int tours;
        public int chrono_origin;
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
        [OmsiStruct(typeof(OmsiRVTypesLine), typeof(OmsiRVTypesLineInternal)] public int list_types_line;
    }
}
