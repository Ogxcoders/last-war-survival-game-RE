using System;
using System.Collections.Generic;
using GameKit.Base;
using UnityEngine;

public class EditorSceneBuilding : WorldScene
{
	private static EditorSceneBuilding _instance;

	public static EditorSceneBuilding Instance => _instance;

	public EditorPointManager EditorPoint => base.PointManager as EditorPointManager;

	public EditorModelManager ModelManager { get; protected set; }

	public static void Init()
	{
		UnityEngine.Object.FindObjectOfType<ShowFPSText>().gameObject.SetActive(value: false);
		EditorMockData.InitPlayerInfo();
		SceneManager.ToggleBlendDepth(b: true);
		SceneManager.ResetNightColor();
		SceneManager.ResetRollStrengthZ();
		SceneManager.ResetSceneShadow();
		_instance = new GameObject("World").AddComponent<EditorSceneBuilding>();
		SceneManager.SetupSceneInEditor(_instance, SceneManager.SceneID.World);
		ProfilerService.Initialize();
		SingletonBehaviour<ProfilerService>.Instance.SRPProfiler.HideStats();
		DebuggerService.Initialize();
		SingletonBehaviour<DebuggerService>.Instance.Debugger.RegisterDebuggerWindow("Building Editor", new EditorBuildingDebuggerWindow());
	}

	public void FocusWorldIndex(int index)
	{
		Vector3 lookat = Instance.TileIndexToWorld(index);
		base.Camera.AutoFocus(lookat, LookAtFocusState.MoveCity, 0.2f, focusToCenter: true, lockView: false, delegate
		{
			Instance.Camera.QuitFocus(0f);
		});
	}

	private void Awake()
	{
		Init(((Component)this).gameObject);
		gameObject.transform.localScale = Vector3.one;
		dynamicObjNode = new GameObject("dynamicObj").transform;
		dynamicObjNode.SetParent(gameObject.transform, worldPositionStays: false);
		buildBubbleNode = new GameObject("buildBubble").transform;
		buildBubbleNode.SetParent(gameObject.transform, worldPositionStays: false);
		InitSubModules();
		LoadTerrainAssets(null);
	}

	private new void Update()
	{
		UpdateSubModules();
	}

	private void OnDestroy()
	{
		ClearSubModules();
	}

	public override PlayerType IsMyEnemy(int serverId, string allianceId)
	{
		return PlayerType.PlayerAlliance;
	}

	public override List<WorldMarch> GetOwnerMarches(string ownerUid, string allianceUid = "")
	{
		return new List<WorldMarch>();
	}

	private T AddSubModule<T>() where T : WorldManagerBase
	{
		T val = (T)Activator.CreateInstance(typeof(T), new object[1] { this });
		subModules.Add(val);
		return val;
	}

	private void InitSubModules()
	{
		base.Camera = AddSubModule<WorldCamera>();
		base.InputManager = AddSubModule<EditorInputManager>();
		base.StaticManager = AddSubModule<WorldStaticManager>();
		base.PointManager = AddSubModule<EditorPointManager>();
		LodManager = AddSubModule<WorldLodManager>();
		ModelManager = AddSubModule<EditorModelManager>();
		base.WorldCollectAnimalManager = AddSubModule<WorldCollectAnimalManager>();
		base.IconRendererFacade = AddSubModule<WorldIconRendererFacade>();
		foreach (WorldManagerBase subModule in subModules)
		{
			subModule.Init();
		}
	}

	private void ClearSubModules()
	{
	}

	private void UpdateSubModules()
	{
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
	}
}
