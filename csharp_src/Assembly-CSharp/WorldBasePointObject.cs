using System.Collections.Generic;
using GameKit.Base;
using UnityEngine;

public class WorldBasePointObject : WorldPointObject, IWorldLodWatcher
{
	public bool needFireOutView;

	private string prefabPath;

	protected InstanceRequest instanceEffect;

	private InstanceRequest virusEffectReq;

	private bool tradeStationShopOpen;

	private bool subscribeAllianceCityVirusRefresh;

	private const string EFF_VIRUS_WORLD = "Assets/Main/Prefabs/Season1/Eff_virus_world.prefab";

	public AsyncMono<WorldAssistanceHeroAsync>.Handle AssistanceHero { get; private set; }

	public override int AutoLookAtThreshold => 5;

	public long Uid => (long)pointType * 10000000L + pointIndex;

	public WorldBasePointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		if (pointType == 1002)
		{
			return;
		}
		if (pointType == 35)
		{
			SetTradeStationState();
		}
		PointInfo info = world.GetPointInfo(pointIndex);
		if (info == null || pointIndex != info.mainIndex)
		{
			return;
		}
		world?.RegisterLodWatcher(this);
		AddOldObject();
		if (instance != null)
		{
			return;
		}
		string str = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", info);
		if (!str.IsNullOrEmpty())
		{
			prefabPath = str;
			instance = world.InstantiateAsyncDynamicObj(str);
			instance.completed += delegate
			{
				gameObject = instance.gameObject;
				if (gameObject != null)
				{
					OnGameObjectCreated(info);
				}
			};
		}
		if (info is SurprisePointInfo)
		{
			GameEntry.Event.Fire(EventId.OnSurpriseBuildingShow, pointIndex);
		}
	}

	private void OnGameObjectCreated(PointInfo info)
	{
		gameObject.transform.SetParent(world.DynamicObjNode);
		gameObject.transform.position = base.WorldPosition;
		gameObject.SetActive(isVisible);
		SetAutoAdjustLod();
		UpdateGameObject();
		CheckShowTroopDestination();
		Transform transform = SetAllianceCityClickEvent();
		if (pointType != 11 && pointType != 25 && pointType != 35 && pointType != 41)
		{
			return;
		}
		string tabName = "lw_worldcity";
		bool flag = false;
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null)
		{
			flag = !curSkinMeta.IsNotSeason();
			if (!string.IsNullOrEmpty(curSkinMeta.world_city_table_name))
			{
				tabName = curSkinMeta.world_city_table_name;
			}
		}
		Transform transform2 = gameObject.transform.Find("Model/BuildGirdSeason");
		if (transform2 != null)
		{
			transform2.gameObject.SetActive(flag && curSkinMeta.IsDesertMode());
		}
		Transform transform3 = gameObject.transform.Find("Model/BlackArea");
		if (transform3 != null)
		{
			transform3.gameObject.SetActive(value: false);
		}
		if (_touchObject != null)
		{
			_touchObject.previewIconPath = "Assets/Main/Sprites/LodIcon/cfm_daditu_chengshi_01.png";
		}
		if (info is AllyCityPointInfo allyCityPointInfo && _touchObject != null)
		{
			int cityId = allyCityPointInfo.CityId;
			string templateData = GameEntry.ConfigCache.GetTemplateData(tabName, cityId, "city_field");
			int result = 0;
			if (transform3 != null && !templateData.IsNullOrEmpty() && int.TryParse(templateData, out result))
			{
				allyCityPointInfo.CityField = result;
			}
			else
			{
				allyCityPointInfo.CityField = 12;
			}
			string templateData2 = GameEntry.ConfigCache.GetTemplateData(tabName, cityId, "level");
			string templateData3 = GameEntry.ConfigCache.GetTemplateData(tabName, cityId, "name");
			WorldAllianceCityType worldAllianceCityType = (WorldAllianceCityType)GameEntry.ConfigCache.GetTemplateData(tabName, cityId, "type").ToInt();
			string templateData4 = GameEntry.ConfigCache.GetTemplateData(tabName, cityId, "size");
			if (!templateData2.IsNullOrEmpty())
			{
				allyCityPointInfo.CityLevel = templateData2.ToInt();
			}
			if (_touchObject != null)
			{
				if (worldAllianceCityType == WorldAllianceCityType.Canon || worldAllianceCityType == WorldAllianceCityType.MissileFactory)
				{
					_touchObject.previewName = GameEntry.Localization.GetString(templateData3);
				}
				else
				{
					_touchObject.previewName = GameEntry.Localization.GetString("140205", templateData2, GameEntry.Localization.GetString(templateData3));
				}
				_touchObject.previewType = WorldPreviewType.AllianceCity;
			}
			if (transform != null && !allyCityPointInfo.HasOwner() && curSkinMeta != null && !curSkinMeta.IsNotSeason() && !curSkinMeta.IsDesertMode() && worldAllianceCityType == WorldAllianceCityType.Stronghold)
			{
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				string templateData5 = GameEntry.ConfigCache.GetTemplateData(tabName, cityId, "guard_monsters");
				if (!templateData5.IsNullOrEmpty())
				{
					string[] array = templateData5.Split("|".ToCharArray());
					if (array != null && array.Length != 0)
					{
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string[] array3 = array2[i].Split(";".ToCharArray());
							if (array3 != null && array3.Length == 2)
							{
								int key = int.Parse(array3[0]);
								if (!dictionary.ContainsKey(key))
								{
									dictionary.Add(key, 1);
								}
							}
						}
					}
				}
				WorldPointCityStronghold orAddComponent = transform.GetOrAddComponent<WorldPointCityStronghold>();
				if (orAddComponent != null)
				{
					orAddComponent.DoInit(cityId, pointIndex, serverId, int.Parse(templateData4), dictionary);
				}
			}
		}
		needFireOutView = true;
		GameEntry.Event.Fire(EventId.CityDomeShow, pointIndex);
	}

	private Transform SetAllianceCityClickEvent()
	{
		if (gameObject == null)
		{
			return null;
		}
		Transform transform = gameObject.transform.Find("Model");
		if ((pointType == 11 || pointType == 25 || pointType == 35 || pointType == 41) && transform != null)
		{
			_touchObject = transform.GetComponent<TouchObjectEventTrigger>();
		}
		else
		{
			_touchObject = gameObject.GetComponent<TouchObjectEventTrigger>();
		}
		if (_touchObject == null)
		{
			return transform;
		}
		_touchObject.onPointerClick = base.OnClickPoint;
		return transform;
	}

	public override void Destroy()
	{
		if (subscribeAllianceCityVirusRefresh)
		{
			GameEntry.Event.Unsubscribe(EventId.AllianceCityVirusRefresh, OnRefreshVirus);
			subscribeAllianceCityVirusRefresh = false;
		}
		world?.UnregisterLodWatcher(this);
		AssistanceHero?.Destroy();
		AssistanceHero = null;
		virusEffectReq?.Destroy();
		virusEffectReq = null;
		RemoveFire();
		RemoveBloodQueenGunnerAttackEffect();
		if (gameObject != null)
		{
			WorldPointCityStronghold componentInChildren = gameObject.GetComponentInChildren<WorldPointCityStronghold>();
			if (componentInChildren != null)
			{
				componentInChildren.UnInit();
			}
		}
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (needFireOutView)
		{
			GameEntry.Event.Fire(EventId.WORLD_BUILD_OUT_VIEW, pointIndex);
			pointInfo = world.GetPointInfo(pointIndex);
			if (pointInfo != null && pointInfo.thermalConductor != null)
			{
				GameEntry.Event.Fire(EventId.IcePointObjectOut, pointInfo.uuid);
			}
			needFireOutView = false;
		}
		if (instanceEffect != null)
		{
			instanceEffect.Destroy();
			instanceEffect = null;
		}
		if (pointInfo != null && pointIndex == pointInfo.mainIndex && pointInfo is SurprisePointInfo)
		{
			GameEntry.Event.Fire(EventId.OnSurpriseBuildingDestroy, pointIndex);
		}
		base.Destroy();
	}

	private void SetTradeStationState()
	{
		tradeStationShopOpen = false;
		if (world.GetPointInfo(pointIndex) is AllyCityPointInfo { WorldTradeInfo: not null } allyCityPointInfo && string.IsNullOrEmpty(allyCityPointInfo.WorldTradeInfo.Uid) && allyCityPointInfo.WorldTradeInfo.ShopState == 1)
		{
			tradeStationShopOpen = true;
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo == null || (pointInfo.pointType != WorldPointType.WORLD_ALLIANCE_CITY && pointInfo.pointType != WorldPointType.WORLD_CITY_STRONGHOLD && pointInfo.pointType != WorldPointType.WORLD_CITY_TRADE && pointInfo.pointType != WorldPointType.GOLD_TREE))
		{
			return;
		}
		RefreshAssistanceHero();
		string text = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", pointInfo);
		if (prefabPath != text)
		{
			RemoveFire();
			RemoveBloodQueenGunnerAttackEffect();
			base.Destroy();
			CreateGameObject();
			if (pointInfo.pointType == WorldPointType.WORLD_CITY_TRADE)
			{
				if (!tradeStationShopOpen)
				{
					if (pointInfo is AllyCityPointInfo { WorldTradeInfo: not null } allyCityPointInfo && !string.IsNullOrEmpty(allyCityPointInfo.WorldTradeInfo.Uid) && allyCityPointInfo.WorldTradeInfo.ShopState == 1)
					{
						tradeStationShopOpen = true;
						if (instanceEffect == null)
						{
							string text2 = "Assets/_Art_LastWar/Effect/Prefab/Common/Eff_Common_dituqingli_01.prefab";
							instanceEffect = GameEntry.Resource.InstantiateAsync(text2);
							instanceEffect.completed += delegate
							{
								GameObject gameObject = instanceEffect.gameObject;
								if (gameObject != null && world != null && world.DynamicObjNode != null)
								{
									gameObject.transform.SetParent(world.DynamicObjNode);
									gameObject.transform.position = base.WorldPosition;
									gameObject.SetActive(value: true);
								}
							};
						}
						else if (instanceEffect.isDone && instanceEffect.gameObject != null)
						{
							instanceEffect.gameObject.SetActive(value: false);
							instanceEffect.gameObject.SetActive(value: true);
						}
					}
				}
				else
				{
					SetTradeStationState();
				}
			}
			else
			{
				bool flag = false;
				SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
				if (curSkinMeta != null)
				{
					flag = !curSkinMeta.IsNotSeason();
				}
				if (flag && instanceEffect == null)
				{
					string text3 = "Assets/Main/Prefabs/World/Saiji/Eff_saiji_dsj_shengshi_zhanling.prefab";
					instanceEffect = GameEntry.Resource.InstantiateAsync(text3);
					instanceEffect.completed += delegate
					{
						GameObject gameObject = instanceEffect.gameObject;
						if (gameObject != null && world != null && world.DynamicObjNode != null)
						{
							gameObject.transform.SetParent(world.DynamicObjNode);
							gameObject.transform.position = base.WorldPosition;
							gameObject.SetActive(value: true);
						}
					};
				}
			}
		}
		if (pointInfo is AllyCityPointInfo allyCityPointInfo2 && gameObject != null && pointInfo.pointType == WorldPointType.WORLD_ALLIANCE_CITY && SceneManager.World.GetLodLevel() <= 2)
		{
			if (!(gameObject == null))
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.AllianceCityRefreshFire", allyCityPointInfo2.CityId, gameObject.transform);
			}
			if (world.GetAllianceCitTypeByItemId(allyCityPointInfo2.CityId) == 8)
			{
				Transform transform = gameObject.transform.Find("Model/dawn/Eff_MountainDawn");
				if (transform != null)
				{
					transform.gameObject.SetActive(world.GetBloodyNightState() == BloodyNightState.None);
				}
			}
		}
		RefreshVirusEffect(pointInfo);
	}

	private void OnRefreshVirus(object pointId)
	{
		if (pointIndex == (int)pointId)
		{
			PointInfo pointInfo = world.GetPointInfo(pointIndex);
			if (pointInfo != null)
			{
				RefreshVirusEffect(pointInfo);
			}
		}
	}

	private void RefreshVirusEffect(PointInfo info)
	{
		if (gameObject == null || world == null || world.GetCurSeasonType() != SeasonType.CityStronghold)
		{
			return;
		}
		if (!subscribeAllianceCityVirusRefresh)
		{
			subscribeAllianceCityVirusRefresh = true;
			GameEntry.Event.Subscribe(EventId.AllianceCityVirusRefresh, OnRefreshVirus);
		}
		if (world.IsInSimpleMode())
		{
			if (virusEffectReq != null)
			{
				virusEffectReq.Destroy();
				virusEffectReq = null;
			}
		}
		else if (info != null && info.GetCityVirusLayer() > 0)
		{
			if (virusEffectReq != null)
			{
				return;
			}
			virusEffectReq = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Season1/Eff_virus_world.prefab");
			if (virusEffectReq == null)
			{
				return;
			}
			virusEffectReq.completed += delegate
			{
				GameObject gameObject = virusEffectReq.gameObject;
				if (!(gameObject == null) && !(base.gameObject == null))
				{
					gameObject.transform.SetParent(base.gameObject.transform.Find("Model"));
					gameObject.transform.localPosition = Vector3.zero;
				}
			};
		}
		else if (virusEffectReq != null)
		{
			virusEffectReq.Destroy();
			virusEffectReq = null;
		}
	}

	public void RemoveFire()
	{
		if (gameObject != null && world.GetPointInfo(pointIndex) is AllyCityPointInfo allyCityPointInfo)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.AllianceCityRemoveFire", allyCityPointInfo.CityId);
		}
	}

	public void ShowBloodQueenGunnerAttackEffect(int cityId)
	{
		if (gameObject != null && world.GetPointInfo(pointIndex) is AllyCityPointInfo allyCityPointInfo && cityId == allyCityPointInfo.CityId)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.ShowBloodQueenGunnerAttackEffect", allyCityPointInfo.CityId, gameObject.transform);
		}
	}

	public void RemoveBloodQueenGunnerAttackEffect()
	{
		if (gameObject != null && world.GetPointInfo(pointIndex) is AllyCityPointInfo allyCityPointInfo)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveBloodQueenGunnerAttackEffect", allyCityPointInfo.CityId);
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

	public override void UpdateTileSize(int tileSize)
	{
		if (pointType == 11)
		{
			if (GameEntry.Data.Player.IsPlayerInSeasonOrHalt())
			{
				base.UpdateTileSize(tileSize);
				return;
			}
			int num = 0;
			num = ((!(world.GetPointInfo(pointIndex) is AllyCityPointInfo allyCityPointInfo)) ? 1 : world.GetAllianceCitTypeByItemId(allyCityPointInfo.CityId));
			if (num != 2 && num != 6)
			{
				int num2 = 3;
				int num3 = tileSize / 2 + num2;
				base.UpdateTileSize(num3 * 2);
			}
			else
			{
				base.UpdateTileSize(tileSize);
			}
		}
		else
		{
			base.UpdateTileSize(tileSize);
		}
	}

	public void UpdateLod(int lod)
	{
		RefreshAssistanceHero();
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
				AssistanceHero = AsyncMono<WorldAssistanceHeroAsync>.Handle.Load("Assets/Main/Prefabs/MainCity/WorldAssistanceHeroAllianceTrade.prefab", world?.DynamicObjNode, delegate
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
