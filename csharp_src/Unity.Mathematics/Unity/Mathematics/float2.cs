using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct float2 : IEquatable<float2>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public float x;

		public float y;

		public DebuggerProxy(float2 v)
		{
			x = v.x;
			y = v.y;
		}
	}

	public float x;

	public float y;

	public static readonly float2 zero;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 xx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 yx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 yy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(y, y);
		}
	}

	public unsafe float this[int index]
	{
		get
		{
			fixed (float2* ptr = &this)
			{
				return ((float*)ptr)[index];
			}
		}
		set
		{
			fixed (float* ptr = &x)
			{
				ptr[index] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(float2 xy)
	{
		x = xy.x;
		y = xy.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(float v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(bool v)
	{
		x = (v ? 1f : 0f);
		y = (v ? 1f : 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(bool2 v)
	{
		x = (v.x ? 1f : 0f);
		y = (v.y ? 1f : 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(int v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(int2 v)
	{
		x = v.x;
		y = v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(uint v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(uint2 v)
	{
		x = v.x;
		y = v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(half v)
	{
		x = v;
		y = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(half2 v)
	{
		x = v.x;
		y = v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(double v)
	{
		x = (float)v;
		y = (float)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2(double2 v)
	{
		x = (float)v.x;
		y = (float)v.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2(float v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float2(bool v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float2(bool2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2(int v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2(int2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2(uint v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2(uint2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2(half v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float2(half2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float2(double v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float2(double2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator *(float2 lhs, float2 rhs)
	{
		return new float2(lhs.x * rhs.x, lhs.y * rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator *(float2 lhs, float rhs)
	{
		return new float2(lhs.x * rhs, lhs.y * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator *(float lhs, float2 rhs)
	{
		return new float2(lhs * rhs.x, lhs * rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator +(float2 lhs, float2 rhs)
	{
		return new float2(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator +(float2 lhs, float rhs)
	{
		return new float2(lhs.x + rhs, lhs.y + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator +(float lhs, float2 rhs)
	{
		return new float2(lhs + rhs.x, lhs + rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator -(float2 lhs, float2 rhs)
	{
		return new float2(lhs.x - rhs.x, lhs.y - rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator -(float2 lhs, float rhs)
	{
		return new float2(lhs.x - rhs, lhs.y - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator -(float lhs, float2 rhs)
	{
		return new float2(lhs - rhs.x, lhs - rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator /(float2 lhs, float2 rhs)
	{
		return new float2(lhs.x / rhs.x, lhs.y / rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator /(float2 lhs, float rhs)
	{
		return new float2(lhs.x / rhs, lhs.y / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator /(float lhs, float2 rhs)
	{
		return new float2(lhs / rhs.x, lhs / rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator %(float2 lhs, float2 rhs)
	{
		return new float2(lhs.x % rhs.x, lhs.y % rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator %(float2 lhs, float rhs)
	{
		return new float2(lhs.x % rhs, lhs.y % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator %(float lhs, float2 rhs)
	{
		return new float2(lhs % rhs.x, lhs % rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator ++(float2 val)
	{
		return new float2(val.x += 1f, val.y += 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator --(float2 val)
	{
		return new float2(val.x -= 1f, val.y -= 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(float2 lhs, float2 rhs)
	{
		return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(float2 lhs, float rhs)
	{
		return new bool2(lhs.x < rhs, lhs.y < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <(float lhs, float2 rhs)
	{
		return new bool2(lhs < rhs.x, lhs < rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(float2 lhs, float2 rhs)
	{
		return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(float2 lhs, float rhs)
	{
		return new bool2(lhs.x <= rhs, lhs.y <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator <=(float lhs, float2 rhs)
	{
		return new bool2(lhs <= rhs.x, lhs <= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(float2 lhs, float2 rhs)
	{
		return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(float2 lhs, float rhs)
	{
		return new bool2(lhs.x > rhs, lhs.y > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >(float lhs, float2 rhs)
	{
		return new bool2(lhs > rhs.x, lhs > rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(float2 lhs, float2 rhs)
	{
		return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(float2 lhs, float rhs)
	{
		return new bool2(lhs.x >= rhs, lhs.y >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator >=(float lhs, float2 rhs)
	{
		return new bool2(lhs >= rhs.x, lhs >= rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator -(float2 val)
	{
		return new float2(0f - val.x, 0f - val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 operator +(float2 val)
	{
		return new float2(val.x, val.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(float2 lhs, float2 rhs)
	{
		return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(float2 lhs, float rhs)
	{
		return new bool2(lhs.x == rhs, lhs.y == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator ==(float lhs, float2 rhs)
	{
		return new bool2(lhs == rhs.x, lhs == rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(float2 lhs, float2 rhs)
	{
		return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(float2 lhs, float rhs)
	{
		return new bool2(lhs.x != rhs, lhs.y != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 operator !=(float lhs, float2 rhs)
	{
		return new bool2(lhs != rhs.x, lhs != rhs.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(float2 rhs)
	{
		if (x == rhs.x)
		{
			return y == rhs.y;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((float2)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"float2({x}f, {y}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float2({x.ToString(format, formatProvider)}f, {y.ToString(format, formatProvider)}f)";
	}

	public static implicit operator Vector2(float2 v)
	{
		return new Vector2(v.x, v.y);
	}

	public static implicit operator float2(Vector2 v)
	{
		return new float2(v.x, v.y);
	}
}
