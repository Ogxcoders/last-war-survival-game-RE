using System;
using UnityEngine;

public class WorldRuinDestroyBuildingObject : WorldPointObject
{
	private const string IconSelf = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_lv.png";

	private const string IconDefault = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_bai.png";

	private const string IconAlly = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_lan.png";

	private const string IconAllianceEnemy = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_hong.png";

	private const string IconZoneEnemy = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_fen.png";

	private const string IconSeasonEnemy = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_fen.png";

	private const string IconSeasonCamp = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_qianlan.png";

	private const string IconSeasonAssist = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_shenlan.png";

	private Transform _cityLabelRoot;

	private UIWorldLabel[] cityLabels;

	private SuperTextMesh _timeText;

	private SpriteRenderer _icon;

	private SpriteRenderer _iconMap;

	private Transform _rootBlue;

	private Transform _rootRed;

	private long _endTime;

	private static float s_timeInterval = 0.5f;

	private float _timeInterval;

	public WorldRuinDestroyBuildingObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		_endTime = 0L;
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		CreateObject();
		GameEntry.Event.Subscribe(EventId.MapGridRenderChange, MapGridRenderChange);
	}

	public override void Destroy()
	{
		GameEntry.Event.Unsubscribe(EventId.MapGridRenderChange, MapGridRenderChange);
		base.Destroy();
	}

	public override void OnUpdate(float deltaTime)
	{
		_timeInterval += deltaTime;
		if (!(_timeInterval < s_timeInterval))
		{
			_timeInterval = 0f;
			UpdateTimeText();
		}
	}

	public void CreateObject()
	{
		string prefabPath = "Assets/Main/Prefabs/Building/BuildRuinDestroyBuilding.prefab";
		instance = GameEntry.Resource.InstantiateAsync(prefabPath);
		instance.completed += AsyncCompleteCallBack;
	}

	public virtual void AsyncCompleteCallBack(InstanceRequest instance)
	{
		WorldRuinDestroyBuildingPointInfo worldRuinDestroyBuildingPointInfo = world.GetPointInfo(pointIndex) as WorldRuinDestroyBuildingPointInfo;
		gameObject = instance.gameObject;
		if (!(gameObject == null) && worldRuinDestroyBuildingPointInfo != null)
		{
			gameObject.name = "WorldRuinDestroyBuilding_" + pointIndex;
			gameObject.transform.SetParent(world.DynamicObjNode);
			gameObject.transform.position = base.WorldPosition;
			gameObject.SetActive(isVisible);
			gameObject.transform.localScale = Vector3.one;
			SetAutoAdjustLod();
			SetClickEvent();
			_cityLabelRoot = gameObject.transform.Find("ModelGo/CityLabel");
			cityLabels = gameObject.GetComponentsInChildren<UIWorldLabel>(includeInactive: true);
			UIWorldLabel[] array = cityLabels;
			foreach (UIWorldLabel obj in array)
			{
				obj.ShowFlag(s: false);
				obj.SetName($"#{worldRuinDestroyBuildingPointInfo.serverId}[{worldRuinDestroyBuildingPointInfo.alAbbr}]", GameDefines.CityLabelColorType.White);
			}
			_timeText = _cityLabelRoot.Find("Time/TimeText").GetComponent<SuperTextMesh>();
			_endTime = worldRuinDestroyBuildingPointInfo.destroyEndTime;
			UpdateTimeText();
			_icon = gameObject.transform.Find("Icon/Sprite").GetComponent<SpriteRenderer>();
			_iconMap = gameObject.transform.Find("ModelGo/Normal/Sprite").GetComponent<SpriteRenderer>();
			_rootBlue = gameObject.transform.Find("ModelGo/Normal/Blue");
			_rootRed = gameObject.transform.Find("ModelGo/Normal/Red");
			UpdateColor(worldRuinDestroyBuildingPointInfo);
			MapGridRenderChange(world.MapGridRenderer.IsShowing);
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		WorldRuinDestroyBuildingPointInfo info = world.GetPointInfo(pointIndex) as WorldRuinDestroyBuildingPointInfo;
		UpdateTimeText();
		UpdateColor(info);
	}

	private void UpdateTimeText()
	{
		if (_endTime > 0 && _timeText != null && _timeText.gameObject.activeInHierarchy)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			long num = _endTime - serverTime;
			if (num <= 0)
			{
				_endTime = 0L;
				_timeText.text = "";
			}
			else
			{
				int num2 = (int)Math.Ceiling((double)num * 1.0 / 1000.0);
				_timeText.text = GameEntry.Timer.SecondToFmtString(num2);
			}
		}
	}

	private void UpdateColor(WorldRuinDestroyBuildingPointInfo info)
	{
		if (info == null)
		{
			return;
		}
		string spritePath;
		MainBuildOrder mainBuildOrder;
		switch (info.GetPlayerType())
		{
		case PlayerType.PlayerSelf:
			spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_lv.png";
			mainBuildOrder = MainBuildOrder.Self;
			break;
		case PlayerType.PlayerAlliance:
			spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_lan.png";
			mainBuildOrder = MainBuildOrder.Ally;
			break;
		case PlayerType.PlayerAllianceLeader:
			spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_lan.png";
			mainBuildOrder = MainBuildOrder.Leader;
			break;
		case PlayerType.PlayerOther:
			switch (world.IsMyEnemy(info.srcServerId, info.allianceId))
			{
			case PlayerType.PlayerAllianceEnemy:
				spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_hong.png";
				mainBuildOrder = MainBuildOrder.Enemy;
				break;
			case PlayerType.PlayerZoneEnemy:
				spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_fen.png";
				mainBuildOrder = MainBuildOrder.Enemy;
				break;
			case PlayerType.PlayerSeasonEnemy:
				spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_fen.png";
				mainBuildOrder = MainBuildOrder.Enemy;
				break;
			case PlayerType.PlayerSeasonCamp:
				spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_qianlan.png";
				mainBuildOrder = MainBuildOrder.Enemy;
				break;
			case PlayerType.PlayerSeasonAssist:
				spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_shenlan.png";
				mainBuildOrder = MainBuildOrder.Enemy;
				break;
			default:
				spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_bai.png";
				mainBuildOrder = MainBuildOrder.Other;
				break;
			}
			break;
		default:
			spritePath = "Assets/Main/Sprites/LodIcon/mjc_daditu_lianmenglingdi_bai.png";
			mainBuildOrder = MainBuildOrder.Other;
			break;
		}
		if (_icon != null)
		{
			_icon.LoadSprite(spritePath);
			_icon.sortingOrder = (int)mainBuildOrder;
		}
		if (_iconMap != null)
		{
			_iconMap.LoadSprite(spritePath);
			_iconMap.sortingOrder = (int)mainBuildOrder;
		}
		bool flag = mainBuildOrder == MainBuildOrder.Enemy || mainBuildOrder == MainBuildOrder.Other;
		if (_rootRed != null)
		{
			_rootRed.gameObject.SetActive(flag);
		}
		if (_rootBlue != null)
		{
			_rootBlue.gameObject.SetActive(!flag);
		}
	}

	private void MapGridRenderChange(object isShow)
	{
		bool flag = (bool)isShow;
		if (_cityLabelRoot != null)
		{
			_cityLabelRoot.gameObject.SetActive(!flag);
		}
	}
}
