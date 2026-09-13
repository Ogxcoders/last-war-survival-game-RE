using System.Collections.Generic;

public class CostLODStrategy : ILODStrategy
{
	private float[] costThresholds;

	public CostLODStrategy()
	{
	}

	public CostLODStrategy(float[] thresholds)
	{
		costThresholds = thresholds;
	}

	public void Configure(float[] thresholds)
	{
		costThresholds = thresholds;
	}

	public int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes)
	{
		if (costThresholds == null || costThresholds.Length == 0)
		{
			return 0;
		}
		float num = 0f;
		foreach (ISceneLODNode node in nodes)
		{
			num += node.GetCost();
		}
		for (int i = 0; i < costThresholds.Length; i++)
		{
			if (num <= costThresholds[i])
			{
				return i;
			}
		}
		return costThresholds.Length;
	}
}
