using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct half2 : IEquatable<half2>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public half x;

		public half y;

		public DebuggerProxy(half2 v)
		{
			x = v.x;
			y = v.y;
		}
	}

	public half x;

	public half y;

	public static readonly half2 zero;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 xxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(x, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 xxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(x, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 xxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(x, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 xxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(x, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 xyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(x, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 xyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(x, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 xyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(x, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 xyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(x, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 yxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(y, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 yxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(y, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 yxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(y, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 yxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(y, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 yyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(y, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 yyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(y, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 yyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(y, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half4 yyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half4(y, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half3 xxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half3(x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half3 xxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half3(x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half3 xyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half3(x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half3 xyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half3(x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half3 yxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half3(y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half3 yxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half3(y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half3 yyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half3(y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half3 yyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half3(y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half2 xx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half2(x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half2 xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half2(x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half2 yx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half2(y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public half2 yy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new half2(y, y);
		}
	}

	public unsafe half this[int index]
	{
		get
		{
			fixed (half2* ptr = &this)
			{
				return ((half*)ptr)[index];
			}
		}
		set
		{
			fixed (half* ptr = &x)
			{
				ptr[index] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public half2(half x, half y)
	{
		this.x = x;
		this.y = y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public half2(half2 xy)
	{
		x = xy.x;
		y = xy.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public half2(half v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public half2(float v)
	{
		x = (half)v;
		y = (half)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public half2(float2 v)
	{
		x = (half)v.x;
		y = (half)v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public half2(double v)
	{
		x = (half)v;
		y = (half)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public half2(double2 v)
	{
		x = (half)v.x;
		y = (half)v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator half2(half v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator half2(float v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator half2(float2 v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator half2(double v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator half2(double2 v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(half2 lhs, half2 rhs)
	{
		return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(half2 lhs, half rhs)
	{
		return new bool2(lhs.x == rhs, lhs.y == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(half lhs, half2 rhs)
	{
		return new bool2(lhs == rhs.x, lhs == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(half2 lhs, half2 rhs)
	{
		return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(half2 lhs, half rhs)
	{
		return new bool2(lhs.x != rhs, lhs.y != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(half lhs, half2 rhs)
	{
		return new bool2(lhs != rhs.x, lhs != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(half2 rhs)
	{
		if (x == rhs.x)
		{
			return y == rhs.y;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((half2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"half2({x}, {y})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"half2({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
	}
}
