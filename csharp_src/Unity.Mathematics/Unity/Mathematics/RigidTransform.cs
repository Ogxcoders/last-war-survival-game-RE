using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct RigidTransform
{
	public quaternion rot;

	public float3 pos;

	public static readonly RigidTransform identity = new RigidTransform(new quaternion(0f, 0f, 0f, 1f), new float3(0f, 0f, 0f));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public RigidTransform(quaternion rotation, float3 translation)
	{
		rot = rotation;
		pos = translation;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public RigidTransform(float3x3 rotation, float3 translation)
	{
		rot = new quaternion(rotation);
		pos = translation;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public RigidTransform(float4x4 transform)
	{
		rot = new quaternion(transform);
		pos = transform.c3.xyz;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform AxisAngle(float3 axis, float angle)
	{
		return new RigidTransform(quaternion.AxisAngle(axis, angle), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerXYZ(float3 xyz)
	{
		return new RigidTransform(quaternion.EulerXYZ(xyz), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerXZY(float3 xyz)
	{
		return new RigidTransform(quaternion.EulerXZY(xyz), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerYXZ(float3 xyz)
	{
		return new RigidTransform(quaternion.EulerYXZ(xyz), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerYZX(float3 xyz)
	{
		return new RigidTransform(quaternion.EulerYZX(xyz), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerZXY(float3 xyz)
	{
		return new RigidTransform(quaternion.EulerZXY(xyz), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerZYX(float3 xyz)
	{
		return new RigidTransform(quaternion.EulerZYX(xyz), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerXYZ(float x, float y, float z)
	{
		return EulerXYZ(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerXZY(float x, float y, float z)
	{
		return EulerXZY(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerYXZ(float x, float y, float z)
	{
		return EulerYXZ(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerYZX(float x, float y, float z)
	{
		return EulerYZX(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerZXY(float x, float y, float z)
	{
		return EulerZXY(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform EulerZYX(float x, float y, float z)
	{
		return EulerZYX(math.float3(x, y, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.ZXY)
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
	public static RigidTransform Euler(float x, float y, float z, math.RotationOrder order = math.RotationOrder.ZXY)
	{
		return Euler(math.float3(x, y, z), order);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform RotateX(float angle)
	{
		return new RigidTransform(quaternion.RotateX(angle), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform RotateY(float angle)
	{
		return new RigidTransform(quaternion.RotateY(angle), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform RotateZ(float angle)
	{
		return new RigidTransform(quaternion.RotateZ(angle), float3.zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform Translate(float3 vector)
	{
		return new RigidTransform(quaternion.identity, vector);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(RigidTransform x)
	{
		if (rot.Equals(x.rot))
		{
			return pos.Equals(x.pos);
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object x)
	{
		return Equals((RigidTransform)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"RigidTransform(({rot.value.x}f, {rot.value.y}f, {rot.value.z}f, {rot.value.w}f),  ({pos.x}f, {pos.y}f, {pos.z}f))";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float4x4(({rot.value.x.ToString(format, formatProvider)}f, {rot.value.y.ToString(format, formatProvider)}f, {rot.value.z.ToString(format, formatProvider)}f, {rot.value.w.ToString(format, formatProvider)}f),  ({pos.x.ToString(format, formatProvider)}f, {pos.y.ToString(format, formatProvider)}f, {pos.z.ToString(format, formatProvider)}f))";
	}
}
