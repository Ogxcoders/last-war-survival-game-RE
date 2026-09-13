using UnityEngine;

public class WorldCollectObject : WorldPointObject
{
	private WorldCollectObjectView collectView;

	public WorldCollectObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		AddOldObject();
		instance = GameEntry.Resource.InstantiateAsync(GetAssetPath());
		instance.completed += delegate
		{
			ClearOldObject();
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(isVisible);
				collectView = gameObject.GetComponent<WorldCollectObjectView>();
				if (collectView != null && world.GetPointInfo(pointIndex) is CollectPointInfo collectPointInfo && collectView != null)
				{
					collectView.Init(pointIndex, collectPointInfo.resourceType);
					collectView.SetName(GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetResourceNameByType", collectPointInfo.GetResourceType()));
					collectView.ShowNameTitle(isShow: true);
				}
				UpdateGameObject();
				CheckShowTroopDestination();
				SetClickEvent();
				GameEntry.Event.Fire(EventId.WorldCollectPointInView, pointIndex);
			}
		};
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		if (world.GetPointInfo(pointIndex) is CollectPointInfo collectPointInfo && collectView != null)
		{
			collectView.SetLevel(collectPointInfo.level);
		}
	}

	public override void Destroy()
	{
		GameEntry.Event.Fire(EventId.WorldCollectPointOutView, pointIndex);
		if (collectView != null)
		{
			collectView.UnInit();
		}
		base.Destroy();
	}

	private string GetAssetPath()
	{
		return (world.GetPointInfo(pointIndex) as CollectPointInfo).resourceType switch
		{
			ResourceType.Oil => "Assets/Main/Prefabs/CollectResource/CollectResourcesOil.prefab", 
			ResourceType.Metal => "Assets/Main/Prefabs/CollectResource/CollectResourceIron.prefab", 
			ResourceType.Water => "Assets/Main/Prefabs/CollectResource/CollectResourceWater.prefab", 
			ResourceType.Electricity => "Assets/Main/Prefabs/CollectResource/CollectResourcesOil.prefab", 
			ResourceType.GOLD => "Assets/Main/Prefabs/CollectResource/CollectResourcesGold.prefab", 
			ResourceType.Food => "Assets/Main/Prefabs/CollectResource/CollectResourcesMoney.prefab", 
			_ => "Assets/Main/Prefabs/CollectResource/CollectResourcesOil.prefab", 
		};
	}

	public override void CheckShowTroopDestination()
	{
		bool flag = isSHowDestination;
		foreach (WorldMarch ownerMarch in world.GetOwnerMarches(GameEntry.Data.Player.Uid))
		{
			if (ownerMarch.IsVisibleMarch() && ownerMarch.target == MarchTargetType.COLLECT && ownerMarch.targetPos == pointIndex)
			{
				flag = false;
				Vector3 realPos = base.WorldPosition;
				int num = 1;
				EnumDestinationSignalType destinationType = world.GetDestinationType(ownerMarch.uuid, ownerMarch.targetUuid, pointIndex, ownerMarch.target, isFormation: false, ref realPos, ref num);
				ShowTroopDestinationSignal(realPos, destinationType, num);
				break;
			}
		}
		if (flag)
		{
			HideTroopDestinationSignal();
		}
	}
}
