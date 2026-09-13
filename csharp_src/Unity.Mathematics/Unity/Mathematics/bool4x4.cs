using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct bool4x4 : IEquatable<bool4x4>
{
	public bool4 c0;

	public bool4 c1;

	public bool4 c2;

	public bool4 c3;

	public unsafe ref bool4 this[int index]
	{
		get
		{
			fixed (bool4x4* ptr = &this)
			{
				return ref *(bool4*)((byte*)ptr + (nint)index * (nint)sizeof(bool4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4x4(bool4 c0, bool4 c1, bool4 c2, bool4 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13, bool m20, bool m21, bool m22, bool m23, bool m30, bool m31, bool m32, bool m33)
	{
		c0 = new bool4(m00, m10, m20, m30);
		c1 = new bool4(m01, m11, m21, m31);
		c2 = new bool4(m02, m12, m22, m32);
		c3 = new bool4(m03, m13, m23, m33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4x4(bool v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool4x4(bool v)
	{
		return new bool4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(bool4x4 lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(bool4x4 lhs, bool rhs)
	{
		return new bool4x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(bool lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(bool4x4 lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(bool4x4 lhs, bool rhs)
	{
		return new bool4x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(bool lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !(bool4x4 val)
	{
		return new bool4x4(!val.c0, !val.c1, !val.c2, !val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator &(bool4x4 lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator &(bool4x4 lhs, bool rhs)
	{
		return new bool4x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator &(bool lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator |(bool4x4 lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator |(bool4x4 lhs, bool rhs)
	{
		return new bool4x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator |(bool lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ^(bool4x4 lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ^(bool4x4 lhs, bool rhs)
	{
		return new bool4x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ^(bool lhs, bool4x4 rhs)
	{
		return new bool4x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(bool4x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((bool4x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"bool4x4({c0.x}, {c1.x}, {c2.x}, {c3.x},  {c0.y}, {c1.y}, {c2.y}, {c3.y},  {c0.z}, {c1.z}, {c2.z}, {c3.z},  {c0.w}, {c1.w}, {c2.w}, {c3.w})";
	}
}
