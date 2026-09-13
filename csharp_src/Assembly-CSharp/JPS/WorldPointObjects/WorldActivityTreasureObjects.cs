using GameFramework;
using UnityEngine;

namespace JPS.WorldPointObjects;

public class WorldActivityTreasureObjects : WorldPointObject
{
	private int _configId;

	public WorldActivityTreasureObjects(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		if (world.GetPointInfo(pointIndex) is WorldActivityTreasureInfo worldActivityTreasureInfo)
		{
			_configId = worldActivityTreasureInfo.cfgId;
		}
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		string templateData = GameEntry.ConfigCache.GetTemplateData("activity_world_treasure", _configId, "models");
		if (string.IsNullOrEmpty(templateData))
		{
			Log.Error("WorldActivityTreasureObjects CreateGameObject modelPath is null");
			return;
		}
		string prefabPath = "Assets/Main/ActivityRes/2025EasterMod/Prefabs/Model/" + templateData + ".prefab";
		instance = GameEntry.Resource.InstantiateAsync(prefabPath);
		instance.completed += AsyncCompleteCallBack;
		GameEntry.Event.Subscribe(EventId.PointThermalRefresh, PointThermalRefreshCallBack);
	}

	public virtual void AsyncCompleteCallBack(InstanceRequest instance)
	{
		gameObject = instance.gameObject;
		if (gameObject != null)
		{
			gameObject.name = "WorldTreasure" + pointIndex;
			gameObject.transform.SetParent(world.DynamicObjNode);
			gameObject.transform.position = base.WorldPosition;
			gameObject.SetActive(isVisible);
			gameObject.transform.localScale = Vector3.one;
			Transform transform = gameObject.transform.Find("select");
			if (transform != null)
			{
				bool active = GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.GetIfShowCheckNode", pointIndex);
				transform.gameObject.SetActive(active);
			}
			SetAutoAdjustLod();
			CheckShowTroopDestination();
			SetClickEvent();
			CheckShowTemperature();
			if (_touchObject != null)
			{
				_touchObject.previewName = GameEntry.Localization.GetString("activity_99144_ui_69");
				_touchObject.previewIconPath = "Assets/Main/ActivityRes/2025EasterMod/Sprites/UI/LWUIActEasterEggMain/lrb_daditu_dingwei_caidan.png";
				_touchObject.previewType = WorldPreviewType.Radar;
			}
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
	}

	private void PointThermalRefreshCallBack(object pUuid)
	{
	}

	public override void Destroy()
	{
		GameEntry.Event.Unsubscribe(EventId.PointThermalRefresh, PointThermalRefreshCallBack);
		base.Destroy();
	}
}
