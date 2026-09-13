using UnityEngine;

namespace MiniGame.Biubiu.Client;

public static class ComponentPrefabClientExt
{
	public static GameObject GetPrefab(this ComponentPrefabClient comp)
	{
		if (comp.ResourceHolder == null)
		{
			return null;
		}
		if (comp.Prefab != null)
		{
			return (GameObject)comp.Prefab;
		}
		if (!comp.ResourceHolder.IsError && comp.ResourceHolder.IsValid && comp.ResourceHolder.IsDone)
		{
			comp.Prefab = comp.ResourceHolder.As<GameObject>();
			return (GameObject)comp.Prefab;
		}
		return null;
	}

	public static void SetPrefab(this ComponentPrefabClient comp, GameObject go)
	{
		if (comp.ResourceHolder == null)
		{
			comp.ResourceHolder = new SnapshotGameObjectHolder(go);
			return;
		}
		comp.ResourceHolder.Dispose();
		((SnapshotGameObjectHolder)comp.ResourceHolder).Data = go;
	}
}
