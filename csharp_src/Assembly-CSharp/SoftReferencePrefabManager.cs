using System;
using System.Collections.Generic;
using UnityEngine;
using VEngine;

public class SoftReferencePrefabManager
{
	private static SoftReferencePrefabManager _instance;

	private HashSet<string> AllPrefabPaths = new HashSet<string>();

	private Dictionary<string, Asset> AllPrefabAssets = new Dictionary<string, Asset>();

	private Dictionary<string, GameObject> AllPrefabGameObjects = new Dictionary<string, GameObject>();

	private Dictionary<string, List<Action<GameObject>>> _waitingCallbackQueue = new Dictionary<string, List<Action<GameObject>>>();

	public static SoftReferencePrefabManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SoftReferencePrefabManager();
			}
			return _instance;
		}
	}

	public void InitPrefabPool(string path)
	{
		if (AllPrefabPaths.Contains(path))
		{
			return;
		}
		AllPrefabPaths.Add(path);
		Asset asset = GameEntry.Resource.LoadAssetAsync(path, typeof(GameObject));
		if (asset == null)
		{
			return;
		}
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset prefab)
		{
			AllPrefabAssets[path] = prefab;
			if ((bool)prefab.asset)
			{
				AllPrefabGameObjects[path] = prefab.asset as GameObject;
				AllPrefabGameObjects[path].GameObjectCreatePool();
				if (_waitingCallbackQueue.TryGetValue(path, out var value))
				{
					foreach (Action<GameObject> item in value)
					{
						item(AllPrefabGameObjects[path].GameObjectSpawn());
					}
					value.Clear();
					_waitingCallbackQueue.Remove(path);
				}
			}
			else
			{
				Debug.LogError("Failed to load prefab at path: " + path);
			}
		});
	}

	public void ClearAll()
	{
		foreach (GameObject value in AllPrefabGameObjects.Values)
		{
			value.GameObjectRecycleAll();
		}
		foreach (Asset value2 in AllPrefabAssets.Values)
		{
			value2.Release();
		}
		AllPrefabPaths.Clear();
		AllPrefabGameObjects.Clear();
		AllPrefabAssets.Clear();
		_waitingCallbackQueue.Clear();
	}

	public void SpawnChildObjectByPath(string path, Action<GameObject> callback)
	{
		if (!GameEntry.Resource.HasAsset(path))
		{
			Debug.LogError("[SoftRef] Cant Find path: " + path);
			return;
		}
		if (AllPrefabGameObjects.TryGetValue(path, out var value))
		{
			callback(value.GameObjectSpawn());
			return;
		}
		if (!_waitingCallbackQueue.TryGetValue(path, out var value2))
		{
			value2 = new List<Action<GameObject>>();
			_waitingCallbackQueue.Add(path, value2);
		}
		value2.Add(callback);
	}
}
