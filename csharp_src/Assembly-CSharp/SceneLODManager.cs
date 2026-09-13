using System;
using System.Collections.Generic;
using GameKit.Base;
using Unity.Collections;
using UnityEngine;

public class SceneLODManager : SingletonBehaviour<SceneLODManager>
{
	private class LODTypeInfo
	{
		public List<ISceneLODNode> nodes = new List<ISceneLODNode>();

		public List<ILODStrategy> strategies = new List<ILODStrategy>();

		public List<ISceneLODNode> updateList = new List<ISceneLODNode>();

		public int currentLevel;

		public float lastUpdateTime;

		public float updateInterval;

		public float updatePercentPerFrame;

		public int updateMinNodesPerFrame;

		public int lastUpdateIndex;
	}

	private int lodMin;

	private int lodMax = 4;

	private Dictionary<LODType, LODTypeInfo> typeInfos = new Dictionary<LODType, LODTypeInfo>();

	public void SetupLODType(LODType type, float updateInterval = 5f, int updateMinNodesPerFrame = 1)
	{
		if (typeInfos.TryGetValue(type, out var value))
		{
			value.updateInterval = updateInterval;
			value.updatePercentPerFrame = 0.03f / updateInterval;
			value.updateMinNodesPerFrame = updateMinNodesPerFrame;
		}
	}

	public List<ILODStrategy> GetStrategies(LODType type)
	{
		if (typeInfos.TryGetValue(type, out var value))
		{
			return value.strategies;
		}
		return new List<ILODStrategy>();
	}

	public ILODStrategy GetStrategy(LODType type, Type strategyType)
	{
		if (typeInfos.TryGetValue(type, out var value))
		{
			foreach (ILODStrategy strategy in value.strategies)
			{
				if (strategy.GetType() == strategyType)
				{
					return strategy;
				}
			}
		}
		return null;
	}

	public void ClearStrategies(LODType type)
	{
		if (typeInfos.TryGetValue(type, out var value))
		{
			value.strategies.Clear();
		}
	}

	public void ClearAllStrategies()
	{
		foreach (KeyValuePair<LODType, LODTypeInfo> typeInfo in typeInfos)
		{
			typeInfo.Value.strategies.Clear();
		}
	}

	public void SetupLODRange(int min, int max)
	{
		lodMin = min;
		lodMax = max;
	}

	public void AddStrategy(LODType type, ILODStrategy strategy)
	{
		if (!typeInfos.TryGetValue(type, out var value))
		{
			value = new LODTypeInfo();
			typeInfos[type] = value;
			SetupLODType(type);
		}
		value.strategies.Add(strategy);
	}

	public void RemoveStrategy(LODType type, ILODStrategy strategy)
	{
		if (typeInfos.TryGetValue(type, out var value))
		{
			value.strategies.Remove(strategy);
		}
	}

	public void AddNode(ISceneLODNode node)
	{
		LODType lODType = node.GetLODType();
		if (!typeInfos.TryGetValue(lODType, out var value))
		{
			value = new LODTypeInfo();
			typeInfos[lODType] = value;
			SetupLODType(lODType);
		}
		value.nodes.Add(node);
		node.UpdateLOD(value.currentLevel);
	}

	public void RemoveNode(ISceneLODNode node)
	{
		LODType lODType = node.GetLODType();
		if (typeInfos.TryGetValue(lODType, out var value))
		{
			value.nodes.RemoveSwapBack(node);
			value.updateList.RemoveSwapBack(node);
		}
	}

	public int GetLODLevel(LODType type)
	{
		if (typeInfos.TryGetValue(type, out var value))
		{
			return value.currentLevel;
		}
		return 0;
	}

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		foreach (KeyValuePair<LODType, LODTypeInfo> typeInfo in typeInfos)
		{
			LODTypeInfo value = typeInfo.Value;
			if (value.lastUpdateIndex >= 0)
			{
				int count = value.updateList.Count;
				int num = Mathf.Max(value.updateMinNodesPerFrame, Mathf.CeilToInt((float)count * value.updatePercentPerFrame));
				int num2 = Mathf.Min(value.lastUpdateIndex, value.updateList.Count - 1);
				int num3 = Mathf.Max(num2 - num, 0);
				for (int num4 = num2; num4 >= num3; num4--)
				{
					value.updateList[num4].UpdateLOD(value.currentLevel);
				}
				value.lastUpdateIndex = num3 - 1;
				if (value.lastUpdateIndex >= 0)
				{
					continue;
				}
			}
			if (value.strategies.Count == 0 || realtimeSinceStartup - value.lastUpdateTime < value.updateInterval)
			{
				continue;
			}
			int num5 = 0;
			foreach (ILODStrategy strategy in value.strategies)
			{
				int value2 = strategy.CalculateLODLevel(value.currentLevel, value.nodes);
				value2 = Mathf.Clamp(value2, lodMin, lodMax);
				num5 = ((num5 > value2) ? num5 : value2);
			}
			if (num5 != value.currentLevel)
			{
				value.currentLevel = num5;
				value.updateList.Clear();
				value.updateList.AddRange(value.nodes);
				value.lastUpdateIndex = value.nodes.Count - 1;
			}
			value.lastUpdateTime = realtimeSinceStartup;
		}
	}
}
