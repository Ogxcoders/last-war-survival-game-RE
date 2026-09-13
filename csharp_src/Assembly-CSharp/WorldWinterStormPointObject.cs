using System.Collections.Generic;
using Protobuf;
using UnityEngine;

public class WorldWinterStormPointObject : WorldPointObject
{
	private int pointId;

	private long uuid;

	private WinterStormPointInfo winterPoint;

	public BattleFieldObj bfObj;

	public AsyncMono<WorldAssistanceLabelAsync>.Handle AssistanceLabel { get; private set; }

	public WorldWinterStormPointObject(WorldScene worldScene, int pointIndex, int pType)
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
		winterPoint = pointInfo as WinterStormPointInfo;
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
					string tabName = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetBattleFieldEntityCfgName", 2);
					string previewIconPath = "";
					string key = "";
					if (winterPoint != null)
					{
						previewIconPath = GameEntry.Lua.CallWithReturn<string, WinterEntityPointInfo, int>("CSharpCallLuaInterface.GetBattleFieldMiniMapSpritePath", winterPoint.detail, 2);
						key = GameEntry.ConfigCache.GetTemplateData(tabName, winterPoint.detail.BuildId, "name");
					}
					_touchObject.previewIconPath = previewIconPath;
					_touchObject.previewName = GameEntry.Localization.GetString(key);
					_touchObject.previewType = WorldPreviewType.WinterEntity;
				}
			}
		};
	}

	private void UpdateSkin()
	{
		if (!(gameObject == null) && winterPoint != null && !(bfObj == null))
		{
			int state = 0;
			int num = winterPoint.detail?.Side ?? 0;
			if (num != 0)
			{
				state = ((GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetPlayerSideInBattleField", 2) != num) ? 1 : 2);
			}
			bfObj.SetState(state);
			GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateBattleFieldSkin", pointIndex, 2);
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
			winterPoint = pointInfo as WinterStormPointInfo;
			UpdateSkin();
			RefreshAssistanceCount();
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
		WinterStormPointInfo winterStormPointInfo = null;
		winterStormPointInfo = world.GetPointInfo(pointIndex) as WinterStormPointInfo;
		num = winterStormPointInfo?.assistanceCount ?? 0;
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
				int maxAssistanceCount = winterStormPointInfo.maxAssistanceCount;
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
