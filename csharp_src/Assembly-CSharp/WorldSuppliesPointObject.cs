using UnityEngine;

public class WorldSuppliesPointObject : WorldPointObject
{
	private UIWorldLabel[] cityLabels;

	protected InstanceRequest bubbleTip;

	private Transform _rootTransform;

	private Transform _botTransform;

	private SuperTextMesh _stealText;

	private Transform _eventIcon;

	private Transform _eventIconGray;

	private SuperTextMesh _timeText;

	private int _bubbleState = -1;

	private long _endTime = -1L;

	private int _limit;

	private long _uuid;

	public WorldSuppliesPointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		CreateObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			_uuid = pointInfo.uuid;
			GameEntry.Event.Fire(EventId.IcePointObjectIn, pointInfo.uuid);
		}
	}

	public void CreateObject()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo == null)
		{
			return;
		}
		AddOldObject();
		WorldSuppliesPoint point;
		if ((point = pointInfo as WorldSuppliesPoint) == null || point.configId <= 0)
		{
			return;
		}
		string templateData = GameEntry.ConfigCache.GetTemplateData("ice_supplies", point.configId, "model_name");
		string templateData2 = GameEntry.ConfigCache.GetTemplateData("ice_supplies", point.configId, "limit");
		_limit = templateData2.ToInt();
		if (templateData.IsNullOrEmpty())
		{
			return;
		}
		instance = GameEntry.Resource.InstantiateAsync(templateData);
		instance.completed += delegate
		{
			gameObject = instance.gameObject;
			cityLabels = gameObject.GetComponentsInChildren<UIWorldLabel>(includeInactive: true);
			if (gameObject != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = base.WorldPosition;
			}
			string templateData3 = GameEntry.ConfigCache.GetTemplateData("ice_supplies", point.configId, "name");
			templateData3 = GameEntry.Localization.GetString(templateData3);
			string templateData4 = GameEntry.ConfigCache.GetTemplateData("ice_supplies", point.configId, "level");
			SetClickEvent();
			if ((bool)_touchObject)
			{
				string previewIconPath = "Assets/Main/Sprites/LodIcon/zyf_wujisuofang_shoujiwuzi.png";
				_touchObject.previewIconPath = previewIconPath;
				_touchObject.previewType = WorldPreviewType.AllianceCity;
				_touchObject.previewName = templateData3;
			}
			UIWorldLabel[] array = cityLabels;
			foreach (UIWorldLabel uIWorldLabel in array)
			{
				uIWorldLabel.ShowFlag(s: false);
				uIWorldLabel.SetName(templateData3, GameDefines.CityLabelColorType.White);
				if (templateData4.ToInt() <= 0)
				{
					uIWorldLabel.SetLevel(1);
				}
				else
				{
					uIWorldLabel.SetLevel(templateData4.ToInt());
				}
			}
			RefreshBubble();
		};
	}

	private void RefreshState()
	{
		if (!(world.GetPointInfo(pointIndex) is WorldSuppliesPoint worldSuppliesPoint) || !_stealText || !_eventIconGray || !_eventIcon)
		{
			return;
		}
		int num = GameEntry.ConfigCache.GetTemplateData("ice_supplies", worldSuppliesPoint.configId, "total_limit").ToInt();
		int limit = _limit;
		int num2 = 0;
		int num3 = 0;
		if (worldSuppliesPoint.uidList != null)
		{
			num3 = worldSuppliesPoint.uidList.Count;
			foreach (string uid in worldSuppliesPoint.uidList)
			{
				if (uid == GameEntry.Data.Player.Uid)
				{
					num2++;
				}
			}
		}
		_bubbleState = 0;
		if (num <= num3)
		{
			_bubbleState = 1;
		}
		else
		{
			_bubbleState = ((num2 >= limit) ? 2 : 0);
		}
		int num4 = num - num3;
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (_bubbleState == 0)
		{
			_eventIcon.gameObject.SetActive(value: true);
			_eventIconGray.gameObject.SetActive(value: false);
			_stealText.text = $"{num4}/{num}";
		}
		else if (_bubbleState == 1)
		{
			_eventIcon.gameObject.SetActive(value: false);
			_eventIconGray.gameObject.SetActive(value: true);
			_stealText.text = $"{num4}/{num}";
		}
		else if (_bubbleState == 2)
		{
			_eventIcon.gameObject.SetActive(value: false);
			_eventIconGray.gameObject.SetActive(value: true);
			_stealText.text = $"{num4}/{num}";
		}
		_endTime = worldSuppliesPoint.workEndTime;
		_timeText.gameObject.SetActive(_endTime > 0);
		string templateData = GameEntry.ConfigCache.GetTemplateData("ice_supplies", worldSuppliesPoint.configId, "ui_y_delta");
		if (!string.IsNullOrEmpty(templateData) && float.TryParse(templateData, out var result))
		{
			_rootTransform.localPosition = new Vector3(0f, result, 0f);
		}
		else
		{
			_rootTransform.localPosition = new Vector3(0f, 0f, 0f);
		}
		string templateData2 = GameEntry.ConfigCache.GetTemplateData("ice_supplies", worldSuppliesPoint.configId, "ui_y_delta_bot");
		if (!string.IsNullOrEmpty(templateData2) && float.TryParse(templateData2, out var result2))
		{
			_botTransform.localPosition = new Vector3(0f, result2, 0f);
		}
		else
		{
			_botTransform.localPosition = new Vector3(0f, 0f, 0f);
		}
	}

	protected override void OnClickWorldPointObject()
	{
		Debug.Log("click supplies");
		if (SceneManager.IsInWorld())
		{
			GameEntry.Lua.Call("UIUtil.OnClickWorld", pointIndex, 1);
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		RefreshState();
		GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateWorldPointGameObject", _uuid, gameObject?.transform);
	}

	private void RefreshBubble()
	{
		PointInfo info = world.GetPointInfo(pointIndex);
		if (info is WorldSuppliesPoint worldSuppliesPoint && GameEntry.ConfigCache.GetTemplateData("ice_supplies", worldSuppliesPoint.configId, "type") == "5" && worldSuppliesPoint.discovererUid != GameEntry.Data.Player.Uid)
		{
			return;
		}
		bubbleTip = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/DispatchTask/IceSuppliesRewardUI.prefab");
		bubbleTip.completed += delegate(InstanceRequest req)
		{
			if (gameObject == null || req.gameObject == null)
			{
				if (bubbleTip != null)
				{
					bubbleTip.Destroy();
					bubbleTip = null;
				}
			}
			else
			{
				bubbleTip.gameObject.SetActive(value: true);
				_rootTransform = bubbleTip.gameObject.transform;
				_rootTransform.SetParent(gameObject.transform);
				_rootTransform.localPosition = Vector3.zero;
				_stealText = _rootTransform.Find("Transform/StealText").GetComponent<SuperTextMesh>();
				_eventIcon = _rootTransform.Find("Transform/Detect_event_icon");
				_eventIconGray = _rootTransform.Find("Transform/Detect_event_icon_gray");
				_botTransform = _rootTransform.Find("bot");
				_timeText = _botTransform.Find("TimeText").GetComponent<SuperTextMesh>();
				bubbleTouchEvent = _rootTransform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
				if (bubbleTouchEvent != null)
				{
					bubbleTouchEvent.onPointerClick = BubbleClick;
				}
				RefreshState();
				CheckShowTemperature();
				GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateWorldPointGameObject", info.uuid, gameObject.transform);
			}
		};
	}

	private void BubbleClick()
	{
		if (_bubbleState == 0)
		{
			OnClickWorldPointObject();
		}
		else if (_bubbleState == 1)
		{
			UIUtils.ShowTips("season_s2_ice_supplies_9", 3f);
		}
		else if (_bubbleState == 2)
		{
			UIUtils.ShowTips("season_s2_ice_supplies_1", _limit);
		}
	}

	private void UpdateTime()
	{
		if (_endTime > 0)
		{
			long num = _endTime - GameEntry.Timer.GetServerTime();
			if (num <= 0)
			{
				_endTime = 0L;
				_timeText.gameObject.SetActive(value: false);
			}
			else
			{
				_timeText.text = GameEntry.Timer.MillisecondToSecondString(num, ":");
			}
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		UpdateTime();
	}

	public override void Destroy()
	{
		GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveWorldPointGameObject", _uuid);
		if (bubbleTip != null)
		{
			bubbleTip.Destroy();
			bubbleTip = null;
		}
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			GameEntry.Event.Fire(EventId.IcePointObjectOut, pointInfo.uuid);
		}
		cityLabels = null;
		_stealText = null;
		_eventIcon = null;
		_eventIconGray = null;
		_bubbleState = -1;
		_limit = 0;
		base.Destroy();
	}
}
