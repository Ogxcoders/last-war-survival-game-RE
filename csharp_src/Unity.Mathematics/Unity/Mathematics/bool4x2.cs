using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct bool4x2 : IEquatable<bool4x2>
{
	public bool4 c0;

	public bool4 c1;

	public unsafe ref bool4 this[int index]
	{
		get
		{
			fixed (bool4x2* ptr = &this)
			{
				return ref *(bool4*)((byte*)ptr + (nint)index * (nint)sizeof(bool4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4x2(bool4 c0, bool4 c1)
	{
		this.c0 = c0;
		this.c1 = c1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4x2(bool m00, bool m01, bool m10, bool m11, bool m20, bool m21, bool m30, bool m31)
	{
		c0 = new bool4(m00, m10, m20, m30);
		c1 = new bool4(m01, m11, m21, m31);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4x2(bool v)
	{
		c0 = v;
		c1 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool4x2(bool v)
	{
		return new bool4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(bool4x2 lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(bool4x2 lhs, bool rhs)
	{
		return new bool4x2(lhs.c0 == rhs, lhs.c1 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ==(bool lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs == rhs.c0, lhs == rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(bool4x2 lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(bool4x2 lhs, bool rhs)
	{
		return new bool4x2(lhs.c0 != rhs, lhs.c1 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !=(bool lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs != rhs.c0, lhs != rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator !(bool4x2 val)
	{
		return new bool4x2(!val.c0, !val.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator &(bool4x2 lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator &(bool4x2 lhs, bool rhs)
	{
		return new bool4x2(lhs.c0 & rhs, lhs.c1 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator &(bool lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs & rhs.c0, lhs & rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator |(bool4x2 lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator |(bool4x2 lhs, bool rhs)
	{
		return new bool4x2(lhs.c0 | rhs, lhs.c1 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator |(bool lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs | rhs.c0, lhs | rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ^(bool4x2 lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ^(bool4x2 lhs, bool rhs)
	{
		return new bool4x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 operator ^(bool lhs, bool4x2 rhs)
	{
		return new bool4x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(bool4x2 rhs)
	{
		if (c0.Equals(rhs.c0))
		{
			return c1.Equals(rhs.c1);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((bool4x2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"bool4x2({c0.x}, {c1.x},  {c0.y}, {c1.y},  {c0.z}, {c1.z},  {c0.w}, {c1.w})";
	}
}
