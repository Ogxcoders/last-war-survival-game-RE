using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct double2 : IEquatable<double2>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public double x;

		public double y;

		public DebuggerProxy(double2 v)
		{
			x = v.x;
			y = v.y;
		}
	}

	public double x;

	public double y;

	public static readonly double2 zero;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 xxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(x, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 xxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(x, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 xxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(x, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 xxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(x, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 xyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(x, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 xyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(x, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 xyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(x, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 xyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(x, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 yxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(y, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 yxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(y, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 yxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(y, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 yxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(y, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 yyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(y, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 yyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(y, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 yyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(y, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double4 yyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double4(y, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double3 xxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double3(x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double3 xxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double3(x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double3 xyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double3(x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double3 xyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double3(x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double3 yxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double3(y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double3 yxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double3(y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double3 yyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double3(y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double3 yyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double3(y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double2 xx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double2(x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double2 xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double2(x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double2 yx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double2(y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public double2 yy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new double2(y, y);
		}
	}

	public unsafe double this[int index]
	{
		get
		{
			fixed (double2* ptr = &this)
			{
				return ((double*)ptr)[index];
			}
		}
		set
		{
			fixed (double* ptr = &x)
			{
				ptr[index] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(double x, double y)
	{
		this.x = x;
		this.y = y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(double2 xy)
	{
		x = xy.x;
		y = xy.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(double v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(bool v)
	{
		x = (v ? 1.0 : 0.0);
		y = (v ? 1.0 : 0.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(bool2 v)
	{
		x = (v.x ? 1.0 : 0.0);
		y = (v.y ? 1.0 : 0.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(int v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(int2 v)
	{
		x = v.x;
		y = v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(uint v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(uint2 v)
	{
		x = v.x;
		y = v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(half v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(half2 v)
	{
		x = v.x;
		y = v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(float v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2(float2 v)
	{
		x = v.x;
		y = v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2(double v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double2(bool v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double2(bool2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2(int v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2(int2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2(uint v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2(uint2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2(half v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2(half2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2(float v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator double2(float2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator *(double2 lhs, double2 rhs)
	{
		return new double2(lhs.x * rhs.x, lhs.y * rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator *(double2 lhs, double rhs)
	{
		return new double2(lhs.x * rhs, lhs.y * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator *(double lhs, double2 rhs)
	{
		return new double2(lhs * rhs.x, lhs * rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator +(double2 lhs, double2 rhs)
	{
		return new double2(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator +(double2 lhs, double rhs)
	{
		return new double2(lhs.x + rhs, lhs.y + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator +(double lhs, double2 rhs)
	{
		return new double2(lhs + rhs.x, lhs + rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator -(double2 lhs, double2 rhs)
	{
		return new double2(lhs.x - rhs.x, lhs.y - rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator -(double2 lhs, double rhs)
	{
		return new double2(lhs.x - rhs, lhs.y - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator -(double lhs, double2 rhs)
	{
		return new double2(lhs - rhs.x, lhs - rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator /(double2 lhs, double2 rhs)
	{
		return new double2(lhs.x / rhs.x, lhs.y / rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator /(double2 lhs, double rhs)
	{
		return new double2(lhs.x / rhs, lhs.y / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator /(double lhs, double2 rhs)
	{
		return new double2(lhs / rhs.x, lhs / rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator %(double2 lhs, double2 rhs)
	{
		return new double2(lhs.x % rhs.x, lhs.y % rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator %(double2 lhs, double rhs)
	{
		return new double2(lhs.x % rhs, lhs.y % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator %(double lhs, double2 rhs)
	{
		return new double2(lhs % rhs.x, lhs % rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator ++(double2 val)
	{
		return new double2(val.x += 1.0, val.y += 1.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator --(double2 val)
	{
		return new double2(val.x -= 1.0, val.y -= 1.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(double2 lhs, double2 rhs)
	{
		return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(double2 lhs, double rhs)
	{
		return new bool2(lhs.x < rhs, lhs.y < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(double lhs, double2 rhs)
	{
		return new bool2(lhs < rhs.x, lhs < rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(double2 lhs, double2 rhs)
	{
		return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(double2 lhs, double rhs)
	{
		return new bool2(lhs.x <= rhs, lhs.y <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(double lhs, double2 rhs)
	{
		return new bool2(lhs <= rhs.x, lhs <= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(double2 lhs, double2 rhs)
	{
		return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(double2 lhs, double rhs)
	{
		return new bool2(lhs.x > rhs, lhs.y > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(double lhs, double2 rhs)
	{
		return new bool2(lhs > rhs.x, lhs > rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(double2 lhs, double2 rhs)
	{
		return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(double2 lhs, double rhs)
	{
		return new bool2(lhs.x >= rhs, lhs.y >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(double lhs, double2 rhs)
	{
		return new bool2(lhs >= rhs.x, lhs >= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator -(double2 val)
	{
		return new double2(0.0 - val.x, 0.0 - val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 operator +(double2 val)
	{
		return new double2(val.x, val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(double2 lhs, double2 rhs)
	{
		return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(double2 lhs, double rhs)
	{
		return new bool2(lhs.x == rhs, lhs.y == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(double lhs, double2 rhs)
	{
		return new bool2(lhs == rhs.x, lhs == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(double2 lhs, double2 rhs)
	{
		return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(double2 lhs, double rhs)
	{
		return new bool2(lhs.x != rhs, lhs.y != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(double lhs, double2 rhs)
	{
		return new bool2(lhs != rhs.x, lhs != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(double2 rhs)
	{
		if (x == rhs.x)
		{
			return y == rhs.y;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((double2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"double2({x}, {y})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"double2({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
	}
}
