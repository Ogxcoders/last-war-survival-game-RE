using System;

namespace UnityEngine.AI;

public struct NavMeshQueryFilter
{
	private const int k_AreaCostElementCount = 32;

	internal float[] costs { get; private set; }

	public int areaMask { get; set; }

	public int agentTypeID { get; set; }

	public float GetAreaCost(int areaIndex)
	{
		if (costs == null)
		{
			if (areaIndex < 0 || areaIndex >= 32)
			{
				string message = $"The valid range is [0:{31}]";
				throw new IndexOutOfRangeException(message);
			}
			return 1f;
		}
		return costs[areaIndex];
	}

	public void SetAreaCost(int areaIndex, float cost)
	{
		if (costs == null)
		{
			costs = new float[32];
			for (int i = 0; i < 32; i++)
			{
				costs[i] = 1f;
			}
		}
		costs[areaIndex] = cost;
	}
}
