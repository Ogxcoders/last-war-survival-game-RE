using System.Collections.Generic;
using UnityEngine;

namespace GameKit.Base;

public sealed class GameObjectPool : SingletonBehaviour<GameObjectPool>
{
	private static List<GameObject> tempList = new List<GameObject>();

	private readonly Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();

	private readonly Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();

	private readonly Dictionary<GameObject, float> pooledTimeStamps = new Dictionary<GameObject, float>();

	private bool startupPoolsCreated;

	private ITimer checkTimer;

	public float ExpiredTime { get; set; } = 60f;

	private void Start()
	{
		checkTimer = GameEntry.Timer.RegisterTimerRepeat(1f, 1f, delegate
		{
			if (CheckExpiredObject())
			{
				GameEntry.Timer.CancelTimer(checkTimer);
			}
		});
	}

	public void Shutdown()
	{
	}

	public void ClearPool()
	{
		pooledTimeStamps.Clear();
		foreach (List<GameObject> value in pooledObjects.Values)
		{
			foreach (GameObject item in value)
			{
				Object.Destroy(item);
			}
		}
		pooledObjects.Clear();
	}

	private bool CheckExpiredObject()
	{
		foreach (KeyValuePair<GameObject, float> pooledTimeStamp in pooledTimeStamps)
		{
			if (pooledTimeStamp.Key == null || Time.realtimeSinceStartup - pooledTimeStamp.Value > ExpiredTime)
			{
				tempList.Add(pooledTimeStamp.Key);
			}
		}
		foreach (GameObject temp in tempList)
		{
			pooledTimeStamps.Remove(temp);
			if (temp != null)
			{
				Object.Destroy(temp);
			}
		}
		tempList.Clear();
		foreach (KeyValuePair<GameObject, List<GameObject>> pooledObject in pooledObjects)
		{
			if (pooledObject.Key == null)
			{
				tempList.Add(pooledObject.Key);
			}
		}
		foreach (GameObject temp2 in tempList)
		{
			pooledObjects.Remove(temp2);
		}
		tempList.Clear();
		return !base.isActiveAndEnabled;
	}

	private static void RecordTimeStamp(GameObject obj)
	{
		SingletonBehaviour<GameObjectPool>.Instance.pooledTimeStamps[obj] = Time.realtimeSinceStartup;
	}

	private static void RemoveFromTimeStamp(GameObject obj)
	{
		if (SingletonBehaviour<GameObjectPool>.Instance.pooledTimeStamps.ContainsKey(obj))
		{
			SingletonBehaviour<GameObjectPool>.Instance.pooledTimeStamps.Remove(obj);
		}
	}

	public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
	{
		CreatePool(prefab.gameObject, initialPoolSize);
	}

	public static void CreatePool(GameObject prefab, int initialPoolSize)
	{
		if (!(prefab != null) || SingletonBehaviour<GameObjectPool>.Instance.pooledObjects.ContainsKey(prefab))
		{
			return;
		}
		List<GameObject> list = new List<GameObject>();
		SingletonBehaviour<GameObjectPool>.Instance.pooledObjects[prefab] = list;
		if (initialPoolSize > 0)
		{
			bool activeSelf = prefab.activeSelf;
			prefab.SetActive(value: false);
			Transform parent = SingletonBehaviour<GameObjectPool>.Instance.transform;
			while (list.Count < initialPoolSize)
			{
				GameObject gameObject = Object.Instantiate(prefab);
				gameObject.name = gameObject.name.Replace("(Clone)", "(Spawn)");
				gameObject.transform.SetParent(parent, worldPositionStays: false);
				list.Add(gameObject);
				RecordTimeStamp(gameObject);
			}
			prefab.SetActive(activeSelf);
		}
	}

	public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
	{
		return Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab, Vector3 position) where T : Component
	{
		return Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab, Transform parent) where T : Component
	{
		return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}

	public static T Spawn<T>(T prefab) where T : Component
	{
		return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}

	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject;
		if (SingletonBehaviour<GameObjectPool>.Instance.pooledObjects.TryGetValue(prefab, out var value))
		{
			gameObject = null;
			if (value.Count > 0)
			{
				while ((gameObject == null || !SingletonBehaviour<GameObjectPool>.Instance.pooledTimeStamps.ContainsKey(gameObject)) && value.Count > 0)
				{
					gameObject = value[0];
					value.RemoveAt(0);
				}
				if (gameObject != null && SingletonBehaviour<GameObjectPool>.Instance.pooledTimeStamps.ContainsKey(gameObject))
				{
					Transform obj = gameObject.transform;
					obj.SetParent(parent, worldPositionStays: false);
					obj.localPosition = position;
					obj.localRotation = rotation;
					gameObject.SetActive(value: true);
					SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects.Add(gameObject, prefab);
					RemoveFromTimeStamp(gameObject);
					return gameObject;
				}
			}
			gameObject = Object.Instantiate(prefab);
			gameObject.name = gameObject.name.Replace("(Clone)", "(Spawn)");
			Transform obj2 = gameObject.transform;
			obj2.SetParent(parent, worldPositionStays: false);
			obj2.localPosition = position;
			obj2.localRotation = rotation;
			gameObject.SetActive(value: true);
			SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects.Add(gameObject, prefab);
		}
		else
		{
			gameObject = Object.Instantiate(prefab);
			Transform component = gameObject.GetComponent<Transform>();
			component.SetParent(parent, worldPositionStays: false);
			component.localPosition = position;
			component.localRotation = rotation;
			gameObject.SetActive(value: true);
		}
		return gameObject;
	}

	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
	{
		return Spawn(prefab, parent, position, Quaternion.identity);
	}

	public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return Spawn(prefab, null, position, rotation);
	}

	public static GameObject Spawn(GameObject prefab, Transform parent)
	{
		return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	public static GameObject Spawn(GameObject prefab, Vector3 position)
	{
		return Spawn(prefab, null, position, Quaternion.identity);
	}

	public static GameObject Spawn(GameObject prefab)
	{
		return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}

	public static void Recycle<T>(T obj) where T : Component
	{
		Recycle(obj.gameObject);
	}

	public static void Recycle(GameObject obj)
	{
		if (SingletonBehaviour<GameObjectPool>.Instance != null && SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects.TryGetValue(obj, out var value))
		{
			if (obj != null)
			{
				Recycle(obj, value);
			}
			else
			{
				SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects.Remove(obj);
			}
		}
		else
		{
			Object.Destroy(obj);
		}
	}

	private static void Recycle(GameObject obj, GameObject prefab)
	{
		SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects.Remove(obj);
		if (SingletonBehaviour<GameObjectPool>.Instance.pooledObjects.TryGetValue(prefab, out var value))
		{
			value.Add(obj);
			obj.transform.SetParent(SingletonBehaviour<GameObjectPool>.Instance.transform, worldPositionStays: false);
			obj.SetActive(value: false);
			RecordTimeStamp(obj);
		}
		else
		{
			Object.Destroy(obj);
		}
	}

	public static void RecycleAll<T>(T prefab) where T : Component
	{
		RecycleAll(prefab.gameObject);
	}

	public static void RecycleAll(GameObject prefab)
	{
		if (!(SingletonBehaviour<GameObjectPool>.Instance != null))
		{
			return;
		}
		foreach (KeyValuePair<GameObject, GameObject> spawnedObject in SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects)
		{
			if (spawnedObject.Value == prefab)
			{
				tempList.Add(spawnedObject.Key);
			}
		}
		for (int i = 0; i < tempList.Count; i++)
		{
			Recycle(tempList[i]);
		}
		tempList.Clear();
	}

	public static bool IsSpawned(GameObject obj)
	{
		return SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects.ContainsKey(obj);
	}

	public static int CountPooled<T>(T prefab) where T : Component
	{
		return CountPooled(prefab.gameObject);
	}

	public static int CountPooled(GameObject prefab)
	{
		int num = 0;
		if (SingletonBehaviour<GameObjectPool>.Instance.pooledObjects.TryGetValue(prefab, out var value))
		{
			foreach (GameObject item in value)
			{
				if (item != null)
				{
					num++;
				}
			}
		}
		return num;
	}

	public static int CountSpawned<T>(T prefab) where T : Component
	{
		return CountSpawned(prefab.gameObject);
	}

	public static int CountSpawned(GameObject prefab)
	{
		int num = 0;
		foreach (GameObject value in SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects.Values)
		{
			if (prefab == value)
			{
				num++;
			}
		}
		return num;
	}

	public static int CountAllPooled()
	{
		int num = 0;
		foreach (List<GameObject> value in SingletonBehaviour<GameObjectPool>.Instance.pooledObjects.Values)
		{
			num += value.Count;
		}
		return num;
	}

	public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		if (SingletonBehaviour<GameObjectPool>.Instance.pooledObjects.TryGetValue(prefab, out var value))
		{
			list.AddRange(value);
		}
		return list;
	}

	public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		if (SingletonBehaviour<GameObjectPool>.Instance.pooledObjects.TryGetValue(prefab.gameObject, out var value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				list.Add(value[i].GetComponent<T>());
			}
		}
		return list;
	}

	public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		foreach (KeyValuePair<GameObject, GameObject> spawnedObject in SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects)
		{
			if (spawnedObject.Value == prefab)
			{
				list.Add(spawnedObject.Key);
			}
		}
		return list;
	}

	public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		GameObject gameObject = prefab.gameObject;
		foreach (KeyValuePair<GameObject, GameObject> spawnedObject in SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects)
		{
			if (spawnedObject.Value == gameObject)
			{
				list.Add(spawnedObject.Key.GetComponent<T>());
			}
		}
		return list;
	}

	public static void DestroyPooled(GameObject prefab)
	{
		if (!(SingletonBehaviour<GameObjectPool>.Instance != null) || !SingletonBehaviour<GameObjectPool>.Instance.pooledObjects.TryGetValue(prefab, out var value))
		{
			return;
		}
		SingletonBehaviour<GameObjectPool>.Instance.pooledObjects.Remove(prefab);
		for (int i = 0; i < value.Count; i++)
		{
			if (value[i] != null)
			{
				Object.Destroy(value[i]);
			}
		}
		value.Clear();
	}

	public static void DestroyPooled<T>(T prefab) where T : Component
	{
		DestroyPooled(prefab.gameObject);
	}

	public static GameObject GetPrefab(GameObject obj)
	{
		if (!SingletonBehaviour<GameObjectPool>.Instance.spawnedObjects.TryGetValue(obj, out var value))
		{
			return null;
		}
		return value;
	}

	public static GameObject GetPrefab<T>(T obj) where T : Component
	{
		return GetPrefab(obj.gameObject);
	}
}
