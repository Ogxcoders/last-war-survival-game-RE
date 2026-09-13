using System;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using UnityEngine;

public class WorldDetectEventItemObject : WorldPointObject
{
	protected GameObject model;

	protected GameObject icon;

	protected InstanceRequest detectEventInst;

	protected InstanceRequest pickGarbageInst;

	protected InstanceRequest colloctEffectObject;

	protected long pickupEndTime;

	protected long pickupStartTime;

	protected SuperTextMesh timeText;

	protected readonly string eventId;

	protected bool IsCollect;

	private bool isDisappearPlayEnd;

	private BoxCollider _boxCollider;

	protected string collectEffectPath = "Assets/_Art/Effect/prefab/scene/Common/VFX_jianlaji.prefab";

	private bool detectEventActiveCache = true;

	protected bool needShowCollectEffect = true;

	protected bool needUpdateProgress = true;

	public WorldDetectEventItemObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		eventId = GetEventId();
		IsCollect = false;
		isDisappearPlayEnd = false;
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
		UpdateProgress();
	}

	public virtual bool DoDisappear()
	{
		if ((bool)model)
		{
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

	protected virtual bool NeedShowDetectEventIcon()
	{
		return false;
	}

	protected virtual bool NeedShowTime()
	{
		return false;
	}

	protected virtual string GetEventId()
	{
		return "";
	}

	protected virtual string GetModePath()
	{
		return "Assets/Main/Prefabs/Sample/sample.prefab";
	}

	public void UpdateProgress()
	{
		long milliSecond = 0L;
		if (!(timeText == null) && pickGarbageInst != null && needUpdateProgress)
		{
			if (pickupEndTime > GameEntry.Timer.GetServerTime())
			{
				milliSecond = pickupEndTime - GameEntry.Timer.GetServerTime();
			}
			timeText.text = GameEntry.Timer.MilliSecondToFmtStringHighestTime(milliSecond);
		}
	}

	private void DoWhenCollectStart(object userData)
	{
		long pointUid = GetPointUid();
		if ((long)userData == pointUid)
		{
			if (needShowCollectEffect)
			{
				ShowCollectParticle();
			}
			IsCollect = true;
		}
	}

	protected virtual long GetPointUid()
	{
		return 0L;
	}

	protected virtual void DoWhenMarchInfoChange(object userData)
	{
		if (!NeedShowTime())
		{
			if (pickGarbageInst != null)
			{
				pickGarbageInst.Destroy();
				pickGarbageInst = null;
				HideCollectParticle();
			}
			detectEventActiveCache = true;
			UpdateDetectEventActive();
			if (detectEventInst != null && detectEventInst.gameObject != null)
			{
				detectEventInst.gameObject.SetActive(value: true);
			}
			return;
		}
		if (pickGarbageInst == null)
		{
			pickGarbageInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/CollectGarbageUI.prefab");
			pickGarbageInst.completed += delegate
			{
				pickGarbageInst.gameObject.transform.SetParent(gameObject.transform);
				pickGarbageInst.gameObject.transform.localPosition = Vector3.zero;
				timeText = pickGarbageInst.gameObject.transform.Find("PosGo/TimeText").GetComponent<SuperTextMesh>();
				timeText.gameObject.SetActive(value: false);
				pickGarbageInst.gameObject.GetComponent<ChangeSceneCircleSlider>().Init(pickupStartTime, pickupEndTime);
				UpdateProgress();
			};
		}
		detectEventActiveCache = false;
		UpdateDetectEventActive();
		if (detectEventInst != null && detectEventInst.gameObject != null)
		{
			detectEventInst.gameObject.SetActive(value: false);
		}
	}

	public override void Destroy()
	{
		if (detectEventInst != null)
		{
			detectEventInst.Destroy();
			detectEventInst = null;
		}
		if (pickGarbageInst != null)
		{
			pickGarbageInst.Destroy();
			pickGarbageInst = null;
		}
		if (colloctEffectObject != null)
		{
			colloctEffectObject.Destroy();
			colloctEffectObject = null;
		}
		if (SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH)
		{
			GameEntry.Event.Unsubscribe(EventId.MarchItemUpdateSelf, DoWhenMarchInfoChange);
		}
		GameEntry.Event.Unsubscribe(EventId.GarbageCollectStart, DoWhenCollectStart);
		base.Destroy();
	}

	public override void UpdateSelfMarch(object o)
	{
		base.UpdateSelfMarch(o);
		if (!SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH)
		{
			DoWhenMarchInfoChange(o);
		}
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		AddOldObject();
		instance = GameEntry.Resource.InstantiateAsync(GetModePath());
		instance.completed += AsyncCompleteCallBack;
		if (SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH)
		{
			GameEntry.Event.Subscribe(EventId.MarchItemUpdateSelf, DoWhenMarchInfoChange);
		}
		GameEntry.Event.Subscribe(EventId.GarbageCollectStart, DoWhenCollectStart);
	}

	public virtual void AsyncCompleteCallBack(InstanceRequest instance)
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
			_boxCollider = gameObject.transform.GetComponent<BoxCollider>();
			Transform transform = gameObject.transform.Find("Model");
			if (transform != null)
			{
				model = transform.gameObject;
				model.transform.localScale = Vector3.one;
			}
			Transform transform2 = gameObject.transform.Find("Icon");
			if (transform2 != null)
			{
				icon = transform2.gameObject;
				icon.SetActive(value: false);
			}
			SetAutoAdjustLod();
			ShowDetectEvent();
			DoWhenMarchInfoChange(null);
			DoWhenCreateComplete(model);
			CheckShowTroopDestination();
			SetClickEvent();
			SetMultiSelect();
			MoveDetectObject(gameObject, pointIndex);
		}
	}

	public void MoveDetectObject(GameObject Obj, int pointIndex)
	{
		if (string.IsNullOrEmpty(eventId))
		{
			Debug.LogError("eventId is null");
		}
		else if (GameEntry.ConfigCache.GetTemplateData("detect_event", eventId.ToInt(), "type").ToInt() == 46)
		{
			ObjectMover.MoveFromSpawn(Obj, eventId.ToInt(), pointIndex);
		}
	}

	public void SetMultiSelect()
	{
		SetMultiSelectTypeForRadar(eventId.ToInt());
	}

	protected virtual void DoWhenCreateComplete(GameObject model)
	{
	}

	public List<Vector3> GetPickPoint(Vector3 startPt)
	{
		List<Vector3> list = new List<Vector3>();
		if (model != null && model.gameObject != null)
		{
			float num = 1.5f;
			int i = 0;
			int num2 = 5;
			float num3 = 360 / num2;
			double x = startPt.x - model.transform.position.x;
			double num4 = Math.Atan2(startPt.z - model.transform.position.z, x) * 180.0 / Math.PI;
			for (; i < num2; i++)
			{
				double num5 = (double)(num3 * (float)i) + num4;
				double num6 = (double)num * Math.Cos(num5 * Math.PI / 180.0);
				double num7 = (double)num * Math.Sin(num5 * Math.PI / 180.0);
				Vector3 item = model.transform.position + new Vector3((float)num6, 0f, (float)num7);
				list.Add(item);
			}
		}
		return list;
	}

	public Vector2 GetIntersection(Vector2 lineFirstStar, Vector2 lineFirstEnd, Vector2 lineSecondStar, Vector2 lineSecondEnd)
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		if (lineFirstStar.x != lineFirstEnd.x)
		{
			num = (lineFirstEnd.y - lineFirstStar.y) / (lineFirstEnd.x - lineFirstStar.x);
			num3 |= 1;
		}
		if (lineSecondStar.x != lineSecondEnd.x)
		{
			num2 = (lineSecondEnd.y - lineSecondStar.y) / (lineSecondEnd.x - lineSecondStar.x);
			num3 |= 2;
		}
		switch (num3)
		{
		case 0:
			Log.Error("wrong position");
			return new Vector2(0f, 0f);
		case 1:
		{
			float x = lineSecondStar.x;
			float y2 = (lineFirstStar.x - x) * (0f - num) + lineFirstStar.y;
			return new Vector2(x, y2);
		}
		case 2:
		{
			float x2 = lineFirstStar.x;
			float y3 = (lineSecondStar.x - x2) * (0f - num2) + lineSecondStar.y;
			return new Vector2(x2, y3);
		}
		case 3:
		{
			if (num == num2)
			{
				Log.Error("wrong position1");
				return new Vector2(0f, 0f);
			}
			float num4 = (num * lineFirstStar.x - num2 * lineSecondStar.x - lineFirstStar.y + lineSecondStar.y) / (num - num2);
			float y = num * num4 - num * lineFirstStar.x + lineFirstStar.y;
			return new Vector2(num4, y);
		}
		default:
			return new Vector2(0f, 0f);
		}
	}

	private void ShowCollectParticle()
	{
		if (colloctEffectObject != null)
		{
			if (colloctEffectObject.gameObject != null)
			{
				colloctEffectObject.gameObject.SetActive(value: true);
			}
			return;
		}
		colloctEffectObject = GameEntry.Resource.InstantiateAsync(collectEffectPath);
		colloctEffectObject.completed += delegate
		{
			GameObject gameObject = colloctEffectObject.gameObject;
			gameObject.SetActive(value: true);
			Transform transform = base.gameObject.transform;
			if (transform != null)
			{
				gameObject.transform.SetParent(transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
			}
		};
	}

	protected void HideCollectParticle()
	{
		if (colloctEffectObject != null)
		{
			colloctEffectObject.Destroy();
			colloctEffectObject = null;
		}
	}

	protected void ShowDetectEvent()
	{
		if (!NeedShowDetectEventIcon())
		{
			return;
		}
		detectEventInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/WorldDetectInfo.prefab");
		detectEventInst.completed += delegate
		{
			if (!(detectEventInst.gameObject == null))
			{
				detectEventInst.gameObject.SetActive(value: true);
				detectEventInst.gameObject.transform.SetParent(gameObject.transform);
				detectEventInst.gameObject.transform.localPosition = Vector3.zero;
				SpriteRenderer component = detectEventInst.gameObject.transform.Find("Transform/Detect_event_quality_icon").GetComponent<SpriteRenderer>();
				SpriteRenderer component2 = detectEventInst.gameObject.transform.Find("Transform/Detect_event_icon").GetComponent<SpriteRenderer>();
				float y = GameEntry.Lua.CallWithReturn<float, string>("CSharpCallLuaInterface.GetWorldDetectIconHighById", eventId);
				string spritePath = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetWorldDetectBgById", eventId);
				string spritePath2 = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetWorldDetectIconById", eventId);
				component2.transform.localPosition = new Vector3(0f, y, 0f);
				component.LoadSprite(spritePath);
				component2.LoadSprite(spritePath2);
				if (pickGarbageInst != null)
				{
					detectEventActiveCache = false;
					UpdateDetectEventActive();
				}
				Transform transform = detectEventInst.gameObject.transform;
				bubbleTouchEvent = transform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
				if (bubbleTouchEvent != null)
				{
					bubbleTouchEvent.onPointerClick = base.OnClickPoint;
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
}
