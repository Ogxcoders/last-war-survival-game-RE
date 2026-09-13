using System.Collections.Generic;
using Protobuf;
using UnityEngine;

public class WorldAllianceBuildObject : WorldPointObject, IWorldLodWatcher
{
	private WorldAllianceBuilding allianceBuild;

	private SpriteRenderer stateIcon;

	private long bUuid;

	private int nState;

	private int nLastDurability;

	private string nLastAllianceId;

	private AllianceBuildType type;

	private long timeStamp;

	private float bubbleCoutdown;

	private const float BubbleCD = 10f;

	private int StoveCenterState = -1;

	private List<InstanceRequest> instanceEffectList;

	private WorldAllianceBuildS4 guardianTower;

	private WorldAllianceBuildFightMode fightMode;

	private InstanceRequest effectSnowFire;

	public AsyncMono<WorldAssistanceLabelAsync>.Handle AssistanceLabel { get; private set; }

	public AsyncMono<WorldAssistanceHeroAsync>.Handle AssistanceHero { get; private set; }

	public override int AutoLookAtThreshold => 5;

	public long Uid => (long)pointType * 10000000L + pointIndex;

	public WorldAllianceBuildObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		world?.RegisterLodWatcher(this);
		CreateAllianceBuildObject();
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		if (type != AllianceBuildType.SiegeCamp || !(gameObject != null))
		{
			return;
		}
		if (bubbleCoutdown < 0f)
		{
			bubbleCoutdown = 10f;
			if (GameEntry.Timer.GetServerTime() < timeStamp)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.ShowSiegeCampBubble", gameObject.transform.position, 3);
			}
		}
		else
		{
			bubbleCoutdown -= deltaTime;
		}
	}

	public void CreateAllianceBuildObject()
	{
		PointInfo info = world.GetPointInfo(pointIndex);
		if (info == null)
		{
			return;
		}
		nLastAllianceId = GameEntry.Data.Player.GetAllianceId();
		bUuid = info.uuid;
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
			StoveCenterState = -1;
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				allianceBuild = gameObject.GetComponent<WorldAllianceBuilding>();
				bUuid = info.uuid;
				if (allianceBuild != null)
				{
					AllianceBuildingPointInfo allianceBuildingPointInfo = AllianceBuildingPointInfo.Parser.ParseFrom(info.extraInfo);
					WorldAllianceBuilding.Param param = new WorldAllianceBuilding.Param
					{
						buildUuid = bUuid
					};
					if (allianceBuildingPointInfo != null)
					{
						nLastDurability = allianceBuildingPointInfo.Durability;
						nState = allianceBuildingPointInfo.State;
						param.buildId = allianceBuildingPointInfo.BuildId;
						param.PositionId = allianceBuildingPointInfo.PositionId;
						param.point = pointIndex;
						param.tileSize = info.tileSize;
						int num = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", allianceBuildingPointInfo.BuildId, "offter_range").ToInt();
						if (num != 9)
						{
							Transform transform = gameObject.transform.Find("ModelGo/range/VFX_AllianceCenterBlockwhite");
							if (transform != null)
							{
								float num2 = (float)num * 0.3333f;
								transform.transform.localScale = new Vector3(num2, num2, num2);
							}
						}
						if (info is AllianceBuildPointInfo allianceBuildPointInfo)
						{
							param.IsCreate = allianceBuildPointInfo.isCreate;
							allianceBuildPointInfo.offter_range = num;
							CheckBuildStatus(allianceBuildPointInfo, allianceBuildingPointInfo);
							allianceBuildPointInfo.isCreate = false;
						}
						Transform transform2 = gameObject.transform.Find("ModelGo/Normal/VFX_AllianceBlockwhite");
						if (transform2 != null)
						{
							string templateData = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", allianceBuildingPointInfo.BuildId, "model");
							transform2.gameObject.SetActive(templateData.Contains("allianceBuilding_center"));
						}
						if (allianceBuildingPointInfo.AllianceId == nLastAllianceId)
						{
							string templateData2 = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", allianceBuildingPointInfo.BuildId, "type");
							type = (AllianceBuildType)templateData2.ToInt();
							timeStamp = GameEntry.Lua.CallWithReturn<long, string>("CSharpCallLuaInterface.GetFormalDeclareTsByAlliId", allianceBuildingPointInfo.AllianceId);
						}
					}
					param.buildSceneType = WorldAllianceBuilding.AllianceBuildSceneType.World;
					allianceBuild.CSInit(param);
					allianceBuild.UpdateCityLabel(info.uuid);
				}
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(isVisible);
				stateIcon = gameObject.transform.Find("ModelGo/stateIcon")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
				SetAutoAdjustLod();
				UpdateGameObject();
				CheckShowTroopDestination();
				SetClickEvent();
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
				AllianceBuildingPointInfo allianceBuildingPointInfo = AllianceBuildingPointInfo.Parser.ParseFrom(pointInfo.extraInfo);
				if (allianceBuildingPointInfo != null && (allianceBuildingPointInfo.State != nState || (allianceBuildingPointInfo.Durability == 0 && nLastDurability != 0) || (allianceBuildingPointInfo.Durability != 0 && nLastDurability == 0)))
				{
					CreateAllianceBuildObject();
					RefreshAssistanceCount();
					RefreshAssistanceHero();
				}
				else if (gameObject != null)
				{
					CheckBuildStatus(pointInfo as AllianceBuildPointInfo, allianceBuildingPointInfo);
					RefreshAssistanceCount();
					RefreshAssistanceHero();
				}
			}
			else
			{
				CreateAllianceBuildObject();
			}
		}
		GameEntry.Event.Fire(EventId.WorldCityBuildObjUpdate, bUuid);
	}

	public override void Destroy()
	{
		world?.UnregisterLodWatcher(this);
		timeStamp = 0L;
		stateIcon = null;
		type = AllianceBuildType.None;
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
		if (allianceBuild != null)
		{
			allianceBuild.CSUninit();
		}
		if (effectSnowFire != null)
		{
			effectSnowFire.Destroy();
			effectSnowFire = null;
		}
		if (fightMode != null)
		{
			fightMode.Destroy();
			fightMode = null;
		}
		if (guardianTower != null)
		{
			guardianTower.Destroy();
			guardianTower = null;
		}
	}

	private void CheckBuildStatus(AllianceBuildPointInfo pt, AllianceBuildingPointInfo extraData)
	{
		if (pt != null)
		{
			if (pt.fightState == 1 && fightMode == null)
			{
				fightMode = new WorldAllianceBuildFightMode(gameObject);
			}
			if (fightMode != null)
			{
				fightMode.UpdateStatus(pt, extraData);
			}
		}
		if (pt != null && pt.buildId == 200000)
		{
			if (pt.furnaceInfo == null || pt.furnaceInfo.State == StoveCenterState)
			{
				return;
			}
			StoveCenterState = pt.furnaceInfo.State;
			if (effectSnowFire != null)
			{
				effectSnowFire.Destroy();
				effectSnowFire = null;
			}
			Transform S2saiji_lianmeng_xue = gameObject.transform.Find("ModelGo/Normal/S2saiji_lianmeng_xue");
			if (StoveCenterState != 0)
			{
				string prefabPath = "Assets/_Art_LastWar/Models/Environment/Build/S2saiji_lianmeng/prefab/S2saiji_lianmeng2.prefab";
				if (StoveCenterState != 1 && StoveCenterState == 2)
				{
					prefabPath = "Assets/_Art_LastWar/Models/Environment/Build/S2saiji_lianmeng/prefab/S2saiji_lianmeng1.prefab";
				}
				effectSnowFire = WorldPointObject.AsyncLoad(prefabPath, gameObject.transform.Find("ModelGo/Normal"), delegate
				{
					if (gameObject != null && S2saiji_lianmeng_xue != null)
					{
						S2saiji_lianmeng_xue.gameObject.SetActive(value: false);
					}
				});
			}
			else if (S2saiji_lianmeng_xue != null)
			{
				S2saiji_lianmeng_xue.gameObject.SetActive(value: true);
			}
		}
		else if (pt != null && (pt.buildId == 400000 || pt.buildId == 402000 || pt.buildId == 403000 || pt.buildId == 404000))
		{
			if (guardianTower == null)
			{
				guardianTower = gameObject.GetComponent<WorldAllianceBuildS4>();
			}
			if (guardianTower != null)
			{
				guardianTower.UpdateStatus(world, pt, extraData);
			}
		}
	}

	public WorldAllianceBuildS4 GetGuardianTower()
	{
		return guardianTower;
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
			AllianceBuildingPointInfo allianceBuildingPointInfo = AllianceBuildingPointInfo.Parser.ParseFrom(pointInfo.extraInfo);
			if (allianceBuildingPointInfo != null)
			{
				if (allianceBuildingPointInfo.State == 1)
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
		AllianceBuildPointInfo allianceBuildPointInfo = null;
		if (num2)
		{
			allianceBuildPointInfo = world.GetPointInfo(pointIndex) as AllianceBuildPointInfo;
			num = allianceBuildPointInfo?.assistanceCount ?? 0;
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
				int maxAssistanceCount = allianceBuildPointInfo.maxAssistanceCount;
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
