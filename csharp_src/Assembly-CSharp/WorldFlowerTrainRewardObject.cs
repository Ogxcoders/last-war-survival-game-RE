using System;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using UnityEngine;

public class WorldFlowerTrainRewardObject : WorldDetectEventItemObject
{
	public SpriteRenderer rewardSpriteRenderer;

	public GameObject normalTypeModel;

	public GameObject rewardTypeModel;

	public SuperTextMesh nameTxt;

	public SuperTextMesh remainTimeTxt;

	public GameObject rewardBubbleContent;

	public SuperTextMesh rewardBubbleTxt;

	public List<GameObject> rewardModelChildList;

	public TouchObjectEventTrigger rewardBoxTouchEvent;

	public SpriteRenderer rewardBgSpriteRenderer;

	private WorldTreasureType type = WorldTreasureType.FlowerTrainCheerTreasure;

	public static Color nameColorBlue = new Color(0.329f, 0.768f, 0.949f);

	public static Color nameColorGreen = new Color(0.71f, 0.972f, 0.192f);

	public bool haveSetDataCompleteByLocalData;

	private SuperTextMesh _disappearTimeText;

	private long _lastCheckTime;

	private bool _isShowDisappearTime;

	private long _treasureDisappearTime;

	private static int SEGMENT_COUNT = 20;

	private static Vector3[] _paths = new Vector3[SEGMENT_COUNT];

	private const float BOX_FLY_DURATION = 1f;

	private const float BOX_ROTATION_SPEED = 1080f;

	private const string BOX_POINT_PATH = "Model/rewardType";

	private Sequence flyEffectSeq;

	public WorldFlowerTrainRewardObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
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

	protected virtual string GetConfigTableName()
	{
		return "world_treasure";
	}

	protected override void DoWhenCreateComplete(GameObject model)
	{
		if (!(model == null))
		{
			PlayFlyAnim();
		}
	}

	public void PlayFlyAnim()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (!(world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo) || serverTime - treasurePointInfo.startTime > 1000)
		{
			return;
		}
		int fromPoint = treasurePointInfo.fromPoint;
		if (fromPoint <= 0)
		{
			return;
		}
		Transform boxTrans = gameObject.transform.Find("Model/rewardType");
		SimpleAnimation simpleAni = boxTrans.GetComponentInChildren<SimpleAnimation>();
		Vector3 vector = world.TileIndexToWorld(fromPoint, serverId) + new Vector3(UnityEngine.Random.Range(1, 2), UnityEngine.Random.Range(1, 2), UnityEngine.Random.Range(1, 2));
		Vector3 position = gameObject.transform.position;
		Vector3 controlPos = Vector3.Lerp(vector, position, UnityEngine.Random.Range(0.2f, 0.4f)) + Vector3.up * UnityEngine.Random.Range(15, 20);
		Vector3[] path = Bezier2Path(vector, controlPos, position);
		boxTrans.position = vector;
		Vector3 vector2 = position - vector;
		Vector3 dir1 = new Vector3(0f - vector2.z, 0f, vector2.x);
		flyEffectSeq = DOTween.Sequence();
		flyEffectSeq.Append(boxTrans.DOPath(path, 1f, PathType.Linear, PathMode.Full3D, 10, Color.cyan).SetEase(Ease.Linear));
		flyEffectSeq.OnUpdate(delegate
		{
			Quaternion localRotation = boxTrans.localRotation * Quaternion.AngleAxis(-1080f * Time.deltaTime, dir1);
			boxTrans.localRotation = localRotation;
		});
		flyEffectSeq.OnComplete(delegate
		{
			flyEffectSeq = null;
			try
			{
				if (boxTrans != null)
				{
					boxTrans.gameObject.SetActive(value: true);
					boxTrans.localRotation = Quaternion.identity;
					boxTrans.localPosition = Vector3.zero;
				}
				if (simpleAni != null)
				{
					simpleAni.Play("Born");
				}
			}
			catch (Exception message)
			{
				Log.Error(message);
				if (boxTrans != null)
				{
					boxTrans.gameObject.SetActive(value: true);
					boxTrans.localRotation = Quaternion.identity;
					boxTrans.localPosition = Vector3.zero;
				}
			}
		});
	}

	private Vector3[] Bezier2Path(Vector3 startPos, Vector3 controlPos, Vector3 endPos)
	{
		for (int i = 1; i <= SEGMENT_COUNT; i++)
		{
			float t = (float)i / (float)SEGMENT_COUNT;
			Vector3 vector = CalculateCubicBezierPointfor2C(t, startPos, controlPos, endPos);
			_paths[i - 1] = vector;
		}
		return _paths;
	}

	private Vector3 CalculateCubicBezierPointfor2C(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float num = 1f - t;
		float num2 = t * t;
		return num * num * p0 + 2f * num * t * p1 + num2 * p2;
	}

	public override void Destroy()
	{
		rewardSpriteRenderer = null;
		normalTypeModel = null;
		rewardTypeModel = null;
		nameTxt = null;
		remainTimeTxt = null;
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
			text = GameEntry.ConfigCache.GetTemplateData("world_treasure", key, "fullPathModels");
			if (string.IsNullOrEmpty(text))
			{
				text = GameEntry.ConfigCache.GetTemplateData("world_treasure", key, "models");
				text = "Assets/Main/Prefabs/Garbage/" + text + ".prefab";
			}
			if (!WorldScene.ModelPathDic.ContainsKey(key))
			{
				WorldScene.ModelPathDic[key] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[key][key2] = text;
		}
		return text;
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
		gameObject.transform.position = world.TileIndexToWorld(pointIndex);
		gameObject.SetActive(isVisible);
		gameObject.transform.localScale = Vector3.one;
		model = gameObject.transform.Find("Model").gameObject;
		model.gameObject.transform.localScale = Vector3.one;
		icon = gameObject.transform.Find("Icon").gameObject;
		icon.SetActive(value: false);
		normalTypeModel = gameObject.transform.Find("Model/normalType").gameObject;
		rewardTypeModel = gameObject.transform.Find("Model/rewardType").gameObject;
		nameTxt = gameObject.transform.Find("Model/nameContent/name").GetComponent<SuperTextMesh>();
		remainTimeTxt = gameObject.transform.Find("Model/timeContent/time").GetComponent<SuperTextMesh>();
		TreasurePointInfo treasurePointInfo = world.GetPointInfo(pointIndex) as TreasurePointInfo;
		rewardBoxTouchEvent = gameObject.transform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
		if (rewardBoxTouchEvent != null)
		{
			string previewName = "";
			if (treasurePointInfo != null)
			{
				previewName = ((!(treasurePointInfo.ownerUid == GameEntry.Data.Player.Uid)) ? GameEntry.Localization.GetString("treasure_world_specialgift_name1") : GameEntry.Localization.GetString("treasure_world_specialgift_name2"));
			}
			rewardBoxTouchEvent.previewName = previewName;
			rewardBoxTouchEvent.previewIconPath = "Assets/Main/Sprites/LodIcon/lyp_daditu_ziyuandian_05.png";
			rewardBoxTouchEvent.previewType = WorldPreviewType.FlowerTrainReward;
			rewardBoxTouchEvent.onPointerClick = OnClickWorldPointObject;
		}
		rewardBubbleContent = gameObject.transform.Find("Model/rewardBubble").gameObject;
		bool flag = CheckInteractAuthority(treasurePointInfo);
		rewardBubbleContent.SetActive(flag);
		if (flag && rewardBubbleContent != null)
		{
			rewardBoxTouchEvent = rewardBubbleContent.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
			if (rewardBoxTouchEvent != null)
			{
				rewardBoxTouchEvent.previewType = WorldPreviewType.HighThanMultiObjects;
				rewardBoxTouchEvent.onPointerClick = ClaimCheerRewardImmediate;
			}
		}
		if (treasurePointInfo != null)
		{
			_treasureDisappearTime = treasurePointInfo.expireTime;
		}
		SetAutoAdjustLod();
		ShowDetectEvent();
		RefreshShowTimeInstance();
		DoWhenMarchInfoChange(null);
		DoWhenInfoChange();
		DoWhenCreateComplete(model);
		CheckShowTroopDestination();
		SetClickEvent();
		RefreshDisappearTime();
	}

	public void DoWhenReachDailyLimit(object obj)
	{
		DoWhenInfoChange();
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
		WorldTreasureType worldTreasureType = type;
		if (worldTreasureType == WorldTreasureType.FlowerTrainCheerTreasure)
		{
			return info.ownerUid == GameEntry.Data.Player.Uid;
		}
		return true;
	}

	protected virtual void DoWhenInfoChange()
	{
		if (!(world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo))
		{
			return;
		}
		int num = treasurePointInfo.eventId.ToInt();
		if (nameTxt != null)
		{
			if (treasurePointInfo.ownerUid == GameEntry.Data.Player.Uid)
			{
				nameTxt.color = nameColorGreen;
			}
			else
			{
				nameTxt.color = nameColorBlue;
			}
			nameTxt.text = UIUtils.FormatAllianceAndName(treasurePointInfo.allianceAbbr, treasurePointInfo.ownerName);
		}
		if (normalTypeModel == null || rewardTypeModel == null || nameTxt == null || rewardBubbleContent == null || rewardBubbleTxt == null || rewardBgSpriteRenderer == null || rewardSpriteRenderer == null)
		{
			return;
		}
		normalTypeModel.SetActive(value: false);
		rewardTypeModel.SetActive(value: true);
		rewardBubbleContent.SetActive(CheckInteractAuthority(treasurePointInfo));
		int num2 = 1;
		if (rewardModelChildList != null)
		{
			for (int i = 0; i < rewardModelChildList.Count; i++)
			{
				rewardModelChildList[i].SetActive(num2 == i + 1);
			}
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
		if (serverTime - _lastCheckTime > 1000)
		{
			_lastCheckTime = serverTime;
			RefreshDisappearTime();
		}
	}

	protected override void OnClickWorldPointObject()
	{
		GameEntry.Lua.Call("UIUtil.OnClickWorld", pointIndex, 1);
	}

	private void ClaimCheerRewardImmediate()
	{
		GameEntry.Lua.Call("UIUtil.GetDetectTreasureReward", pointIndex);
	}

	private void RefreshDisappearTime()
	{
		if (!(remainTimeTxt == null))
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			long num = _treasureDisappearTime - serverTime;
			if (num < 0)
			{
				num = 0L;
			}
			remainTimeTxt.text = GameEntry.Localization.GetString("radar_tips_16", GameEntry.Timer.MilliSecondToFmtString(num));
		}
	}
}
