using UnityEngine;

public class DetectEventSuppliesSearchObject : WorldPointObject
{
	public string eventId;

	private SimpleAnimation _animation;

	public DetectEventSuppliesSearchObject(WorldScene worldScene, int pointIndex, int pType)
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
	}

	public virtual void AsyncCompleteCallBack(InstanceRequest instance)
	{
		gameObject = instance.gameObject;
		if (gameObject != null)
		{
			gameObject.name = "DetectEventSuppliesSearch_" + pointIndex;
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
			SetClickEvent();
		}
	}

	public override void Destroy()
	{
		base.Destroy();
	}
}
