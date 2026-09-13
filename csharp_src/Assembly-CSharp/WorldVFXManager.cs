using System;
using System.Collections.Generic;
using FibMatrix;
using UnityEngine;

public class WorldVFXManager : WorldManagerBase
{
	private ObjectPool<WorldVFXObj> pool;

	private Dictionary<int, WorldVFXObj> allVFXObjs;

	private int count;

	private List<int> toDelete;

	public WorldVFXManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		toDelete = new List<int>();
		allVFXObjs = new Dictionary<int, WorldVFXObj>(4);
		pool = new ObjectPool<WorldVFXObj>(4);
	}

	public override void UnInit()
	{
		pool.RecycleNoClear(allVFXObjs.Values);
		pool.Dispose();
		allVFXObjs.Clear();
	}

	public int CreateVFX(string prefabPath, Vector3 pos, float duration, float delay, Action<GameObject> callback)
	{
		WorldVFXObj worldVFXObj = pool.Allocate();
		worldVFXObj.SetData(++count, prefabPath, pos, duration, delay, callback);
		allVFXObjs[count] = worldVFXObj;
		return count;
	}

	public void RemoveVFX(int id)
	{
		if (allVFXObjs.TryGetValue(id, out var value))
		{
			pool.Recycle(value);
			allVFXObjs.Remove(id);
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		if (allVFXObjs == null)
		{
			return;
		}
		foreach (WorldVFXObj value in allVFXObjs.Values)
		{
			if (value.OnUpdate(deltaTime))
			{
				toDelete.Add(value.id);
				pool.Recycle(value);
			}
		}
		if (toDelete.Count <= 0)
		{
			return;
		}
		foreach (int item in toDelete)
		{
			allVFXObjs.Remove(item);
		}
		toDelete.Clear();
	}
}
