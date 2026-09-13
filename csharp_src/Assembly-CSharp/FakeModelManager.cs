using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class FakeModelManager : WorldManagerBase
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

		private bool isOther;

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

		public void StartPrint(List<int> tempRoadPoints, bool isOtherRoad, int id)
		{
			printId = id;
			startPosIndex = tempRoadPoints[0];
			roads = tempRoadPoints;
			isOther = isOtherRoad;
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
				(SceneManager.World.GetObjectByPoint(road) as WorldBoardObject)?.StartPrint((startPosIndex == road) ? printId : 0);
			}
			SceneManager.World.CreateRoadRobot(roads, isOther: true, printId);
			isStart = true;
		}

		public void Update()
		{
			if (!isStart)
			{
				return;
			}
			if (robot == null)
			{
				robot = SceneManager.World.GetRoadRobot(printId);
				return;
			}
			float deltaTime = Time.deltaTime;
			isWorking |= robot.isWorking();
			if (!isWorking)
			{
				robot.BotUpdate(Vector3.zero);
				return;
			}
			progress += deltaTime;
			float num = progress / GameEntry.Lua.CallWithReturn<float>("CSharpCallLuaInterface.GetRoadBuildTime");
			for (int i = 0; i < roads.Count; i++)
			{
				float num2 = Mathf.Clamp01(num - (float)i);
				if (SceneManager.World.GetObjectByPoint(roads[i]) is WorldBoardObject worldBoardObject)
				{
					worldBoardObject.UpdatePrintProgress(num2, dirs[i]);
				}
			}
			Vector3 scanPos = calScanPos(num);
			if (num > 1f)
			{
				int index = (int)num - 1;
				if (SceneManager.World.GetObjectByPoint(roads[index]) is WorldBoardObject worldBoardObject2 && worldBoardObject2.GetState() == BoardState.Updating)
				{
					worldBoardObject2.FinishPrint();
				}
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

	private List<InstanceRequest> instanceEffectList;

	public FakeWorldBuilding preCreateBuild;

	public Queue<FakeWorldBuilding> placeFalseBuild;

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

	public FakeAllianceBuilding preCreateAllianceBuild;

	public Queue<FakeAllianceBuilding> placeFalseAllianceBuild;

	public FakeWorldMoveMarch preCreateWorldMoveMarch;

	public Queue<FakeWorldMoveMarch> placeFalseWorldMoveMarch;

	public FakeWorldFlowerTrain preCreateWorldFlowerTrain;

	public FakeWorldTrigger preCreateWorldTrigger;

	public FakeWorldMovingModel preCreateWorldMovingModel;

	public FakeModelManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void UnInit()
	{
		UIDestroyRreCreateBuild();
	}

	public override void OnUpdate(float deltaTime)
	{
		foreach (KeyValuePair<int, PrintRoadAnimation> printRoadAnim in printRoadAnimList)
		{
			printRoadAnim.Value.Update();
		}
		if (preCreateBuild != null && preCreateBuild.city != null && preCreateBuild.city.OpenFakeAutoDestroy)
		{
			preCreateBuild.city.CheckFakeAutoDestroy();
		}
	}

	public void UICreateBuilding(int buildId, long buildUuid, int point, int buildTopType, LuaTable noBuildList = null)
	{
		int id = buildId + 1;
		string text = GameEntry.ConfigCache.GetTemplateData("building", id, "model_world_path");
		if (text.IsNullOrEmpty())
		{
			text = GameEntry.ConfigCache.GetTemplateData("building", id, "model_world");
		}
		if (text.IsNullOrEmpty() || preCreateBuild != null)
		{
			return;
		}
		preCreateBuild = new FakeWorldBuilding();
		BuildPointInfo buildPointInfo = ((buildTopType == 4) ? (SceneManager.World.GetMyPointInfo() as BuildPointInfo) : (SceneManager.World.GetPointInfoByUuid(buildUuid) as BuildPointInfo));
		if (buildPointInfo != null && buildPointInfo.itemId == 10100000 && buildPointInfo.appearanceId == 2)
		{
			text += "_girl";
		}
		string prefabPath = $"Assets/Main/Prefabs/Building/{text}.prefab";
		if (text.StartsWith("Assets/"))
		{
			prefabPath = text;
		}
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(prefabPath);
		preCreateBuild.request = request;
		request.completed += delegate
		{
			preCreateBuild.city = request.gameObject.GetComponent<WorldBuilding>();
			preCreateBuild.city.CSInit(new WorldBuilding.Param
			{
				buildId = buildId,
				buildUuid = buildUuid,
				point = point,
				BuildTopType = (PlaceBuildType)buildTopType,
				noPutPoint = noBuildList,
				buildSceneType = WorldBuilding.BuildSceneType.Fake
			});
			preCreateBuild.city.UpdateCityLabel(buildUuid);
			if (buildId == 418000)
			{
				isPaitai = true;
				attackRange = request.gameObject.transform.Find("Range").gameObject;
				if (!attackRange.activeSelf)
				{
					attackRange.SetActive(value: true);
				}
			}
			GameEntry.Event.Fire(EventId.UICreateFakePlaceBuild);
		};
	}

	public void UICreateBuildingModelPath(int buildId, long buildUuid, int point, int buildTopType, string modelPath, LuaTable noBuildList = null, LuaTable param = null)
	{
		int id = buildId + 1;
		string text = "";
		if (GameEntry.Data.Player.GetMainUuid() == buildUuid && GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IAmWerewolf") && !GameEntry.Data.Player.IsInBattleField())
		{
			text = "Assets/Main/SeasonRes/S4/Prefabs/Building/A_build_werewolf_world.prefab";
		}
		else if (string.IsNullOrEmpty(modelPath))
		{
			text = GameEntry.ConfigCache.GetTemplateData("building", id, "model_world_path");
			if (text.IsNullOrEmpty())
			{
				text = GameEntry.ConfigCache.GetTemplateData("building", id, "model_world");
			}
		}
		else
		{
			text = modelPath;
		}
		if (text.IsNullOrEmpty() || preCreateBuild != null)
		{
			return;
		}
		string prefabPath = $"Assets/Main/Prefabs/Building/{text}.prefab";
		if (text.StartsWith("Assets/"))
		{
			prefabPath = text;
		}
		world.MapGridRenderer.ShowMapGrid(world.CurTarget);
		world.MapElectricityRenderer.ShowMapGrid(point);
		preCreateBuild = new FakeWorldBuilding();
		int serverId = ((param != null && param.ContainsKey("serverId")) ? param.Get<int>("serverId") : SeasonDataManager.Instance.GetServerIdFromWorldPos(world.curTouchPoint));
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(prefabPath);
		preCreateBuild.request = request;
		request.completed += delegate
		{
			preCreateBuild.city = request.gameObject.GetComponent<WorldBuilding>();
			PlaceBuildType placeBuildType = (PlaceBuildType)buildTopType;
			preCreateBuild.city.CSInit(new WorldBuilding.Param
			{
				buildId = buildId,
				buildUuid = buildUuid,
				point = point,
				BuildTopType = placeBuildType,
				noPutPoint = noBuildList,
				param = param,
				buildSceneType = WorldBuilding.BuildSceneType.Fake,
				serverId = serverId
			});
			preCreateBuild.city.UpdateCityLabel(buildUuid);
			if (placeBuildType == PlaceBuildType.EpidemicSkill)
			{
				UICreateFakeEpidemicSkill();
			}
			if (buildId == 418000)
			{
				isPaitai = true;
				attackRange = request.gameObject.transform.Find("Range").gameObject;
				if (!attackRange.activeSelf)
				{
					attackRange.SetActive(value: true);
				}
			}
			Transform transform = request.gameObject.transform.Find("Icon/Sprite2");
			if (transform != null)
			{
				SpriteRenderer componentInChildren = transform.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
				if (componentInChildren != null)
				{
					componentInChildren.LoadSprite("Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_lv.png");
				}
			}
			transform = request.gameObject.transform.Find("Icon/Sprite");
			if (transform != null)
			{
				SpriteRenderer componentInChildren2 = transform.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
				if (componentInChildren2 != null)
				{
					componentInChildren2.LoadSprite("Assets/Main/Sprites/LodIcon/huojian3.png");
				}
			}
			Transform transform2 = request.gameObject.transform.Find("ModelGo/CityLabel/Virus");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(value: false);
			}
			GameEntry.Event.Fire(EventId.UICreateFakePlaceBuild);
		};
	}

	public void UICreateFakeEpidemicSkill()
	{
		if (preCreateBuild != null && preCreateBuild.city != null)
		{
			preCreateBuild.city.FakeEpidemicSkill();
		}
	}

	public void UIChangeBuilding(int index)
	{
		if (!(preCreateBuild.city != null))
		{
			return;
		}
		if (placeFalseBuild == null)
		{
			placeFalseBuild = new Queue<FakeWorldBuilding>();
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
		if (preCreateBuild != null)
		{
			if (isPaitai)
			{
				if (attackRange != null && attackRange.activeSelf)
				{
					attackRange.SetActive(value: false);
				}
				isPaitai = false;
			}
			if (preCreateBuild.city != null)
			{
				preCreateBuild.city.CSUninit();
			}
			if (preCreateBuild.request != null)
			{
				preCreateBuild.request.Destroy();
			}
			preCreateBuild = null;
		}
		world.MapGridRenderer.HideMapGrid();
		world.MapElectricityRenderer.HideMapGrid();
	}

	public void UIDestroyBuilding()
	{
		if (placeFalseBuild != null && placeFalseBuild.Count > 0)
		{
			FakeWorldBuilding fakeWorldBuilding = placeFalseBuild.Dequeue();
			fakeWorldBuilding.city.CSUninit();
			fakeWorldBuilding.request.Destroy();
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

	public void StartPrintRoad(List<int> roads, bool isOther)
	{
		if (roads.Count > 0)
		{
			PrintRoadAnimation printRoadAnimation = new PrintRoadAnimation();
			PrintRoadID++;
			printRoadAnimation.StartPrint(roads, isOther, PrintRoadID);
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

	public void UICreateAllianceBuilding(int buildId, long buildUuid, int point, int buildTopType, LuaTable noBuildList = null)
	{
		PlaceBuildType BuildTopType = (PlaceBuildType)buildTopType;
		string text = null;
		if (BuildTopType == PlaceBuildType.CityAttachment)
		{
			long num = buildUuid % 1000;
			text = $"Assets/Main/Prefabs/AllianceBuilding/CityAttachment/Slot{num}.prefab";
		}
		else
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", buildId, "model");
			text = (templateData.StartsWith("Assets/Main/") ? ((!templateData.EndsWith(".prefab")) ? (templateData + ".prefab") : templateData) : ((buildId != 300000 && buildId != 301000) ? ((buildId != 400000 && buildId != 401000) ? string.Format("Assets/Main/Prefabs/AllianceBuilding/{0}.prefab", templateData.Replace(".prefab", "")) : string.Format("Assets/Main/SeasonRes/S4/Prefabs/AllianceBuilding/{0}.prefab", templateData.Replace(".prefab", ""))) : string.Format("Assets/Main/SeasonRes/S3/Prefabs/AllianceBuilding/{0}.prefab", templateData.Replace(".prefab", ""))));
		}
		if (text.IsNullOrEmpty() || preCreateBuild != null)
		{
			return;
		}
		preCreateAllianceBuild = new FakeAllianceBuilding();
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(text);
		preCreateAllianceBuild.request = request;
		request.completed += delegate
		{
			GameObject gameObject = request.gameObject;
			preCreateAllianceBuild.allianceBuild = gameObject.GetComponent<WorldAllianceBuilding>();
			preCreateAllianceBuild.allianceBuild.CSInit(new WorldAllianceBuilding.Param
			{
				buildId = buildId,
				buildUuid = buildUuid,
				point = point,
				tileSize = 3,
				BuildTopType = (PlaceBuildType)buildTopType,
				noPutPoint = noBuildList,
				buildSceneType = WorldAllianceBuilding.AllianceBuildSceneType.Fake
			});
			GameObject gameObject2 = gameObject.transform.Find("ModelGo/stateIcon")?.gameObject;
			if (gameObject2 != null)
			{
				gameObject2.SetActive(value: false);
			}
			if (BuildTopType == PlaceBuildType.CityAttachment)
			{
				string tabName = "season_builders_alliance_list";
				string templateData2 = GameEntry.ConfigCache.GetTemplateData(tabName, buildId, "model");
				if (!templateData2.IsNullOrEmpty())
				{
					if (instanceEffectList == null)
					{
						instanceEffectList = new List<InstanceRequest>();
					}
					instanceEffectList.Add(AsyncLoad(templateData2, gameObject.transform.Find("Model"), delegate
					{
					}));
				}
				GameObject gameObject3 = gameObject.transform.Find("Model/BuildBloodTip")?.gameObject;
				if (gameObject3 != null)
				{
					gameObject3.SetActive(value: false);
				}
				GameObject gameObject4 = gameObject.transform.Find("Model/Upgrade")?.gameObject;
				if (gameObject4 != null)
				{
					gameObject4.SetActive(value: false);
				}
			}
			GameEntry.Event.Fire(EventId.UICreateFakePlaceAllianceBuild);
		};
	}

	public void UIChangeAllianceBuilding(int index)
	{
		if (preCreateAllianceBuild.allianceBuild != null)
		{
			if (placeFalseAllianceBuild == null)
			{
				placeFalseAllianceBuild = new Queue<FakeAllianceBuilding>();
			}
			placeFalseAllianceBuild.Enqueue(preCreateAllianceBuild);
			preCreateAllianceBuild.allianceBuild.ResetParam(index);
			preCreateAllianceBuild = null;
		}
		DestroyEffect();
	}

	public void UIDestroyRreCreateAllianceBuild()
	{
		if (preCreateAllianceBuild != null && preCreateAllianceBuild.request != null && preCreateAllianceBuild.allianceBuild != null)
		{
			preCreateAllianceBuild.allianceBuild.CSUninit();
			preCreateAllianceBuild.request.Destroy();
			preCreateAllianceBuild = null;
		}
		DestroyEffect();
	}

	public void UIDestroyAllianceBuilding()
	{
		if (placeFalseAllianceBuild != null && placeFalseAllianceBuild.Count > 0)
		{
			FakeAllianceBuilding fakeAllianceBuilding = placeFalseAllianceBuild.Dequeue();
			fakeAllianceBuilding.allianceBuild.CSUninit();
			fakeAllianceBuilding.request.Destroy();
		}
		DestroyEffect();
	}

	private void DestroyEffect()
	{
		if (instanceEffectList != null && instanceEffectList.Count > 0)
		{
			instanceEffectList.ForEach(delegate(InstanceRequest m)
			{
				m.Destroy();
			});
			instanceEffectList.Clear();
		}
	}

	public void UICreateWorldMoveMarch(string modelPath, long uuid, Vector3 pos)
	{
		if (string.IsNullOrEmpty(modelPath) || preCreateWorldMoveMarch != null)
		{
			return;
		}
		WorldMarch march = SceneManager.World.GetMarch(uuid);
		Vector3 worldPos = TileCoord.TileIndexToWorld(march.targetPos, ForceChangeScene.World, march.targetServer);
		world.MapGridRenderer.ShowMapGrid(worldPos);
		world.MapElectricityRenderer.ShowMapGrid(march.targetPos);
		preCreateWorldMoveMarch = new FakeWorldMoveMarch();
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(modelPath);
		preCreateWorldMoveMarch.request = request;
		request.completed += delegate
		{
			if (!(request.gameObject == null))
			{
				MoveAbleWorldMarch moveAbleWorldMarch = new MoveAbleWorldMarch();
				moveAbleWorldMarch.Init(uuid, request.gameObject, pos);
				preCreateWorldMoveMarch.moveAbleWorldMarch = moveAbleWorldMarch;
			}
		};
	}

	public void UIDestroyRreCreateMarch()
	{
		world.MapGridRenderer.HideMapGrid();
		world.MapElectricityRenderer.HideMapGrid();
		if (preCreateWorldMoveMarch != null && preCreateWorldMoveMarch.request != null && preCreateWorldMoveMarch.moveAbleWorldMarch != null)
		{
			preCreateWorldMoveMarch.request.Destroy();
			preCreateWorldMoveMarch.moveAbleWorldMarch.Destroy();
			preCreateWorldMoveMarch = null;
		}
	}

	public void UICreateWorldFlowerTrain(string modelPath, int pointId, int goodsId)
	{
		if (string.IsNullOrEmpty(modelPath) || preCreateWorldFlowerTrain != null)
		{
			return;
		}
		Vector3 worldPos = TileCoord.TileIndexToWorld(pointId, ForceChangeScene.World, GameEntry.Data.Player.GetCurServerId());
		world.MapGridRenderer.ShowMapGrid(worldPos);
		world.MapElectricityRenderer.ShowMapGrid(pointId);
		preCreateWorldFlowerTrain = new FakeWorldFlowerTrain();
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(modelPath);
		preCreateWorldFlowerTrain.request = request;
		request.completed += delegate
		{
			if (!(request.gameObject == null))
			{
				MovableFlowerTrain movableFlowerTrain = new MovableFlowerTrain();
				movableFlowerTrain.Init(request.gameObject, pointId, goodsId);
				preCreateWorldFlowerTrain.movableflowerTrain = movableFlowerTrain;
			}
		};
	}

	public void UIDestroyPreCreateFlowerTrain()
	{
		world.MapGridRenderer.HideMapGrid();
		world.MapElectricityRenderer.HideMapGrid();
		if (preCreateWorldFlowerTrain != null && preCreateWorldFlowerTrain.request != null && preCreateWorldFlowerTrain.movableflowerTrain != null)
		{
			preCreateWorldFlowerTrain.request.Destroy();
			preCreateWorldFlowerTrain.movableflowerTrain.Destroy();
			preCreateWorldFlowerTrain = null;
		}
	}

	public void UICreateWorldTrigger(string modelPath, int cfgId, int pointId, int skillId, int serverId)
	{
		if (string.IsNullOrEmpty(modelPath) || preCreateWorldTrigger != null)
		{
			return;
		}
		Vector3 worldPos = TileCoord.TileIndexToWorld(pointId, ForceChangeScene.World, serverId);
		world.MapGridRenderer.ShowMapGrid(worldPos);
		world.MapElectricityRenderer.ShowMapGrid(pointId);
		preCreateWorldTrigger = new FakeWorldTrigger();
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(modelPath);
		preCreateWorldTrigger.request = request;
		request.completed += delegate
		{
			if (!(request.gameObject == null))
			{
				MovableTrigger movableTrigger = new MovableTrigger();
				movableTrigger.Init(serverId, cfgId, request.gameObject, pointId, skillId, worldPos);
				preCreateWorldTrigger.movableTrigger = movableTrigger;
			}
		};
	}

	public void UIDestroyPreCreateTrigger()
	{
		world.MapGridRenderer.HideMapGrid();
		world.MapElectricityRenderer.HideMapGrid();
		if (preCreateWorldTrigger != null)
		{
			if (preCreateWorldTrigger.request != null)
			{
				preCreateWorldTrigger.request.Destroy();
			}
			if (preCreateWorldTrigger.movableTrigger != null)
			{
				preCreateWorldTrigger.movableTrigger.Destroy();
			}
			preCreateWorldTrigger = null;
		}
	}

	public void UICreateWorldMovingModel(string modelPath, int flag, int pointId, int size)
	{
		if (string.IsNullOrEmpty(modelPath) || preCreateWorldMovingModel != null)
		{
			return;
		}
		Vector3 worldPos = TileCoord.TileIndexToWorld(pointId, ForceChangeScene.World, GameEntry.Data.Player.GetCurServerId());
		world.MapGridRenderer.ShowMapGrid(worldPos);
		world.MapElectricityRenderer.ShowMapGrid(pointId);
		preCreateWorldMovingModel = new FakeWorldMovingModel();
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(modelPath);
		preCreateWorldMovingModel.request = request;
		request.completed += delegate
		{
			if (!(request.gameObject == null))
			{
				WorldMovingModel worldMovingModel = new WorldMovingModel();
				worldMovingModel.Init(request.gameObject, (FakeMovingModelFlag)flag, pointId, size);
				preCreateWorldMovingModel.movableModel = worldMovingModel;
				GameEntry.Lua.Call("CSharpCallLuaInterface.CreateMovingModelFinished", flag, request.gameObject.transform);
			}
		};
	}

	public void UIDestroyRreCreateModel()
	{
		if (preCreateWorldMovingModel != null)
		{
			world?.MapGridRenderer?.HideMapGrid();
			world?.MapElectricityRenderer?.HideMapGrid();
		}
		if (preCreateWorldMovingModel != null && preCreateWorldMovingModel.request != null && preCreateWorldMovingModel.movableModel != null)
		{
			preCreateWorldMovingModel.request.Destroy();
			preCreateWorldMovingModel.movableModel.Destroy();
			preCreateWorldMovingModel = null;
		}
	}

	private static InstanceRequest AsyncLoad(string prefabPath, Transform parent, Action<InstanceRequest> completed = null)
	{
		return AsyncLoad(prefabPath, parent, Vector3.zero, Vector3.one, Quaternion.identity, completed);
	}

	private static InstanceRequest AsyncLoad(string prefabPath, Transform parent, Vector3 pos, Vector3 scale, Quaternion rotation, Action<InstanceRequest> completed = null)
	{
		InstanceRequest instance = GameEntry.Resource.InstantiateAsync(prefabPath);
		instance.completed += delegate
		{
			GameObject gameObject = instance.gameObject;
			if (gameObject != null && parent != null && parent.gameObject != null)
			{
				gameObject.transform.SetParent(parent);
				gameObject.SetActive(value: true);
				gameObject.transform.localPosition = pos;
				gameObject.transform.localScale = scale;
				gameObject.transform.localRotation = rotation;
			}
			else if (gameObject != null)
			{
				gameObject.SetActive(value: false);
			}
			if (completed != null)
			{
				completed(instance);
			}
		};
		return instance;
	}
}
