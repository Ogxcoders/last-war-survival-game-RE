using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct int4x3 : IEquatable<int4x3>, IFormattable
{
	public int4 c0;

	public int4 c1;

	public int4 c2;

	public static readonly int4x3 zero;

	public unsafe ref int4 this[int index]
	{
		get
		{
			fixed (int4x3* ptr = &this)
			{
				return ref *(int4*)((byte*)ptr + (nint)index * (nint)sizeof(int4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(int4 c0, int4 c1, int4 c2)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(int m00, int m01, int m02, int m10, int m11, int m12, int m20, int m21, int m22, int m30, int m31, int m32)
	{
		c0 = new int4(m00, m10, m20, m30);
		c1 = new int4(m01, m11, m21, m31);
		c2 = new int4(m02, m12, m22, m32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(bool v)
	{
		c0 = math.select(new int4(0), new int4(1), v);
		c1 = math.select(new int4(0), new int4(1), v);
		c2 = math.select(new int4(0), new int4(1), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(bool4x3 v)
	{
		c0 = math.select(new int4(0), new int4(1), v.c0);
		c1 = math.select(new int4(0), new int4(1), v.c1);
		c2 = math.select(new int4(0), new int4(1), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(uint v)
	{
		c0 = (int4)v;
		c1 = (int4)v;
		c2 = (int4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(uint4x3 v)
	{
		c0 = (int4)v.c0;
		c1 = (int4)v.c1;
		c2 = (int4)v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(float v)
	{
		c0 = (int4)v;
		c1 = (int4)v;
		c2 = (int4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(float4x3 v)
	{
		c0 = (int4)v.c0;
		c1 = (int4)v.c1;
		c2 = (int4)v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(double v)
	{
		c0 = (int4)v;
		c1 = (int4)v;
		c2 = (int4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4x3(double4x3 v)
	{
		c0 = (int4)v.c0;
		c1 = (int4)v.c1;
		c2 = (int4)v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator int4x3(int v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4x3(bool v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4x3(bool4x3 v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4x3(uint v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4x3(uint4x3 v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4x3(float v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4x3(float4x3 v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4x3(double v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4x3(double4x3 v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator *(int4x3 lhs, int4x3 rhs)
	{
		return new int4x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator *(int4x3 lhs, int rhs)
	{
		return new int4x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator *(int lhs, int4x3 rhs)
	{
		return new int4x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator +(int4x3 lhs, int4x3 rhs)
	{
		return new int4x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator +(int4x3 lhs, int rhs)
	{
		return new int4x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator +(int lhs, int4x3 rhs)
	{
		return new int4x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator -(int4x3 lhs, int4x3 rhs)
	{
		return new int4x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator -(int4x3 lhs, int rhs)
	{
		return new int4x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator -(int lhs, int4x3 rhs)
	{
		return new int4x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator /(int4x3 lhs, int4x3 rhs)
	{
		return new int4x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator /(int4x3 lhs, int rhs)
	{
		return new int4x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator /(int lhs, int4x3 rhs)
	{
		return new int4x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator %(int4x3 lhs, int4x3 rhs)
	{
		return new int4x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator %(int4x3 lhs, int rhs)
	{
		return new int4x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator %(int lhs, int4x3 rhs)
	{
		return new int4x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator ++(int4x3 val)
	{
		return new int4x3(++val.c0, ++val.c1, ++val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator --(int4x3 val)
	{
		return new int4x3(--val.c0, --val.c1, --val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <(int4x3 lhs, int4x3 rhs)
	{
		return new bool4x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <(int4x3 lhs, int rhs)
	{
		return new bool4x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <(int lhs, int4x3 rhs)
	{
		return new bool4x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <=(int4x3 lhs, int4x3 rhs)
	{
		return new bool4x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <=(int4x3 lhs, int rhs)
	{
		return new bool4x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator <=(int lhs, int4x3 rhs)
	{
		return new bool4x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >(int4x3 lhs, int4x3 rhs)
	{
		return new bool4x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >(int4x3 lhs, int rhs)
	{
		return new bool4x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >(int lhs, int4x3 rhs)
	{
		return new bool4x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >=(int4x3 lhs, int4x3 rhs)
	{
		return new bool4x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >=(int4x3 lhs, int rhs)
	{
		return new bool4x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator >=(int lhs, int4x3 rhs)
	{
		return new bool4x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator -(int4x3 val)
	{
		return new int4x3(-val.c0, -val.c1, -val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator +(int4x3 val)
	{
		return new int4x3(+val.c0, +val.c1, +val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator <<(int4x3 x, int n)
	{
		return new int4x3(x.c0 << n, x.c1 << n, x.c2 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator >>(int4x3 x, int n)
	{
		return new int4x3(x.c0 >> n, x.c1 >> n, x.c2 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator ==(int4x3 lhs, int4x3 rhs)
	{
		return new bool4x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator ==(int4x3 lhs, int rhs)
	{
		return new bool4x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator ==(int lhs, int4x3 rhs)
	{
		return new bool4x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator !=(int4x3 lhs, int4x3 rhs)
	{
		return new bool4x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator !=(int4x3 lhs, int rhs)
	{
		return new bool4x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 operator !=(int lhs, int4x3 rhs)
	{
		return new bool4x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator ~(int4x3 val)
	{
		return new int4x3(~val.c0, ~val.c1, ~val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator &(int4x3 lhs, int4x3 rhs)
	{
		return new int4x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator &(int4x3 lhs, int rhs)
	{
		return new int4x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator &(int lhs, int4x3 rhs)
	{
		return new int4x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator |(int4x3 lhs, int4x3 rhs)
	{
		return new int4x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator |(int4x3 lhs, int rhs)
	{
		return new int4x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator |(int lhs, int4x3 rhs)
	{
		return new int4x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator ^(int4x3 lhs, int4x3 rhs)
	{
		return new int4x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator ^(int4x3 lhs, int rhs)
	{
		return new int4x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 operator ^(int lhs, int4x3 rhs)
	{
		return new int4x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int4x3 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1))
		{
			return c2.Equals(rhs.c2);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((int4x3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"int4x3({c0.x}, {c1.x}, {c2.x},  {c0.y}, {c1.y}, {c2.y},  {c0.z}, {c1.z}, {c2.z},  {c0.w}, {c1.w}, {c2.w})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"int4x3({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)}, {c2.z.ToString(format, formatProvider)},  {c0.w.ToString(format, formatProvider)}, {c1.w.ToString(format, formatProvider)}, {c2.w.ToString(format, formatProvider)})";
	}
}
