using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct int2x4 : IEquatable<int2x4>, IFormattable
{
	public int2 c0;

	public int2 c1;

	public int2 c2;

	public int2 c3;

	public static readonly int2x4 zero;

	public unsafe ref int2 this[int index]
	{
		get
		{
			fixed (int2x4* ptr = &this)
			{
				return ref *(int2*)((byte*)ptr + (nint)index * (nint)sizeof(int2));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(int2 c0, int2 c1, int2 c2, int2 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13)
	{
		c0 = new int2(m00, m10);
		c1 = new int2(m01, m11);
		c2 = new int2(m02, m12);
		c3 = new int2(m03, m13);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(bool v)
	{
		c0 = math.select(new int2(0), new int2(1), v);
		c1 = math.select(new int2(0), new int2(1), v);
		c2 = math.select(new int2(0), new int2(1), v);
		c3 = math.select(new int2(0), new int2(1), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(bool2x4 v)
	{
		c0 = math.select(new int2(0), new int2(1), v.c0);
		c1 = math.select(new int2(0), new int2(1), v.c1);
		c2 = math.select(new int2(0), new int2(1), v.c2);
		c3 = math.select(new int2(0), new int2(1), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(uint v)
	{
		c0 = (int2)v;
		c1 = (int2)v;
		c2 = (int2)v;
		c3 = (int2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(uint2x4 v)
	{
		c0 = (int2)v.c0;
		c1 = (int2)v.c1;
		c2 = (int2)v.c2;
		c3 = (int2)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(float v)
	{
		c0 = (int2)v;
		c1 = (int2)v;
		c2 = (int2)v;
		c3 = (int2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(float2x4 v)
	{
		c0 = (int2)v.c0;
		c1 = (int2)v.c1;
		c2 = (int2)v.c2;
		c3 = (int2)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(double v)
	{
		c0 = (int2)v;
		c1 = (int2)v;
		c2 = (int2)v;
		c3 = (int2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2x4(double2x4 v)
	{
		c0 = (int2)v.c0;
		c1 = (int2)v.c1;
		c2 = (int2)v.c2;
		c3 = (int2)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator int2x4(int v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2x4(bool v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2x4(bool2x4 v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2x4(uint v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2x4(uint2x4 v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2x4(float v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2x4(float2x4 v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2x4(double v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2x4(double2x4 v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator *(int2x4 lhs, int2x4 rhs)
	{
		return new int2x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator *(int2x4 lhs, int rhs)
	{
		return new int2x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator *(int lhs, int2x4 rhs)
	{
		return new int2x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator +(int2x4 lhs, int2x4 rhs)
	{
		return new int2x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator +(int2x4 lhs, int rhs)
	{
		return new int2x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator +(int lhs, int2x4 rhs)
	{
		return new int2x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator -(int2x4 lhs, int2x4 rhs)
	{
		return new int2x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator -(int2x4 lhs, int rhs)
	{
		return new int2x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator -(int lhs, int2x4 rhs)
	{
		return new int2x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator /(int2x4 lhs, int2x4 rhs)
	{
		return new int2x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator /(int2x4 lhs, int rhs)
	{
		return new int2x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator /(int lhs, int2x4 rhs)
	{
		return new int2x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator %(int2x4 lhs, int2x4 rhs)
	{
		return new int2x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator %(int2x4 lhs, int rhs)
	{
		return new int2x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator %(int lhs, int2x4 rhs)
	{
		return new int2x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator ++(int2x4 val)
	{
		return new int2x4(++val.c0, ++val.c1, ++val.c2, ++val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator --(int2x4 val)
	{
		return new int2x4(--val.c0, --val.c1, --val.c2, --val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(int2x4 lhs, int2x4 rhs)
	{
		return new bool2x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(int2x4 lhs, int rhs)
	{
		return new bool2x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(int lhs, int2x4 rhs)
	{
		return new bool2x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(int2x4 lhs, int2x4 rhs)
	{
		return new bool2x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(int2x4 lhs, int rhs)
	{
		return new bool2x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(int lhs, int2x4 rhs)
	{
		return new bool2x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(int2x4 lhs, int2x4 rhs)
	{
		return new bool2x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(int2x4 lhs, int rhs)
	{
		return new bool2x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(int lhs, int2x4 rhs)
	{
		return new bool2x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(int2x4 lhs, int2x4 rhs)
	{
		return new bool2x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(int2x4 lhs, int rhs)
	{
		return new bool2x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(int lhs, int2x4 rhs)
	{
		return new bool2x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator -(int2x4 val)
	{
		return new int2x4(-val.c0, -val.c1, -val.c2, -val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator +(int2x4 val)
	{
		return new int2x4(+val.c0, +val.c1, +val.c2, +val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator <<(int2x4 x, int n)
	{
		return new int2x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator >>(int2x4 x, int n)
	{
		return new int2x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(int2x4 lhs, int2x4 rhs)
	{
		return new bool2x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(int2x4 lhs, int rhs)
	{
		return new bool2x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(int lhs, int2x4 rhs)
	{
		return new bool2x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(int2x4 lhs, int2x4 rhs)
	{
		return new bool2x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(int2x4 lhs, int rhs)
	{
		return new bool2x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(int lhs, int2x4 rhs)
	{
		return new bool2x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator ~(int2x4 val)
	{
		return new int2x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator &(int2x4 lhs, int2x4 rhs)
	{
		return new int2x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator &(int2x4 lhs, int rhs)
	{
		return new int2x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator &(int lhs, int2x4 rhs)
	{
		return new int2x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator |(int2x4 lhs, int2x4 rhs)
	{
		return new int2x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator |(int2x4 lhs, int rhs)
	{
		return new int2x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator |(int lhs, int2x4 rhs)
	{
		return new int2x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator ^(int2x4 lhs, int2x4 rhs)
	{
		return new int2x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator ^(int2x4 lhs, int rhs)
	{
		return new int2x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 operator ^(int lhs, int2x4 rhs)
	{
		return new int2x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int2x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((int2x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"int2x4({c0.x}, {c1.x}, {c2.x}, {c3.x},  {c0.y}, {c1.y}, {c2.y}, {c3.y})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"int2x4({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)}, {c3.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)}, {c3.y.ToString(format, formatProvider)})";
	}
}
