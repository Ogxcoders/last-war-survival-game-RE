using System.Collections.Generic;
using UnityEngine;

public class CameraDistLODStrategy : ILODStrategy
{
	private Transform cameraTransform;

	private float[] heightThresholds;

	private int maxLODLevel;

	public CameraDistLODStrategy(Transform cameraTransform, float[] heightThresholds)
	{
		this.cameraTransform = cameraTransform;
		this.heightThresholds = heightThresholds;
		maxLODLevel = heightThresholds.Length;
	}

	public int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes)
	{
		if (cameraTransform == null || heightThresholds == null || heightThresholds.Length == 0)
		{
			return 0;
		}
		float y = cameraTransform.position.y;
		for (int i = 0; i < heightThresholds.Length; i++)
		{
			if (y <= heightThresholds[i])
			{
				return i;
			}
		}
		return maxLODLevel;
	}
}
