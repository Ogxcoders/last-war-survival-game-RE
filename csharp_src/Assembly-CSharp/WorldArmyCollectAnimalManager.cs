using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class WorldArmyCollectAnimalManager : WorldManagerBase
{
	public class Param
	{
		public int pointIndex;

		public int targetPointId;

		public ResourceType resourceType;

		public long uuid;

		public InstanceRequest request;

		public CollectArmyAnimalModel collectAnimalModel;
	}

	private Dictionary<long, Param> _animDict = new Dictionary<long, Param>();

	public WorldArmyCollectAnimalManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		AddListener();
	}

	public override void UnInit()
	{
		RemoveListener();
		foreach (Param value in _animDict.Values)
		{
			if (value.collectAnimalModel != null)
			{
				value.collectAnimalModel.UnInit();
			}
			if (value.request != null)
			{
				value.request.Destroy();
			}
		}
		_animDict.Clear();
	}

	private void AddListener()
	{
	}

	private void RemoveListener()
	{
	}

	public void CreateAnimalObject(long MarchUuid, int resPointId)
	{
		WorldMarch march = world.GetMarch(MarchUuid);
		CollectPointInfo collectRangePoint = world.GetCollectRangePoint(resPointId);
		if (collectRangePoint == null)
		{
			return;
		}
		Param param = new Param
		{
			pointIndex = march.targetPos,
			targetPointId = collectRangePoint.mainIndex,
			resourceType = collectRangePoint.resourceType,
			uuid = MarchUuid
		};
		if (_animDict.ContainsKey(MarchUuid))
		{
			if (_animDict[MarchUuid].collectAnimalModel != null)
			{
				_animDict[MarchUuid].collectAnimalModel.Init(param);
			}
			return;
		}
		_animDict.Add(MarchUuid, param);
		InstanceRequest robotInstance = GameEntry.Resource.InstantiateAsync(GetModelName(MarchUuid));
		param.request = robotInstance;
		robotInstance.completed += delegate
		{
			GameObject gameObject = robotInstance.gameObject;
			gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			CollectArmyAnimalModel component = gameObject.GetComponent<CollectArmyAnimalModel>();
			component.Init(param);
			if (_animDict.ContainsKey(param.uuid))
			{
				_animDict[param.uuid].collectAnimalModel = component;
			}
		};
	}

	public void DestroyAnimalObject(long MarchUuid)
	{
		if (_animDict.ContainsKey(MarchUuid))
		{
			if (_animDict[MarchUuid].collectAnimalModel != null)
			{
				_animDict[MarchUuid].collectAnimalModel.UnInit();
			}
			if (_animDict[MarchUuid].request != null)
			{
				_animDict[MarchUuid].request.Destroy();
			}
			_animDict.Remove(MarchUuid);
		}
	}

	private string GetModelName(long marchUuid)
	{
		if (marchUuid != 0L)
		{
			WorldMarch march = world.GetMarch(marchUuid);
			if (march != null)
			{
				if (march.IsMine())
				{
					Log.Info("CollectArmyAnimalModel : CollectArmyAnimalModel : uuid : {0}", marchUuid);
					return "Assets/Main/Prefabs/CollectResource/CollectArmyAnimalModel.prefab";
				}
				if (march.allianceUid == GameEntry.Data.Player.GetAllianceId())
				{
					Log.Info("CollectArmyAnimalModel : CollectArmyAnimalModelAlliance : uuid : {0}", marchUuid);
					return "Assets/Main/Prefabs/CollectResource/CollectArmyAnimalModelAlliance.prefab";
				}
			}
		}
		Log.Info("CollectArmyAnimalModel : CollectArmyAnimalModelEnemy : uuid : {0}", marchUuid);
		return "Assets/Main/Prefabs/CollectResource/CollectArmyAnimalModelEnemy.prefab";
	}
}
