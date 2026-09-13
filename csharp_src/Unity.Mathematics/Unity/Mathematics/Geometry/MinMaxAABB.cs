using System;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics.Geometry;

[Serializable]
internal struct MinMaxAABB : IEquatable<MinMaxAABB>
{
	public float3 Min;

	public float3 Max;

	public float3 Extents => Max - Min;

	public float3 HalfExtents => (Max - Min) * 0.5f;

	public float3 Center => (Max + Min) * 0.5f;

	public bool IsValid => math.all(Min <= Max);

	public float SurfaceArea
	{
		get
		{
			float3 x = Max - Min;
			return 2f * math.dot(x, x.yzx);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public MinMaxAABB(float3 min, float3 max)
	{
		Min = min;
		Max = max;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static MinMaxAABB CreateFromCenterAndExtents(float3 center, float3 extents)
	{
		return CreateFromCenterAndHalfExtents(center, extents * 0.5f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static MinMaxAABB CreateFromCenterAndHalfExtents(float3 center, float3 halfExtents)
	{
		return new MinMaxAABB(center - halfExtents, center + halfExtents);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(float3 point)
	{
		return math.all((point >= Min) & (point <= Max));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(MinMaxAABB aabb)
	{
		return math.all((Min <= aabb.Min) & (Max >= aabb.Max));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Overlaps(MinMaxAABB aabb)
	{
		return math.all((Max >= aabb.Min) & (Min <= aabb.Max));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Expand(float signedDistance)
	{
		Min -= signedDistance;
		Max += signedDistance;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Encapsulate(MinMaxAABB aabb)
	{
		Min = math.min(Min, aabb.Min);
		Max = math.max(Max, aabb.Max);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Encapsulate(float3 point)
	{
		Min = math.min(Min, point);
		Max = math.max(Max, point);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(MinMaxAABB other)
	{
		if (Min.Equals(other.Min))
		{
			return Max.Equals(other.Max);
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"MinMaxAABB({Min}, {Max})";
	}
}
