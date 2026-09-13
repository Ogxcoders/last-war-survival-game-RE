using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BitBenderGames;
using GameFramework;
using Protobuf;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VEngine;
using XLua;

public class WorldScene : MonoBehaviour, SceneInterface
{
	public enum WorldBIType
	{
		PlayerBuilding,
		WorldResource,
		MonsterTroop
	}

	public interface ISelfMarchUpdateObserver
	{
		ulong id { get; }

		void UpdateSelfMarch(object data);
	}

	private class SelfMarchUpdateObserverCompare : IComparer<ISelfMarchUpdateObserver>
	{
		public int Compare(ISelfMarchUpdateObserver x, ISelfMarchUpdateObserver y)
		{
			ulong id = x.id;
			ulong id2 = y.id;
			if (id >= id2)
			{
				if (id <= id2)
				{
					return 0;
				}
				return 1;
			}
			return -1;
		}
	}

	private const int LOD_MIN = 1;

	private const int LOD_MAX = 8;

	private static Dictionary<int, int> biShowedTimes;

	private static Dictionary<int, int> biNotShowedTimes;

	private static Dictionary<int, float> lodRemainSec = new Dictionary<int, float>();

	private static Dictionary<int, float> lodDragDistance = new Dictionary<int, float>();

	private static float sendBITimer = 0f;

	private static Dictionary<string, object> biParams;

	private static string[] lodAttributeName = new string[8] { "completenum", "computenum1", "computenum2", "consume_num", "cur_num", "i_num", "i_refresh_num", "i_common_num" };

	private static float sendLodBITimer = 0f;

	private static int currentDragLod = -1;

	private static float dragDistance = 0f;

	private static Vector3 lastCameraPos = Vector3.zero;

	private static float dragCoolTimer = 0f;

	private static Dictionary<int, int> lodShowedTimes = new Dictionary<int, int>(16);

	private static Dictionary<int, int> lodNotShowedTimes = new Dictionary<int, int>(16);

	private static StringBuilder sbForBI = new StringBuilder();

	private const float THRESHOLD_SPD_FLASH = 0.5f;

	private const float THRESHOLD_SPD_METEOR = 1f;

	private const float THRESHOLD_SPD_BOY = 2f;

	private static int spdCountFlash = 0;

	private static int spdCountMeteor = 0;

	private static int spdCountBoy = 0;

	private static int spdCountOther = 0;

	public const int kTileCountX = 1000;

	public const int kTileCountY = 1000;

	private const int kBlockSize = 500;

	public const int MaxHappyLv = 200;

	private static readonly Vector2Int kBlockCount = new Vector2Int(2, 2);

	public static Dictionary<int, Dictionary<int, string>> ModelPathDic = new Dictionary<int, Dictionary<int, string>>();

	public static long selectMarchUuid = 0L;

	protected InstanceRequest sceneInst;

	private HashSet<MonoBehaviour> activePhysicObj = new HashSet<MonoBehaviour>();

	protected Transform dynamicObjNode;

	private static SortedSet<ISelfMarchUpdateObserver> _selfMarchUpdateObservers = null;

	public static bool ENABLE_DYNAMIC_OBJ_POOL = false;

	protected Transform buildBubbleNode;

	protected new GameObject gameObject;

	private float _saveCameraHeightMin;

	private float _saveCameraHeightMax;

	protected List<WorldManagerBase> subModules = new List<WorldManagerBase>();

	protected WorldLodManager LodManager;

	private WorldCityTruckManager CityTruckManager;

	private WorldTileUnlockManager TileUnlockManager;

	private WorldCloudManager WorldCloudManager;

	public WorldFogManager mWorldFogManager;

	private bool _init_Use_lw_Aoi_Flag;

	private bool _USE_LW_AOI = true;

	private Asset goAsset;

	private GameObject goAssetGO;

	private static Dictionary<Type, string> PostprocessSettingKeys = new Dictionary<Type, string>
	{
		[typeof(Bloom)] = "QualitySetting.PostProcess.Bloom",
		[typeof(Tonemapping)] = "QualitySetting.PostProcess.Tonemapping",
		[typeof(LiftGammaGain)] = "QualitySetting.PostProcess.LiftGammaGain",
		[typeof(DepthOfField)] = "QualitySetting.PostProcess.DepthOfField"
	};

	private float _lastUpdateCameraHeight = -1f;

	private Transform camerTran;

	private const float _DefaultScaleOverRide = 0.019157087f;

	private InstanceRequest gfxConsoleRequest;

	private int _AUTO_INC_SYNC_TIMELINE_HANDLE;

	private AutoDisposePoolManager _poolManager;

	public Vector2Int BlockCount => kBlockCount;

	public int BlockSize => 500;

	public float TileSize => 2f;

	public Vector2Int TileCount { get; private set; }

	public Transform DynamicObjNode => dynamicObjNode;

	public Transform BuildBubbleNode => buildBubbleNode;

	public GameObject SceneInstanceGameObject => sceneInst?.gameObject;

	public Transform Transform => gameObject.transform;

	public int CurTileCountXMin { get; private set; }

	public int CurTileCountXMax { get; private set; }

	public int CurTileCountYMin { get; private set; }

	public int CurTileCountYMax { get; private set; }

	public int WorldSize { get; private set; }

	public FakeModelManager FakeModelManager { get; private set; }

	public WorldCamera Camera { get; protected set; }

	private WorldCulling Culling { get; set; }

	protected WorldInputManager InputManager { get; set; }

	public WorldStaticManager StaticManager { get; protected set; }

	public WorldTroopManager TroopManager { get; protected set; }

	private WorldTriggerObjManager TriggerObjManager { get; set; }

	private WorldTriggerDataManager TriggerDataManager { get; set; }

	private WorldWerewolfManager WerewolfDataManager { get; set; }

	private MultiKillPointManager MultiKillPointManager { get; set; }

	private MultiKillDataManager MultiKillDataManager { get; set; }

	private LuaCacheDataManager LuaCacheDataManager { get; set; }

	private WorldVFXManager VFXManager { get; set; }

	private HSRTroopManager HSRTroopMgr { get; set; }

	public WorldPointManager PointManager { get; protected set; }

	public WorldTroopLineManager TroopLineManager { get; protected set; }

	public WorldMarchDataManager MarchDataManager { get; private set; }

	private WorldRobotManager WorldRobotManager { get; set; }

	protected WorldCollectAnimalManager WorldCollectAnimalManager { get; set; }

	private WorldArmyCollectAnimalManager WorldArmyCollectAnimalManager { get; set; }

	private WorldMapZoneManager WorldMapZoneManager { get; set; }

	public WorldIconRendererFacade IconRendererFacade { get; protected set; }

	public WorldWeatherManager WorldWeatherManager { get; private set; }

	public WorldMapGridRenderer MapGridRenderer { get; private set; }

	public WorldMapElectricityRenderer MapElectricityRenderer { get; private set; }

	public bool USE_LW_AOI
	{
		get
		{
			if (!_init_Use_lw_Aoi_Flag)
			{
				_USE_LW_AOI = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetUseLWAoi");
				_init_Use_lw_Aoi_Flag = true;
			}
			return _USE_LW_AOI;
		}
	}

	public int CurrentLodLevel => Camera?.CurrentLodLevel ?? 1;

	public bool EnableWorldIconGPUInstancing { get; private set; }

	public bool EnableWorldMonsterGPUInstancing { get; private set; }

	public bool EnableWorldAssistanceOpt => PointManager?.EnableWorldAssistanceOpt ?? false;

	public bool hasReceiveViewPointsReply { get; set; }

	private Transform CameraTran
	{
		get
		{
			if (camerTran == null)
			{
				camerTran = Camera.__camera?.transform;
			}
			return camerTran;
		}
	}

	public float LodCameraDistanceScale { get; private set; } = 1f;

	public Vector3 CurTarget => Camera.CurTarget;

	public float InitZoom => Camera?.InitZoom ?? 0f;

	public float Zoom
	{
		get
		{
			return Camera?.Zoom ?? 0f;
		}
		set
		{
			Camera.Zoom = value;
		}
	}

	public bool CanMoving
	{
		get
		{
			return Camera.CanMoving;
		}
		set
		{
			Camera.CanMoving = value;
		}
	}

	public bool DisableClampToEdge { get; set; }

	public TouchInputController TouchInputController => Camera.TouchInputController;

	public bool IsFocus => Camera.IsFocus;

	public long TrackMarchId => Camera.TrackMarchId;

	public Vector2Int CurTilePos => Camera.CurTilePos;

	public Vector2Int CurTilePosClamped => Camera.CurTilePosClamped;

	public bool Enabled
	{
		get
		{
			return Camera.Enabled;
		}
		set
		{
			Camera.Enabled = value;
		}
	}

	public int frameBufferWidth => Camera.frameBufferWidth;

	public int frameBufferHeight => Camera.frameBufferHeight;

	public int curIndex
	{
		get
		{
			return InputManager.curIndex;
		}
		set
		{
			InputManager.curIndex = value;
		}
	}

	public Vector2Int curTouchTile
	{
		get
		{
			return InputManager.curTouchTile;
		}
		set
		{
			InputManager.curTouchTile = value;
		}
	}

	public Vector3 curTouchPoint
	{
		get
		{
			return InputManager.curTouchPoint;
		}
		set
		{
			InputManager.curTouchPoint = value;
		}
	}

	public long marchUuid
	{
		get
		{
			return InputManager.marchUuid;
		}
		set
		{
			InputManager.marchUuid = value;
		}
	}

	public List<int> touchPickablePos
	{
		get
		{
			return InputManager.touchPickablePos;
		}
		set
		{
			InputManager.touchPickablePos = value;
		}
	}

	public ITouchPickable SelectBuild
	{
		get
		{
			return InputManager.SelectBuild;
		}
		set
		{
			InputManager.SelectBuild = value;
		}
	}

	public FakeWorldBuilding preCreateBuild
	{
		get
		{
			return FakeModelManager.preCreateBuild;
		}
		set
		{
			FakeModelManager.preCreateBuild = value;
		}
	}

	public Queue<FakeWorldBuilding> placeFalseBuild
	{
		get
		{
			return FakeModelManager.placeFalseBuild;
		}
		set
		{
			FakeModelManager.placeFalseBuild = value;
		}
	}

	public AutoDisposePoolManager poolManager => _poolManager;

	public event Action _afterUpdate;

	public event Action AfterUpdate
	{
		add
		{
			_afterUpdate += value;
		}
		remove
		{
			_afterUpdate -= value;
		}
	}

	public static void RecordWorldObjectState(WorldBIType biType, bool showed)
	{
		if (biShowedTimes == null)
		{
			biShowedTimes = new Dictionary<int, int>();
			biNotShowedTimes = new Dictionary<int, int>();
		}
		Dictionary<int, int> dictionary = (showed ? biShowedTimes : biNotShowedTimes);
		if (dictionary.TryGetValue((int)biType, out var value))
		{
			dictionary[(int)biType] = value + 1;
		}
		else
		{
			dictionary[(int)biType] = 1;
		}
	}

	private static void BIUpdate(int lod, float deltaTime)
	{
		sendBITimer += deltaTime;
		sendLodBITimer += deltaTime;
		if (lodRemainSec.TryGetValue(lod, out var value))
		{
			lodRemainSec[lod] = value + deltaTime;
		}
		else
		{
			lodRemainSec[lod] = value;
		}
		if (sendBITimer >= 60f)
		{
			BIDump();
		}
		if (sendLodBITimer >= 35f)
		{
			LodBIDump();
			LodDragBIDump();
			sendLodBITimer = 0f;
		}
	}

	private static void BIDump()
	{
		LegacyShowedStateBI();
		DumpLodShowedStates();
		sendBITimer = 0f;
	}

	private static void LegacyShowedStateBI()
	{
		if (biShowedTimes != null && (biShowedTimes.Count != 0 || biNotShowedTimes.Count != 0))
		{
			biParams = biParams ?? new Dictionary<string, object>();
			biParams.Clear();
			biShowedTimes.TryGetValue(0, out var value);
			biNotShowedTimes.TryGetValue(0, out var value2);
			biShowedTimes.TryGetValue(1, out var value3);
			biNotShowedTimes.TryGetValue(1, out var value4);
			biShowedTimes.TryGetValue(2, out var value5);
			biNotShowedTimes.TryGetValue(2, out var value6);
			biParams["playerShowed"] = value;
			biParams["playerNotShowed"] = value2;
			biParams["worldResShowed"] = value3;
			biParams["worldResNotShowed"] = value4;
			biParams["monsterShowed"] = value5;
			biParams["monsterNotShowed"] = value6;
			PostEventLog.TrackMap("WorldObjectShowedStateRecord", biParams);
			biShowedTimes.Clear();
			biNotShowedTimes.Clear();
		}
	}

	private static void LodBIDump()
	{
		if (lodRemainSec.Count <= 0)
		{
			return;
		}
		biParams = biParams ?? new Dictionary<string, object>();
		biParams.Clear();
		int i = 0;
		for (int num = lodAttributeName.Length; i < num; i++)
		{
			int key = i + 1;
			if (lodRemainSec.TryGetValue(key, out var value))
			{
				string key2 = lodAttributeName[i];
				biParams[key2] = value;
			}
		}
		PostEventLog.TrackMap("WorldLodTimeRecord", biParams);
		lodRemainSec.Clear();
	}

	private void AccumulateCameraDragDistance(int lod, Vector3 cameraPosition)
	{
		if (currentDragLod != lod)
		{
			RecordCameraDragDistance();
			currentDragLod = lod;
			lastCameraPos = cameraPosition;
			dragDistance = 0f;
			dragCoolTimer = 0.5f;
			return;
		}
		if (dragCoolTimer > 0f)
		{
			dragCoolTimer -= Time.deltaTime;
			lastCameraPos = cameraPosition;
			return;
		}
		Vector3 vector = cameraPosition - lastCameraPos;
		lastCameraPos = cameraPosition;
		vector.y = 0f;
		float sqrMagnitude = vector.sqrMagnitude;
		if (!(sqrMagnitude < 50f) && !(sqrMagnitude > 100000f))
		{
			dragDistance += Mathf.Sqrt(sqrMagnitude);
		}
	}

	private static void RecordCameraDragDistance()
	{
		if (currentDragLod >= 1 && currentDragLod <= 8)
		{
			if (lodDragDistance.TryGetValue(currentDragLod, out var value))
			{
				lodDragDistance[currentDragLod] = value + dragDistance;
			}
			else
			{
				lodDragDistance[currentDragLod] = dragDistance;
			}
		}
		dragDistance = 0f;
	}

	private void InitWorldSceneBI()
	{
		RecordCameraDragDistance();
		currentDragLod = -1;
	}

	private static void LodDragBIDump()
	{
		RecordCameraDragDistance();
		if (lodDragDistance.Count <= 0)
		{
			return;
		}
		biParams = biParams ?? new Dictionary<string, object>();
		biParams.Clear();
		float num = 0f;
		int i = 0;
		for (int num2 = lodAttributeName.Length; i < num2; i++)
		{
			int key = i + 1;
			if (lodDragDistance.TryGetValue(key, out var value))
			{
				string key2 = lodAttributeName[i];
				biParams[key2] = value;
				num += value;
			}
		}
		lodDragDistance.Clear();
		if (!(num < 10f))
		{
			PostEventLog.TrackMap("WorldLodDragRecord", biParams);
		}
	}

	private static void BICameraLookAt()
	{
		dragCoolTimer = 0.25f;
	}

	public static void RecordLodState(LodType lodType, bool showed)
	{
		Dictionary<int, int> dictionary = (showed ? lodShowedTimes : lodNotShowedTimes);
		if (dictionary.TryGetValue((int)lodType, out var value))
		{
			dictionary[(int)lodType] = value + 1;
		}
		else
		{
			dictionary[(int)lodType] = 1;
		}
	}

	private static void DumpLodShowedStates()
	{
		if (lodShowedTimes.Count <= 0 && lodNotShowedTimes.Count <= 0 && spdCountFlash <= 0 && spdCountMeteor <= 0 && spdCountBoy <= 0 && spdCountOther <= 0)
		{
			return;
		}
		biParams = biParams ?? new Dictionary<string, object>();
		biParams.Clear();
		sbForBI.Length = 0;
		if (lodShowedTimes.Count > 0)
		{
			foreach (KeyValuePair<int, int> lodShowedTime in lodShowedTimes)
			{
				if (sbForBI.Length > 0)
				{
					sbForBI.Append(";");
				}
				sbForBI.AppendFormat("{0},{1}", lodShowedTime.Key, lodShowedTime.Value);
			}
			biParams["param1"] = sbForBI.ToString();
		}
		sbForBI.Length = 0;
		if (lodNotShowedTimes.Count > 0)
		{
			foreach (KeyValuePair<int, int> lodNotShowedTime in lodNotShowedTimes)
			{
				if (sbForBI.Length > 0)
				{
					sbForBI.Append(";");
				}
				sbForBI.AppendFormat("{0},{1}", lodNotShowedTime.Key, lodNotShowedTime.Value);
			}
			biParams["param2"] = sbForBI.ToString();
		}
		biParams["completenum"] = spdCountFlash;
		biParams["computenum1"] = spdCountMeteor;
		biParams["computenum2"] = spdCountBoy;
		biParams["i_common_num"] = spdCountOther;
		PostEventLog.TrackMap("WorldLodObjShowedState", biParams);
		lodShowedTimes.Clear();
		lodNotShowedTimes.Clear();
		spdCountFlash = 0;
		spdCountMeteor = 0;
		spdCountBoy = 0;
		spdCountOther = 0;
	}

	public static void RecordWorldCityFastBoy(float timeElapsed)
	{
		if (timeElapsed < 0.5f)
		{
			spdCountFlash++;
		}
		else if (timeElapsed < 1f)
		{
			spdCountMeteor++;
		}
		else if (timeElapsed < 2f)
		{
			spdCountBoy++;
		}
		else
		{
			spdCountOther++;
		}
	}

	public void Init(GameObject go)
	{
		ModelPathDic.Clear();
		gameObject = go;
		gameObject.SetActive(value: true);
		CurTileCountXMin = 0;
		CurTileCountXMax = 1000;
		CurTileCountYMin = 0;
		CurTileCountYMax = 1000;
		InitWorldSceneBI();
		TileCount = new Vector2Int(1000, 1000);
		_lastUpdateCameraHeight = -1f;
		if (SeasonDataManager.Instance.InSeasonBigMapMode())
		{
			SetWorldSize(3000);
		}
		else
		{
			SetWorldSize(1000);
		}
		WorldInstancingRenderers.Init();
		WorldIconRendererFacade.InitConfig();
		GameEntry.Resource.PreloadAsset("Assets/Main/Prefabs/World/Terrain_World.prefab", typeof(GameObject));
		SetFogVisible(visible: false);
		if (!SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH)
		{
			_selfMarchUpdateObservers = new SortedSet<ISelfMarchUpdateObserver>(new SelfMarchUpdateObserverCompare());
			GameEntry.Event.Subscribe(EventId.MarchItemUpdateSelf, MarchItemUpdateSelfHandler);
		}
		GameEntry.Event.Subscribe(EventId.MakeWorldColorDirty, OnWorldColorDirty);
		EnableWorldIconGPUInstancing = GameEntry.Data?.Player?.CheckSwitch("world_building_optimize_mode", defaultVal: false) ?? false;
		EnableWorldMonsterGPUInstancing = GameEntry.Data?.Player?.CheckSwitch("world_monster_optimize_mode", defaultVal: false) ?? false;
		Log.Info($"[WorldScene][GpuInstancing]Enable world monster gpu instancing:{EnableWorldMonsterGPUInstancing}");
		ENABLE_DYNAMIC_OBJ_POOL = ClientSwitch.IsOn(3);
		Log.Info($"[WorldDynamicObjPool] enable: {ENABLE_DYNAMIC_OBJ_POOL}");
	}

	public static void AddMarchItemUpdateSelfObserver(ISelfMarchUpdateObserver observer)
	{
		_selfMarchUpdateObservers?.Add(observer);
	}

	public static void RemoveMarchItemUpdateSelfObserver(ISelfMarchUpdateObserver observer)
	{
		_selfMarchUpdateObservers?.Remove(observer);
	}

	private void MarchItemUpdateSelfHandler(object obj)
	{
		if (_selfMarchUpdateObservers == null)
		{
			GameEntry.Event.Unsubscribe(EventId.MarchItemUpdateSelf, MarchItemUpdateSelfHandler);
			return;
		}
		foreach (ISelfMarchUpdateObserver selfMarchUpdateObserver in _selfMarchUpdateObservers)
		{
			try
			{
				selfMarchUpdateObserver.UpdateSelfMarch(obj);
			}
			catch (Exception ex)
			{
				Log.Error("HandleEvent MarchItemUpdateSelf exception!!! exception:{0}", ex.ToString());
			}
		}
	}

	public void CreateScene(Action callback = null)
	{
		GameEntry.Lua.Call("CSharpCallLuaInterface.SetIsInCity", param1: false);
		GameEntry.Data.Building.SetMainPos();
		SceneManager.MarchDataMgr.WorldGetMarchInfos();
		DisableClampToEdge = false;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (dynamicObjNode == null)
		{
			dynamicObjNode = new GameObject("dynamicObj").transform;
			dynamicObjNode.SetParent(gameObject.transform, worldPositionStays: false);
		}
		if (buildBubbleNode == null)
		{
			buildBubbleNode = new GameObject("buildBubbleNode").transform;
			buildBubbleNode.SetParent(dynamicObjNode, worldPositionStays: false);
		}
		hasReceiveViewPointsReply = false;
		if (subModules.Count != 0)
		{
			return;
		}
		Camera = AddSubModule<WorldCamera>();
		InputManager = AddSubModule<WorldInputManager>();
		TroopManager = AddSubModule<WorldTroopManager>();
		TriggerDataManager = AddSubModule<WorldTriggerDataManager>();
		TriggerObjManager = AddSubModule<WorldTriggerObjManager>();
		WerewolfDataManager = AddSubModule<WorldWerewolfManager>();
		VFXManager = AddSubModule<WorldVFXManager>();
		HSRTroopMgr = AddSubModule<HSRTroopManager>();
		LuaCacheDataManager = AddSubModule<LuaCacheDataManager>();
		MultiKillDataManager = AddSubModule<MultiKillDataManager>();
		MultiKillPointManager = AddSubModule<MultiKillPointManager>();
		StaticManager = AddSubModule<WorldStaticManager>();
		PointManager = AddSubModule<WorldPointManager>();
		TroopLineManager = AddSubModule<WorldTroopLineManager>();
		MarchDataManager = SceneManager.MarchDataMgr;
		Culling = AddSubModule<WorldCulling>();
		WorldRobotManager = AddSubModule<WorldRobotManager>();
		WorldCollectAnimalManager = AddSubModule<WorldCollectAnimalManager>();
		WorldArmyCollectAnimalManager = AddSubModule<WorldArmyCollectAnimalManager>();
		FakeModelManager = AddSubModule<FakeModelManager>();
		LodManager = AddSubModule<WorldLodManager>();
		WorldMapZoneManager = AddSubModule<WorldMapZoneManager>();
		TileUnlockManager = AddSubModule<WorldTileUnlockManager>();
		MapGridRenderer = AddSubModule<WorldMapGridRenderer>();
		IconRendererFacade = AddSubModule<WorldIconRendererFacade>();
		MapElectricityRenderer = AddSubModule<WorldMapElectricityRenderer>();
		mWorldFogManager = AddSubModule<WorldFogManager>();
		WorldWeatherManager = AddSubModule<WorldWeatherManager>();
		foreach (WorldManagerBase subModule in subModules)
		{
			subModule.Init();
		}
		LoadTerrainAssets(callback);
	}

	protected void LoadTerrainAssets(Action callback)
	{
		StaticManager.LoadTerrainAssets(delegate
		{
			if (sceneInst == null)
			{
				string prefabPath = "Assets/Main/Prefabs/World/Scene_World.prefab";
				SceneSkinMeta meta = SceneSkinManager.Instance.GetCurSkinMeta();
				if (meta != null)
				{
					if (meta.IsSnowMode())
					{
						prefabPath = "Assets/Main/Prefabs/World/Scene_World_SnowS2.prefab";
					}
					else if (meta.IsMummyMode())
					{
						prefabPath = "Assets/Main/SeasonRes/S3/Prefabs/World/Scene_World_MummyS3.prefab";
					}
				}
				sceneInst = GameEntry.Resource.InstantiateAsync(prefabPath);
				sceneInst.completed += delegate
				{
					if (gameObject == null)
					{
						Log.Error("WorldScene::gameobject is null when instantiate:Assets/Main/Prefabs/World/Scene_World.prefab complete");
					}
					DCPlayer player = GameEntry.Data.Player;
					int selfServerId = player.GetSelfServerId();
					int worldMainPos = GameEntry.Data.Building.GetWorldMainPos();
					sceneInst.gameObject.transform.SetParent(gameObject.transform);
					SetPostProcessQuality();
					Camera.SetTouchInputControllerEnable(able: true);
					Camera.AfterUpdate += AfterCameraUpdate;
					Camera.Lookat(TileIndexToWorld(worldMainPos, selfServerId));
					Camera.MarkLodChanged();
					PointManager.StartViewRequest();
					if (player.GetWorldId() <= 0)
					{
						if (meta != null && meta.mapType != 0 && meta.camera_scaling > 0.1f)
						{
							SetCameraMaxHeight((int)(5000f * meta.camera_scaling));
						}
						GameEntry.Lua.Call("CSharpCallLuaInterface.CheckReplaceMain");
					}
					else
					{
						GameEntry.Lua.Call("CSharpCallLuaInterface.OnGotoSpecialWorld");
					}
					callback?.Invoke();
				};
			}
			else
			{
				Log.Info("CreateScene sceneInst == null");
			}
		});
	}

	public void Uninit()
	{
		GameEntry.Event.Unsubscribe(EventId.MakeWorldColorDirty, OnWorldColorDirty);
		if (!SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH)
		{
			GameEntry.Event.Unsubscribe(EventId.MarchItemUpdateSelf, MarchItemUpdateSelfHandler);
			_selfMarchUpdateObservers?.Clear();
			_selfMarchUpdateObservers = null;
		}
		if (Camera != null)
		{
			Camera.AfterUpdate -= AfterCameraUpdate;
		}
		WorldZoneEdgeDataCache.CleanCache();
		WorldPointCityStronghold.CleanAsset();
		ModelPathDic.Clear();
		foreach (WorldManagerBase subModule in subModules)
		{
			subModule.UnInit();
		}
		subModules.Clear();
		if (sceneInst != null)
		{
			sceneInst.Destroy();
			sceneInst = null;
		}
		if (buildBubbleNode != null)
		{
			UnityEngine.Object.Destroy(buildBubbleNode.gameObject);
			buildBubbleNode = null;
		}
		ClearDynamicObjPools();
		if (dynamicObjNode != null)
		{
			UnityEngine.Object.Destroy(dynamicObjNode.gameObject);
			dynamicObjNode = null;
		}
		BIDump();
		LodBIDump();
		WorldInstancingRenderers.Dispose();
		if (goAssetGO != null)
		{
			UnityEngine.Object.Destroy(goAssetGO);
			goAssetGO = null;
		}
		if (goAsset != null)
		{
			goAsset.Release();
			goAsset = null;
		}
	}

	private T AddSubModule<T>() where T : WorldManagerBase
	{
		T val = (T)Activator.CreateInstance(typeof(T), new object[1] { this });
		subModules.Add(val);
		return val;
	}

	private void SetPostProcessQuality()
	{
		if (sceneInst == null || sceneInst.gameObject == null)
		{
			return;
		}
		Volume componentInChildren = sceneInst.gameObject.GetComponentInChildren<Volume>(includeInactive: true);
		if (componentInChildren == null || componentInChildren.profile == null)
		{
			return;
		}
		componentInChildren.enabled = true;
		bool flag = false;
		List<VolumeComponent> list = new List<VolumeComponent>();
		componentInChildren.profile.TryGetAllSubclassOf(typeof(VolumeComponent), list);
		foreach (VolumeComponent item in list)
		{
			if (PostprocessSettingKeys.TryGetValue(item.GetType(), out var value))
			{
				if (GameEntry.Setting.GetInt(value, 0) == 0)
				{
					item.active = false;
					continue;
				}
				item.active = true;
				flag = true;
			}
		}
		if (flag)
		{
			Camera.EnablePostProcess();
			return;
		}
		Camera.DisablePostProcess();
		componentInChildren.enabled = false;
	}

	public void ChangeQualitySetting()
	{
		SetPostProcessQuality();
		if (StaticManager != null)
		{
			StaticManager.ChangeTerrain();
		}
	}

	public void Update()
	{
		UpdateSubModule();
		BIUpdate(CurrentLodLevel, Time.deltaTime);
		if (_poolManager != null)
		{
			_poolManager.Tick();
		}
		SyncDebugWorldInfo();
	}

	private void UpdateSubModule()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		foreach (WorldManagerBase subModule in subModules)
		{
			try
			{
				subModule.OnUpdate(Time.deltaTime);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}
		_ = (double)(Time.realtimeSinceStartup - realtimeSinceStartup);
		_ = 0.015;
	}

	private void SyncDebugWorldInfo()
	{
		if (GMSwitch.IsGM)
		{
			int param = PointManager?.ObjsCount ?? 0;
			int param2 = MarchDataManager?.AllMarchesCount ?? 0;
			int param3 = TroopManager?.TroopCount ?? 0;
			int param4 = TroopLineManager?.LegacyTroopLineCount ?? 0;
			int param5 = TroopLineManager?.NewTroopLineCount ?? 0;
			int param6 = TilePosToIndex(Camera?.CurTilePos ?? Vector2Int.zero);
			float f = Camera?.GetLodDistance() ?? 0f;
			GameEntry.Lua?.Call("GMUtils.SyncDebugWorldInfo", param, param2, param3, param4, param5);
			GameEntry.Lua?.Call("GMUtils.SyncWorldCameraInfo", param6, Mathf.CeilToInt(f));
		}
	}

	public void FixedUpdate()
	{
	}

	public Vector3 TileToWorld(Vector2Int tilePos)
	{
		return TileCoord.TileToWorld(tilePos, GameEntry.Data?.Player?.GetCurServerId() ?? 0);
	}

	public Vector3 TileToWorld(int tilePosX, int tilePosY)
	{
		return TileCoord.TileToWorld(tilePosX, tilePosY, GameEntry.Data?.Player?.GetCurServerId() ?? 0);
	}

	public Vector2Int WorldToTile(Vector3 worldPos)
	{
		return TileCoord.WorldToTile(worldPos);
	}

	public Vector3 SnapToTileCenter(Vector3 worldPos)
	{
		return TileCoord.SnapToTileCenter(worldPos);
	}

	public Vector3 TileFloatToWorld(Vector2 tilePos)
	{
		return TileCoord.TileFloatToWorld(tilePos, GameEntry.Data?.Player?.GetCurServerId() ?? 0);
	}

	public Vector3 TileFloatToWorld(float x, float y)
	{
		return TileCoord.TileFloatToWorld(x, y, GameEntry.Data?.Player?.GetCurServerId() ?? 0);
	}

	public Vector2 WorldToTileFloat(Vector3 worldPos)
	{
		return TileCoord.WorldToTileFloat(worldPos);
	}

	public Vector2Int IndexToTilePos(int index)
	{
		return TileCoord.IndexToTilePos(index, TileCount);
	}

	public int TilePosToIndex(Vector2Int tilePos)
	{
		return TileCoord.TilePosToIndex(tilePos, TileCount);
	}

	public Vector3 TileIndexToWorld(int index)
	{
		return TileCoord.TileIndexToWorld(index, TileCount, GameEntry.Data?.Player?.GetCurServerId() ?? 0);
	}

	public Vector3 TileIndexToWorld(int index, int serverId)
	{
		return TileCoord.TileIndexToWorld(index, TileCount, serverId);
	}

	public int WorldToTileIndex(Vector3 pos)
	{
		return TileCoord.WorldToTileIndex(pos, TileCount);
	}

	public float TileDistance(Vector2Int a, Vector2Int b)
	{
		return TileCoord.TileDistance(a, b);
	}

	public void SetWorldSize(int size)
	{
		if (WorldSize != size && size > 0)
		{
			WorldSize = size;
			if (size <= 1000)
			{
				int num = size / 2;
				CurTileCountXMin = Math.Max(0, 500 - num);
				CurTileCountXMax = Math.Min(999, 500 + num - 1);
				CurTileCountYMin = Math.Max(0, 500 - num);
				CurTileCountYMax = Math.Min(999, 500 + num - 1);
			}
			else
			{
				CurTileCountXMin = 0;
				CurTileCountXMax = size;
				CurTileCountYMin = 0;
				CurTileCountYMax = size;
			}
		}
	}

	public void SetWorldSize(int width, int height)
	{
		int num = (WorldSize = Mathf.Max(width, height));
		if (num <= 1000)
		{
			int num3 = width / 2;
			int num4 = height / 2;
			CurTileCountXMin = Math.Max(0, 500 - num3);
			CurTileCountXMax = Math.Min(999, 500 + num3 - 1);
			CurTileCountYMin = Math.Max(0, 500 - num4);
			CurTileCountYMax = Math.Min(999, 500 + num4 - 1);
		}
		else
		{
			CurTileCountXMin = 0;
			CurTileCountXMax = width;
			CurTileCountYMin = 0;
			CurTileCountYMax = height;
		}
	}

	public void SetMapZoneActive(bool active)
	{
		WorldMapZoneManager.SetMapZoneActive(active);
	}

	public int GetBuildOffsetRangeByBuildId(int buildId)
	{
		return PointManager.GetBuildOffsetRangeByBuildId(buildId);
	}

	private void AfterCameraUpdate()
	{
		if (!DisableClampToEdge)
		{
			Camera.ClampToEdge();
		}
		Camera.RefreshCameraAnchor();
		if (WorldSize > 1000)
		{
			UpdateCrossEdge();
		}
		Transform cameraTran = CameraTran;
		if (cameraTran != null)
		{
			Vector3 position = cameraTran.position;
			float y = position.y;
			if (!Mathf.Approximately(_lastUpdateCameraHeight, y))
			{
				_lastUpdateCameraHeight = y;
				float num = _lastUpdateCameraHeight / Mathf.Sin(MathF.PI / 180f * cameraTran.eulerAngles.x);
				LodCameraDistanceScale = Mathf.Max(0.019157087f * num, 0f);
				Shader.SetGlobalFloat("_LodCameraDistanceScale", LodCameraDistanceScale);
			}
			AccumulateCameraDragDistance(CurrentLodLevel, position);
		}
		else
		{
			Shader.SetGlobalFloat("_LodCameraDistanceScale", 1f);
		}
		this._afterUpdate?.Invoke();
	}

	public void ChangeServer(int serverId)
	{
		GameEntry.Lua.Call("GoToUtil.GotoServerZone", serverId, Camera.IsInMoveToState());
	}

	public void OnChangeServerRemove()
	{
		PointManager.RemoveAllObject();
	}

	public void RemoveBlackDesert()
	{
		StaticManager.RemoveBlackDesert();
	}

	public void InitBlackBlock()
	{
		StaticManager.InitBlackBlock();
	}

	public GameObject GetDragonLandRangeObj()
	{
		return StaticManager.GetDragonLandRangeObj();
	}

	public void CreateDragonLandRange()
	{
		StaticManager.CreateDragonLandRange();
	}

	public void RemoveDragonLandRange()
	{
		StaticManager.RemoveDragonLandRange();
	}

	public void RemoveDragonLandPoint(int pointIndex)
	{
		StaticManager.RemoveDragonLandPoint(pointIndex);
	}

	private void UpdateCrossEdge()
	{
		Vector3 curTarget = Camera.CurTarget;
		int cameraLookAtServerId = GetCameraLookAtServerId(curTarget);
		if (cameraLookAtServerId > 0 && cameraLookAtServerId != GameEntry.Data.Player.GetCurServerId())
		{
			ChangeServer(cameraLookAtServerId);
		}
	}

	private int GetCameraLookAtServerId(Vector3 lookAtWorldPos)
	{
		return SeasonDataManager.Instance.GetServerIdFromWorldPos(lookAtWorldPos);
	}

	public void RegisterPhysics(MonoBehaviour obj)
	{
	}

	public void UnregisterPhysics(MonoBehaviour obj)
	{
	}

	public List<int> FindPathForTruck(Vector2Int start, Vector2Int end)
	{
		return FindPathForTruck(TilePosToIndex(start), TilePosToIndex(end));
	}

	public List<int> FindPathForTruck(int startIndex, int endIndex)
	{
		List<int> list = new List<int>();
		int num = startIndex;
		Vector2Int vector2Int = IndexToTilePos(startIndex);
		Vector2Int vector2Int2 = IndexToTilePos(endIndex);
		int num2 = vector2Int2.x - vector2Int.x;
		int num3 = ((num2 > 0) ? num2 : (-num2));
		int num4 = vector2Int2.y - vector2Int.y;
		int num5 = ((num4 > 0) ? num4 : (-num4));
		while (num != endIndex)
		{
			int num6 = 0;
			if (num3 > 0)
			{
				num6 = ((num2 > 0) ? 1 : (-1));
				num3--;
				num = GetIndexByOffset(num, num6);
				list.Add(num);
				continue;
			}
			int y = 0;
			if (num5 > 0)
			{
				y = ((num4 > 0) ? 1 : (-1));
				num5--;
			}
			num = GetIndexByOffset(num, 0, y);
			list.Add(num);
		}
		return list;
	}

	public int GetIndexByOffset(int index, int x = 0, int y = 0)
	{
		int indexByOffsetX = GetIndexByOffsetX(index, x);
		if (indexByOffsetX > 0)
		{
			return GetIndexByOffsetY(indexByOffsetX, y);
		}
		return 0;
	}

	private int GetIndexByOffsetX(int index, int offset = 1)
	{
		int num = index - 1;
		num %= 1000;
		num += offset;
		if (num >= 0 && num < 1000)
		{
			return index + offset;
		}
		return 0;
	}

	private int GetIndexByOffsetY(int index, int offset = 1)
	{
		int num = index - 1;
		num /= 1000;
		num += offset;
		if (num >= 0 && num < 1000)
		{
			return index + 1000 * offset;
		}
		return 0;
	}

	public int GetIndexByOffsetByDirection(int index, int dir)
	{
		return dir switch
		{
			4 => GetIndexByOffset(index, 0, -1), 
			3 => GetIndexByOffset(index, -1), 
			2 => GetIndexByOffset(index, 1), 
			1 => GetIndexByOffset(index, 0, 1), 
			_ => 0, 
		};
	}

	public void OnPlayerBankruptcyFinish(string uid)
	{
	}

	public void ShowBattleBlood(object param, string path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab")
	{
		InstanceRequest temp = GameEntry.Resource.InstantiateAsync(path);
		temp.completed += delegate
		{
			GameObject obj = temp.gameObject;
			obj.transform.SetParent(dynamicObjNode);
			obj.GetComponent<BattleDecBloodTip>().CSShow(param, temp);
		};
	}

	public Vector2Int GetTouchTilePos()
	{
		return WorldToTile(GetTouchPoint());
	}

	public Vector3 GetTouchPoint()
	{
		return Camera.GetRaycastGroundPoint(Input.mousePosition);
	}

	public Vector3 GetTouchPoint(Vector3 screenPos)
	{
		return Camera.GetRaycastGroundPoint(screenPos);
	}

	public Vector3 GetRaycastGroundPoint(Vector3 screenPos)
	{
		return Camera.GetRaycastGroundPoint(screenPos);
	}

	public void AutoFocus(Vector3 lookat, LookAtFocusState state, float time, bool focusToCenter = true, bool lockView = false, Action onComplete = null)
	{
		Camera.AutoFocus(lookat, state, time, focusToCenter, lockView, onComplete);
		BICameraLookAt();
	}

	public void QuitFocus(float time)
	{
		Camera.QuitFocus(time);
	}

	public float GetLodDistance()
	{
		return Camera.GetLodDistance();
	}

	public float GetLodDistanceByLod(int lod)
	{
		return Camera?.GetLodDistance(lod) ?? 0f;
	}

	public Quaternion GetRotation()
	{
		return Camera.GetRotation();
	}

	public void Get_rotation(out float x, out float y, out float z, out float w)
	{
		Quaternion rotation = GetRotation();
		x = rotation.x;
		y = rotation.y;
		z = rotation.z;
		w = rotation.w;
	}

	public float GetPreviousLodDistance()
	{
		return Camera.GetPreviousLodDistance();
	}

	public float GetMapIconScale()
	{
		return Camera.GetMapIconScale();
	}

	public Vector3 GetCameraPos()
	{
		return Camera.GetPosition();
	}

	public Ray ScreenPointToRay(Vector3 pos)
	{
		return Camera.ScreenPointToRay(pos);
	}

	public float GetMinLodDistance()
	{
		return Camera.GetMinLodDistance();
	}

	public Vector3 WorldToScreenPoint(Vector3 worldPos)
	{
		return Camera.WorldToScreenPoint(worldPos);
	}

	public void AutoLookat(Vector3 lookat, float zoom = -1f, float time = 0.2f, Action onComplete = null)
	{
		Camera.AutoLookat(lookat, zoom, time, onComplete);
		BICameraLookAt();
	}

	public void AutoZoom(float zoom, float time = 0.2f, Action onComplete = null)
	{
		Camera.AutoZoom(zoom, time, onComplete);
	}

	public void Lookat(Vector3 lookWorldPosition)
	{
		Camera.Lookat(lookWorldPosition);
		BICameraLookAt();
	}

	public Vector3 ScreenPointToWorld(Vector3 worldPos, float disPlane = 0f)
	{
		BICameraLookAt();
		return Camera.ScreenPointToWorld(worldPos, disPlane);
	}

	public int GetLodLevel()
	{
		return Camera.GetLodLevel();
	}

	public void TrackMarch(long marchId)
	{
		Camera.TrackMarch(marchId);
	}

	public void TrackHSR(long marchId, GameObject go)
	{
		Camera.TrackHSR(marchId, go);
	}

	public void DisablePostProcess()
	{
		Camera.DisablePostProcess();
	}

	public void EnablePostProcess()
	{
		Camera.EnablePostProcess();
	}

	public void SetTouchInputControllerEnable(bool able)
	{
		Camera.SetTouchInputControllerEnable(able);
	}

	public bool GetTouchInputControllerEnable()
	{
		return Camera.GetTouchInputControllerEnable();
	}

	public void StopCameraMove()
	{
		Camera?.StopMove();
	}

	public bool IsCameraInMoveToState()
	{
		return Camera?.IsInMoveToState() ?? false;
	}

	public bool IsCameraInFreeLookStateWithSpeed(float speed)
	{
		return Camera?.IsInFreeLookStateWithAtLeastSpeed(speed) ?? false;
	}

	public int GetIndexByOffset_New(int index, int x = 0, int y = 0)
	{
		int indexByOffsetX = GetIndexByOffsetX(index, x);
		if (indexByOffsetX > 0)
		{
			return GetIndexByOffsetY(indexByOffsetX, y);
		}
		return 0;
	}

	public int GetIndexByOffsetByDirection_New(int index, int dir)
	{
		return dir switch
		{
			4 => GetIndexByOffset_New(index, 0, -1), 
			3 => GetIndexByOffset_New(index, -1), 
			2 => GetIndexByOffset_New(index, 1), 
			1 => GetIndexByOffset_New(index, 0, 1), 
			_ => 0, 
		};
	}

	public void SetZoomParams(int level, float y, float offsetZ, float sensitivity)
	{
		Camera.SetZoomParams(level, y, offsetZ, sensitivity);
	}

	public void SetCameraFOV(float fov)
	{
		Camera.SetFOV(fov);
	}

	public bool IsInMap(Vector2Int pt)
	{
		if (GameEntry.Data.Player.IsInBattleField())
		{
			return true;
		}
		if (pt.x < CurTileCountXMin || pt.x > CurTileCountXMax || pt.y < CurTileCountYMin || pt.y > CurTileCountYMax)
		{
			return false;
		}
		return true;
	}

	public void OnSkinChange(bool ignoreCache)
	{
		if (SeasonDataManager.Instance.InSeasonBigMapMode())
		{
			SetWorldSize(3000);
		}
		else
		{
			SetWorldSize(1000);
		}
		if (!(dynamicObjNode == null))
		{
			StaticManager.OnSkinChange();
			WorldMapZoneManager.OnSkinChange();
			TroopLineManager.OnSkinChange();
		}
	}

	public GameObject GetPeopleById(int index)
	{
		return null;
	}

	public float PausePeopleAndPlayAnim(int index, string anim)
	{
		return 0f;
	}

	public void ResumePeople(int index)
	{
	}

	public bool IsInMapByIndex(int index)
	{
		return IsInMap(IndexToTilePos(index));
	}

	public Vector2Int ClampTilePos(Vector2Int tilePos)
	{
		if (tilePos.x < 0)
		{
			tilePos.x = 0;
		}
		if (tilePos.x > 999)
		{
			tilePos.x = 999;
		}
		if (tilePos.y < 0)
		{
			tilePos.y = 0;
		}
		if (tilePos.y > 999)
		{
			tilePos.y = 999;
		}
		return tilePos;
	}

	public int GetCollectResourceBuildRange()
	{
		return PointManager.GetCollectResourceBuildRange();
	}

	public int GetCollectResourceTile()
	{
		return PointManager.GetCollectResourceTile();
	}

	public int GetGlobalShaderLOD()
	{
		return 1;
	}

	public bool SetGlobalShaderLOD(int level)
	{
		switch ((GlobalShaderLod)level)
		{
		case GlobalShaderLod.LOW:
			Shader.globalMaximumLOD = 201;
			Debug.Log("切换low");
			return false;
		case GlobalShaderLod.MIDDLE:
			Shader.globalMaximumLOD = 401;
			Debug.Log("切换mid");
			return false;
		case GlobalShaderLod.HIGH:
			Shader.globalMaximumLOD = 601;
			Debug.Log("切换high");
			break;
		}
		return false;
	}

	public bool GetProfileTerrainSwitch()
	{
		return StaticManager.GetProfileTerrainSwitch();
	}

	public void ProfileToggleTerrain()
	{
		StaticManager.ProfileToggleTerrain();
	}

	public void ProfileToggleGlass()
	{
	}

	public bool GetProfileBuildingSwitch()
	{
		return false;
	}

	public void ProfileToggleBuilding()
	{
	}

	public bool GetProfileStaticSwitch()
	{
		return false;
	}

	public void ProfileToggleStatic()
	{
	}

	public bool GetHeightFogSwitch()
	{
		return false;
	}

	public void ProfileToggleHeightFog()
	{
	}

	public bool GetGraphySwitch()
	{
		return gfxConsoleRequest != null;
	}

	public void ProfileToggleMarch()
	{
		if (gfxConsoleRequest == null)
		{
			gfxConsoleRequest = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Debug/GFXConsole.prefab");
			gfxConsoleRequest.completed += delegate
			{
			};
		}
		else
		{
			gfxConsoleRequest.Destroy();
			gfxConsoleRequest = null;
		}
	}

	public void HandleSandWormUpdate(BuildPointInfo bi, SandWormAnim anim)
	{
		PointManager.HandleSandWormUpdate(bi, anim);
	}

	public ExplorePointInfo GetExplorePointInfoByIndex(int pointIndex)
	{
		return PointManager.GetExplorePointInfoByIndex(pointIndex);
	}

	public SamplePointInfo GetSamplePointInfoByIndex(int pointIndex)
	{
		return PointManager.GetSamplePointInfoByIndex(pointIndex);
	}

	public DetectRetryTaskPointInfo GetDetectRetryTaskPointInfo(int pointIndex)
	{
		return PointManager.GetDetectRetryTaskPointInfo(pointIndex);
	}

	public DetectAttackCityS0TaskPointInfo GetDetectAttackCityS0TaskPointInfo(int pointIndex)
	{
		return PointManager.GetDetectAttackCityS0TaskPointInfo(pointIndex);
	}

	public HeroDispatchMissionPointInfo GetHeroDispatchTaskPointInfoByIndex(int pointIndex)
	{
		return PointManager.GetHeroDispatchTaskPointInfoByIndex(pointIndex);
	}

	public GhostreconPointInfo GetGhostreconPointInfoByIndex(int pointIndex)
	{
		return PointManager.GetGhostreconPointInfoByIndex(pointIndex);
	}

	public ResPointInfo GetResourcePointInfoByIndex(int pointIndex)
	{
		return PointManager.GetResourcePointInfoByIndex(pointIndex);
	}

	public bool IsCollectRangePoint(int pointIndex)
	{
		return PointManager.IsCollectRangePoint(pointIndex);
	}

	public PointInfo GetPointInfo(int pointIndex)
	{
		return PointManager.GetPointInfo(pointIndex);
	}

	public PointInfo GetPointInfoWithServer(int pointIndex, int serverId)
	{
		return PointManager.GetPointInfo(pointIndex, serverId);
	}

	public WorldDesertInfo GetWorldDesertInfo(int pointIndex)
	{
		return PointManager.GetWorldDesertInfo(pointIndex);
	}

	public WorldTileInfo GetWorldTileInfo(int pointIndex)
	{
		return PointManager.GetWorldTileInfo(pointIndex);
	}

	public PointInfo GetYellowLand(int pointIndex)
	{
		return PointManager.GetYellowLand(pointIndex);
	}

	public int GetBuildTileByItemId(int itemId)
	{
		return PointManager.GetBuildTileByItemId(itemId);
	}

	public int GetAllianceCitySizeByItemId(int itemId)
	{
		return PointManager.GetAllianceCitySizeByItemId(itemId);
	}

	public int GetAllianceCitTypeByItemId(int itemId)
	{
		return PointManager.GetAllianceCityTypeByItemId(itemId);
	}

	public int GetTreasureSizeByItemId(int itemId)
	{
		return PointManager.GetTreasureSizeByItemId(itemId);
	}

	public int GetDragonBuildSizeByItemId(int itemId)
	{
		return PointManager.GetDragonBuildSizeByItemId(itemId);
	}

	public CollectPointInfo GetCollectRangePoint(int pointIndex)
	{
		return PointManager.GetCollectRangePoint(pointIndex);
	}

	public int GetCollectPoint(int resourceType)
	{
		return PointManager.GetCollectPoint(resourceType);
	}

	public List<int> GetAllCollectRangePoint(int resourceType)
	{
		return PointManager.GetAllCollectRangePoint(resourceType);
	}

	public List<int> GetAllCollectRangePointType(int resourceType, int mainIndex)
	{
		return PointManager.GetAllCollectRangePointType(resourceType, mainIndex);
	}

	public bool IsCollectPoint(int pointIndex)
	{
		return PointManager.IsCollectPoint(pointIndex);
	}

	public CollectPointInfo GetCollectInfoByIndex(int pointIndex)
	{
		return PointManager.GetCollectInfoByIndex(pointIndex);
	}

	public GarbagePointInfo GetGarbagePointInfoByIndex(int pointIndex)
	{
		return PointManager.GetGarbagePointInfoByIndex(pointIndex);
	}

	public virtual PointInfo GetPointInfoByUuid(long uuid)
	{
		return PointManager.GetPointInfoByUuid(uuid);
	}

	public WorldDesertInfo GetDesertInfoByUuid(long uuid)
	{
		return PointManager.GetDesertInfoByUuid(uuid);
	}

	public PointInfo GetMyPointInfo()
	{
		return PointManager.GetMyPointInfo();
	}

	public WorldPointObject GetObjectByPoint(int pointIndex)
	{
		return PointManager.GetObjectByPoint(pointIndex);
	}

	public bool IsOutCityByPoint(int point)
	{
		return PointManager.IsOutCityByPoint(point);
	}

	public BuildPointInfo GetBaseMainByScreen()
	{
		return PointManager.GetBaseMainByScreen();
	}

	public List<BuildPointInfo> GetMainByScreen()
	{
		return PointManager.GetMainByScreen();
	}

	public List<BuildPointInfo> GetAllMainBaseList()
	{
		return PointManager.GetAllMainBaseList();
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0)
	{
		return PointManager.GetAllMainBaseListByType(t0);
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1)
	{
		return PointManager.GetAllMainBaseListByType(t0, t1);
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1, PlayerType t2)
	{
		return PointManager.GetAllMainBaseListByType(t0, t1, t2);
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1, PlayerType t2, PlayerType t3)
	{
		return PointManager.GetAllMainBaseListByType(t0, t1, t2, t3);
	}

	public List<PointInfo> GetAllDragonPointList()
	{
		return PointManager.GetAllDragonPointList();
	}

	public List<int> GetAllDragonResourceList()
	{
		return PointManager.GetAllDragonResourceList();
	}

	public void ShowObject(int point)
	{
		PointManager.ShowObject(point);
	}

	public CityBuilding GetBuildingByPoint(int pointIndex)
	{
		if (PointManager != null)
		{
			return PointManager.GetBuildingByPoint(pointIndex);
		}
		return null;
	}

	public void SetLevelUpActive(int pointIndex, bool active)
	{
	}

	public float GetBuildingHeight(int pointIndex)
	{
		CityBuilding buildingByPoint = GetBuildingByPoint(pointIndex);
		if (buildingByPoint != null)
		{
			return buildingByPoint.GetHeight();
		}
		return 1f;
	}

	public void HandleViewPointsReply(ISFSObject message)
	{
		PointManager.HandleViewPointsReply(message);
		hasReceiveViewPointsReply = true;
	}

	public void HandleViewAssistanceInfoUpdateNotify(ISFSObject message)
	{
		PointManager.HandleViewAssistanceInfoUpdateNotify(message);
	}

	public void HandleViewUpdateNotify(ISFSObject message)
	{
		PointManager.HandleViewUpdateNotify(message);
	}

	public void HandlePushWorldObjStateChange(ISFSObject message)
	{
		PointManager.HandlePushWorldObjStateChange(message);
	}

	public void HandleViewTileUpdateNotify(ISFSObject message)
	{
		PointManager.HandleViewTileUpdateNotify(message);
	}

	public void HandleLandUpdate(ISFSObject message)
	{
	}

	public void SendViewRequest(Vector2Int tilePos, int viewLevel, int serverId)
	{
		PointManager.SendViewRequest(tilePos, PointManager.GetServerLod(viewLevel), serverId);
	}

	public void UpdateViewRequest(bool isForce = false)
	{
		PointManager.UpdateViewRequest(isForce);
	}

	public void SetFirstViewRequestFlag(bool isFirstTime)
	{
		PointManager.SetFirstViewRequestFlag(isFirstTime);
	}

	public virtual bool IsNeedPlayPlacedAnim(long uuid)
	{
		return PointManager.IsNeedPlayPlacedAnim(uuid);
	}

	public bool IsSelfRoad(int index)
	{
		return PointManager.IsSelfRoad(index);
	}

	public bool IsBuildFinish()
	{
		if (sceneInst != null)
		{
			return sceneInst.isDone;
		}
		return false;
	}

	public WorldPointObject GetObjectByUuid(long uuid)
	{
		return PointManager.GetObjectByUuid(uuid);
	}

	public CityBuilding GetBuildingByUuid(long uuid)
	{
		return PointManager.GetBuildingByUuid(uuid);
	}

	public WorldBuilding GetWorldBuildingByPoint(int pointIndex)
	{
		return PointManager.GetWorldBuildingByPoint(pointIndex);
	}

	public WorldBuilding GetWorldBuildingByUuid(long uuid)
	{
		return PointManager.GetWorldBuildingByUuid(uuid);
	}

	public bool HasPointInfo(int pointIndex)
	{
		return PointManager.HasPointInfo(pointIndex);
	}

	public void AddToDeleteList(int index)
	{
		PointManager.AddToDeleteList(index);
	}

	public BuildPointInfo GetBaseMainInfoByOwnerUid(string ownerUid)
	{
		return PointManager.GetBaseMainInfoByOwnerUid(ownerUid);
	}

	public void CheckNeedRefreshRoad()
	{
		PointManager.RefreshRoads();
	}

	public void HideObject(int point)
	{
		PointManager.HideObject(point);
	}

	public bool IsSelfPoint(int pointIndex)
	{
		return PointManager.IsSelfPoint(pointIndex);
	}

	public int GetPointType(int index)
	{
		return PointManager.GetPointInfo(index)?.PointType ?? 0;
	}

	public bool IsSelfFreeBoard(int index)
	{
		return PointManager.IsSelfFreeBoard(index);
	}

	public void RemoveObjectByPoint(int point)
	{
	}

	public void RemoveOneObjectByPointType(int index, int pointType)
	{
	}

	public void RefreshView()
	{
		FakeModelManager.UIDestroyBuilding();
		FakeModelManager.UIDestroyRoad();
		SendViewRequest(GameEntry.Data.Building.GetMainPos(), SceneManager.World.GetLodLevel(), GameEntry.Data.Player.GetSelfServerId());
	}

	public void OnMainBuildMove()
	{
	}

	public bool IsRoadPoint(int index, string uid, int dir)
	{
		return PointManager.IsRoadPoint(index, uid, dir);
	}

	public List<int> GetGarbagePoint()
	{
		return PointManager.GetGarbagePoint();
	}

	public Dictionary<int, int> GetSpecialPointDic()
	{
		return PointManager.GetSpecialPointDic();
	}

	public void HandleTriggerWorldGetBlock(ISFSObject message, int serverId, int worldId)
	{
		TriggerDataManager.HandleWorldGetBlock(message, serverId, worldId);
	}

	public void HandlePushWorldTriggerUpdate(ISFSObject message, int serverId, int worldId)
	{
		TriggerDataManager.HandlePushWorldTriggerUpdate(message, serverId, worldId);
	}

	public void HandlePushWorldTriggerDel(ISFSObject message, int serverId, int worldId)
	{
		TriggerDataManager.HandlePushWorldTriggerDel(message, serverId, worldId);
	}

	public void HandleUpdateLightData(ISFSObject message)
	{
		TriggerDataManager.HandlePushWorldLightUpdate(message);
	}

	public void CreateOrRefreshOneTrigger(WorldTriggerData data)
	{
		TriggerObjManager.CreateOrRefreshOneTrigger(data);
	}

	public void RemoveOneTrigger(long uuid)
	{
		TriggerObjManager.RemoveOneTrigger(uuid);
	}

	public WorldTriggerData GetWorldTriggerData(long uuid)
	{
		return TriggerDataManager.GetWorldTriggerData(uuid);
	}

	public WorldTriggerData GetTriggerDataByPointId(int pointId, int serverId)
	{
		return TriggerDataManager.GetTriggerDataByPointId(pointId, serverId);
	}

	public void HandlePushWolfStatusChange(ISFSObject message)
	{
		WerewolfDataManager.HandlePushWolfStatusChange(message);
	}

	public void HandleWerewolfWorldGetBlock(WorldView2WolfPointInfoMsg message)
	{
		WerewolfDataManager.HandleWorldGetBlock(message);
	}

	public int GetWerewolfMaxHp()
	{
		return WerewolfDataManager.MaxHp;
	}

	public WerewolfAnimState GetWerewolfAnimState(int pointId)
	{
		return WerewolfDataManager.GetAnimState(pointId);
	}

	public virtual PlayerType IsMyEnemy(int serverId, string allianceId)
	{
		return LuaCacheDataManager.IsMyEnemy(serverId, allianceId);
	}

	public void HandlePushMultiKillUpdate(ISFSObject message)
	{
		MultiKillDataManager.HandlePushMultiKillUpdate(message);
	}

	public void CreateMultiKillFakeData()
	{
		MultiKillDataManager.CreateFakeData();
	}

	public void MultiKillDataRecycle(MultiKillBubbleData data)
	{
		MultiKillDataManager.Recycle(data);
	}

	public void MultiKillPointAddTask(MultiKillBubbleData data)
	{
		MultiKillPointManager.MultiKillPointAddTask(data);
	}

	public void CleanAllianceCacheData()
	{
		LuaCacheDataManager.CleanAllianceCacheData();
	}

	public WorldTroop CreateGroupTroop(WorldMarch march)
	{
		return TroopManager.CreateGroupTroop(march);
	}

	public WorldMarch GetMarch(long uuid)
	{
		return MarchDataManager.GetMarch(uuid);
	}

	public WorldMarch GetMonster(long targetPoint)
	{
		return MarchDataManager.GetMonster(targetPoint);
	}

	public Dictionary<long, WorldMarch> GetAllSampleFakeData()
	{
		return MarchDataManager.GetAllSampleFakeData();
	}

	public void RemoveFakeSampleMarchData(long index)
	{
		MarchDataManager.RemoveFakeSampleMarchData(index);
	}

	public void UpdateFakeSampleMarchDataWhenBack(long index, long startTime, long endTime)
	{
		MarchDataManager.UpdateFakeSampleMarchDataWhenBack(index, startTime, endTime);
	}

	public void UpdateFakeSampleMarchDataWhenStartPick(long index, long endTime)
	{
		MarchDataManager.UpdateFakeSampleMarchDataWhenStartPick(index, endTime);
	}

	public void AddFakeSampleMarchData(long startIndex, long endIndex, long startTime, long endTime, int marchTargetType)
	{
		MarchDataManager.AddFakeSampleMarchData(startIndex, endIndex, startTime, endTime, marchTargetType);
	}

	public void AddFakeSampleMarchDataWithServerId(long startIndex, long endIndex, long startTime, long endTime, int marchTargetType, int srcServer = -1, int targetServer = -1)
	{
		MarchDataManager.AddFakeSampleMarchData(startIndex, endIndex, startTime, endTime, marchTargetType, srcServer, targetServer);
	}

	public void AddFakeAttackMonsterMarchData(long startIndex, long endIndex, float marchTimeSec, string ownerUid)
	{
		MarchDataManager.AddFakeAttackMonsterMarchData(startIndex, endIndex, marchTimeSec, ownerUid);
	}

	public void DelFakeAttackMonsterMarchDataRandom(int count)
	{
		MarchDataManager.DelFakeAttackMonsterMarchDataRandom(count);
	}

	public void UpdateFakeAttackMonsterMarch(long uuid)
	{
		MarchDataManager.UpdateFakeAttackMonsterMarch(uuid);
	}

	public void StartMarch(int targetType, int targetPoint, long targetUuid, int timeIndex, long marchUuid = 0L, long formationUuid = 0L, int backHome = 1, byte[] sfsObjBinary = null, int startPos = 0, int targetServerId = -1)
	{
		MarchDataManager.StartMarch(targetType, targetPoint, targetUuid, timeIndex, marchUuid, formationUuid, backHome, sfsObjBinary, startPos, targetServerId);
	}

	public WorldMarch GetOwnerFormationMarch(string ownerUid, long formationUuid, string allianceUid = "")
	{
		return MarchDataManager.GetOwnerFormationMarch(ownerUid, formationUuid, allianceUid);
	}

	public WorldMarch GetAllianceMarchesInTeam(string allianceUid, long teamUuid)
	{
		return MarchDataManager.GetAllianceMarchesInTeam(allianceUid, teamUuid);
	}

	public virtual List<WorldMarch> GetOwnerMarches(string ownerUid, string allianceUid = "")
	{
		return MarchDataManager.GetOwnerMarches(ownerUid, allianceUid);
	}

	public void HandlePushWorldMarchAdd(ISFSObject message)
	{
		MarchDataManager.HandlePushWorldMarchAdd(message);
	}

	public void HandlePushWorldMarchDel(ISFSObject message)
	{
		MarchDataManager.HandlePushWorldMarchDel(message);
	}

	public void HandleWorldMarchGet(ISFSObject message)
	{
		MarchDataManager.HandleWorldMarchGet(message);
	}

	public void HandleFormationMarch(ISFSObject message)
	{
		MarchDataManager.HandleFormationMarch(message);
	}

	public void HandleFormationMarchChange(ISFSObject message)
	{
		MarchDataManager.HandleFormationMarchChange(message);
	}

	public bool ExistMarch(long uuid)
	{
		return MarchDataManager.ExistMarch(uuid);
	}

	public bool IsInRallyMarch(long uuid)
	{
		return MarchDataManager.IsInRallyMarch(uuid);
	}

	public bool IsInCollectMarch(long uuid)
	{
		return MarchDataManager.IsInCollectMarch(uuid);
	}

	public bool IsInAssistanceMarch(long uuid)
	{
		return MarchDataManager.IsInAssistanceMarch(uuid);
	}

	public bool IsSelfInCurrentMarchTeam(long rallyMarchUuid)
	{
		return MarchDataManager.IsSelfInCurrentMarchTeam(rallyMarchUuid);
	}

	public int GetMyAssistanceCount(int pointIndex)
	{
		return MarchDataManager.GetMyAssistanceCountByPointIndex(pointIndex);
	}

	public int GetMyAssistanceFirstHero(int pointIndex)
	{
		return MarchDataManager.GetMyAssistanceFirstHero(pointIndex);
	}

	public void MarkPointIsDirty(long dirtyPointUuid)
	{
		PointManager.MarkPointIsDirty(dirtyPointUuid);
	}

	public bool IsTargetForMine(WorldMarch marchData)
	{
		return MarchDataManager.IsTargetForMine(marchData);
	}

	public bool IsTargetForAlly(WorldMarch marchData)
	{
		return MarchDataManager.IsTargetForAlly(marchData);
	}

	public Dictionary<long, WorldMarch> GetMarchesBossInfo()
	{
		return MarchDataManager.GetMarchesBossInfo();
	}

	public void GetMonsterListInArea(Vector2Int center, int size, Dictionary<int, int> monsterIds, Dictionary<long, Vector2Int> result)
	{
		MarchDataManager.GetMonsterListInArea(center, size, monsterIds, result);
	}

	public WorldMarch GetMarchesByStartIndex(int posIndex)
	{
		return MarchDataManager.GetMarchesByStartIndex(posIndex);
	}

	public void SetFocusPoint(int point)
	{
		PointManager?.SetFocusPoint(point);
	}

	public Dictionary<long, WorldMarch> GetAllMarches()
	{
		return MarchDataManager.GetAllMarchesByCS();
	}

	public void DestroyBerserkBossMarchData(long marchUuid)
	{
		MarchDataManager.DestroyBerserkBossMarchData(marchUuid);
	}

	public string SaveCreateMarchRecordTime()
	{
		return MarchDataManager.SaveCreateMarchRecordTime();
	}

	public void RemoveCreateMarchRecordTime(WorldMarch worldMarch)
	{
		MarchDataManager.RemoveCreateMarchRecordTime(worldMarch);
	}

	public bool CanUseInput()
	{
		return InputManager.CanUseInput();
	}

	public void SetUseInput(bool canUse)
	{
		InputManager.SetUseInput(canUse);
	}

	public void SetSelectedPickable(ITouchPickable pickable)
	{
		InputManager.SetSelectedPickable(pickable);
	}

	public void DragSelectedPickable(Vector3 position)
	{
		InputManager.DragSelectedPickable(position);
	}

	public void ShowLoad(Vector3 pos)
	{
		InputManager.ShowLoad(pos);
	}

	public void SetDragFormationData(long uuid, int pointId)
	{
		InputManager.SetDragFormationData(uuid, pointId);
	}

	public int GetClickWorldBulidingPos()
	{
		return InputManager.GetClickWorldBulidingPos();
	}

	public void HideTouchEffect()
	{
		InputManager.HideTouchEffect();
	}

	public long GetRaycastHitMarch(Vector3 screenPos)
	{
		return InputManager.GetRaycastHitMarch(screenPos);
	}

	public bool IsTileWalkable(Vector3 worldPos)
	{
		return StaticManager.IsTileWalkable(worldPos);
	}

	public void AddOccupyPoints(Vector2Int p, Vector2Int size, int serverId = 0)
	{
		StaticManager.AddOccupyPoints(p, serverId, size);
	}

	public void RemoveOccupyPoints(Vector2Int p, Vector2Int size, int serverId = 0)
	{
		StaticManager.RemoveOccupyPoints(p, serverId, size);
	}

	public void GreenAreaChange(WorldAreaGreenInfo.GreenType type, HashSet<int> changePoints = null)
	{
		StaticManager.GreenAreaChange(type, changePoints);
	}

	public void UpdateGreenArea(int xMin, int yMin, int xMax, int yMax)
	{
		StaticManager.UpdateGreenArea(xMin, yMin, xMax, yMax);
	}

	public bool IsGreen(int pointIndex)
	{
		return PointManager.worldGreen.IsGreen(pointIndex);
	}

	public bool CanGreen(int pointIndex, bool showTip = false)
	{
		return PointManager.worldGreen.CanGreen(pointIndex, showTip);
	}

	public void SetStaticVisibleChunk(int range)
	{
	}

	public float GetModelHeight(long marchUuid)
	{
		return TroopManager.GetModelHeight(marchUuid);
	}

	public WorldTroop GetTroop(long marchUuid)
	{
		return TroopManager.GetTroop(marchUuid);
	}

	public EnumDestinationSignalType GetDestinationType(long marchUuid, long targetMarchUuid, int endPos, MarchTargetType targetType, bool isFormation, ref Vector3 realPos, ref int tileSize)
	{
		return TroopManager.GetDestinationType(marchUuid, targetMarchUuid, endPos, targetType, isFormation, ref realPos, ref tileSize);
	}

	public MarchTargetType GetTargetType(long targetMarchUuid, int pointId)
	{
		return TroopManager.GetTargetType(targetMarchUuid, pointId);
	}

	public void AddHSRTroop(WorldMarch march, Transform transform)
	{
		HSRTroopMgr.AddTroop(march, transform);
	}

	public void RefreshHSRTroop(WorldMarch march)
	{
		HSRTroopMgr.RefreshTroop(march);
	}

	public void RemoveHSRTroop(long marchUuid)
	{
		HSRTroopMgr.RemoveTroop(marchUuid);
	}

	public void CreateBattleVFX(string prefabPath, float life, Action<GameObject> onComplete)
	{
		TroopManager.CreateBattleVFX(prefabPath, life, onComplete);
	}

	public int CreateVFX(string prefabPath, Vector3 pos, float duration, float delay = 0f, Action<GameObject> onComplete = null)
	{
		return VFXManager.CreateVFX(prefabPath, pos, duration, delay, onComplete);
	}

	public void RemoveVFX(int id)
	{
		VFXManager.RemoveVFX(id);
	}

	public int GetCurPosAndRotationTroopNum(long marchUuid, int pointId, Quaternion rot)
	{
		return TroopManager.GetCurPosAndRotationTroopNum(marchUuid, pointId, rot);
	}

	public void RemovePosAndRotationDataByMarchUuid(long marchUuid)
	{
		TroopManager.RemovePosAndRotationDataByMarchUuid(marchUuid);
	}

	public void OnTroopDragUpdate(long marchUuid, Vector3 dragPosCurrent, long targetMarchUuid, int startPointId = 0, bool isFormation = false)
	{
		TroopManager.OnDragUpdate(marchUuid, dragPosCurrent, targetMarchUuid, startPointId, isFormation);
	}

	public void OnTroopDragStop(long marchUuid, long targetMarchUuid, bool isFormation = false)
	{
		TroopManager.OnDragStop(marchUuid, targetMarchUuid, isFormation);
	}

	public int GetPointSize(int index)
	{
		return 1;
	}

	public bool IsTroopCreate(long marchUuid)
	{
		return TroopManager.IsTroopCreate(marchUuid);
	}

	[Obsolete]
	public bool IsTroopInCreateQueue(long marchUuid)
	{
		return TroopManager.IsTroopInCreateQueue(marchUuid);
	}

	public void CreateTroop(WorldMarch march)
	{
		TroopManager.CreateTroop(march);
	}

	public void UpdateTroop(WorldMarch march)
	{
		TroopManager.UpdateTroop(march);
	}

	public void TroopRefreshPosition(WorldMarch march)
	{
		TroopManager.RefreshPosition(march);
	}

	public void RefreshNeedCreateTroop(WorldMarch march)
	{
		TroopManager.RefreshNeedCreateTroop(march);
	}

	public float DestroyTroop(long marchUuid, bool isBattleFailed = false)
	{
		return TroopManager.DestroyTroop(marchUuid, isBattleFailed);
	}

	public void OnMonsterIceBroken(long uuid)
	{
		TroopManager.OnMonsterIceBroken(uuid);
	}

	public void CreateTroopLine(WorldMarch march)
	{
		TroopLineManager.CreateTroopLine(march);
	}

	public void DestroyTroopLine(long marchUuid)
	{
		TroopLineManager.DestroyTroopLine(marchUuid);
	}

	public bool IsTroopLineCreate(long marchUuid)
	{
		return TroopLineManager.IsTroopLineCreate(marchUuid);
	}

	public bool IsMarchOutOfData(WorldMarch march)
	{
		return TroopLineManager.IsTroopLineMarchOutOfData(march);
	}

	public void UpdateTroopLineNew(WorldMarch march, Vector3 currPos)
	{
		TroopLineManager.UpdateTroopLineNew(march, currPos);
	}

	[Obsolete]
	public void UpdateTroopLine(WorldMarch march, WorldTroopPathSegment[] path, int currPath, Vector3 currPos, int realTargetPos = 0, bool needRefresh = false, bool clear = false)
	{
		TroopLineManager.UpdateTroopLine(march, path, currPath, currPos, realTargetPos, needRefresh, clear);
	}

	public void HideTroopDestination(long uuid)
	{
		TroopLineManager.HideDestination(uuid);
	}

	public int GetCurrentDisplayLevel()
	{
		return TroopLineManager.CurrentDisplayLevel;
	}

	public bool IsInSimpleMode()
	{
		return TroopLineManager.CurrentDisplayLevel < 0;
	}

	public bool IsTruckRoad(Vector2Int point, FindPathType findPathType)
	{
		return false;
	}

	public void RemoveCullingBounds(WorldCulling.ICullingObject cullingObject)
	{
		Culling.RemoveCullingBounds(cullingObject);
	}

	public void AddCullingBounds(WorldCulling.ICullingObject cullingObject)
	{
		Culling.AddCullingBounds(cullingObject);
	}

	public void BattleFinish(ISFSObject message)
	{
		MarchDataManager.BattleFinish(message);
	}

	public void UpdateBattleMessage(ISFSObject message)
	{
		MarchDataManager.UpdateBattleMessage(message);
	}

	public void StartPrintRoad(List<int> roads, bool isOther)
	{
		FakeModelManager.StartPrintRoad(roads, isOther);
	}

	public void StartPrintRoadByPathStr(string paths, bool isOther, float buildPerRoadTime = 0f)
	{
		if (!string.IsNullOrEmpty(paths))
		{
			List<int> list = new List<int>();
			string[] array = paths.Split(new char[1] { ';' });
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array[i].ToInt());
			}
			FakeModelManager.StartPrintRoad(list, isOther);
		}
	}

	public void FinishPrintRoad(int pointId)
	{
		FakeModelManager.FinishPrintRoad(pointId);
	}

	public List<int> UIDestroyRoad()
	{
		return FakeModelManager.UIDestroyRoad();
	}

	public void UICreateBuilding(int buildId, long buildUuid, int point, int buildTopType, LuaTable noBuildListStr = null)
	{
		FakeModelManager.UICreateBuilding(buildId, buildUuid, point, buildTopType, noBuildListStr);
	}

	public void UICreateAllianceBuilding(int buildId, long buildUuid, int point, int buildTopType, LuaTable noBuildListStr = null)
	{
		FakeModelManager.UICreateAllianceBuilding(buildId, buildUuid, point, buildTopType, noBuildListStr);
	}

	public void UIChangeBuilding(int index)
	{
		FakeModelManager.UIChangeBuilding(index);
	}

	public void UICreateBuildingModelPath(int buildId, long buildUuid, int point, int buildTopType, string modelPath, LuaTable noBuildListStr = null, LuaTable param = null)
	{
		FakeModelManager.UICreateBuildingModelPath(buildId, buildUuid, point, buildTopType, modelPath, noBuildListStr, param);
	}

	public void UIDestroyBuilding()
	{
		FakeModelManager.UIDestroyBuilding();
	}

	public void UIChangeAllianceBuilding(int index)
	{
		FakeModelManager.UIChangeAllianceBuilding(index);
	}

	public void UIDestroyAllianceBuilding()
	{
		FakeModelManager.UIDestroyAllianceBuilding();
	}

	public void UIChangeRoad()
	{
		FakeModelManager.UIChangeRoad();
	}

	public void UICreateWorldMoveMarch(string modelPath, long uuid, Vector3 pos)
	{
		FakeModelManager.UICreateWorldMoveMarch(modelPath, uuid, pos);
	}

	public void UICreateWorldTrigger(string modelPath, int cfgId, int pointId, int skillId, int serverId)
	{
		FakeModelManager.UICreateWorldTrigger(modelPath, cfgId, pointId, skillId, serverId);
	}

	public void UICreateWorldFlowerTrain(string modelPath, int pointId, int goodsId)
	{
		FakeModelManager.UICreateWorldFlowerTrain(modelPath, pointId, goodsId);
	}

	public void UIDestroyRreCreateMarch()
	{
		FakeModelManager.UIDestroyRreCreateMarch();
	}

	public void UIDestroyPreCreateTrigger()
	{
		FakeModelManager.UIDestroyPreCreateTrigger();
	}

	public void UIDestroyPreCreateFlowerTrain()
	{
		FakeModelManager.UIDestroyPreCreateFlowerTrain();
	}

	public void UICreateFakeEpidemicSkill()
	{
		FakeModelManager.UICreateFakeEpidemicSkill();
	}

	public void UICreateWorldMovingModel(string modelPath, int flag, int pointId, int size)
	{
		FakeModelManager.UICreateWorldMovingModel(modelPath, flag, pointId, size);
	}

	public void UIDestroyRreCreateModel()
	{
		FakeModelManager.UIDestroyRreCreateModel();
	}

	public void UICreateBoard(int index, bool isAfter = true)
	{
		FakeModelManager.UICreateBoard(index, isAfter);
	}

	public void UIHideBoard(int deleteCount, bool isAfter = true)
	{
		FakeModelManager.UIHideBoard(deleteCount, isAfter);
	}

	public void UIDestroyRreCreateBuild()
	{
		FakeModelManager.UIDestroyRreCreateBuild();
	}

	public void UIDestroyRreCreateAllianceBuild()
	{
		FakeModelManager.UIDestroyRreCreateAllianceBuild();
	}

	public ModelManager.ModelObject AddObjectByPointId(int index, int type)
	{
		return null;
	}

	public ModelManager.ModelObject GetObjectByPointId(int index)
	{
		return null;
	}

	public List<Vector2Int> GetNearestPathForBuildingConnect(long uuid)
	{
		LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(uuid);
		if (buildingDataByUuid == null)
		{
			return null;
		}
		return GetNearestConnectPath(buildingDataByUuid);
	}

	private List<Vector2Int> GetNearestConnectPath(LuaBuildData buildingData)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		List<Vector2Int> entries;
		List<Vector2Int> truckPathMainOutPosList = PathUtils.GetTruckPathMainOutPosList(out entries);
		List<Vector2Int> entries2;
		List<Vector2Int> buildingNeighbors = PathUtils.GetBuildingNeighbors(buildingData, out entries2);
		for (int num = truckPathMainOutPosList.Count - 1; num >= 0; num--)
		{
			if (!PathUtils.IsRoad(truckPathMainOutPosList[num]))
			{
				truckPathMainOutPosList.RemoveAt(num);
				entries.RemoveAt(num);
			}
		}
		for (int num2 = buildingNeighbors.Count - 1; num2 >= 0; num2--)
		{
			if (!PathUtils.IsRoad(buildingNeighbors[num2]))
			{
				buildingNeighbors.RemoveAt(num2);
				entries2.RemoveAt(num2);
			}
		}
		for (int i = 0; i < truckPathMainOutPosList.Count; i++)
		{
			_ = truckPathMainOutPosList[i];
			Vector2Int item = entries[i];
			for (int j = 0; j < buildingNeighbors.Count; j++)
			{
				_ = buildingNeighbors[j];
				Vector2Int item2 = entries2[j];
				List<Vector2Int> list2 = new List<Vector2Int>();
				if (list2.Count != 0)
				{
					list2.Insert(0, item);
					list2.Add(item2);
					if (list.Count == 0 || list2.Count < list.Count)
					{
						list = list2;
					}
				}
			}
		}
		return list;
	}

	public void CreateRoadRobot(List<int> roads, bool isOther, long printId)
	{
		WorldRobotManager.CreateRoadRobot(roads, isOther, printId);
	}

	public void AddToNeedRemoveList(long bUuid)
	{
		WorldRobotManager.AddToNeedRemoveList(bUuid);
	}

	public WorldRoadRobot GetRoadRobot(int roodPoint)
	{
		return WorldRobotManager.GetRoadRobot(roodPoint);
	}

	public void ChangeBuildRobotState(long bUuid, WorldBuildRobot.State state)
	{
		WorldRobotManager.ChangeBuildRobotState(bUuid, state);
	}

	public void ChangeBuildRobotFinishTime(long bUuid, float finishTime)
	{
		WorldRobotManager.ChangeBuildRobotFinishTime(bUuid, finishTime);
	}

	public void CreateBuildRobot(long bUuid, Vector3 targetPos, float height, float duration, int tileSizeX, int tileSizeY, bool isTransit = false)
	{
		WorldRobotManager.CreateBuildRobot(bUuid, targetPos, height, duration, tileSizeX, tileSizeY, isTransit);
	}

	public void CreateOtherBuildRobot(long bUuid, Vector3 targetPos, float height, float duration, int tileSizeX, int tileSizeY)
	{
		WorldRobotManager.CreateOtherBuildRobot(bUuid, targetPos, height, duration, tileSizeX, tileSizeY);
	}

	public WorldBuildRobot GetBuildRobot(long bUuid)
	{
		return WorldRobotManager.GetBuildRobot(bUuid);
	}

	public void CreateAnimalObject(long uuid)
	{
		WorldCollectAnimalManager.CreateAnimalObject(uuid);
	}

	public void DestroyAnimalObject(long uuid)
	{
		WorldCollectAnimalManager.DestroyAnimalObject(uuid);
	}

	public void CreateArmyAnimalObject(long MarchUuid, int resPointId)
	{
		WorldArmyCollectAnimalManager.CreateAnimalObject(MarchUuid, resPointId);
	}

	public void DestroyArmyAnimalObject(long MarchUuid)
	{
		WorldArmyCollectAnimalManager.DestroyAnimalObject(MarchUuid);
	}

	public void InitFogOfWar(BitArray fogData)
	{
	}

	public void ReInitFogOfWar()
	{
	}

	public void UnlockFogOfWar(int fogIndex)
	{
	}

	public void UnlockFogOfWar2x2(int unlockIndex)
	{
	}

	public void SetFogVisible(bool visible)
	{
	}

	public void RegisterFogCompleteAction(Action callback)
	{
	}

	public void ReInitObject()
	{
	}

	public void ClearReInitObject()
	{
	}

	public CityTroop GetCityTroop()
	{
		return null;
	}

	public long GetFormationUuid()
	{
		return 0L;
	}

	public void LoadCityTroop(int createPos, int targetPos = 0)
	{
	}

	public void DestroyCityTroop()
	{
	}

	public Dictionary<string, LodConfig> GetLodConfigs(int lodType)
	{
		return LodManager.GetLodConfigs(lodType);
	}

	public void AddLodAdjuster(AutoAdjustLod adjuster)
	{
		LodManager.AddLodAdjuster(adjuster);
	}

	public void RemoveLodAdjuster(AutoAdjustLod adjuster)
	{
		LodManager.RemoveLodAdjuster(adjuster);
	}

	public int GetZoneIdByPosId(int pointId)
	{
		return WorldMapZoneManager.GetZoneIdByPosId(pointId);
	}

	public int GetZoneIdByWorldPos(Vector3 worldPos)
	{
		return WorldMapZoneManager.GetZoneIdByWorldPos(worldPos);
	}

	public bool IsPointInAllianceCity(int pointId, int serverId)
	{
		return WorldMapZoneManager.IsPointInAllianceCity(pointId, serverId);
	}

	public bool IsPointInBlackArea(int pointId)
	{
		return WorldMapZoneManager.IsInBlackArea(pointId);
	}

	public void ShowBlackArea(int point, int tileWidth, int tileHeight)
	{
		WorldMapZoneManager.ShowBlackArea(point, tileWidth, tileHeight);
	}

	public void HideBlackArea(int point, int tileWidth, int tileHeight)
	{
		WorldMapZoneManager.HideBlackArea(point, tileWidth, tileHeight);
	}

	public WorldZoneData GetZoneData(int zoneId)
	{
		return WorldMapZoneManager.GetZoneData(zoneId);
	}

	public void SetLandPointInfos(List<LandPointInfo> landPointInfos)
	{
	}

	public bool IsInSelfLandBlock(int pointIndex)
	{
		return false;
	}

	public CitySpaceMan CreateCitySpaceMan()
	{
		return null;
	}

	public void SetVisibleByPointType(int pointType, bool isVisible)
	{
	}

	public void SetCameraMaxHeight(int height)
	{
		if (_saveCameraHeightMax <= 0f)
		{
			_saveCameraHeightMax = Camera.ZoomMax;
		}
		Camera.ZoomMax = height;
	}

	public void SetCameraMinHeight(int height)
	{
		if (_saveCameraHeightMin <= 0f)
		{
			_saveCameraHeightMin = Camera.ZoomMin;
		}
		Camera.ZoomMin = height;
	}

	public void UpdateTroopLineColor(string colorArgs)
	{
		TroopLineManager?.UpdateTroopLineColor(colorArgs);
	}

	public void SetCameraRotRange(bool overrideVal, Vector2 range)
	{
		Camera.SetRotRangeOverride(overrideVal, range);
	}

	public void SetCameraLodRange(bool overrideVal, int minLod, int maxLod)
	{
		Camera.SetLodRange(overrideVal, minLod, maxLod);
	}

	public void ResetCameraMaxHeight()
	{
		if (_saveCameraHeightMax > 0f)
		{
			Camera.ZoomMax = _saveCameraHeightMax;
			_saveCameraHeightMax = 0f;
		}
	}

	public void ResetCameraMinHeight()
	{
		if (_saveCameraHeightMin > 0f)
		{
			Camera.ZoomMin = _saveCameraHeightMin;
			_saveCameraHeightMin = 0f;
		}
	}

	public void DrawBuildGrid(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count)
	{
	}

	public int EnterTimeline(Camera camInTimeline, float transitionTime)
	{
		DisableClampToEdge = true;
		Camera.BeginSyncWithTimeline(camInTimeline, transitionTime);
		return ++_AUTO_INC_SYNC_TIMELINE_HANDLE;
	}

	public bool ExitTimeline(int handle)
	{
		if (_AUTO_INC_SYNC_TIMELINE_HANDLE == handle)
		{
			DisableClampToEdge = false;
			Camera.EndSyncWithTimeline();
			return true;
		}
		return false;
	}

	public void LockCamera(Vector3 pos, float duration)
	{
		Camera.LockCamera(pos, duration);
	}

	public void FreeCamera()
	{
		Camera.FreeCamera();
	}

	public bool IsOutOfLWAoi(int pointIndex, int serverId = 0)
	{
		return PointManager.IsOutOfLWAoi(pointIndex, serverId);
	}

	public void ChangeOutEdgeScale(float scale)
	{
		PointManager.ChangeOutEdgeScale(scale);
	}

	public Color GetLabelSkinColor(int skinId, int colorType)
	{
		return PointManager.GetLabelSkinColor(skinId, colorType);
	}

	public float GetLabelSkinOffset(int skinId)
	{
		return PointManager.GetLabelSkinOffset(skinId);
	}

	public float GetLabelSkinSizeAdd(int skinId)
	{
		return PointManager.GetLabelSkinSizeAdd(skinId);
	}

	public void TestNetworkDisconnect()
	{
		GameEntry.Network.SyncPingPong(0);
		GameEntry.Network.Disconnect();
		if (GameEntry.NetworkCross != null)
		{
			GameEntry.NetworkCross.Disconnect();
		}
		ApplicationLaunch.Instance.DisconnectRetry();
	}

	public void UpdateBattleSoundData()
	{
		MarchDataManager?.UpdateBattleSoundData();
	}

	public string Description(string name, object[] args = null)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		name = name.ToLower();
		switch (name)
		{
		case "worldpoint":
		{
			int result;
			if (args == null)
			{
				stringBuilder.AppendLine(PointManager?.Description());
			}
			else if (int.TryParse(args[0].ToString(), out result))
			{
				PointInfo pointInfo = PointManager?.GetPointInfo(result);
				if (pointInfo == null)
				{
					stringBuilder.AppendLine($"Get point info by index {result} failed.");
				}
				else
				{
					pointInfo.Description(stringBuilder);
				}
			}
			else
			{
				stringBuilder.AppendLine("???");
			}
			break;
		}
		case "mapgridrenderer":
			stringBuilder.AppendLine(MapGridRenderer?.Description());
			break;
		case "gpuiconrenderer":
			stringBuilder.AppendLine(IconRendererFacade?.Description());
			break;
		case "scene":
			stringBuilder.AppendLine($"建筑Icon实例化开关状态:{EnableWorldIconGPUInstancing}");
			break;
		default:
			stringBuilder.AppendLine("Unsupported description name => " + name);
			break;
		}
		return stringBuilder.ToString();
	}

	public void RegisterLodWatcher(IWorldLodWatcher watcher)
	{
		PointManager?.RegisterLodWatcher(watcher);
	}

	public void UnregisterLodWatcher(IWorldLodWatcher watcher)
	{
		PointManager?.UnregisterLodWatcher(watcher);
	}

	public InstanceRequest InstantiateAsyncDynamicObj(string prefabPath, int priority = 1)
	{
		if (ENABLE_DYNAMIC_OBJ_POOL)
		{
			return GameEntry.Resource.InstantiateAsync(prefabPath, CreateDynamicObjPools, priority);
		}
		return GameEntry.Resource.InstantiateAsync(prefabPath, ObjectPoolTag.Normal, priority);
	}

	private BasePool CreateDynamicObjPools(string prefabPath)
	{
		_poolManager = _poolManager ?? new AutoDisposePoolManager();
		if (!_poolManager.TryGetPool(prefabPath, out var pool))
		{
			pool = new WorldDynamicObjPool(_poolManager, prefabPath, GameEntry.Resource);
			_poolManager.RegisterPool(pool);
		}
		return pool;
	}

	private void ClearDynamicObjPools()
	{
		if (_poolManager != null)
		{
			_poolManager.Clear();
			_poolManager = null;
		}
	}

	public BloodyNightState GetBloodyNightState()
	{
		if (GetCurSeasonType() != SeasonType.Darkness)
		{
			return BloodyNightState.None;
		}
		if (mWorldFogManager.mIsDawn)
		{
			return BloodyNightState.None;
		}
		if (mWorldFogManager.mIsBloodyNight)
		{
			return BloodyNightState.Bloody;
		}
		return BloodyNightState.Silent;
	}

	public SeasonType GetCurSeasonType()
	{
		return StaticManager.CurSeasonType;
	}

	public void SendGetALPointsRequest(int serverId)
	{
		GetALPointsMessage.Instance.Send(serverId);
	}

	private void OnWorldColorDirty(object userData)
	{
		string text = userData?.ToString() ?? string.Empty;
		if (!text.IsNullOrEmpty())
		{
			LuaCacheDataManager.OnWorldColorDirty(text);
			PointManager.OnWorldColorDirty(text);
		}
	}

	public void OnHandleALPoints(ISFSObject message)
	{
		if (message.ContainsKey("errorCode"))
		{
			if (CommonUtils.IsDebug())
			{
				UIUtils.ShowTips("Error:" + message.TryGetString("errorCode"), 3f);
			}
		}
		else
		{
			PointManager.HandleALPoints(message);
		}
	}

	public void GetALMemberPoints(out long leaderPosition, out List<long> memberPositions)
	{
		if (PointManager == null)
		{
			leaderPosition = 0L;
			memberPositions = null;
		}
		else
		{
			PointManager.GetALMemberPoints(out leaderPosition, out memberPositions);
		}
	}

	public void ClearALMemberPoints()
	{
		PointManager?.ClearALMemberPoints();
	}
}
