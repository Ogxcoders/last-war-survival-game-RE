using System;
using UnityEngine;

public class WorldWerewolfObject : IDisposable
{
	private const string WOLF_ALLY = "Assets/Main/SeasonRes/S4/Prefabs/World/WerewolfAlly.prefab";

	private const string WOLF_ENEMY = "Assets/Main/SeasonRes/S4/Prefabs/World/WerewolfEnemy.prefab";

	public WorldIconRendererFacade.HappyIcon happyIcon;

	private InstanceRequest instance;

	public void Dispose()
	{
		happyIcon?.Destroy();
		happyIcon = null;
		instance?.Destroy();
		instance = null;
	}

	public void Init(WorldScene world, int point, bool isAlly, bool supportInstancing)
	{
		Vector3 worldPosition = world.TileIndexToWorld(point);
		if (supportInstancing)
		{
			happyIcon = happyIcon ?? world?.IconRendererFacade?.CreateIcon("HappyWolf");
			happyIcon?.Refresh(worldPosition, isAlly ? 10 : 9);
			return;
		}
		instance = GameEntry.Resource.InstantiateAsync(isAlly ? "Assets/Main/SeasonRes/S4/Prefabs/World/WerewolfAlly.prefab" : "Assets/Main/SeasonRes/S4/Prefabs/World/WerewolfEnemy.prefab");
		instance.completed += delegate
		{
			GameObject gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = worldPosition;
				(gameObject.GetComponent<AutoAdjustScale>() ?? gameObject.AddComponent<AutoAdjustScale>())?.SetStandaloneScaleSpeed(0, 7, 50f);
			}
		};
	}
}
