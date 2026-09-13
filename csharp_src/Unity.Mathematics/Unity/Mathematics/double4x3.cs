using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct double4x3 : IEquatable<double4x3>, IFormattable
{
	public double4 c0;

	public double4 c1;

	public double4 c2;

	public static readonly double4x3 zero;

	public unsafe ref double4 this[int index]
	{
		get
		{
			fixed (double4x3* ptr = &this)
			{
				return ref *(double4*)((byte*)ptr + (nint)index * (nint)sizeof(double4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(double4 c0, double4 c1, double4 c2)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22, double m30, double m31, double m32)
	{
		c0 = new double4(m00, m10, m20, m30);
		c1 = new double4(m01, m11, m21, m31);
		c2 = new double4(m02, m12, m22, m32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(double v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(bool v)
	{
		c0 = math.select(new double4(0.0), new double4(1.0), v);
		c1 = math.select(new double4(0.0), new double4(1.0), v);
		c2 = math.select(new double4(0.0), new double4(1.0), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(bool4x3 v)
	{
		c0 = math.select(new double4(0.0), new double4(1.0), v.c0);
		c1 = math.select(new double4(0.0), new double4(1.0), v.c1);
		c2 = math.select(new double4(0.0), new double4(1.0), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(int4x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(uint4x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(float v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x3(float4x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x3(double v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double4x3(bool v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double4x3(bool4x3 v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x3(int v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x3(int4x3 v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x3(uint v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x3(uint4x3 v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x3(float v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x3(float4x3 v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator *(double4x3 lhs, double4x3 rhs)
	{
		return new double4x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator *(double4x3 lhs, double rhs)
	{
		return new double4x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator *(double lhs, double4x3 rhs)
	{
		return new double4x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator +(double4x3 lhs, double4x3 rhs)
	{
		return new double4x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator +(double4x3 lhs, double rhs)
	{
		return new double4x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator +(double lhs, double4x3 rhs)
	{
		return new double4x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator -(double4x3 lhs, double4x3 rhs)
	{
		return new double4x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator -(double4x3 lhs, double rhs)
	{
		return new double4x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator -(double lhs, double4x3 rhs)
	{
		return new double4x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator /(double4x3 lhs, double4x3 rhs)
	{
		return new double4x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator /(double4x3 lhs, double rhs)
	{
		return new double4x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator /(double lhs, double4x3 rhs)
	{
		return new double4x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator %(double4x3 lhs, double4x3 rhs)
	{
		return new double4x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator %(double4x3 lhs, double rhs)
	{
		return new double4x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator %(double lhs, double4x3 rhs)
	{
		return new double4x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator ++(double4x3 val)
	{
		return new double4x3(++val.c0, ++val.c1, ++val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator --(double4x3 val)
	{
		return new double4x3(--val.c0, --val.c1, --val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <(double4x3 lhs, double4x3 rhs)
	{
		return new bool4x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <(double4x3 lhs, double rhs)
	{
		return new bool4x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <(double lhs, double4x3 rhs)
	{
		return new bool4x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <=(double4x3 lhs, double4x3 rhs)
	{
		return new bool4x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <=(double4x3 lhs, double rhs)
	{
		return new bool4x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <=(double lhs, double4x3 rhs)
	{
		return new bool4x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >(double4x3 lhs, double4x3 rhs)
	{
		return new bool4x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >(double4x3 lhs, double rhs)
	{
		return new bool4x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >(double lhs, double4x3 rhs)
	{
		return new bool4x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >=(double4x3 lhs, double4x3 rhs)
	{
		return new bool4x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >=(double4x3 lhs, double rhs)
	{
		return new bool4x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >=(double lhs, double4x3 rhs)
	{
		return new bool4x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator -(double4x3 val)
	{
		return new double4x3(-val.c0, -val.c1, -val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 operator +(double4x3 val)
	{
		return new double4x3(+val.c0, +val.c1, +val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator ==(double4x3 lhs, double4x3 rhs)
	{
		return new bool4x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator ==(double4x3 lhs, double rhs)
	{
		return new bool4x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator ==(double lhs, double4x3 rhs)
	{
		return new bool4x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator !=(double4x3 lhs, double4x3 rhs)
	{
		return new bool4x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator !=(double4x3 lhs, double rhs)
	{
		return new bool4x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator !=(double lhs, double4x3 rhs)
	{
		return new bool4x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(double4x3 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1))
		{
			return c2.Equals(rhs.c2);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((double4x3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"double4x3({c0.x}, {c1.x}, {c2.x},  {c0.y}, {c1.y}, {c2.y},  {c0.z}, {c1.z}, {c2.z},  {c0.w}, {c1.w}, {c2.w})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"double4x3({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)}, {c2.z.ToString(format, formatProvider)},  {c0.w.ToString(format, formatProvider)}, {c1.w.ToString(format, formatProvider)}, {c2.w.ToString(format, formatProvider)})";
	}
}
