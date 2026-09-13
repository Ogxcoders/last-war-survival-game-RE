using System;
using Unity.Mathematics;

[Serializable]
public struct MinMaxAABB : IEquatable<MinMaxAABB>
{
	public float3 Min;

	public float3 Max;

	public bool IsEmpty => Equals(Empty);

	public static MinMaxAABB Empty => new MinMaxAABB
	{
		Min = math.float3(float.PositiveInfinity),
		Max = math.float3(float.NegativeInfinity)
	};

	public void Encapsulate(MinMaxAABB aabb)
	{
		Min = math.min(Min, aabb.Min);
		Max = math.max(Max, aabb.Max);
	}

	public void Encapsulate(float3 point)
	{
		Min = math.min(Min, point);
		Max = math.max(Max, point);
	}

	public static implicit operator MinMaxAABB(AABB aabb)
	{
		return new MinMaxAABB
		{
			Min = aabb.Center - aabb.Extents,
			Max = aabb.Center + aabb.Extents
		};
	}

	public static implicit operator AABB(MinMaxAABB aabb)
	{
		return new AABB
		{
			Center = (aabb.Min + aabb.Max) * 0.5f,
			Extents = (aabb.Max - aabb.Min) * 0.5f
		};
	}

	public bool Equals(MinMaxAABB other)
	{
		if (Min.Equals(other.Min))
		{
			return Max.Equals(other.Max);
		}
		return false;
	}
}
