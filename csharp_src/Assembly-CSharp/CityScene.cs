using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BitBenderGames;
using GameFramework;
using Main.Scripts.Scene;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VEngine;
using XLua;

public class CityScene : MonoBehaviour, SceneInterface
{
	private CitySceneType citySceneType = CitySceneType.Guide;

	private const int kTileCountX = 100;

	private const int kTileCountY = 100;

	private const int kBlockSize = 100;

	private static readonly Vector2Int kBlockCount = new Vector2Int(1, 1);

	private static readonly int Prop_Control = Shader.PropertyToID("_Control");

	private static readonly int Prop_Splat0 = Shader.PropertyToID("_Splat0");

	private static readonly int Prop_Splat1 = Shader.PropertyToID("_Splat1");

	private static readonly int Prop_Control_ST = Shader.PropertyToID("_Control_ST");

	private static readonly int Prop_Splat0_ST = Shader.PropertyToID("_Splat0_ST");

	private static readonly int Prop_Splat1_ST = Shader.PropertyToID("_Splat1_ST");

	private static readonly int Prop_TerrainBounds = Shader.PropertyToID("_TerrainBounds");

	public CityNightColorRenderer mCityNightRenderer;

	private CityWeatherRenderer mCityWeatherRenderer;

	private List<CityManagerBase> subModules = new List<CityManagerBase>();

	private Transform dynamicObjNode;

	private float prevLodDist = -1f;

	private Transform buildBubbleNode;

	private InstanceRequest sceneInst;

	private InstanceRequest sceneInst_dig;

	private bool digCreate;

	private bool isBuildComplete;

	private new GameObject gameObject;

	private InstanceRequest terrainInst;

	private Asset terrainSetting;

	private InstanceRequest terrainInstNew;

	private Asset terrainSettingNew;

	private Asset postProcessVolumeAsset;

	private FOWSystem fowSystem;

	private Action fogCallBack;

	private Transform cityGrid;

	private static int[] lodArray;

	private HashSet<MonoBehaviour> activePhysicObj = new HashSet<MonoBehaviour>();

	private CityCityTruckManager CityTruckManager;

	private CityCollectAnimalManager CityCollectAnimalManager;

	private CityLodManager LodManager;

	private CityZoneGroundManager CityZoneGroundManager;

	private float _saveCameraHeightMin;

	private float _saveCameraHeightMax;

	private Dictionary<string, Asset> terrainMatAssets = new Dictionary<string, Asset>();

	private static Dictionary<Type, string> PostprocessSettingKeys = new Dictionary<Type, string>
	{
		[typeof(Bloom)] = "QualitySetting.PostProcess.Bloom",
		[typeof(Tonemapping)] = "QualitySetting.PostProcess.Tonemapping",
		[typeof(LiftGammaGain)] = "QualitySetting.PostProcess.LiftGammaGain",
		[typeof(DepthOfField)] = "QualitySetting.PostProcess.LiftGammaGain"
	};

	private float _truckShowTime;

	private InstanceRequest gfxConsoleRequest;

	private BitArray _fogData;

	private float kFogTileSize = 8f;

	private float kFogTile2x2Size = 4f;

	private static Vector2Int kFogTileCount = new Vector2Int(25, 25);

	private static Vector2Int kFogTile2x2Count = new Vector2Int(50, 50);

	private static int kFogTileTotalCount = kFogTileCount.x * kFogTileCount.y;

	private bool cheatOpen;

	private bool consoleVisible;

	private bool borderColliderVisible;

	private bool gridVisible;

	private int _AUTO_INC_SYNC_TIMELINE_HANDLE;

	public Vector2Int BlockCount => kBlockCount;

	public int BlockSize => 100;

	public float TileSize => 2f;

	public Vector2Int TileCount { get; private set; }

	public Transform DynamicObjNode => dynamicObjNode;

	public Transform BuildBubbleNode => buildBubbleNode;

	public static int[] LodArray
	{
		get
		{
			if (lodArray == null)
			{
				LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetLodArray");
				List<int> list = new List<int>();
				for (int i = 0; i <= luaTable.Length; i++)
				{
					list.Add(luaTable.Get<int>(i));
				}
				lodArray = list.ToArray();
			}
			return lodArray;
		}
	}

	public GameObject SceneInstanceGameObject => sceneInst?.gameObject;

	public Transform Transform => gameObject.transform;

	public int CurTileCountXMin { get; private set; }

	public int CurTileCountXMax { get; private set; }

	public int CurTileCountYMin { get; private set; }

	public int CurTileCountYMax { get; private set; }

	public int WorldSize { get; private set; }

	private CityCamera Camera { get; set; }

	private CityStaticManager StaticManager { get; set; }

	private ModelManager ModelManager { get; set; }

	private FakeCityModelManager FakeCityModelManager { get; set; }

	private CityRobotManager CityRobotManager { get; set; }

	private CityInputManager InputManager { get; set; }

	public Vector3 CurTarget => Camera.CurTarget;

	public float InitZoom
	{
		get
		{
			return Camera?.InitZoom ?? 0f;
		}
		set
		{
			Camera.InitZoom = value;
		}
	}

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

	public int frameBufferWidth => Screen.width;

	public int frameBufferHeight => Screen.height;

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

	public FakeBuilding preCreateBuild
	{
		get
		{
			return FakeCityModelManager.preCreateBuild;
		}
		set
		{
			FakeCityModelManager.preCreateBuild = value;
		}
	}

	public Queue<FakeBuilding> placeFalseBuild
	{
		get
		{
			return FakeCityModelManager.placeFalseBuild;
		}
		set
		{
			FakeCityModelManager.placeFalseBuild = value;
		}
	}

	public float LodCameraDistanceScale => 1f;

	public event Action AfterUpdate
	{
		add
		{
			if (Camera != null)
			{
				Camera.AfterUpdate += value;
			}
		}
		remove
		{
			if (Camera != null)
			{
				Camera.AfterUpdate -= value;
			}
		}
	}

	public void Init(GameObject go)
	{
		gameObject = go;
		gameObject.SetActive(value: true);
		int num = (WorldSize = 100);
		CurTileCountXMin = 0;
		CurTileCountXMax = num - 1;
		CurTileCountYMin = 0;
		CurTileCountYMax = num - 1;
		TileCount = new Vector2Int(100, 100);
		SceneSkinManager.Instance.SetViewModeSkinMeta(null);
		GameEntry.Lua.Call("CSharpCallLuaInterface.SetIsInCity", param1: true);
		SceneManager.MarchDataMgr.WorldGetMarchInfos();
		string path = "Assets/Main/Prefabs/World/Scene_City2.prefab";
		GameEntry.Resource.PreloadAsset(path, typeof(GameObject));
		GameEntry.Resource.PreloadAsset((SceneQualitySetting.GetTerrainLevel() == 3) ? "Assets/Main/Prefabs/World/Terrain_City_High.prefab" : "Assets/Main/Prefabs/World/Terrain_City.prefab", typeof(GameObject));
		SetFogVisible(visible: true);
		GameEntry.Event.Subscribe(EventId.LWSeasonWeatherInfoUpdate, SeasonWeatherInfoUpdate);
		GameEntry.Event.Subscribe(EventId.BloodyNightActivityRefresh, OnBloodyNightActivityRefresh);
		GameEntry.Event.Subscribe(EventId.BatteryPowerResourceUpdated, OnBatteryPowerResourceUpdated);
		GameEntry.Event.Subscribe(EventId.LuaEntryEffectRefreshStatus, OnBatteryPowerResourceUpdated);
		GameEntry.Event.Subscribe(EventId.GF_pve_battle_exit, OnPveBattleExit);
		GameEntry.Event.Subscribe(EventId.GF_goto_pve_battle_loaded, OnPveBattleEnter);
	}

	public void OnSkinChange(bool ignoreCache)
	{
	}

	public Color GetLabelSkinColor(int skinId, int colorType)
	{
		return GameEntry.Lua.CallWithReturn<Color, int, int>("CSharpCallLuaInterface.GetLabelTextColor", skinId, colorType);
	}

	public float GetLabelSkinOffset(int skinId)
	{
		return GameEntry.Lua.CallWithReturn<float, int>("CSharpCallLuaInterface.GetTitleNameDeltaX", skinId);
	}

	public float GetLabelSkinSizeAdd(int skinId)
	{
		return GameEntry.Lua.CallWithReturn<float, int>("CSharpCallLuaInterface.GetWorldNameBgSize", skinId);
	}

	public GameObject GetPeopleById(int index)
	{
		return CityTruckManager.GetPeopleObjByIndex(index);
	}

	public float PausePeopleAndPlayAnim(int index, string anim)
	{
		return CityTruckManager.PauseAndPlayAnim(index, anim);
	}

	public void ResumePeople(int index)
	{
		CityTruckManager.Resume(index);
	}

	public void CreateScene(Action callback = null)
	{
		digCreate = false;
		isBuildComplete = false;
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
		if (sceneInst == null)
		{
			citySceneType = CitySceneType.DigDig;
			sceneInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/Scene_City2.prefab", ObjectPoolTag.City);
			sceneInst.completed += delegate
			{
				sceneInst.gameObject.transform.SetParent(gameObject.transform);
				if (_fogData != null)
				{
					InitFogAlpha(_fogData);
				}
				LoadCityPostProcessVolume();
				SetPostProcessQuality();
				Camera.SetTouchInputControllerEnable(able: true);
				cityGrid = sceneInst.gameObject.transform.Find("CityGrid");
				CreateDigLandLock();
				CheckInvokeCreateComplete(callback);
			};
		}
		try
		{
			GameEntry.pvrExtension.Init();
		}
		catch (Exception ex)
		{
			Log.Error("pvrExtension init fail " + ex.ToString());
		}
		if (terrainInst == null)
		{
			string prefabPath = ((SceneQualitySetting.GetTerrainLevel() == 3) ? "Assets/Main/Prefabs/World/Terrain_City_High.prefab" : "Assets/Main/Prefabs/World/Terrain_City.prefab");
			terrainInst = GameEntry.Resource.InstantiateAsync(prefabPath, ObjectPoolTag.City);
			terrainInst.completed += delegate
			{
				terrainInst.gameObject.transform.SetParent(gameObject.transform);
				terrainInst.gameObject.SetActive(value: true);
				UpdateView();
				CheckInvokeCreateComplete(callback);
			};
		}
		if (terrainSetting == null)
		{
			string path = ((SceneQualitySetting.GetTerrainLevel() == 3) ? "Assets/Main/Prefabs/World/TerrainSetting_City_High.asset" : "Assets/Main/Prefabs/World/TerrainSetting_City_Low.asset");
			terrainSetting = GameEntry.Resource.LoadAssetAsync(path, typeof(ScriptableObject));
			Asset asset = terrainSetting;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (terrainSetting != null)
				{
					ChangeTerrainSetting(terrainSetting.asset as TerrainSetting);
				}
			});
		}
		GameEntry.Data.Building.SetMainPos();
		if (subModules.Count == 0)
		{
			Camera = AddSubModule<CityCamera>();
			StaticManager = AddSubModule<CityStaticManager>();
			FakeCityModelManager = AddSubModule<FakeCityModelManager>();
			ModelManager = AddSubModule<ModelManager>();
			InputManager = AddSubModule<CityInputManager>();
			CityRobotManager = AddSubModule<CityRobotManager>();
			CityTruckManager = AddSubModule<CityCityTruckManager>();
			CityCollectAnimalManager = AddSubModule<CityCollectAnimalManager>();
			LodManager = AddSubModule<CityLodManager>();
			CityZoneGroundManager = AddSubModule<CityZoneGroundManager>();
			foreach (CityManagerBase subModule in subModules)
			{
				subModule.Init();
			}
			Camera.AfterUpdate += AfterCameraUpdate;
			Vector3 lookWorldPosition = new Vector3(97f, 0f, 97f);
			Camera.Lookat(lookWorldPosition);
		}
		CheckInvokeCreateComplete(callback);
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && curSkinMeta.IsDarknessMode())
		{
			int curServerId = GameEntry.Data.Player.GetCurServerId();
			if (IsDawn(curServerId))
			{
				CloseS4NightColorRenderer();
				return;
			}
			Color currentEvColorInDarknessSeason = LightDataManager.GetInstance().GetCurrentEvColorInDarknessSeason();
			Color currentFogColorInDarknessSeason = LightDataManager.GetInstance().GetCurrentFogColorInDarknessSeason();
			mCityNightRenderer = CrateS4NightColorRenderer(currentEvColorInDarknessSeason, currentFogColorInDarknessSeason);
		}
		else if (curSkinMeta != null && curSkinMeta.IsCityStrongholdMode())
		{
			SeasonWeatherInfoUpdate(null);
		}
	}

	public void LoadCityPostProcessVolume()
	{
		if (postProcessVolumeAsset != null)
		{
			postProcessVolumeAsset.Release();
			postProcessVolumeAsset = null;
		}
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta == null || curSkinMeta.cityPostProcessVolume.IsNullOrEmpty())
		{
			return;
		}
		string cityPostProcessVolume = curSkinMeta.cityPostProcessVolume;
		postProcessVolumeAsset = GameEntry.Resource.LoadAssetAsync(cityPostProcessVolume, typeof(VolumeProfile));
		if (postProcessVolumeAsset == null)
		{
			Log.Error("Failed to load postProcessVolumeAsset: {0}", cityPostProcessVolume);
			return;
		}
		Asset asset = postProcessVolumeAsset;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (postProcessVolumeAsset != null && !(postProcessVolumeAsset.asset == null))
			{
				if (SceneManager.CurrSceneID != 1)
				{
					postProcessVolumeAsset?.Release();
					postProcessVolumeAsset = null;
				}
				else
				{
					SetNewPostProcessVolume();
				}
			}
		});
	}

	public void SetNewPostProcessVolume()
	{
		VolumeProfile volumeProfile = postProcessVolumeAsset.asset as VolumeProfile;
		if (volumeProfile == null)
		{
			postProcessVolumeAsset?.Release();
			postProcessVolumeAsset = null;
			return;
		}
		if (sceneInst == null || sceneInst.gameObject == null)
		{
			postProcessVolumeAsset?.Release();
			postProcessVolumeAsset = null;
			return;
		}
		Volume componentInChildren = sceneInst.gameObject.GetComponentInChildren<Volume>(includeInactive: true);
		if (componentInChildren == null)
		{
			postProcessVolumeAsset?.Release();
			postProcessVolumeAsset = null;
		}
		else
		{
			componentInChildren.profile = volumeProfile;
			SetPostProcessQuality();
		}
	}

	public CityNightColorRenderer CrateS4NightColorRenderer(Color color, Color cityFogColor)
	{
		CityNightColorRenderer cityNightColorRenderer = ScriptableObject.CreateInstance<CityNightColorRenderer>();
		cityNightColorRenderer.AttachToRenderer(Camera.camera, color, cityFogColor);
		return cityNightColorRenderer;
	}

	public void CloseS4NightColorRenderer()
	{
		if (mCityNightRenderer != null)
		{
			mCityNightRenderer.DetachFromRenderer(Camera.camera);
		}
	}

	private void CreateDigLandLock(Action callback = null)
	{
		if (!GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.CanShowLandLock") && sceneInst_dig == null)
		{
			digCreate = true;
			sceneInst_dig = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/Scene_City_Dig.prefab");
			sceneInst_dig.completed += delegate
			{
				sceneInst_dig.gameObject.transform.SetParent(gameObject.transform);
				GameEntry.Lua.Call("CSharpCallLuaInterface.OnLoadSceneOK", (object)base.transform, "Assets/Main/Prefabs/World/Scene_City_Dig.prefab");
				CheckInvokeCreateComplete(callback);
			};
		}
	}

	private void CheckInvokeCreateComplete(Action onComplete)
	{
		if (terrainInst != null && terrainInst.isDone && sceneInst != null && sceneInst.isDone && (!digCreate || (sceneInst_dig != null && sceneInst_dig.isDone)))
		{
			isBuildComplete = true;
			onComplete?.Invoke();
		}
	}

	public void RemoveFakeSampleMarchData(long index)
	{
	}

	public void UpdateFakeSampleMarchDataWhenBack(long index, long startTime, long endTime)
	{
	}

	public void UpdateFakeSampleMarchDataWhenStartPick(long index, long endTime)
	{
	}

	public void AddFakeSampleMarchData(long startIndex, long endIndex, long startTime, long endTime, int marchTargetType)
	{
	}

	public void EndDig()
	{
		if (sceneInst != null)
		{
			sceneInst.Destroy();
			sceneInst = null;
		}
		if (sceneInst_dig != null)
		{
			sceneInst_dig.Destroy();
			sceneInst_dig = null;
		}
		if (terrainInst != null)
		{
			terrainInst.gameObject.SetActive(value: true);
		}
		StaticManager?.ToggleShow(t: true);
	}

	public void UninitSubModulesAndCameraUpdate()
	{
		foreach (CityManagerBase subModule in subModules)
		{
			subModule?.UnInit();
		}
		subModules.Clear();
		if (Camera != null)
		{
			Camera.AfterUpdate -= AfterCameraUpdate;
		}
	}

	public void UninitSubModulesAndCameraUpdate_withoutCameraMove()
	{
		foreach (CityManagerBase subModule in subModules)
		{
			if (subModule != null)
			{
				if (subModule is CityCamera)
				{
					(subModule as CityCamera).UnInit_withoutMove();
				}
				else
				{
					subModule.UnInit();
				}
			}
		}
		subModules.Clear();
		if (Camera != null)
		{
			Camera.AfterUpdate -= AfterCameraUpdate;
		}
	}

	public void UninitCameraMovement()
	{
		Camera.UnInit_withMove();
	}

	public void Uninit()
	{
		UninitSubModulesAndCameraUpdate();
		if (terrainInst != null)
		{
			terrainInst.Destroy();
			terrainInst = null;
		}
		if (terrainInstNew != null)
		{
			terrainInstNew.Destroy();
			terrainInstNew = null;
		}
		foreach (KeyValuePair<string, Asset> terrainMatAsset in terrainMatAssets)
		{
			terrainMatAsset.Value?.Release();
		}
		terrainMatAssets.Clear();
		if (terrainSetting != null)
		{
			terrainSetting.Release();
			terrainSetting = null;
		}
		if (terrainSettingNew != null)
		{
			terrainSettingNew.Release();
			terrainSettingNew = null;
		}
		if (buildBubbleNode != null)
		{
			UnityEngine.Object.Destroy(buildBubbleNode.gameObject);
			buildBubbleNode = null;
		}
		if (dynamicObjNode != null)
		{
			UnityEngine.Object.Destroy(dynamicObjNode.gameObject);
			dynamicObjNode = null;
		}
		if (fowSystem != null)
		{
			fowSystem.ClearRevealers();
			fowSystem.Clear();
			fowSystem = null;
		}
		if (postProcessVolumeAsset != null)
		{
			postProcessVolumeAsset.Release();
			postProcessVolumeAsset = null;
		}
		digCreate = false;
		isBuildComplete = false;
		GameEntry.Event.Unsubscribe(EventId.LWSeasonWeatherInfoUpdate, SeasonWeatherInfoUpdate);
		GameEntry.Event.Unsubscribe(EventId.BloodyNightActivityRefresh, OnBloodyNightActivityRefresh);
		GameEntry.Event.Unsubscribe(EventId.BatteryPowerResourceUpdated, OnBatteryPowerResourceUpdated);
		GameEntry.Event.Unsubscribe(EventId.LuaEntryEffectRefreshStatus, OnBatteryPowerResourceUpdated);
		GameEntry.Event.Unsubscribe(EventId.GF_pve_battle_exit, OnPveBattleExit);
		GameEntry.Event.Unsubscribe(EventId.GF_goto_pve_battle_loaded, OnPveBattleEnter);
		CloseS4NightColorRenderer();
		CloseWeatherRenderer();
	}

	private T AddSubModule<T>() where T : CityManagerBase
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
		ChangeTerrain();
	}

	public void Update()
	{
		UpdateSubModule();
	}

	private void ChangeTerrain()
	{
		Action onChangeComplete = delegate
		{
			if (terrainInstNew != null && terrainInstNew.isDone && terrainSettingNew != null && terrainSettingNew.isDone)
			{
				if (terrainInst != null)
				{
					terrainInst.Destroy();
				}
				if (terrainSetting != null)
				{
					terrainSetting.Release();
				}
				terrainInstNew.gameObject.transform.SetParent(gameObject.transform);
				ChangeTerrainSetting(terrainSettingNew.asset as TerrainSetting);
				terrainInst = terrainInstNew;
				terrainSetting = terrainSettingNew;
				terrainInstNew = null;
				terrainSettingNew = null;
				UpdateView();
			}
		};
		if (terrainInstNew != null)
		{
			terrainInstNew.Destroy();
			terrainInstNew = null;
		}
		string prefabPath = ((SceneQualitySetting.GetTerrainLevel() == 3) ? "Assets/Main/Prefabs/World/Terrain_City_High.prefab" : "Assets/Main/Prefabs/World/Terrain_City.prefab");
		terrainInstNew = GameEntry.Resource.InstantiateAsync(prefabPath);
		terrainInstNew.completed += delegate
		{
			onChangeComplete();
		};
		string path = ((SceneQualitySetting.GetTerrainLevel() == 3) ? "Assets/Main/Prefabs/World/TerrainSetting_High.asset" : "Assets/Main/Prefabs/World/TerrainSetting_Low.asset");
		terrainSettingNew = GameEntry.Resource.LoadAssetAsync(path, typeof(ScriptableObject));
		Asset asset = terrainSettingNew;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			onChangeComplete();
		});
	}

	private void UpdateView()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta == null || curSkinMeta.city_terrain.IsNullOrEmpty())
		{
			return;
		}
		MeshRenderer[] componentsInChildren = terrainInst.gameObject.transform.GetComponentsInChildren<MeshRenderer>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		string city_terrain = curSkinMeta.city_terrain;
		if (string.IsNullOrEmpty(city_terrain))
		{
			return;
		}
		if (!terrainMatAssets.ContainsKey(city_terrain))
		{
			Asset value = GameEntry.Resource.LoadAsset(city_terrain, typeof(Material));
			terrainMatAssets[city_terrain] = value;
		}
		Asset asset = terrainMatAssets[city_terrain];
		if (asset != null && asset.asset != null)
		{
			Material sharedMaterial = (Material)asset.asset;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].sharedMaterial = sharedMaterial;
			}
		}
	}

	private void ChangeTerrainSetting(TerrainSetting setting)
	{
		Shader.SetGlobalTexture(Prop_Control, setting.control);
		Shader.SetGlobalTexture(Prop_Splat0, setting.splat0);
		Shader.SetGlobalTexture(Prop_Splat1, setting.splat1);
		Shader.SetGlobalVector(Prop_Control_ST, setting.control_st);
		Shader.SetGlobalVector(Prop_Splat0_ST, setting.splat0_st);
		Shader.SetGlobalVector(Prop_Splat1_ST, setting.splat1_st);
		Shader.SetGlobalVector(Prop_TerrainBounds, setting.terrainBounds);
	}

	private void OnDrawGizmos()
	{
		if (Camera != null)
		{
			Camera.OnDrawGizmos();
		}
	}

	private void UpdateSubModule()
	{
		foreach (CityManagerBase subModule in subModules)
		{
			subModule.OnUpdate(Time.deltaTime);
		}
	}

	public void FixedUpdate()
	{
	}

	public Vector3 TileToWorld(Vector2Int tilePos)
	{
		return TileCoord.TileToWorld(tilePos, 0);
	}

	public Vector3 TileToWorld(int tilePosX, int tilePosY)
	{
		return TileCoord.TileToWorld(tilePosX, tilePosY, 0);
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
		return TileCoord.TileFloatToWorld(tilePos, 0);
	}

	public Vector3 TileFloatToWorld(float x, float y)
	{
		return TileCoord.TileFloatToWorld(x, y, 0);
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
		return TileCoord.TileIndexToWorld(index, TileCount, 0);
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
		int num = size / 2;
		CurTileCountXMin = Math.Max(0, 50 - num);
		CurTileCountXMax = Math.Min(99, 50 + num - 1);
		CurTileCountYMin = Math.Max(0, 50 - num);
		CurTileCountYMax = Math.Min(99, 50 + num - 1);
	}

	public void SetWorldSize(int width, int height)
	{
		int worldSize = Mathf.Max(width, height);
		WorldSize = worldSize;
		int num = width / 2;
		int num2 = height / 2;
		CurTileCountXMin = Math.Max(0, 50 - num);
		CurTileCountXMax = Math.Min(99, 50 + num - 1);
		CurTileCountYMin = Math.Max(0, 50 - num2);
		CurTileCountYMax = Math.Min(99, 50 + num2 - 1);
	}

	public void SetMapZoneActive(bool active)
	{
	}

	public int GetBuildOffsetRangeByBuildId(int buildId)
	{
		return GameEntry.ConfigCache.GetTemplateData("building", buildId, "offer_range").ToInt();
	}

	private void AfterCameraUpdate()
	{
		if (!DisableClampToEdge)
		{
			Camera.ClampToEdge();
		}
	}

	public void ChangeServer(int serverId)
	{
	}

	public void OnChangeServerRemove()
	{
	}

	public void RemoveBlackDesert()
	{
	}

	public void InitBlackBlock()
	{
	}

	public GameObject GetDragonLandRangeObj()
	{
		return null;
	}

	public void CreateDragonLandRange()
	{
	}

	public void RemoveDragonLandRange()
	{
	}

	public void RemoveDragonLandPoint(int pointIndex)
	{
	}

	public void HandlePushWolfStatusChange(ISFSObject message)
	{
	}

	public List<BuildPointInfo> GetMainByScreen()
	{
		return new List<BuildPointInfo>();
	}

	public List<BuildPointInfo> GetAllMainBaseList()
	{
		return new List<BuildPointInfo>();
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0)
	{
		return new List<BuildPointInfo>();
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1)
	{
		return new List<BuildPointInfo>();
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1, PlayerType t2)
	{
		return new List<BuildPointInfo>();
	}

	public List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1, PlayerType t2, PlayerType t3)
	{
		return new List<BuildPointInfo>();
	}

	public List<PointInfo> GetAllDragonPointList()
	{
		return new List<PointInfo>();
	}

	public List<int> GetAllDragonResourceList()
	{
		return new List<int>();
	}

	public void ShowTruck(List<List<Vector2Int>> _path)
	{
		throw new NotImplementedException();
	}

	public void ShowFreeTruck(List<List<Vector2Int>> _path, WorldCityTruck freeTruck)
	{
		throw new NotImplementedException();
	}

	private int GetFreeTruckIndex(int innerBuildCount)
	{
		throw new NotImplementedException();
	}

	private void ShowRandomTruck()
	{
		throw new NotImplementedException();
	}

	public List<Vector2Int> GetTruckPathString(List<Vector2Int> tempList)
	{
		throw new NotImplementedException();
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
		num %= 100;
		num += offset;
		if (num >= 0 && num < 100)
		{
			return index + offset;
		}
		return 0;
	}

	private int GetIndexByOffsetY(int index, int offset = 1)
	{
		int num = index - 1;
		num /= 100;
		num += offset;
		if (num >= 0 && num < 100)
		{
			return index + 100 * offset;
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

	public void OnExtendDome()
	{
	}

	public void ShowBattleBlood(object param, string path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab")
	{
	}

	public void RegisterPhysics(MonoBehaviour obj)
	{
	}

	public int GetBuildTileByItemId(int itemId)
	{
		return GameEntry.ConfigCache.GetTemplateData("building", itemId, "tiles").ToInt();
	}

	public int GetAllianceCitySizeByItemId(int itemId)
	{
		return 7;
	}

	public int GetTreasureSizeByItemId(int itemId)
	{
		return 1;
	}

	public int GetDragonBuildSizeByItemId(int itemId)
	{
		return 1;
	}

	public void UnregisterPhysics(MonoBehaviour obj)
	{
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
	}

	public void QuitFocus(float time)
	{
		Camera.QuitFocus(time);
	}

	public float GetLodDistance()
	{
		if (Camera == null)
		{
			return 0f;
		}
		return Camera.GetLodDistance();
	}

	public float GetLodDistanceByLod(int lod)
	{
		return Camera?.InitZoom ?? 0f;
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

	public void Lookat(Vector3 lookWorldPosition)
	{
		Camera.Lookat(lookWorldPosition);
	}

	public void AutoLookat(Vector3 lookat, float zoom = -1f, float time = 0.2f, Action onComplete = null)
	{
		Camera.AutoLookat(lookat, zoom, time, onComplete);
	}

	public void AutoZoom(float zoom, float time = 0.2f, Action onComplete = null)
	{
		Camera.AutoZoom(zoom, time, onComplete);
	}

	public Vector3 ScreenPointToWorld(Vector3 worldPos, float disPlane = 0f)
	{
		return Camera.ScreenPointToWorld(worldPos, disPlane);
	}

	public int GetLodLevel()
	{
		return 1;
	}

	public void TrackMarch(long marchId)
	{
		Camera.TrackMarch(marchId);
	}

	public void TrackHSR(long marchId, GameObject go)
	{
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
		InputManager.SetTouchInputControllerEnable(able);
	}

	public void SetCameraTouchEnable(bool able)
	{
		Camera.SetTouchInputControllerEnable(able);
	}

	public void UpdateTroopLineColor(string colorArgs)
	{
	}

	public bool GetTouchInputControllerEnable()
	{
		return InputManager.GetTouchInputControllerEnable();
	}

	public void StopCameraMove()
	{
		Camera?.StopMove();
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

	public List<MobileTouchCamera.ZoomParam> GetZoomParams()
	{
		return Camera.GetZoomParams();
	}

	public float GetFOV()
	{
		return Camera.GetFOV();
	}

	public bool IsInMap(Vector2Int pt)
	{
		if (pt.x < CurTileCountXMin || pt.x > CurTileCountXMax || pt.y < CurTileCountYMin || pt.y > CurTileCountYMax)
		{
			return false;
		}
		return true;
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
		if (tilePos.x > 99)
		{
			tilePos.x = 99;
		}
		if (tilePos.y < 0)
		{
			tilePos.y = 0;
		}
		if (tilePos.y > 99)
		{
			tilePos.y = 99;
		}
		return tilePos;
	}

	public int GetCollectResourceBuildRange()
	{
		return 0;
	}

	public int GetCollectResourceTile()
	{
		return 0;
	}

	public void CheckNeedRefreshRoad()
	{
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
		return false;
	}

	public void ProfileToggleTerrain()
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

	public ExplorePointInfo GetExplorePointInfoByIndex(int pointIndex)
	{
		return null;
	}

	public SamplePointInfo GetSamplePointInfoByIndex(int pointIndex)
	{
		return null;
	}

	public DetectRetryTaskPointInfo GetDetectRetryTaskPointInfo(int pointIndex)
	{
		return null;
	}

	public DetectAttackCityS0TaskPointInfo GetDetectAttackCityS0TaskPointInfo(int pointIndex)
	{
		return null;
	}

	public HeroDispatchMissionPointInfo GetHeroDispatchTaskPointInfoByIndex(int pointIndex)
	{
		return null;
	}

	public GhostreconPointInfo GetGhostreconPointInfoByIndex(int pointIndex)
	{
		return null;
	}

	public ResPointInfo GetResourcePointInfoByIndex(int pointIndex)
	{
		return null;
	}

	public bool IsCollectRangePoint(int pointIndex)
	{
		return false;
	}

	public WorldTriggerData GetWorldTriggerData(long uuid)
	{
		return null;
	}

	public WorldTriggerData GetTriggerDataByPointId(int pointIndex, int serverId)
	{
		return null;
	}

	public PointInfo GetPointInfo(int pointIndex)
	{
		return null;
	}

	public PointInfo GetPointInfoWithServer(int pointIndex, int serverId)
	{
		return null;
	}

	public WorldDesertInfo GetWorldDesertInfo(int pointIndex)
	{
		return null;
	}

	public WorldMarch GetMarchesByStartIndex(int pointIndex)
	{
		return null;
	}

	public WorldTileInfo GetWorldTileInfo(int pointIndex)
	{
		return null;
	}

	public PointInfo GetYellowLand(int pointIndex)
	{
		return null;
	}

	public CollectPointInfo GetCollectRangePoint(int pointIndex)
	{
		return null;
	}

	public int GetCollectPoint(int resourceType)
	{
		return 0;
	}

	public List<int> GetAllCollectRangePoint(int resourceType)
	{
		return null;
	}

	public List<int> GetAllCollectRangePointType(int resourceType, int mainIndex)
	{
		return null;
	}

	public bool IsCollectPoint(int pointIndex)
	{
		return false;
	}

	public CollectPointInfo GetCollectInfoByIndex(int pointIndex)
	{
		return null;
	}

	public GarbagePointInfo GetGarbagePointInfoByIndex(int pointIndex)
	{
		return null;
	}

	public PointInfo GetPointInfoByUuid(long uuid)
	{
		return null;
	}

	public WorldDesertInfo GetDesertInfoByUuid(long uuid)
	{
		return null;
	}

	public PointInfo GetMyPointInfo()
	{
		return null;
	}

	public WorldPointObject GetObjectByPoint(int pointIndex)
	{
		return null;
	}

	public bool IsOutCityByPoint(int point)
	{
		return false;
	}

	public BuildPointInfo GetBaseMainByScreen()
	{
		return null;
	}

	public void ShowObject(int point)
	{
		(ModelManager?.GetObjectByPointId(point))?.SetIsVisible(visible: true);
	}

	public CityBuilding GetBuildingByPoint(int pointIndex)
	{
		if (ModelManager != null)
		{
			ModelManager.ModelObject objectByPointId = ModelManager.GetObjectByPointId(pointIndex);
			if (objectByPointId != null && objectByPointId is ModelManager.BuildObject buildObject)
			{
				return buildObject.cityBuilding;
			}
		}
		return null;
	}

	public void SetLevelUpActive(int pointIndex, bool active)
	{
		CityBuilding buildingByPoint = GetBuildingByPoint(pointIndex);
		if (!(buildingByPoint == null))
		{
			buildingByPoint.SetBuildUpLevelTipActive(active);
		}
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
	}

	public void HandleViewAssistanceInfoUpdateNotify(ISFSObject message)
	{
	}

	public void HandleViewUpdateNotify(ISFSObject message)
	{
	}

	public void HandleViewTileUpdateNotify(ISFSObject message)
	{
	}

	public void HandleLandUpdate(ISFSObject message)
	{
	}

	public void SendViewRequest(Vector2Int tilePos, int viewLevel, int serverId)
	{
	}

	public bool IsNeedPlayPlacedAnim(long uuid)
	{
		return false;
	}

	public bool IsSelfRoad(int index)
	{
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsTruckRoad", index);
	}

	public bool IsBuildFinish()
	{
		return isBuildComplete;
	}

	public WorldPointObject GetObjectByUuid(long uuid)
	{
		return null;
	}

	public CityBuilding GetBuildingByUuid(long uuid)
	{
		return null;
	}

	public WorldBuilding GetWorldBuildingByPoint(int pointIndex)
	{
		return null;
	}

	public WorldBuilding GetWorldBuildingByUuid(long uuid)
	{
		return null;
	}

	public bool HasPointInfo(int pointIndex)
	{
		return GetPointType(pointIndex) != 0;
	}

	public void AddToDeleteList(int index)
	{
	}

	public BuildPointInfo GetBaseMainInfoByOwnerUid(string ownerUid)
	{
		return null;
	}

	public void HideObject(int point)
	{
		(ModelManager?.GetObjectByPointId(point))?.SetIsVisible(visible: false);
	}

	public bool IsSelfPoint(int pointIndex)
	{
		return false;
	}

	public int GetPointType(int index)
	{
		return GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetCityPointType", index);
	}

	public bool IsSelfFreeBoard(int index)
	{
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsTruckRoad", index);
	}

	public void AddObjectByPoint(int point, int type)
	{
		if (ModelManager != null)
		{
			ModelManager.LoadOneObject(point, type);
		}
	}

	public void RemoveObjectByPoint(int point)
	{
		if (ModelManager != null)
		{
			ModelManager.RemoveOneObject(point);
		}
	}

	public void RemoveOneObjectByPointType(int index, int pointType)
	{
		if (ModelManager != null)
		{
			ModelManager.RemoveOneObjectByPointType(index, pointType);
		}
	}

	public void RefreshView()
	{
		if (FakeCityModelManager != null)
		{
			FakeCityModelManager.UIDestroyBuilding();
			FakeCityModelManager.UIDestroyRoad();
		}
		if (ModelManager != null)
		{
			ModelManager.ClearReInitObjectByFilter();
			ModelManager.ReInitObject();
		}
	}

	public void OnMainBuildMove()
	{
		CityTruckManager.ClearAllTruck();
	}

	public void UpdateViewRequest(bool isForce = false)
	{
	}

	public void SetFirstViewRequestFlag(bool isFirstTime)
	{
	}

	public bool IsRoadPoint(int index, string uid, int dir)
	{
		return FakeCityModelManager.HasBoard(index, uid, dir);
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
		for (int num = buildingNeighbors.Count - 1; num >= 0; num--)
		{
			if (!PathUtils.IsRoad(buildingNeighbors[num]))
			{
				buildingNeighbors.RemoveAt(num);
			}
		}
		for (int i = 0; i < truckPathMainOutPosList.Count; i++)
		{
			Vector2Int start = truckPathMainOutPosList[i];
			for (int j = 0; j < buildingNeighbors.Count; j++)
			{
				Vector2Int end = buildingNeighbors[j];
				List<Vector2Int> list2 = CityTruckManager.TruckManagerBase.FindRoadPath(start, end, FindPathType.OnlyRoad);
				if (list2.Count != 0 && (list.Count == 0 || list2.Count < list.Count))
				{
					list = list2;
				}
			}
		}
		return list;
	}

	public List<int> GetGarbagePoint()
	{
		return null;
	}

	public PlayerType IsMyEnemy(int serverId, string allianceId)
	{
		return PlayerType.PlayerOther;
	}

	public void CleanAllianceCacheData()
	{
	}

	public WorldTroop CreateGroupTroop(WorldMarch march)
	{
		return null;
	}

	public WorldMarch GetMarch(long uuid)
	{
		return null;
	}

	public WorldMarch GetMonster(long targetPoint)
	{
		return null;
	}

	public bool IsTargetForMine(WorldMarch marchData)
	{
		return false;
	}

	public bool IsTargetForAlly(WorldMarch marchData)
	{
		return false;
	}

	public void StartMarch(int targetType, int targetPoint, long targetUuid, int timeIndex, long marchUuid = 0L, long formationUuid = 0L, int backHome = 1, byte[] sfsObjBinary = null, int startPos = 0, int targetServerId = -1)
	{
	}

	public WorldMarch GetOwnerFormationMarch(string ownerUid, long formationUuid, string allianceUid = "")
	{
		return null;
	}

	public WorldMarch GetAllianceMarchesInTeam(string allianceUid, long teamUuid)
	{
		return null;
	}

	public List<WorldMarch> GetOwnerMarches(string ownerUid, string allianceUid = "")
	{
		return null;
	}

	public void HandlePushWorldTriggerUpdate(ISFSObject message, int serverId, int worldId)
	{
	}

	public void HandlePushWorldTriggerDel(ISFSObject message, int serverId, int worldId)
	{
	}

	public void HandlePushMultiKillUpdate(ISFSObject message)
	{
	}

	public void HandlePushWorldMarchAdd(ISFSObject message)
	{
	}

	public void HandlePushWorldMarchDel(ISFSObject message)
	{
	}

	public void HandleWorldMarchGet(ISFSObject message)
	{
	}

	public void HandleFormationMarch(ISFSObject message)
	{
	}

	public void HandleFormationMarchChange(ISFSObject message)
	{
	}

	public void HandleUpdateLightData(ISFSObject message)
	{
	}

	public bool ExistMarch(long uuid)
	{
		return false;
	}

	public bool IsInRallyMarch(long uuid)
	{
		return false;
	}

	public bool IsInCollectMarch(long uuid)
	{
		return false;
	}

	public bool IsInAssistanceMarch(long uuid)
	{
		return false;
	}

	public bool IsSelfInCurrentMarchTeam(long rallyMarchUuid)
	{
		return false;
	}

	public int GetMyAssistanceCount(int pointIndex)
	{
		return 0;
	}

	public int GetMyAssistanceFirstHero(int pointIndex)
	{
		return 0;
	}

	public void MarkPointIsDirty(long dirtyPointUuid)
	{
	}

	public Dictionary<long, WorldMarch> GetMarchesBossInfo()
	{
		return new Dictionary<long, WorldMarch>();
	}

	public void GetMonsterListInArea(Vector2Int center, int size, Dictionary<int, int> monsterIds, Dictionary<long, Vector2Int> result)
	{
	}

	public void DestroyBerserkBossMarchData(long marchUuid)
	{
	}

	public string SaveCreateMarchRecordTime()
	{
		return "";
	}

	public void RemoveCreateMarchRecordTime(WorldMarch worldMarch)
	{
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
	}

	public void ShowLoad(Vector3 pos)
	{
		InputManager.ShowLoad(pos);
	}

	public void SetDragFormationData(long uuid, int pointId)
	{
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

	public void AddOccupyPoints(Vector2Int p, Vector2Int size, int serverIn = 0)
	{
		StaticManager.AddOccupyPoints(p, size);
	}

	public void RemoveOccupyPoints(Vector2Int p, Vector2Int size, int serverIn = 0)
	{
		StaticManager.RemoveOccupyPoints(p, size);
	}

	public void GreenAreaChange(WorldAreaGreenInfo.GreenType type, HashSet<int> changePoints = null)
	{
	}

	public void UpdateGreenArea(int xMin, int yMin, int xMax, int yMax)
	{
	}

	public bool IsGreen(int pointIndex)
	{
		return false;
	}

	public bool CanGreen(int pointIndex, bool showTip = false)
	{
		return false;
	}

	public void SetStaticVisibleChunk(int range)
	{
		StaticManager.SetVisibleChunk(range);
	}

	public float GetModelHeight(long marchUuid)
	{
		return 0f;
	}

	public WorldTroop GetTroop(long marchUuid)
	{
		return null;
	}

	public EnumDestinationSignalType GetDestinationType(long marchUuid, long targetMarchUuid, int endPos, MarchTargetType targetType, bool isFormation, ref Vector3 realPos, ref int tileSize)
	{
		if (!GameEntry.Data.Fog.IsUnlock(WorldToTileIndex(realPos)))
		{
			return EnumDestinationSignalType.UnReachAble;
		}
		return targetType switch
		{
			MarchTargetType.STATE => EnumDestinationSignalType.EmptyGround, 
			MarchTargetType.PICK_GARBAGE => EnumDestinationSignalType.My, 
			MarchTargetType.ATTACK_MONSTER => EnumDestinationSignalType.EnemyMarch, 
			_ => EnumDestinationSignalType.None, 
		};
	}

	public MarchTargetType GetTargetType(long targetMarchUuid, int pointId)
	{
		if (GameEntry.Data.Fog.IsUnlock(pointId))
		{
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", pointId);
			if (luaTable != null)
			{
				switch (luaTable.Get<int>("type"))
				{
				case 1:
					return MarchTargetType.PICK_GARBAGE;
				case 102:
					return MarchTargetType.PICK_GARBAGE;
				case 2:
					return MarchTargetType.ATTACK_MONSTER;
				}
			}
		}
		return MarchTargetType.STATE;
	}

	public void CreateBattleVFX(string prefabPath, float life, Action<GameObject> onComplete)
	{
	}

	public int CreateVFX(string prefabPath, Vector3 pos, float duration, float delay = 0f, Action<GameObject> onComplete = null)
	{
		return 0;
	}

	public void RemoveVFX(int id)
	{
	}

	public void OnTroopDragUpdate(long marchUuid, Vector3 dragPosCurrent, long targetMarchUuid, int startPointId = 0, bool isFormation = false)
	{
	}

	public void OnTroopDragStop(long marchUuid, long targetMarchUuid, bool isFormation = false)
	{
	}

	public int GetPointSize(int index)
	{
		return GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetCityPointSize", index);
	}

	public bool IsTroopCreate(long marchUuid)
	{
		return false;
	}

	public void CreateTroop(WorldMarch march)
	{
	}

	public void UpdateTroop(WorldMarch march)
	{
	}

	public float DestroyTroop(long marchUuid, bool isBattleFailed = false)
	{
		return 0f;
	}

	public void CreateTroopLine(WorldMarch march)
	{
	}

	public void DestroyTroopLine(long marchUuid)
	{
	}

	public void UpdateTroopLine(WorldMarch march, WorldTroopPathSegment[] path, int currPath, Vector3 currPos, int realTargetPos = 0, bool needRefresh = false, bool clear = false)
	{
	}

	public bool IsTruckRoad(Vector2Int point, FindPathType findPathType)
	{
		return CityTruckManager.TruckManagerBase.IsTruckRoad(point, findPathType);
	}

	public void RemoveCullingBounds(WorldCulling.ICullingObject cullingObject)
	{
	}

	public void AddCullingBounds(WorldCulling.ICullingObject cullingObject)
	{
	}

	public void BattleFinish(ISFSObject message)
	{
	}

	public void UpdateBattleMessage(ISFSObject message)
	{
	}

	public void StartPrintRoad(List<int> roads, bool isOther)
	{
		FakeCityModelManager.StartPrintRoad(roads, isOther);
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
			FakeCityModelManager.StartPrintRoad(list, isOther, buildPerRoadTime);
		}
	}

	public void FinishPrintRoad(int pointId)
	{
		FakeCityModelManager.FinishPrintRoad(pointId);
	}

	public void UICreateBoard(int index, bool isAfter = true)
	{
		FakeCityModelManager.UICreateBoard(index, isAfter);
	}

	public void UIHideBoard(int deleteCount, bool isAfter = true)
	{
		FakeCityModelManager.UIHideBoard(deleteCount, isAfter);
	}

	public List<int> UIDestroyRoad()
	{
		return FakeCityModelManager.UIDestroyRoad();
	}

	public void UICreateBuilding(int buildId, long buildUuid, int point, int buildTopType, LuaTable noBuildListStr = null)
	{
		FakeCityModelManager.UICreateBuilding(buildId, buildUuid, point, buildTopType, noBuildListStr);
	}

	public void UICreateAllianceBuilding(int buildId, long buildUuid, int point, int buildTopType, LuaTable noBuildListStr = null)
	{
	}

	public void UIChangeBuilding(int index)
	{
		FakeCityModelManager.UIChangeBuilding(index);
	}

	public void UIDestroyBuilding()
	{
		FakeCityModelManager.UIDestroyBuilding();
	}

	public void UIChangeRoad()
	{
		FakeCityModelManager.UIChangeRoad();
	}

	public void UIChangeAllianceBuilding(int index)
	{
	}

	public void UIDestroyAllianceBuilding()
	{
	}

	public void UICreateWorldMoveMarch(string modelPath, long uuid, Vector3 pos)
	{
	}

	public void UICreateWorldTrigger(string modelPath, int uuid, int pos, int skillId, int serverId)
	{
	}

	public void UIDestroyRreCreateMarch()
	{
	}

	public void UIDestroyPreCreateTrigger()
	{
	}

	public void UICreateWorldFlowerTrain(string modelPath, int pointId, int goodsId)
	{
	}

	public void UIDestroyPreCreateFlowerTrain()
	{
	}

	public void UIDestroyRreCreateBuild()
	{
		FakeCityModelManager.UIDestroyRreCreateBuild();
	}

	public void UIDestroyRreCreateAllianceBuild()
	{
	}

	public ModelManager.ModelObject AddObjectByPointId(int index, int type)
	{
		if (ModelManager != null)
		{
			return ModelManager.LoadOneObject(index, type);
		}
		return null;
	}

	public ModelManager.ModelObject GetObjectByPointId(int index)
	{
		if (ModelManager != null)
		{
			return ModelManager.GetObjectByPointId(index);
		}
		return null;
	}

	public void CreateRoadRobot(List<int> roads, bool isOther, long printId)
	{
		CityRobotManager.CreateRoadRobot(roads, isOther, printId);
	}

	public void AddToNeedRemoveList(long bUuid)
	{
		CityRobotManager.AddToNeedRemoveList(bUuid);
	}

	public WorldRoadRobot GetRoadRobot(int roodPoint)
	{
		return CityRobotManager.GetRoadRobot(roodPoint);
	}

	public void ChangeBuildRobotState(long bUuid, WorldBuildRobot.State state)
	{
		CityRobotManager.ChangeBuildRobotState(bUuid, state);
	}

	public void ChangeBuildRobotFinishTime(long bUuid, float finishTime)
	{
		CityRobotManager.ChangeBuildRobotFinishTime(bUuid, finishTime);
	}

	public void CreateBuildRobot(long bUuid, Vector3 targetPos, float height, float duration, int tileSizeX, int tileSizeY, bool isTransit = false)
	{
		CityRobotManager.CreateBuildRobot(bUuid, targetPos, height, duration, tileSizeX, tileSizeY, isTransit);
	}

	public void CreateOtherBuildRobot(long bUuid, Vector3 targetPos, float height, float duration, int tileSizeX, int tileSizeY)
	{
		CityRobotManager.CreateOtherBuildRobot(bUuid, targetPos, height, duration, tileSizeX, tileSizeY);
	}

	public WorldBuildRobot GetBuildRobot(long bUuid)
	{
		return CityRobotManager.GetBuildRobot(bUuid);
	}

	public void CreateAnimalObject(long uuid)
	{
		CityCollectAnimalManager.CreateAnimalObject(uuid);
	}

	public void DestroyAnimalObject(long uuid)
	{
		CityCollectAnimalManager.DestroyAnimalObject(uuid);
	}

	public void InitFogOfWar(BitArray fogData)
	{
		kFogTileSize = 2f * (float)DCFog.FogSizeX;
		kFogTile2x2Size = kFogTileSize / 2f;
		kFogTileCount = DCFog.FogTileCount;
		kFogTile2x2Count = new Vector2Int(DCFog.FogTileCount.x * 2, DCFog.FogTileCount.y * 2);
		kFogTileTotalCount = kFogTileCount.x * kFogTileCount.y;
		_fogData = fogData;
		if (_fogData != null)
		{
			InitFogAlpha(fogData);
		}
	}

	public void ReInitFogOfWar()
	{
		SceneManager.World.InitFogOfWar(GameEntry.Data.Fog.GetAllFogData());
	}

	private void InitFogAlpha(BitArray fogData)
	{
		if (fowSystem != null || fogData.Length <= kFogTileTotalCount / 8 || fogData.Length < 20000 || !GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsBeforePrologue"))
		{
			return;
		}
		GameObject gameObject = new GameObject("FOWSystem", typeof(FOWSystem));
		fowSystem = gameObject.GetComponent<FOWSystem>();
		fowSystem.transform.position = new Vector3(110f, 0f, 110f);
		fowSystem.worldSize = 256;
		fowSystem.textureSize = 512;
		fowSystem.updateFrequency = 0.33f;
		fowSystem.textureBlendTime = 0.2f;
		fowSystem.blurIterations = 10;
		UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
		if (universalRenderPipelineAsset != null)
		{
			ScriptableRendererData[] obj = (ScriptableRendererData[])universalRenderPipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(universalRenderPipelineAsset);
			foreach (ScriptableRendererFeature rendererFeature in ((obj != null) ? obj[0] : null).rendererFeatures)
			{
				if (rendererFeature is HeightFogRenderFeature)
				{
					(rendererFeature as HeightFogRenderFeature).fogSetting.fowSystem.isRevealRect = true;
				}
			}
		}
		for (int i = 1; i <= kFogTileTotalCount; i++)
		{
			int num = (i - 1) / kFogTileCount.x;
			int num2 = (i - 1) % kFogTileCount.x;
			Vector3 position = new Vector3(((float)num2 + 0.5f) * kFogTileSize, 0f, ((float)num + 0.5f) * kFogTileSize);
			if (fogData.Get(i))
			{
				new FOWRevealer().Init(position, new Vector2(kFogTileSize * 0.5f, kFogTileSize * 0.5f));
			}
		}
		fowSystem.InitAllBuffer();
		fowSystem.RegisterCompleteAction(fogCallBack);
	}

	public void UnlockFogOfWar(int fogIndex)
	{
		int num = (fogIndex - 1) / kFogTileCount.x;
		int num2 = (fogIndex - 1) % kFogTileCount.x;
		Vector3 position = new Vector3(((float)num2 + 0.5f) * kFogTileSize, 0f, ((float)num + 0.5f) * kFogTileSize);
		new FOWRevealer().Init(position, new Vector2(kFogTileSize * 0.5f, kFogTileSize * 0.5f));
	}

	public void UnlockFogOfWar2x2(int unlockIndex)
	{
		int num = (unlockIndex - 1) / kFogTile2x2Count.x;
		int num2 = (unlockIndex - 1) % kFogTile2x2Count.x;
		Vector3 position = new Vector3(((float)num2 + 0.5f) * kFogTile2x2Size, 0f, ((float)num + 0.5f) * kFogTile2x2Size);
		new FOWRevealer().Init(position, new Vector2(kFogTile2x2Size * 0.5f, kFogTile2x2Size * 0.5f));
	}

	public void SetFogVisible(bool visible)
	{
	}

	public void RegisterFogCompleteAction(Action callback)
	{
		fogCallBack = callback;
		if (fowSystem != null)
		{
			fowSystem.RegisterCompleteAction(callback);
		}
	}

	public void ReInitObject()
	{
		if (ModelManager != null)
		{
			ModelManager.ReInitObject();
		}
	}

	public void ClearReInitObject()
	{
		if (ModelManager != null)
		{
			ModelManager.ClearReInitObject();
		}
	}

	public CityTroop GetCityTroop()
	{
		if (ModelManager != null)
		{
			return ModelManager.GetCityTroop();
		}
		return null;
	}

	public long GetFormationUuid()
	{
		if (ModelManager != null)
		{
			return ModelManager.GetFormationUuid();
		}
		return 0L;
	}

	public void LoadCityTroop(int createPos, int targetPos = 0)
	{
		if (ModelManager != null)
		{
			ModelManager.LoadCityTroop(createPos, targetPos);
		}
	}

	public void DestroyCityTroop()
	{
		if (ModelManager != null)
		{
			ModelManager.DestroyCityTroop();
		}
	}

	public Dictionary<string, LodConfig> GetLodConfigs(int lodType)
	{
		return LodManager.GetLodConfigs(lodType);
	}

	public CitySpaceMan CreateCitySpaceMan()
	{
		if (ModelManager != null)
		{
			return ModelManager.CreateCitySpaceMan();
		}
		return null;
	}

	public void DestroyCitySpaceMan()
	{
		if (ModelManager != null)
		{
			ModelManager.DestroyCitySpaceMan();
		}
	}

	public void AddLodAdjuster(AutoAdjustLod adjuster)
	{
		LodManager?.AddLodAdjuster(adjuster);
	}

	public void RemoveLodAdjuster(AutoAdjustLod adjuster)
	{
		LodManager?.RemoveLodAdjuster(adjuster);
	}

	public void SetVisibleByPointType(int pointType, bool isVisible)
	{
		if (ModelManager != null)
		{
			ModelManager.SetVisibleByPointType(pointType, isVisible);
		}
	}

	public Dictionary<int, int> GetSpecialPointDic()
	{
		return new Dictionary<int, int>();
	}

	private void ToggleBorderCollider()
	{
		borderColliderVisible = !borderColliderVisible;
		Transform transform = dynamicObjNode.Find("BorderRoot");
		if (transform == null)
		{
			Debug.LogError("could not find BorderRoot!");
			return;
		}
		MeshRenderer[] componentsInChildren = transform.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = borderColliderVisible;
		}
	}

	private void ToggleGrid()
	{
		gridVisible = !gridVisible;
		if (cityGrid == null)
		{
			Debug.LogError("could not find Grid Node!");
		}
		else
		{
			cityGrid.gameObject.SetActive(gridVisible);
		}
	}

	public void SetFocusPoint(int point)
	{
	}

	private void ToggleCheat()
	{
		GameEntry.Lua.Call("CSharpCallLuaInterface.ToggleCityCheat");
	}

	private void ToggleFog()
	{
	}

	private void InitFogParam(float height, float posY, Color color)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
		if (!(universalRenderPipelineAsset != null))
		{
			return;
		}
		ScriptableRendererData[] obj = (ScriptableRendererData[])universalRenderPipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(universalRenderPipelineAsset);
		ScriptableRendererData scriptableRendererData = ((obj != null) ? obj[0] : null);
		foreach (ScriptableRendererFeature rendererFeature in scriptableRendererData.rendererFeatures)
		{
			if (rendererFeature is HeightFogRenderFeature)
			{
				HeightFogRenderFeature obj2 = rendererFeature as HeightFogRenderFeature;
				obj2.fogSetting._FogDisappearHeight = height;
				obj2.fogSetting._FogPosY = posY;
				obj2.fogSetting.unexploredColor = color;
			}
		}
		scriptableRendererData.SetDirty();
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

	public void SetCameraRotRange(bool overrideVal, Vector2 range)
	{
		Camera.SetRotRangeOverride(overrideVal, range);
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

	public float GetPreviousLodDistance()
	{
		return prevLodDist;
	}

	public void DrawBuildGrid(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count)
	{
		Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices, count);
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

	public int GetZoneIdByPosId(int pointId)
	{
		return 0;
	}

	public int GetZoneIdByWorldPos(Vector3 worldPos)
	{
		return 0;
	}

	private void SeasonWeatherInfoUpdate(object obj)
	{
		if (Camera == null)
		{
			return;
		}
		SeasonWeatherType seasonWeatherType = (SeasonWeatherType)GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetWeatherType");
		if (seasonWeatherType == SeasonWeatherType.Unknown)
		{
			CloseWeatherRenderer();
			return;
		}
		if (mCityWeatherRenderer == null)
		{
			mCityWeatherRenderer = ScriptableObject.CreateInstance<CityWeatherRenderer>();
		}
		Color color = GameEntry.Lua.CallWithReturn<Color, int>("CSharpCallLuaInterface.GetWeatherCityColor", (int)seasonWeatherType);
		Color fogColor = GameEntry.Lua.CallWithReturn<Color, int>("CSharpCallLuaInterface.GetWeatherCityFogColor", (int)seasonWeatherType);
		if (color.Equals(Color.clear) || fogColor.Equals(Color.clear))
		{
			CloseWeatherRenderer();
		}
		else
		{
			mCityWeatherRenderer.AttachToRenderer(Camera.camera, color, fogColor);
		}
	}

	private void CloseWeatherRenderer()
	{
		mCityWeatherRenderer?.DetachFromRenderer(Camera.camera);
	}

	public void OnBloodyNightActivityRefresh(object serverId)
	{
		if (serverId is long)
		{
			int num = serverId.ToInt();
			int curServerId = GameEntry.Data.Player.GetCurServerId();
			if (curServerId != num)
			{
				return;
			}
			if (IsDawn(curServerId))
			{
				CloseS4NightColorRenderer();
				return;
			}
			Color currentEvColorInDarknessSeason = LightDataManager.GetInstance().GetCurrentEvColorInDarknessSeason();
			Color currentFogColorInDarknessSeason = LightDataManager.GetInstance().GetCurrentFogColorInDarknessSeason();
			if (mCityNightRenderer != null)
			{
				mCityNightRenderer.SetNightColor(currentEvColorInDarknessSeason, currentFogColorInDarknessSeason);
			}
		}
		else
		{
			Log.Error("OnBloodyNightActivityRefresh Invalid serverId");
		}
	}

	public bool IsDawn(int targetServerId)
	{
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsDawn", targetServerId);
	}

	public void OnBatteryPowerResourceUpdated(object obj)
	{
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (IsDawn(curServerId))
		{
			CloseS4NightColorRenderer();
			return;
		}
		Color currentEvColorInDarknessSeason = LightDataManager.GetInstance().GetCurrentEvColorInDarknessSeason();
		Color currentFogColorInDarknessSeason = LightDataManager.GetInstance().GetCurrentFogColorInDarknessSeason();
		if (mCityNightRenderer != null)
		{
			mCityNightRenderer.SetNightColor(currentEvColorInDarknessSeason, currentFogColorInDarknessSeason);
		}
	}

	public void OnPveBattleExit(object val)
	{
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (IsDawn(curServerId))
		{
			CloseS4NightColorRenderer();
			return;
		}
		Color currentEvColorInDarknessSeason = LightDataManager.GetInstance().GetCurrentEvColorInDarknessSeason();
		Color currentFogColorInDarknessSeason = LightDataManager.GetInstance().GetCurrentFogColorInDarknessSeason();
		if (mCityNightRenderer != null)
		{
			mCityNightRenderer.SetNightColor(currentEvColorInDarknessSeason, currentFogColorInDarknessSeason);
		}
	}

	public void OnPveBattleEnter(object val)
	{
		CloseS4NightColorRenderer();
	}

	public bool IsPointInAllianceCity(int pointId, int serverId)
	{
		return false;
	}

	public bool IsPointInBlackArea(int pointId)
	{
		return false;
	}

	public void ShowBlackArea(int point, int tileWidth, int tileHeight)
	{
	}

	public void HideBlackArea(int point, int tileWidth, int tileHeight)
	{
	}

	public WorldZoneData GetZoneData(int zoneId)
	{
		return null;
	}

	public void UpdateBattleSoundData()
	{
	}

	public string Description(string name, object[] args)
	{
		return "I am the king of the world.";
	}

	public SeasonType GetCurSeasonType()
	{
		return SeasonType.Unknown;
	}
}
