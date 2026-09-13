using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public abstract class WorldPointObject : WorldScene.ISelfMarchUpdateObserver
{
	protected WorldScene world;

	protected WorldIconRendererFacade iconRendererFacade;

	protected int pointIndex;

	protected int serverId;

	protected bool isVisible;

	protected GameObject gameObject;

	protected InstanceRequest instance;

	private InstanceRequest tempInstance;

	private UITemperatureLabel uiTempLabel;

	protected List<InstanceRequest> oldInstances;

	private InstanceRequest _troopDestinationInst;

	private WorldTroopDestinationSignal _troopDestination;

	protected bool isSHowDestination;

	protected TouchObjectEventTrigger _touchObject;

	protected TouchObjectEventTrigger bubbleTouchEvent;

	protected AutoAdjustLod adjuster;

	protected int pointType;

	protected InstanceRequest _desertTileInst;

	protected PlayerType desertPlayerType = PlayerType.PlayerNone;

	protected bool hasAssistance;

	private long desertUuid;

	private bool isFireDesertEffect;

	private bool isFireDesertMine;

	private int desertId;

	private bool isRedDesert;

	private bool isYellowDesert;

	protected int tileSize;

	protected const int DEFAULT_LOOKAT_THRESHOLD = 3;

	public int WorldId { get; private set; } = -1;

	public BattleFieldType PointBattlefieldType { get; private set; }

	public bool InstanceRequested => instance != null;

	public bool TitleRequested => _desertTileInst != null;

	protected virtual AutoAdjustLod AdjustLod => adjuster;

	public ulong id => (ulong)(((long)pointIndex << 32) | (uint)pointType);

	public virtual int AutoLookAtThreshold => -1;

	public virtual AutoAdjustLod AutoAdjustLod => adjuster;

	public Vector3 WorldPosition
	{
		get
		{
			if (world == null)
			{
				return Vector3.zero;
			}
			if (world.WorldSize <= 1000)
			{
				return world.TileIndexToWorld(pointIndex, 0);
			}
			return world.TileIndexToWorld(pointIndex, serverId);
		}
	}

	public WorldPointObject(WorldScene worldScene, int pointIndex, int pType)
	{
		world = worldScene;
		iconRendererFacade = world.IconRendererFacade;
		this.pointIndex = pointIndex;
		isVisible = true;
		pointType = pType;
		desertUuid = 0L;
		oldInstances = new List<InstanceRequest>();
	}

	public virtual void InitByPointInfo(PointInfo pointInfo)
	{
		if (pointInfo != null)
		{
			serverId = pointInfo.serverId;
			WorldId = pointInfo.worldId;
			DCPlayer dCPlayer = GameEntry.Data?.Player;
			if (dCPlayer != null)
			{
				PointBattlefieldType = ((dCPlayer.GetWorldId() == WorldId) ? dCPlayer.GetBattleFieldType() : BattleFieldType.Default);
			}
			else
			{
				PointBattlefieldType = BattleFieldType.Default;
			}
		}
	}

	public int GetServerId()
	{
		return serverId;
	}

	public void SetVisible(bool v)
	{
		isVisible = v;
		if (gameObject != null)
		{
			gameObject.SetActive(v);
		}
	}

	public int GetPointIndex()
	{
		return pointIndex;
	}

	public int GetPointType()
	{
		return pointType;
	}

	public GameObject GetGameObject()
	{
		return gameObject;
	}

	public virtual void CreateGameObject()
	{
		if (SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH)
		{
			GameEntry.Event.Subscribe(EventId.MarchItemUpdateSelf, UpdateSelfMarch);
		}
		else
		{
			WorldScene.AddMarchItemUpdateSelfObserver(this);
		}
		CheckShowDesertTile();
	}

	public void SetClickEvent()
	{
		if (!(gameObject == null))
		{
			_touchObject = gameObject.GetComponent<TouchObjectEventTrigger>();
			if (!(_touchObject == null))
			{
				_touchObject.onPointerClick = OnClickPoint;
			}
		}
	}

	protected virtual void OnClickWorldPointObject()
	{
		GameEntry.Lua.Call("UIUtil.OnClickWorld", pointIndex, 1);
	}

	protected void OnClickPoint()
	{
		int lodLevel = SceneManager.World.GetLodLevel();
		int num = ((AutoLookAtThreshold >= 0) ? AutoLookAtThreshold : 3);
		if (lodLevel > num)
		{
			Vector3 worldPosition = WorldPosition;
			SceneManager.World.AutoLookat(worldPosition, SceneManager.World.InitZoom);
		}
		else
		{
			OnClickWorldPointObject();
		}
	}

	public virtual void UpdateSelfMarch(object o)
	{
		CheckShowTroopDestination();
	}

	public virtual void UpdateGameObject()
	{
		CheckShowDesertTile();
		CheckShowTemperature();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			serverId = pointInfo.serverId;
			if (pointInfo.thermalConductor != null || pointInfo.pointType == WorldPointType.TREASURE)
			{
				GameEntry.Event.Fire(EventId.PointObjectUpdate, pointInfo.uuid);
			}
		}
	}

	protected void CheckShowTemperature()
	{
		PointInfo info = world.GetPointInfo(pointIndex);
		if (info == null || info.thermalConductor == null || gameObject == null || info.worldId > 0)
		{
			return;
		}
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if ((curSkinMeta == null || !curSkinMeta.IsSnowMode()) && !info.thermalConductor.IsPhaseChanging())
		{
			return;
		}
		if (gameObject != null && (info.pointType == WorldPointType.TREASURE || info.pointType == WorldPointType.PlayerBuilding))
		{
			BuildEffect component = gameObject.GetComponent<BuildEffect>();
			if (component != null)
			{
				if (info.IsFrozen())
				{
					component.StopAnim();
				}
				else
				{
					component.PlayAnim();
				}
			}
		}
		if (uiTempLabel != null)
		{
			RefreshTemperatureLabel(info.thermalConductor);
		}
		else
		{
			if (tempInstance != null)
			{
				return;
			}
			tempInstance = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/TemperatureLabel.prefab");
			tempInstance.completed += delegate
			{
				GameObject gameObject = tempInstance.gameObject;
				if (!(this.gameObject == null) && !(gameObject == null))
				{
					Transform transform = this.gameObject.transform.Find("ModelGo");
					if (transform == null)
					{
						transform = this.gameObject.transform.Find("Model");
						if (transform == null)
						{
							Log.Error("大世界建筑" + this.gameObject.name + "上找不到ModelGo or Model！");
							return;
						}
					}
					gameObject.transform.SetParent(transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					uiTempLabel = gameObject.GetComponent<UITemperatureLabel>();
					RefreshTemperatureLabel(info.thermalConductor);
				}
			};
		}
	}

	protected void SetMultiSelectTypeForRadar(int cfgId)
	{
		if (_touchObject != null)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("detect_event", cfgId, "icon");
			string templateData2 = GameEntry.ConfigCache.GetTemplateData("detect_event", cfgId, "icon_custom");
			string templateData3 = GameEntry.ConfigCache.GetTemplateData("detect_event", cfgId, "name");
			string templateData4 = GameEntry.ConfigCache.GetTemplateData("detect_event", cfgId, "name_value");
			_touchObject.previewName = (string.IsNullOrEmpty(templateData4) ? GameEntry.Localization.GetString(templateData3) : GameEntry.Localization.GetString(templateData3, templateData4));
			if (!string.IsNullOrEmpty(templateData2))
			{
				_touchObject.previewIconPath = templateData2;
			}
			else
			{
				_touchObject.previewIconPath = "Assets/Main/Sprites/UI/UIRadarCenter/" + templateData + ".png";
			}
			_touchObject.previewType = WorldPreviewType.Radar;
		}
	}

	private void RefreshTemperatureLabel(ThermalConductor conductor)
	{
		uiTempLabel.SetConductor(conductor, pointType == 6 && HeatSourceDataManager.GetInstance().CurWorldIsSnowSeason);
	}

	public virtual void OnUpdate(float deltaTime)
	{
	}

	public virtual void OnWorldColorDirty(string allianceId)
	{
	}

	public virtual void CheckShowTroopDestination()
	{
	}

	public void ShowTroopDestinationSignal(Vector3 localDestination, EnumDestinationSignalType signalType, int tileSize)
	{
		if (!(gameObject == null))
		{
			if (_troopDestinationInst == null)
			{
				CreateTroopDestinationSignal(localDestination, signalType, tileSize);
				isSHowDestination = true;
			}
			else if (_troopDestinationInst != null && _troopDestination != null)
			{
				_troopDestination.SetDestinationForMarch(localDestination, signalType, tileSize);
				isSHowDestination = true;
			}
		}
	}

	public void HideTroopDestinationSignal()
	{
		if (_troopDestinationInst != null)
		{
			if (_troopDestination != null)
			{
				_troopDestination.HideDestination();
			}
			else
			{
				DestroyTroopDestinationSignal();
			}
		}
		isSHowDestination = false;
	}

	private void CreateTroopDestinationSignal(Vector3 localDestination, EnumDestinationSignalType signalType, int tileSize)
	{
		if (gameObject == null)
		{
			return;
		}
		_troopDestinationInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopDestinationSignal.prefab");
		_troopDestinationInst.completed += delegate
		{
			if (gameObject == null)
			{
				DestroyTroopDestinationSignal();
			}
			else
			{
				_troopDestinationInst.gameObject.transform.SetParent(gameObject.transform);
				_troopDestinationInst.gameObject.transform.localScale = Vector3.one;
				_troopDestination = _troopDestinationInst.gameObject.GetComponent<WorldTroopDestinationSignal>();
				_troopDestination.SetDestinationForMarch(localDestination, signalType, tileSize);
			}
		};
	}

	private void DestroyTroopDestinationSignal()
	{
		if (_troopDestinationInst != null)
		{
			_troopDestinationInst.Destroy();
			_troopDestinationInst = null;
			_troopDestination = null;
		}
		isSHowDestination = false;
	}

	public virtual void OnUpdateIconScale(Quaternion rot, float scale)
	{
	}

	public virtual void Destroy()
	{
		if (AdjustLod != null && pointType == 7)
		{
			WorldScene.RecordWorldObjectState(WorldScene.WorldBIType.WorldResource, AdjustLod.LowLodLevelHasShowed);
		}
		if (SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH)
		{
			GameEntry.Event.Unsubscribe(EventId.MarchItemUpdateSelf, UpdateSelfMarch);
		}
		else
		{
			WorldScene.RemoveMarchItemUpdateSelfObserver(this);
		}
		if (_touchObject != null)
		{
			_touchObject.onPointerClick = null;
			_touchObject.previewName = null;
			_touchObject.previewIconPath = null;
		}
		if (bubbleTouchEvent != null)
		{
			bubbleTouchEvent.onPointerClick = null;
			bubbleTouchEvent = null;
		}
		DestroyTroopDestinationSignal();
		DestroyDesertTile();
		ClearOldObject();
		oldInstances = null;
		if (tempInstance != null)
		{
			tempInstance.Destroy();
			tempInstance = null;
			uiTempLabel?.Dispose();
			uiTempLabel = null;
		}
		if (instance != null)
		{
			instance.Destroy();
			instance = null;
			gameObject = null;
		}
		iconRendererFacade = null;
	}

	protected virtual void ClearOldObject()
	{
		if (tempInstance != null)
		{
			tempInstance.Destroy();
			tempInstance = null;
			uiTempLabel?.Dispose();
			uiTempLabel = null;
		}
		if (oldInstances == null)
		{
			return;
		}
		foreach (InstanceRequest oldInstance in oldInstances)
		{
			oldInstance?.Destroy();
		}
		oldInstances.Clear();
	}

	protected void AddOldObject()
	{
		if (instance != null && oldInstances != null)
		{
			oldInstances.Add(instance);
		}
	}

	public virtual void SetAutoAdjustLod()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo == null)
		{
			return;
		}
		serverId = pointInfo.serverId;
		if (!GameEntry.Data.Player.IsInBattleField())
		{
			adjuster = gameObject.GetComponent<AutoAdjustLod>();
			if (adjuster == null)
			{
				adjuster = gameObject.AddComponent<AutoAdjustLod>();
			}
			switch (pointInfo.pointType)
			{
			case WorldPointType.EXPLORE_POINT:
				adjuster.SetLodType(LodType.Explore);
				break;
			case WorldPointType.SAMPLE_POINT:
			case WorldPointType.SAMPLE_POINT_NEW:
			case WorldPointType.RESCUE_POINT:
			case WorldPointType.DETECT_RETRY_TASK:
			case WorldPointType.DETECT_ALLIANCE_CITY_SCOUT_MONSTER:
				adjuster.SetLodType(LodType.Sample);
				break;
			case WorldPointType.GARBAGE:
				adjuster.SetLodType(LodType.Garbage);
				break;
			case WorldPointType.MONSTER_REWARD:
				adjuster.SetLodType(LodType.MonsterReward);
				break;
			case WorldPointType.DETECT_EVENT_PVE:
			case WorldPointType.TREASURE:
				adjuster.SetLodType(LodType.RadarPve);
				break;
			}
		}
	}

	private void CreateDesertTile(int tileSize, int pos, WorldDesertInfo info)
	{
		if (_desertTileInst != null)
		{
			return;
		}
		desertPlayerType = info.GetPlayerType();
		hasAssistance = info.hasAssistance;
		desertId = info.desertId;
		serverId = info.serverId;
		isRedDesert = info.IsRed();
		isYellowDesert = info.IsYellow();
		int num = info.desertId;
		_desertTileInst = GameEntry.Resource.InstantiateAsync(GetModelPath(num, info));
		_desertTileInst.completed += delegate(InstanceRequest request)
		{
			if (request.gameObject == null)
			{
				DestroyDesertTile();
			}
			else
			{
				request.gameObject.transform.SetParent(world.DynamicObjNode);
				int num2 = tileSize - 1;
				request.gameObject.transform.position = SceneManager.World.TileIndexToWorld(pos, serverId) + new Vector3(-num2, 0f, -num2);
				request.gameObject.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
				request.gameObject.SetActive(value: true);
				string templateData = GameEntry.ConfigCache.GetTemplateData("desert", info.desertId, "desert_level");
				string templateData2 = GameEntry.ConfigCache.GetTemplateData("desert", info.desertId, "desert_type");
				int num3 = 0;
				string text = templateData;
				string text2 = templateData;
				if (!templateData.IsNullOrEmpty())
				{
					num3 = templateData.ToInt();
					if (num3 > 0)
					{
						world.AddOccupyPoints(world.IndexToTilePos(pointIndex), Vector2Int.one);
					}
				}
				if (info.oriDesertId != 0 && info.oriDesertId != info.desertId)
				{
					text2 = GameEntry.ConfigCache.GetTemplateData("desert", info.oriDesertId, "desert_level");
					if (!text2.IsNullOrEmpty())
					{
						text = templateData + "(" + text2 + ")";
					}
				}
				GameObject gameObject = request.gameObject.transform.Find("ModelLevel/bg")?.gameObject;
				SuperTextMesh superTextMesh = request.gameObject.transform.Find("ModelLevel/name")?.GetComponent<SuperTextMesh>();
				Transform transform = request.gameObject.transform.Find("Icon/Sprite");
				Transform transform2 = request.gameObject.transform.Find("Icon/Level");
				GameObject gameObject2 = request.gameObject.transform.Find("Icon/Level/bg")?.gameObject;
				SuperTextMesh superTextMesh2 = request.gameObject.transform.Find("Icon/Level/name")?.GetComponent<SuperTextMesh>();
				GameObject gameObject3 = request.gameObject.transform.Find("ModelGo/assistance")?.gameObject;
				if (transform2 != null)
				{
					transform2.Set_localScale(0.8f, 0.8f, 0.8f);
				}
				if (transform != null)
				{
					transform.Set_localScale(0.2f, 0.2f, 0.2f);
					SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
					if (component != null)
					{
						GameObject gameObject4 = transform.gameObject;
						if (templateData2 == "1")
						{
							if (!gameObject4.activeSelf)
							{
								gameObject4.SetActive(value: true);
							}
							component.LoadSprite("Assets/Main/Sprites/LodIcon/mineral.png");
						}
						else if (templateData2 == "2")
						{
							if (!gameObject4.activeSelf)
							{
								gameObject4.SetActive(value: true);
							}
							component.LoadSprite("Assets/Main/Sprites/LodIcon/UIditu_wood.png");
						}
						else if (gameObject4.activeSelf)
						{
							gameObject4.SetActive(value: false);
						}
					}
				}
				if (gameObject != null)
				{
					if (num3 > 0)
					{
						if (!gameObject.activeSelf)
						{
							gameObject.SetActive(value: true);
						}
					}
					else if (gameObject.activeSelf)
					{
						gameObject.SetActive(value: false);
					}
				}
				if (gameObject2 != null)
				{
					if (num3 > 0)
					{
						if (!gameObject2.activeSelf)
						{
							gameObject2.SetActive(value: true);
						}
					}
					else if (gameObject2.activeSelf)
					{
						gameObject2.SetActive(value: false);
					}
				}
				if (superTextMesh != null)
				{
					if (num3 > 0)
					{
						if (!superTextMesh.gameObject.activeSelf)
						{
							superTextMesh.gameObject.SetActive(value: true);
						}
						superTextMesh.text = text;
					}
					else if (superTextMesh.gameObject.activeSelf)
					{
						superTextMesh.gameObject.SetActive(value: false);
					}
				}
				if (superTextMesh2 != null)
				{
					if (num3 > 0)
					{
						if (!superTextMesh2.gameObject.activeSelf)
						{
							superTextMesh2.gameObject.SetActive(value: true);
						}
						superTextMesh2.text = text;
					}
					else if (superTextMesh2.gameObject.activeSelf)
					{
						superTextMesh2.gameObject.SetActive(value: false);
					}
				}
				if (gameObject3 != null)
				{
					if (info.hasAssistance)
					{
						if (!gameObject3.activeSelf)
						{
							gameObject3.SetActive(value: true);
						}
					}
					else if (gameObject3.activeSelf)
					{
						gameObject3.SetActive(value: false);
					}
				}
				CheckEvent(info);
			}
		};
	}

	private void CheckEvent(WorldDesertInfo info)
	{
		if (desertUuid != 0L && info != null)
		{
			int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
			if (info.giveUpTime > serverTimeSeconds || info.protectEndTime > serverTimeSeconds || desertPlayerType == PlayerType.PlayerSelf)
			{
				isFireDesertEffect = true;
				GameEntry.Event.Fire(EventId.DesertEffectInView, desertUuid);
			}
			if (info.mineId > 0 || info.desertId > 0)
			{
				isFireDesertMine = true;
				GameEntry.Event.Fire(EventId.DesertMineInView, desertUuid);
			}
			GameEntry.Event.Fire(EventId.DesertInView, desertUuid);
		}
	}

	private string GetModelPath(int desertId, WorldDesertInfo info)
	{
		string text = "";
		int key = 0;
		if (WorldScene.ModelPathDic.ContainsKey(desertId))
		{
			Dictionary<int, string> dictionary = WorldScene.ModelPathDic[desertId];
			if (dictionary.ContainsKey(key))
			{
				text = dictionary[key];
			}
		}
		if (text.IsNullOrEmpty())
		{
			text = GameEntry.ConfigCache.GetTemplateData("desert", desertId, "desert_model");
			if (!WorldScene.ModelPathDic.ContainsKey(desertId))
			{
				WorldScene.ModelPathDic[desertId] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[desertId][key] = text;
		}
		if (desertPlayerType == PlayerType.PlayerSelf)
		{
			return "Assets/Main/Prefabs/Building/" + text + "Self.prefab";
		}
		if (desertPlayerType == PlayerType.PlayerAlliance)
		{
			return "Assets/Main/Prefabs/Building/" + text + "Alliance.prefab";
		}
		if (desertPlayerType == PlayerType.PlayerOther)
		{
			if (info.IsYellow())
			{
				return "Assets/Main/Prefabs/Building/" + text + "Yellow.prefab";
			}
			if (info.IsRed())
			{
				return "Assets/Main/Prefabs/Building/" + text + "Other.prefab";
			}
			return "Assets/Main/Prefabs/Building/" + text + "White.prefab";
		}
		return "Assets/Main/Prefabs/Building/" + text + "None.prefab";
	}

	private void UpdateDesertTile(int tileSize, int pos, WorldDesertInfo info)
	{
		PlayerType playerType = info.GetPlayerType();
		bool flag = false;
		if (hasAssistance != info.hasAssistance || desertId != info.desertId)
		{
			flag = true;
			DestroyDesertTile();
			CreateDesertTile(tileSize, pos, info);
		}
		else if (playerType != desertPlayerType)
		{
			flag = true;
			DestroyDesertTile();
			CreateDesertTile(tileSize, pos, info);
		}
		else if (playerType == desertPlayerType && desertPlayerType == PlayerType.PlayerOther)
		{
			bool flag2 = info.IsYellow();
			if (flag2 != isYellowDesert)
			{
				flag = true;
				isYellowDesert = flag2;
				DestroyDesertTile();
				CreateDesertTile(tileSize, pos, info);
			}
			else
			{
				bool flag3 = info.IsRed();
				if (flag3 != isRedDesert)
				{
					flag = true;
					isRedDesert = flag3;
					DestroyDesertTile();
					CreateDesertTile(tileSize, pos, info);
				}
			}
		}
		if (!flag)
		{
			CheckEvent(info);
		}
	}

	private void DestroyDesertTile()
	{
		if (_desertTileInst != null)
		{
			_desertTileInst.Destroy();
			_desertTileInst = null;
		}
		if (desertUuid != 0L)
		{
			if (isFireDesertEffect)
			{
				GameEntry.Event.Fire(EventId.DesertEffectOutView, desertUuid);
				isFireDesertEffect = false;
			}
			if (isFireDesertMine)
			{
				GameEntry.Event.Fire(EventId.DesertMineOutView, desertUuid);
				isFireDesertMine = false;
			}
			GameEntry.Event.Fire(EventId.DesertOutView, desertUuid);
		}
	}

	public void CheckShowDesertTile()
	{
		if (world == null)
		{
			return;
		}
		WorldTileInfo worldTileInfo = world.GetWorldTileInfo(pointIndex);
		if (worldTileInfo == null)
		{
			return;
		}
		WorldDesertInfo worldDesertInfo = worldTileInfo.GetWorldDesertInfo();
		PointInfo pointInfo = worldTileInfo.GetPointInfo();
		serverId = worldTileInfo.serverId;
		if (worldDesertInfo != null && (pointInfo == null || pointInfo.pointType == WorldPointType.Other || worldDesertInfo.GetPlayerType() != PlayerType.PlayerNone))
		{
			desertUuid = worldDesertInfo.uuid;
			if (_desertTileInst == null)
			{
				CreateDesertTile(1, pointIndex, worldDesertInfo);
			}
			else
			{
				UpdateDesertTile(1, pointIndex, worldDesertInfo);
			}
		}
		else if (worldDesertInfo == null && _desertTileInst != null)
		{
			DestroyDesertTile();
		}
	}

	public static InstanceRequest AsyncLoad(string prefabPath, Transform parent, Action<InstanceRequest> completed = null)
	{
		return AsyncLoad(prefabPath, parent, Vector3.zero, Vector3.one, Quaternion.identity, completed);
	}

	public static InstanceRequest AsyncLoad(string prefabPath, Transform parent, Vector3 pos, Vector3 scale, Quaternion rotation, Action<InstanceRequest> completed = null)
	{
		InstanceRequest instance = GameEntry.Resource.InstantiateAsync(prefabPath);
		instance.completed += delegate
		{
			GameObject gameObject = instance.gameObject;
			if (gameObject != null && parent != null && parent.gameObject != null)
			{
				gameObject.transform.SetParent(parent);
				gameObject.SetActive(value: true);
				gameObject.transform.localPosition = pos;
				gameObject.transform.localScale = scale;
				gameObject.transform.localRotation = rotation;
			}
			else if (gameObject != null)
			{
				gameObject.SetActive(value: false);
			}
			if (completed != null)
			{
				completed(instance);
			}
		};
		return instance;
	}

	public virtual void UpdateTileSize(int tileSize)
	{
		this.tileSize = tileSize;
	}

	public virtual void RecordBlockIndex(Dictionary<int, int> set)
	{
		if (tileSize <= 0 || pointType == 0)
		{
			return;
		}
		if (tileSize == 1)
		{
			set[pointIndex] = serverId;
			return;
		}
		int num = pointIndex - 1;
		int num2 = num % 1000;
		int num3 = num / 1000;
		int num4 = tileSize / 2;
		for (int i = -num4; i <= num4; i++)
		{
			for (int j = -num4; j <= num4; j++)
			{
				int num5 = num2 + i;
				int num6 = num3 + j;
				int key = num5 + num6 * 1000 + 1;
				set[key] = serverId;
			}
		}
	}

	public virtual void RemoveBlockIndex(Dictionary<int, int> set)
	{
		if (tileSize <= 0)
		{
			return;
		}
		if (tileSize == 1)
		{
			set.Remove(pointIndex);
			return;
		}
		int num = pointIndex - 1;
		int num2 = num % 1000;
		int num3 = num / 1000;
		int num4 = tileSize / 2;
		for (int i = -num4; i <= num4; i++)
		{
			for (int j = -num4; j <= num4; j++)
			{
				int num5 = num2 + i;
				int num6 = num3 + j;
				int key = num5 + num6 * 1000 + 1;
				set.Remove(key);
			}
		}
	}
}
