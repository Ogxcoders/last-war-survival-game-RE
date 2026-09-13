using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct float4x3 : IEquatable<float4x3>, IFormattable
{
	public float4 c0;

	public float4 c1;

	public float4 c2;

	public static readonly float4x3 zero;

	public unsafe ref float4 this[int index]
	{
		get
		{
			fixed (float4x3* ptr = &this)
			{
				return ref *(float4*)((byte*)ptr + (nint)index * (nint)sizeof(float4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(float4 c0, float4 c1, float4 c2)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22, float m30, float m31, float m32)
	{
		c0 = new float4(m00, m10, m20, m30);
		c1 = new float4(m01, m11, m21, m31);
		c2 = new float4(m02, m12, m22, m32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(float v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(bool v)
	{
		c0 = math.select(new float4(0f), new float4(1f), v);
		c1 = math.select(new float4(0f), new float4(1f), v);
		c2 = math.select(new float4(0f), new float4(1f), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(bool4x3 v)
	{
		c0 = math.select(new float4(0f), new float4(1f), v.c0);
		c1 = math.select(new float4(0f), new float4(1f), v.c1);
		c2 = math.select(new float4(0f), new float4(1f), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(int4x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(uint4x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(double v)
	{
		c0 = (float4)v;
		c1 = (float4)v;
		c2 = (float4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x3(double4x3 v)
	{
		c0 = (float4)v.c0;
		c1 = (float4)v.c1;
		c2 = (float4)v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x3(float v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x3(bool v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x3(bool4x3 v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x3(int v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x3(int4x3 v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x3(uint v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x3(uint4x3 v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x3(double v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x3(double4x3 v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator *(float4x3 lhs, float4x3 rhs)
	{
		return new float4x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator *(float4x3 lhs, float rhs)
	{
		return new float4x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator *(float lhs, float4x3 rhs)
	{
		return new float4x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator +(float4x3 lhs, float4x3 rhs)
	{
		return new float4x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator +(float4x3 lhs, float rhs)
	{
		return new float4x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator +(float lhs, float4x3 rhs)
	{
		return new float4x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator -(float4x3 lhs, float4x3 rhs)
	{
		return new float4x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator -(float4x3 lhs, float rhs)
	{
		return new float4x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator -(float lhs, float4x3 rhs)
	{
		return new float4x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator /(float4x3 lhs, float4x3 rhs)
	{
		return new float4x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator /(float4x3 lhs, float rhs)
	{
		return new float4x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator /(float lhs, float4x3 rhs)
	{
		return new float4x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator %(float4x3 lhs, float4x3 rhs)
	{
		return new float4x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator %(float4x3 lhs, float rhs)
	{
		return new float4x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator %(float lhs, float4x3 rhs)
	{
		return new float4x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator ++(float4x3 val)
	{
		return new float4x3(++val.c0, ++val.c1, ++val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator --(float4x3 val)
	{
		return new float4x3(--val.c0, --val.c1, --val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <(float4x3 lhs, float4x3 rhs)
	{
		return new bool4x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <(float4x3 lhs, float rhs)
	{
		return new bool4x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <(float lhs, float4x3 rhs)
	{
		return new bool4x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <=(float4x3 lhs, float4x3 rhs)
	{
		return new bool4x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <=(float4x3 lhs, float rhs)
	{
		return new bool4x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <=(float lhs, float4x3 rhs)
	{
		return new bool4x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >(float4x3 lhs, float4x3 rhs)
	{
		return new bool4x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >(float4x3 lhs, float rhs)
	{
		return new bool4x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >(float lhs, float4x3 rhs)
	{
		return new bool4x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >=(float4x3 lhs, float4x3 rhs)
	{
		return new bool4x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >=(float4x3 lhs, float rhs)
	{
		return new bool4x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >=(float lhs, float4x3 rhs)
	{
		return new bool4x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator -(float4x3 val)
	{
		return new float4x3(-val.c0, -val.c1, -val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 operator +(float4x3 val)
	{
		return new float4x3(+val.c0, +val.c1, +val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator ==(float4x3 lhs, float4x3 rhs)
	{
		return new bool4x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator ==(float4x3 lhs, float rhs)
	{
		return new bool4x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator ==(float lhs, float4x3 rhs)
	{
		return new bool4x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator !=(float4x3 lhs, float4x3 rhs)
	{
		return new bool4x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator !=(float4x3 lhs, float rhs)
	{
		return new bool4x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator !=(float lhs, float4x3 rhs)
	{
		return new bool4x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(float4x3 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1))
		{
			return c2.Equals(rhs.c2);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((float4x3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"float4x3({c0.x}f, {c1.x}f, {c2.x}f,  {c0.y}f, {c1.y}f, {c2.y}f,  {c0.z}f, {c1.z}f, {c2.z}f,  {c0.w}f, {c1.w}f, {c2.w}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float4x3({c0.x.ToString(format, formatProvider)}f, {c1.x.ToString(format, formatProvider)}f, {c2.x.ToString(format, formatProvider)}f,  {c0.y.ToString(format, formatProvider)}f, {c1.y.ToString(format, formatProvider)}f, {c2.y.ToString(format, formatProvider)}f,  {c0.z.ToString(format, formatProvider)}f, {c1.z.ToString(format, formatProvider)}f, {c2.z.ToString(format, formatProvider)}f,  {c0.w.ToString(format, formatProvider)}f, {c1.w.ToString(format, formatProvider)}f, {c2.w.ToString(format, formatProvider)}f)";
	}
}
