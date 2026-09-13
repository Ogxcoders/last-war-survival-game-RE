using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct int3x4 : IEquatable<int3x4>, IFormattable
{
	public int3 c0;

	public int3 c1;

	public int3 c2;

	public int3 c3;

	public static readonly int3x4 zero;

	public unsafe ref int3 this[int index]
	{
		get
		{
			fixed (int3x4* ptr = &this)
			{
				return ref *(int3*)((byte*)ptr + (nint)index * (nint)sizeof(int3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(int3 c0, int3 c1, int3 c2, int3 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13, int m20, int m21, int m22, int m23)
	{
		c0 = new int3(m00, m10, m20);
		c1 = new int3(m01, m11, m21);
		c2 = new int3(m02, m12, m22);
		c3 = new int3(m03, m13, m23);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(bool v)
	{
		c0 = math.select(new int3(0), new int3(1), v);
		c1 = math.select(new int3(0), new int3(1), v);
		c2 = math.select(new int3(0), new int3(1), v);
		c3 = math.select(new int3(0), new int3(1), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(bool3x4 v)
	{
		c0 = math.select(new int3(0), new int3(1), v.c0);
		c1 = math.select(new int3(0), new int3(1), v.c1);
		c2 = math.select(new int3(0), new int3(1), v.c2);
		c3 = math.select(new int3(0), new int3(1), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(uint v)
	{
		c0 = (int3)v;
		c1 = (int3)v;
		c2 = (int3)v;
		c3 = (int3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(uint3x4 v)
	{
		c0 = (int3)v.c0;
		c1 = (int3)v.c1;
		c2 = (int3)v.c2;
		c3 = (int3)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(float v)
	{
		c0 = (int3)v;
		c1 = (int3)v;
		c2 = (int3)v;
		c3 = (int3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(float3x4 v)
	{
		c0 = (int3)v.c0;
		c1 = (int3)v.c1;
		c2 = (int3)v.c2;
		c3 = (int3)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(double v)
	{
		c0 = (int3)v;
		c1 = (int3)v;
		c2 = (int3)v;
		c3 = (int3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3x4(double3x4 v)
	{
		c0 = (int3)v.c0;
		c1 = (int3)v.c1;
		c2 = (int3)v.c2;
		c3 = (int3)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator int3x4(int v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3x4(bool v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3x4(bool3x4 v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3x4(uint v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3x4(uint3x4 v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3x4(float v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3x4(float3x4 v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3x4(double v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3x4(double3x4 v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator *(int3x4 lhs, int3x4 rhs)
	{
		return new int3x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator *(int3x4 lhs, int rhs)
	{
		return new int3x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator *(int lhs, int3x4 rhs)
	{
		return new int3x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator +(int3x4 lhs, int3x4 rhs)
	{
		return new int3x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator +(int3x4 lhs, int rhs)
	{
		return new int3x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator +(int lhs, int3x4 rhs)
	{
		return new int3x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator -(int3x4 lhs, int3x4 rhs)
	{
		return new int3x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator -(int3x4 lhs, int rhs)
	{
		return new int3x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator -(int lhs, int3x4 rhs)
	{
		return new int3x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator /(int3x4 lhs, int3x4 rhs)
	{
		return new int3x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator /(int3x4 lhs, int rhs)
	{
		return new int3x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator /(int lhs, int3x4 rhs)
	{
		return new int3x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator %(int3x4 lhs, int3x4 rhs)
	{
		return new int3x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator %(int3x4 lhs, int rhs)
	{
		return new int3x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator %(int lhs, int3x4 rhs)
	{
		return new int3x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator ++(int3x4 val)
	{
		return new int3x4(++val.c0, ++val.c1, ++val.c2, ++val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator --(int3x4 val)
	{
		return new int3x4(--val.c0, --val.c1, --val.c2, --val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <(int3x4 lhs, int3x4 rhs)
	{
		return new bool3x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <(int3x4 lhs, int rhs)
	{
		return new bool3x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <(int lhs, int3x4 rhs)
	{
		return new bool3x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <=(int3x4 lhs, int3x4 rhs)
	{
		return new bool3x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <=(int3x4 lhs, int rhs)
	{
		return new bool3x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <=(int lhs, int3x4 rhs)
	{
		return new bool3x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >(int3x4 lhs, int3x4 rhs)
	{
		return new bool3x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >(int3x4 lhs, int rhs)
	{
		return new bool3x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >(int lhs, int3x4 rhs)
	{
		return new bool3x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >=(int3x4 lhs, int3x4 rhs)
	{
		return new bool3x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >=(int3x4 lhs, int rhs)
	{
		return new bool3x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >=(int lhs, int3x4 rhs)
	{
		return new bool3x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator -(int3x4 val)
	{
		return new int3x4(-val.c0, -val.c1, -val.c2, -val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator +(int3x4 val)
	{
		return new int3x4(+val.c0, +val.c1, +val.c2, +val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator <<(int3x4 x, int n)
	{
		return new int3x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator >>(int3x4 x, int n)
	{
		return new int3x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ==(int3x4 lhs, int3x4 rhs)
	{
		return new bool3x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ==(int3x4 lhs, int rhs)
	{
		return new bool3x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ==(int lhs, int3x4 rhs)
	{
		return new bool3x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !=(int3x4 lhs, int3x4 rhs)
	{
		return new bool3x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !=(int3x4 lhs, int rhs)
	{
		return new bool3x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !=(int lhs, int3x4 rhs)
	{
		return new bool3x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator ~(int3x4 val)
	{
		return new int3x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator &(int3x4 lhs, int3x4 rhs)
	{
		return new int3x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator &(int3x4 lhs, int rhs)
	{
		return new int3x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator &(int lhs, int3x4 rhs)
	{
		return new int3x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator |(int3x4 lhs, int3x4 rhs)
	{
		return new int3x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator |(int3x4 lhs, int rhs)
	{
		return new int3x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator |(int lhs, int3x4 rhs)
	{
		return new int3x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator ^(int3x4 lhs, int3x4 rhs)
	{
		return new int3x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator ^(int3x4 lhs, int rhs)
	{
		return new int3x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 operator ^(int lhs, int3x4 rhs)
	{
		return new int3x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int3x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((int3x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"int3x4({c0.x}, {c1.x}, {c2.x}, {c3.x},  {c0.y}, {c1.y}, {c2.y}, {c3.y},  {c0.z}, {c1.z}, {c2.z}, {c3.z})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"int3x4({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)}, {c3.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)}, {c3.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)}, {c2.z.ToString(format, formatProvider)}, {c3.z.ToString(format, formatProvider)})";
	}
}
