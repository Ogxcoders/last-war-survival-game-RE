using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct uint4x4 : IEquatable<uint4x4>, IFormattable
{
	public uint4 c0;

	public uint4 c1;

	public uint4 c2;

	public uint4 c3;

	public static readonly uint4x4 identity = new uint4x4(1u, 0u, 0u, 0u, 0u, 1u, 0u, 0u, 0u, 0u, 1u, 0u, 0u, 0u, 0u, 1u);

	public static readonly uint4x4 zero;

	public unsafe ref uint4 this[int index]
	{
		get
		{
			fixed (uint4x4* ptr = &this)
			{
				return ref *(uint4*)((byte*)ptr + (nint)index * (nint)sizeof(uint4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(uint4 c0, uint4 c1, uint4 c2, uint4 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13, uint m20, uint m21, uint m22, uint m23, uint m30, uint m31, uint m32, uint m33)
	{
		c0 = new uint4(m00, m10, m20, m30);
		c1 = new uint4(m01, m11, m21, m31);
		c2 = new uint4(m02, m12, m22, m32);
		c3 = new uint4(m03, m13, m23, m33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(bool v)
	{
		c0 = math.select(new uint4(0u), new uint4(1u), v);
		c1 = math.select(new uint4(0u), new uint4(1u), v);
		c2 = math.select(new uint4(0u), new uint4(1u), v);
		c3 = math.select(new uint4(0u), new uint4(1u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(bool4x4 v)
	{
		c0 = math.select(new uint4(0u), new uint4(1u), v.c0);
		c1 = math.select(new uint4(0u), new uint4(1u), v.c1);
		c2 = math.select(new uint4(0u), new uint4(1u), v.c2);
		c3 = math.select(new uint4(0u), new uint4(1u), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(int v)
	{
		c0 = (uint4)v;
		c1 = (uint4)v;
		c2 = (uint4)v;
		c3 = (uint4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(int4x4 v)
	{
		c0 = (uint4)v.c0;
		c1 = (uint4)v.c1;
		c2 = (uint4)v.c2;
		c3 = (uint4)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(float v)
	{
		c0 = (uint4)v;
		c1 = (uint4)v;
		c2 = (uint4)v;
		c3 = (uint4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(float4x4 v)
	{
		c0 = (uint4)v.c0;
		c1 = (uint4)v.c1;
		c2 = (uint4)v.c2;
		c3 = (uint4)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(double v)
	{
		c0 = (uint4)v;
		c1 = (uint4)v;
		c2 = (uint4)v;
		c3 = (uint4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4x4(double4x4 v)
	{
		c0 = (uint4)v.c0;
		c1 = (uint4)v.c1;
		c2 = (uint4)v.c2;
		c3 = (uint4)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint4x4(uint v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x4(bool v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x4(bool4x4 v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x4(int v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x4(int4x4 v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x4(float v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x4(float4x4 v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x4(double v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint4x4(double4x4 v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator *(uint4x4 lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator *(uint4x4 lhs, uint rhs)
	{
		return new uint4x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator *(uint lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator +(uint4x4 lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator +(uint4x4 lhs, uint rhs)
	{
		return new uint4x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator +(uint lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator -(uint4x4 lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator -(uint4x4 lhs, uint rhs)
	{
		return new uint4x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator -(uint lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator /(uint4x4 lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator /(uint4x4 lhs, uint rhs)
	{
		return new uint4x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator /(uint lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator %(uint4x4 lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator %(uint4x4 lhs, uint rhs)
	{
		return new uint4x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator %(uint lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator ++(uint4x4 val)
	{
		return new uint4x4(++val.c0, ++val.c1, ++val.c2, ++val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator --(uint4x4 val)
	{
		return new uint4x4(--val.c0, --val.c1, --val.c2, --val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <(uint4x4 lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <(uint4x4 lhs, uint rhs)
	{
		return new bool4x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <(uint lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <=(uint4x4 lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <=(uint4x4 lhs, uint rhs)
	{
		return new bool4x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <=(uint lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >(uint4x4 lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >(uint4x4 lhs, uint rhs)
	{
		return new bool4x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >(uint lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >=(uint4x4 lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >=(uint4x4 lhs, uint rhs)
	{
		return new bool4x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >=(uint lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator -(uint4x4 val)
	{
		return new uint4x4(-val.c0, -val.c1, -val.c2, -val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator +(uint4x4 val)
	{
		return new uint4x4(+val.c0, +val.c1, +val.c2, +val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator <<(uint4x4 x, int n)
	{
		return new uint4x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator >>(uint4x4 x, int n)
	{
		return new uint4x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(uint4x4 lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(uint4x4 lhs, uint rhs)
	{
		return new bool4x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(uint lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(uint4x4 lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(uint4x4 lhs, uint rhs)
	{
		return new bool4x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(uint lhs, uint4x4 rhs)
	{
		return new bool4x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator ~(uint4x4 val)
	{
		return new uint4x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator &(uint4x4 lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator &(uint4x4 lhs, uint rhs)
	{
		return new uint4x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator &(uint lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator |(uint4x4 lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator |(uint4x4 lhs, uint rhs)
	{
		return new uint4x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator |(uint lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator ^(uint4x4 lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator ^(uint4x4 lhs, uint rhs)
	{
		return new uint4x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 operator ^(uint lhs, uint4x4 rhs)
	{
		return new uint4x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(uint4x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((uint4x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"uint4x4({c0.x}, {c1.x}, {c2.x}, {c3.x},  {c0.y}, {c1.y}, {c2.y}, {c3.y},  {c0.z}, {c1.z}, {c2.z}, {c3.z},  {c0.w}, {c1.w}, {c2.w}, {c3.w})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"uint4x4({c0.x.ToString(format, formatProvider)}, {c1.x.ToString(format, formatProvider)}, {c2.x.ToString(format, formatProvider)}, {c3.x.ToString(format, formatProvider)},  {c0.y.ToString(format, formatProvider)}, {c1.y.ToString(format, formatProvider)}, {c2.y.ToString(format, formatProvider)}, {c3.y.ToString(format, formatProvider)},  {c0.z.ToString(format, formatProvider)}, {c1.z.ToString(format, formatProvider)}, {c2.z.ToString(format, formatProvider)}, {c3.z.ToString(format, formatProvider)},  {c0.w.ToString(format, formatProvider)}, {c1.w.ToString(format, formatProvider)}, {c2.w.ToString(format, formatProvider)}, {c3.w.ToString(format, formatProvider)})";
	}
}
