using System.Collections.Generic;
using UnityEngine;

public class WorldTreasureObject : WorldDetectEventItemObject
{
	public SpriteRenderer rewardSpriteRenderer;

	public GameObject normalTypeModel;

	public GameObject rewardTypeModel;

	public GameObject nameContent;

	public SuperTextMesh nameTxt;

	public GameObject rewardBubbleContent;

	public SuperTextMesh rewardBubbleTxt;

	public List<GameObject> rewardModelChildList;

	public SimpleAnimation simpleAni;

	public TouchObjectEventTrigger rewardBoxTouchEvent;

	public SpriteRenderer rewardBgSpriteRenderer;

	private WorldTreasureType type = WorldTreasureType.RadarTreasure;

	private string allianceId;

	public static Color nameColorBlue = new Color(0.329f, 0.768f, 0.949f);

	public bool haveSetDataCompleteByLocalData;

	private GameObject _disappearContent;

	private SuperTextMesh _disappearTimeText;

	private long _lastCheckTime;

	private bool _isShowDisappearTime;

	private long _treasureDisappearTime;

	private long _shareTime;

	private bool _canBeShared;

	public WorldTreasureObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		haveSetDataCompleteByLocalData = false;
		if (!(world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo))
		{
			return;
		}
		allianceId = treasurePointInfo.allianceId;
		type = treasurePointInfo.type;
		_shareTime = 0L;
		_canBeShared = false;
		string text = treasurePointInfo.eventId;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		int result = 0;
		int.TryParse(text, out result);
		if (result > 0)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData(GetConfigTableName(), result, "share_para");
			int result2 = 0;
			int.TryParse(templateData, out result2);
			if (result2 > 0)
			{
				_shareTime = treasurePointInfo.createTime + result2 * 1000;
			}
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

	protected bool CheckIsSameAlliance()
	{
		string text = GameEntry.Data.Player.GetAllianceId();
		if (!string.IsNullOrEmpty(text))
		{
			return allianceId == text;
		}
		return false;
	}

	public bool CheckSelfInPoint(TreasurePointInfo info)
	{
		if (info != null && info.diggingUserList != null && info.diggingUserList.Count > 0)
		{
			for (int i = 0; i < info.diggingUserList.Count; i++)
			{
				if (info.diggingUserList[i].Uid == GameEntry.Data.Player.GetUid())
				{
					return true;
				}
			}
		}
		return false;
	}

	protected string GetConfigTableName()
	{
		return "detect_event";
	}

	public override void Destroy()
	{
		normalTypeModel = null;
		rewardTypeModel = null;
		nameContent = null;
		nameTxt = null;
		rewardBubbleContent = null;
		rewardBubbleTxt = null;
		rewardModelChildList = null;
		simpleAni = null;
		if (rewardBoxTouchEvent != null)
		{
			rewardBoxTouchEvent.onPointerClick = null;
			rewardBoxTouchEvent = null;
		}
		rewardSpriteRenderer = null;
		rewardBgSpriteRenderer = null;
		base.Destroy();
	}

	protected override bool NeedShowDetectEventIcon()
	{
		bool result = false;
		if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
		{
			result = (CheckIsSameAlliance() || treasurePointInfo.IsMine()) && !treasurePointInfo.IsComplete();
		}
		return result;
	}

	protected override bool NeedShowTime()
	{
		bool result = false;
		if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
		{
			int sourceServerId = GameEntry.Data.Player.GetSourceServerId();
			bool flag = treasurePointInfo.serverId != sourceServerId;
			result = (CheckIsSameAlliance() || treasurePointInfo.IsMine() || (_canBeShared && !flag)) && !treasurePointInfo.IsComplete();
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
		int key2 = 21;
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
			text = GameEntry.ConfigCache.GetTemplateData("detect_event", key, "image");
			if (!WorldScene.ModelPathDic.ContainsKey(key))
			{
				WorldScene.ModelPathDic[key] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[key][key2] = text;
		}
		string text2 = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetActivityRadarDigModel");
		return $"Assets/Main/Prefabs/Garbage/{text2 ?? text}.prefab";
	}

	public override void AsyncCompleteCallBack(InstanceRequest instance)
	{
		ClearOldObject();
		gameObject = instance.gameObject;
		if (!(gameObject != null))
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
		nameContent = gameObject.transform.Find("Model/nameContent").gameObject;
		nameTxt = gameObject.transform.Find("Model/nameContent/name").GetComponent<SuperTextMesh>();
		simpleAni = normalTypeModel.GetComponentInChildren<SimpleAnimation>();
		Transform transform = gameObject.transform.Find("Model/disappearContent");
		if (transform != null)
		{
			_disappearContent = transform.gameObject;
			_disappearTimeText = _disappearContent.transform.Find("disappearTimeText").GetComponent<SuperTextMesh>();
		}
		rewardBubbleContent = gameObject.transform.Find("Model/rewardBubble").gameObject;
		if (rewardBubbleContent != null)
		{
			rewardBubbleTxt = gameObject.transform.Find("Model/rewardBubble/rewardTxt").GetComponent<SuperTextMesh>();
			rewardBgSpriteRenderer = gameObject.transform.Find("Model/rewardBubble/rewardBg").GetComponent<SpriteRenderer>();
			rewardSpriteRenderer = gameObject.transform.Find("Model/rewardBubble/rewardIcon").GetComponent<SpriteRenderer>();
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
		if (nameContent != null)
		{
			nameContent.transform.localPosition = new Vector3(0f, 0.5f, -1.5f);
			nameContent.SetActive(value: true);
		}
		if (_disappearContent != null)
		{
			_disappearContent.transform.localPosition = new Vector3(0f, 0f, -1.5f);
		}
		SetAutoAdjustLod();
		ShowDetectEvent();
		RefreshShowTimeInstance();
		DoWhenMarchInfoChange(null);
		DoWhenInfoChange();
		DoWhenCreateComplete(model);
		CheckShowTroopDestination();
		SetClickEvent();
		SetMultiSelect();
		CheckShowTemperature();
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
		if (info == null)
		{
			return false;
		}
		bool num = info.IsMine();
		int sourceServerId = GameEntry.Data.Player.GetSourceServerId();
		int result = -1;
		if (!string.IsNullOrEmpty(info.customInfoStr))
		{
			result = (int.TryParse(info.customInfoStr, out result) ? result : (-1));
		}
		bool flag = info.serverId != sourceServerId && sourceServerId != result;
		if (!num && !CheckIsSameAlliance())
		{
			if (_canBeShared)
			{
				return !flag;
			}
			return false;
		}
		return true;
	}

	protected void DoWhenInfoChange()
	{
		if (!(world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo) || normalTypeModel == null || rewardTypeModel == null || nameTxt == null || rewardBubbleContent == null || rewardBubbleTxt == null || rewardBgSpriteRenderer == null || rewardSpriteRenderer == null || nameContent == null)
		{
			return;
		}
		if (CheckIsSameAlliance())
		{
			nameTxt.color = nameColorBlue;
		}
		else
		{
			nameTxt.color = Color.white;
		}
		string text = "";
		text = ((!string.IsNullOrEmpty(treasurePointInfo.allianceAbbr)) ? $"[{treasurePointInfo.allianceAbbr}]{treasurePointInfo.ownerName}" : treasurePointInfo.ownerName);
		string text2 = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetDetectEventTreasureNameDes", treasurePointInfo.eventId, text);
		nameTxt.text = text2;
		if (!treasurePointInfo.IsComplete())
		{
			normalTypeModel.SetActive(value: true);
			rewardTypeModel.SetActive(value: false);
			rewardBubbleContent.SetActive(value: false);
			if (treasurePointInfo.startTime > 0)
			{
				if (simpleAni != null && simpleAni.GetState("idle") != null)
				{
					simpleAni.Play("idle");
				}
			}
			else if (simpleAni != null)
			{
				if (simpleAni.GetState("Default") != null)
				{
					simpleAni.Play("Default");
				}
				else
				{
					simpleAni.Stop();
				}
			}
		}
		else
		{
			normalTypeModel.SetActive(value: false);
			rewardTypeModel.SetActive(value: true);
			rewardBubbleContent.SetActive(CheckInteractAuthority(treasurePointInfo));
			int num = treasurePointInfo.eventId.ToInt();
			int num2 = GameEntry.ConfigCache.GetTemplateData("detect_event", num, "c_para").ToInt();
			for (int i = 0; i < rewardModelChildList.Count; i++)
			{
				GameObject gameObject = rewardModelChildList[i];
				if (num2 == i + 1)
				{
					gameObject.SetActive(value: true);
				}
				else
				{
					gameObject.SetActive(value: false);
				}
			}
			int num3 = treasurePointInfo.GetRewardMaxNum();
			if (treasurePointInfo.multiple > 1)
			{
				num3 = treasurePointInfo.GetRewardMaxNum() * treasurePointInfo.multiple;
			}
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
		}
		RefreshDetectTimeIconShow();
		RefreshTreasureDisappearTimeShow();
	}

	public void RefreshDetectTimeIconShow()
	{
		pickupEndTime = 0L;
		if (!(world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo))
		{
			return;
		}
		if (!treasurePointInfo.IsComplete())
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
			if (pickGarbageInst == null || !(pickGarbageInst.gameObject != null))
			{
				return;
			}
			if (treasurePointInfo.startTime > 0)
			{
				pickupStartTime = treasurePointInfo.startTime;
				pickupEndTime = treasurePointInfo.completionTime;
				pickGarbageInst.gameObject.SetActive(value: true);
				pickGarbageInst.gameObject.GetComponent<ChangeSceneCircleSlider>().Init(pickupStartTime, pickupEndTime);
				if (treasurePointInfo.diggingUserList != null && treasurePointInfo.diggingUserList.Count > 0)
				{
					needUpdateProgress = true;
					UpdateProgress();
					return;
				}
				needUpdateProgress = false;
				if (timeText != null)
				{
					timeText.text = GameEntry.Localization.GetString("detect_dig_tips_stop");
				}
			}
			else
			{
				pickGarbageInst.gameObject.SetActive(value: false);
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
			UpdateTreasureDisappearCutDownTime();
			if (_shareTime > 0 && !_canBeShared && serverTime > _shareTime)
			{
				_canBeShared = true;
				DoWhenInfoChange();
			}
		}
	}

	private void RefreshTreasureDisappearTimeShow()
	{
		_isShowDisappearTime = false;
		_treasureDisappearTime = 0L;
		if (_disappearContent != null && _disappearTimeText != null)
		{
			if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo && treasurePointInfo.IsComplete())
			{
				_isShowDisappearTime = true;
				_treasureDisappearTime = treasurePointInfo.expireTime;
				_disappearContent.SetActive(value: true);
				UpdateTreasureDisappearCutDownTime();
			}
			else
			{
				_disappearContent.SetActive(value: false);
			}
		}
	}

	private void UpdateTreasureDisappearCutDownTime()
	{
		if (_isShowDisappearTime && _disappearTimeText != null)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			long num = _treasureDisappearTime - serverTime;
			if (num < 0)
			{
				num = 0L;
			}
			_disappearTimeText.text = GameEntry.Localization.GetString("radar_tips_16", GameEntry.Timer.MilliSecondToFmtString(num));
		}
	}

	public bool GetCanBeShared()
	{
		return _canBeShared;
	}
}
