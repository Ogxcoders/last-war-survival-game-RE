using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct double3x3 : IEquatable<double3x3>, IFormattable
{
	public double3 c0;

	public double3 c1;

	public double3 c2;

	public static readonly double3x3 identity = new double3x3(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);

	public static readonly double3x3 zero;

	public unsafe ref double3 this[int index]
	{
		get
		{
			fixed (double3x3* ptr = &this)
			{
				return ref *(double3*)((byte*)ptr + (nint)index * (nint)sizeof(double3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(double3 c0, double3 c1, double3 c2)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22)
	{
		c0 = new double3(m00, m10, m20);
		c1 = new double3(m01, m11, m21);
		c2 = new double3(m02, m12, m22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(double v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(bool v)
	{
		c0 = math.select(new double3(0.0), new double3(1.0), v);
		c1 = math.select(new double3(0.0), new double3(1.0), v);
		c2 = math.select(new double3(0.0), new double3(1.0), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(bool3x3 v)
	{
		c0 = math.select(new double3(0.0), new double3(1.0), v.c0);
		c1 = math.select(new double3(0.0), new double3(1.0), v.c1);
		c2 = math.select(new double3(0.0), new double3(1.0), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(int3x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(uint3x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(float v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3x3(float3x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x3(double v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double3x3(bool v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double3x3(bool3x3 v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x3(int v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x3(int3x3 v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x3(uint v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x3(uint3x3 v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x3(float v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double3x3(float3x3 v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator *(double3x3 lhs, double3x3 rhs)
	{
		return new double3x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator *(double3x3 lhs, double rhs)
	{
		return new double3x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator *(double lhs, double3x3 rhs)
	{
		return new double3x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator +(double3x3 lhs, double3x3 rhs)
	{
		return new double3x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator +(double3x3 lhs, double rhs)
	{
		return new double3x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator +(double lhs, double3x3 rhs)
	{
		return new double3x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator -(double3x3 lhs, double3x3 rhs)
	{
		return new double3x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator -(double3x3 lhs, double rhs)
	{
		return new double3x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator -(double lhs, double3x3 rhs)
	{
		return new double3x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator /(double3x3 lhs, double3x3 rhs)
	{
		return new double3x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator /(double3x3 lhs, double rhs)
	{
		return new double3x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator /(double lhs, double3x3 rhs)
	{
		return new double3x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator %(double3x3 lhs, double3x3 rhs)
	{
		return new double3x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator %(double3x3 lhs, double rhs)
	{
		return new double3x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator %(double lhs, double3x3 rhs)
	{
		return new double3x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator ++(double3x3 val)
	{
		return new double3x3(++val.c0, ++val.c1, ++val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator --(double3x3 val)
	{
		return new double3x3(--val.c0, --val.c1, --val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <(double3x3 lhs, double3x3 rhs)
	{
		return new bool3x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <(double3x3 lhs, double rhs)
	{
		return new bool3x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <(double lhs, double3x3 rhs)
	{
		return new bool3x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <=(double3x3 lhs, double3x3 rhs)
	{
		return new bool3x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <=(double3x3 lhs, double rhs)
	{
		return new bool3x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <=(double lhs, double3x3 rhs)
	{
		return new bool3x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >(double3x3 lhs, double3x3 rhs)
	{
		return new bool3x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >(double3x3 lhs, double rhs)
	{
		return new bool3x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >(double lhs, double3x3 rhs)
	{
		return new bool3x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >=(double3x3 lhs, double3x3 rhs)
	{
		return new bool3x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >=(double3x3 lhs, double rhs)
	{
		return new bool3x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >=(double lhs, double3x3 rhs)
	{
		return new bool3x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator -(double3x3 val)
	{
		return new double3x3(-val.c0, -val.c1, -val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 operator +(double3x3 val)
	{
		return new double3x3(+val.c0, +val.c1, +val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator ==(double3x3 lhs, double3x3 rhs)
	{
		return new bool3x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator ==(double3x3 lhs, double rhs)
	{
		return new bool3x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator ==(double lhs, double3x3 rhs)
	{
		return new bool3x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator !=(double3x3 lhs, double3x3 rhs)
	{
		return new bool3x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator !=(double3x3 lhs, double rhs)
	{
		return new bool3x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator !=(double lhs, double3x3 rhs)
	{
		return new bool3x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(double3x3 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1))
		{
			return c2.Equals(rhs.c2);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((double3x3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"double3x3({c0.x}, {c1.x}, {c2.x},  {c0.y}, {c1.y}, {c2.y},  {c0.z}, {c1.z}, {c2.z})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"double3x3({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)}, {c2.z.ToString(format, formatProvider)})";
	}
}
