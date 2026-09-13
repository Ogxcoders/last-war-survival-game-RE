using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct int2 : IEquatable<int2>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public int x;

		public int y;

		public DebuggerProxy(int2 v)
		{
			x = v.x;
			y = v.y;
		}
	}

	public int x;

	public int y;

	public static readonly int2 zero;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 xx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 yx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 yy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(y, y);
		}
	}

	public unsafe int this[int index]
	{
		get
		{
			fixed (int2* ptr = &this)
			{
				return ((int*)ptr)[index];
			}
		}
		set
		{
			fixed (int* ptr = &x)
			{
				ptr[index] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(int2 xy)
	{
		x = xy.x;
		y = xy.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(int v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(bool v)
	{
		x = (v ? 1 : 0);
		y = (v ? 1 : 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(bool2 v)
	{
		x = (v.x ? 1 : 0);
		y = (v.y ? 1 : 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(uint v)
	{
		x = (int)v;
		y = (int)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(uint2 v)
	{
		x = (int)v.x;
		y = (int)v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(float v)
	{
		x = (int)v;
		y = (int)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(float2 v)
	{
		x = (int)v.x;
		y = (int)v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(double v)
	{
		x = (int)v;
		y = (int)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2(double2 v)
	{
		x = (int)v.x;
		y = (int)v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator int2(int v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2(bool v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2(bool2 v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2(uint v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2(uint2 v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2(float v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2(float2 v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2(double v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int2(double2 v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator *(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x * rhs.x, lhs.y * rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator *(int2 lhs, int rhs)
	{
		return new int2(lhs.x * rhs, lhs.y * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator *(int lhs, int2 rhs)
	{
		return new int2(lhs * rhs.x, lhs * rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator +(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator +(int2 lhs, int rhs)
	{
		return new int2(lhs.x + rhs, lhs.y + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator +(int lhs, int2 rhs)
	{
		return new int2(lhs + rhs.x, lhs + rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator -(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x - rhs.x, lhs.y - rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator -(int2 lhs, int rhs)
	{
		return new int2(lhs.x - rhs, lhs.y - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator -(int lhs, int2 rhs)
	{
		return new int2(lhs - rhs.x, lhs - rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator /(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x / rhs.x, lhs.y / rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator /(int2 lhs, int rhs)
	{
		return new int2(lhs.x / rhs, lhs.y / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator /(int lhs, int2 rhs)
	{
		return new int2(lhs / rhs.x, lhs / rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator %(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x % rhs.x, lhs.y % rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator %(int2 lhs, int rhs)
	{
		return new int2(lhs.x % rhs, lhs.y % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator %(int lhs, int2 rhs)
	{
		return new int2(lhs % rhs.x, lhs % rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator ++(int2 val)
	{
		return new int2(++val.x, ++val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator --(int2 val)
	{
		return new int2(--val.x, --val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(int2 lhs, int2 rhs)
	{
		return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(int2 lhs, int rhs)
	{
		return new bool2(lhs.x < rhs, lhs.y < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(int lhs, int2 rhs)
	{
		return new bool2(lhs < rhs.x, lhs < rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(int2 lhs, int2 rhs)
	{
		return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(int2 lhs, int rhs)
	{
		return new bool2(lhs.x <= rhs, lhs.y <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(int lhs, int2 rhs)
	{
		return new bool2(lhs <= rhs.x, lhs <= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(int2 lhs, int2 rhs)
	{
		return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(int2 lhs, int rhs)
	{
		return new bool2(lhs.x > rhs, lhs.y > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(int lhs, int2 rhs)
	{
		return new bool2(lhs > rhs.x, lhs > rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(int2 lhs, int2 rhs)
	{
		return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(int2 lhs, int rhs)
	{
		return new bool2(lhs.x >= rhs, lhs.y >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(int lhs, int2 rhs)
	{
		return new bool2(lhs >= rhs.x, lhs >= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator -(int2 val)
	{
		return new int2(-val.x, -val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator +(int2 val)
	{
		return new int2(val.x, val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator <<(int2 x, int n)
	{
		return new int2(x.x << n, x.y << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator >>(int2 x, int n)
	{
		return new int2(x.x >> n, x.y >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(int2 lhs, int2 rhs)
	{
		return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(int2 lhs, int rhs)
	{
		return new bool2(lhs.x == rhs, lhs.y == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(int lhs, int2 rhs)
	{
		return new bool2(lhs == rhs.x, lhs == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(int2 lhs, int2 rhs)
	{
		return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(int2 lhs, int rhs)
	{
		return new bool2(lhs.x != rhs, lhs.y != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(int lhs, int2 rhs)
	{
		return new bool2(lhs != rhs.x, lhs != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator ~(int2 val)
	{
		return new int2(~val.x, ~val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator &(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x & rhs.x, lhs.y & rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator &(int2 lhs, int rhs)
	{
		return new int2(lhs.x & rhs, lhs.y & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator &(int lhs, int2 rhs)
	{
		return new int2(lhs & rhs.x, lhs & rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator |(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x | rhs.x, lhs.y | rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator |(int2 lhs, int rhs)
	{
		return new int2(lhs.x | rhs, lhs.y | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator |(int lhs, int2 rhs)
	{
		return new int2(lhs | rhs.x, lhs | rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator ^(int2 lhs, int2 rhs)
	{
		return new int2(lhs.x ^ rhs.x, lhs.y ^ rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator ^(int2 lhs, int rhs)
	{
		return new int2(lhs.x ^ rhs, lhs.y ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 operator ^(int lhs, int2 rhs)
	{
		return new int2(lhs ^ rhs.x, lhs ^ rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int2 rhs)
	{
		if (x == rhs.x)
		{
			return y == rhs.y;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((int2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"int2({x}, {y})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"int2({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
	}
}
