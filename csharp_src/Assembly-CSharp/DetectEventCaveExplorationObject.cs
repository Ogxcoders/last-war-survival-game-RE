using UnityEngine;

public class DetectEventCaveExplorationObject : WorldPointObject
{
	public string eventId;

	private SimpleAnimation _animation;

	public DetectEventCaveExplorationObject(WorldScene worldScene, int pointIndex, int pType)
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
			SetClickEvent();
			if (!(_touchObject == null))
			{
				string templateData2 = GameEntry.ConfigCache.GetTemplateData("detect_event", eventId.ToInt(), "name");
				_touchObject.previewName = GameEntry.Localization.GetString(templateData2);
				_touchObject.previewType = WorldPreviewType.DetectCaveExplore;
			}
		}
	}

	public override void Destroy()
	{
		base.Destroy();
	}
}
