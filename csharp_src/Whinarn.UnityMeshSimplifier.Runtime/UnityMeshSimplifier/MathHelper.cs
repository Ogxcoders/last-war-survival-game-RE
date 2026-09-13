using System;
using System.Runtime.CompilerServices;

namespace UnityMeshSimplifier;

public static class MathHelper
{
	public const float PI = MathF.PI;

	public const double PId = Math.PI;

	public const float Deg2Rad = MathF.PI / 180f;

	public const double Deg2Radd = Math.PI / 180.0;

	public const float Rad2Deg = 180f / MathF.PI;

	public const double Rad2Degd = 180.0 / Math.PI;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double Min(double val1, double val2, double val3)
	{
		if (!(val1 < val2))
		{
			if (!(val2 < val3))
			{
				return val3;
			}
			return val2;
		}
		if (!(val1 < val3))
		{
			return val3;
		}
		return val1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double Clamp(double value, double min, double max)
	{
		if (!(value >= min))
		{
			return min;
		}
		if (!(value <= max))
		{
			return max;
		}
		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double TriangleArea(ref Vector3d p0, ref Vector3d p1, ref Vector3d p2)
	{
		Vector3d from = p1 - p0;
		Vector3d to = p2 - p0;
		return from.Magnitude * (Math.Sin(Vector3d.Angle(ref from, ref to) * (Math.PI / 180.0)) * to.Magnitude) * 0.5;
	}
}
