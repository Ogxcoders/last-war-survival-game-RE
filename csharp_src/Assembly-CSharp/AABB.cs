using System;
using Unity.Mathematics;

[Serializable]
public struct AABB
{
	public float3 Center;

	public float3 Extents;

	public float3 Size => Extents * 2f;

	public float3 Min => Center - Extents;

	public float3 Max => Center + Extents;

	public override string ToString()
	{
		return $"AABB(Center:{Center}, Extents:{Extents}";
	}

	public bool Contains(float3 point)
	{
		if (point[0] < Center[0] - Extents[0])
		{
			return false;
		}
		if (point[0] > Center[0] + Extents[0])
		{
			return false;
		}
		if (point[1] < Center[1] - Extents[1])
		{
			return false;
		}
		if (point[1] > Center[1] + Extents[1])
		{
			return false;
		}
		if (point[2] < Center[2] - Extents[2])
		{
			return false;
		}
		if (point[2] > Center[2] + Extents[2])
		{
			return false;
		}
		return true;
	}

	public bool Contains(AABB b)
	{
		if (Contains(b.Center + math.float3(0f - b.Extents.x, 0f - b.Extents.y, 0f - b.Extents.z)) && Contains(b.Center + math.float3(0f - b.Extents.x, 0f - b.Extents.y, b.Extents.z)) && Contains(b.Center + math.float3(0f - b.Extents.x, b.Extents.y, 0f - b.Extents.z)) && Contains(b.Center + math.float3(0f - b.Extents.x, b.Extents.y, b.Extents.z)) && Contains(b.Center + math.float3(b.Extents.x, 0f - b.Extents.y, 0f - b.Extents.z)) && Contains(b.Center + math.float3(b.Extents.x, 0f - b.Extents.y, b.Extents.z)) && Contains(b.Center + math.float3(b.Extents.x, b.Extents.y, 0f - b.Extents.z)))
		{
			return Contains(b.Center + math.float3(b.Extents.x, b.Extents.y, b.Extents.z));
		}
		return false;
	}

	private static float3 RotateExtents(float3 extents, float3 m0, float3 m1, float3 m2)
	{
		return math.abs(m0 * extents.x) + math.abs(m1 * extents.y) + math.abs(m2 * extents.z);
	}

	public static AABB Transform(float4x4 transform, AABB localBounds)
	{
		AABB result = default(AABB);
		result.Extents = RotateExtents(localBounds.Extents, transform.c0.xyz, transform.c1.xyz, transform.c2.xyz);
		result.Center = math.transform(transform, localBounds.Center);
		return result;
	}

	public float DistanceSq(float3 point)
	{
		return math.lengthsq(math.max(math.abs(point - Center), Extents) - Extents);
	}
}
