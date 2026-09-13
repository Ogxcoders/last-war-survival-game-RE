using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct double4x2 : IEquatable<double4x2>, IFormattable
{
	public double4 c0;

	public double4 c1;

	public static readonly double4x2 zero;

	public unsafe ref double4 this[int index]
	{
		get
		{
			fixed (double4x2* ptr = &this)
			{
				return ref *(double4*)((byte*)ptr + (nint)index * (nint)sizeof(double4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(double4 c0, double4 c1)
	{
		this.c0 = c0;
		this.c1 = c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(double m00, double m01, double m10, double m11, double m20, double m21, double m30, double m31)
	{
		c0 = new double4(m00, m10, m20, m30);
		c1 = new double4(m01, m11, m21, m31);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(double v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(bool v)
	{
		c0 = math.select(new double4(0.0), new double4(1.0), v);
		c1 = math.select(new double4(0.0), new double4(1.0), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(bool4x2 v)
	{
		c0 = math.select(new double4(0.0), new double4(1.0), v.c0);
		c1 = math.select(new double4(0.0), new double4(1.0), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(int v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(int4x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(uint v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(uint4x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(float v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x2(float4x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x2(double v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double4x2(bool v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double4x2(bool4x2 v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x2(int v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x2(int4x2 v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x2(uint v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x2(uint4x2 v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x2(float v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x2(float4x2 v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator *(double4x2 lhs, double4x2 rhs)
	{
		return new double4x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator *(double4x2 lhs, double rhs)
	{
		return new double4x2(lhs.c0 * rhs, lhs.c1 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator *(double lhs, double4x2 rhs)
	{
		return new double4x2(lhs * rhs.c0, lhs * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator +(double4x2 lhs, double4x2 rhs)
	{
		return new double4x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator +(double4x2 lhs, double rhs)
	{
		return new double4x2(lhs.c0 + rhs, lhs.c1 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator +(double lhs, double4x2 rhs)
	{
		return new double4x2(lhs + rhs.c0, lhs + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator -(double4x2 lhs, double4x2 rhs)
	{
		return new double4x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator -(double4x2 lhs, double rhs)
	{
		return new double4x2(lhs.c0 - rhs, lhs.c1 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator -(double lhs, double4x2 rhs)
	{
		return new double4x2(lhs - rhs.c0, lhs - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator /(double4x2 lhs, double4x2 rhs)
	{
		return new double4x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator /(double4x2 lhs, double rhs)
	{
		return new double4x2(lhs.c0 / rhs, lhs.c1 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator /(double lhs, double4x2 rhs)
	{
		return new double4x2(lhs / rhs.c0, lhs / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator %(double4x2 lhs, double4x2 rhs)
	{
		return new double4x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator %(double4x2 lhs, double rhs)
	{
		return new double4x2(lhs.c0 % rhs, lhs.c1 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator %(double lhs, double4x2 rhs)
	{
		return new double4x2(lhs % rhs.c0, lhs % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator ++(double4x2 val)
	{
		return new double4x2(++val.c0, ++val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator --(double4x2 val)
	{
		return new double4x2(--val.c0, --val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <(double4x2 lhs, double4x2 rhs)
	{
		return new bool4x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <(double4x2 lhs, double rhs)
	{
		return new bool4x2(lhs.c0 < rhs, lhs.c1 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <(double lhs, double4x2 rhs)
	{
		return new bool4x2(lhs < rhs.c0, lhs < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <=(double4x2 lhs, double4x2 rhs)
	{
		return new bool4x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <=(double4x2 lhs, double rhs)
	{
		return new bool4x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <=(double lhs, double4x2 rhs)
	{
		return new bool4x2(lhs <= rhs.c0, lhs <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >(double4x2 lhs, double4x2 rhs)
	{
		return new bool4x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >(double4x2 lhs, double rhs)
	{
		return new bool4x2(lhs.c0 > rhs, lhs.c1 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >(double lhs, double4x2 rhs)
	{
		return new bool4x2(lhs > rhs.c0, lhs > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >=(double4x2 lhs, double4x2 rhs)
	{
		return new bool4x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >=(double4x2 lhs, double rhs)
	{
		return new bool4x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >=(double lhs, double4x2 rhs)
	{
		return new bool4x2(lhs >= rhs.c0, lhs >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator -(double4x2 val)
	{
		return new double4x2(-val.c0, -val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 operator +(double4x2 val)
	{
		return new double4x2(+val.c0, +val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(double4x2 lhs, double4x2 rhs)
	{
		return new bool4x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(double4x2 lhs, double rhs)
	{
		return new bool4x2(lhs.c0 == rhs, lhs.c1 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(double lhs, double4x2 rhs)
	{
		return new bool4x2(lhs == rhs.c0, lhs == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(double4x2 lhs, double4x2 rhs)
	{
		return new bool4x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(double4x2 lhs, double rhs)
	{
		return new bool4x2(lhs.c0 != rhs, lhs.c1 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(double lhs, double4x2 rhs)
	{
		return new bool4x2(lhs != rhs.c0, lhs != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(double4x2 rhs)
	{
		if (c0.Equals(rhs.c0))
		{
			return c1.Equals(rhs.c1);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((double4x2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"double4x2({c0.x}, {c1.x},  {c0.y}, {c1.y},  {c0.z}, {c1.z},  {c0.w}, {c1.w})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"double4x2({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)},  {c0.w.ToString(format, formatProvider)}, {c1.w.ToString(format, formatProvider)})";
	}
}
