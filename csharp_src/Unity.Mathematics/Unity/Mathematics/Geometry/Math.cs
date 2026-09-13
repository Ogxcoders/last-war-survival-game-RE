using System.Runtime.CompilerServices;

namespace Unity.Mathematics.Geometry;

internal static class Math
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static MinMaxAABB Transform(RigidTransform transform, MinMaxAABB aabb)
	{
		float3 halfExtents = aabb.HalfExtents;
		float3 x = math.rotate(transform.rot, new float3(halfExtents.x, 0f, 0f));
		float3 x2 = math.rotate(transform.rot, new float3(0f, halfExtents.y, 0f));
		float3 x3 = math.rotate(transform.rot, new float3(0f, 0f, halfExtents.z));
		float3 @float = math.abs(x) + math.abs(x2) + math.abs(x3);
		float3 float2 = math.transform(transform, aabb.Center);
		return new MinMaxAABB(float2 - @float, float2 + @float);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static MinMaxAABB Transform(float4x4 transform, MinMaxAABB aabb)
	{
		MinMaxAABB result = Transform(new float3x3(transform), aabb);
		result.Min += transform.c3.xyz;
		result.Max += transform.c3.xyz;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static MinMaxAABB Transform(float3x3 transform, MinMaxAABB aabb)
	{
		float3 @float = transform.c0.xyz * aabb.Min.xxx;
		float3 float2 = transform.c0.xyz * aabb.Max.xxx;
		bool3 @bool = @float < float2;
		MinMaxAABB result = new MinMaxAABB(math.select(float2, @float, @bool), math.select(float2, @float, !@bool));
		@float = transform.c1.xyz * aabb.Min.yyy;
		float2 = transform.c1.xyz * aabb.Max.yyy;
		@bool = @float < float2;
		result.Min += math.select(float2, @float, @bool);
		result.Max += math.select(float2, @float, !@bool);
		@float = transform.c2.xyz * aabb.Min.zzz;
		float2 = transform.c2.xyz * aabb.Max.zzz;
		@bool = @float < float2;
		result.Min += math.select(float2, @float, @bool);
		result.Max += math.select(float2, @float, !@bool);
		return result;
	}
}
