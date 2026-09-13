using System;
using System.Collections.Generic;
using BaseUtils;
using DG.Tweening;
using UnityEngine;

public class WorldHeroDispatchTaskObject : WorldPointObject
{
	protected GameObject model;

	private GameObject icon;

	protected InstanceRequest dispatchRewardInst;

	protected InstanceRequest dispatchCdInst;

	protected InstanceRequest dispatchOpenInst;

	protected long pickupEndTime;

	protected long pickupStartTime;

	protected long lastUpdateServerTime;

	protected TextMeshProEx timeText;

	protected bool worldOpen;

	private TouchObjectEventTrigger openBubbleTouchEvent;

	private bool markDelete;

	protected AutoAdjustLod dispatchCdLod;

	public WorldHeroDispatchTaskObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		markDelete = false;
		if (world.GetPointInfo(pointIndex) is HeroDispatchMissionPointInfo heroDispatchMissionPointInfo)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", heroDispatchMissionPointInfo.cfgId, "world_open");
			if (!templateData.IsNullOrEmpty())
			{
				worldOpen = templateData == "1";
			}
		}
	}

	public override void SetAutoAdjustLod()
	{
		adjuster = gameObject.GetComponent<AutoAdjustLod>();
		if (adjuster == null)
		{
			adjuster = gameObject.AddComponent<AutoAdjustLod>();
		}
		LodType lodType = adjuster.getLodType();
		LodType lodType2 = LodType.DispatchTaskNode;
		if (worldOpen && world.GetPointInfo(pointIndex) is HeroDispatchMissionPointInfo heroDispatchMissionPointInfo && heroDispatchMissionPointInfo.completionTime > 0 && heroDispatchMissionPointInfo.rewarded == 0 && heroDispatchMissionPointInfo.completionTime <= GameEntry.Timer.GetServerTime())
		{
			lodType2 = LodType.DispatchTask;
			if (icon != null && !icon.activeSelf)
			{
				icon.SetActive(value: true);
			}
		}
		if (lodType != lodType2)
		{
			if (lodType2 == LodType.DispatchTaskNode && (bool)icon && icon.activeSelf)
			{
				icon.SetActive(value: false);
			}
			adjuster.SetLodType(lodType2);
		}
	}

	public virtual void UpdateDetectEventActive()
	{
		if (adjuster != null)
		{
			adjuster.UpdateLod(SceneManager.World.GetLodLevel());
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		if (dispatchCdInst == null && pickupStartTime > 0 && GameEntry.Timer.GetServerTime() >= pickupStartTime)
		{
			DoWhenMarchInfoChange(null);
		}
		else
		{
			UpdateProgress();
		}
	}

	public virtual bool DoDisappear()
	{
		markDelete = true;
		if ((bool)model)
		{
			DestroyInstanceRequests();
			Sequence sequence = DOTween.Sequence();
			sequence.Append(model.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
			sequence.Append(model.gameObject.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.3f));
			sequence.onComplete = delegate
			{
				SceneManager.World.AddToDeleteList(pointIndex);
			};
			return true;
		}
		return false;
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		DoWhenMarchInfoChange(null);
	}

	protected virtual bool NeedShowTime()
	{
		if (!(world.GetPointInfo(pointIndex) is HeroDispatchMissionPointInfo heroDispatchMissionPointInfo))
		{
			return false;
		}
		pickupStartTime = 0L;
		pickupEndTime = 0L;
		long serverTime = GameEntry.Timer.GetServerTime();
		if (heroDispatchMissionPointInfo.completionTime == 0L || heroDispatchMissionPointInfo.rewarded == 1 || heroDispatchMissionPointInfo.completionTime <= serverTime)
		{
			return false;
		}
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", heroDispatchMissionPointInfo.cfgId, "times");
		if (templateData.IsNullOrEmpty())
		{
			return false;
		}
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		int num = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", heroDispatchMissionPointInfo.cfgId, "show_time").ToInt();
		if (num > 0 && GameEntry.Data.Player.Uid != heroDispatchMissionPointInfo.ownerUid && (BaseUtils.StringUtils.IsNullOrEmpty(heroDispatchMissionPointInfo.allianceId) || allianceId != heroDispatchMissionPointInfo.allianceId))
		{
			num *= 1000;
			if (heroDispatchMissionPointInfo.completionTime - num > serverTime)
			{
				return false;
			}
		}
		long num2 = templateData.ToLong() * 1000;
		long num3 = (pickupStartTime = heroDispatchMissionPointInfo.completionTime - num2);
		pickupEndTime = heroDispatchMissionPointInfo.completionTime;
		if (num3 > serverTime)
		{
			return false;
		}
		return true;
	}

	protected virtual string GetModePath()
	{
		if (world.GetPointInfo(pointIndex) is HeroDispatchMissionPointInfo heroDispatchMissionPointInfo)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", heroDispatchMissionPointInfo.cfgId, "model_name");
			if (!templateData.IsNullOrEmpty())
			{
				return templateData;
			}
		}
		return "Assets/Main/Prefabs/DispatchTask/dispatchTask1.prefab";
	}

	public void UpdateProgress()
	{
		if (timeText == null || dispatchCdInst == null)
		{
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		if (pickupEndTime > serverTime)
		{
			if ((!(dispatchCdLod != null) || dispatchCdLod.IsShow()) && serverTime - lastUpdateServerTime > 500)
			{
				int num = (int)Math.Ceiling((double)(pickupEndTime - serverTime) * 1.0 / 1000.0);
				timeText.text = GameEntry.Timer.SecondToFmtString(num);
				lastUpdateServerTime = serverTime;
			}
			return;
		}
		if (dispatchCdInst != null)
		{
			dispatchCdInst.Destroy();
			dispatchCdInst = null;
			dispatchCdLod = null;
		}
		CheckShowRewardUI();
	}

	protected virtual void DoWhenMarchInfoChange(object userData)
	{
		if (gameObject == null || markDelete)
		{
			return;
		}
		if (!NeedShowTime())
		{
			if (dispatchCdInst != null)
			{
				dispatchCdInst.Destroy();
				dispatchCdInst = null;
				dispatchCdLod = null;
			}
			UpdateDetectEventActive();
			CheckShowRewardUI();
			return;
		}
		if (dispatchCdInst == null)
		{
			dispatchCdInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/DispatchTask/dispatchTaskCdUI.prefab");
			dispatchCdInst.completed += delegate
			{
				if (gameObject == null)
				{
					if (dispatchCdInst != null)
					{
						dispatchCdInst.Destroy();
						dispatchCdInst = null;
						dispatchCdLod = null;
					}
				}
				else
				{
					dispatchCdInst.gameObject.transform.SetParent(gameObject.transform);
					dispatchCdInst.gameObject.transform.localPosition = Vector3.zero;
					timeText = dispatchCdInst.gameObject.transform.Find("PosGo/TimeText").GetComponent<TextMeshProEx>();
					dispatchCdLod = dispatchCdInst.gameObject.GetComponent<AutoAdjustLod>();
					dispatchCdInst.gameObject.GetComponent<ChangeSceneCircleSlider>().Init(pickupStartTime, pickupEndTime);
					UpdateProgress();
				}
			};
		}
		UpdateDetectEventActive();
		CheckShowRewardUI();
	}

	private void OnAllianceBaseDataUpdated(object userData)
	{
		if (!(gameObject == null) && !markDelete && world.GetPointInfo(pointIndex) is HeroDispatchMissionPointInfo info && dispatchRewardInst != null && dispatchRewardInst.gameObject != null)
		{
			UpdateShowRewardUI(info, dispatchRewardInst.gameObject);
		}
	}

	private void OnTodayNumUpdated(object userData)
	{
		if (!(gameObject == null) && !markDelete && world.GetPointInfo(pointIndex) is HeroDispatchMissionPointInfo info && dispatchRewardInst != null && (bool)dispatchRewardInst.gameObject)
		{
			UpdateShowRewardUI(info, dispatchRewardInst.gameObject);
		}
	}

	public override void Destroy()
	{
		GameEntry.Event.Unsubscribe(EventId.AllianceBaseDataUpdated, OnAllianceBaseDataUpdated);
		GameEntry.Event.Unsubscribe(EventId.DispatchTaskTodayNumUpdate, OnTodayNumUpdated);
		GameEntry.Event.Unsubscribe(EventId.DispatchTaskMarkPush, OnMarkPush);
		DestroyInstanceRequests();
		base.Destroy();
	}

	private void DestroyInstanceRequests()
	{
		if (dispatchRewardInst != null)
		{
			dispatchRewardInst.Destroy();
			dispatchRewardInst = null;
		}
		if (dispatchCdInst != null)
		{
			dispatchCdInst.Destroy();
			dispatchCdInst = null;
			dispatchCdLod = null;
		}
		if (dispatchOpenInst != null)
		{
			dispatchOpenInst.Destroy();
			dispatchOpenInst = null;
		}
		if (openBubbleTouchEvent != null)
		{
			openBubbleTouchEvent.onPointerClick = null;
			openBubbleTouchEvent = null;
		}
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		AddOldObject();
		GameEntry.Event.Subscribe(EventId.AllianceBaseDataUpdated, OnAllianceBaseDataUpdated);
		GameEntry.Event.Subscribe(EventId.DispatchTaskTodayNumUpdate, OnTodayNumUpdated);
		GameEntry.Event.Subscribe(EventId.DispatchTaskMarkPush, OnMarkPush);
		instance = GameEntry.Resource.InstantiateAsync(GetModePath());
		instance.completed += delegate
		{
			ClearOldObject();
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.name = "WorldPointObject_" + pointIndex;
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(isVisible);
				gameObject.transform.localScale = Vector3.one;
				model = gameObject.transform.Find("Model").gameObject;
				model.gameObject.transform.localScale = Vector3.one;
				model.SetActive(value: true);
				icon = gameObject.transform.Find("Icon").gameObject;
				icon.SetActive(value: false);
				SetClickEvent();
				if (world.GetPointInfo(pointIndex) is HeroDispatchMissionPointInfo heroDispatchMissionPointInfo)
				{
					UIWorldLabel uIWorldLabel = gameObject.transform.Find("Model/ModelLabel")?.GetComponent<UIWorldLabel>();
					UIWorldLabel uIWorldLabel2 = gameObject.transform.Find("Icon/IconLabel")?.GetComponent<UIWorldLabel>();
					string templateData = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", heroDispatchMissionPointInfo.cfgId, "level");
					if (uIWorldLabel != null)
					{
						uIWorldLabel.gameObject.SetActive(value: true);
						uIWorldLabel.SetLevel(templateData.ToInt());
					}
					if (uIWorldLabel2 != null)
					{
						uIWorldLabel2.gameObject.SetActive(value: true);
						uIWorldLabel2.SetLevel(templateData.ToInt());
					}
				}
				DoWhenMarchInfoChange(null);
				DoWhenCreateComplete(model);
				CheckShowTroopDestination();
			}
		};
	}

	protected virtual void DoWhenCreateComplete(GameObject model)
	{
	}

	private void UpdateShowRewardUI(HeroDispatchMissionPointInfo info, GameObject dispatchReward)
	{
		if (info == null || dispatchReward == null || dispatchReward.transform == null)
		{
			return;
		}
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		SuperTextMesh component = dispatchReward.transform.Find("Transform/StealText").GetComponent<SuperTextMesh>();
		SpriteRenderer component2 = dispatchReward.transform.Find("Transform/Detect_event_icon").GetComponent<SpriteRenderer>();
		SpriteRenderer component3 = dispatchReward.transform.Find("Transform/Detect_event_quality_icon").GetComponent<SpriteRenderer>();
		if (GameEntry.Data.Player.Uid == info.ownerUid)
		{
			component2.LoadSprite("Assets/Main/Sprites/UI/UIBuildBtns/lyp_daditu_paiqianlingqu.png");
			component3.LoadSprite("Assets/Main/Sprites/UI/UIBuildBubble/cfm_zhujiemian_qipao_2");
			component.gameObject.SetActive(value: false);
			return;
		}
		if (!BaseUtils.StringUtils.IsNullOrEmpty(info.allianceId) && allianceId == info.allianceId)
		{
			int num = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "ActDispatchTaskTodayAssistNum");
			int num2 = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "ActDispatchTaskMaxAssistNum");
			if (num < num2)
			{
				component2.LoadSprite("Assets/Main/Sprites/UI/UIBuildBtns/lyp_daditu_paiqianlingqu.png");
				component3.LoadSprite("Assets/Main/Sprites/UI/UIBuildBubble/cfm_zhujiemian_qipao_lan");
				component.gameObject.SetActive(value: false);
			}
			else
			{
				component.gameObject.SetActive(value: false);
				dispatchReward.SetActive(value: false);
			}
			return;
		}
		component3.LoadSprite("Assets/Main/Sprites/UI/UIBuildBubble/cfm_zhujiemian_qipao_1");
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", info.cfgId, "steal_maxtimes");
		int num3 = int.Parse(templateData);
		if (info.stealList.Count < num3)
		{
			component.gameObject.SetActive(value: true);
			component.text = GameEntry.Localization.GetString("135225", info.stealList.Count, templateData);
			bool flag = GameEntry.Lua.CallWithReturn<bool, long>("CSharpCallLuaInterface.DispatchTaskHasMarked", info.uuid);
			if (info.stealList.Contains(GameEntry.Data.Player.Uid))
			{
				dispatchReward.SetActive(!flag);
				if (!flag)
				{
					component2.LoadSprite("Assets/Main/Sprites/UI/UIBuildBtns/zyf_zhujiemian_qipao_fenxiang.png");
				}
				return;
			}
			int num4 = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "ActDispatchTaskTodayStealNum");
			int num5 = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "ActDispatchTaskMaxStealNum");
			if (num4 < num5)
			{
				component2.LoadSprite("Assets/Main/Sprites/UI/UIBuildBtns/lyp_daditu_paiqianlingqu.png");
				return;
			}
			dispatchReward.SetActive(!flag);
			if (!flag)
			{
				component2.LoadSprite("Assets/Main/Sprites/UI/UIBuildBtns/zyf_zhujiemian_qipao_fenxiang.png");
			}
		}
		else
		{
			component.gameObject.SetActive(value: false);
			dispatchReward.SetActive(value: false);
		}
	}

	private void CheckShowRewardUI()
	{
		if (gameObject == null || markDelete)
		{
			return;
		}
		HeroDispatchMissionPointInfo info = world.GetPointInfo(pointIndex) as HeroDispatchMissionPointInfo;
		if (info == null)
		{
			return;
		}
		SetAutoAdjustLod();
		if (info.completionTime == 0L || info.rewarded == 1 || info.completionTime > GameEntry.Timer.GetServerTime())
		{
			if (dispatchRewardInst != null)
			{
				dispatchRewardInst.Destroy();
				dispatchRewardInst = null;
			}
			if (info.completionTime == 0L && info.ownerUid == GameEntry.Data.Player.Uid)
			{
				if (dispatchOpenInst != null)
				{
					return;
				}
				dispatchOpenInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/DispatchTask/dispatchTaskOpenUI.prefab");
				dispatchOpenInst.completed += delegate
				{
					if (gameObject == null)
					{
						if (dispatchOpenInst != null)
						{
							dispatchOpenInst.Destroy();
							dispatchOpenInst = null;
						}
					}
					else
					{
						dispatchOpenInst.gameObject.SetActive(value: true);
						dispatchOpenInst.gameObject.transform.SetParent(gameObject.transform);
						dispatchOpenInst.gameObject.transform.localPosition = Vector3.zero;
						Transform transform = dispatchOpenInst.gameObject.transform;
						openBubbleTouchEvent = transform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
						if (openBubbleTouchEvent != null)
						{
							openBubbleTouchEvent.onPointerClick = base.OnClickPoint;
							string templateData = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", info.cfgId, "level");
							string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", info.cfgId, "color");
							string templateData3 = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", info.cfgId, "name");
							openBubbleTouchEvent.previewName = "Lv." + templateData + GameEntry.Localization.GetString(templateData3);
							openBubbleTouchEvent.previewIconPath = "Assets/Main/Sprites/LodIcon/lyp_daditu_ziyuandian_0" + templateData2 + ".png";
							openBubbleTouchEvent.previewType = WorldPreviewType.Dispatch;
						}
					}
				};
			}
			else if (dispatchOpenInst != null)
			{
				dispatchOpenInst.Destroy();
				dispatchOpenInst = null;
			}
			return;
		}
		if (dispatchRewardInst != null)
		{
			if ((bool)dispatchRewardInst.gameObject)
			{
				UpdateShowRewardUI(info, dispatchRewardInst.gameObject);
			}
			return;
		}
		dispatchRewardInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/DispatchTask/dispatchTaskRewardUI.prefab");
		dispatchRewardInst.completed += delegate
		{
			if (gameObject == null)
			{
				if (dispatchRewardInst != null)
				{
					dispatchRewardInst.Destroy();
					dispatchRewardInst = null;
				}
			}
			else
			{
				dispatchRewardInst.gameObject.SetActive(value: true);
				dispatchRewardInst.gameObject.transform.SetParent(gameObject.transform);
				dispatchRewardInst.gameObject.transform.localPosition = Vector3.zero;
				UpdateShowRewardUI(info, dispatchRewardInst.gameObject);
				UpdateDetectEventActive();
				Transform transform = dispatchRewardInst.gameObject.transform;
				bubbleTouchEvent = transform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
				if (bubbleTouchEvent != null)
				{
					bubbleTouchEvent.onPointerClick = base.OnClickPoint;
					string templateData = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", info.cfgId, "level");
					string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", info.cfgId, "color");
					string templateData3 = GameEntry.ConfigCache.GetTemplateData("lw_dispatch_tasks", info.cfgId, "name");
					bubbleTouchEvent.previewName = "Lv." + templateData + GameEntry.Localization.GetString(templateData3);
					bubbleTouchEvent.previewIconPath = "Assets/Main/Sprites/LodIcon/lyp_daditu_ziyuandian_0" + templateData2 + ".png";
					bubbleTouchEvent.previewType = WorldPreviewType.Dispatch;
				}
			}
		};
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

	public override void RecordBlockIndex(Dictionary<int, int> set)
	{
	}

	public override void RemoveBlockIndex(Dictionary<int, int> set)
	{
		if (world.GetPointInfo(pointIndex) is HeroDispatchMissionPointInfo heroDispatchMissionPointInfo && heroDispatchMissionPointInfo.completionTime > 0 && heroDispatchMissionPointInfo.completionTime <= GameEntry.Timer.GetServerTime())
		{
			base.RemoveBlockIndex(set);
		}
	}

	private void OnMarkPush(object obj)
	{
		if (!(gameObject == null) && !markDelete && world.GetPointInfo(pointIndex) is HeroDispatchMissionPointInfo heroDispatchMissionPointInfo && dispatchRewardInst != null && (bool)dispatchRewardInst.gameObject)
		{
			long num = (long)obj;
			if (heroDispatchMissionPointInfo.uuid == num)
			{
				UpdateShowRewardUI(heroDispatchMissionPointInfo, dispatchRewardInst.gameObject);
			}
		}
	}
}
