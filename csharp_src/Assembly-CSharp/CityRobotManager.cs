using System.Collections.Generic;
using UnityEngine;

public class CityRobotManager : CityManagerBase
{
	private Dictionary<long, MonoBehaviour> _robotDict = new Dictionary<long, MonoBehaviour>();

	private List<long> _destroyList = new List<long>();

	private Dictionary<long, InstanceRequest> _loadingRobot;

	public CityRobotManager(CityScene scene)
		: base(scene)
	{
		_loadingRobot = new Dictionary<long, InstanceRequest>();
	}

	public override void Init()
	{
	}

	public override void UnInit()
	{
		foreach (MonoBehaviour value in _robotDict.Values)
		{
			if (value is WorldBuildRobot worldBuildRobot)
			{
				worldBuildRobot.UnInit();
			}
			if (value is WorldRoadRobot worldRoadRobot)
			{
				worldRoadRobot.UnInit();
			}
		}
		foreach (KeyValuePair<long, InstanceRequest> item in _loadingRobot)
		{
			item.Value.Destroy();
		}
		_robotDict.Clear();
		_loadingRobot.Clear();
	}

	public WorldBuildRobot GetBuildRobot(long bUuid)
	{
		if (_robotDict.TryGetValue(bUuid, out var value) && value is WorldBuildRobot result)
		{
			return result;
		}
		return null;
	}

	public WorldRoadRobot GetRoadRobot(int roodPoint)
	{
		long key = roodPoint;
		if (_robotDict.TryGetValue(key, out var value) && value is WorldRoadRobot result)
		{
			return result;
		}
		return null;
	}

	public void AddRoadRobot(WorldRoadRobot roadRobot)
	{
		if (!_robotDict.ContainsKey(roadRobot.uuid))
		{
			_robotDict.Add(roadRobot.uuid, roadRobot);
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		UpdateRobot(deltaTime);
	}

	private void UpdateRobot(float deltaTime)
	{
		foreach (MonoBehaviour value in _robotDict.Values)
		{
			if (value is WorldBuildRobot worldBuildRobot)
			{
				worldBuildRobot.BotUpdate(deltaTime);
			}
			if (value is WorldRoadRobot { canAutoUpdate: not false } worldRoadRobot)
			{
				worldRoadRobot.BotUpdate(Vector3.zero);
			}
		}
		foreach (long destroy in _destroyList)
		{
			DestroyRobot(destroy);
		}
		_destroyList.Clear();
	}

	public void CreateBuildRobot(long bUuid, Vector3 targetPos, float height, float duration, int tileSizeX, int tileSizeY, bool isTransit = false)
	{
		if (bUuid == 0L)
		{
			return;
		}
		Vector3 pos = targetPos;
		if (_robotDict.ContainsKey(bUuid) || _loadingRobot.ContainsKey(bUuid))
		{
			return;
		}
		LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(477000);
		if (buildingDataByBuildId != null && buildingDataByBuildId.state != 2)
		{
			pos = SceneManager.World.TileIndexToWorld(buildingDataByBuildId.pointId);
		}
		else
		{
			buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
			if (buildingDataByBuildId != null && buildingDataByBuildId.state != 2)
			{
				pos = SceneManager.World.TileIndexToWorld(buildingDataByBuildId.pointId);
			}
		}
		InstanceRequest robotInstance = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/WorldBuildingRobot.prefab");
		_loadingRobot.Add(bUuid, robotInstance);
		robotInstance.completed += delegate
		{
			_loadingRobot.Remove(bUuid);
			GameObject gameObject = robotInstance.gameObject;
			gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			WorldBuildRobot component = gameObject.GetComponent<WorldBuildRobot>();
			component.Init();
			component.StartBuild(bUuid, pos, targetPos, height, duration, tileSizeX, tileSizeY, isTransit);
			_robotDict.Add(bUuid, component);
		};
	}

	public void CreateOtherBuildRobot(long bUuid, Vector3 targetPos, float height, float duration, int tileSizeX, int tileSizeY)
	{
		if (bUuid != 0L && !_robotDict.ContainsKey(bUuid) && !_loadingRobot.ContainsKey(bUuid))
		{
			InstanceRequest robotInstance = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/WorldBuildingRobot.prefab");
			_loadingRobot.Add(bUuid, robotInstance);
			robotInstance.completed += delegate
			{
				_loadingRobot.Remove(bUuid);
				GameObject gameObject = robotInstance.gameObject;
				gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				WorldBuildRobot component = gameObject.GetComponent<WorldBuildRobot>();
				component.Init();
				component.StarBuildOther(bUuid, targetPos, targetPos, height, duration, tileSizeX, tileSizeY);
				_robotDict.Add(bUuid, component);
			};
		}
	}

	public void CreateRoadRobot(List<int> roads, bool isOther, long printId)
	{
		if (!_robotDict.ContainsKey(printId) && !_loadingRobot.ContainsKey(printId))
		{
			InstanceRequest request = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/WorldRoadRobot.prefab");
			_loadingRobot.Add(printId, request);
			request.completed += delegate
			{
				_loadingRobot.Remove(printId);
				GameObject gameObject = request.gameObject;
				gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				WorldRoadRobot component = gameObject.GetComponent<WorldRoadRobot>();
				component.Init(printId);
				component.StartBuild(roads.ToArray(), isOther);
				_robotDict.Add(printId, component);
			};
		}
	}

	private void DestroyRobot(long bUuid)
	{
		if (_robotDict.TryGetValue(bUuid, out var value))
		{
			_robotDict.Remove(bUuid);
			if (value is WorldBuildRobot worldBuildRobot)
			{
				worldBuildRobot.ChangeState(WorldBuildRobot.State.Idle);
				worldBuildRobot.UnInit();
			}
			if (value is WorldRoadRobot worldRoadRobot)
			{
				worldRoadRobot.ChangeState(WorldRoadRobot.State.Idle);
				worldRoadRobot.UnInit();
			}
		}
		if (_loadingRobot.ContainsKey(bUuid))
		{
			_loadingRobot[bUuid].Destroy();
			_loadingRobot.Remove(bUuid);
		}
	}

	public void AddToNeedRemoveList(long bUuid)
	{
		_destroyList.Add(bUuid);
	}

	public void ChangeBuildRobotState(long bUuid, WorldBuildRobot.State state)
	{
		WorldBuildRobot buildRobot = GetBuildRobot(bUuid);
		if (buildRobot != null)
		{
			WorldBuildRobot.State curState = buildRobot.GetCurState();
			if ((uint)(curState - 7) > 3u || state == WorldBuildRobot.State.Idle)
			{
				buildRobot.ChangeState(state);
			}
		}
	}

	public void ChangeBuildRobotFinishTime(long bUuid, float finishTime)
	{
		WorldBuildRobot buildRobot = GetBuildRobot(bUuid);
		if (buildRobot != null)
		{
			buildRobot.ChangeRobotFinishTime(finishTime);
		}
	}
}
