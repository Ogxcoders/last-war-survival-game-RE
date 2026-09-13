using System.Collections.Generic;
using UnityEngine;

public class DynamicCostLODStrategy : ILODStrategy
{
	private float upThresholds;

	private float downThresholds;

	private float lastUpdateTime;

	private float hysteresisTime = 5f;

	public DynamicCostLODStrategy(float upThresholds, float downThresholds, float hysteresisTime = 5f)
	{
		this.upThresholds = upThresholds;
		this.downThresholds = downThresholds;
		this.hysteresisTime = hysteresisTime;
		lastUpdateTime = 0f;
	}

	public void Configure(float upThresholds, float downThresholds, float hysteresisTime = 5f)
	{
		this.upThresholds = Mathf.Max(0.1f, upThresholds);
		this.downThresholds = Mathf.Max(0.1f, downThresholds);
		this.hysteresisTime = Mathf.Max(0.1f, hysteresisTime);
		if (this.upThresholds <= this.downThresholds)
		{
			this.upThresholds = this.downThresholds + 0.1f;
		}
	}

	public int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup - lastUpdateTime < hysteresisTime)
		{
			return currentLevel;
		}
		float num = 0f;
		foreach (ISceneLODNode node in nodes)
		{
			num += node.GetDynamicCost();
		}
		int num2 = currentLevel;
		if (num > upThresholds)
		{
			num2++;
		}
		else if (num < downThresholds)
		{
			num2--;
		}
		if (num2 != currentLevel)
		{
			lastUpdateTime = realtimeSinceStartup;
		}
		return num2;
	}
}
