using Tayx.Graphy;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ProfilerGFXPanel : BaseGFXPanel
{
	private InstanceRequest graphyRequest;

	private readonly GameObject _uiRoot;

	private readonly GameObject _dynamic;

	private readonly WorldScene _worldScene;

	private bool fogVisible = true;

	public ProfilerGFXPanel()
		: base("Profiler")
	{
		_uiRoot = GameObject.Find("UIContainer");
		_dynamic = GameObject.Find("World/dynamicObj");
		_worldScene = Object.FindObjectOfType<WorldScene>();
	}

	public override void DrawGUI()
	{
		if (graphyRequest == null)
		{
			if (GUILayout.Button("显示 Graphy"))
			{
				graphyRequest = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Debug/Graphy.prefab");
				graphyRequest.completed += delegate
				{
					graphyRequest.gameObject.GetComponent<GraphyManager>().SetLuaMemoryGetter(() => GameEntry.Lua.Env.Memroy * 1024);
				};
			}
		}
		else if (GUILayout.Button("隐藏 Graphy"))
		{
			graphyRequest.Destroy();
			graphyRequest = null;
		}
		if (LuaClientProfiler.IsAttached)
		{
			if (GUILayout.Button("Detach Lua Profiler"))
			{
				LuaClientProfiler.Detach();
			}
		}
		else if (GUILayout.Button("Attach Lua Profiler"))
		{
			LuaClientProfiler.Attach();
		}
		if (GameEntry.Resource.Loggable)
		{
			if (GUILayout.Button("关闭资源日志"))
			{
				GameEntry.Resource.Loggable = false;
				GameEntry.Setting.SetBool("Setting.Resource.Logger", value: false);
			}
		}
		else if (GUILayout.Button("打开资源日志"))
		{
			GameEntry.Resource.Loggable = true;
			GameEntry.Setting.SetBool("Setting.Resource.Logger", value: true);
		}
		GUILayout.Space(30f);
		GUILayout.Label("SRPBatch: " + GraphicsSettings.useScriptableRenderPipelineBatching);
		if (GUILayout.Button("开关SPRBatch"))
		{
			UniversalRenderPipelineAsset obj = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
			obj.useSRPBatcher = !obj.useSRPBatcher;
			GraphicsSettings.useScriptableRenderPipelineBatching = obj.useSRPBatcher;
		}
		GUILayout.Space(30f);
		if (GUILayout.Button("Toggle 迷雾"))
		{
			fogVisible = !fogVisible;
			SceneManager.World.SetFogVisible(fogVisible);
		}
		GUILayout.Space(30f);
		if (GUILayout.Button("Toggle 地表") && (bool)_worldScene)
		{
			_worldScene.ProfileToggleTerrain();
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("Toggle 建筑"))
		{
			_dynamic.SetActive(!_dynamic.activeSelf);
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("Toggle UI"))
		{
			_uiRoot.SetActive(!_uiRoot.activeSelf);
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("Toggle Glass") && (bool)_worldScene)
		{
			_worldScene.ProfileToggleGlass();
		}
	}
}
