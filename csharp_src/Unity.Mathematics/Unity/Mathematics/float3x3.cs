using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct float3x3 : IEquatable<float3x3>, IFormattable
{
	public float3 c0;

	public float3 c1;

	public float3 c2;

	public static readonly float3x3 identity = new float3x3(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);

	public static readonly float3x3 zero;

	public unsafe ref float3 this[int index]
	{
		get
		{
			fixed (float3x3* ptr = &this)
			{
				return ref *(float3*)((byte*)ptr + (nint)index * (nint)sizeof(float3));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(float3 c0, float3 c1, float3 c2)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
	{
		c0 = new float3(m00, m10, m20);
		c1 = new float3(m01, m11, m21);
		c2 = new float3(m02, m12, m22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(float v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(bool v)
	{
		c0 = math.select(new float3(0f), new float3(1f), v);
		c1 = math.select(new float3(0f), new float3(1f), v);
		c2 = math.select(new float3(0f), new float3(1f), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(bool3x3 v)
	{
		c0 = math.select(new float3(0f), new float3(1f), v.c0);
		c1 = math.select(new float3(0f), new float3(1f), v.c1);
		c2 = math.select(new float3(0f), new float3(1f), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(int3x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(uint3x3 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(double v)
	{
		c0 = (float3)v;
		c1 = (float3)v;
		c2 = (float3)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3x3(double3x3 v)
	{
		c0 = (float3)v.c0;
		c1 = (float3)v.c1;
		c2 = (float3)v.c2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x3(float v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3x3(bool v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3x3(bool3x3 v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x3(int v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x3(int3x3 v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x3(uint v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3x3(uint3x3 v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3x3(double v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3x3(double3x3 v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator *(float3x3 lhs, float3x3 rhs)
	{
		return new float3x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator *(float3x3 lhs, float rhs)
	{
		return new float3x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator *(float lhs, float3x3 rhs)
	{
		return new float3x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator +(float3x3 lhs, float3x3 rhs)
	{
		return new float3x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator +(float3x3 lhs, float rhs)
	{
		return new float3x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator +(float lhs, float3x3 rhs)
	{
		return new float3x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator -(float3x3 lhs, float3x3 rhs)
	{
		return new float3x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator -(float3x3 lhs, float rhs)
	{
		return new float3x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator -(float lhs, float3x3 rhs)
	{
		return new float3x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator /(float3x3 lhs, float3x3 rhs)
	{
		return new float3x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator /(float3x3 lhs, float rhs)
	{
		return new float3x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator /(float lhs, float3x3 rhs)
	{
		return new float3x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator %(float3x3 lhs, float3x3 rhs)
	{
		return new float3x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator %(float3x3 lhs, float rhs)
	{
		return new float3x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator %(float lhs, float3x3 rhs)
	{
		return new float3x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator ++(float3x3 val)
	{
		return new float3x3(++val.c0, ++val.c1, ++val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator --(float3x3 val)
	{
		return new float3x3(--val.c0, --val.c1, --val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <(float3x3 lhs, float3x3 rhs)
	{
		return new bool3x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <(float3x3 lhs, float rhs)
	{
		return new bool3x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <(float lhs, float3x3 rhs)
	{
		return new bool3x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <=(float3x3 lhs, float3x3 rhs)
	{
		return new bool3x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <=(float3x3 lhs, float rhs)
	{
		return new bool3x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator <=(float lhs, float3x3 rhs)
	{
		return new bool3x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >(float3x3 lhs, float3x3 rhs)
	{
		return new bool3x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >(float3x3 lhs, float rhs)
	{
		return new bool3x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >(float lhs, float3x3 rhs)
	{
		return new bool3x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >=(float3x3 lhs, float3x3 rhs)
	{
		return new bool3x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >=(float3x3 lhs, float rhs)
	{
		return new bool3x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator >=(float lhs, float3x3 rhs)
	{
		return new bool3x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator -(float3x3 val)
	{
		return new float3x3(-val.c0, -val.c1, -val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 operator +(float3x3 val)
	{
		return new float3x3(+val.c0, +val.c1, +val.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator ==(float3x3 lhs, float3x3 rhs)
	{
		return new bool3x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator ==(float3x3 lhs, float rhs)
	{
		return new bool3x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator ==(float lhs, float3x3 rhs)
	{
		return new bool3x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator !=(float3x3 lhs, float3x3 rhs)
	{
		return new bool3x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator !=(float3x3 lhs, float rhs)
	{
		return new bool3x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 operator !=(float lhs, float3x3 rhs)
	{
		return new bool3x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(float3x3 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1))
		{
			return c2.Equals(rhs.c2);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((float3x3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"float3x3({c0.x}f, {c1.x}f, {c2.x}f,  {c0.y}f, {c1.y}f, {c2.y}f,  {c0.z}f, {c1.z}f, {c2.z}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float3x3({c0.x.ToString(format, formatProvider)}f, {c1.x.ToString(format, formatProvider)}f, {c2.x.ToString(format, formatProvider)}f,  {c0.y.ToString(format, formatProvider)}f, {c1.y.ToString(format, formatProvider)}f, {c2.y.ToString(format, formatProvider)}f,  {c0.z.ToString(format, formatProvider)}f, {c1.z.ToString(format, formatProvider)}f, {c2.z.ToString(format, formatProvider)}f)";
	}

	public float3x3(float4x4 f4x4)
	{
		c0 = f4x4.c0.xyz;
		c1 = f4x4.c1.xyz;
		c2 = f4x4.c2.xyz;
	}

	public float3x3(quaternion q)
	{
		float4 value = q.value;
		float4 float5 = value + value;
		uint3 uint5 = math.uint3(2147483648u, 0u, 2147483648u);
		uint3 uint6 = math.uint3(2147483648u, 2147483648u, 0u);
		uint3 uint7 = math.uint3(0u, 2147483648u, 2147483648u);
		c0 = float5.y * math.asfloat(math.asuint(value.yxw) ^ uint5) - float5.z * math.asfloat(math.asuint(value.zwx) ^ uint7) + math.float3(1f, 0f, 0f);
		c1 = float5.z * math.asfloat(math.asuint(value.wzy) ^ uint6) - float5.x * math.asfloat(math.asuint(value.yxw) ^ uint5) + math.float3(0f, 1f, 0f);
		c2 = float5.x * math.asfloat(math.asuint(value.zwx) ^ uint7) - float5.y * math.asfloat(math.asuint(value.wzy) ^ uint6) + math.float3(0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 AxisAngle(float3 axis, float angle)
	{
		math.sincos(angle, out var s, out var c);
		float3 float5 = axis;
		_ = float5.yzx;
		_ = float5.zxy;
		float3 float6 = float5 - float5 * c;
		float4 float7 = math.float4(float5 * s, c);
		uint3 uint5 = math.uint3(0u, 0u, 2147483648u);
		uint3 uint6 = math.uint3(2147483648u, 0u, 0u);
		uint3 uint7 = math.uint3(0u, 2147483648u, 0u);
		return math.float3x3(float5.x * float6 + math.asfloat(math.asuint(float7.wzy) ^ uint5), float5.y * float6 + math.asfloat(math.asuint(float7.zwx) ^ uint6), float5.z * float6 + math.asfloat(math.asuint(float7.yxw) ^ uint7));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerXYZ(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float3x3(c.y * c.z, c.z * s.x * s.y - c.x * s.z, c.x * c.z * s.y + s.x * s.z, c.y * s.z, c.x * c.z + s.x * s.y * s.z, c.x * s.y * s.z - c.z * s.x, 0f - s.y, c.y * s.x, c.x * c.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerXZY(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float3x3(c.y * c.z, s.x * s.y - c.x * c.y * s.z, c.x * s.y + c.y * s.x * s.z, s.z, c.x * c.z, (0f - c.z) * s.x, (0f - c.z) * s.y, c.y * s.x + c.x * s.y * s.z, c.x * c.y - s.x * s.y * s.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerYXZ(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float3x3(c.y * c.z - s.x * s.y * s.z, (0f - c.x) * s.z, c.z * s.y + c.y * s.x * s.z, c.z * s.x * s.y + c.y * s.z, c.x * c.z, s.y * s.z - c.y * c.z * s.x, (0f - c.x) * s.y, s.x, c.x * c.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerYZX(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float3x3(c.y * c.z, 0f - s.z, c.z * s.y, s.x * s.y + c.x * c.y * s.z, c.x * c.z, c.x * s.y * s.z - c.y * s.x, c.y * s.x * s.z - c.x * s.y, c.z * s.x, c.x * c.y + s.x * s.y * s.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerZXY(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float3x3(c.y * c.z + s.x * s.y * s.z, c.z * s.x * s.y - c.y * s.z, c.x * s.y, c.x * s.z, c.x * c.z, 0f - s.x, c.y * s.x * s.z - c.z * s.y, c.y * c.z * s.x + s.y * s.z, c.x * c.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerZYX(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float3x3(c.y * c.z, (0f - c.y) * s.z, s.y, c.z * s.x * s.y + c.x * s.z, c.x * c.z - s.x * s.y * s.z, (0f - c.y) * s.x, s.x * s.z - c.x * c.z * s.y, c.z * s.x + c.x * s.y * s.z, c.x * c.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerXYZ(float x, float y, float z)
	{
		return EulerXYZ(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerXZY(float x, float y, float z)
	{
		return EulerXZY(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerYXZ(float x, float y, float z)
	{
		return EulerYXZ(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerYZX(float x, float y, float z)
	{
		return EulerYZX(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerZXY(float x, float y, float z)
	{
		return EulerZXY(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 EulerZYX(float x, float y, float z)
	{
		return EulerZYX(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.ZXY)
	{
		return order switch
		{
			math.RotationOrder.XYZ => EulerXYZ(xyz), 
			math.RotationOrder.XZY => EulerXZY(xyz), 
			math.RotationOrder.YXZ => EulerYXZ(xyz), 
			math.RotationOrder.YZX => EulerYZX(xyz), 
			math.RotationOrder.ZXY => EulerZXY(xyz), 
			math.RotationOrder.ZYX => EulerZYX(xyz), 
			_ => identity, 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 Euler(float x, float y, float z, math.RotationOrder order = math.RotationOrder.ZXY)
	{
		return Euler(math.float3(x, y, z), order);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 RotateX(float angle)
	{
		math.sincos(angle, out var s, out var c);
		return math.float3x3(1f, 0f, 0f, 0f, c, 0f - s, 0f, s, c);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 RotateY(float angle)
	{
		math.sincos(angle, out var s, out var c);
		return math.float3x3(c, 0f, s, 0f, 1f, 0f, 0f - s, 0f, c);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 RotateZ(float angle)
	{
		math.sincos(angle, out var s, out var c);
		return math.float3x3(c, 0f - s, 0f, s, c, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 Scale(float s)
	{
		return math.float3x3(s, 0f, 0f, 0f, s, 0f, 0f, 0f, s);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 Scale(float x, float y, float z)
	{
		return math.float3x3(x, 0f, 0f, 0f, y, 0f, 0f, 0f, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 Scale(float3 v)
	{
		return Scale(v.x, v.y, v.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 LookRotation(float3 forward, float3 up)
	{
		float3 y = math.normalize(math.cross(up, forward));
		return math.float3x3(y, math.cross(forward, y), forward);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 LookRotationSafe(float3 forward, float3 up)
	{
		float x = math.dot(forward, forward);
		float num = math.dot(up, up);
		forward *= math.rsqrt(x);
		up *= math.rsqrt(num);
		float3 float5 = math.cross(up, forward);
		float num2 = math.dot(float5, float5);
		float5 *= math.rsqrt(num2);
		float num3 = math.min(math.min(x, num), num2);
		float num4 = math.max(math.max(x, num), num2);
		bool c = num3 > 1E-35f && num4 < 1E+35f && math.isfinite(x) && math.isfinite(num) && math.isfinite(num2);
		return math.float3x3(math.select(math.float3(1f, 0f, 0f), float5, c), math.select(math.float3(0f, 1f, 0f), math.cross(forward, float5), c), math.select(math.float3(0f, 0f, 1f), forward, c));
	}

	public static explicit operator float3x3(float4x4 f4x4)
	{
		return new float3x3(f4x4);
	}
}
