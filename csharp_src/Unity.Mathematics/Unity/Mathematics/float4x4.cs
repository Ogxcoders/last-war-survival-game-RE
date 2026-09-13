using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics;

[Serializable]
public struct float4x4 : IEquatable<float4x4>, IFormattable
{
	public float4 c0;

	public float4 c1;

	public float4 c2;

	public float4 c3;

	public static readonly float4x4 identity = new float4x4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);

	public static readonly float4x4 zero;

	public unsafe ref float4 this[int index]
	{
		get
		{
			fixed (float4x4* ptr = &this)
			{
				return ref *(float4*)((byte*)ptr + (nint)index * (nint)sizeof(float4));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(float4 c0, float4 c1, float4 c2, float4 c3)
	{
		this.c0 = c0;
		this.c1 = c1;
		this.c2 = c2;
		this.c3 = c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
	{
		c0 = new float4(m00, m10, m20, m30);
		c1 = new float4(m01, m11, m21, m31);
		c2 = new float4(m02, m12, m22, m32);
		c3 = new float4(m03, m13, m23, m33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(float v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(bool v)
	{
		c0 = math.select(new float4(0f), new float4(1f), v);
		c1 = math.select(new float4(0f), new float4(1f), v);
		c2 = math.select(new float4(0f), new float4(1f), v);
		c3 = math.select(new float4(0f), new float4(1f), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(bool4x4 v)
	{
		c0 = math.select(new float4(0f), new float4(1f), v.c0);
		c1 = math.select(new float4(0f), new float4(1f), v.c1);
		c2 = math.select(new float4(0f), new float4(1f), v.c2);
		c3 = math.select(new float4(0f), new float4(1f), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(int v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(int4x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(uint v)
	{
		c0 = v;
		c1 = v;
		c2 = v;
		c3 = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(uint4x4 v)
	{
		c0 = v.c0;
		c1 = v.c1;
		c2 = v.c2;
		c3 = v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(double v)
	{
		c0 = (float4)v;
		c1 = (float4)v;
		c2 = (float4)v;
		c3 = (float4)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4x4(double4x4 v)
	{
		c0 = (float4)v.c0;
		c1 = (float4)v.c1;
		c2 = (float4)v.c2;
		c3 = (float4)v.c3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x4(float v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x4(bool v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x4(bool4x4 v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x4(int v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x4(int4x4 v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x4(uint v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4x4(uint4x4 v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x4(double v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4x4(double4x4 v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator *(float4x4 lhs, float4x4 rhs)
	{
		return new float4x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator *(float4x4 lhs, float rhs)
	{
		return new float4x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator *(float lhs, float4x4 rhs)
	{
		return new float4x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator +(float4x4 lhs, float4x4 rhs)
	{
		return new float4x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator +(float4x4 lhs, float rhs)
	{
		return new float4x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator +(float lhs, float4x4 rhs)
	{
		return new float4x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator -(float4x4 lhs, float4x4 rhs)
	{
		return new float4x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator -(float4x4 lhs, float rhs)
	{
		return new float4x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator -(float lhs, float4x4 rhs)
	{
		return new float4x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator /(float4x4 lhs, float4x4 rhs)
	{
		return new float4x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator /(float4x4 lhs, float rhs)
	{
		return new float4x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator /(float lhs, float4x4 rhs)
	{
		return new float4x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator %(float4x4 lhs, float4x4 rhs)
	{
		return new float4x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator %(float4x4 lhs, float rhs)
	{
		return new float4x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator %(float lhs, float4x4 rhs)
	{
		return new float4x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator ++(float4x4 val)
	{
		return new float4x4(++val.c0, ++val.c1, ++val.c2, ++val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator --(float4x4 val)
	{
		return new float4x4(--val.c0, --val.c1, --val.c2, --val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <(float4x4 lhs, float4x4 rhs)
	{
		return new bool4x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <(float4x4 lhs, float rhs)
	{
		return new bool4x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <(float lhs, float4x4 rhs)
	{
		return new bool4x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <=(float4x4 lhs, float4x4 rhs)
	{
		return new bool4x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <=(float4x4 lhs, float rhs)
	{
		return new bool4x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator <=(float lhs, float4x4 rhs)
	{
		return new bool4x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >(float4x4 lhs, float4x4 rhs)
	{
		return new bool4x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >(float4x4 lhs, float rhs)
	{
		return new bool4x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >(float lhs, float4x4 rhs)
	{
		return new bool4x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >=(float4x4 lhs, float4x4 rhs)
	{
		return new bool4x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >=(float4x4 lhs, float rhs)
	{
		return new bool4x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator >=(float lhs, float4x4 rhs)
	{
		return new bool4x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator -(float4x4 val)
	{
		return new float4x4(-val.c0, -val.c1, -val.c2, -val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 operator +(float4x4 val)
	{
		return new float4x4(+val.c0, +val.c1, +val.c2, +val.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(float4x4 lhs, float4x4 rhs)
	{
		return new bool4x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(float4x4 lhs, float rhs)
	{
		return new bool4x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator ==(float lhs, float4x4 rhs)
	{
		return new bool4x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(float4x4 lhs, float4x4 rhs)
	{
		return new bool4x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(float4x4 lhs, float rhs)
	{
		return new bool4x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 operator !=(float lhs, float4x4 rhs)
	{
		return new bool4x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(float4x4 rhs)
	{
		if (c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2))
		{
			return c3.Equals(rhs.c3);
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((float4x4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"float4x4({c0.x}f, {c1.x}f, {c2.x}f, {c3.x}f,  {c0.y}f, {c1.y}f, {c2.y}f, {c3.y}f,  {c0.z}f, {c1.z}f, {c2.z}f, {c3.z}f,  {c0.w}f, {c1.w}f, {c2.w}f, {c3.w}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float4x4({c0.x.ToString(format, formatProvider)}f, {c1.x.ToString(format, formatProvider)}f, {c2.x.ToString(format, formatProvider)}f, {c3.x.ToString(format, formatProvider)}f,  {c0.y.ToString(format, formatProvider)}f, {c1.y.ToString(format, formatProvider)}f, {c2.y.ToString(format, formatProvider)}f, {c3.y.ToString(format, formatProvider)}f,  {c0.z.ToString(format, formatProvider)}f, {c1.z.ToString(format, formatProvider)}f, {c2.z.ToString(format, formatProvider)}f, {c3.z.ToString(format, formatProvider)}f,  {c0.w.ToString(format, formatProvider)}f, {c1.w.ToString(format, formatProvider)}f, {c2.w.ToString(format, formatProvider)}f, {c3.w.ToString(format, formatProvider)}f)";
	}

	public static implicit operator float4x4(Matrix4x4 m)
	{
		return new float4x4(m.GetColumn(0), m.GetColumn(1), m.GetColumn(2), m.GetColumn(3));
	}

	public static implicit operator Matrix4x4(float4x4 m)
	{
		return new Matrix4x4(m.c0, m.c1, m.c2, m.c3);
	}

	public float4x4(float3x3 rotation, float3 translation)
	{
		c0 = math.float4(rotation.c0, 0f);
		c1 = math.float4(rotation.c1, 0f);
		c2 = math.float4(rotation.c2, 0f);
		c3 = math.float4(translation, 1f);
	}

	public float4x4(quaternion rotation, float3 translation)
	{
		float3x3 float3x5 = math.float3x3(rotation);
		c0 = math.float4(float3x5.c0, 0f);
		c1 = math.float4(float3x5.c1, 0f);
		c2 = math.float4(float3x5.c2, 0f);
		c3 = math.float4(translation, 1f);
	}

	public float4x4(RigidTransform transform)
	{
		float3x3 float3x5 = math.float3x3(transform.rot);
		c0 = math.float4(float3x5.c0, 0f);
		c1 = math.float4(float3x5.c1, 0f);
		c2 = math.float4(float3x5.c2, 0f);
		c3 = math.float4(transform.pos, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 AxisAngle(float3 axis, float angle)
	{
		math.sincos(angle, out var s, out var c);
		float4 float5 = math.float4(axis, 0f);
		_ = float5.yzxx;
		_ = float5.zxyx;
		float4 float6 = float5 - float5 * c;
		float4 float7 = math.float4(float5.xyz * s, c);
		uint4 uint5 = math.uint4(0u, 0u, 2147483648u, 0u);
		uint4 uint6 = math.uint4(2147483648u, 0u, 0u, 0u);
		uint4 uint7 = math.uint4(0u, 2147483648u, 0u, 0u);
		uint4 uint8 = math.uint4(uint.MaxValue, uint.MaxValue, uint.MaxValue, 0u);
		return math.float4x4(float5.x * float6 + math.asfloat((math.asuint(float7.wzyx) ^ uint5) & uint8), float5.y * float6 + math.asfloat((math.asuint(float7.zwxx) ^ uint6) & uint8), float5.z * float6 + math.asfloat((math.asuint(float7.yxwx) ^ uint7) & uint8), math.float4(0f, 0f, 0f, 1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerXYZ(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float4x4(c.y * c.z, c.z * s.x * s.y - c.x * s.z, c.x * c.z * s.y + s.x * s.z, 0f, c.y * s.z, c.x * c.z + s.x * s.y * s.z, c.x * s.y * s.z - c.z * s.x, 0f, 0f - s.y, c.y * s.x, c.x * c.y, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerXZY(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float4x4(c.y * c.z, s.x * s.y - c.x * c.y * s.z, c.x * s.y + c.y * s.x * s.z, 0f, s.z, c.x * c.z, (0f - c.z) * s.x, 0f, (0f - c.z) * s.y, c.y * s.x + c.x * s.y * s.z, c.x * c.y - s.x * s.y * s.z, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerYXZ(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float4x4(c.y * c.z - s.x * s.y * s.z, (0f - c.x) * s.z, c.z * s.y + c.y * s.x * s.z, 0f, c.z * s.x * s.y + c.y * s.z, c.x * c.z, s.y * s.z - c.y * c.z * s.x, 0f, (0f - c.x) * s.y, s.x, c.x * c.y, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerYZX(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float4x4(c.y * c.z, 0f - s.z, c.z * s.y, 0f, s.x * s.y + c.x * c.y * s.z, c.x * c.z, c.x * s.y * s.z - c.y * s.x, 0f, c.y * s.x * s.z - c.x * s.y, c.z * s.x, c.x * c.y + s.x * s.y * s.z, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerZXY(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float4x4(c.y * c.z + s.x * s.y * s.z, c.z * s.x * s.y - c.y * s.z, c.x * s.y, 0f, c.x * s.z, c.x * c.z, 0f - s.x, 0f, c.y * s.x * s.z - c.z * s.y, c.y * c.z * s.x + s.y * s.z, c.x * c.y, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerZYX(float3 xyz)
	{
		math.sincos(xyz, out var s, out var c);
		return math.float4x4(c.y * c.z, (0f - c.y) * s.z, s.y, 0f, c.z * s.x * s.y + c.x * s.z, c.x * c.z - s.x * s.y * s.z, (0f - c.y) * s.x, 0f, s.x * s.z - c.x * c.z * s.y, c.z * s.x + c.x * s.y * s.z, c.x * c.y, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerXYZ(float x, float y, float z)
	{
		return EulerXYZ(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerXZY(float x, float y, float z)
	{
		return EulerXZY(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerYXZ(float x, float y, float z)
	{
		return EulerYXZ(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerYZX(float x, float y, float z)
	{
		return EulerYZX(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerZXY(float x, float y, float z)
	{
		return EulerZXY(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 EulerZYX(float x, float y, float z)
	{
		return EulerZYX(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.ZXY)
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
	public static float4x4 Euler(float x, float y, float z, math.RotationOrder order = math.RotationOrder.ZXY)
	{
		return Euler(math.float3(x, y, z), order);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 RotateX(float angle)
	{
		math.sincos(angle, out var s, out var c);
		return math.float4x4(1f, 0f, 0f, 0f, 0f, c, 0f - s, 0f, 0f, s, c, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 RotateY(float angle)
	{
		math.sincos(angle, out var s, out var c);
		return math.float4x4(c, 0f, s, 0f, 0f, 1f, 0f, 0f, 0f - s, 0f, c, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 RotateZ(float angle)
	{
		math.sincos(angle, out var s, out var c);
		return math.float4x4(c, 0f - s, 0f, 0f, s, c, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 Scale(float s)
	{
		return math.float4x4(s, 0f, 0f, 0f, 0f, s, 0f, 0f, 0f, 0f, s, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 Scale(float x, float y, float z)
	{
		return math.float4x4(x, 0f, 0f, 0f, 0f, y, 0f, 0f, 0f, 0f, z, 0f, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 Scale(float3 scales)
	{
		return Scale(scales.x, scales.y, scales.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 Translate(float3 vector)
	{
		return math.float4x4(math.float4(1f, 0f, 0f, 0f), math.float4(0f, 1f, 0f, 0f), math.float4(0f, 0f, 1f, 0f), math.float4(vector.x, vector.y, vector.z, 1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 LookAt(float3 eye, float3 target, float3 up)
	{
		float3x3 float3x5 = float3x3.LookRotation(math.normalize(target - eye), up);
		float4x4 result = default(float4x4);
		result.c0 = math.float4(float3x5.c0, 0f);
		result.c1 = math.float4(float3x5.c1, 0f);
		result.c2 = math.float4(float3x5.c2, 0f);
		result.c3 = math.float4(eye, 1f);
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 Ortho(float width, float height, float near, float far)
	{
		float num = 1f / width;
		float num2 = 1f / height;
		float num3 = 1f / (far - near);
		return math.float4x4(2f * num, 0f, 0f, 0f, 0f, 2f * num2, 0f, 0f, 0f, 0f, -2f * num3, (0f - (far + near)) * num3, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 OrthoOffCenter(float left, float right, float bottom, float top, float near, float far)
	{
		float num = 1f / (right - left);
		float num2 = 1f / (top - bottom);
		float num3 = 1f / (far - near);
		return math.float4x4(2f * num, 0f, 0f, (0f - (right + left)) * num, 0f, 2f * num2, 0f, (0f - (top + bottom)) * num2, 0f, 0f, -2f * num3, (0f - (far + near)) * num3, 0f, 0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 PerspectiveFov(float verticalFov, float aspect, float near, float far)
	{
		float num = 1f / math.tan(verticalFov * 0.5f);
		float num2 = 1f / (near - far);
		return math.float4x4(num / aspect, 0f, 0f, 0f, 0f, num, 0f, 0f, 0f, 0f, (far + near) * num2, 2f * near * far * num2, 0f, 0f, -1f, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
	{
		float num = 1f / (near - far);
		float num2 = 1f / (right - left);
		float num3 = 1f / (top - bottom);
		return math.float4x4(2f * near * num2, 0f, (left + right) * num2, 0f, 0f, 2f * near * num3, (bottom + top) * num3, 0f, 0f, 0f, (far + near) * num, 2f * near * far * num, 0f, 0f, -1f, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 TRS(float3 translation, quaternion rotation, float3 scale)
	{
		float3x3 float3x5 = math.float3x3(rotation);
		return math.float4x4(math.float4(float3x5.c0 * scale.x, 0f), math.float4(float3x5.c1 * scale.y, 0f), math.float4(float3x5.c2 * scale.z, 0f), math.float4(translation, 1f));
	}
}
