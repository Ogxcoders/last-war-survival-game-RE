using UnityEngine;

public class WorldZoneMobilizationObject : WorldPointObject
{
	private int pointId;

	private long uuid;

	private WorldZoneMobilizationPointInfo pointInfo;

	private Transform transform;

	private string worldModelPath = "Model/WorldModel";

	public WorldZoneMobilizationObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		if (world.GetPointInfo(pointIndex) is WorldZoneMobilizationPointInfo worldZoneMobilizationPointInfo)
		{
			serverId = worldZoneMobilizationPointInfo.serverId;
			pointInfo = worldZoneMobilizationPointInfo;
		}
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		Create();
	}

	public override void Destroy()
	{
		base.Destroy();
		GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveZMBuildingCtrl", uuid);
	}

	public override void SetAutoAdjustLod()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			serverId = pointInfo.serverId;
			adjuster = gameObject.GetComponent<AutoAdjustLod>();
			if (adjuster == null)
			{
				adjuster = gameObject.AddComponent<AutoAdjustLod>();
			}
			adjuster.SetLodType(LodType.Resource);
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			serverId = pointInfo.serverId;
			pointId = pointInfo.mainIndex;
			uuid = pointInfo.uuid;
			this.pointInfo = pointInfo as WorldZoneMobilizationPointInfo;
			GameEntry.Lua.Call("CSharpCallLuaInterface.OnWorldPointInfoChanged", uuid);
		}
	}

	private void Create()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo == null)
		{
			return;
		}
		serverId = pointInfo.serverId;
		pointId = pointInfo.mainIndex;
		uuid = pointInfo.uuid;
		this.pointInfo = pointInfo as WorldZoneMobilizationPointInfo;
		AddOldObject();
		string modelPath = this.pointInfo.GetModelPath();
		instance = GameEntry.Resource.InstantiateAsync(modelPath);
		instance.completed += delegate
		{
			ClearOldObject();
			gameObject = instance.gameObject;
			if (!(gameObject == null))
			{
				this.transform = gameObject.transform;
				this.transform.SetParent(world.DynamicObjNode);
				this.transform.position = base.WorldPosition;
				gameObject.SetActive(isVisible);
				SetAutoAdjustLod();
				SetClickEvent();
				Transform transform = this.transform.Find("Icon/IconSprite");
				if (transform != null)
				{
					SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
					if (component != null)
					{
						string iconPath = this.pointInfo.GetIconPath();
						if (!string.IsNullOrEmpty(iconPath))
						{
							component.LoadSprite(iconPath);
						}
					}
				}
				Transform transform2 = this.transform.Find("Model/ModelLabel/Name/NameText");
				if (!(transform2 == null))
				{
					TextMeshProEx component2 = transform2.GetComponent<TextMeshProEx>();
					if (!(component2 == null))
					{
						string name = this.pointInfo.GetName();
						if (!string.IsNullOrEmpty(name))
						{
							component2.text = name;
						}
						GameEntry.Lua.Call("CSharpCallLuaInterface.CreateZMBuildingCtrl", uuid, this.transform);
					}
				}
			}
		};
	}
}
