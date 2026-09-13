using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldCameraLODStrategy : ILODStrategy
{
	private int[] pointThresholds;

	private int[] troopThresholds;

	public void Configure(int[] points = null, int[] troops = null)
	{
		pointThresholds = points;
		troopThresholds = troops;
	}

	public int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes)
	{
		if (pointThresholds == null && troopThresholds == null)
		{
			return currentLevel;
		}
		WorldScene worldScene = SceneManager.World as WorldScene;
		if (worldScene == null)
		{
			return 0;
		}
		WorldCamera camera = worldScene.Camera;
		Transform dynamicObjNode = worldScene.DynamicObjNode;
		WorldMarchDataManager marchDataManager = worldScene.MarchDataManager;
		if (camera == null || dynamicObjNode == null || marchDataManager == null)
		{
			return currentLevel;
		}
		if (camera.GetLodLevel() > 2)
		{
			return currentLevel;
		}
		worldScene.Camera.GetLodLevel();
		int value = worldScene.PointManager?.ObjsCount ?? 0;
		int value2 = worldScene.TroopManager?.TroopCount ?? 0;
		int lODLevel = GetLODLevel(value, pointThresholds);
		int lODLevel2 = GetLODLevel(value2, troopThresholds);
		if (lODLevel <= lODLevel2)
		{
			return lODLevel2;
		}
		return lODLevel;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int GetLODLevel(int value, int[] thresholds)
	{
		if (thresholds == null)
		{
			return 0;
		}
		for (int i = 0; i < thresholds.Length; i++)
		{
			if (value < thresholds[i])
			{
				return i;
			}
		}
		return thresholds.Length;
	}
}
