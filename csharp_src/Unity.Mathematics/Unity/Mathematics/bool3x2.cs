using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct bool3x2 : IEquatable<bool3x2>
{
	public bool3 c0;

	public bool3 c1;

	public unsafe ref bool3 this[int index]
	{
		get
		{
			fixed (bool3x2* ptr = &this)
			{
				return ref *(bool3*)((byte*)ptr + (nint)index * (nint)sizeof(bool3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool3x2(bool3 c0, bool3 c1)
	{
		this.c0 = c0;
		this.c1 = c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool3x2(bool m00, bool m01, bool m10, bool m11, bool m20, bool m21)
	{
		c0 = new bool3(m00, m10, m20);
		c1 = new bool3(m01, m11, m21);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool3x2(bool v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool3x2(bool v)
	{
		return new bool3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(bool3x2 lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(bool3x2 lhs, bool rhs)
	{
		return new bool3x2(lhs.c0 == rhs, lhs.c1 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ==(bool lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs == rhs.c0, lhs == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(bool3x2 lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(bool3x2 lhs, bool rhs)
	{
		return new bool3x2(lhs.c0 != rhs, lhs.c1 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !=(bool lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs != rhs.c0, lhs != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator !(bool3x2 val)
	{
		return new bool3x2(!val.c0, !val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator &(bool3x2 lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator &(bool3x2 lhs, bool rhs)
	{
		return new bool3x2(lhs.c0 & rhs, lhs.c1 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator &(bool lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs & rhs.c0, lhs & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator |(bool3x2 lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator |(bool3x2 lhs, bool rhs)
	{
		return new bool3x2(lhs.c0 | rhs, lhs.c1 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator |(bool lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs | rhs.c0, lhs | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ^(bool3x2 lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ^(bool3x2 lhs, bool rhs)
	{
		return new bool3x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 operator ^(bool lhs, bool3x2 rhs)
	{
		return new bool3x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(bool3x2 rhs)
	{
		if (c0.Equals(rhs.c0))
		{
			return c1.Equals(rhs.c1);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((bool3x2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"bool3x2({c0.x}, {c1.x},  {c0.y}, {c1.y},  {c0.z}, {c1.z})";
	}
}
