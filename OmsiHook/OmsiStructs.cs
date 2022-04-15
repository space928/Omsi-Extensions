using System;
using System.Runtime.InteropServices;

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

	public struct OmsiTicketPackInternal
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

	public struct OmsiTicketInternal
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
}
