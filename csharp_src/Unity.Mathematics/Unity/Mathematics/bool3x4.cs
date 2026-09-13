using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct bool3x4 : IEquatable<bool3x4>
{
	public bool3 c0;

	public bool3 c1;

	public bool3 c2;

	public bool3 c3;

	public unsafe ref bool3 this[int index]
	{
		get
		{
			fixed (bool3x4* ptr = &this)
			{
				return ref *(bool3*)((byte*)ptr + (nint)index * (nint)sizeof(bool3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool3x4(bool3 c0, bool3 c1, bool3 c2, bool3 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool3x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13, bool m20, bool m21, bool m22, bool m23)
	{
		c0 = new bool3(m00, m10, m20);
		c1 = new bool3(m01, m11, m21);
		c2 = new bool3(m02, m12, m22);
		c3 = new bool3(m03, m13, m23);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool3x4(bool v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool3x4(bool v)
	{
		return new bool3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ==(bool3x4 lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ==(bool3x4 lhs, bool rhs)
	{
		return new bool3x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ==(bool lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !=(bool3x4 lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !=(bool3x4 lhs, bool rhs)
	{
		return new bool3x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !=(bool lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator !(bool3x4 val)
	{
		return new bool3x4(!val.c0, !val.c1, !val.c2, !val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator &(bool3x4 lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator &(bool3x4 lhs, bool rhs)
	{
		return new bool3x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator &(bool lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator |(bool3x4 lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator |(bool3x4 lhs, bool rhs)
	{
		return new bool3x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator |(bool lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ^(bool3x4 lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ^(bool3x4 lhs, bool rhs)
	{
		return new bool3x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 operator ^(bool lhs, bool3x4 rhs)
	{
		return new bool3x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(bool3x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((bool3x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"bool3x4({c0.x}, {c1.x}, {c2.x}, {c3.x},  {c0.y}, {c1.y}, {c2.y}, {c3.y},  {c0.z}, {c1.z}, {c2.z}, {c3.z})";
	}
}
