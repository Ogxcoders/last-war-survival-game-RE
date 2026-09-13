using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RVO;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct RVOMath
{
	internal const float RVO_EPSILON = 1E-05f;

	public static float abs(Vector2 vector)
	{
		return sqrt(absSq(vector));
	}

	public static float absSq(Vector2 vector)
	{
		return vector * vector;
	}

	public static Vector2 normalize(Vector2 vector)
	{
		return vector / abs(vector);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static float det(Vector2 vector1, Vector2 vector2)
	{
		return vector1.x_ * vector2.y_ - vector1.y_ * vector2.x_;
	}

	internal static float distSqPointLineSegment(Vector2 vector1, Vector2 vector2, Vector2 vector3)
	{
		float num = (vector3 - vector1) * (vector2 - vector1) / absSq(vector2 - vector1);
		if (num < 0f)
		{
			return absSq(vector3 - vector1);
		}
		if (num > 1f)
		{
			return absSq(vector3 - vector2);
		}
		return absSq(vector3 - (vector1 + num * (vector2 - vector1)));
	}

	internal static float fabs(float scalar)
	{
		return Math.Abs(scalar);
	}

	internal static float leftOf(Vector2 a, Vector2 b, Vector2 c)
	{
		float num = a.x_ - c.x_;
		float num2 = a.y_ - c.y_;
		float num3 = b.x_ - a.x_;
		float num4 = b.y_ - a.y_;
		return num * num4 - num2 * num3;
	}

	internal static float sqr(float scalar)
	{
		return scalar * scalar;
	}

	internal static float sqrt(float scalar)
	{
		return (float)Math.Sqrt(scalar);
	}
}
