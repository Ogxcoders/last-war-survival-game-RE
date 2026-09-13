using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct uint2x4 : IEquatable<uint2x4>, IFormattable
{
	public uint2 c0;

	public uint2 c1;

	public uint2 c2;

	public uint2 c3;

	public static readonly uint2x4 zero;

	public unsafe ref uint2 this[int index]
	{
		get
		{
			fixed (uint2x4* ptr = &this)
			{
				return ref *(uint2*)((byte*)ptr + (nint)index * (nint)sizeof(uint2));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(uint2 c0, uint2 c1, uint2 c2, uint2 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13)
	{
		c0 = new uint2(m00, m10);
		c1 = new uint2(m01, m11);
		c2 = new uint2(m02, m12);
		c3 = new uint2(m03, m13);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(bool v)
	{
		c0 = math.select(new uint2(0u), new uint2(1u), v);
		c1 = math.select(new uint2(0u), new uint2(1u), v);
		c2 = math.select(new uint2(0u), new uint2(1u), v);
		c3 = math.select(new uint2(0u), new uint2(1u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(bool2x4 v)
	{
		c0 = math.select(new uint2(0u), new uint2(1u), v.c0);
		c1 = math.select(new uint2(0u), new uint2(1u), v.c1);
		c2 = math.select(new uint2(0u), new uint2(1u), v.c2);
		c3 = math.select(new uint2(0u), new uint2(1u), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(int v)
	{
		c0 = (uint2)v;
		c1 = (uint2)v;
		c2 = (uint2)v;
		c3 = (uint2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(int2x4 v)
	{
		c0 = (uint2)v.c0;
		c1 = (uint2)v.c1;
		c2 = (uint2)v.c2;
		c3 = (uint2)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(float v)
	{
		c0 = (uint2)v;
		c1 = (uint2)v;
		c2 = (uint2)v;
		c3 = (uint2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(float2x4 v)
	{
		c0 = (uint2)v.c0;
		c1 = (uint2)v.c1;
		c2 = (uint2)v.c2;
		c3 = (uint2)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(double v)
	{
		c0 = (uint2)v;
		c1 = (uint2)v;
		c2 = (uint2)v;
		c3 = (uint2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x4(double2x4 v)
	{
		c0 = (uint2)v.c0;
		c1 = (uint2)v.c1;
		c2 = (uint2)v.c2;
		c3 = (uint2)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint2x4(uint v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x4(bool v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x4(bool2x4 v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x4(int v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x4(int2x4 v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x4(float v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x4(float2x4 v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x4(double v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x4(double2x4 v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator *(uint2x4 lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator *(uint2x4 lhs, uint rhs)
	{
		return new uint2x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator *(uint lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator +(uint2x4 lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator +(uint2x4 lhs, uint rhs)
	{
		return new uint2x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator +(uint lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator -(uint2x4 lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator -(uint2x4 lhs, uint rhs)
	{
		return new uint2x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator -(uint lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator /(uint2x4 lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator /(uint2x4 lhs, uint rhs)
	{
		return new uint2x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator /(uint lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator %(uint2x4 lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator %(uint2x4 lhs, uint rhs)
	{
		return new uint2x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator %(uint lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator ++(uint2x4 val)
	{
		return new uint2x4(++val.c0, ++val.c1, ++val.c2, ++val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator --(uint2x4 val)
	{
		return new uint2x4(--val.c0, --val.c1, --val.c2, --val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(uint2x4 lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(uint2x4 lhs, uint rhs)
	{
		return new bool2x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <(uint lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(uint2x4 lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(uint2x4 lhs, uint rhs)
	{
		return new bool2x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator <=(uint lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(uint2x4 lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(uint2x4 lhs, uint rhs)
	{
		return new bool2x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >(uint lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(uint2x4 lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(uint2x4 lhs, uint rhs)
	{
		return new bool2x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator >=(uint lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator -(uint2x4 val)
	{
		return new uint2x4(-val.c0, -val.c1, -val.c2, -val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator +(uint2x4 val)
	{
		return new uint2x4(+val.c0, +val.c1, +val.c2, +val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator <<(uint2x4 x, int n)
	{
		return new uint2x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator >>(uint2x4 x, int n)
	{
		return new uint2x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(uint2x4 lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(uint2x4 lhs, uint rhs)
	{
		return new bool2x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator ==(uint lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(uint2x4 lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(uint2x4 lhs, uint rhs)
	{
		return new bool2x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 operator !=(uint lhs, uint2x4 rhs)
	{
		return new bool2x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator ~(uint2x4 val)
	{
		return new uint2x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator &(uint2x4 lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator &(uint2x4 lhs, uint rhs)
	{
		return new uint2x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator &(uint lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator |(uint2x4 lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator |(uint2x4 lhs, uint rhs)
	{
		return new uint2x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator |(uint lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator ^(uint2x4 lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator ^(uint2x4 lhs, uint rhs)
	{
		return new uint2x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 operator ^(uint lhs, uint2x4 rhs)
	{
		return new uint2x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(uint2x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((uint2x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"uint2x4({c0.x}, {c1.x}, {c2.x}, {c3.x},  {c0.y}, {c1.y}, {c2.y}, {c3.y})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"uint2x4({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)}, {c3.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)}, {c3.y.ToString(format, formatProvider)})";
	}
}
