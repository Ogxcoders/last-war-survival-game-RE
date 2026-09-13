using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class TruckManagerBase
{
	private enum BuildingPositionEnum
	{
		NorthEast,
		NorthWest,
		SouthEast,
		SouthWest
	}

	public List<InstanceRequest> TruckRequest;

	public List<InstanceRequest> PeopleRequest;

	private int _circleTruckCount;

	private float _truckShowTime;

	private float _peopleShowTime;

	private readonly PathManager _pathManager = new PathManager();

	private int _nowRandDirIndex;

	private List<int> _rndTruckIndexList = new List<int>();

	private List<int> _nowGenTruckIndexList = new List<int>();

	private int _alreadyTruckNum;

	private int _alreadyCircleTruckNum;

	private List<int> _rndPeopleIndexList = new List<int>();

	private int _alreadyPeopleNum;

	private float _truckInterval = -1f;

	private int _truckRefreshCountPer = -1;

	private int _circleTruckNum;

	private confMaxCount[] cacheMaxCount1;

	private confMaxCount[] cacheMaxCount2;

	private int domeRange;

	private Vector3 cityMainPos = Vector3.zero;

	private static readonly float[] _circleRadius = new float[2] { 33.95f, 43.95f };

	private static readonly string[] AllTruckPrefabs = new string[7] { "Assets/Main/Prefabs/Vehicle/WorldCityTruck01.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityTruck02.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityTruck03.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityTruck04.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityTruck05.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityTruck06.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityTruck07.prefab" };

	private static readonly string[] AllPeoplePrefabs = new string[8] { "Assets/Main/Prefabs/Vehicle/WorldCityPeople01.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityPeople02.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityPeople03.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityPeople04.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityPeople05.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityPeople06.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityPeople07.prefab", "Assets/Main/Prefabs/Vehicle/WorldCityPeople08.prefab" };

	private List<int> _canShowTruckPrefab = new List<int>();

	private List<int> _canShowPeoplePrefab = new List<int>();

	private bool _canShowTruckAndPeople;

	public long MainBuildUuid;

	private bool isBuildChange;

	private List<LuaBuildData> allBuildDatasCache = new List<LuaBuildData>();

	private Dictionary<int, WorldCityTruck> FreeTruckList { get; set; }

	private Dictionary<int, WorldCityPeople> FreePeopleList { get; set; }

	public TruckManagerBase()
	{
		TruckRequest = new List<InstanceRequest>();
		PeopleRequest = new List<InstanceRequest>();
		UpdateMainBuildCheck();
	}

	public void Destroy()
	{
		ClearAllTruck();
	}

	public void SetBuildCacheChange(bool isChange)
	{
		isBuildChange = isChange;
	}

	private void ShowFreeTruck(List<List<Vector2Int>> path, WorldCityTruck freeTruck, Transform node, string prefab, int truckIndex, bool isCircle, Vector3 mainPos, float radius, float initAngle, bool isInner)
	{
		if (freeTruck == null)
		{
			InstanceRequest temp = GameEntry.Resource.InstantiateAsync(prefab);
			TruckRequest.Add(temp);
			_nowGenTruckIndexList.Add(truckIndex);
			temp.completed += delegate
			{
				GameObject gameObject = temp.gameObject;
				if (!(gameObject == null))
				{
					gameObject.transform.SetParent(node);
					WorldCityTruck component = gameObject.GetComponent<WorldCityTruck>();
					component.Manager = this;
					component.CSInit();
					WorldCityTruck.Param userData2 = new WorldCityTruck.Param
					{
						_pathList = path,
						isRadom = true,
						isCircle = isCircle,
						mainPos = mainPos,
						radius = radius,
						initAngle = initAngle,
						isInner = isInner
					};
					component.CSShow(userData2);
					FreeTruckList.Add(truckIndex, component);
					_nowGenTruckIndexList.Remove(truckIndex);
				}
			};
		}
		else
		{
			WorldCityTruck.Param userData = new WorldCityTruck.Param
			{
				_pathList = path,
				isRadom = true
			};
			freeTruck.CSShow(userData);
		}
	}

	private void ShowFreePeople(List<List<Vector2Int>> path, WorldCityPeople freePeople, Transform node, string prefab, int peopleIndex, WorldCityPeople.CityPeopleType peopleType)
	{
		if (freePeople == null)
		{
			InstanceRequest temp = GameEntry.Resource.InstantiateAsync(prefab);
			PeopleRequest.Add(temp);
			temp.completed += delegate
			{
				GameObject gameObject = temp.gameObject;
				if (!(gameObject == null))
				{
					gameObject.transform.SetParent(node);
					WorldCityPeople component = gameObject.GetComponent<WorldCityPeople>();
					component.Manager = this;
					component.CSInit();
					WorldCityPeople.Param userData2 = new WorldCityPeople.Param
					{
						_pathList = path,
						PeopleType = peopleType,
						index = peopleIndex
					};
					FreePeopleList[peopleIndex] = component;
					component.CSShow(userData2);
					GameEntry.Event.Fire(EventId.CitySolderCreate, peopleIndex);
				}
			};
		}
		else
		{
			WorldCityPeople.Param userData = new WorldCityPeople.Param
			{
				_pathList = path,
				PeopleType = peopleType,
				index = peopleIndex
			};
			freePeople.CSShow(userData);
		}
	}

	public void OnTruckEnterInit(bool isEnter)
	{
		_alreadyTruckNum += ((!isEnter) ? 1 : (-1));
	}

	public void OnPeopleEnterInit(bool isEnter)
	{
		_alreadyPeopleNum += ((!isEnter) ? 1 : (-1));
	}

	private void processCache()
	{
		if (cacheMaxCount1 == null)
		{
			cacheMaxCount1 = parseConfig(GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "city_car", "k1"));
		}
		if (cacheMaxCount2 == null)
		{
			cacheMaxCount2 = parseConfig(GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "city_car", "k2"));
		}
	}

	private confMaxCount[] parseConfig(string s)
	{
		if (s == null)
		{
			Log.Error("parseConfig param null!");
			return new confMaxCount[0];
		}
		string[] array = s.Split(new char[1] { '|' });
		confMaxCount[] array2 = new confMaxCount[s.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string[] array3 = array[i].Split(new char[1] { ';' });
			if (array3.Length == 3)
			{
				array2[i] = new confMaxCount
				{
					minLevel = array3[0].ToInt(),
					maxLevel = array3[1].ToInt(),
					count = array3[2].ToInt()
				};
			}
		}
		return array2;
	}

	private int GetShowMaxCount(bool isTruck)
	{
		int mainLv = GameEntry.Data.Building.GetMainLv();
		processCache();
		confMaxCount[] array = cacheMaxCount1;
		if (!isTruck)
		{
			array = cacheMaxCount2;
		}
		if (array != null)
		{
			confMaxCount[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				confMaxCount confMaxCount2 = array2[i];
				if (mainLv >= confMaxCount2.minLevel && mainLv <= confMaxCount2.maxLevel)
				{
					return confMaxCount2.count;
				}
			}
		}
		return 0;
	}

	private int GetFreeTruckIndex()
	{
		int showMaxCount = GetShowMaxCount(isTruck: true);
		showMaxCount += _circleTruckNum;
		if (FreeTruckList == null)
		{
			if (showMaxCount > 0)
			{
				FreeTruckList = new Dictionary<int, WorldCityTruck>();
				return 0;
			}
			return -1;
		}
		if (_alreadyTruckNum >= showMaxCount)
		{
			return -1;
		}
		int num = Mathf.Max(showMaxCount, AllTruckPrefabs.Length);
		if (_rndTruckIndexList.Count < num)
		{
			for (int i = _rndTruckIndexList.Count; i < num; i++)
			{
				_rndTruckIndexList.Add(i);
			}
		}
		_rndTruckIndexList = ListRandom(_rndTruckIndexList);
		foreach (int rndTruckIndex in _rndTruckIndexList)
		{
			if (FreeTruckList.ContainsKey(rndTruckIndex))
			{
				if (FreeTruckList[rndTruckIndex]._curState == WorldPeopleTruckBase.States.Init)
				{
					return rndTruckIndex;
				}
			}
			else if (!_nowGenTruckIndexList.Contains(rndTruckIndex))
			{
				return rndTruckIndex;
			}
		}
		return -1;
	}

	private int GetFreePeopleIndex()
	{
		int showMaxCount = GetShowMaxCount(isTruck: false);
		if (FreePeopleList == null)
		{
			if (showMaxCount > 0)
			{
				FreePeopleList = new Dictionary<int, WorldCityPeople>();
				return 0;
			}
			return -1;
		}
		if (_alreadyPeopleNum >= showMaxCount)
		{
			return -1;
		}
		int num = Mathf.Max(showMaxCount, AllPeoplePrefabs.Length);
		if (_rndPeopleIndexList.Count < num)
		{
			for (int i = _rndPeopleIndexList.Count; i < num; i++)
			{
				_rndPeopleIndexList.Add(i);
			}
		}
		_rndPeopleIndexList = ListRandom(_rndPeopleIndexList);
		foreach (int rndPeopleIndex in _rndPeopleIndexList)
		{
			if (FreePeopleList.ContainsKey(rndPeopleIndex))
			{
				if (FreePeopleList[rndPeopleIndex]._curState == WorldPeopleTruckBase.States.Init)
				{
					return rndPeopleIndex;
				}
				continue;
			}
			return rndPeopleIndex;
		}
		return -1;
	}

	public GameObject GetPeopleObjByIndex(int index)
	{
		if (FreePeopleList.ContainsKey(index))
		{
			return FreePeopleList[index].gameObject;
		}
		return null;
	}

	public float PauseAndPlayAnim(int index, string anim)
	{
		if (FreePeopleList.TryGetValue(index, out var value))
		{
			return value.PauseAndPlayAnim(anim);
		}
		return 0f;
	}

	public void Resume(int index)
	{
		if (FreePeopleList.TryGetValue(index, out var value))
		{
			value.Resume();
		}
	}

	private string GetFreeTruckPrefab()
	{
		if (_canShowTruckPrefab.Count == 0)
		{
			int num = 0;
			string[] allTruckPrefabs = AllTruckPrefabs;
			for (int i = 0; i < allTruckPrefabs.Length; i++)
			{
				_ = allTruckPrefabs[i];
				_canShowTruckPrefab.Add(num);
				num++;
			}
		}
		_canShowTruckPrefab = ListRandom(_canShowTruckPrefab);
		int num2 = _canShowTruckPrefab[0];
		_canShowTruckPrefab.RemoveAt(0);
		return AllTruckPrefabs[num2];
	}

	private string GetFreePeoplePrefab()
	{
		if (_canShowPeoplePrefab.Count == 0)
		{
			int num = 0;
			string[] allPeoplePrefabs = AllPeoplePrefabs;
			for (int i = 0; i < allPeoplePrefabs.Length; i++)
			{
				_ = allPeoplePrefabs[i];
				_canShowPeoplePrefab.Add(num);
				num++;
			}
		}
		_canShowPeoplePrefab = ListRandom(_canShowPeoplePrefab);
		int num2 = _canShowPeoplePrefab[0];
		_canShowPeoplePrefab.RemoveAt(0);
		return AllPeoplePrefabs[num2];
	}

	private BuildingPositionEnum GetPosDirEnum(Vector2Int mainCenter, Vector2Int pos)
	{
		Vector2Int vector2Int = pos - mainCenter;
		if (vector2Int.x >= 0)
		{
			if (vector2Int.y < 0)
			{
				return BuildingPositionEnum.SouthEast;
			}
			return BuildingPositionEnum.NorthEast;
		}
		if (vector2Int.y < 0)
		{
			return BuildingPositionEnum.SouthWest;
		}
		return BuildingPositionEnum.NorthWest;
	}

	private int GenRandomDir(List<int> availableDirs, int nowDir, HashSet<int> alreadyDirSet)
	{
		if (availableDirs.Count == 0)
		{
			return nowDir;
		}
		if (availableDirs.Count == 1)
		{
			return availableDirs[0];
		}
		List<int> list = ListRandom(availableDirs);
		foreach (int item in list)
		{
			if (!alreadyDirSet.Contains(item))
			{
				return item;
			}
		}
		return list[0];
	}

	private List<LuaBuildData> GetRandomBuild(List<LuaBuildData> buildingDates, bool isTruck)
	{
		int num = (isTruck ? 3 : 2);
		if (buildingDates == null || buildingDates.Count < num)
		{
			return null;
		}
		List<LuaBuildData> list = new List<LuaBuildData>();
		buildingDates = ListRandom(buildingDates);
		int num2 = 0;
		LuaBuildData luaBuildData = null;
		foreach (LuaBuildData buildingDate in buildingDates)
		{
			if (luaBuildData == null)
			{
				list.Add(buildingDate);
				luaBuildData = buildingDate;
				num2++;
			}
			else if (Vector2Int.Distance(SceneManager.World.IndexToTilePos(luaBuildData.pointId), SceneManager.World.IndexToTilePos(buildingDate.pointId)) >= 5f)
			{
				list.Add(buildingDate);
				luaBuildData = buildingDate;
				num2++;
			}
			if (num2 >= num)
			{
				break;
			}
		}
		return list;
	}

	public void UpdateMainBuildCheck()
	{
		LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
		if (buildingDataByBuildId == null || buildingDataByBuildId.state == 2 || buildingDataByBuildId.level < 1)
		{
			_canShowTruckAndPeople = false;
		}
		else
		{
			_canShowTruckAndPeople = true;
		}
		if (buildingDataByBuildId != null)
		{
			MainBuildUuid = buildingDataByBuildId.uuid;
		}
	}

	public void ShowRandomTruckAndPeople(Transform node)
	{
		if (_canShowTruckAndPeople)
		{
			float deltaTime = Time.deltaTime;
			ShowRandomPeople(node, deltaTime);
		}
	}

	private void CheckCanShowTruckAndPeople()
	{
	}

	private void loadRefreshTruckConfig()
	{
		if (_truckInterval < 0f || _truckRefreshCountPer < 0 || domeRange <= 0)
		{
			string[] array = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "city_car", "k4").Split(new char[1] { ';' });
			if (array.Length > 1)
			{
				_truckInterval = array[0].ToFloat();
				_truckRefreshCountPer = array[1].ToInt();
			}
			string str = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "city_car", "k5");
			if (!str.IsNullOrEmpty())
			{
				_circleTruckNum = str.ToInt();
			}
			domeRange = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetDomeRange");
			cityMainPos = SceneManager.World.TileToWorld(GameEntry.Data.Building.GetMainPos());
			cityMainPos = cityMainPos + Vector3.back + Vector3.left;
		}
	}

	private void ShowRandomTruck(Transform node, float deltaTime)
	{
		_truckShowTime += deltaTime;
		loadRefreshTruckConfig();
		if (_truckInterval <= 0f || _truckRefreshCountPer <= 0)
		{
			return;
		}
		float truckInterval = _truckInterval;
		if (_truckShowTime < truckInterval)
		{
			return;
		}
		_truckShowTime = 0f;
		bool flag = false;
		if (domeRange > 0 && domeRange <= _circleRadius.Length && !cityMainPos.Equals(Vector3.zero) && _alreadyCircleTruckNum < _circleTruckNum)
		{
			float num = 0f;
			float num2 = 360f / (float)_circleTruckNum * 2f;
			for (int i = _alreadyPeopleNum; i < _circleTruckNum; i++)
			{
				float num3 = UnityEngine.Random.Range(num + 3f, num + num2 - 3f);
				num += num2;
				int freeTruckIndex = GetFreeTruckIndex();
				if (freeTruckIndex == -1)
				{
					break;
				}
				WorldCityTruck worldCityTruck = null;
				if (FreeTruckList.ContainsKey(freeTruckIndex))
				{
					worldCityTruck = FreeTruckList[freeTruckIndex];
				}
				if (worldCityTruck != null && worldCityTruck._curState != WorldPeopleTruckBase.States.Init)
				{
					break;
				}
				ShowFreeTruck(new List<List<Vector2Int>>(), worldCityTruck, node, GetFreeTruckPrefab(), freeTruckIndex, isCircle: true, cityMainPos, _circleRadius[domeRange - 1], num3 % 360f, num3 > 360f);
			}
			_alreadyCircleTruckNum = _circleTruckNum;
			flag = true;
		}
		if (flag)
		{
			return;
		}
		for (int j = 0; j < _truckRefreshCountPer; j++)
		{
			int freeTruckIndex2 = GetFreeTruckIndex();
			if (freeTruckIndex2 == -1)
			{
				break;
			}
			WorldCityTruck worldCityTruck2 = null;
			if (FreeTruckList.ContainsKey(freeTruckIndex2))
			{
				worldCityTruck2 = FreeTruckList[freeTruckIndex2];
			}
			if (worldCityTruck2 != null && worldCityTruck2._curState != WorldPeopleTruckBase.States.Init)
			{
				break;
			}
			List<List<Vector2Int>> list = new List<List<Vector2Int>>();
			for (int k = 1; k < 5; k++)
			{
				List<Vector2Int> list2 = GameEntry.Data.Road.StreetManager.autoFindPath();
				if (list2.Count > 5)
				{
					list.Add(list2);
					break;
				}
			}
			if (list.Count > 0)
			{
				ShowFreeTruck(list, worldCityTruck2, node, GetFreeTruckPrefab(), freeTruckIndex2, isCircle: false, Vector3.zero, 0f, 0f, isInner: false);
			}
		}
	}

	private WorldCityPeople.CityPeopleType GetRandomPeopleType()
	{
		string[] array = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "city_car", "k3").Split(new char[1] { '|' });
		List<WorldCityPeople.CityPeopleType> list = new List<WorldCityPeople.CityPeopleType>();
		List<int> list2 = new List<int>();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string[] array3 = array2[i].Split(new char[1] { ';' });
			if (array3.Length == 2)
			{
				int num = array3[1].ToInt();
				if (num <= 0)
				{
					break;
				}
				list.Add((WorldCityPeople.CityPeopleType)array3[0].ToInt());
				list2.Add(num);
			}
		}
		if (list2.Count != 0)
		{
			int num2 = RandomArrIndex(list2);
			if (num2 > 0 && num2 < list.Count)
			{
				return list[num2];
			}
		}
		return WorldCityPeople.CityPeopleType.Normal;
	}

	private void ShowRandomPeople(Transform node, float deltaTime)
	{
		_peopleShowTime += deltaTime;
		float num = 5f;
		if (_peopleShowTime < num)
		{
			return;
		}
		_peopleShowTime = 0f;
		int freePeopleIndex = GetFreePeopleIndex();
		if (freePeopleIndex == -1)
		{
			return;
		}
		WorldCityPeople worldCityPeople = null;
		if (FreePeopleList.ContainsKey(freePeopleIndex))
		{
			worldCityPeople = FreePeopleList[freePeopleIndex];
		}
		if (worldCityPeople != null && worldCityPeople._curState != WorldPeopleTruckBase.States.Init)
		{
			return;
		}
		List<List<Vector2Int>> list = null;
		WorldCityPeople.CityPeopleType randomPeopleType = GetRandomPeopleType();
		List<LuaBuildData> param = new List<LuaBuildData>();
		List<LuaBuildData> list2 = GameEntry.Lua.CallWithReturn<List<LuaBuildData>, List<LuaBuildData>>("CSharpCallLuaInterface.GetAllInBaseTruckShowBuild", param);
		if (list2 == null || list2.Count < 1)
		{
			list = new List<List<Vector2Int>>();
			for (int i = 1; i < 5; i++)
			{
				List<Vector2Int> list3 = GameEntry.Data.Road.StreetManager.autoFindPath();
				if (list3.Count > 5)
				{
					list.Add(list3);
					break;
				}
			}
			if (list.Count > 0)
			{
				ShowFreePeople(list, worldCityPeople, node, GetFreePeoplePrefab(), freePeopleIndex, randomPeopleType);
			}
			return;
		}
		if (randomPeopleType == WorldCityPeople.CityPeopleType.Normal)
		{
			List<LuaBuildData> randomBuild = GetRandomBuild(list2, isTruck: true);
			if (randomBuild == null || randomBuild.Count < 2)
			{
				return;
			}
			list = GetPeoplePathByTargetBuildings(randomBuild);
		}
		else
		{
			int index = UnityEngine.Random.Range(0, list2.Count);
			LuaBuildData startBase = list2[index];
			if (randomPeopleType == WorldCityPeople.CityPeopleType.RoundBase)
			{
				list = GetPeopleRoundBasePath(startBase);
			}
		}
		if (list != null && list.Count != 0)
		{
			ShowFreePeople(list, worldCityPeople, node, GetFreePeoplePrefab(), freePeopleIndex, randomPeopleType);
		}
	}

	private List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, FindPathType findPathType = FindPathType.WorldDomeTruck, Predicate<WorldTileData> neighborFilter = null)
	{
		return _pathManager.FindingPath(start, end, findPathType, neighborFilter);
	}

	public List<Vector2Int> FindRoadPath(Vector2Int start, Vector2Int end, FindPathType findPathType = FindPathType.WorldDomeTruck, bool findMainBuildRoad = false)
	{
		if (!findMainBuildRoad)
		{
			return FindPath(start, end, findPathType, (WorldTileData neighbor) => PathUtils.IsWalkAble(neighbor.Pos, end, findPathType));
		}
		Vector2Int nearestMainOutPos = PathUtils.GetNearestMainOutPos(start);
		Vector2Int nearestMainOutPos2 = PathUtils.GetNearestMainOutPos(end);
		if (nearestMainOutPos == nearestMainOutPos2)
		{
			return FindPath(start, end, findPathType, (WorldTileData neighbor) => PathUtils.IsWalkAble(neighbor.Pos, end, findPathType));
		}
		List<Vector2Int> list = FindPath(start, nearestMainOutPos, findPathType, (WorldTileData neighbor) => PathUtils.IsWalkAble(neighbor.Pos, end, findPathType));
		list.AddRange(PathUtils.FindMainBuildRoadRoundPath(nearestMainOutPos, nearestMainOutPos2));
		list.AddRange(FindPath(nearestMainOutPos2, end, findPathType, (WorldTileData neighbor) => PathUtils.IsWalkAble(neighbor.Pos, end, findPathType)));
		return list;
	}

	private List<List<Vector2Int>> GetTruckPathByTargetBuildings(List<LuaBuildData> targetBuildingList)
	{
		List<List<Vector2Int>> list = new List<List<Vector2Int>>();
		Vector2Int startPos = SceneManager.World.IndexToTilePos(targetBuildingList[1].pointId);
		Vector2Int[] sortedBuildingNeighbors = PathUtils.GetSortedBuildingNeighbors(targetBuildingList[0].buildId, targetBuildingList[0].pointId, startPos);
		List<Vector2Int> list2 = null;
		Vector2Int[] array = sortedBuildingNeighbors;
		foreach (Vector2Int vector2Int in array)
		{
			Vector2Int[] sortedBuildingNeighbors2 = PathUtils.GetSortedBuildingNeighbors(targetBuildingList[1].buildId, targetBuildingList[1].pointId, vector2Int);
			foreach (Vector2Int end in sortedBuildingNeighbors2)
			{
				List<Vector2Int> list3 = FindRoadPath(vector2Int, end);
				if (list3.Count > 1)
				{
					list2 = list3;
					break;
				}
			}
			if (list2 != null && list2.Count > 1)
			{
				Vector2Int vector2Int2 = list2[list2.Count - 1];
				int tiles = GameEntry.ConfigCache.GetTemplateData("building", targetBuildingList[1].buildId, "tiles").ToInt();
				list2.Add(vector2Int2 - PathUtils.GetNeighborDir(vector2Int2, SceneManager.World.IndexToTilePos(targetBuildingList[1].pointId), tiles));
				break;
			}
		}
		if (list2 != null)
		{
			_ = list2.Count;
			_ = 1;
		}
		Vector2Int vector2Int3 = list2[list2.Count - 1];
		list.Add(list2);
		for (int k = 2; k < targetBuildingList.Count; k++)
		{
			LuaBuildData luaBuildData = targetBuildingList[k];
			Vector2Int[] sortedBuildingNeighbors3 = PathUtils.GetSortedBuildingNeighbors(luaBuildData.buildId, luaBuildData.pointId, vector2Int3);
			List<Vector2Int> list4 = null;
			array = sortedBuildingNeighbors3;
			foreach (Vector2Int end2 in array)
			{
				List<Vector2Int> list5 = FindRoadPath(vector2Int3, end2);
				if (list5.Count > 1)
				{
					list4 = list5;
					break;
				}
			}
			if (list4 == null || list4.Count <= 1)
			{
				break;
			}
			Vector2Int vector2Int4 = list4[list4.Count - 1];
			int tiles2 = GameEntry.ConfigCache.GetTemplateData("building", luaBuildData.buildId, "tiles").ToInt();
			list4.Add(vector2Int4 - PathUtils.GetNeighborDir(vector2Int4, SceneManager.World.IndexToTilePos(luaBuildData.pointId), tiles2));
			list.Add(list4);
			vector2Int3 = list4[list4.Count - 1];
		}
		if (list.Count >= 1)
		{
			LuaBuildData luaBuildData2 = targetBuildingList[0];
			Vector2Int[] sortedBuildingNeighbors4 = PathUtils.GetSortedBuildingNeighbors(luaBuildData2.buildId, luaBuildData2.pointId, vector2Int3);
			List<Vector2Int> list6 = null;
			array = sortedBuildingNeighbors4;
			foreach (Vector2Int end3 in array)
			{
				List<Vector2Int> list7 = FindRoadPath(vector2Int3, end3);
				if (list7.Count > 1)
				{
					list6 = list7;
					break;
				}
			}
			if (list6 == null || list6.Count <= 1)
			{
				return list;
			}
			Vector2Int vector2Int5 = list6[list6.Count - 1];
			int tiles3 = GameEntry.ConfigCache.GetTemplateData("building", luaBuildData2.buildId, "tiles").ToInt();
			list6.Add(vector2Int5 - PathUtils.GetNeighborDir(vector2Int5, SceneManager.World.IndexToTilePos(luaBuildData2.pointId), tiles3));
			list.Add(list6);
		}
		return list;
	}

	private List<List<Vector2Int>> GetPeoplePathByTargetBuildings(List<LuaBuildData> targetBuildingList)
	{
		List<List<Vector2Int>> list = new List<List<Vector2Int>>();
		Vector2Int startPos = SceneManager.World.IndexToTilePos(targetBuildingList[1].pointId);
		Vector2Int[] sortedBuildingNeighbors = PathUtils.GetSortedBuildingNeighbors(targetBuildingList[0].buildId, targetBuildingList[0].pointId, startPos);
		List<Vector2Int> list2 = null;
		Vector2Int[] array = sortedBuildingNeighbors;
		foreach (Vector2Int vector2Int in array)
		{
			Vector2Int[] sortedBuildingNeighbors2 = PathUtils.GetSortedBuildingNeighbors(targetBuildingList[1].buildId, targetBuildingList[1].pointId, vector2Int);
			foreach (Vector2Int end in sortedBuildingNeighbors2)
			{
				List<Vector2Int> list3 = FindRoadPath(vector2Int, end);
				if (list3.Count > 1)
				{
					list2 = list3;
					break;
				}
			}
			if (list2 != null && list2.Count > 1)
			{
				Vector2Int vector2Int2 = list2[list2.Count - 1];
				int tiles = GameEntry.ConfigCache.GetTemplateData("building", targetBuildingList[1].buildId, "tiles").ToInt();
				list2.Add(vector2Int2 - PathUtils.GetNeighborDir(vector2Int2, SceneManager.World.IndexToTilePos(targetBuildingList[1].pointId), tiles));
				break;
			}
		}
		if (list2 != null)
		{
			_ = list2.Count;
			_ = 1;
		}
		list.Add(list2);
		return list;
	}

	private List<List<Vector2Int>> GetPeopleRoundBasePath(LuaBuildData startBase)
	{
		List<List<Vector2Int>> list = new List<List<Vector2Int>>();
		Vector2Int[] array = PathUtils.GetBuildingNeighbors(startBase.buildId, startBase.pointId);
		Vector2Int[] peopleNearestMainOutPos = PathUtils.GetPeopleNearestMainOutPos(array, SceneManager.World.IndexToTilePos(startBase.pointId));
		if (peopleNearestMainOutPos == null || peopleNearestMainOutPos.Length < 2)
		{
			return list;
		}
		List<Vector2Int> list2 = null;
		if (peopleNearestMainOutPos[0] != peopleNearestMainOutPos[1])
		{
			array = PathUtils.GetSortedBuildingNeighbors(startBase, peopleNearestMainOutPos[0], array);
			Vector2Int[] array2 = array;
			foreach (Vector2Int start in array2)
			{
				List<Vector2Int> list3 = FindRoadPath(start, peopleNearestMainOutPos[0]);
				if (list3.Count > 1)
				{
					list2 = list3;
				}
			}
			if (list2 != null)
			{
				_ = list2.Count;
				_ = 1;
			}
			list2.Add(peopleNearestMainOutPos[1]);
		}
		else
		{
			list2 = new List<Vector2Int>();
		}
		Vector2Int vector2Int = peopleNearestMainOutPos[1];
		list2.AddRange(PathUtils.GetPeopleMainAroundPath(vector2Int));
		Vector2Int vector2Int2;
		if (peopleNearestMainOutPos[0] != peopleNearestMainOutPos[1])
		{
			List<Vector2Int> list4 = null;
			Vector2Int[] array2 = array;
			foreach (Vector2Int end in array2)
			{
				List<Vector2Int> list5 = FindRoadPath(vector2Int, end);
				if (list5.Count > 1)
				{
					list4 = list5;
					break;
				}
			}
			if (list4 == null || list4.Count <= 1)
			{
				return list;
			}
			vector2Int2 = list4[list4.Count - 1];
			list2.AddRange(list4);
		}
		else
		{
			vector2Int2 = peopleNearestMainOutPos[0];
		}
		int tiles = GameEntry.ConfigCache.GetTemplateData("building", startBase.buildId, "tiles").ToInt();
		list2.Add(vector2Int2 - PathUtils.GetNeighborDir(vector2Int2, SceneManager.World.IndexToTilePos(startBase.pointId), tiles));
		list.Add(list2);
		return list;
	}

	public void ClearAllTruck()
	{
		if (TruckRequest.Count > 0)
		{
			for (int i = 0; i < TruckRequest.Count; i++)
			{
				TruckRequest[i].Destroy();
			}
			TruckRequest.Clear();
		}
		FreeTruckList = null;
		if (PeopleRequest.Count > 0)
		{
			for (int j = 0; j < PeopleRequest.Count; j++)
			{
				PeopleRequest[j].Destroy();
			}
			PeopleRequest.Clear();
		}
		FreePeopleList = null;
		_truckShowTime = 0f;
		_peopleShowTime = 0f;
		_nowRandDirIndex = 0;
		_rndTruckIndexList.Clear();
		_nowGenTruckIndexList.Clear();
		_alreadyTruckNum = 0;
		_alreadyCircleTruckNum = 0;
		_rndPeopleIndexList.Clear();
		_alreadyPeopleNum = 0;
		_truckInterval = -1f;
		_truckRefreshCountPer = -1;
		_circleTruckNum = 0;
		cacheMaxCount1 = null;
		cacheMaxCount2 = null;
		domeRange = 0;
		cityMainPos = Vector3.zero;
		_canShowTruckPrefab.Clear();
		_canShowPeoplePrefab.Clear();
		_canShowTruckAndPeople = false;
		MainBuildUuid = 0L;
	}

	public static List<T> ListRandom<T>(List<T> list)
	{
		System.Random random = new System.Random();
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int num2 = random.Next(num + 1);
			int index = num2;
			int index2 = num;
			T val = list[num];
			T val2 = list[num2];
			T val3 = (list[index] = val);
			val3 = (list[index2] = val2);
		}
		return list;
	}

	public int RandomArrIndex(List<int> probability)
	{
		int result = int.MaxValue;
		int num = 0;
		foreach (int item in probability)
		{
			num += item;
		}
		if (num <= 0)
		{
			return -1;
		}
		int num2 = 0;
		int num3 = UnityEngine.Random.Range(0, num) + 1;
		int i = 0;
		for (int count = probability.Count; i < count; i++)
		{
			num2 += probability[i];
			if (num3 <= num2)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public bool IsTruckRoad(Vector2Int point, FindPathType findPathType)
	{
		return _pathManager.IsTruckRoad(point, findPathType);
	}

	public void ExpireRoadCache()
	{
		_pathManager.ExpireRoadCache();
	}
}
