using UnityEngine;

public class DominatorGuideWorldPointObject : WorldPointObject
{
	public string eventId;

	private SimpleAnimation _animation;

	public DominatorGuideWorldPointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		if (world.GetPointInfo(pointIndex) is SamplePointInfo samplePointInfo)
		{
			eventId = samplePointInfo.eventId;
		}
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		string prefabPath = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetWorldExploreDetectEventModel", eventId);
		instance = GameEntry.Resource.InstantiateAsync(prefabPath);
		instance.completed += AsyncCompleteCallBack;
		GameEntry.Event.Subscribe(EventId.PointThermalRefresh, PointThermalRefreshCallBack);
	}

	public virtual void AsyncCompleteCallBack(InstanceRequest instance)
	{
		gameObject = instance.gameObject;
		if (gameObject != null)
		{
			gameObject.name = "DominatorGuide_" + pointIndex;
			gameObject.transform.SetParent(world.DynamicObjNode);
			gameObject.transform.position = base.WorldPosition;
			gameObject.SetActive(isVisible);
			gameObject.transform.localScale = Vector3.one;
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
		GameEntry.Event.Unsubscribe(EventId.PointThermalRefresh, PointThermalRefreshCallBack);
		base.Destroy();
	}
}
