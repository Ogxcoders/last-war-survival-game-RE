using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct double3x2 : IEquatable<double3x2>, IFormattable
{
	public double3 c0;

	public double3 c1;

	public static readonly double3x2 zero;

	public unsafe ref double3 this[int index]
	{
		get
		{
			fixed (double3x2* ptr = &this)
			{
				return ref *(double3*)((byte*)ptr + (nint)index * (nint)sizeof(double3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(double3 c0, double3 c1)
	{
		this.c0 = c0;
		this.c1 = c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(double m00, double m01, double m10, double m11, double m20, double m21)
	{
		c0 = new double3(m00, m10, m20);
		c1 = new double3(m01, m11, m21);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(double v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(bool v)
	{
		c0 = math.select(new double3(0.0), new double3(1.0), v);
		c1 = math.select(new double3(0.0), new double3(1.0), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(bool3x2 v)
	{
		c0 = math.select(new double3(0.0), new double3(1.0), v.c0);
		c1 = math.select(new double3(0.0), new double3(1.0), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(int v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(int3x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(uint v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(uint3x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(float v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x2(float3x2 v)
	{
		c0 = v.c0;
		c1 = v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x2(double v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double3x2(bool v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double3x2(bool3x2 v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x2(int v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x2(int3x2 v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x2(uint v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x2(uint3x2 v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x2(float v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x2(float3x2 v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator *(double3x2 lhs, double3x2 rhs)
	{
		return new double3x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator *(double3x2 lhs, double rhs)
	{
		return new double3x2(lhs.c0 * rhs, lhs.c1 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator *(double lhs, double3x2 rhs)
	{
		return new double3x2(lhs * rhs.c0, lhs * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator +(double3x2 lhs, double3x2 rhs)
	{
		return new double3x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator +(double3x2 lhs, double rhs)
	{
		return new double3x2(lhs.c0 + rhs, lhs.c1 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator +(double lhs, double3x2 rhs)
	{
		return new double3x2(lhs + rhs.c0, lhs + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator -(double3x2 lhs, double3x2 rhs)
	{
		return new double3x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator -(double3x2 lhs, double rhs)
	{
		return new double3x2(lhs.c0 - rhs, lhs.c1 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator -(double lhs, double3x2 rhs)
	{
		return new double3x2(lhs - rhs.c0, lhs - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator /(double3x2 lhs, double3x2 rhs)
	{
		return new double3x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator /(double3x2 lhs, double rhs)
	{
		return new double3x2(lhs.c0 / rhs, lhs.c1 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator /(double lhs, double3x2 rhs)
	{
		return new double3x2(lhs / rhs.c0, lhs / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator %(double3x2 lhs, double3x2 rhs)
	{
		return new double3x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator %(double3x2 lhs, double rhs)
	{
		return new double3x2(lhs.c0 % rhs, lhs.c1 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator %(double lhs, double3x2 rhs)
	{
		return new double3x2(lhs % rhs.c0, lhs % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator ++(double3x2 val)
	{
		return new double3x2(++val.c0, ++val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator --(double3x2 val)
	{
		return new double3x2(--val.c0, --val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <(double3x2 lhs, double3x2 rhs)
	{
		return new bool3x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <(double3x2 lhs, double rhs)
	{
		return new bool3x2(lhs.c0 < rhs, lhs.c1 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <(double lhs, double3x2 rhs)
	{
		return new bool3x2(lhs < rhs.c0, lhs < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <=(double3x2 lhs, double3x2 rhs)
	{
		return new bool3x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <=(double3x2 lhs, double rhs)
	{
		return new bool3x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <=(double lhs, double3x2 rhs)
	{
		return new bool3x2(lhs <= rhs.c0, lhs <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >(double3x2 lhs, double3x2 rhs)
	{
		return new bool3x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >(double3x2 lhs, double rhs)
	{
		return new bool3x2(lhs.c0 > rhs, lhs.c1 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >(double lhs, double3x2 rhs)
	{
		return new bool3x2(lhs > rhs.c0, lhs > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >=(double3x2 lhs, double3x2 rhs)
	{
		return new bool3x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >=(double3x2 lhs, double rhs)
	{
		return new bool3x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >=(double lhs, double3x2 rhs)
	{
		return new bool3x2(lhs >= rhs.c0, lhs >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator -(double3x2 val)
	{
		return new double3x2(-val.c0, -val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 operator +(double3x2 val)
	{
		return new double3x2(+val.c0, +val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(double3x2 lhs, double3x2 rhs)
	{
		return new bool3x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(double3x2 lhs, double rhs)
	{
		return new bool3x2(lhs.c0 == rhs, lhs.c1 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(double lhs, double3x2 rhs)
	{
		return new bool3x2(lhs == rhs.c0, lhs == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(double3x2 lhs, double3x2 rhs)
	{
		return new bool3x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(double3x2 lhs, double rhs)
	{
		return new bool3x2(lhs.c0 != rhs, lhs.c1 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(double lhs, double3x2 rhs)
	{
		return new bool3x2(lhs != rhs.c0, lhs != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(double3x2 rhs)
	{
		if (c0.Equals(rhs.c0))
		{
			return c1.Equals(rhs.c1);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((double3x2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"double3x2({c0.x}, {c1.x},  {c0.y}, {c1.y},  {c0.z}, {c1.z})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"double3x2({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)})";
	}
}
