using System;
using System.Collections.Generic;
using UnityEngine;

namespace VEngine;

public class Assets
{
	private readonly List<Asset> preload = new List<Asset>();

	private readonly Dictionary<string, Asset> cache = new Dictionary<string, Asset>();

	private readonly Queue<Asset> queue = new Queue<Asset>();

	private int queueSize = 10;

	public void SetQueueSize(int size, bool autoMaxSize = true)
	{
		queueSize = (autoMaxSize ? Math.Max(queueSize, size) : size);
	}

	public Asset Enqueue(string path, Type type, Action<Asset> completed = null)
	{
		if (queue.Count >= queueSize)
		{
			queue.Dequeue().Release();
		}
		Asset asset = Asset.LoadAsync(path, type, completed);
		queue.Enqueue(asset);
		return asset;
	}

	public Asset Preload(string path, Type type, Action<Asset> completed = null)
	{
		if (cache.TryGetValue(path, out var value))
		{
			return value;
		}
		value = Asset.LoadAsync(path, type, completed);
		cache.Add(path, value);
		preload.Add(value);
		return value;
	}

	public T GetAsset<T>(string path) where T : UnityEngine.Object
	{
		if (cache.TryGetValue(path, out var value))
		{
			return value.Get<T>();
		}
		return null;
	}

	public float GetProgress()
	{
		int num = 0;
		foreach (Asset item in preload)
		{
			if (item.isDone)
			{
				num++;
			}
		}
		return (float)num * 1f / (float)preload.Count;
	}

	public void Clear()
	{
		foreach (Asset item in queue)
		{
			item.Release();
		}
		queue.Clear();
		foreach (Asset item2 in preload)
		{
			if (string.IsNullOrEmpty(item2.error))
			{
				item2.Release();
			}
		}
		preload.Clear();
		cache.Clear();
	}
}
