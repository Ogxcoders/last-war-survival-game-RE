using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public static class SceneManager
{
	public enum SceneID
	{
		None,
		City,
		World,
		PVE,
		Custom
	}

	public static bool DISABLE_UNIFORM_EVENT_DISPATCH = false;

	private static ScriptableRendererData _scriptableRendererData;

	private static readonly int _NightCol = Shader.PropertyToID("_NightColor");

	private static readonly int _RollStrengthZ = Shader.PropertyToID("_RollStrengthZ");

	private static readonly int _PlayerPosAndRollStrengthX = Shader.PropertyToID("_PlayerPosAndRollStrengthX");

	private static readonly int _PVEShadowColorId = Shader.PropertyToID("_ShadowColorPVE");

	private static readonly int _PVEShadowFalloffID = Shader.PropertyToID("_ShadowFalloffPVE");

	private static readonly int _PVEShadowSwitchID = Shader.PropertyToID("_SwitchPVEShadow");

	private static SceneID currSceneID = SceneID.None;

	private static string _curSceneSubType = "default";

	private static SceneInterface world;

	private static WorldMarchDataManager marchDataMgr;

	private static GameObject worldObj;

	private static WorldTerrainAssetHandler worldTerrainAssetHandler;

	public static SceneInterface World => world;

	public static bool IsEditorMode { get; private set; }

	public static WorldMarchDataManager MarchDataMgr
	{
		get
		{
			if (marchDataMgr == null)
			{
				if (world != null && world is WorldScene scene)
				{
					marchDataMgr = new WorldMarchDataManager(scene);
				}
				else
				{
					marchDataMgr = new WorldMarchDataManager(null);
				}
				marchDataMgr.Init();
			}
			return marchDataMgr;
		}
	}

	public static int CurrSceneID
	{
		get
		{
			return (int)currSceneID;
		}
		set
		{
			currSceneID = (SceneID)value;
			_curSceneSubType = "default";
		}
	}

	public static string CurrentSceneSubType
	{
		get
		{
			return _curSceneSubType;
		}
		set
		{
			_curSceneSubType = value;
		}
	}

	public static WorldTerrainAssetHandler WorldTerrainAssetHandler
	{
		get
		{
			if (worldTerrainAssetHandler == null)
			{
				worldTerrainAssetHandler = new WorldTerrainAssetHandler();
				worldTerrainAssetHandler.Init();
			}
			return worldTerrainAssetHandler;
		}
	}

	public static void ToggleBlendDepth(bool b)
	{
		if (_scriptableRendererData == null)
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
			if (universalRenderPipelineAsset != null)
			{
				ScriptableRendererData[] obj = (ScriptableRendererData[])universalRenderPipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(universalRenderPipelineAsset);
				_scriptableRendererData = ((obj != null) ? obj[0] : null);
			}
		}
		if (!(_scriptableRendererData != null))
		{
			return;
		}
		foreach (ScriptableRendererFeature rendererFeature in _scriptableRendererData.rendererFeatures)
		{
			if (rendererFeature.name.Equals("Blend"))
			{
				((RenderObjects)rendererFeature).settings.overrideDepthState = b;
			}
		}
		_scriptableRendererData.SetDirty();
	}

	public static void SetupSceneInEditor(SceneInterface scene, SceneID sceneID)
	{
		IsEditorMode = true;
		currSceneID = sceneID;
		world = scene;
	}

	public static void CreateWorld()
	{
		worldObj = new GameObject("World", typeof(WorldScene));
		world = worldObj.GetComponent<WorldScene>();
		world.Init(worldObj);
		currSceneID = SceneID.World;
		_curSceneSubType = "default";
		ToggleBlendDepth(b: true);
		GameEntry.Lua.Call("CSharpCallLuaInterface.ChangeScene", param1: true);
		marchDataMgr.SetWorldScene(world as WorldScene);
	}

	public static void CreateCity()
	{
		worldObj = new GameObject("City", typeof(CityScene));
		world = worldObj.GetComponent<CityScene>();
		world.Init(worldObj);
		currSceneID = SceneID.City;
		_curSceneSubType = "default";
		ToggleBlendDepth(b: false);
		GameEntry.Lua.Call("CSharpCallLuaInterface.ChangeScene", param1: false);
		marchDataMgr.SetWorldScene(null);
	}

	public static void ChangeScene(SceneID sceneId)
	{
		if (currSceneID != sceneId)
		{
			DestroyCurScene();
			switch (sceneId)
			{
			case SceneID.City:
				CreateCity();
				break;
			case SceneID.World:
				CreateWorld();
				break;
			}
		}
	}

	public static void DestroyCurScene()
	{
		if (currSceneID != SceneID.None)
		{
			if (worldObj != null)
			{
				world.Uninit();
				world = null;
				Object.Destroy(worldObj);
				worldObj = null;
			}
			if (WorldTerrainAssetHandler != null)
			{
				WorldTerrainAssetHandler.Release();
			}
			currSceneID = SceneID.None;
			_curSceneSubType = "default";
			GameEntry.Resource.UnloadUnusedAssetsSceneChange();
		}
	}

	public static void Destroy()
	{
		DestroyCurScene();
		if (marchDataMgr != null)
		{
			marchDataMgr.UnInit();
			marchDataMgr = null;
		}
	}

	public static void DestroyScene(MonoBehaviour obj)
	{
		if (obj is SceneInterface sceneInterface)
		{
			sceneInterface.Uninit();
			Object.Destroy(obj.gameObject);
		}
		GameEntry.Resource.UnloadUnusedAssetsSceneChange();
	}

	public static void Update()
	{
		marchDataMgr?.OnUpdate(Time.deltaTime);
		WorldTerrainAssetHandler?.Update();
		if (CommonUtils.IsDebug())
		{
			DebugClick();
		}
	}

	public static void DebugClick()
	{
		if (!GMSwitch.DebugClickLogWarning && !GMSwitch.FocusMyClick)
		{
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			HandleClick(Input.mousePosition);
			return;
		}
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (touch.phase == TouchPhase.Began)
			{
				HandleClick(touch.position);
			}
		}
	}

	private static void HandleClick(Vector3 position)
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current)
		{
			position = Input.mousePosition
		};
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		RaycastHit hitInfo;
		if (list.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				if (i == 0)
				{
					stringBuilder.Append("<color=yellow>点击到的UI：" + list[0].gameObject.name + "</color>，盖住的UI：");
				}
				else
				{
					stringBuilder.Append(list[i].gameObject.name + "|");
				}
			}
			if (GMSwitch.DebugClickLogWarning)
			{
				Log.Warning(stringBuilder.ToString());
			}
		}
		else if (Camera.main != null && Physics.Raycast(Camera.main.ScreenPointToRay(position), out hitInfo) && GMSwitch.DebugClickLogWarning)
		{
			Log.Warning("<color=yellow>点击到的GameObject: " + hitInfo.collider.gameObject.name + "</color>");
		}
	}

	public static void LateUpdate()
	{
		DynamicAtlasManager.Instance.Upload();
		GPUSkinnedMeshRenderer.UpdateAllActiveGpuSkinRenderer();
	}

	public static void FixedUpdate()
	{
	}

	public static bool IsInWorld()
	{
		return currSceneID == SceneID.World;
	}

	public static bool IsInCity()
	{
		return currSceneID == SceneID.City;
	}

	public static bool IsInPVE()
	{
		return currSceneID == SceneID.PVE;
	}

	public static bool IsSceneNone()
	{
		return currSceneID == SceneID.None;
	}

	public static bool IsSceneBuildFninsh()
	{
		if (IsInWorld())
		{
			return World.IsBuildFinish();
		}
		return true;
	}

	public static int GetCurZoneId()
	{
		if (IsInWorld())
		{
			return world.GetZoneIdByWorldPos(world.CurTarget);
		}
		return 0;
	}

	public static void SetNightColor(float r, float g, float b, float intensity)
	{
		Shader.SetGlobalColor(value: new Color(Mathf.Pow(r / 255f, 2.2f) * intensity, Mathf.Pow(g / 255f, 2.2f) * intensity, Mathf.Pow(b / 255f, 2.2f) * intensity), nameID: _NightCol);
	}

	public static void ResetNightColor()
	{
		Shader.SetGlobalColor(_NightCol, Color.white);
	}

	public static void SetRollStrengthZ(float z)
	{
		Shader.SetGlobalFloat(_RollStrengthZ, z * 0.001f);
	}

	public static void SetRollStrengthX(float posX, float posZ, float x)
	{
		Shader.SetGlobalVector(_PlayerPosAndRollStrengthX, new Vector4(posX, 0f, posZ, x * 0.001f));
	}

	public static void ResetRollStrengthZ()
	{
		Shader.SetGlobalFloat(_RollStrengthZ, 0f);
		Shader.SetGlobalVector(_PlayerPosAndRollStrengthX, new Vector4(0f, 0f, 0f, 0f));
	}

	public static bool SetSceneShadow(Color shadowColor, float shadowFalloff)
	{
		Shader.SetGlobalColor(_PVEShadowColorId, shadowColor);
		Shader.SetGlobalFloat(_PVEShadowFalloffID, shadowFalloff);
		Shader.SetGlobalFloat(_PVEShadowSwitchID, 1f);
		return true;
	}

	public static void ResetSceneShadow()
	{
		Shader.SetGlobalFloat(_PVEShadowSwitchID, 0f);
	}

	public static void EditorFocusGameObject(GameObject go)
	{
	}
}
