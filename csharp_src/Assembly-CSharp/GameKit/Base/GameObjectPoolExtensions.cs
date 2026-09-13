using System.Collections.Generic;
using UnityEngine;

namespace GameKit.Base;

public static class GameObjectPoolExtensions
{
	public static void CreatePool<T>(this T prefab) where T : Component
	{
		GameObjectPool.CreatePool(prefab, 0);
	}

	public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
	{
		GameObjectPool.CreatePool(prefab, initialPoolSize);
	}

	public static void CreatePool(this GameObject prefab)
	{
		GameObjectPool.CreatePool(prefab, 0);
	}

	public static void CreatePool(this GameObject prefab, int initialPoolSize)
	{
		GameObjectPool.CreatePool(prefab, initialPoolSize);
	}

	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return GameObjectPool.Spawn(prefab, parent, position, rotation);
	}

	public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return GameObjectPool.Spawn(prefab, null, position, rotation);
	}

	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
	{
		return GameObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
	}

	public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
	{
		return GameObjectPool.Spawn(prefab, null, position, Quaternion.identity);
	}

	public static T Spawn<T>(this T prefab, Transform parent) where T : Component
	{
		return GameObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	public static T Spawn<T>(this T prefab) where T : Component
	{
		return GameObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}

	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		return GameObjectPool.Spawn(prefab, parent, position, rotation);
	}

	public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return GameObjectPool.Spawn(prefab, null, position, rotation);
	}

	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
	{
		return GameObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
	}

	public static GameObject Spawn(this GameObject prefab, Vector3 position)
	{
		return GameObjectPool.Spawn(prefab, null, position, Quaternion.identity);
	}

	public static GameObject Spawn(this GameObject prefab, Transform parent)
	{
		return GameObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	public static GameObject Spawn(this GameObject prefab)
	{
		return GameObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}

	public static void Recycle<T>(this T obj) where T : Component
	{
		GameObjectPool.Recycle(obj);
	}

	public static void Recycle(this GameObject obj)
	{
		GameObjectPool.Recycle(obj);
	}

	public static void RecycleAll<T>(this T prefab) where T : Component
	{
		GameObjectPool.RecycleAll(prefab);
	}

	public static void RecycleAll(this GameObject prefab)
	{
		GameObjectPool.RecycleAll(prefab);
	}

	public static int CountPooled<T>(this T prefab) where T : Component
	{
		return GameObjectPool.CountPooled(prefab);
	}

	public static int CountPooled(this GameObject prefab)
	{
		return GameObjectPool.CountPooled(prefab);
	}

	public static int CountSpawned<T>(this T prefab) where T : Component
	{
		return GameObjectPool.CountSpawned(prefab);
	}

	public static int CountSpawned(this GameObject prefab)
	{
		return GameObjectPool.CountSpawned(prefab);
	}

	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return GameObjectPool.GetSpawned(prefab, list, appendList);
	}

	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
	{
		return GameObjectPool.GetSpawned(prefab, list, appendList: false);
	}

	public static List<GameObject> GetSpawned(this GameObject prefab)
	{
		return GameObjectPool.GetSpawned(prefab, null, appendList: false);
	}

	public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return GameObjectPool.GetSpawned(prefab, list, appendList);
	}

	public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
	{
		return GameObjectPool.GetSpawned(prefab, list, appendList: false);
	}

	public static List<T> GetSpawned<T>(this T prefab) where T : Component
	{
		return GameObjectPool.GetSpawned(prefab, null, appendList: false);
	}

	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return GameObjectPool.GetPooled(prefab, list, appendList);
	}

	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
	{
		return GameObjectPool.GetPooled(prefab, list, appendList: false);
	}

	public static List<GameObject> GetPooled(this GameObject prefab)
	{
		return GameObjectPool.GetPooled(prefab, null, appendList: false);
	}

	public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return GameObjectPool.GetPooled(prefab, list, appendList);
	}

	public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
	{
		return GameObjectPool.GetPooled(prefab, list, appendList: false);
	}

	public static List<T> GetPooled<T>(this T prefab) where T : Component
	{
		return GameObjectPool.GetPooled(prefab, null, appendList: false);
	}

	public static void DestroyPooled(this GameObject prefab)
	{
		GameObjectPool.DestroyPooled(prefab);
	}

	public static void DestroyPooled<T>(this T prefab) where T : Component
	{
		GameObjectPool.DestroyPooled(prefab.gameObject);
	}

	public static GameObject GetPrefab(this GameObject obj)
	{
		return GameObjectPool.GetPrefab(obj);
	}

	public static GameObject GetPrefab<T>(this T obj) where T : Component
	{
		return GameObjectPool.GetPrefab(obj.gameObject);
	}
}
