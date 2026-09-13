using System.Collections.Generic;
using UnityEngine;
using XLua;

public class FakeCityModelManager : CityManagerBase
{
	public enum TempRoadType
	{
		MakeRoad,
		DeleteRoad,
		Collect,
		CreateBoard,
		StartPoint,
		Green
	}

	public class Param
	{
		public int index;

		public TempRoadType type;

		public InstanceRequest req;

		public string prefabName;

		public int order;

		public MeshRenderer[] renderers;

		public string showPrefabName;
	}

	private class PrintRoadAnimation
	{
		private bool isStart;

		private float progress;

		private List<int> roads = new List<int>();

		private List<Vector4> dirs = new List<Vector4>();

		private WorldRoadRobot robot;

		private bool isWorking;

		private bool isUseRobot;

		private float _buildPerRoadTime;

		public int printId { get; private set; }

		public int startPosIndex { get; private set; }

		public void AddRoad(int pointIndex)
		{
			roads.Add(pointIndex);
		}

		public void RemoveRoad(int pointIndex)
		{
			roads.Remove(pointIndex);
		}

		public void Clear(bool isFinish = false)
		{
			dirs.Clear();
			roads.Clear();
			isStart = false;
			isWorking = false;
			progress = 0f;
			if (robot != null)
			{
				if (isFinish)
				{
					robot.canAutoUpdate = true;
				}
				else
				{
					SceneManager.World.AddToNeedRemoveList(robot.uuid);
				}
			}
		}

		public void StartPrint(List<int> tempRoadPoints, bool isOtherRoad, float buildPerRoadTime = 0f, int id = 0)
		{
			printId = id;
			if (buildPerRoadTime > 0f)
			{
				_buildPerRoadTime = buildPerRoadTime;
			}
			else
			{
				_buildPerRoadTime = GameEntry.Lua.CallWithReturn<float>("CSharpCallLuaInterface.GetRoadBuildTime");
			}
			startPosIndex = tempRoadPoints[0];
			roads = tempRoadPoints;
			isUseRobot = !isOtherRoad;
			if (roads.Count > 0)
			{
				dirs.Add(new Vector4(1f, 0f, 1f, 0f));
			}
			if (roads.Count > 1)
			{
				for (int i = 1; i < roads.Count; i++)
				{
					Vector2Int vector2Int = SceneManager.World.IndexToTilePos(roads[i - 1]);
					Vector2Int vector2Int2 = SceneManager.World.IndexToTilePos(roads[i]) - vector2Int;
					Vector4 vector = new Vector4(vector2Int2.x, 0f, vector2Int2.y, 0f);
					if (Mathf.Abs(vector.x) > Mathf.Epsilon)
					{
						vector.z = 1f;
						vector.w = 0f;
					}
					else if (Mathf.Abs(vector.z) > Mathf.Epsilon)
					{
						vector.x = 1f;
						vector.w = 1f;
					}
					if (i > 1)
					{
						dirs.Add(vector);
						continue;
					}
					dirs[i - 1] = vector;
					dirs.Add(vector);
				}
			}
			foreach (int road in roads)
			{
				(SceneManager.World.GetObjectByPointId(road) as ModelManager.RoadObject)?.StartPrint((startPosIndex == road) ? printId : 0);
			}
			if (isUseRobot)
			{
				SceneManager.World.CreateRoadRobot(roads, isUseRobot, printId);
			}
			isStart = true;
		}

		public void Update()
		{
			if (!isStart)
			{
				return;
			}
			if (isUseRobot && robot == null)
			{
				robot = SceneManager.World.GetRoadRobot(printId);
				return;
			}
			float deltaTime = Time.deltaTime;
			if (isUseRobot)
			{
				isWorking |= robot.isWorking();
			}
			else
			{
				isWorking = true;
			}
			if (!isWorking)
			{
				robot.BotUpdate(Vector3.zero);
				return;
			}
			progress += deltaTime;
			float num = progress / _buildPerRoadTime;
			for (int i = 0; i < roads.Count; i++)
			{
				float num2 = Mathf.Clamp01(num - (float)i);
				if (SceneManager.World.GetObjectByPointId(roads[i]) is ModelManager.RoadObject roadObject)
				{
					roadObject.UpdatePrintProgress(num2, dirs[i]);
				}
			}
			Vector3 scanPos = calScanPos(num);
			if (num > 1f)
			{
				int index = (int)num - 1;
				(SceneManager.World.GetObjectByPointId(roads[index]) as ModelManager.RoadObject)?.FinishPrint();
			}
			if (num > (float)roads.Count)
			{
				Clear(isFinish: true);
			}
			else if (robot != null)
			{
				robot.BotUpdate(scanPos);
			}
		}

		private Vector3 calScanPos(float alreadyPrintGrid)
		{
			int num = Mathf.FloorToInt(alreadyPrintGrid);
			if (num >= roads.Count)
			{
				return SceneManager.World.TileIndexToWorld(roads[0]) + new Vector3(dirs[0].x * SceneManager.World.TileSize / 2f, 0f, 0f);
			}
			Vector4 vector = ((num >= roads.Count - 1) ? dirs[roads.Count - 1] : ((!(Vector4.Distance(dirs[num], dirs[num + 1]) > Mathf.Epsilon)) ? dirs[num] : ((alreadyPrintGrid - (float)num <= 0.5f) ? dirs[num] : dirs[num + 1])));
			Vector3 vector2 = ((vector.w == 0f) ? new Vector3(vector.x * SceneManager.World.TileSize / 2f, 0f, 0f) : new Vector3(0f, 0f, vector.z * SceneManager.World.TileSize / 2f));
			Vector3 vector3 = SceneManager.World.TileIndexToWorld(roads[num]);
			return Vector3.Lerp(vector3 - vector2, vector3 + vector2, alreadyPrintGrid - (float)num);
		}
	}

	public FakeBuilding preCreateBuild;

	public Queue<FakeBuilding> placeFalseBuild;

	public GameObject attackRange;

	public bool isPaitai;

	private List<Param> _curRoad = new List<Param>();

	private Queue<Queue<Param>> _saveBuildRoad = new Queue<Queue<Param>>();

	private Dictionary<string, Queue<InstanceRequest>> _free = new Dictionary<string, Queue<InstanceRequest>>();

	private Dictionary<int, PrintRoadAnimation> printRoadAnimList = new Dictionary<int, PrintRoadAnimation>();

	public static readonly string[] shapeString = new string[17]
	{
		"0000", "0101", "1100", "0110", "1001", "0011", "0101", "1001", "1100", "1010",
		"0110", "0011", "1010", "0001", "0010", "0100", "1000"
	};

	public static int PrintRoadID = 0;

	public FakeCityModelManager(CityScene scene)
		: base(scene)
	{
	}

	public override void OnUpdate(float deltaTime)
	{
		foreach (KeyValuePair<int, PrintRoadAnimation> printRoadAnim in printRoadAnimList)
		{
			printRoadAnim.Value.Update();
		}
	}

	public void UICreateBuilding(int buildId, long buildUuid, int point, int buildTopType, LuaTable noBuildList = null)
	{
		int id = buildId + 1;
		string templateData = GameEntry.ConfigCache.GetTemplateData("building", id, "model_path");
		if (templateData.IsNullOrEmpty())
		{
			templateData = GameEntry.ConfigCache.GetTemplateData("building", id, "model");
		}
		if (templateData.IsNullOrEmpty())
		{
			return;
		}
		string prefabPath = $"Assets/Main/Prefabs/Building/{templateData}.prefab";
		if (templateData.StartsWith("Assets/"))
		{
			prefabPath = templateData;
		}
		preCreateBuild = new FakeBuilding();
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(prefabPath);
		preCreateBuild.request = request;
		request.completed += delegate
		{
			preCreateBuild.city = request.gameObject.GetComponent<CityBuilding>();
			preCreateBuild.city.CSInit(new CityBuilding.Param
			{
				buildId = buildId,
				buildUuid = buildUuid,
				point = point,
				BuildTopType = (PlaceBuildType)buildTopType,
				noPutPoint = noBuildList,
				buildSceneType = CityBuilding.BuildSceneType.Fake,
				noDoAnim = true,
				visible = true
			});
			if (buildId == 418000)
			{
				isPaitai = true;
				attackRange = request.gameObject.transform.Find("Range").gameObject;
				if (!attackRange.activeSelf)
				{
					attackRange.SetActive(value: true);
				}
			}
			if (buildId == 10224000)
			{
				preCreateBuild.city.AddActivityAlarmClockEffect();
			}
			GameEntry.Event.Fire(EventId.UICreateFakePlaceBuild);
		};
	}

	public void UIChangeBuilding(int index)
	{
		if (!(preCreateBuild.city != null))
		{
			return;
		}
		if (placeFalseBuild == null)
		{
			placeFalseBuild = new Queue<FakeBuilding>();
		}
		placeFalseBuild.Enqueue(preCreateBuild);
		if (isPaitai)
		{
			if (attackRange.activeSelf)
			{
				attackRange.SetActive(value: false);
			}
			isPaitai = false;
		}
		preCreateBuild.city.ResetParam(index);
		preCreateBuild = null;
	}

	public void UIDestroyRreCreateBuild()
	{
		if (preCreateBuild == null || preCreateBuild.request == null || !(preCreateBuild.city != null))
		{
			return;
		}
		if (isPaitai)
		{
			if (attackRange.activeSelf)
			{
				attackRange.SetActive(value: false);
			}
			isPaitai = false;
		}
		preCreateBuild.city.CSUninit();
		preCreateBuild.request.Destroy();
		preCreateBuild = null;
	}

	public void UIDestroyBuilding()
	{
		if (placeFalseBuild != null && placeFalseBuild.Count > 0)
		{
			FakeBuilding fakeBuilding = placeFalseBuild.Dequeue();
			if (fakeBuilding != null)
			{
				fakeBuilding.city.CSUninit();
				fakeBuilding.request.Destroy();
			}
		}
	}

	public void UICreateBoard(int index, bool isAfter = true)
	{
		GameEntry.Sound.PlayEffect("effect_road");
		if (isAfter)
		{
			Param param = new Param
			{
				index = index,
				type = TempRoadType.CreateBoard,
				req = null,
				prefabName = "",
				order = _curRoad.Count
			};
			_curRoad.Add(param);
			int count = _curRoad.Count;
			Vector2Int vector2Int = ((count > 1) ? SceneManager.World.IndexToTilePos(_curRoad[count - 2].index) : Vector2Int.zero);
			Vector2Int param2 = SceneManager.World.IndexToTilePos(index);
			Vector2Int zero = Vector2Int.zero;
			int num = GameEntry.Lua.CallWithReturn<int, Vector2Int, Vector2Int, Vector2Int>("CSharpCallLuaInterface.GetDirByPos", vector2Int, param2, zero);
			param.prefabName = $"Assets/Main/Prefabs/Road/Road_Fake_{shapeString[num]}.prefab";
			ShowOneRoad(param);
			count = _curRoad.Count;
			if (vector2Int != Vector2Int.zero)
			{
				ChangeIndexRoad(count - 2);
			}
		}
		else
		{
			Param param3 = new Param
			{
				index = index,
				type = TempRoadType.CreateBoard,
				req = null,
				prefabName = "",
				order = _curRoad.Count
			};
			_curRoad.Insert(0, param3);
			int count2 = _curRoad.Count;
			Vector2Int zero2 = Vector2Int.zero;
			Vector2Int param4 = SceneManager.World.IndexToTilePos(index);
			Vector2Int vector2Int2 = ((count2 > 1) ? SceneManager.World.IndexToTilePos(_curRoad[1].index) : Vector2Int.zero);
			int num2 = GameEntry.Lua.CallWithReturn<int, Vector2Int, Vector2Int, Vector2Int>("CSharpCallLuaInterface.GetDirByPos", zero2, param4, vector2Int2);
			param3.prefabName = $"Assets/Main/Prefabs/Road/Road_Fake_{shapeString[num2]}.prefab";
			ShowOneRoad(param3);
			_ = _curRoad.Count;
			if (vector2Int2 != Vector2Int.zero)
			{
				ChangeIndexRoad(1);
			}
		}
	}

	private void AddOneFree(string prefabName, InstanceRequest request)
	{
		if (!_free.ContainsKey(prefabName))
		{
			_free.Add(prefabName, new Queue<InstanceRequest>());
		}
		if (request != null)
		{
			if (request.gameObject != null)
			{
				request.gameObject.SetActive(value: false);
				_free[prefabName].Enqueue(request);
			}
			else
			{
				request.Destroy();
			}
		}
	}

	public void UIHideBoard(int deleteCount, bool isAfter = true)
	{
		int count = _curRoad.Count;
		if (isAfter)
		{
			if (count >= deleteCount)
			{
				for (int i = 0; i < deleteCount; i++)
				{
					HideRoad(count - i - 1);
				}
				if (_curRoad.Count > 0)
				{
					ChangeIndexRoad(_curRoad.Count - 1);
				}
			}
		}
		else if (count >= deleteCount)
		{
			for (int num = deleteCount; num > 0; num--)
			{
				HideRoad(num - 1);
			}
			if (_curRoad.Count > 0)
			{
				ChangeIndexRoad(0);
			}
		}
	}

	private void ChangeIndexRoad(int index)
	{
		int count = _curRoad.Count;
		Vector2Int param = ((index - 1 >= 0) ? SceneManager.World.IndexToTilePos(_curRoad[index - 1].index) : Vector2Int.zero);
		Vector2Int param2 = SceneManager.World.IndexToTilePos(_curRoad[index].index);
		Vector2Int param3 = ((index + 1 < count) ? SceneManager.World.IndexToTilePos(_curRoad[index + 1].index) : Vector2Int.zero);
		int num = GameEntry.Lua.CallWithReturn<int, Vector2Int, Vector2Int, Vector2Int>("CSharpCallLuaInterface.GetDirByPos", param, param2, param3);
		string text = $"Assets/Main/Prefabs/Road/Road_Fake_{shapeString[num]}.prefab";
		Param param4 = _curRoad[index];
		if (param4.prefabName != text)
		{
			AddOneFree(param4.prefabName, param4.req);
			param4.prefabName = text;
			ShowOneRoad(param4);
		}
	}

	public void UIChangeRoad()
	{
		Queue<Param> queue = new Queue<Param>();
		for (int i = 0; i < _curRoad.Count; i++)
		{
			queue.Enqueue(_curRoad[i]);
		}
		_saveBuildRoad.Enqueue(queue);
		_curRoad.Clear();
	}

	public List<int> UIDestroyRoad()
	{
		List<int> list = new List<int>();
		if (_saveBuildRoad.Count > 0)
		{
			Queue<Param> queue = _saveBuildRoad.Dequeue();
			int count = queue.Count;
			for (int i = 0; i < count; i++)
			{
				Param param = queue.Dequeue();
				list.Add(param.index);
				param.req.Destroy();
			}
		}
		return list;
	}

	public void StartPrintRoad(List<int> roads, bool isOther, float buildPerRoadTime = 0f)
	{
		if (roads.Count > 0)
		{
			PrintRoadAnimation printRoadAnimation = new PrintRoadAnimation();
			PrintRoadID++;
			printRoadAnimation.StartPrint(roads, isOther, buildPerRoadTime, PrintRoadID);
			printRoadAnimList.Add(PrintRoadID, printRoadAnimation);
		}
	}

	public void FinishPrintRoad(int pointId)
	{
		if (printRoadAnimList.ContainsKey(pointId))
		{
			printRoadAnimList[pointId].Clear();
			printRoadAnimList.Remove(pointId);
		}
	}

	private void ShowOneRoad(Param param)
	{
		if (_free.ContainsKey(param.prefabName) && _free[param.prefabName].Count > 0)
		{
			param.req = _free[param.prefabName].Dequeue();
			param.req.gameObject.SetActive(value: true);
			param.req.gameObject.transform.position = SceneManager.World.TileIndexToWorld(param.index) + GameDefines.BlockPos;
			param.renderers = null;
			SetRoadSortOrder(param);
			return;
		}
		param.req = GameEntry.Resource.InstantiateAsync(param.prefabName);
		param.req.completed += delegate
		{
			param.req.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			param.req.gameObject.SetActive(value: true);
			param.req.gameObject.transform.position = SceneManager.World.TileIndexToWorld(param.index) + GameDefines.BlockPos;
			param.renderers = null;
			SetRoadSortOrder(param);
		};
	}

	private void HideRoad(int index)
	{
		if (_curRoad.Count > index)
		{
			AddOneFree(_curRoad[index].prefabName, _curRoad[index].req);
			_curRoad.RemoveAt(index);
		}
	}

	public bool HasBoard(int index, string uid, int dir)
	{
		int indexByOffsetByDirection = SceneManager.World.GetIndexByOffsetByDirection(index, dir);
		if (GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetBoardDataByPointId", indexByOffsetByDirection) != null)
		{
			return true;
		}
		LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
		if (buildingDataByBuildId != null)
		{
			int num = GameEntry.ConfigCache.GetTemplateData("building", buildingDataByBuildId.buildId, "tiles").ToInt();
			if (num > 0)
			{
				int index2 = GameEntry.Lua.CallWithReturn<int, int, int>("CSharpCallLuaInterface.GetBuildModelCenter", buildingDataByBuildId.pointId, num);
				int num2 = num / 2;
				if (indexByOffsetByDirection == SceneManager.World.GetIndexByOffset(index2, num2) || indexByOffsetByDirection == SceneManager.World.GetIndexByOffset(index2, -num2) || indexByOffsetByDirection == SceneManager.World.GetIndexByOffset(index2, 0, num2) || indexByOffsetByDirection == SceneManager.World.GetIndexByOffset(index2, 0, -num2))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void SetRoadSortOrder(Param param)
	{
		if (param.req == null)
		{
			return;
		}
		if (param.renderers == null)
		{
			param.renderers = param.req.gameObject.GetComponentsInChildren<MeshRenderer>();
		}
		if (param.renderers != null)
		{
			MeshRenderer[] renderers = param.renderers;
			for (int i = 0; i < renderers.Length; i++)
			{
				renderers[i].sortingOrder = param.order;
			}
		}
	}

	private float GetBuildPerRoadTime()
	{
		return GameEntry.Lua.CallWithReturn<float>("CSharpCallLuaInterface.GetRoadBuildTime");
	}
}
