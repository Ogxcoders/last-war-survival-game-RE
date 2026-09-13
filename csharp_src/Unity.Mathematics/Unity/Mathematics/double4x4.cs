using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct double4x4 : IEquatable<double4x4>, IFormattable
{
	public double4 c0;

	public double4 c1;

	public double4 c2;

	public double4 c3;

	public static readonly double4x4 identity = new double4x4(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);

	public static readonly double4x4 zero;

	public unsafe ref double4 this[int index]
	{
		get
		{
			fixed (double4x4* ptr = &this)
			{
				return ref *(double4*)((byte*)ptr + (nint)index * (nint)sizeof(double4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(double4 c0, double4 c1, double4 c2, double4 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
	{
		c0 = new double4(m00, m10, m20, m30);
		c1 = new double4(m01, m11, m21, m31);
		c2 = new double4(m02, m12, m22, m32);
		c3 = new double4(m03, m13, m23, m33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(double v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(bool v)
	{
		c0 = math.select(new double4(0.0), new double4(1.0), v);
		c1 = math.select(new double4(0.0), new double4(1.0), v);
		c2 = math.select(new double4(0.0), new double4(1.0), v);
		c3 = math.select(new double4(0.0), new double4(1.0), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(bool4x4 v)
	{
		c0 = math.select(new double4(0.0), new double4(1.0), v.c0);
		c1 = math.select(new double4(0.0), new double4(1.0), v.c1);
		c2 = math.select(new double4(0.0), new double4(1.0), v.c2);
		c3 = math.select(new double4(0.0), new double4(1.0), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(int4x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(uint4x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(float v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4x4(float4x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x4(double v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double4x4(bool v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double4x4(bool4x4 v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x4(int v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x4(int4x4 v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x4(uint v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x4(uint4x4 v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x4(float v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double4x4(float4x4 v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator *(double4x4 lhs, double4x4 rhs)
	{
		return new double4x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator *(double4x4 lhs, double rhs)
	{
		return new double4x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator *(double lhs, double4x4 rhs)
	{
		return new double4x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator +(double4x4 lhs, double4x4 rhs)
	{
		return new double4x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator +(double4x4 lhs, double rhs)
	{
		return new double4x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator +(double lhs, double4x4 rhs)
	{
		return new double4x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator -(double4x4 lhs, double4x4 rhs)
	{
		return new double4x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator -(double4x4 lhs, double rhs)
	{
		return new double4x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator -(double lhs, double4x4 rhs)
	{
		return new double4x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator /(double4x4 lhs, double4x4 rhs)
	{
		return new double4x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator /(double4x4 lhs, double rhs)
	{
		return new double4x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator /(double lhs, double4x4 rhs)
	{
		return new double4x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator %(double4x4 lhs, double4x4 rhs)
	{
		return new double4x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator %(double4x4 lhs, double rhs)
	{
		return new double4x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator %(double lhs, double4x4 rhs)
	{
		return new double4x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator ++(double4x4 val)
	{
		return new double4x4(++val.c0, ++val.c1, ++val.c2, ++val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator --(double4x4 val)
	{
		return new double4x4(--val.c0, --val.c1, --val.c2, --val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <(double4x4 lhs, double4x4 rhs)
	{
		return new bool4x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <(double4x4 lhs, double rhs)
	{
		return new bool4x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <(double lhs, double4x4 rhs)
	{
		return new bool4x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <=(double4x4 lhs, double4x4 rhs)
	{
		return new bool4x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <=(double4x4 lhs, double rhs)
	{
		return new bool4x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <=(double lhs, double4x4 rhs)
	{
		return new bool4x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >(double4x4 lhs, double4x4 rhs)
	{
		return new bool4x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >(double4x4 lhs, double rhs)
	{
		return new bool4x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >(double lhs, double4x4 rhs)
	{
		return new bool4x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >=(double4x4 lhs, double4x4 rhs)
	{
		return new bool4x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >=(double4x4 lhs, double rhs)
	{
		return new bool4x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >=(double lhs, double4x4 rhs)
	{
		return new bool4x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator -(double4x4 val)
	{
		return new double4x4(-val.c0, -val.c1, -val.c2, -val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 operator +(double4x4 val)
	{
		return new double4x4(+val.c0, +val.c1, +val.c2, +val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(double4x4 lhs, double4x4 rhs)
	{
		return new bool4x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(double4x4 lhs, double rhs)
	{
		return new bool4x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(double lhs, double4x4 rhs)
	{
		return new bool4x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(double4x4 lhs, double4x4 rhs)
	{
		return new bool4x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(double4x4 lhs, double rhs)
	{
		return new bool4x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(double lhs, double4x4 rhs)
	{
		return new bool4x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(double4x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((double4x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"double4x4({c0.x}, {c1.x}, {c2.x}, {c3.x},  {c0.y}, {c1.y}, {c2.y}, {c3.y},  {c0.z}, {c1.z}, {c2.z}, {c3.z},  {c0.w}, {c1.w}, {c2.w}, {c3.w})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"double4x4({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)}, {c3.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)}, {c3.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)}, {c2.z.ToString(format, formatProvider)}, {c3.z.ToString(format, formatProvider)},  {c0.w.ToString(format, formatProvider)}, {c1.w.ToString(format, formatProvider)}, {c2.w.ToString(format, formatProvider)}, {c3.w.ToString(format, formatProvider)})";
	}
}
