using System.Collections.Generic;
using Protobuf;
using UnityEngine;

public class WorldOutpostPointObject : WorldPointObject, IWorldLodWatcher
{
	private WorldOutpostPoint ptInfo;

	private SpriteRenderer stateIcon;

	private Transform nodeBad;

	private Transform nodeModel;

	private long bUuid;

	private int buildState;

	private int nRepairScore;

	private int ownerServerId;

	private int tmpOwnerServerId;

	private string nLastAllianceId;

	private WorldAllianceBuildFightMode fightMode;

	private InstanceRequest theUpgradeModel;

	private InstanceRequest theFixEffect;

	public AsyncMono<WorldAssistanceLabelAsync>.Handle AssistanceLabel { get; private set; }

	public AsyncMono<WorldAssistanceHeroAsync>.Handle AssistanceHero { get; private set; }

	public override int AutoLookAtThreshold => 5;

	public long Uid => (long)pointType * 10000000L + pointIndex;

	public WorldOutpostPointObject(WorldScene worldScene, int pointIndex, int pType)
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
		ptInfo = info as WorldOutpostPoint;
		if (ptInfo == null)
		{
			return;
		}
		ownerServerId = ptInfo.ownerServerId;
		tmpOwnerServerId = ptInfo.tmpOwnerServerId;
		buildState = ptInfo.buildState;
		nRepairScore = ptInfo.cityInfo.RepairScore;
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
				nodeModel = gameObject.transform.Find("Model");
				if (nodeModel != null)
				{
					nodeBad = nodeModel.Find("Qianshaozhan_S5_PO");
					_touchObject = nodeModel.GetComponent<TouchObjectEventTrigger>();
					if (_touchObject != null)
					{
						_touchObject.previewIconPath = "Assets/Main/SeasonRes/S5/Sprites/CommonS5/LodIcon/zyf_S5_wujisuofang_5.png";
						_touchObject.previewName = GameEntry.Localization.GetString("war_zone_outpost_1");
						_touchObject.previewType = WorldPreviewType.AllianceCity;
						_touchObject.onPointerClick = base.OnClickPoint;
					}
				}
				stateIcon = gameObject.transform.Find("ModelGo/stateIcon")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
				SetAutoAdjustLod();
				UpdateGameObject();
				CheckShowTroopDestination();
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
				if (pointInfo is WorldOutpostPoint worldOutpostPoint)
				{
					ptInfo = worldOutpostPoint;
					CheckBuildStatus();
				}
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
		nodeBad = null;
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
		if (fightMode != null)
		{
			fightMode.Destroy();
			fightMode = null;
		}
		if (theUpgradeModel != null)
		{
			theUpgradeModel.Destroy();
			theUpgradeModel = null;
		}
		if (theFixEffect != null)
		{
			theFixEffect.Destroy();
			theFixEffect = null;
		}
	}

	public void SwitchToNormal()
	{
		buildState = 1;
		if (theUpgradeModel != null)
		{
			theUpgradeModel.Destroy();
			theUpgradeModel = null;
		}
		if (theFixEffect != null || !(nodeBad != null) || !(nodeModel != null))
		{
			return;
		}
		string prefabPath = "Assets/Main/SeasonRes/S5/Prefabs/Effect/Build/Qianshaozhan_S5_Fix.prefab";
		theFixEffect = WorldPointObject.AsyncLoad(prefabPath, nodeModel, delegate(InstanceRequest m)
		{
			if (m != null)
			{
				GameObject gameObject = m.gameObject;
				if (gameObject != null)
				{
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					if (nodeBad != null)
					{
						nodeBad.gameObject?.SetActive(value: false);
					}
				}
			}
		});
	}

	public void CheckRepairStatus(int _buildState, int _nRepairScore)
	{
		buildState = _buildState;
		nRepairScore = _nRepairScore;
		if (buildState == 0)
		{
			if (nodeBad != null)
			{
				nodeBad.gameObject?.SetActive(value: true);
			}
			if (nRepairScore <= 0)
			{
				return;
			}
			if (theUpgradeModel != null)
			{
				theUpgradeModel.gameObject?.SetActive(value: true);
			}
			else
			{
				if (!(nodeModel != null))
				{
					return;
				}
				string prefabPath = "Assets/Main/SeasonRes/S5/Prefabs/WorldCity/worldcity_S5_qianshaozhan_fix.prefab";
				theUpgradeModel = WorldPointObject.AsyncLoad(prefabPath, nodeModel, delegate(InstanceRequest m)
				{
					if (m != null)
					{
						GameObject gameObject = m.gameObject;
						if (gameObject != null)
						{
							gameObject.transform.localPosition = new Vector3(1.5f, 0f, -0.8f);
							gameObject.transform.localScale = Vector3.one;
						}
					}
				});
			}
		}
		else
		{
			if (theUpgradeModel != null)
			{
				theUpgradeModel.gameObject?.SetActive(value: false);
			}
			SwitchToNormal();
		}
	}

	private void CheckBuildStatus()
	{
		if (ptInfo == null)
		{
			return;
		}
		if (buildState != ptInfo.buildState && (ptInfo.buildState == 1 || buildState == 1))
		{
			SwitchToNormal();
		}
		else if (ptInfo.buildState == 0)
		{
			CheckRepairStatus(ptInfo.buildState, ptInfo.cityInfo.RepairScore);
		}
		else if (ptInfo.buildState == 1)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			if (ptInfo.battleStartTime <= serverTime && ptInfo.protectTime <= serverTime && fightMode == null)
			{
				fightMode = new WorldAllianceBuildFightMode(gameObject);
			}
			if (fightMode != null)
			{
				fightMode.UpdateStatus(ptInfo, ptInfo.cityInfo);
			}
			if (theUpgradeModel != null)
			{
				theUpgradeModel.Destroy();
				theUpgradeModel = null;
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
			OutpostInfo outpostInfo = OutpostInfo.Parser.ParseFrom(pointInfo.extraInfo);
			if (outpostInfo != null)
			{
				if (outpostInfo.State == 1)
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
	}
}
