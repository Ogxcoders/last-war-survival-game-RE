using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics;

[Serializable]
public struct quaternion : IEquatable<quaternion>, IFormattable
{
	public float4 value;

	public static readonly quaternion identity = new quaternion(0f, 0f, 0f, 1f);

	public static implicit operator Quaternion(quaternion q)
	{
		return new Quaternion(q.value.x, q.value.y, q.value.z, q.value.w);
	}

	public static implicit operator quaternion(Quaternion q)
	{
		return new quaternion(q.x, q.y, q.z, q.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public quaternion(float x, float y, float z, float w)
	{
		value.x = x;
		value.y = y;
		value.z = z;
		value.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public quaternion(float4 value)
	{
		this.value = value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator quaternion(float4 v)
	{
		return new quaternion(v);
	}

	public quaternion(float3x3 m)
	{
		float3 c = m.c0;
		float3 c2 = m.c1;
		float3 c3 = m.c2;
		uint num = math.asuint(c.x) & 0x80000000u;
		float x = c2.y + math.asfloat(math.asuint(c3.z) ^ num);
		uint4 uint5 = math.uint4((int)num >> 31);
		uint4 uint6 = math.uint4(math.asint(x) >> 31);
		float x2 = 1f + math.abs(c.x);
		uint4 uint7 = math.uint4(0u, 2147483648u, 2147483648u, 2147483648u) ^ (uint5 & math.uint4(0u, 2147483648u, 0u, 2147483648u)) ^ (uint6 & math.uint4(2147483648u, 2147483648u, 2147483648u, 0u));
		value = math.float4(x2, c.y, c3.x, c2.z) + math.asfloat(math.asuint(math.float4(x, c2.x, c.z, c3.y)) ^ uint7);
		value = math.asfloat((math.asuint(value) & ~uint5) | (math.asuint(value.zwxy) & uint5));
		value = math.asfloat((math.asuint(value.wzyx) & ~uint6) | (math.asuint(value) & uint6));
		value = math.normalize(value);
	}

	public quaternion(float4x4 m)
	{
		float4 c = m.c0;
		float4 c2 = m.c1;
		float4 c3 = m.c2;
		uint num = math.asuint(c.x) & 0x80000000u;
		float x = c2.y + math.asfloat(math.asuint(c3.z) ^ num);
		uint4 uint5 = math.uint4((int)num >> 31);
		uint4 uint6 = math.uint4(math.asint(x) >> 31);
		float x2 = 1f + math.abs(c.x);
		uint4 uint7 = math.uint4(0u, 2147483648u, 2147483648u, 2147483648u) ^ (uint5 & math.uint4(0u, 2147483648u, 0u, 2147483648u)) ^ (uint6 & math.uint4(2147483648u, 2147483648u, 2147483648u, 0u));
		value = math.float4(x2, c.y, c3.x, c2.z) + math.asfloat(math.asuint(math.float4(x, c2.x, c.z, c3.y)) ^ uint7);
		value = math.asfloat((math.asuint(value) & ~uint5) | (math.asuint(value.zwxy) & uint5));
		value = math.asfloat((math.asuint(value.wzyx) & ~uint6) | (math.asuint(value) & uint6));
		value = math.normalize(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion AxisAngle(float3 axis, float angle)
	{
		math.sincos(0.5f * angle, out var s, out var c);
		return math.quaternion(math.float4(axis * s, c));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerXYZ(float3 xyz)
	{
		math.sincos(0.5f * xyz, out var s, out var c);
		return math.quaternion(math.float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * math.float4(c.xyz, s.x) * math.float4(-1f, 1f, -1f, 1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerXZY(float3 xyz)
	{
		math.sincos(0.5f * xyz, out var s, out var c);
		return math.quaternion(math.float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * math.float4(c.xyz, s.x) * math.float4(1f, 1f, -1f, -1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerYXZ(float3 xyz)
	{
		math.sincos(0.5f * xyz, out var s, out var c);
		return math.quaternion(math.float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * math.float4(c.xyz, s.x) * math.float4(-1f, 1f, 1f, -1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerYZX(float3 xyz)
	{
		math.sincos(0.5f * xyz, out var s, out var c);
		return math.quaternion(math.float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * math.float4(c.xyz, s.x) * math.float4(-1f, -1f, 1f, 1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerZXY(float3 xyz)
	{
		math.sincos(0.5f * xyz, out var s, out var c);
		return math.quaternion(math.float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * math.float4(c.xyz, s.x) * math.float4(1f, -1f, -1f, 1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerZYX(float3 xyz)
	{
		math.sincos(0.5f * xyz, out var s, out var c);
		return math.quaternion(math.float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * math.float4(c.xyz, s.x) * math.float4(1f, -1f, 1f, -1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerXYZ(float x, float y, float z)
	{
		return EulerXYZ(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerXZY(float x, float y, float z)
	{
		return EulerXZY(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerYXZ(float x, float y, float z)
	{
		return EulerYXZ(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerYZX(float x, float y, float z)
	{
		return EulerYZX(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerZXY(float x, float y, float z)
	{
		return EulerZXY(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion EulerZYX(float x, float y, float z)
	{
		return EulerZYX(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.ZXY)
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
	public static quaternion Euler(float x, float y, float z, math.RotationOrder order = math.RotationOrder.ZXY)
	{
		return Euler(math.float3(x, y, z), order);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion RotateX(float angle)
	{
		math.sincos(0.5f * angle, out var s, out var c);
		return math.quaternion(s, 0f, 0f, c);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion RotateY(float angle)
	{
		math.sincos(0.5f * angle, out var s, out var c);
		return math.quaternion(0f, s, 0f, c);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion RotateZ(float angle)
	{
		math.sincos(0.5f * angle, out var s, out var c);
		return math.quaternion(0f, 0f, s, c);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion LookRotation(float3 forward, float3 up)
	{
		float3 float5 = math.normalize(math.cross(up, forward));
		return math.quaternion(math.float3x3(float5, math.cross(forward, float5), forward));
	}

	public static quaternion LookRotationSafe(float3 forward, float3 up)
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
		return math.quaternion(math.select(math.float4(0f, 0f, 0f, 1f), math.quaternion(math.float3x3(float5, math.cross(forward, float5), forward)).value, c));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(quaternion x)
	{
		if (value.x == x.value.x && value.y == x.value.y && value.z == x.value.z)
		{
			return value.w == x.value.w;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object x)
	{
		return Equals((quaternion)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"quaternion({value.x}f, {value.y}f, {value.z}f, {value.w}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"quaternion({value.x.ToString(format, formatProvider)}f, {value.y.ToString(format, formatProvider)}f, {value.z.ToString(format, formatProvider)}f, {value.w.ToString(format, formatProvider)}f)";
	}
}
