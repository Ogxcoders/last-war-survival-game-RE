using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct float3x2 : IEquatable<float3x2>, IFormattable
{
	public float3 c0;

	public float3 c1;

	public static readonly float3x2 zero;

	public unsafe ref float3 this[int index]
	{
		get
		{
			fixed (float3x2* ptr = &this)
			{
				return ref *(float3*)((byte*)ptr + (nint)index * (nint)sizeof(float3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(float3 c0, float3 c1)
	{
		this.c0 = c0;
		this.c1 = c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(float m00, float m01, float m10, float m11, float m20, float m21)
	{
		c0 = new float3(m00, m10, m20);
		c1 = new float3(m01, m11, m21);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(float v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(bool v)
	{
		c0 = math.select(new float3(0f), new float3(1f), v);
		c1 = math.select(new float3(0f), new float3(1f), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(bool3x2 v)
	{
		c0 = math.select(new float3(0f), new float3(1f), v.c0);
		c1 = math.select(new float3(0f), new float3(1f), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(int v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(int3x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(uint v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(uint3x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(double v)
	{
		c0 = (float3)v;
		c1 = (float3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x2(double3x2 v)
	{
		c0 = (float3)v.c0;
		c1 = (float3)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x2(float v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3x2(bool v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3x2(bool3x2 v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x2(int v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x2(int3x2 v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x2(uint v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x2(uint3x2 v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3x2(double v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3x2(double3x2 v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator *(float3x2 lhs, float3x2 rhs)
	{
		return new float3x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator *(float3x2 lhs, float rhs)
	{
		return new float3x2(lhs.c0 * rhs, lhs.c1 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator *(float lhs, float3x2 rhs)
	{
		return new float3x2(lhs * rhs.c0, lhs * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator +(float3x2 lhs, float3x2 rhs)
	{
		return new float3x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator +(float3x2 lhs, float rhs)
	{
		return new float3x2(lhs.c0 + rhs, lhs.c1 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator +(float lhs, float3x2 rhs)
	{
		return new float3x2(lhs + rhs.c0, lhs + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator -(float3x2 lhs, float3x2 rhs)
	{
		return new float3x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator -(float3x2 lhs, float rhs)
	{
		return new float3x2(lhs.c0 - rhs, lhs.c1 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator -(float lhs, float3x2 rhs)
	{
		return new float3x2(lhs - rhs.c0, lhs - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator /(float3x2 lhs, float3x2 rhs)
	{
		return new float3x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator /(float3x2 lhs, float rhs)
	{
		return new float3x2(lhs.c0 / rhs, lhs.c1 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator /(float lhs, float3x2 rhs)
	{
		return new float3x2(lhs / rhs.c0, lhs / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator %(float3x2 lhs, float3x2 rhs)
	{
		return new float3x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator %(float3x2 lhs, float rhs)
	{
		return new float3x2(lhs.c0 % rhs, lhs.c1 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator %(float lhs, float3x2 rhs)
	{
		return new float3x2(lhs % rhs.c0, lhs % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator ++(float3x2 val)
	{
		return new float3x2(++val.c0, ++val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator --(float3x2 val)
	{
		return new float3x2(--val.c0, --val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <(float3x2 lhs, float3x2 rhs)
	{
		return new bool3x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <(float3x2 lhs, float rhs)
	{
		return new bool3x2(lhs.c0 < rhs, lhs.c1 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <(float lhs, float3x2 rhs)
	{
		return new bool3x2(lhs < rhs.c0, lhs < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <=(float3x2 lhs, float3x2 rhs)
	{
		return new bool3x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <=(float3x2 lhs, float rhs)
	{
		return new bool3x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <=(float lhs, float3x2 rhs)
	{
		return new bool3x2(lhs <= rhs.c0, lhs <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >(float3x2 lhs, float3x2 rhs)
	{
		return new bool3x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >(float3x2 lhs, float rhs)
	{
		return new bool3x2(lhs.c0 > rhs, lhs.c1 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >(float lhs, float3x2 rhs)
	{
		return new bool3x2(lhs > rhs.c0, lhs > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >=(float3x2 lhs, float3x2 rhs)
	{
		return new bool3x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >=(float3x2 lhs, float rhs)
	{
		return new bool3x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >=(float lhs, float3x2 rhs)
	{
		return new bool3x2(lhs >= rhs.c0, lhs >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator -(float3x2 val)
	{
		return new float3x2(-val.c0, -val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 operator +(float3x2 val)
	{
		return new float3x2(+val.c0, +val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(float3x2 lhs, float3x2 rhs)
	{
		return new bool3x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(float3x2 lhs, float rhs)
	{
		return new bool3x2(lhs.c0 == rhs, lhs.c1 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(float lhs, float3x2 rhs)
	{
		return new bool3x2(lhs == rhs.c0, lhs == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(float3x2 lhs, float3x2 rhs)
	{
		return new bool3x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(float3x2 lhs, float rhs)
	{
		return new bool3x2(lhs.c0 != rhs, lhs.c1 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(float lhs, float3x2 rhs)
	{
		return new bool3x2(lhs != rhs.c0, lhs != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(float3x2 rhs)
	{
		if (c0.Equals(rhs.c0))
		{
			return c1.Equals(rhs.c1);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((float3x2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"float3x2({c0.x}f, {c1.x}f,  {c0.y}f, {c1.y}f,  {c0.z}f, {c1.z}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float3x2({c0.x.ToString(format, formatProvider)}f, {c1.x.ToString(format, formatProvider)}f,  {c0.y.ToString(format, formatProvider)}f, {c1.y.ToString(format, formatProvider)}f,  {c0.z.ToString(format, formatProvider)}f, {c1.z.ToString(format, formatProvider)}f)";
	}
}
