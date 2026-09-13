using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct uint3x4 : IEquatable<uint3x4>, IFormattable
{
	public uint3 c0;

	public uint3 c1;

	public uint3 c2;

	public uint3 c3;

	public static readonly uint3x4 zero;

	public unsafe ref uint3 this[int index]
	{
		get
		{
			fixed (uint3x4* ptr = &this)
			{
				return ref *(uint3*)((byte*)ptr + (nint)index * (nint)sizeof(uint3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(uint3 c0, uint3 c1, uint3 c2, uint3 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13, uint m20, uint m21, uint m22, uint m23)
	{
		c0 = new uint3(m00, m10, m20);
		c1 = new uint3(m01, m11, m21);
		c2 = new uint3(m02, m12, m22);
		c3 = new uint3(m03, m13, m23);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(bool v)
	{
		c0 = math.select(new uint3(0u), new uint3(1u), v);
		c1 = math.select(new uint3(0u), new uint3(1u), v);
		c2 = math.select(new uint3(0u), new uint3(1u), v);
		c3 = math.select(new uint3(0u), new uint3(1u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(bool3x4 v)
	{
		c0 = math.select(new uint3(0u), new uint3(1u), v.c0);
		c1 = math.select(new uint3(0u), new uint3(1u), v.c1);
		c2 = math.select(new uint3(0u), new uint3(1u), v.c2);
		c3 = math.select(new uint3(0u), new uint3(1u), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(int v)
	{
		c0 = (uint3)v;
		c1 = (uint3)v;
		c2 = (uint3)v;
		c3 = (uint3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(int3x4 v)
	{
		c0 = (uint3)v.c0;
		c1 = (uint3)v.c1;
		c2 = (uint3)v.c2;
		c3 = (uint3)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(float v)
	{
		c0 = (uint3)v;
		c1 = (uint3)v;
		c2 = (uint3)v;
		c3 = (uint3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(float3x4 v)
	{
		c0 = (uint3)v.c0;
		c1 = (uint3)v.c1;
		c2 = (uint3)v.c2;
		c3 = (uint3)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(double v)
	{
		c0 = (uint3)v;
		c1 = (uint3)v;
		c2 = (uint3)v;
		c3 = (uint3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3x4(double3x4 v)
	{
		c0 = (uint3)v.c0;
		c1 = (uint3)v.c1;
		c2 = (uint3)v.c2;
		c3 = (uint3)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint3x4(uint v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x4(bool v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x4(bool3x4 v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x4(int v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x4(int3x4 v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x4(float v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x4(float3x4 v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x4(double v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3x4(double3x4 v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator *(uint3x4 lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator *(uint3x4 lhs, uint rhs)
	{
		return new uint3x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator *(uint lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator +(uint3x4 lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator +(uint3x4 lhs, uint rhs)
	{
		return new uint3x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator +(uint lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator -(uint3x4 lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator -(uint3x4 lhs, uint rhs)
	{
		return new uint3x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator -(uint lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator /(uint3x4 lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator /(uint3x4 lhs, uint rhs)
	{
		return new uint3x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator /(uint lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator %(uint3x4 lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator %(uint3x4 lhs, uint rhs)
	{
		return new uint3x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator %(uint lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator ++(uint3x4 val)
	{
		return new uint3x4(++val.c0, ++val.c1, ++val.c2, ++val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator --(uint3x4 val)
	{
		return new uint3x4(--val.c0, --val.c1, --val.c2, --val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <(uint3x4 lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <(uint3x4 lhs, uint rhs)
	{
		return new bool3x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <(uint lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <=(uint3x4 lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <=(uint3x4 lhs, uint rhs)
	{
		return new bool3x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator <=(uint lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >(uint3x4 lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >(uint3x4 lhs, uint rhs)
	{
		return new bool3x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >(uint lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >=(uint3x4 lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >=(uint3x4 lhs, uint rhs)
	{
		return new bool3x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator >=(uint lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator -(uint3x4 val)
	{
		return new uint3x4(-val.c0, -val.c1, -val.c2, -val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator +(uint3x4 val)
	{
		return new uint3x4(+val.c0, +val.c1, +val.c2, +val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator <<(uint3x4 x, int n)
	{
		return new uint3x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator >>(uint3x4 x, int n)
	{
		return new uint3x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ==(uint3x4 lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ==(uint3x4 lhs, uint rhs)
	{
		return new bool3x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ==(uint lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !=(uint3x4 lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !=(uint3x4 lhs, uint rhs)
	{
		return new bool3x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !=(uint lhs, uint3x4 rhs)
	{
		return new bool3x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator ~(uint3x4 val)
	{
		return new uint3x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator &(uint3x4 lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator &(uint3x4 lhs, uint rhs)
	{
		return new uint3x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator &(uint lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator |(uint3x4 lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator |(uint3x4 lhs, uint rhs)
	{
		return new uint3x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator |(uint lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator ^(uint3x4 lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator ^(uint3x4 lhs, uint rhs)
	{
		return new uint3x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 operator ^(uint lhs, uint3x4 rhs)
	{
		return new uint3x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(uint3x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((uint3x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"uint3x4({c0.x}, {c1.x}, {c2.x}, {c3.x},  {c0.y}, {c1.y}, {c2.y}, {c3.y},  {c0.z}, {c1.z}, {c2.z}, {c3.z})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"uint3x4({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)}, {c3.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)}, {c3.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)}, {c2.z.ToString(format, formatProvider)}, {c3.z.ToString(format, formatProvider)})";
	}
}
