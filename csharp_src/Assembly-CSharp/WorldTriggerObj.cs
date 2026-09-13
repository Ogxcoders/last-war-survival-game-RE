using System;
using GameFramework;
using UnityEngine;

public class WorldTriggerObj : IDisposable
{
	public WorldTriggerData triggerData;

	private InstanceRequest requestInst;

	private TouchObjectEventTrigger touchObject;

	public void Dispose()
	{
		if (requestInst != null)
		{
			requestInst.Destroy();
			requestInst = null;
		}
		if (touchObject != null)
		{
			touchObject.onPointerClick = null;
			touchObject.previewName = null;
			touchObject.previewIconPath = null;
			touchObject.previewType = WorldPreviewType.Default;
		}
		triggerData = null;
	}

	public void RefreshData(WorldTriggerData data)
	{
		triggerData = data;
		if (requestInst != null)
		{
			return;
		}
		string prefab = triggerData.config.prefab;
		requestInst = GameEntry.Resource.InstantiateAsync(prefab);
		if (requestInst == null)
		{
			return;
		}
		try
		{
			requestInst.completed += OnGameObjectCreate;
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message + ex.StackTrace);
		}
	}

	private void OnGameObjectCreate(InstanceRequest request)
	{
		GameObject gameObject = request.gameObject;
		gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
		gameObject.transform.position = SceneManager.World.TileIndexToWorld(triggerData.pointId, triggerData.serverId);
		SetClickEvent(gameObject);
	}

	private void SetClickEvent(GameObject go)
	{
		touchObject = go.GetComponent<TouchObjectEventTrigger>();
		if (!(touchObject == null))
		{
			touchObject.onPointerClick = OnClickPoint;
			touchObject.previewIconPath = triggerData.config.icon;
			touchObject.previewName = GameEntry.Localization.GetString(triggerData.config.name, triggerData.config.level);
			touchObject.previewType = WorldPreviewType.WorldTrigger;
		}
	}

	protected void OnClickPoint()
	{
		if (SceneManager.World.GetLodLevel() >= 3)
		{
			Vector3 lookat = SceneManager.World.TileIndexToWorld(triggerData.pointId, triggerData.serverId);
			SceneManager.World.AutoLookat(lookat, SceneManager.World.InitZoom);
		}
		else
		{
			GameEntry.Lua.Call("UIUtil.OnClickWorld", triggerData.pointId, ClickWorldType.Collider, triggerData.uuid);
		}
	}
}
