using System.Collections.Generic;
using UnityEngine;

public class WolfShadowObject : WorldPointObject
{
	private GameObject model;

	private GameObject icon;

	private int treasureId = 501;

	private long expireTime;

	private float refreshCD;

	private TextMeshProEx tmpEx;

	public WolfShadowObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
		{
			int.TryParse(treasurePointInfo.eventId, out treasureId);
			expireTime = treasurePointInfo.expireTime;
		}
	}

	protected string GetModePath()
	{
		string text = "";
		int key = treasureId;
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

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		AddOldObject();
		instance = GameEntry.Resource.InstantiateAsync(GetModePath());
		instance.completed += AsyncCompleteCallBack;
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
			Transform transform = gameObject.transform.Find("Model");
			tmpEx = gameObject.transform.Find("ModelGo/Tip/time").GetComponent<TextMeshProEx>();
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
			CheckShowTroopDestination();
			SetClickEvent();
			if (_touchObject != null)
			{
				_touchObject.previewName = GameEntry.Localization.GetString("season_s4_blood_hunter_shadow_name");
				_touchObject.previewIconPath = "Assets/Main/SeasonRes/S4/Sprites/UI/Hunter/ljq_s4_xueselieren_icon_03.png";
				_touchObject.previewType = WorldPreviewType.Radar;
			}
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		if (tmpEx == null)
		{
			return;
		}
		refreshCD -= deltaTime;
		if (refreshCD > 0f)
		{
			return;
		}
		refreshCD = 1f;
		long serverTime = GameEntry.Timer.GetServerTime();
		long num = expireTime - serverTime;
		num = ((num > 0) ? num : 0);
		string text = GameEntry.Timer.MillisecondsToStringWithoutHour(num, ":");
		tmpEx.text = GameEntry.Localization.GetString("season4_supplies_UI_20", text);
		if (num <= 0)
		{
			tmpEx = null;
			if (world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo)
			{
				SeasonHunterRefreshShadowInfoMessage.Instance.Send(treasurePointInfo.uuid, serverId);
			}
		}
	}

	public override void Destroy()
	{
		tmpEx = null;
		base.Destroy();
	}
}
