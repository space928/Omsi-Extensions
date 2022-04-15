using System;

namespace OmsiHook
{
	public struct D3DVector
	{
		public float x, y, z;
	}

	public struct D3DMatrix
    {
		public float _00, _01, _02, _03,
					 _10, _11, _12, _13,
					 _20, _21, _22, _23,
					 _30, _31, _32, _33;
    }

	public struct D3DXQuaternion
    {
		public float x, y, z, w;
    }

	public struct OmsiPoint
    {
		public int x, y;
    }
	public struct D3DXPlane
    {
		public float a, b, c, d;
    }
}
