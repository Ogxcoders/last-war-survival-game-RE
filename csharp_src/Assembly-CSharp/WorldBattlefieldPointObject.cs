using System.Collections.Generic;
using Protobuf;
using UnityEngine;

public class WorldBattlefieldPointObject : WorldPointObject
{
	private int pointId;

	private BattlefieldBuildPointInfo _battlefieldPoint;

	private Transform stateIconParent;

	private SpriteRenderer stateIcon;

	private long _gatherMarchUuid;

	private string stateIconPath;

	public BattleFieldObj bfObj;

	public BattlefieldBuildingObject buildingObj;

	private int buildType;

	private int buildRole;

	private int worldType;

	private BattleFieldType battleFieldType;

	private bool IsRes => buildType == 5;

	private bool IsScore => buildType == 6;

	public AsyncMono<WorldAssistanceLabelAsync>.Handle AssistanceLabel { get; private set; }

	public WorldBattlefieldPointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		worldType = GameEntry.Data?.Player?.GetWorldType() ?? 0;
		battleFieldType = (BattleFieldType)worldType;
		string tbName = string.Empty;
		if (pointInfo == null)
		{
			return;
		}
		pointId = pointInfo.mainIndex;
		_battlefieldPoint = pointInfo as BattlefieldBuildPointInfo;
		if (_battlefieldPoint != null)
		{
			tbName = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetBattleFieldEntityCfgName", worldType);
			buildType = GameEntry.ConfigCache.GetTemplateData(tbName, _battlefieldPoint.detail.BuildId, "type").ToInt();
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
				buildingObj = gameObject.GetComponent<BattlefieldBuildingObject>();
				if (buildingObj == null)
				{
					bfObj = gameObject.transform.GetComponent<BattleFieldObj>();
				}
				SetAutoAdjustLod();
				UpdateGameObject();
				CheckShowTroopDestination();
				SetClickEvent();
				RefreshAssistanceCount();
				if (_touchObject != null && !string.IsNullOrEmpty(tbName))
				{
					string templateData = GameEntry.ConfigCache.GetTemplateData(tbName, _battlefieldPoint.detail.BuildId, "name");
					string previewIconPath = GameEntry.Lua.CallWithReturn<string, QuarantinePointInfo, int>("CSharpCallLuaInterface.GetBattlefieldPreviewSpritePath", _battlefieldPoint.detail, worldType);
					_touchObject.previewIconPath = previewIconPath;
					_touchObject.previewName = GameEntry.Localization.GetString(templateData);
					_touchObject.previewType = WorldPreviewType.BattlefieldBuild;
				}
				if (IsRes)
				{
					if (buildingObj != null)
					{
						UpdateMarchNew();
					}
					else
					{
						stateIconParent = gameObject.transform.Find("Model/stateIcon");
						stateIcon = stateIconParent?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
						UpdateMarchLegacy();
					}
				}
			}
		};
	}

	private void UpdateSkinLegacy()
	{
		if (!(gameObject == null) && _battlefieldPoint != null && !(bfObj == null))
		{
			int state = 0;
			int num = _battlefieldPoint.detail?.Role ?? 0;
			if (num != 0)
			{
				state = (GameEntry.Lua.CallWithReturn<bool, int, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", num, worldType) ? 1 : 2);
			}
			bfObj.SetState(state);
			if (bfObj.SimpleAnimation != null)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateBattleFieldSkin", pointIndex, worldType);
			}
		}
	}

	private void UpdateSkinNew()
	{
		if (!(gameObject == null) && _battlefieldPoint != null)
		{
			int param = _battlefieldPoint.detail?.Role ?? 0;
			int styleIndex = GameEntry.Lua.CallWithReturn<int, int, int, int>("CSharpCallLuaInterface.GetBattlefieldBuildingColor", pointIndex, worldType, param);
			buildingObj?.SetStyleIndex(styleIndex);
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
		if (pointInfo == null)
		{
			return;
		}
		_battlefieldPoint = pointInfo as BattlefieldBuildPointInfo;
		if (!IsScore)
		{
			if (bfObj != null)
			{
				UpdateSkinLegacy();
			}
			else if (buildingObj != null)
			{
				UpdateSkinNew();
			}
			RefreshAssistanceCount();
			if (IsRes)
			{
				if (buildingObj != null)
				{
					UpdateMarchNew();
				}
				else
				{
					UpdateMarchLegacy();
				}
			}
		}
		GameEntry.Event.Fire(EventId.DragonBuildInView, pointIndex);
		int num = _battlefieldPoint?.detail?.Role ?? (-1);
		if (num != buildRole && _battlefieldPoint != null && !IsScore)
		{
			buildRole = num;
			GameEntry.Event.Fire(EventId.BattlefieldBuildRoleChanged, pointIndex);
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

	public void UpdateMarchLegacy()
	{
		if (gameObject == null || _battlefieldPoint == null || _battlefieldPoint.detail == null || !IsRes || stateIconParent == null)
		{
			return;
		}
		QuarantinePointInfo detail = _battlefieldPoint.detail;
		if (detail.MarchUid.IsNullOrEmpty())
		{
			stateIconPath = null;
			stateIconParent.gameObject.SetActive(value: false);
			return;
		}
		stateIconParent.gameObject.SetActive(value: true);
		string text = ((GameEntry.Data.Player.Uid == detail.MarchUid) ? "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect" : ((!GameEntry.Lua.CallWithReturn<bool, int, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", detail.Role, worldType)) ? "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_alliance" : "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_other"));
		if (stateIconPath == null || !text.Equals(stateIconPath))
		{
			stateIcon?.LoadSprite(text);
			stateIconPath = text;
		}
	}

	public void UpdateMarchNew()
	{
		if (gameObject == null || _battlefieldPoint == null || _battlefieldPoint.detail == null)
		{
			return;
		}
		QuarantinePointInfo detail = _battlefieldPoint.detail;
		if (detail.MarchUid.IsNullOrEmpty())
		{
			stateIconPath = null;
			buildingObj.SetIcon(null);
			return;
		}
		string text = ((GameEntry.Data.Player.Uid == detail.MarchUid) ? "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect" : ((!GameEntry.Lua.CallWithReturn<bool, int, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", detail.Role, worldType)) ? "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_alliance" : "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_other"));
		if (stateIconPath == null || !text.Equals(stateIconPath))
		{
			buildingObj.SetIcon(text);
			stateIconPath = text;
		}
	}

	private void RefreshAssistanceCount()
	{
		int currentLodLevel = world.CurrentLodLevel;
		int num = 0;
		BattlefieldBuildPointInfo battlefieldBuildPointInfo = null;
		battlefieldBuildPointInfo = world.GetPointInfo(pointIndex) as BattlefieldBuildPointInfo;
		num = battlefieldBuildPointInfo?.assistanceCount ?? 0;
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
				int maxAssistanceCount = battlefieldBuildPointInfo.maxAssistanceCount;
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
