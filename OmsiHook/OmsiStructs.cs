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

	/// <summary>
	/// Defines a 2 dimensional int vector
	/// </summary>
	public struct OmsiPoint
    {
		public int x, y;
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
		[MarshalAs(UnmanagedType.LPStr)]
		public string shaderBlock;
		public bool splineScaleByLength;
	}

	public struct OmsiMapKachelInfo
    {
		public OmsiPoint position;
		[MarshalAs(UnmanagedType.LPStr)]
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

	public struct OmsiGroundType
    {
		[MarshalAs(UnmanagedType.LPStr)]
		public string texture;
		[MarshalAs(UnmanagedType.LPStr)]
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
}
