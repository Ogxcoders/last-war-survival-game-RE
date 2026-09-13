using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct double2x3 : IEquatable<double2x3>, IFormattable
{
	public double2 c0;

	public double2 c1;

	public double2 c2;

	public static readonly double2x3 zero;

	public unsafe ref double2 this[int index]
	{
		get
		{
			fixed (double2x3* ptr = &this)
			{
				return ref *(double2*)((byte*)ptr + (nint)index * (nint)sizeof(double2));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(double2 c0, double2 c1, double2 c2)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(double m00, double m01, double m02, double m10, double m11, double m12)
	{
		c0 = new double2(m00, m10);
		c1 = new double2(m01, m11);
		c2 = new double2(m02, m12);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(double v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(bool v)
	{
		c0 = math.select(new double2(0.0), new double2(1.0), v);
		c1 = math.select(new double2(0.0), new double2(1.0), v);
		c2 = math.select(new double2(0.0), new double2(1.0), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(bool2x3 v)
	{
		c0 = math.select(new double2(0.0), new double2(1.0), v.c0);
		c1 = math.select(new double2(0.0), new double2(1.0), v.c1);
		c2 = math.select(new double2(0.0), new double2(1.0), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(int2x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(uint2x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(float v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x3(float2x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x3(double v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double2x3(bool v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double2x3(bool2x3 v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x3(int v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x3(int2x3 v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x3(uint v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x3(uint2x3 v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x3(float v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x3(float2x3 v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator *(double2x3 lhs, double2x3 rhs)
	{
		return new double2x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator *(double2x3 lhs, double rhs)
	{
		return new double2x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator *(double lhs, double2x3 rhs)
	{
		return new double2x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator +(double2x3 lhs, double2x3 rhs)
	{
		return new double2x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator +(double2x3 lhs, double rhs)
	{
		return new double2x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator +(double lhs, double2x3 rhs)
	{
		return new double2x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator -(double2x3 lhs, double2x3 rhs)
	{
		return new double2x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator -(double2x3 lhs, double rhs)
	{
		return new double2x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator -(double lhs, double2x3 rhs)
	{
		return new double2x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator /(double2x3 lhs, double2x3 rhs)
	{
		return new double2x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator /(double2x3 lhs, double rhs)
	{
		return new double2x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator /(double lhs, double2x3 rhs)
	{
		return new double2x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator %(double2x3 lhs, double2x3 rhs)
	{
		return new double2x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator %(double2x3 lhs, double rhs)
	{
		return new double2x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator %(double lhs, double2x3 rhs)
	{
		return new double2x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator ++(double2x3 val)
	{
		return new double2x3(++val.c0, ++val.c1, ++val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator --(double2x3 val)
	{
		return new double2x3(--val.c0, --val.c1, --val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator <(double2x3 lhs, double2x3 rhs)
	{
		return new bool2x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator <(double2x3 lhs, double rhs)
	{
		return new bool2x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator <(double lhs, double2x3 rhs)
	{
		return new bool2x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator <=(double2x3 lhs, double2x3 rhs)
	{
		return new bool2x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator <=(double2x3 lhs, double rhs)
	{
		return new bool2x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator <=(double lhs, double2x3 rhs)
	{
		return new bool2x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator >(double2x3 lhs, double2x3 rhs)
	{
		return new bool2x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator >(double2x3 lhs, double rhs)
	{
		return new bool2x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator >(double lhs, double2x3 rhs)
	{
		return new bool2x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator >=(double2x3 lhs, double2x3 rhs)
	{
		return new bool2x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator >=(double2x3 lhs, double rhs)
	{
		return new bool2x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator >=(double lhs, double2x3 rhs)
	{
		return new bool2x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator -(double2x3 val)
	{
		return new double2x3(-val.c0, -val.c1, -val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 operator +(double2x3 val)
	{
		return new double2x3(+val.c0, +val.c1, +val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator ==(double2x3 lhs, double2x3 rhs)
	{
		return new bool2x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator ==(double2x3 lhs, double rhs)
	{
		return new bool2x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator ==(double lhs, double2x3 rhs)
	{
		return new bool2x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator !=(double2x3 lhs, double2x3 rhs)
	{
		return new bool2x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator !=(double2x3 lhs, double rhs)
	{
		return new bool2x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 operator !=(double lhs, double2x3 rhs)
	{
		return new bool2x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(double2x3 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1))
		{
			return c2.Equals(rhs.c2);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((double2x3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"double2x3({c0.x}, {c1.x}, {c2.x},  {c0.y}, {c1.y}, {c2.y})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"double2x3({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)})";
	}
}
