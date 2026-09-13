using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using VEngine;

public class WorldTerrainAssetHandler
{
	private Asset terrainAsset;

	private Action terrainAssetLoadCallback;

	private List<GameObject> objPool = new List<GameObject>();

	private float lastRecyleTime = float.MaxValue;

	public readonly float CleanPoolTime = 120f;

	public void Init()
	{
		terrainAsset = null;
		terrainAssetLoadCallback = null;
	}

	public void Release()
	{
		ClearPool();
		ClearAsset();
	}

	public void Update()
	{
		if (Time.realtimeSinceStartup - lastRecyleTime >= CleanPoolTime && objPool.Count > 0)
		{
			ClearPool();
		}
	}

	private void ClearPool()
	{
		while (objPool.Count != 0)
		{
			GameObject gameObject = objPool[0];
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			objPool.RemoveAt(0);
		}
	}

	private void ClearAsset()
	{
		if (terrainAsset != null)
		{
			terrainAsset.Release();
		}
		terrainAsset = null;
	}

	public bool IsAssetLoadFinish()
	{
		if (terrainAsset != null && terrainAsset.isDone)
		{
			return terrainAsset.asset != null;
		}
		return false;
	}

	public void LoadTerrain(string terrainPrefabName, Action callback)
	{
		terrainAssetLoadCallback = callback;
		if (terrainAsset != null && terrainAsset.pathOrURL != terrainPrefabName)
		{
			ClearPool();
			ClearAsset();
		}
		if (terrainAsset != null && terrainAsset.isDone)
		{
			InvokeLoadCallback();
		}
		else if (terrainAsset == null)
		{
			terrainAsset = GameEntry.Resource.LoadAsset(terrainPrefabName, typeof(GameObject));
			Asset asset = terrainAsset;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, new Action<Asset>(OnTerrainLoaded));
		}
		else if (terrainAsset.isDone)
		{
			InvokeLoadCallback();
		}
		else
		{
			LogWarning("Load terrain when loading is still processing");
		}
	}

	public GameObject Spawn()
	{
		lastRecyleTime = float.MaxValue;
		GameObject gameObject = null;
		if (objPool.Count > 0)
		{
			gameObject = objPool[0];
			objPool.RemoveAt(0);
		}
		else if (terrainAsset != null)
		{
			if (terrainAsset.asset != null)
			{
				gameObject = UnityEngine.Object.Instantiate(terrainAsset.asset) as GameObject;
			}
			else
			{
				LogError("spawn when load is not done");
			}
		}
		else
		{
			LogError("spawn when load is not started");
		}
		if (gameObject != null)
		{
			gameObject.name = gameObject.name.Replace("_hide", "");
		}
		return gameObject;
	}

	public void Recycle(GameObject gameObject)
	{
		if (!(gameObject == null))
		{
			objPool.Add(gameObject);
			gameObject.name += "_hide";
			gameObject.transform.position = new Vector3(10000f, 10000f, 0f);
			lastRecyleTime = Time.realtimeSinceStartup;
		}
	}

	private void InvokeLoadCallback()
	{
		if (terrainAssetLoadCallback != null)
		{
			terrainAssetLoadCallback();
			terrainAssetLoadCallback = null;
		}
	}

	private void OnTerrainLoaded(Asset asset)
	{
		InvokeLoadCallback();
	}

	private void LogWarning(string message)
	{
		Log.Warning("World Terrain Asset Handler Warning: " + message);
	}

	private void LogError(string message)
	{
		Log.Error("World Terrain Asset Handler Error: " + message);
	}
}
