using System.Collections.Generic;
using Protobuf;
using UnityEngine;

public class WorldDragonPointObject : WorldPointObject
{
	private int pointId;

	private long uuid;

	private DragonPointInfo dragonPoint;

	private DragonScorePointInfo dragonScorePoint;

	public BattleFieldObj bfObj;

	public AsyncMono<WorldAssistanceLabelAsync>.Handle AssistanceLabel { get; private set; }

	public WorldDragonPointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo == null)
		{
			return;
		}
		pointId = pointInfo.mainIndex;
		uuid = pointInfo.uuid;
		if (pointInfo.pointType == WorldPointType.DRAGON_BUILDING)
		{
			dragonPoint = pointInfo as DragonPointInfo;
		}
		else if (pointInfo.pointType == WorldPointType.DRAGON_SCORE_POINT)
		{
			dragonScorePoint = pointInfo as DragonScorePointInfo;
		}
		AddOldObject();
		if (instance != null)
		{
			return;
		}
		string text = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", pointInfo);
		if (text.IsNullOrEmpty())
		{
			return;
		}
		instance = GameEntry.Resource.InstantiateAsync(text);
		instance.completed += delegate
		{
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(isVisible);
				bfObj = gameObject.transform.GetComponent<BattleFieldObj>();
				SetAutoAdjustLod();
				UpdateGameObject();
				CheckShowTroopDestination();
				SetClickEvent();
				RefreshAssistanceCount();
				GameEntry.Event.Fire(EventId.DragonBuildInView, pointId);
				if (_touchObject != null)
				{
					string tabName = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetBattleFieldEntityCfgName", 1);
					string previewIconPath = "";
					string key = "";
					if (dragonPoint != null)
					{
						previewIconPath = GameEntry.Lua.CallWithReturn<string, DragonBuildingPointInfo, int>("CSharpCallLuaInterface.GetBattleFieldMiniMapSpritePath", dragonPoint.detail, 1);
						key = GameEntry.ConfigCache.GetTemplateData(tabName, dragonPoint.detail.BuildId, "name");
					}
					else if (dragonScorePoint != null)
					{
						previewIconPath = "Assets/Main/Sprites/UI/UIDesertBattle/detail/" + GameEntry.ConfigCache.GetTemplateData(tabName, dragonScorePoint.detail.ItemId, "map_icon");
						key = GameEntry.ConfigCache.GetTemplateData(tabName, dragonScorePoint.detail.ItemId, "name");
					}
					_touchObject.previewIconPath = previewIconPath;
					_touchObject.previewName = GameEntry.Localization.GetString(key);
					_touchObject.previewType = WorldPreviewType.DragonBuilding;
				}
			}
		};
	}

	private void UpdateSkin()
	{
		if (!(gameObject == null) && dragonPoint != null && !(bfObj == null))
		{
			int state = 0;
			string text = dragonPoint.detail?.AllianceId;
			if (!string.IsNullOrEmpty(text))
			{
				string allianceId = GameEntry.Data.Player.GetAllianceId();
				state = ((text != allianceId) ? 1 : 2);
			}
			bfObj.SetState(state);
		}
	}

	public override void Destroy()
	{
		AssistanceLabel?.Destroy();
		AssistanceLabel = null;
		GameEntry.Event.Fire(EventId.DragonBuildOutView, pointId);
		base.Destroy();
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			if (pointInfo.pointType == WorldPointType.DRAGON_BUILDING)
			{
				dragonPoint = pointInfo as DragonPointInfo;
				UpdateSkin();
				RefreshAssistanceCount();
			}
			else if (pointInfo.pointType == WorldPointType.DRAGON_SCORE_POINT)
			{
				dragonScorePoint = pointInfo as DragonScorePointInfo;
			}
			GameEntry.Event.Fire(EventId.DragonBuildInView, pointIndex);
		}
	}

	public override void CheckShowTroopDestination()
	{
		bool flag = isSHowDestination;
		List<WorldMarch> ownerMarches = world.GetOwnerMarches(GameEntry.Data.Player.Uid);
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		foreach (WorldMarch item in ownerMarches)
		{
			if (item.IsVisibleMarch() && pointInfo != null && pointInfo.uuid == item.targetUuid && item.targetUuid != 0L)
			{
				flag = false;
				Vector3 realPos = base.WorldPosition;
				int num = 1;
				EnumDestinationSignalType destinationType = world.GetDestinationType(item.uuid, item.targetUuid, pointIndex, item.target, isFormation: false, ref realPos, ref num);
				ShowTroopDestinationSignal(realPos, destinationType, num);
				break;
			}
		}
		if (flag)
		{
			HideTroopDestinationSignal();
		}
	}

	private void RefreshAssistanceCount()
	{
		int currentLodLevel = world.CurrentLodLevel;
		int num = 0;
		DragonPointInfo dragonPointInfo = null;
		dragonPointInfo = world.GetPointInfo(pointIndex) as DragonPointInfo;
		num = dragonPointInfo?.assistanceCount ?? 0;
		if (num > 0)
		{
			if (AssistanceLabel == null)
			{
				AssistanceLabel = AsyncMono<WorldAssistanceLabelAsync>.Handle.Load("Assets/Main/Prefabs/MainCity/WorldAssistanceLabelAllianceBuilding.prefab", world.DynamicObjNode, delegate
				{
					Transform transform = AssistanceLabel.Transform;
					if (transform != null)
					{
						transform.localPosition = base.WorldPosition;
					}
					RefreshAssistanceCount();
				});
			}
			else if (AssistanceLabel.MonoInstance != null)
			{
				AssistanceLabel.SetActive(active: true);
				WorldAssistanceLabelAsync monoInstance = AssistanceLabel.MonoInstance;
				int count = num;
				int maxAssistanceCount = dragonPointInfo.maxAssistanceCount;
				bool showMax = currentLodLevel < 4;
				WorldScene worldScene = world;
				monoInstance.SetAssistance(count, maxAssistanceCount, showMax, (object)worldScene != null && worldScene.GetMyAssistanceCount(pointIndex) > 0);
			}
		}
		else
		{
			AssistanceLabel?.SetActive(active: false);
		}
	}
}
