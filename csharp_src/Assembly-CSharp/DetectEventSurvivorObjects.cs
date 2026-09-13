using UnityEngine;

public class DetectEventSurvivorObjects : WorldPointObject
{
	public string eventId;

	private SimpleAnimation _animation;

	public DetectEventSurvivorObjects(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		if (world.GetPointInfo(pointIndex) is ExplorePointInfo explorePointInfo)
		{
			eventId = explorePointInfo.eventId;
		}
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		string prefabPath = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetWorldExploreDetectEventModel", eventId);
		instance = GameEntry.Resource.InstantiateAsync(prefabPath);
		instance.completed += AsyncCompleteCallBack;
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			GameEntry.Event.Fire(EventId.IcePointObjectIn, pointInfo.uuid);
		}
		GameEntry.Event.Subscribe(EventId.PointThermalRefresh, PointThermalRefreshCallBack);
	}

	public virtual void AsyncCompleteCallBack(InstanceRequest instance)
	{
		gameObject = instance.gameObject;
		if (gameObject != null)
		{
			gameObject.name = "DetectEventSurvivor_" + pointIndex;
			gameObject.transform.SetParent(world.DynamicObjNode);
			gameObject.transform.position = base.WorldPosition;
			gameObject.SetActive(isVisible);
			gameObject.transform.localScale = Vector3.one;
			SuperTextMesh component = gameObject.transform.Find("Model/nameContent/name").GetComponent<SuperTextMesh>();
			string templateData = GameEntry.ConfigCache.GetTemplateData("detect_event", eventId.ToInt(), "name");
			component.text = GameEntry.Localization.GetString(templateData);
			_animation = gameObject.GetComponentInChildren<SimpleAnimation>();
			PointInfo pointInfo = world.GetPointInfo(pointIndex);
			if (pointInfo != null && _animation != null)
			{
				_animation.enabled = !pointInfo.IsFrozen();
			}
			SetAutoAdjustLod();
			CheckShowTroopDestination();
			SetClickEvent();
			CheckShowTemperature();
			SetMultiSelectTypeForRadar(eventId.ToInt());
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null && _animation != null)
		{
			_animation.enabled = !pointInfo.IsFrozen();
		}
	}

	private void PointThermalRefreshCallBack(object pUuid)
	{
		if (pUuid is long num)
		{
			PointInfo pointInfo = world.GetPointInfo(pointIndex);
			if (pointInfo != null && pointInfo.uuid == num && pointInfo != null && _animation != null)
			{
				_animation.enabled = !pointInfo.IsFrozen();
			}
		}
	}

	public override void Destroy()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			GameEntry.Event.Fire(EventId.IcePointObjectOut, pointInfo.uuid);
		}
		GameEntry.Event.Unsubscribe(EventId.PointThermalRefresh, PointThermalRefreshCallBack);
		base.Destroy();
	}
}
