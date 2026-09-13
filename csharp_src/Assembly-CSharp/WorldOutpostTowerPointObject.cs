using System.Collections.Generic;
using Protobuf;
using UnityEngine;

public class WorldOutpostTowerPointObject : WorldPointObject, IWorldLodWatcher
{
	private WorldOutpostTowerPoint ptInfo;

	private SpriteRenderer stateIcon;

	private SuperTextMesh nameText;

	private long bUuid;

	private int buildState;

	private int tmpOwnerServerId;

	private string nLastAllianceId;

	private List<InstanceRequest> instanceEffectList;

	private MissileFire _missileFireScript;

	private WorldAllianceBuildFightMode fightMode;

	public AsyncMono<WorldAssistanceLabelAsync>.Handle AssistanceLabel { get; private set; }

	public AsyncMono<WorldAssistanceHeroAsync>.Handle AssistanceHero { get; private set; }

	public MissileFire MissileFire => _missileFireScript;

	public override int AutoLookAtThreshold => 5;

	public long Uid => (long)pointType * 10000000L + pointIndex;

	public WorldOutpostTowerPointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		world?.RegisterLodWatcher(this);
		CreateOutpostCityObject();
	}

	public void CreateOutpostCityObject()
	{
		PointInfo info = world.GetPointInfo(pointIndex);
		if (info == null)
		{
			return;
		}
		nLastAllianceId = GameEntry.Data.Player.GetAllianceId();
		bUuid = info.uuid;
		ptInfo = info as WorldOutpostTowerPoint;
		if (ptInfo == null)
		{
			return;
		}
		tmpOwnerServerId = ptInfo.tmpOwnerServerId;
		buildState = ptInfo.buildState;
		AddOldObject();
		string text = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", info);
		if (text.IsNullOrEmpty())
		{
			return;
		}
		instance = GameEntry.Resource.InstantiateAsync(text);
		instance.completed += delegate
		{
			ClearOldObject();
			stateIcon = null;
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				bUuid = info.uuid;
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(isVisible);
				nameText = gameObject.transform.Find("Model/CityLabel/NameLabel/NameText").GetComponent<SuperTextMesh>();
				stateIcon = gameObject.transform.Find("ModelGo/stateIcon")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
				InitCrossZoneOutpostCanon();
				SetAutoAdjustLod();
				UpdateGameObject();
				OwnerChanged(tmpOwnerServerId);
				CheckShowTroopDestination();
				Transform transform = gameObject.transform.Find("Model");
				if (transform != null)
				{
					_touchObject = transform.GetComponent<TouchObjectEventTrigger>();
					if (_touchObject != null)
					{
						_touchObject.previewIconPath = "Assets/Main/Sprites/LodIcon/lrb_wujisuofang_icon03.png";
						_touchObject.previewName = GameEntry.Localization.GetString("135118");
						_touchObject.previewType = WorldPreviewType.AllianceCity;
						_touchObject.onPointerClick = base.OnClickPoint;
					}
				}
			}
		};
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			if (string.Equals(GameEntry.Data.Player.GetAllianceId(), nLastAllianceId))
			{
				if (!(pointInfo is WorldOutpostTowerPoint worldOutpostTowerPoint) || worldOutpostTowerPoint.buildState != buildState || worldOutpostTowerPoint.tmpOwnerServerId != tmpOwnerServerId)
				{
					CreateOutpostCityObject();
				}
				else
				{
					CheckBuildStatus(worldOutpostTowerPoint, worldOutpostTowerPoint.towerInfo);
				}
				RefreshAssistanceCount();
				RefreshAssistanceHero();
			}
			else
			{
				CreateOutpostCityObject();
			}
		}
		GameEntry.Event.Fire(EventId.WorldCityBuildObjUpdate, bUuid);
	}

	public override void Destroy()
	{
		world?.UnregisterLodWatcher(this);
		stateIcon = null;
		ClearEffectObject();
		GameEntry.Event.Fire(EventId.WORLD_BUILD_OUT_VIEW, bUuid);
		AssistanceLabel?.Destroy();
		AssistanceLabel = null;
		AssistanceHero?.Destroy();
		AssistanceHero = null;
		base.Destroy();
	}

	protected override void ClearOldObject()
	{
		ClearEffectObject();
		base.ClearOldObject();
	}

	private void ClearEffectObject()
	{
		if (instanceEffectList != null && instanceEffectList.Count > 0)
		{
			instanceEffectList.ForEach(delegate(InstanceRequest m)
			{
				m.Destroy();
			});
			instanceEffectList.Clear();
		}
		if (fightMode != null)
		{
			fightMode.Destroy();
			fightMode = null;
		}
	}

	private void InitCrossZoneOutpostCanon()
	{
		_missileFireScript = gameObject.GetComponentInChildren<MissileFire>(includeInactive: true);
		if (_missileFireScript == null)
		{
			return;
		}
		string tabName = "season_city_s5";
		int num = GameEntry.ConfigCache.GetTemplateData(tabName, ptInfo.CityId, "parent_output").ToInt();
		if (num <= 0)
		{
			return;
		}
		string templateData = GameEntry.ConfigCache.GetTemplateData(tabName, num, "location");
		if (templateData.IsNullOrEmpty())
		{
			return;
		}
		string[] array = templateData.Split(new char[1] { '|' });
		if (array.Length == 2)
		{
			int num2 = int.Parse(array[0]);
			int num3 = int.Parse(array[1]);
			Vector3 worldBasePos = SeasonDataManager.Instance.GetWorldBasePos(serverId);
			worldBasePos.x += (float)num2 * 2f;
			worldBasePos.y = 2f;
			worldBasePos.z += (float)num3 * 2f;
			_missileFireScript.towerAnimTime = 0.1f;
			_missileFireScript.moveTime = 0.5f;
			_missileFireScript.targetPos = worldBasePos;
			LookAtTarget[] componentsInChildren = gameObject.GetComponentsInChildren<LookAtTarget>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].targetPosition = worldBasePos;
			}
		}
	}

	public void OwnerChanged(int theOwnerServerId)
	{
		if (!(nameText != null))
		{
			return;
		}
		if (theOwnerServerId > 0)
		{
			if (GameEntry.Data.Player.GetSourceServerId() == theOwnerServerId)
			{
				nameText.color = new Color32(84, 196, 242, byte.MaxValue);
			}
			else
			{
				nameText.color = new Color32(229, 39, 39, byte.MaxValue);
			}
			nameText.text = $"#{theOwnerServerId}";
		}
		else
		{
			nameText.color = Color.white;
			nameText.text = GameEntry.Localization.GetString("outpost_gun_name");
		}
		nameText.Rebuild();
	}

	private void CheckBuildStatus(WorldOutpostTowerPoint pt, OutpostTowerInfo extraData)
	{
		if (pt != null)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			if (pt.battleStartTime <= serverTime && pt.protectTime <= serverTime && fightMode == null)
			{
				fightMode = new WorldAllianceBuildFightMode(gameObject);
			}
			if (fightMode != null)
			{
				fightMode.UpdateStatus(pt, extraData);
			}
		}
	}

	public override void CheckShowTroopDestination()
	{
		bool flag = isSHowDestination;
		List<WorldMarch> ownerMarches = world.GetOwnerMarches(GameEntry.Data.Player.Uid);
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		bool flag2 = false;
		foreach (WorldMarch item in ownerMarches)
		{
			if (!flag2 && pointInfo != null && pointInfo.uuid == item.targetUuid && item.targetUuid != 0L && (item.status == MarchStatus.BUILD_ALLIANCE_BUILDING || item.status == MarchStatus.COLLECTING || item.status == MarchStatus.COLLECTING_ASSISTANCE))
			{
				flag2 = true;
			}
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
			if (stateIcon != null)
			{
				stateIcon.gameObject.SetActive(value: false);
			}
		}
		if (pointInfo != null && flag2)
		{
			OutpostTowerInfo outpostTowerInfo = OutpostTowerInfo.Parser.ParseFrom(pointInfo.extraInfo);
			if (outpostTowerInfo != null)
			{
				if (outpostTowerInfo.State == 1)
				{
					if (stateIcon != null)
					{
						stateIcon.LoadSprite("Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_dig");
					}
				}
				else if (stateIcon != null)
				{
					stateIcon.LoadSprite("Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect");
				}
			}
		}
		if (stateIcon != null)
		{
			stateIcon.gameObject.SetActive(flag2);
		}
	}

	public void UpdateLod(int lod)
	{
		RefreshAssistanceCount();
		RefreshAssistanceHero();
	}

	private void RefreshAssistanceCount()
	{
		int currentLodLevel = world.CurrentLodLevel;
		int num = 0;
		bool num2 = currentLodLevel >= 1 && currentLodLevel <= 5;
		WorldOutpostTowerPoint worldOutpostTowerPoint = null;
		if (num2)
		{
			worldOutpostTowerPoint = world.GetPointInfo(pointIndex) as WorldOutpostTowerPoint;
			num = worldOutpostTowerPoint?.assistanceCount ?? 0;
		}
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
				int maxAssistanceCount = worldOutpostTowerPoint.maxAssistanceCount;
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

	private void RefreshAssistanceHero()
	{
		int currentLodLevel = world.CurrentLodLevel;
		int num = 0;
		if (currentLodLevel >= 3 && currentLodLevel <= 4)
		{
			num = world?.GetMyAssistanceFirstHero(pointIndex) ?? 0;
		}
		if (num > 0)
		{
			if (AssistanceHero == null)
			{
				AssistanceHero = AsyncMono<WorldAssistanceHeroAsync>.Handle.Load("Assets/Main/Prefabs/MainCity/WorldAssistanceHeroAllianceBuilding.prefab", world?.DynamicObjNode, delegate
				{
					Transform transform = AssistanceHero?.Transform;
					if (transform != null)
					{
						transform.localPosition = base.WorldPosition;
						RefreshAssistanceHero();
					}
				});
			}
			else if (AssistanceHero.MonoInstance != null)
			{
				AssistanceHero.SetActive(active: true);
				AssistanceHero.MonoInstance.SetHeroHead(num);
			}
		}
		else
		{
			AssistanceHero?.SetActive(active: false);
		}
	}
}
