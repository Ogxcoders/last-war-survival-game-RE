using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WorldGhostreconTaskObject : WorldPointObject
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

	public WorldGhostreconTaskObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		markDelete = false;
		if (world.GetPointInfo(pointIndex) is GhostreconPointInfo ghostreconPointInfo)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_ghostrecon_tasks", ghostreconPointInfo.cfgId, "world_open");
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
		if (worldOpen && world.GetPointInfo(pointIndex) is GhostreconPointInfo ghostreconPointInfo && ghostreconPointInfo.completionTime > 0 && ghostreconPointInfo.completionTime <= GameEntry.Timer.GetServerTime())
		{
			lodType2 = LodType.DispatchTask;
			if (icon != null && !icon.activeSelf)
			{
				if (ghostreconPointInfo.OwnHaveReward())
				{
					icon.SetActive(value: true);
				}
				else if (!ghostreconPointInfo.OwnIsAlly())
				{
					int num = GameEntry.ConfigCache.GetTemplateData("lw_ghostrecon_tasks", ghostreconPointInfo.cfgId, "steal_maxtimes").ToInt();
					if (ghostreconPointInfo.stealList.Count < num)
					{
						icon.SetActive(value: true);
					}
				}
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
		if (!(world.GetPointInfo(pointIndex) is GhostreconPointInfo ghostreconPointInfo))
		{
			return false;
		}
		pickupStartTime = 0L;
		pickupEndTime = 0L;
		long serverTime = GameEntry.Timer.GetServerTime();
		if (ghostreconPointInfo.completionTime > 0 && ghostreconPointInfo.completionTime > serverTime && (ghostreconPointInfo.OwnIsJoin() || ghostreconPointInfo.OwnIsAlly() || ghostreconPointInfo.completionTime - serverTime <= 600000))
		{
			long num = GameEntry.ConfigCache.GetTemplateData("lw_ghostrecon_tasks", ghostreconPointInfo.cfgId, "times").ToLong() * 1000;
			long num2 = ghostreconPointInfo.completionTime - num;
			pickupStartTime = num2;
			pickupEndTime = ghostreconPointInfo.completionTime;
		}
		return pickupEndTime > 0;
	}

	protected virtual string GetModePath()
	{
		if (world.GetPointInfo(pointIndex) is GhostreconPointInfo ghostreconPointInfo)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_ghostrecon_tasks", ghostreconPointInfo.cfgId, "model_name");
			if (!templateData.IsNullOrEmpty())
			{
				return templateData;
			}
		}
		return "Assets/Main/Prefabs/DispatchTask/dispatchTask_1_SSR.prefab";
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
		if (!(gameObject == null) && !markDelete && world.GetPointInfo(pointIndex) is GhostreconPointInfo)
		{
			CheckShowRewardUI();
		}
	}

	private void OnTodayNumUpdated(object userData)
	{
		if (!(gameObject == null) && !markDelete && world.GetPointInfo(pointIndex) is GhostreconPointInfo)
		{
			CheckShowRewardUI();
		}
	}

	public override void Destroy()
	{
		GameEntry.Event.Unsubscribe(EventId.AllianceBaseDataUpdated, OnAllianceBaseDataUpdated);
		GameEntry.Event.Unsubscribe(EventId.DispatchTaskTodayNumUpdate, OnTodayNumUpdated);
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
				icon = gameObject.transform.Find("Icon").gameObject;
				icon.SetActive(value: false);
				SetClickEvent();
				if (world.GetPointInfo(pointIndex) is GhostreconPointInfo ghostreconPointInfo)
				{
					UIWorldLabel uIWorldLabel = gameObject.transform.Find("Model/ModelLabel")?.GetComponent<UIWorldLabel>();
					UIWorldLabel uIWorldLabel2 = gameObject.transform.Find("Icon/IconLabel")?.GetComponent<UIWorldLabel>();
					string templateData = GameEntry.ConfigCache.GetTemplateData("lw_ghostrecon_tasks", ghostreconPointInfo.cfgId, "level");
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
					if (_touchObject != null)
					{
						string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_ghostrecon_tasks", ghostreconPointInfo.cfgId, "color");
						string templateData3 = GameEntry.ConfigCache.GetTemplateData("lw_ghostrecon_tasks", ghostreconPointInfo.cfgId, "name");
						_touchObject.previewName = "Lv." + templateData + GameEntry.Localization.GetString(templateData3);
						_touchObject.previewIconPath = "Assets/Main/Sprites/LodIcon/lyp_daditu_ziyuandian_0" + templateData2 + ".png";
						_touchObject.previewType = WorldPreviewType.Dispatch;
					}
				}
				SetAutoAdjustLod();
				CheckShowRewardUI();
				DoWhenMarchInfoChange(null);
				DoWhenCreateComplete(model);
				CheckShowTroopDestination();
			}
		};
	}

	protected virtual void DoWhenCreateComplete(GameObject model)
	{
	}

	private void CheckShowRewardUI()
	{
		if (gameObject == null || markDelete || !(world.GetPointInfo(pointIndex) is GhostreconPointInfo ghostreconPointInfo))
		{
			return;
		}
		SetAutoAdjustLod();
		if (dispatchRewardInst != null && dispatchRewardInst.gameObject != null)
		{
			dispatchRewardInst.gameObject.SetActive(value: false);
			TextMeshProEx component = dispatchRewardInst.gameObject.transform.Find("Transform/StealText").GetComponent<TextMeshProEx>();
			component.color32 = GameDefines.CityLabelTextColor.White;
			component.fontSize = 8f;
			SpriteRenderer component2 = dispatchRewardInst.gameObject.transform.Find("Transform/Detect_event_icon").GetComponent<SpriteRenderer>();
			SpriteRenderer component3 = dispatchRewardInst.gameObject.transform.Find("Transform/Detect_event_quality_icon").GetComponent<SpriteRenderer>();
			if (ghostreconPointInfo.completionTime > 0 && ghostreconPointInfo.completionTime <= GameEntry.Timer.GetServerTime())
			{
				if (ghostreconPointInfo.OwnIsJoin())
				{
					if (ghostreconPointInfo.OwnHaveReward())
					{
						component2.LoadSprite("Assets/Main/Sprites/UI/UIBuildBtns/lyp_daditu_paiqianlingqu.png");
						component3.LoadSprite("Assets/Main/Sprites/UI/UIBuildBubble/cfm_zhujiemian_qipao_2");
						dispatchRewardInst.gameObject.SetActive(value: true);
					}
					component.gameObject.SetActive(value: false);
				}
				else
				{
					if (!GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.GetActGhostreconCanGet", ghostreconPointInfo.ownerServer) || ghostreconPointInfo.OwnIsAlly())
					{
						return;
					}
					int num = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "ActGhostreconTodayStealNum");
					int num2 = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "ActGhostreconMaxStealNum");
					int num3 = GameEntry.ConfigCache.GetTemplateData("lw_ghostrecon_tasks", ghostreconPointInfo.cfgId, "steal_maxtimes").ToInt();
					if (ghostreconPointInfo.stealList.Count < num3)
					{
						if (num < num2 && !ghostreconPointInfo.stealList.Contains(GameEntry.Data.Player.Uid))
						{
							component2.LoadSprite("Assets/Main/Sprites/UI/UIBuildBtns/lyp_daditu_paiqianlingqu.png");
							component3.LoadSprite("Assets/Main/Sprites/UI/UIBuildBubble/cfm_zhujiemian_qipao_1");
							dispatchRewardInst.gameObject.SetActive(value: true);
							component.gameObject.SetActive(value: true);
							component.text = GameEntry.Localization.GetString("135225", ghostreconPointInfo.stealList.Count, num3);
						}
						else
						{
							component2.LoadSprite("Assets/Main/Sprites/UI/UIBuildBtns/zyf_zhujiemian_qipao_fenxiang.png");
							component3.LoadSprite("Assets/Main/Sprites/UI/UIBuildBubble/cfm_zhujiemian_qipao_1");
							dispatchRewardInst.gameObject.SetActive(value: true);
							component.gameObject.SetActive(value: false);
						}
					}
				}
			}
			else if (ghostreconPointInfo.teamStartTime > 0 && ghostreconPointInfo.completionTime == 0L && (ghostreconPointInfo.OwnIsJoin() || ghostreconPointInfo.OwnIsAlly()))
			{
				component2.LoadSprite("Assets/Main/Sprites/UI/UIBuildBtns/ljq_zhujiemian_qipao_zudui.png");
				component3.LoadSprite("Assets/Main/Sprites/UI/UIBuildBubble/cfm_zhujiemian_qipao_1");
				dispatchRewardInst.gameObject.SetActive(value: true);
				component.text = GameEntry.Localization.GetString("ghostrecon_054", ghostreconPointInfo.memberList.Count);
				component.gameObject.SetActive(value: true);
				component.fontSize = 7f;
				if (ghostreconPointInfo.OwnIsLeader() && ghostreconPointInfo.memberList.Count == 5)
				{
					component.color32 = new Color32(185, 251, 63, byte.MaxValue);
				}
				else
				{
					component.color32 = GameDefines.CityLabelTextColor.White;
				}
			}
		}
		else
		{
			if (dispatchRewardInst != null)
			{
				return;
			}
			dispatchRewardInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/DispatchTask/ghostreconRewardUI.prefab");
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
					dispatchRewardInst.gameObject.SetActive(value: false);
					dispatchRewardInst.gameObject.transform.SetParent(gameObject.transform);
					dispatchRewardInst.gameObject.transform.localPosition = new Vector3(0f, 6f, 0f);
					CheckShowRewardUI();
					UpdateDetectEventActive();
					Transform transform = dispatchRewardInst.gameObject.transform;
					bubbleTouchEvent = transform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
					if (bubbleTouchEvent != null)
					{
						bubbleTouchEvent.onPointerClick = base.OnClickPoint;
					}
				}
			};
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

	public override void RecordBlockIndex(Dictionary<int, int> set)
	{
		base.RecordBlockIndex(set);
	}

	public override void RemoveBlockIndex(Dictionary<int, int> set)
	{
		base.RemoveBlockIndex(set);
	}
}
