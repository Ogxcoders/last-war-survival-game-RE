using System.Collections.Generic;
using GameKit.Base;
using Unity.Collections;
using UnityEngine;

public class ManualUpdateManager : SingletonBehaviour<ManualUpdateManager>
{
	private int lastUpdateIndex;

	private readonly List<IManualUpdator> _updators = new List<IManualUpdator>();

	public float manualUpdatePercent { get; set; } = 1f;

	public void Add(IManualUpdator updator)
	{
		updator.lastUpdateTime = Time.time;
		_updators.Add(updator);
	}

	public void Remove(IManualUpdator updator)
	{
		_updators.RemoveSwapBack(updator);
	}

	public void Update()
	{
		float time = Time.time;
		int count = _updators.Count;
		if (count == 0)
		{
			return;
		}
		int num = 0;
		if (SceneManager.World != null && SceneManager.World.GetLodLevel() >= 3)
		{
			for (int i = 0; i < count; i++)
			{
				_updators[i].lastUpdateTime = time;
			}
			return;
		}
		if (manualUpdatePercent >= 1f)
		{
			for (int j = 0; j < count; j++)
			{
				_updators[j].ManualUpdate(time - _updators[j].lastUpdateTime);
				_updators[j].lastUpdateTime = time;
				num++;
			}
			return;
		}
		if (lastUpdateIndex >= count)
		{
			lastUpdateIndex = 0;
		}
		int num2 = Mathf.Clamp((int)((float)count * manualUpdatePercent), 2, count);
		int num3 = lastUpdateIndex + num2;
		if (num3 >= count)
		{
			for (int k = lastUpdateIndex; k < count; k++)
			{
				_updators[k].ManualUpdate(time - _updators[k].lastUpdateTime);
				_updators[k].lastUpdateTime = time;
				num++;
			}
			lastUpdateIndex = 0;
			num3 -= count;
		}
		for (int l = lastUpdateIndex; l < num3; l++)
		{
			_updators[l].ManualUpdate(time - _updators[l].lastUpdateTime);
			_updators[l].lastUpdateTime = time;
			num++;
		}
		lastUpdateIndex = num3;
	}
}
