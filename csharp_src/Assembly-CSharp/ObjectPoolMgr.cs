using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;

public class ObjectPoolMgr
{
	private static readonly float _maxUpdateTimeSlice = 10f;

	private static double _cleanStartTime = 0.0;

	private GameObject root;

	private Dictionary<string, ObjectPool> poolList = new Dictionary<string, ObjectPool>();

	private List<string> unusedPool = new List<string>();

	private HashSet<string> _cleanPoolList = new HashSet<string>();

	private bool canCleanPool => (double)Time.realtimeSinceStartup - _cleanStartTime < (double)_maxUpdateTimeSlice;

	public Transform Root => root.transform;

	public Dictionary<string, ObjectPool> interface_poolList => poolList;

	public ObjectPoolMgr()
	{
		root = new GameObject("ObjectPoolRoot");
		Object.DontDestroyOnLoad(root);
	}

	public ObjectPool GetPool(string prefabPath, ResourceManager resourceManager, ObjectPoolTag tag = ObjectPoolTag.Normal)
	{
		if (prefabPath == null)
		{
			return null;
		}
		if (!poolList.TryGetValue(prefabPath, out var value))
		{
			value = new ObjectPool(this, prefabPath, resourceManager, tag);
			poolList.Add(prefabPath, value);
		}
		return value;
	}

	public bool PrefabHasCache(string prefabPath)
	{
		if (string.IsNullOrEmpty(prefabPath))
		{
			return false;
		}
		if (!poolList.TryGetValue(prefabPath, out var value))
		{
			return false;
		}
		return value.GetPoolCount() > 0;
	}

	public bool PrefabPoolIsReady(string prefabPath)
	{
		if (string.IsNullOrEmpty(prefabPath))
		{
			return false;
		}
		if (!poolList.TryGetValue(prefabPath, out var value))
		{
			return false;
		}
		if (value?.Request == null)
		{
			return false;
		}
		return value.Request.isDone;
	}

	public void ClearPool(string prefabPath)
	{
		if (poolList.TryGetValue(prefabPath, out var value))
		{
			value.Clear();
			poolList.Remove(prefabPath);
			_cleanPoolList.Remove(prefabPath);
		}
	}

	public void ClearPoolByTag(ObjectPoolTag tag)
	{
		foreach (KeyValuePair<string, ObjectPool> pool in poolList)
		{
			ObjectPool value = pool.Value;
			if (value.tag == tag)
			{
				value.SetWaitForClear();
			}
		}
	}

	public void ClearPoolByTagGroup(ObjectPoolTagGroup group)
	{
		ObjectPoolTag[] tags = group.GetTags();
		for (int i = 0; i < tags.Length; i++)
		{
			ClearPoolByTag(tags[i]);
		}
	}

	public void ClearAllPool()
	{
		foreach (ObjectPool value in poolList.Values)
		{
			value.Clear();
		}
		poolList.Clear();
		_cleanPoolList.Clear();
	}

	public void ClearUnusedPool()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, ObjectPool> pool in poolList)
		{
			if (pool.Value.Clear())
			{
				list.Add(pool.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			poolList.Remove(list[i]);
			_cleanPoolList.Remove(list[i]);
		}
	}

	public void DebugOutput()
	{
		int num = 0;
		int num2 = 0;
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, ObjectPool> pool in poolList)
		{
			string key = pool.Key;
			ObjectPool value = pool.Value;
			num += value.GetPoolCount();
			num2 += value.GetObjCount();
			stringBuilder.AppendLine($"pool: {key}, pool obj count: {value.GetPoolCount()}, total obj count: {value.GetObjCount()}");
		}
		Log.Info($"ObjectPoolMgr: total pool obj count: {num}, total obj count: {num2}\n" + stringBuilder.ToString());
	}

	public void TryCleanPool()
	{
		_cleanStartTime = Time.realtimeSinceStartup;
		HashSet<string>.Enumerator enumerator = _cleanPoolList.GetEnumerator();
		while (enumerator.MoveNext() && canCleanPool)
		{
			string current = enumerator.Current;
			if (!poolList.TryGetValue(current, out var value))
			{
				unusedPool.Add(current);
			}
			else if (value.TryClean())
			{
				unusedPool.Add(current);
			}
		}
		if (unusedPool.Count <= 0)
		{
			return;
		}
		foreach (string item in unusedPool)
		{
			poolList.Remove(item);
			_cleanPoolList.Remove(item);
		}
		unusedPool.Clear();
	}

	public void RegisterToCleanPoolList(string prefabPath)
	{
		_cleanPoolList.Add(prefabPath);
	}

	public void UnRegisterToCleanPoolList(string prefabPath)
	{
		_cleanPoolList.Remove(prefabPath);
	}
}
