using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct uint2 : IEquatable<uint2>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public uint x;

		public uint y;

		public DebuggerProxy(uint2 v)
		{
			x = v.x;
			y = v.y;
		}
	}

	public uint x;

	public uint y;

	public static readonly uint2 zero;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 xx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 yx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 yy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(y, y);
		}
	}

	public unsafe uint this[int index]
	{
		get
		{
			fixed (uint2* ptr = &this)
			{
				return ((uint*)ptr)[index];
			}
		}
		set
		{
			fixed (uint* ptr = &x)
			{
				ptr[index] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(uint x, uint y)
	{
		this.x = x;
		this.y = y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(uint2 xy)
	{
		x = xy.x;
		y = xy.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(uint v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(bool v)
	{
		x = (v ? 1u : 0u);
		y = (v ? 1u : 0u);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(bool2 v)
	{
		x = (v.x ? 1u : 0u);
		y = (v.y ? 1u : 0u);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(int v)
	{
		x = (uint)v;
		y = (uint)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(int2 v)
	{
		x = (uint)v.x;
		y = (uint)v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(float v)
	{
		x = (uint)v;
		y = (uint)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(float2 v)
	{
		x = (uint)v.x;
		y = (uint)v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(double v)
	{
		x = (uint)v;
		y = (uint)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2(double2 v)
	{
		x = (uint)v.x;
		y = (uint)v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint2(uint v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2(bool v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2(bool2 v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2(int v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2(int2 v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2(float v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2(float2 v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2(double v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint2(double2 v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator *(uint2 lhs, uint2 rhs)
	{
		return new uint2(lhs.x * rhs.x, lhs.y * rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator *(uint2 lhs, uint rhs)
	{
		return new uint2(lhs.x * rhs, lhs.y * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator *(uint lhs, uint2 rhs)
	{
		return new uint2(lhs * rhs.x, lhs * rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator +(uint2 lhs, uint2 rhs)
	{
		return new uint2(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator +(uint2 lhs, uint rhs)
	{
		return new uint2(lhs.x + rhs, lhs.y + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator +(uint lhs, uint2 rhs)
	{
		return new uint2(lhs + rhs.x, lhs + rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator -(uint2 lhs, uint2 rhs)
	{
		return new uint2(lhs.x - rhs.x, lhs.y - rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator -(uint2 lhs, uint rhs)
	{
		return new uint2(lhs.x - rhs, lhs.y - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator -(uint lhs, uint2 rhs)
	{
		return new uint2(lhs - rhs.x, lhs - rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator /(uint2 lhs, uint2 rhs)
	{
		return new uint2(lhs.x / rhs.x, lhs.y / rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator /(uint2 lhs, uint rhs)
	{
		return new uint2(lhs.x / rhs, lhs.y / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator /(uint lhs, uint2 rhs)
	{
		return new uint2(lhs / rhs.x, lhs / rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator %(uint2 lhs, uint2 rhs)
	{
		return new uint2(lhs.x % rhs.x, lhs.y % rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator %(uint2 lhs, uint rhs)
	{
		return new uint2(lhs.x % rhs, lhs.y % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator %(uint lhs, uint2 rhs)
	{
		return new uint2(lhs % rhs.x, lhs % rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator ++(uint2 val)
	{
		return new uint2(++val.x, ++val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator --(uint2 val)
	{
		return new uint2(--val.x, --val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(uint2 lhs, uint2 rhs)
	{
		return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(uint2 lhs, uint rhs)
	{
		return new bool2(lhs.x < rhs, lhs.y < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(uint lhs, uint2 rhs)
	{
		return new bool2(lhs < rhs.x, lhs < rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(uint2 lhs, uint2 rhs)
	{
		return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(uint2 lhs, uint rhs)
	{
		return new bool2(lhs.x <= rhs, lhs.y <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(uint lhs, uint2 rhs)
	{
		return new bool2(lhs <= rhs.x, lhs <= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(uint2 lhs, uint2 rhs)
	{
		return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(uint2 lhs, uint rhs)
	{
		return new bool2(lhs.x > rhs, lhs.y > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(uint lhs, uint2 rhs)
	{
		return new bool2(lhs > rhs.x, lhs > rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(uint2 lhs, uint2 rhs)
	{
		return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(uint2 lhs, uint rhs)
	{
		return new bool2(lhs.x >= rhs, lhs.y >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(uint lhs, uint2 rhs)
	{
		return new bool2(lhs >= rhs.x, lhs >= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator -(uint2 val)
	{
		return new uint2((uint)(0uL - (ulong)val.x), (uint)(0uL - (ulong)val.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator +(uint2 val)
	{
		return new uint2(val.x, val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator <<(uint2 x, int n)
	{
		return new uint2(x.x << n, x.y << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator >>(uint2 x, int n)
	{
		return new uint2(x.x >> n, x.y >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(uint2 lhs, uint2 rhs)
	{
		return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(uint2 lhs, uint rhs)
	{
		return new bool2(lhs.x == rhs, lhs.y == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(uint lhs, uint2 rhs)
	{
		return new bool2(lhs == rhs.x, lhs == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(uint2 lhs, uint2 rhs)
	{
		return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(uint2 lhs, uint rhs)
	{
		return new bool2(lhs.x != rhs, lhs.y != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(uint lhs, uint2 rhs)
	{
		return new bool2(lhs != rhs.x, lhs != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator ~(uint2 val)
	{
		return new uint2(~val.x, ~val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator &(uint2 lhs, uint2 rhs)
	{
		return new uint2(lhs.x & rhs.x, lhs.y & rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator &(uint2 lhs, uint rhs)
	{
		return new uint2(lhs.x & rhs, lhs.y & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator &(uint lhs, uint2 rhs)
	{
		return new uint2(lhs & rhs.x, lhs & rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator |(uint2 lhs, uint2 rhs)
	{
		return new uint2(lhs.x | rhs.x, lhs.y | rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator |(uint2 lhs, uint rhs)
	{
		return new uint2(lhs.x | rhs, lhs.y | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator |(uint lhs, uint2 rhs)
	{
		return new uint2(lhs | rhs.x, lhs | rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator ^(uint2 lhs, uint2 rhs)
	{
		return new uint2(lhs.x ^ rhs.x, lhs.y ^ rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator ^(uint2 lhs, uint rhs)
	{
		return new uint2(lhs.x ^ rhs, lhs.y ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 operator ^(uint lhs, uint2 rhs)
	{
		return new uint2(lhs ^ rhs.x, lhs ^ rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(uint2 rhs)
	{
		if (x == rhs.x)
		{
			return y == rhs.y;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((uint2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"uint2({x}, {y})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"uint2({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
	}
}
