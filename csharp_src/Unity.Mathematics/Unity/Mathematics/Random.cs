using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
public struct Random
{
	public uint state;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Random(uint seed)
	{
		state = seed;
		NextState();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Random CreateFromIndex(uint index)
	{
		return new Random(WangHash(index + 62));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint WangHash(uint n)
	{
		n = n ^ 0x3D ^ (n >> 16);
		n *= 9;
		n ^= n >> 4;
		n *= 668265261;
		n ^= n >> 15;
		return n;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void InitState(uint seed = 1851936439u)
	{
		state = seed;
		NextState();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool NextBool()
	{
		return (NextState() & 1) == 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool2 NextBool2()
	{
		return (math.uint2(NextState()) & math.uint2(1u, 2u)) == 0u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool3 NextBool3()
	{
		return (math.uint3(NextState()) & math.uint3(1u, 2u, 4u)) == 0u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4 NextBool4()
	{
		return (math.uint4(NextState()) & math.uint4(1u, 2u, 4u, 8u)) == 0u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int NextInt()
	{
		return (int)NextState() ^ int.MinValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2 NextInt2()
	{
		return math.int2((int)NextState(), (int)NextState()) ^ int.MinValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3 NextInt3()
	{
		return math.int3((int)NextState(), (int)NextState(), (int)NextState()) ^ int.MinValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4 NextInt4()
	{
		return math.int4((int)NextState(), (int)NextState(), (int)NextState(), (int)NextState()) ^ int.MinValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int NextInt(int max)
	{
		return (int)(NextState() * max >>> 32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2 NextInt2(int2 max)
	{
		return math.int2((int)(NextState() * max.x >>> 32), (int)(NextState() * max.y >>> 32));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3 NextInt3(int3 max)
	{
		return math.int3((int)(NextState() * max.x >>> 32), (int)(NextState() * max.y >>> 32), (int)(NextState() * max.z >>> 32));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4 NextInt4(int4 max)
	{
		return math.int4((int)(NextState() * max.x >>> 32), (int)(NextState() * max.y >>> 32), (int)(NextState() * max.z >>> 32), (int)(NextState() * max.w >>> 32));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int NextInt(int min, int max)
	{
		uint num = (uint)(max - min);
		return (int)((long)NextState() * (long)num >>> 32) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int2 NextInt2(int2 min, int2 max)
	{
		uint2 uint5 = (uint2)(max - min);
		return math.int2((int)((long)NextState() * (long)uint5.x >>> 32), (int)((long)NextState() * (long)uint5.y >>> 32)) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3 NextInt3(int3 min, int3 max)
	{
		uint3 uint5 = (uint3)(max - min);
		return math.int3((int)((long)NextState() * (long)uint5.x >>> 32), (int)((long)NextState() * (long)uint5.y >>> 32), (int)((long)NextState() * (long)uint5.z >>> 32)) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4 NextInt4(int4 min, int4 max)
	{
		uint4 uint5 = (uint4)(max - min);
		return math.int4((int)((long)NextState() * (long)uint5.x >>> 32), (int)((long)NextState() * (long)uint5.y >>> 32), (int)((long)NextState() * (long)uint5.z >>> 32), (int)((long)NextState() * (long)uint5.w >>> 32)) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint NextUInt()
	{
		return NextState() - 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2 NextUInt2()
	{
		return math.uint2(NextState(), NextState()) - 1u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3 NextUInt3()
	{
		return math.uint3(NextState(), NextState(), NextState()) - 1u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4 NextUInt4()
	{
		return math.uint4(NextState(), NextState(), NextState(), NextState()) - 1u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint NextUInt(uint max)
	{
		return (uint)((ulong)((long)NextState() * (long)max) >> 32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2 NextUInt2(uint2 max)
	{
		return math.uint2((uint)((ulong)((long)NextState() * (long)max.x) >> 32), (uint)((ulong)((long)NextState() * (long)max.y) >> 32));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3 NextUInt3(uint3 max)
	{
		return math.uint3((uint)((ulong)((long)NextState() * (long)max.x) >> 32), (uint)((ulong)((long)NextState() * (long)max.y) >> 32), (uint)((ulong)((long)NextState() * (long)max.z) >> 32));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4 NextUInt4(uint4 max)
	{
		return math.uint4((uint)((ulong)((long)NextState() * (long)max.x) >> 32), (uint)((ulong)((long)NextState() * (long)max.y) >> 32), (uint)((ulong)((long)NextState() * (long)max.z) >> 32), (uint)((ulong)((long)NextState() * (long)max.w) >> 32));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint NextUInt(uint min, uint max)
	{
		uint num = max - min;
		return (uint)(int)((long)NextState() * (long)num >>> 32) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint2 NextUInt2(uint2 min, uint2 max)
	{
		uint2 uint5 = max - min;
		return math.uint2((uint)((ulong)((long)NextState() * (long)uint5.x) >> 32), (uint)((ulong)((long)NextState() * (long)uint5.y) >> 32)) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3 NextUInt3(uint3 min, uint3 max)
	{
		uint3 uint5 = max - min;
		return math.uint3((uint)((ulong)((long)NextState() * (long)uint5.x) >> 32), (uint)((ulong)((long)NextState() * (long)uint5.y) >> 32), (uint)((ulong)((long)NextState() * (long)uint5.z) >> 32)) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint4 NextUInt4(uint4 min, uint4 max)
	{
		uint4 uint5 = max - min;
		return math.uint4((uint)((ulong)((long)NextState() * (long)uint5.x) >> 32), (uint)((ulong)((long)NextState() * (long)uint5.y) >> 32), (uint)((ulong)((long)NextState() * (long)uint5.z) >> 32), (uint)((ulong)((long)NextState() * (long)uint5.w) >> 32)) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float NextFloat()
	{
		return math.asfloat(0x3F800000 | (NextState() >> 9)) - 1f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2 NextFloat2()
	{
		return math.asfloat(1065353216u | (math.uint2(NextState(), NextState()) >> 9)) - 1f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3 NextFloat3()
	{
		return math.asfloat(1065353216u | (math.uint3(NextState(), NextState(), NextState()) >> 9)) - 1f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4 NextFloat4()
	{
		return math.asfloat(1065353216u | (math.uint4(NextState(), NextState(), NextState(), NextState()) >> 9)) - 1f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float NextFloat(float max)
	{
		return NextFloat() * max;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2 NextFloat2(float2 max)
	{
		return NextFloat2() * max;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3 NextFloat3(float3 max)
	{
		return NextFloat3() * max;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4 NextFloat4(float4 max)
	{
		return NextFloat4() * max;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float NextFloat(float min, float max)
	{
		return NextFloat() * (max - min) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2 NextFloat2(float2 min, float2 max)
	{
		return NextFloat2() * (max - min) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3 NextFloat3(float3 min, float3 max)
	{
		return NextFloat3() * (max - min) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4 NextFloat4(float4 min, float4 max)
	{
		return NextFloat4() * (max - min) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double NextDouble()
	{
		ulong num = ((ulong)NextState() << 20) ^ NextState();
		return math.asdouble(0x3FF0000000000000L | num) - 1.0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2 NextDouble2()
	{
		ulong num = ((ulong)NextState() << 20) ^ NextState();
		ulong num2 = ((ulong)NextState() << 20) ^ NextState();
		return math.double2(math.asdouble(0x3FF0000000000000L | num), math.asdouble(0x3FF0000000000000L | num2)) - 1.0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3 NextDouble3()
	{
		ulong num = ((ulong)NextState() << 20) ^ NextState();
		ulong num2 = ((ulong)NextState() << 20) ^ NextState();
		ulong num3 = ((ulong)NextState() << 20) ^ NextState();
		return math.double3(math.asdouble(0x3FF0000000000000L | num), math.asdouble(0x3FF0000000000000L | num2), math.asdouble(0x3FF0000000000000L | num3)) - 1.0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4 NextDouble4()
	{
		ulong num = ((ulong)NextState() << 20) ^ NextState();
		ulong num2 = ((ulong)NextState() << 20) ^ NextState();
		ulong num3 = ((ulong)NextState() << 20) ^ NextState();
		ulong num4 = ((ulong)NextState() << 20) ^ NextState();
		return math.double4(math.asdouble(0x3FF0000000000000L | num), math.asdouble(0x3FF0000000000000L | num2), math.asdouble(0x3FF0000000000000L | num3), math.asdouble(0x3FF0000000000000L | num4)) - 1.0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double NextDouble(double max)
	{
		return NextDouble() * max;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2 NextDouble2(double2 max)
	{
		return NextDouble2() * max;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3 NextDouble3(double3 max)
	{
		return NextDouble3() * max;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4 NextDouble4(double4 max)
	{
		return NextDouble4() * max;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double NextDouble(double min, double max)
	{
		return NextDouble() * (max - min) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2 NextDouble2(double2 min, double2 max)
	{
		return NextDouble2() * (max - min) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3 NextDouble3(double3 min, double3 max)
	{
		return NextDouble3() * (max - min) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double4 NextDouble4(double4 min, double4 max)
	{
		return NextDouble4() * (max - min) + min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float2 NextFloat2Direction()
	{
		math.sincos(NextFloat() * MathF.PI * 2f, out var s, out var c);
		return math.float2(c, s);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double2 NextDouble2Direction()
	{
		math.sincos(NextDouble() * Math.PI * 2.0, out var s, out var c);
		return math.double2(c, s);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3 NextFloat3Direction()
	{
		float2 obj = NextFloat2();
		float num = obj.x * 2f - 1f;
		float num2 = math.sqrt(math.max(1f - num * num, 0f));
		math.sincos(obj.y * MathF.PI * 2f, out var s, out var c);
		return math.float3(c * num2, s * num2, num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double3 NextDouble3Direction()
	{
		double2 obj = NextDouble2();
		double num = obj.x * 2.0 - 1.0;
		double num2 = math.sqrt(math.max(1.0 - num * num, 0.0));
		math.sincos(obj.y * Math.PI * 2.0, out var s, out var c);
		return math.double3(c * num2, s * num2, num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public quaternion NextQuaternionRotation()
	{
		float3 float5 = NextFloat3(math.float3(MathF.PI * 2f, MathF.PI * 2f, 1f));
		float z = float5.z;
		float2 xy = float5.xy;
		float num = math.sqrt(1f - z);
		float num2 = math.sqrt(z);
		math.sincos(xy, out var s, out var c);
		quaternion quaternion2 = math.quaternion(num * s.x, num * c.x, num2 * s.y, num2 * c.y);
		return math.quaternion(math.select(quaternion2.value, -quaternion2.value, quaternion2.value.w < 0f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private uint NextState()
	{
		uint result = state;
		state ^= state << 13;
		state ^= state >> 17;
		state ^= state << 5;
		return result;
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckInitState()
	{
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private static void CheckIndexForHash(uint index)
	{
		if (index == uint.MaxValue)
		{
			throw new ArgumentException("Index must not be uint.MaxValue");
		}
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckState()
	{
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckNextIntMax(int max)
	{
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckNextIntMinMax(int min, int max)
	{
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckNextUIntMinMax(uint min, uint max)
	{
	}
}
