using UnityEngine;

public class WorldBarricadeObject : WorldPointObject
{
	public WorldBarricadeObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		AddOldObject();
		instance = GameEntry.Resource.InstantiateAsync(GetModePath());
		instance.completed += delegate
		{
			ClearOldObject();
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.name = "WWorldBarricadeObject_" + pointIndex;
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(value: true);
				gameObject.transform.localScale = Vector3.one;
				SetClickEvent();
			}
		};
	}

	protected string GetModePath()
	{
		return "Assets/Main/Prefabs/World/DetectEvent_barricade.prefab";
	}
}
