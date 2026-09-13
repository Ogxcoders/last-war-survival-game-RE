using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using XLua;

public class CityCollectAnimalManager : CityManagerBase
{
	private Dictionary<long, CollectAnimalModel.Param> _animDict = new Dictionary<long, CollectAnimalModel.Param>();

	public CityCollectAnimalManager(CityScene scene)
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
		foreach (CollectAnimalModel.Param value in _animDict.Values)
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
		GameEntry.Event.Subscribe(EventId.BuildConnect, BuildConnectSignal);
		GameEntry.Event.Subscribe(EventId.BuildLackConnect, BuildLackConnectSignal);
		GameEntry.Event.Subscribe(EventId.UPDATE_BUILD_DATA, BuildDataUpdateSignal);
	}

	private void RemoveListener()
	{
		GameEntry.Event.Unsubscribe(EventId.BuildConnect, BuildConnectSignal);
		GameEntry.Event.Unsubscribe(EventId.BuildLackConnect, BuildLackConnectSignal);
		GameEntry.Event.Unsubscribe(EventId.UPDATE_BUILD_DATA, BuildDataUpdateSignal);
	}

	public void CreateAnimalObject(long uuid)
	{
		LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(uuid);
		if (buildingDataByUuid == null || buildingDataByUuid.level <= 0 || buildingDataByUuid.state != 0 || GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetResourceTypeByBuildId", buildingDataByUuid.buildId) == -1)
		{
			return;
		}
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCollectRangeInfoByIndex", buildingDataByUuid.pointId);
		if (luaTable == null)
		{
			return;
		}
		CollectAnimalModel.Param param = new CollectAnimalModel.Param
		{
			pointIndex = buildingDataByUuid.pointId,
			targetPointId = luaTable.Get<int>("pointId"),
			buildId = buildingDataByUuid.buildId,
			uuid = uuid
		};
		if (_animDict.ContainsKey(uuid))
		{
			if (_animDict[uuid].collectAnimalModel != null)
			{
				_animDict[uuid].collectAnimalModel.Init(param);
			}
			return;
		}
		_animDict.Add(uuid, param);
		InstanceRequest robotInstance = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/CollectResource/CollectAnimalModel.prefab");
		Log.Info("CollectAnimalModel : CityCollectAnimalManager : uuid : {0}", uuid);
		param.request = robotInstance;
		robotInstance.completed += delegate
		{
			GameObject gameObject = robotInstance.gameObject;
			gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			CollectAnimalModel component = gameObject.GetComponent<CollectAnimalModel>();
			component.Init(param);
			if (_animDict.ContainsKey(param.uuid))
			{
				_animDict[param.uuid].collectAnimalModel = component;
			}
		};
	}

	public void DestroyAnimalObject(long uuid)
	{
		if (_animDict.ContainsKey(uuid))
		{
			if (_animDict[uuid].collectAnimalModel != null)
			{
				_animDict[uuid].collectAnimalModel.UnInit();
			}
			if (_animDict[uuid].request != null)
			{
				_animDict[uuid].request.Destroy();
			}
			_animDict.Remove(uuid);
		}
	}

	private void BuildConnectSignal(object userData)
	{
		long uuid = (long)userData;
		RefreshState(uuid);
	}

	private void BuildLackConnectSignal(object userData)
	{
		long uuid = (long)userData;
		RefreshState(uuid);
	}

	private void BuildDataUpdateSignal(object userData)
	{
		long uuid = (long)userData;
		RefreshState(uuid);
	}

	private void RefreshState(long uuid)
	{
		if (_animDict.ContainsKey(uuid))
		{
			if (_animDict[uuid].collectAnimalModel != null)
			{
				_animDict[uuid].collectAnimalModel.RefreshState();
			}
		}
		else
		{
			CreateAnimalObject(uuid);
		}
	}
}
