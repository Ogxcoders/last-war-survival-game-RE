using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct uint3x2 : IEquatable<uint3x2>, IFormattable
{
	public uint3 c0;

	public uint3 c1;

	public static readonly uint3x2 zero;

	public unsafe ref uint3 this[int index]
	{
		get
		{
			fixed (uint3x2* ptr = &this)
			{
				return ref *(uint3*)((byte*)ptr + (nint)index * (nint)sizeof(uint3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(uint3 c0, uint3 c1)
	{
		this.c0 = c0;
		this.c1 = c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(uint m00, uint m01, uint m10, uint m11, uint m20, uint m21)
	{
		c0 = new uint3(m00, m10, m20);
		c1 = new uint3(m01, m11, m21);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(uint v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(bool v)
	{
		c0 = math.select(new uint3(0u), new uint3(1u), v);
		c1 = math.select(new uint3(0u), new uint3(1u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(bool3x2 v)
	{
		c0 = math.select(new uint3(0u), new uint3(1u), v.c0);
		c1 = math.select(new uint3(0u), new uint3(1u), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(int v)
	{
		c0 = (uint3)v;
		c1 = (uint3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(int3x2 v)
	{
		c0 = (uint3)v.c0;
		c1 = (uint3)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(float v)
	{
		c0 = (uint3)v;
		c1 = (uint3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(float3x2 v)
	{
		c0 = (uint3)v.c0;
		c1 = (uint3)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(double v)
	{
		c0 = (uint3)v;
		c1 = (uint3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x2(double3x2 v)
	{
		c0 = (uint3)v.c0;
		c1 = (uint3)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint3x2(uint v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x2(bool v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x2(bool3x2 v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x2(int v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x2(int3x2 v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x2(float v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x2(float3x2 v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x2(double v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x2(double3x2 v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator *(uint3x2 lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator *(uint3x2 lhs, uint rhs)
	{
		return new uint3x2(lhs.c0 * rhs, lhs.c1 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator *(uint lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs * rhs.c0, lhs * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator +(uint3x2 lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator +(uint3x2 lhs, uint rhs)
	{
		return new uint3x2(lhs.c0 + rhs, lhs.c1 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator +(uint lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs + rhs.c0, lhs + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator -(uint3x2 lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator -(uint3x2 lhs, uint rhs)
	{
		return new uint3x2(lhs.c0 - rhs, lhs.c1 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator -(uint lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs - rhs.c0, lhs - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator /(uint3x2 lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator /(uint3x2 lhs, uint rhs)
	{
		return new uint3x2(lhs.c0 / rhs, lhs.c1 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator /(uint lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs / rhs.c0, lhs / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator %(uint3x2 lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator %(uint3x2 lhs, uint rhs)
	{
		return new uint3x2(lhs.c0 % rhs, lhs.c1 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator %(uint lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs % rhs.c0, lhs % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator ++(uint3x2 val)
	{
		return new uint3x2(++val.c0, ++val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator --(uint3x2 val)
	{
		return new uint3x2(--val.c0, --val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <(uint3x2 lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <(uint3x2 lhs, uint rhs)
	{
		return new bool3x2(lhs.c0 < rhs, lhs.c1 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <(uint lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs < rhs.c0, lhs < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <=(uint3x2 lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <=(uint3x2 lhs, uint rhs)
	{
		return new bool3x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator <=(uint lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs <= rhs.c0, lhs <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >(uint3x2 lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >(uint3x2 lhs, uint rhs)
	{
		return new bool3x2(lhs.c0 > rhs, lhs.c1 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >(uint lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs > rhs.c0, lhs > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >=(uint3x2 lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >=(uint3x2 lhs, uint rhs)
	{
		return new bool3x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator >=(uint lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs >= rhs.c0, lhs >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator -(uint3x2 val)
	{
		return new uint3x2(-val.c0, -val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator +(uint3x2 val)
	{
		return new uint3x2(+val.c0, +val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator <<(uint3x2 x, int n)
	{
		return new uint3x2(x.c0 << n, x.c1 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator >>(uint3x2 x, int n)
	{
		return new uint3x2(x.c0 >> n, x.c1 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(uint3x2 lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(uint3x2 lhs, uint rhs)
	{
		return new bool3x2(lhs.c0 == rhs, lhs.c1 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(uint lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs == rhs.c0, lhs == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(uint3x2 lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(uint3x2 lhs, uint rhs)
	{
		return new bool3x2(lhs.c0 != rhs, lhs.c1 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(uint lhs, uint3x2 rhs)
	{
		return new bool3x2(lhs != rhs.c0, lhs != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator ~(uint3x2 val)
	{
		return new uint3x2(~val.c0, ~val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator &(uint3x2 lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator &(uint3x2 lhs, uint rhs)
	{
		return new uint3x2(lhs.c0 & rhs, lhs.c1 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator &(uint lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs & rhs.c0, lhs & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator |(uint3x2 lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator |(uint3x2 lhs, uint rhs)
	{
		return new uint3x2(lhs.c0 | rhs, lhs.c1 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator |(uint lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs | rhs.c0, lhs | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator ^(uint3x2 lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator ^(uint3x2 lhs, uint rhs)
	{
		return new uint3x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 operator ^(uint lhs, uint3x2 rhs)
	{
		return new uint3x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(uint3x2 rhs)
	{
		if (c0.Equals(rhs.c0))
		{
			return c1.Equals(rhs.c1);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((uint3x2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"uint3x2({c0.x}, {c1.x},  {c0.y}, {c1.y},  {c0.z}, {c1.z})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"uint3x2({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)})";
	}
}
