using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct double2x4 : IEquatable<double2x4>, IFormattable
{
	public double2 c0;

	public double2 c1;

	public double2 c2;

	public double2 c3;

	public static readonly double2x4 zero;

	public unsafe ref double2 this[int index]
	{
		get
		{
			fixed (double2x4* ptr = &this)
			{
				return ref *(double2*)((byte*)ptr + (nint)index * (nint)sizeof(double2));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(double2 c0, double2 c1, double2 c2, double2 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13)
	{
		c0 = new double2(m00, m10);
		c1 = new double2(m01, m11);
		c2 = new double2(m02, m12);
		c3 = new double2(m03, m13);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(double v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(bool v)
	{
		c0 = math.select(new double2(0.0), new double2(1.0), v);
		c1 = math.select(new double2(0.0), new double2(1.0), v);
		c2 = math.select(new double2(0.0), new double2(1.0), v);
		c3 = math.select(new double2(0.0), new double2(1.0), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(bool2x4 v)
	{
		c0 = math.select(new double2(0.0), new double2(1.0), v.c0);
		c1 = math.select(new double2(0.0), new double2(1.0), v.c1);
		c2 = math.select(new double2(0.0), new double2(1.0), v.c2);
		c3 = math.select(new double2(0.0), new double2(1.0), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(int2x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(uint2x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(float v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2x4(float2x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x4(double v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double2x4(bool v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double2x4(bool2x4 v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x4(int v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x4(int2x4 v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x4(uint v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x4(uint2x4 v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x4(float v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2x4(float2x4 v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator *(double2x4 lhs, double2x4 rhs)
	{
		return new double2x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator *(double2x4 lhs, double rhs)
	{
		return new double2x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator *(double lhs, double2x4 rhs)
	{
		return new double2x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator +(double2x4 lhs, double2x4 rhs)
	{
		return new double2x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator +(double2x4 lhs, double rhs)
	{
		return new double2x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator +(double lhs, double2x4 rhs)
	{
		return new double2x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator -(double2x4 lhs, double2x4 rhs)
	{
		return new double2x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator -(double2x4 lhs, double rhs)
	{
		return new double2x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator -(double lhs, double2x4 rhs)
	{
		return new double2x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator /(double2x4 lhs, double2x4 rhs)
	{
		return new double2x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator /(double2x4 lhs, double rhs)
	{
		return new double2x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator /(double lhs, double2x4 rhs)
	{
		return new double2x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator %(double2x4 lhs, double2x4 rhs)
	{
		return new double2x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator %(double2x4 lhs, double rhs)
	{
		return new double2x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator %(double lhs, double2x4 rhs)
	{
		return new double2x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator ++(double2x4 val)
	{
		return new double2x4(++val.c0, ++val.c1, ++val.c2, ++val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator --(double2x4 val)
	{
		return new double2x4(--val.c0, --val.c1, --val.c2, --val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(double2x4 lhs, double2x4 rhs)
	{
		return new bool2x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(double2x4 lhs, double rhs)
	{
		return new bool2x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(double lhs, double2x4 rhs)
	{
		return new bool2x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(double2x4 lhs, double2x4 rhs)
	{
		return new bool2x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(double2x4 lhs, double rhs)
	{
		return new bool2x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(double lhs, double2x4 rhs)
	{
		return new bool2x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(double2x4 lhs, double2x4 rhs)
	{
		return new bool2x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(double2x4 lhs, double rhs)
	{
		return new bool2x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(double lhs, double2x4 rhs)
	{
		return new bool2x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(double2x4 lhs, double2x4 rhs)
	{
		return new bool2x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(double2x4 lhs, double rhs)
	{
		return new bool2x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(double lhs, double2x4 rhs)
	{
		return new bool2x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator -(double2x4 val)
	{
		return new double2x4(-val.c0, -val.c1, -val.c2, -val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 operator +(double2x4 val)
	{
		return new double2x4(+val.c0, +val.c1, +val.c2, +val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(double2x4 lhs, double2x4 rhs)
	{
		return new bool2x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(double2x4 lhs, double rhs)
	{
		return new bool2x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(double lhs, double2x4 rhs)
	{
		return new bool2x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(double2x4 lhs, double2x4 rhs)
	{
		return new bool2x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(double2x4 lhs, double rhs)
	{
		return new bool2x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(double lhs, double2x4 rhs)
	{
		return new bool2x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(double2x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((double2x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"double2x4({c0.x}, {c1.x}, {c2.x}, {c3.x},  {c0.y}, {c1.y}, {c2.y}, {c3.y})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"double2x4({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)}, {c3.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)}, {c3.y.ToString(format, formatProvider)})";
	}
}
