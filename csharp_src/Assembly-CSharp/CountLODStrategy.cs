using System.Collections.Generic;

public class CountLODStrategy : ILODStrategy
{
	private int[] countThresholds;

	public CountLODStrategy()
	{
	}

	public CountLODStrategy(int[] thresholds)
	{
		countThresholds = thresholds;
	}

	public void Configure(int[] thresholds)
	{
		countThresholds = thresholds;
	}

	public int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes)
	{
		if (countThresholds == null || countThresholds.Length == 0)
		{
			return 0;
		}
		int count = nodes.Count;
		for (int i = 0; i < countThresholds.Length; i++)
		{
			if (count <= countThresholds[i])
			{
				return i;
			}
		}
		return countThresholds.Length;
	}
}
