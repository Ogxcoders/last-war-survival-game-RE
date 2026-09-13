using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct uint3x3 : IEquatable<uint3x3>, IFormattable
{
	public uint3 c0;

	public uint3 c1;

	public uint3 c2;

	public static readonly uint3x3 identity = new uint3x3(1u, 0u, 0u, 0u, 1u, 0u, 0u, 0u, 1u);

	public static readonly uint3x3 zero;

	public unsafe ref uint3 this[int index]
	{
		get
		{
			fixed (uint3x3* ptr = &this)
			{
				return ref *(uint3*)((byte*)ptr + (nint)index * (nint)sizeof(uint3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(uint3 c0, uint3 c1, uint3 c2)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12, uint m20, uint m21, uint m22)
	{
		c0 = new uint3(m00, m10, m20);
		c1 = new uint3(m01, m11, m21);
		c2 = new uint3(m02, m12, m22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(bool v)
	{
		c0 = math.select(new uint3(0u), new uint3(1u), v);
		c1 = math.select(new uint3(0u), new uint3(1u), v);
		c2 = math.select(new uint3(0u), new uint3(1u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(bool3x3 v)
	{
		c0 = math.select(new uint3(0u), new uint3(1u), v.c0);
		c1 = math.select(new uint3(0u), new uint3(1u), v.c1);
		c2 = math.select(new uint3(0u), new uint3(1u), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(int v)
	{
		c0 = (uint3)v;
		c1 = (uint3)v;
		c2 = (uint3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(int3x3 v)
	{
		c0 = (uint3)v.c0;
		c1 = (uint3)v.c1;
		c2 = (uint3)v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(float v)
	{
		c0 = (uint3)v;
		c1 = (uint3)v;
		c2 = (uint3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(float3x3 v)
	{
		c0 = (uint3)v.c0;
		c1 = (uint3)v.c1;
		c2 = (uint3)v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(double v)
	{
		c0 = (uint3)v;
		c1 = (uint3)v;
		c2 = (uint3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x3(double3x3 v)
	{
		c0 = (uint3)v.c0;
		c1 = (uint3)v.c1;
		c2 = (uint3)v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint3x3(uint v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x3(bool v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x3(bool3x3 v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x3(int v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x3(int3x3 v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x3(float v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x3(float3x3 v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x3(double v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x3(double3x3 v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator *(uint3x3 lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator *(uint3x3 lhs, uint rhs)
	{
		return new uint3x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator *(uint lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator +(uint3x3 lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator +(uint3x3 lhs, uint rhs)
	{
		return new uint3x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator +(uint lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator -(uint3x3 lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator -(uint3x3 lhs, uint rhs)
	{
		return new uint3x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator -(uint lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator /(uint3x3 lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator /(uint3x3 lhs, uint rhs)
	{
		return new uint3x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator /(uint lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator %(uint3x3 lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator %(uint3x3 lhs, uint rhs)
	{
		return new uint3x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator %(uint lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator ++(uint3x3 val)
	{
		return new uint3x3(++val.c0, ++val.c1, ++val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator --(uint3x3 val)
	{
		return new uint3x3(--val.c0, --val.c1, --val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <(uint3x3 lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <(uint3x3 lhs, uint rhs)
	{
		return new bool3x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <(uint lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <=(uint3x3 lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <=(uint3x3 lhs, uint rhs)
	{
		return new bool3x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <=(uint lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >(uint3x3 lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >(uint3x3 lhs, uint rhs)
	{
		return new bool3x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >(uint lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >=(uint3x3 lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >=(uint3x3 lhs, uint rhs)
	{
		return new bool3x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >=(uint lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator -(uint3x3 val)
	{
		return new uint3x3(-val.c0, -val.c1, -val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator +(uint3x3 val)
	{
		return new uint3x3(+val.c0, +val.c1, +val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator <<(uint3x3 x, int n)
	{
		return new uint3x3(x.c0 << n, x.c1 << n, x.c2 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator >>(uint3x3 x, int n)
	{
		return new uint3x3(x.c0 >> n, x.c1 >> n, x.c2 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator ==(uint3x3 lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator ==(uint3x3 lhs, uint rhs)
	{
		return new bool3x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator ==(uint lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator !=(uint3x3 lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator !=(uint3x3 lhs, uint rhs)
	{
		return new bool3x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator !=(uint lhs, uint3x3 rhs)
	{
		return new bool3x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator ~(uint3x3 val)
	{
		return new uint3x3(~val.c0, ~val.c1, ~val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator &(uint3x3 lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator &(uint3x3 lhs, uint rhs)
	{
		return new uint3x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator &(uint lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator |(uint3x3 lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator |(uint3x3 lhs, uint rhs)
	{
		return new uint3x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator |(uint lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator ^(uint3x3 lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator ^(uint3x3 lhs, uint rhs)
	{
		return new uint3x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 operator ^(uint lhs, uint3x3 rhs)
	{
		return new uint3x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(uint3x3 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1))
		{
			return c2.Equals(rhs.c2);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((uint3x3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"uint3x3({c0.x}, {c1.x}, {c2.x},  {c0.y}, {c1.y}, {c2.y},  {c0.z}, {c1.z}, {c2.z})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"uint3x3({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)}, {c2.z.ToString(format, formatProvider)})";
	}
}
