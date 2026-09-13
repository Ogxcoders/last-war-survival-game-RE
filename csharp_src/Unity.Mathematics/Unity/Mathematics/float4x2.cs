using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct float4x2 : IEquatable<float4x2>, IFormattable
{
	public float4 c0;

	public float4 c1;

	public static readonly float4x2 zero;

	public unsafe ref float4 this[int index]
	{
		get
		{
			fixed (float4x2* ptr = &this)
			{
				return ref *(float4*)((byte*)ptr + (nint)index * (nint)sizeof(float4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(float4 c0, float4 c1)
	{
		this.c0 = c0;
		this.c1 = c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(float m00, float m01, float m10, float m11, float m20, float m21, float m30, float m31)
	{
		c0 = new float4(m00, m10, m20, m30);
		c1 = new float4(m01, m11, m21, m31);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(float v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(bool v)
	{
		c0 = math.select(new float4(0f), new float4(1f), v);
		c1 = math.select(new float4(0f), new float4(1f), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(bool4x2 v)
	{
		c0 = math.select(new float4(0f), new float4(1f), v.c0);
		c1 = math.select(new float4(0f), new float4(1f), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(int v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(int4x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(uint v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(uint4x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(double v)
	{
		c0 = (float4)v;
		c1 = (float4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x2(double4x2 v)
	{
		c0 = (float4)v.c0;
		c1 = (float4)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x2(float v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x2(bool v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x2(bool4x2 v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x2(int v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x2(int4x2 v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x2(uint v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x2(uint4x2 v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x2(double v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x2(double4x2 v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator *(float4x2 lhs, float4x2 rhs)
	{
		return new float4x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator *(float4x2 lhs, float rhs)
	{
		return new float4x2(lhs.c0 * rhs, lhs.c1 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator *(float lhs, float4x2 rhs)
	{
		return new float4x2(lhs * rhs.c0, lhs * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator +(float4x2 lhs, float4x2 rhs)
	{
		return new float4x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator +(float4x2 lhs, float rhs)
	{
		return new float4x2(lhs.c0 + rhs, lhs.c1 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator +(float lhs, float4x2 rhs)
	{
		return new float4x2(lhs + rhs.c0, lhs + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator -(float4x2 lhs, float4x2 rhs)
	{
		return new float4x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator -(float4x2 lhs, float rhs)
	{
		return new float4x2(lhs.c0 - rhs, lhs.c1 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator -(float lhs, float4x2 rhs)
	{
		return new float4x2(lhs - rhs.c0, lhs - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator /(float4x2 lhs, float4x2 rhs)
	{
		return new float4x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator /(float4x2 lhs, float rhs)
	{
		return new float4x2(lhs.c0 / rhs, lhs.c1 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator /(float lhs, float4x2 rhs)
	{
		return new float4x2(lhs / rhs.c0, lhs / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator %(float4x2 lhs, float4x2 rhs)
	{
		return new float4x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator %(float4x2 lhs, float rhs)
	{
		return new float4x2(lhs.c0 % rhs, lhs.c1 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator %(float lhs, float4x2 rhs)
	{
		return new float4x2(lhs % rhs.c0, lhs % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator ++(float4x2 val)
	{
		return new float4x2(++val.c0, ++val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator --(float4x2 val)
	{
		return new float4x2(--val.c0, --val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <(float4x2 lhs, float4x2 rhs)
	{
		return new bool4x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <(float4x2 lhs, float rhs)
	{
		return new bool4x2(lhs.c0 < rhs, lhs.c1 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <(float lhs, float4x2 rhs)
	{
		return new bool4x2(lhs < rhs.c0, lhs < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <=(float4x2 lhs, float4x2 rhs)
	{
		return new bool4x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <=(float4x2 lhs, float rhs)
	{
		return new bool4x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <=(float lhs, float4x2 rhs)
	{
		return new bool4x2(lhs <= rhs.c0, lhs <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >(float4x2 lhs, float4x2 rhs)
	{
		return new bool4x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >(float4x2 lhs, float rhs)
	{
		return new bool4x2(lhs.c0 > rhs, lhs.c1 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >(float lhs, float4x2 rhs)
	{
		return new bool4x2(lhs > rhs.c0, lhs > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >=(float4x2 lhs, float4x2 rhs)
	{
		return new bool4x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >=(float4x2 lhs, float rhs)
	{
		return new bool4x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >=(float lhs, float4x2 rhs)
	{
		return new bool4x2(lhs >= rhs.c0, lhs >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator -(float4x2 val)
	{
		return new float4x2(-val.c0, -val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 operator +(float4x2 val)
	{
		return new float4x2(+val.c0, +val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(float4x2 lhs, float4x2 rhs)
	{
		return new bool4x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(float4x2 lhs, float rhs)
	{
		return new bool4x2(lhs.c0 == rhs, lhs.c1 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(float lhs, float4x2 rhs)
	{
		return new bool4x2(lhs == rhs.c0, lhs == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(float4x2 lhs, float4x2 rhs)
	{
		return new bool4x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(float4x2 lhs, float rhs)
	{
		return new bool4x2(lhs.c0 != rhs, lhs.c1 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(float lhs, float4x2 rhs)
	{
		return new bool4x2(lhs != rhs.c0, lhs != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(float4x2 rhs)
	{
		if (c0.Equals(rhs.c0))
		{
			return c1.Equals(rhs.c1);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((float4x2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"float4x2({c0.x}f, {c1.x}f,  {c0.y}f, {c1.y}f,  {c0.z}f, {c1.z}f,  {c0.w}f, {c1.w}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float4x2({c0.x.ToString(format, formatProvider)}f, {c1.x.ToString(format, formatProvider)}f,  {c0.y.ToString(format, formatProvider)}f, {c1.y.ToString(format, formatProvider)}f,  {c0.z.ToString(format, formatProvider)}f, {c1.z.ToString(format, formatProvider)}f,  {c0.w.ToString(format, formatProvider)}f, {c1.w.ToString(format, formatProvider)}f)";
	}
}
