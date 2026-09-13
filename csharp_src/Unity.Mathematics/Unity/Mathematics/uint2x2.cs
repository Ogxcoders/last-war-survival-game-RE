using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct uint2x2 : IEquatable<uint2x2>, IFormattable
{
	public uint2 c0;

	public uint2 c1;

	public static readonly uint2x2 identity = new uint2x2(1u, 0u, 0u, 1u);

	public static readonly uint2x2 zero;

	public unsafe ref uint2 this[int index]
	{
		get
		{
			fixed (uint2x2* ptr = &this)
			{
				return ref *(uint2*)((byte*)ptr + (nint)index * (nint)sizeof(uint2));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(uint2 c0, uint2 c1)
	{
		this.c0 = c0;
		this.c1 = c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(uint m00, uint m01, uint m10, uint m11)
	{
		c0 = new uint2(m00, m10);
		c1 = new uint2(m01, m11);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(uint v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(bool v)
	{
		c0 = math.select(new uint2(0u), new uint2(1u), v);
		c1 = math.select(new uint2(0u), new uint2(1u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(bool2x2 v)
	{
		c0 = math.select(new uint2(0u), new uint2(1u), v.c0);
		c1 = math.select(new uint2(0u), new uint2(1u), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(int v)
	{
		c0 = (uint2)v;
		c1 = (uint2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(int2x2 v)
	{
		c0 = (uint2)v.c0;
		c1 = (uint2)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(float v)
	{
		c0 = (uint2)v;
		c1 = (uint2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(float2x2 v)
	{
		c0 = (uint2)v.c0;
		c1 = (uint2)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(double v)
	{
		c0 = (uint2)v;
		c1 = (uint2)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2x2(double2x2 v)
	{
		c0 = (uint2)v.c0;
		c1 = (uint2)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint2x2(uint v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x2(bool v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x2(bool2x2 v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x2(int v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x2(int2x2 v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x2(float v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x2(float2x2 v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x2(double v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2x2(double2x2 v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator *(uint2x2 lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator *(uint2x2 lhs, uint rhs)
	{
		return new uint2x2(lhs.c0 * rhs, lhs.c1 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator *(uint lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs * rhs.c0, lhs * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator +(uint2x2 lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator +(uint2x2 lhs, uint rhs)
	{
		return new uint2x2(lhs.c0 + rhs, lhs.c1 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator +(uint lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs + rhs.c0, lhs + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator -(uint2x2 lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator -(uint2x2 lhs, uint rhs)
	{
		return new uint2x2(lhs.c0 - rhs, lhs.c1 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator -(uint lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs - rhs.c0, lhs - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator /(uint2x2 lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator /(uint2x2 lhs, uint rhs)
	{
		return new uint2x2(lhs.c0 / rhs, lhs.c1 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator /(uint lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs / rhs.c0, lhs / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator %(uint2x2 lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator %(uint2x2 lhs, uint rhs)
	{
		return new uint2x2(lhs.c0 % rhs, lhs.c1 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator %(uint lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs % rhs.c0, lhs % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator ++(uint2x2 val)
	{
		return new uint2x2(++val.c0, ++val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator --(uint2x2 val)
	{
		return new uint2x2(--val.c0, --val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator <(uint2x2 lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator <(uint2x2 lhs, uint rhs)
	{
		return new bool2x2(lhs.c0 < rhs, lhs.c1 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator <(uint lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs < rhs.c0, lhs < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator <=(uint2x2 lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator <=(uint2x2 lhs, uint rhs)
	{
		return new bool2x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator <=(uint lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs <= rhs.c0, lhs <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator >(uint2x2 lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator >(uint2x2 lhs, uint rhs)
	{
		return new bool2x2(lhs.c0 > rhs, lhs.c1 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator >(uint lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs > rhs.c0, lhs > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator >=(uint2x2 lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator >=(uint2x2 lhs, uint rhs)
	{
		return new bool2x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator >=(uint lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs >= rhs.c0, lhs >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator -(uint2x2 val)
	{
		return new uint2x2(-val.c0, -val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator +(uint2x2 val)
	{
		return new uint2x2(+val.c0, +val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator <<(uint2x2 x, int n)
	{
		return new uint2x2(x.c0 << n, x.c1 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator >>(uint2x2 x, int n)
	{
		return new uint2x2(x.c0 >> n, x.c1 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator ==(uint2x2 lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator ==(uint2x2 lhs, uint rhs)
	{
		return new bool2x2(lhs.c0 == rhs, lhs.c1 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator ==(uint lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs == rhs.c0, lhs == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator !=(uint2x2 lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator !=(uint2x2 lhs, uint rhs)
	{
		return new bool2x2(lhs.c0 != rhs, lhs.c1 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 operator !=(uint lhs, uint2x2 rhs)
	{
		return new bool2x2(lhs != rhs.c0, lhs != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator ~(uint2x2 val)
	{
		return new uint2x2(~val.c0, ~val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator &(uint2x2 lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator &(uint2x2 lhs, uint rhs)
	{
		return new uint2x2(lhs.c0 & rhs, lhs.c1 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator &(uint lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs & rhs.c0, lhs & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator |(uint2x2 lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator |(uint2x2 lhs, uint rhs)
	{
		return new uint2x2(lhs.c0 | rhs, lhs.c1 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator |(uint lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs | rhs.c0, lhs | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator ^(uint2x2 lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator ^(uint2x2 lhs, uint rhs)
	{
		return new uint2x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 operator ^(uint lhs, uint2x2 rhs)
	{
		return new uint2x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(uint2x2 rhs)
	{
		if (c0.Equals(rhs.c0))
		{
			return c1.Equals(rhs.c1);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((uint2x2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"uint2x2({c0.x}, {c1.x},  {c0.y}, {c1.y})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"uint2x2({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)})";
	}
}
