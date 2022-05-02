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

	internal struct OmsiGroundTypeInternal
    {
		[OmsiStrPtr] public int texture;
		[OmsiStrPtr]public int det_texture;
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
		public OmsiPathPoint point;
		public D3DVector stamperPos;
		public bool valid;
    }
	public struct OmsiPassCabinTicketSale
	{
		public OmsiPathPoint point;
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

	public struct OmsiAmpelInternal
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

	public struct OmsiTriggerBox
    {
		public D3DOBB box;
		public bool reverb;
		public float reverb_time;
		public float reverb_dist;
    }

	public struct OmsiCollFeedback
    {
		public D3DVector position;
		public float energie;
    }
}
