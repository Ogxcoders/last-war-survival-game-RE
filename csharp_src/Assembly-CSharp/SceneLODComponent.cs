using System;
using System.Collections.Generic;
using GameKit.Base;
using UnityEngine;

public class SceneLODComponent : MonoBehaviour, ISceneLODNode
{
	[Serializable]
	public class LODGameObject
	{
		public GameObject gameObject;

		public int minLOD;

		public int maxLOD;

		private void ValidateLODRange()
		{
			if (maxLOD < minLOD)
			{
				maxLOD = minLOD;
			}
		}
	}

	[SerializeField]
	protected LODType type;

	protected int currentLODLevel = -1;

	[SerializeField]
	protected List<LODGameObject> lodGameObjects = new List<LODGameObject>();

	[SerializeField]
	protected float cost = 1f;

	protected float dynamicCost = -1f;

	protected virtual void OnEnable()
	{
		SingletonBehaviour<SceneLODManager>.Instance.AddNode(this);
	}

	protected virtual void OnDisable()
	{
		SingletonBehaviour<SceneLODManager>.Instance?.RemoveNode(this);
	}

	public LODType GetLODType()
	{
		return type;
	}

	public float GetCost()
	{
		return cost;
	}

	public float GetDynamicCost()
	{
		if (dynamicCost < 0f)
		{
			OnLODLevelChanged(currentLODLevel);
		}
		return dynamicCost;
	}

	public int CurrentLOD()
	{
		return currentLODLevel;
	}

	public void UpdateLOD(int level)
	{
		if (currentLODLevel != level)
		{
			currentLODLevel = level;
			OnLODLevelChanged(level);
		}
	}

	protected virtual void OnLODLevelChanged(int level)
	{
		dynamicCost = 0f;
		foreach (LODGameObject lodGameObject in lodGameObjects)
		{
			if (lodGameObject.gameObject != null)
			{
				bool flag = level >= lodGameObject.minLOD && level <= lodGameObject.maxLOD;
				lodGameObject.gameObject.SetActive(flag);
				if (flag)
				{
					dynamicCost += 1f;
				}
			}
		}
	}

	protected virtual bool ShouldShowEditType()
	{
		return true;
	}
}
