using GameKit.Base;
using UnityEngine;

public static class UnityEngineGameObjectExtention
{
	public static void GameObjectCreatePool(this GameObject prefab)
	{
		prefab.CreatePool();
	}

	public static GameObject GameObjectSpawn(this GameObject prefab)
	{
		return prefab.Spawn();
	}

	public static GameObject GameObjectSpawn(this GameObject prefab, Transform parent)
	{
		return prefab.Spawn(parent);
	}

	public static void GameObjectRecycle(this GameObject obj)
	{
		obj.Recycle();
	}

	public static void GameObjectRecycleAll(this GameObject prefab)
	{
		prefab.RecycleAll();
	}

	public static void GameObjectDestroyAll(this GameObject prefab)
	{
		prefab.RecycleAll();
		prefab.DestroyPooled();
	}

	public static int CountPooled(this GameObject prefab)
	{
		return GameObjectPool.CountPooled(prefab);
	}

	public static GameObject GetRootParent(this GameObject obj)
	{
		Transform transform = obj.transform;
		while (transform.parent != null)
		{
			transform = transform.parent;
		}
		return transform.gameObject;
	}
}
