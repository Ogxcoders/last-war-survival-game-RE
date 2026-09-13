using System;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using UnityEngine;

public class SiegeTreasureObject : WorldDetectEventItemObject
{
	protected string[] allianceIds;

	public SpriteRenderer rewardSpriteRenderer;

	public GameObject normalTypeModel;

	public GameObject rewardTypeModel;

	public SuperTextMesh nameTxt;

	public GameObject rewardBubbleContent;

	public SuperTextMesh rewardBubbleTxt;

	public List<GameObject> rewardModelChildList;

	public TouchObjectEventTrigger rewardBoxTouchEvent;

	public SpriteRenderer rewardBgSpriteRenderer;

	protected WorldTreasureType type = WorldTreasureType.RadarTreasure;

	public static Color nameColorBlue = new Color(0.329f, 0.768f, 0.949f);

	public bool haveSetDataCompleteByLocalData;

	private SuperTextMesh _disappearTimeText;

	private long _lastCheckTime;

	private bool _isShowDisappearTime;

	private long _treasureDisappearTime;

	private string killerId;

	private const string FLY_EFFECT_PATH = "Assets/_Art_LastWar/Effect/Prefab/Prefab01/Eff_s_jiluofu_baoxiang_fly.prefab";

	private const string BOOM_EFFECT_PATH = "Assets/_Art_LastWar/Effect/Prefab/Prefab01/Eff_s_jiluofu_baoxiang.prefab";

	public SiegeTreasureObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		haveSetDataCompleteByLocalData = false;
		if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
		{
			allianceIds = treasurePointInfo.allianceId.Split(new char[1] { ';' });
			type = treasurePointInfo.type;
			killerId = treasurePointInfo.killerId;
		}
	}

	public override void SetAutoAdjustLod()
	{
		if (world.GetPointInfo(pointIndex) != null)
		{
			adjuster = gameObject.GetComponent<AutoAdjustLod>();
			if (adjuster == null)
			{
				adjuster = gameObject.AddComponent<AutoAdjustLod>();
			}
			adjuster.SetLodType(LodType.SiegeTreasure);
		}
	}

	protected virtual bool CheckIsSameAlliance()
	{
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		if (string.IsNullOrEmpty(allianceId))
		{
			return false;
		}
		string[] array = allianceIds;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == allianceId)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual string GetConfigTableName()
	{
		return "world_treasure";
	}

	public override void Destroy()
	{
		if (killerId == GameEntry.Data.Player.Uid)
		{
			Log.Info($"Destroy with killerId {pointIndex} ");
		}
		GameEntry.Event.Unsubscribe(EventId.WorldTreasureReachDailyLimit, DoWhenReachDailyLimit);
		rewardSpriteRenderer = null;
		allianceIds = null;
		normalTypeModel = null;
		rewardTypeModel = null;
		nameTxt = null;
		rewardBubbleContent = null;
		rewardBubbleTxt = null;
		rewardModelChildList = null;
		if (rewardBoxTouchEvent != null)
		{
			rewardBoxTouchEvent.onPointerClick = null;
			rewardBoxTouchEvent = null;
		}
		rewardBgSpriteRenderer = null;
		base.Destroy();
	}

	protected override bool NeedShowDetectEventIcon()
	{
		bool result = false;
		if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
		{
			result = CheckIsSameAlliance() && !treasurePointInfo.complete;
		}
		return result;
	}

	protected override bool NeedShowTime()
	{
		bool result = false;
		if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
		{
			result = CheckIsSameAlliance() && !treasurePointInfo.complete;
		}
		return result;
	}

	protected override long GetPointUid()
	{
		if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
		{
			return treasurePointInfo.uuid;
		}
		return 0L;
	}

	protected override string GetEventId()
	{
		if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
		{
			return treasurePointInfo.eventId;
		}
		return "";
	}

	protected override string GetModePath()
	{
		string text = "";
		int key = eventId.ToInt();
		int key2 = 1001;
		if (WorldScene.ModelPathDic.ContainsKey(key))
		{
			Dictionary<int, string> dictionary = WorldScene.ModelPathDic[key];
			if (dictionary.ContainsKey(key2))
			{
				text = dictionary[key2];
			}
		}
		if (text.IsNullOrEmpty())
		{
			text = GameEntry.ConfigCache.GetTemplateData("world_treasure", key, "models");
			if (!WorldScene.ModelPathDic.ContainsKey(key))
			{
				WorldScene.ModelPathDic[key] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[key][key2] = text;
		}
		return "Assets/Main/Prefabs/Garbage/" + text + ".prefab";
	}

	public override void AsyncCompleteCallBack(InstanceRequest instance)
	{
		ClearOldObject();
		gameObject = instance.gameObject;
		if (gameObject == null)
		{
			return;
		}
		gameObject.name = "WorldPointObject_" + pointIndex;
		gameObject.transform.SetParent(world.DynamicObjNode);
		gameObject.transform.position = base.WorldPosition;
		gameObject.SetActive(isVisible);
		gameObject.transform.localScale = Vector3.one;
		model = gameObject.transform.Find("Model").gameObject;
		model.gameObject.transform.localScale = Vector3.one;
		icon = gameObject.transform.Find("Icon").gameObject;
		icon.SetActive(value: false);
		normalTypeModel = gameObject.transform.Find("Model/normalType").gameObject;
		rewardTypeModel = gameObject.transform.Find("Model/rewardType").gameObject;
		nameTxt = gameObject.transform.Find("Model/nameContent/name").GetComponent<SuperTextMesh>();
		rewardBubbleContent = gameObject.transform.Find("Model/rewardBubble").gameObject;
		if (rewardBubbleContent != null)
		{
			rewardSpriteRenderer = gameObject.transform.Find("Model/rewardBubble/rewardIcon").GetComponent<SpriteRenderer>();
			rewardBubbleTxt = gameObject.transform.Find("Model/rewardBubble/rewardTxt").GetComponent<SuperTextMesh>();
			rewardBgSpriteRenderer = gameObject.transform.Find("Model/rewardBubble/rewardBg").GetComponent<SpriteRenderer>();
			rewardModelChildList = new List<GameObject>();
			int childCount = rewardTypeModel.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				GameObject item = rewardTypeModel.transform.GetChild(i).gameObject;
				rewardModelChildList.Add(item);
			}
			rewardBoxTouchEvent = rewardBubbleContent.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
			if (rewardBoxTouchEvent != null)
			{
				rewardBoxTouchEvent.previewType = WorldPreviewType.HighThanMultiObjects;
				rewardBoxTouchEvent.onPointerClick = delegate
				{
					GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetDetectTreasureReward", pointIndex);
				};
			}
		}
		SetAutoAdjustLod();
		ShowDetectEvent();
		RefreshShowTimeInstance();
		DoWhenMarchInfoChange(null);
		DoWhenInfoChange();
		DoWhenCreateComplete(model);
		CheckShowTroopDestination();
		SetClickEvent();
		PlayFlyAnim();
		GameEntry.Event.Subscribe(EventId.WorldTreasureReachDailyLimit, DoWhenReachDailyLimit);
	}

	public void DoWhenReachDailyLimit(object obj)
	{
		DoWhenInfoChange();
	}

	public virtual void PlayFlyAnim()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (!(world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo) || serverTime > treasurePointInfo.startTime + 2000)
		{
			return;
		}
		int num = treasurePointInfo.fromPoint;
		if (type == WorldTreasureType.SiegeTreasure && allianceIds != null && allianceIds[0] != null)
		{
			num = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetCityPointIdByAlliId", allianceIds[0]);
		}
		if (num <= 0)
		{
			return;
		}
		gameObject.SetActive(value: false);
		Vector3 startPos = world.TileIndexToWorld(num, serverId);
		Vector3 destPos = gameObject.transform.position;
		InstanceRequest flyEffectReq = GameEntry.Resource.InstantiateAsync("Assets/_Art_LastWar/Effect/Prefab/Prefab01/Eff_s_jiluofu_baoxiang_fly.prefab");
		if (flyEffectReq == null)
		{
			return;
		}
		flyEffectReq.completed += delegate
		{
			GameObject gameObject = flyEffectReq.gameObject;
			if (!(gameObject == null))
			{
				Transform transform = gameObject.transform;
				transform.position = startPos;
				DOTween.Sequence().Append(AnimationHelper.DOBezierCurve3D(transform, startPos, destPos, 2f, 0.5f, forward: false)).SetEase(Ease.Linear)
					.OnComplete(delegate
					{
						try
						{
							if (flyEffectReq != null)
							{
								flyEffectReq.Destroy();
								flyEffectReq = null;
							}
							if (base.gameObject != null)
							{
								base.gameObject.SetActive(value: true);
								world?.CreateVFX("Assets/_Art_LastWar/Effect/Prefab/Prefab01/Eff_s_jiluofu_baoxiang.prefab", destPos, 2f);
							}
						}
						catch (Exception message)
						{
							Log.Error(message);
							if (flyEffectReq != null)
							{
								flyEffectReq.Destroy();
								flyEffectReq = null;
							}
							if (base.gameObject != null)
							{
								base.gameObject.SetActive(value: true);
							}
						}
					});
			}
		};
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		DoWhenInfoChange();
	}

	protected override void DoWhenMarchInfoChange(object userData)
	{
	}

	public bool CheckInteractAuthority(TreasurePointInfo info)
	{
		switch (type)
		{
		case WorldTreasureType.ZONE_MOBILIZATION:
			if (info == null)
			{
				return false;
			}
			return info.serverId == GameEntry.Data.Player.GetSourceServerId();
		case WorldTreasureType.SiegeTreasure:
		case WorldTreasureType.InvasionTreasure:
		case WorldTreasureType.AllianceBossSandBox:
			return CheckIsSameAlliance();
		case WorldTreasureType.SandWormTreasure:
		case WorldTreasureType.FlowerCar:
		case WorldTreasureType.BloodyQueen:
		case WorldTreasureType.FlowerTrainUpgradeTreasure:
			return true;
		case WorldTreasureType.PlayerKillMonsterTreasure:
		case WorldTreasureType.GeneFragment:
			return info.killerId == GameEntry.Data.Player.GetUid();
		case WorldTreasureType.FlowerTrainCheerTreasure:
			return info.ownerUid == GameEntry.Data.Player.Uid;
		default:
			return true;
		}
	}

	protected virtual void DoWhenInfoChange()
	{
		if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo && !(normalTypeModel == null) && !(rewardTypeModel == null) && !(nameTxt == null) && !(rewardBubbleContent == null) && !(rewardBubbleTxt == null) && !(rewardBgSpriteRenderer == null) && !(rewardSpriteRenderer == null))
		{
			nameTxt.color = (CheckIsSameAlliance() ? nameColorBlue : Color.white);
			int num = treasurePointInfo.eventId.ToInt();
			string templateData = GameEntry.ConfigCache.GetTemplateData("world_treasure", num, "name");
			nameTxt.text = GameEntry.Localization.GetString(templateData);
			normalTypeModel.SetActive(value: false);
			rewardTypeModel.SetActive(value: true);
			rewardBubbleContent.SetActive(CheckInteractAuthority(treasurePointInfo));
			int num2 = 1;
			for (int i = 0; i < rewardModelChildList.Count; i++)
			{
				rewardModelChildList[i].SetActive(num2 == i + 1);
			}
			int num3 = GameEntry.ConfigCache.GetTemplateData("world_treasure", num, "max_reward_times").ToInt();
			int count = treasurePointInfo.rewardUserList.Count;
			int num4 = num3 - count;
			rewardBubbleTxt.text = $"{num4}/{num3}";
			string uid = GameEntry.Data.Player.GetUid();
			bool flag = treasurePointInfo.IsHaveGetReward(uid);
			string spritePath = GameEntry.Lua.CallWithReturn<string, bool>("CSharpCallLuaInterface.GetDetectTreasureRewardBg", flag);
			rewardBgSpriteRenderer.LoadSprite(spritePath);
			string spritePath2 = "Assets/Main/Sprites/UI/UIBuildBtns/lyp_daditu_paiqianlingqu.png";
			rewardSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
			if (num4 <= 0)
			{
				rewardSpriteRenderer.color = new Color(0.4f, 0.4f, 0.4f, 1f);
			}
			else if (flag)
			{
				spritePath2 = "Assets/Main/Sprites/UI/UIBuildBtns/zyf_zhujiemian_qipao_fenxiang.png";
			}
			rewardSpriteRenderer.LoadSprite(spritePath2);
			RefreshDetectTimeIconShow();
		}
	}

	public void RefreshDetectTimeIconShow()
	{
		pickupEndTime = 0L;
		if (!(world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo))
		{
			return;
		}
		if (!treasurePointInfo.complete)
		{
			if (detectEventInst != null && detectEventInst.gameObject != null)
			{
				if (treasurePointInfo.startTime > 0)
				{
					detectEventInst.gameObject.SetActive(value: false);
				}
				else
				{
					detectEventInst.gameObject.SetActive(value: true);
				}
			}
			if (pickGarbageInst != null && pickGarbageInst.gameObject != null)
			{
				if (treasurePointInfo.startTime > 0)
				{
					pickupStartTime = treasurePointInfo.startTime;
					pickupEndTime = treasurePointInfo.completionTime;
					pickGarbageInst.gameObject.SetActive(value: true);
					pickGarbageInst.gameObject.GetComponent<ChangeSceneCircleSlider>().Init(pickupStartTime, pickupEndTime);
					UpdateProgress();
				}
				else
				{
					pickGarbageInst.gameObject.SetActive(value: false);
				}
			}
		}
		else
		{
			if (detectEventInst != null && detectEventInst.gameObject != null)
			{
				detectEventInst.gameObject.SetActive(value: false);
			}
			if (pickGarbageInst != null && pickGarbageInst.gameObject != null)
			{
				pickGarbageInst.gameObject.SetActive(value: false);
			}
		}
	}

	public void RefreshShowTimeInstance()
	{
		if (!NeedShowTime() || pickGarbageInst != null)
		{
			return;
		}
		pickGarbageInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/CollectGarbageUI.prefab");
		pickGarbageInst.completed += delegate
		{
			pickGarbageInst.gameObject.transform.SetParent(gameObject.transform);
			pickGarbageInst.gameObject.transform.localPosition = Vector3.zero;
			timeText = pickGarbageInst.gameObject.transform.Find("PosGo/TimeText").GetComponent<SuperTextMesh>();
			timeText.gameObject.SetActive(value: true);
			pickGarbageInst.gameObject.GetComponent<ChangeSceneCircleSlider>().Init(pickupStartTime, pickupEndTime);
			UpdateProgress();
			SpriteRenderer component = pickGarbageInst.gameObject.transform.Find("PosGo/Icon").GetComponent<SpriteRenderer>();
			if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
			{
				string spritePath = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetDetectEventTreasureIcon", treasurePointInfo.eventId);
				component.LoadSprite(spritePath);
			}
			RefreshDetectTimeIconShow();
		};
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		long serverTime = GameEntry.Timer.GetServerTime();
		if (!haveSetDataCompleteByLocalData && pickupEndTime > 0 && serverTime > pickupEndTime + 1000)
		{
			haveSetDataCompleteByLocalData = true;
			if (world.GetPointInfo(pointIndex) is TreasurePointInfo { complete: false } treasurePointInfo)
			{
				treasurePointInfo.complete = true;
				UpdateGameObject();
			}
		}
	}
}
