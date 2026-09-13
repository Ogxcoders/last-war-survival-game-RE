using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct uint4x2 : IEquatable<uint4x2>, IFormattable
{
	public uint4 c0;

	public uint4 c1;

	public static readonly uint4x2 zero;

	public unsafe ref uint4 this[int index]
	{
		get
		{
			fixed (uint4x2* ptr = &this)
			{
				return ref *(uint4*)((byte*)ptr + (nint)index * (nint)sizeof(uint4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(uint4 c0, uint4 c1)
	{
		this.c0 = c0;
		this.c1 = c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(uint m00, uint m01, uint m10, uint m11, uint m20, uint m21, uint m30, uint m31)
	{
		c0 = new uint4(m00, m10, m20, m30);
		c1 = new uint4(m01, m11, m21, m31);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(uint v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(bool v)
	{
		c0 = math.select(new uint4(0u), new uint4(1u), v);
		c1 = math.select(new uint4(0u), new uint4(1u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(bool4x2 v)
	{
		c0 = math.select(new uint4(0u), new uint4(1u), v.c0);
		c1 = math.select(new uint4(0u), new uint4(1u), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(int v)
	{
		c0 = (uint4)v;
		c1 = (uint4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(int4x2 v)
	{
		c0 = (uint4)v.c0;
		c1 = (uint4)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(float v)
	{
		c0 = (uint4)v;
		c1 = (uint4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(float4x2 v)
	{
		c0 = (uint4)v.c0;
		c1 = (uint4)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(double v)
	{
		c0 = (uint4)v;
		c1 = (uint4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x2(double4x2 v)
	{
		c0 = (uint4)v.c0;
		c1 = (uint4)v.c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint4x2(uint v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x2(bool v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x2(bool4x2 v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x2(int v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x2(int4x2 v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x2(float v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x2(float4x2 v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x2(double v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x2(double4x2 v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator *(uint4x2 lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator *(uint4x2 lhs, uint rhs)
	{
		return new uint4x2(lhs.c0 * rhs, lhs.c1 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator *(uint lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs * rhs.c0, lhs * rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator +(uint4x2 lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator +(uint4x2 lhs, uint rhs)
	{
		return new uint4x2(lhs.c0 + rhs, lhs.c1 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator +(uint lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs + rhs.c0, lhs + rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator -(uint4x2 lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator -(uint4x2 lhs, uint rhs)
	{
		return new uint4x2(lhs.c0 - rhs, lhs.c1 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator -(uint lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs - rhs.c0, lhs - rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator /(uint4x2 lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator /(uint4x2 lhs, uint rhs)
	{
		return new uint4x2(lhs.c0 / rhs, lhs.c1 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator /(uint lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs / rhs.c0, lhs / rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator %(uint4x2 lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator %(uint4x2 lhs, uint rhs)
	{
		return new uint4x2(lhs.c0 % rhs, lhs.c1 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator %(uint lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs % rhs.c0, lhs % rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator ++(uint4x2 val)
	{
		return new uint4x2(++val.c0, ++val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator --(uint4x2 val)
	{
		return new uint4x2(--val.c0, --val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <(uint4x2 lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <(uint4x2 lhs, uint rhs)
	{
		return new bool4x2(lhs.c0 < rhs, lhs.c1 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <(uint lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs < rhs.c0, lhs < rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <=(uint4x2 lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <=(uint4x2 lhs, uint rhs)
	{
		return new bool4x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator <=(uint lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs <= rhs.c0, lhs <= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >(uint4x2 lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >(uint4x2 lhs, uint rhs)
	{
		return new bool4x2(lhs.c0 > rhs, lhs.c1 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >(uint lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs > rhs.c0, lhs > rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >=(uint4x2 lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >=(uint4x2 lhs, uint rhs)
	{
		return new bool4x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator >=(uint lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs >= rhs.c0, lhs >= rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator -(uint4x2 val)
	{
		return new uint4x2(-val.c0, -val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator +(uint4x2 val)
	{
		return new uint4x2(+val.c0, +val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator <<(uint4x2 x, int n)
	{
		return new uint4x2(x.c0 << n, x.c1 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator >>(uint4x2 x, int n)
	{
		return new uint4x2(x.c0 >> n, x.c1 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(uint4x2 lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(uint4x2 lhs, uint rhs)
	{
		return new bool4x2(lhs.c0 == rhs, lhs.c1 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(uint lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs == rhs.c0, lhs == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(uint4x2 lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(uint4x2 lhs, uint rhs)
	{
		return new bool4x2(lhs.c0 != rhs, lhs.c1 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(uint lhs, uint4x2 rhs)
	{
		return new bool4x2(lhs != rhs.c0, lhs != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator ~(uint4x2 val)
	{
		return new uint4x2(~val.c0, ~val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator &(uint4x2 lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator &(uint4x2 lhs, uint rhs)
	{
		return new uint4x2(lhs.c0 & rhs, lhs.c1 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator &(uint lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs & rhs.c0, lhs & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator |(uint4x2 lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator |(uint4x2 lhs, uint rhs)
	{
		return new uint4x2(lhs.c0 | rhs, lhs.c1 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator |(uint lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs | rhs.c0, lhs | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator ^(uint4x2 lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator ^(uint4x2 lhs, uint rhs)
	{
		return new uint4x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 operator ^(uint lhs, uint4x2 rhs)
	{
		return new uint4x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(uint4x2 rhs)
	{
		if (c0.Equals(rhs.c0))
		{
			return c1.Equals(rhs.c1);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((uint4x2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"uint4x2({c0.x}, {c1.x},  {c0.y}, {c1.y},  {c0.z}, {c1.z},  {c0.w}, {c1.w})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"uint4x2({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)},  {c0.w.ToString(format, formatProvider)}, {c1.w.ToString(format, formatProvider)})";
	}
}
