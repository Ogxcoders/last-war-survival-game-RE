using System.Collections.Generic;
using GameKit.Base;
using RiverGame.PerformanceAnalysis;
using UnityEngine;

public class CPULODStrategy : ILODStrategy
{
	public int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes)
	{
		int lODLevel = SingletonBehaviour<SceneLODManager>.Instance.GetLODLevel(LODType.Effect);
		int lODLevel2 = SingletonBehaviour<SceneLODManager>.Instance.GetLODLevel(LODType.Squad);
		int a = lODLevel + lODLevel2;
		int b = 0;
		float value = PerformanceMetrics.AverageFPS.Value;
		if (value > 40f)
		{
			b = 2;
		}
		else if (value > 20f)
		{
			b = 3;
		}
		return Mathf.Min(a, b);
	}
}
