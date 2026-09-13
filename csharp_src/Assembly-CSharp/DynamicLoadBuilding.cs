using System;
using UnityEngine;

public class DynamicLoadBuilding
{
	public enum ModelType
	{
		City,
		World
	}

	private int skinId;

	private IDynamicLoadBuilding host;

	private string modelPath = string.Empty;

	private InstanceRequest modelRequest;

	private Transform dynamicModelNode;

	private GameObject[] defaultItems;

	private Action onLoaded;

	public void Init(IDynamicLoadBuilding host, int skinId, ModelType type, GameObject normalObj, Action onLoaded)
	{
		this.skinId = skinId;
		this.host = host;
		this.onLoaded = onLoaded;
		dynamicModelNode = normalObj.transform.Find("DynamicModel");
		if (dynamicModelNode == null)
		{
			defaultItems = null;
			return;
		}
		int childCount = dynamicModelNode.childCount;
		if (childCount > 0)
		{
			defaultItems = new GameObject[childCount];
			for (int i = 0; i < childCount; i++)
			{
				defaultItems[i] = dynamicModelNode.GetChild(i).gameObject;
			}
		}
		switch (type)
		{
		case ModelType.City:
			modelPath = GameEntry.ConfigCache.GetTemplateData("lw_decoration", skinId, "model_advanced");
			break;
		case ModelType.World:
			modelPath = GameEntry.ConfigCache.GetTemplateData("lw_decoration", skinId, "model_world_advanced");
			break;
		}
	}

	public void LoadDynamicModel()
	{
		if (!(dynamicModelNode == null) && !modelPath.IsNullOrEmpty())
		{
			string prefabPath = "Assets/Main/RemoteRes/PackFolder/" + modelPath + ".prefab";
			if (!GameEntry.Resource.PrefabAssetsDownloaded(prefabPath))
			{
				ShowDefaultItems(show: true);
			}
			else
			{
				ShowDefaultItems(show: false);
			}
			modelRequest = GameEntry.Resource.InstantiateAsync(prefabPath);
			modelRequest.completed += OnDynamicModelLoaded;
		}
	}

	public void UnloadDynamicModel()
	{
		host.OnDynamicModelUnload();
		if (modelRequest != null)
		{
			modelRequest.Destroy();
			modelRequest = null;
		}
	}

	private void ShowDefaultItems(bool show)
	{
		if (defaultItems != null)
		{
			int i = 0;
			for (int num = defaultItems.Length; i < num; i++)
			{
				defaultItems[i].SetActive(show);
			}
		}
	}

	private void OnDynamicModelLoaded(InstanceRequest request)
	{
		ShowDefaultItems(show: false);
		Transform transform = request.gameObject.transform;
		transform.SetParent(dynamicModelNode);
		transform.Reset();
		SimpleAnimation buildingAnim = null;
		Transform child = transform.GetChild(0);
		if (child != null)
		{
			buildingAnim = child.GetComponentInChildren<SimpleAnimation>();
		}
		WorldBuildingAniEffect componentInChildren = transform.GetComponentInChildren<WorldBuildingAniEffect>();
		WorldBuildingAniEffectAni componentInChildren2 = transform.GetComponentInChildren<WorldBuildingAniEffectAni>();
		host.OnDynamicModelLoad(skinId, buildingAnim, componentInChildren, componentInChildren2);
		onLoaded?.Invoke();
	}
}
