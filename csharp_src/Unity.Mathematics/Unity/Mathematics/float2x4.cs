using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct float2x4 : IEquatable<float2x4>, IFormattable
{
	public float2 c0;

	public float2 c1;

	public float2 c2;

	public float2 c3;

	public static readonly float2x4 zero;

	public unsafe ref float2 this[int index]
	{
		get
		{
			fixed (float2x4* ptr = &this)
			{
				return ref *(float2*)((byte*)ptr + (nint)index * (nint)sizeof(float2));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(float2 c0, float2 c1, float2 c2, float2 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13)
	{
		c0 = new float2(m00, m10);
		c1 = new float2(m01, m11);
		c2 = new float2(m02, m12);
		c3 = new float2(m03, m13);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(float v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(bool v)
	{
		c0 = math.select(new float2(0f), new float2(1f), v);
		c1 = math.select(new float2(0f), new float2(1f), v);
		c2 = math.select(new float2(0f), new float2(1f), v);
		c3 = math.select(new float2(0f), new float2(1f), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(bool2x4 v)
	{
		c0 = math.select(new float2(0f), new float2(1f), v.c0);
		c1 = math.select(new float2(0f), new float2(1f), v.c1);
		c2 = math.select(new float2(0f), new float2(1f), v.c2);
		c3 = math.select(new float2(0f), new float2(1f), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(int2x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(uint2x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(double v)
	{
		c0 = (float2)v;
		c1 = (float2)v;
		c2 = (float2)v;
		c3 = (float2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2x4(double2x4 v)
	{
		c0 = (float2)v.c0;
		c1 = (float2)v.c1;
		c2 = (float2)v.c2;
		c3 = (float2)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2x4(float v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float2x4(bool v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float2x4(bool2x4 v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2x4(int v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2x4(int2x4 v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2x4(uint v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2x4(uint2x4 v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float2x4(double v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float2x4(double2x4 v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator *(float2x4 lhs, float2x4 rhs)
	{
		return new float2x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator *(float2x4 lhs, float rhs)
	{
		return new float2x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator *(float lhs, float2x4 rhs)
	{
		return new float2x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator +(float2x4 lhs, float2x4 rhs)
	{
		return new float2x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator +(float2x4 lhs, float rhs)
	{
		return new float2x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator +(float lhs, float2x4 rhs)
	{
		return new float2x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator -(float2x4 lhs, float2x4 rhs)
	{
		return new float2x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator -(float2x4 lhs, float rhs)
	{
		return new float2x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator -(float lhs, float2x4 rhs)
	{
		return new float2x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator /(float2x4 lhs, float2x4 rhs)
	{
		return new float2x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator /(float2x4 lhs, float rhs)
	{
		return new float2x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator /(float lhs, float2x4 rhs)
	{
		return new float2x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator %(float2x4 lhs, float2x4 rhs)
	{
		return new float2x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator %(float2x4 lhs, float rhs)
	{
		return new float2x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator %(float lhs, float2x4 rhs)
	{
		return new float2x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator ++(float2x4 val)
	{
		return new float2x4(++val.c0, ++val.c1, ++val.c2, ++val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator --(float2x4 val)
	{
		return new float2x4(--val.c0, --val.c1, --val.c2, --val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(float2x4 lhs, float2x4 rhs)
	{
		return new bool2x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(float2x4 lhs, float rhs)
	{
		return new bool2x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(float lhs, float2x4 rhs)
	{
		return new bool2x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(float2x4 lhs, float2x4 rhs)
	{
		return new bool2x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(float2x4 lhs, float rhs)
	{
		return new bool2x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(float lhs, float2x4 rhs)
	{
		return new bool2x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(float2x4 lhs, float2x4 rhs)
	{
		return new bool2x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(float2x4 lhs, float rhs)
	{
		return new bool2x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(float lhs, float2x4 rhs)
	{
		return new bool2x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(float2x4 lhs, float2x4 rhs)
	{
		return new bool2x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(float2x4 lhs, float rhs)
	{
		return new bool2x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(float lhs, float2x4 rhs)
	{
		return new bool2x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator -(float2x4 val)
	{
		return new float2x4(-val.c0, -val.c1, -val.c2, -val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 operator +(float2x4 val)
	{
		return new float2x4(+val.c0, +val.c1, +val.c2, +val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(float2x4 lhs, float2x4 rhs)
	{
		return new bool2x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(float2x4 lhs, float rhs)
	{
		return new bool2x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(float lhs, float2x4 rhs)
	{
		return new bool2x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(float2x4 lhs, float2x4 rhs)
	{
		return new bool2x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(float2x4 lhs, float rhs)
	{
		return new bool2x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(float lhs, float2x4 rhs)
	{
		return new bool2x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(float2x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((float2x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"float2x4({c0.x}f, {c1.x}f, {c2.x}f, {c3.x}f,  {c0.y}f, {c1.y}f, {c2.y}f, {c3.y}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float2x4({c0.x.ToString(format, formatProvider)}f, {c1.x.ToString(format, formatProvider)}f, {c2.x.ToString(format, formatProvider)}f, {c3.x.ToString(format, formatProvider)}f,  {c0.y.ToString(format, formatProvider)}f, {c1.y.ToString(format, formatProvider)}f, {c2.y.ToString(format, formatProvider)}f, {c3.y.ToString(format, formatProvider)}f)";
	}
}
