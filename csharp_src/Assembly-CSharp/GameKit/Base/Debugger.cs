using System.Collections.Generic;
using UnityEngine;

namespace GameKit.Base;

public class Debugger : MonoBehaviour
{
	private readonly DebuggerWindow.ConsoleWindow _consoleWindow = new DebuggerWindow.ConsoleWindow();

	private readonly DebuggerWindow.SystemInformationWindow _systemInformationWindow = new DebuggerWindow.SystemInformationWindow();

	private readonly DebuggerWindow.EnvironmentInformationWindow _environmentInformationWindow = new DebuggerWindow.EnvironmentInformationWindow();

	private readonly DebuggerWindow.ScreenInformationWindow _screenInformationWindow = new DebuggerWindow.ScreenInformationWindow();

	private readonly DebuggerWindow.GraphicsInformationWindow _graphicsInformationWindow = new DebuggerWindow.GraphicsInformationWindow();

	private readonly DebuggerWindow.InputSummaryInformationWindow _inputSummaryInformationWindow = new DebuggerWindow.InputSummaryInformationWindow();

	private readonly DebuggerWindow.InputTouchInformationWindow _inputTouchInformationWindow = new DebuggerWindow.InputTouchInformationWindow();

	private readonly DebuggerWindow.InputAccelerationInformationWindow _inputAccelerationInformationWindow = new DebuggerWindow.InputAccelerationInformationWindow();

	private readonly DebuggerWindow.InputGyroscopeInformationWindow _inputGyroscopeInformationWindow = new DebuggerWindow.InputGyroscopeInformationWindow();

	private readonly DebuggerWindow.InputCompassInformationWindow _inputCompassInformationWindow = new DebuggerWindow.InputCompassInformationWindow();

	private readonly DebuggerWindow.PathInformationWindow _pathInformationWindow = new DebuggerWindow.PathInformationWindow();

	private readonly DebuggerWindow.SceneInformationWindow _sceneInformationWindow = new DebuggerWindow.SceneInformationWindow();

	private readonly DebuggerWindow.TimeInformationWindow _timeInformationWindow = new DebuggerWindow.TimeInformationWindow();

	private readonly DebuggerWindow.QualityInformationWindow _qualityInformationWindow = new DebuggerWindow.QualityInformationWindow();

	private readonly DebuggerWindow.ProfilerInformationWindow _profilerInformationWindow = new DebuggerWindow.ProfilerInformationWindow();

	private readonly DebuggerWindow.WebPlayerInformationWindow _webPlayerInformationWindow = new DebuggerWindow.WebPlayerInformationWindow();

	private readonly DebuggerWindow.RuntimeMemorySummaryWindow _runtimeMemorySummaryWindow = new DebuggerWindow.RuntimeMemorySummaryWindow();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<Object> _runtimeMemoryAllInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<Object>();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<Texture> _runtimeMemoryTextureInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<Texture>();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<Mesh> _runtimeMemoryMeshInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<Mesh>();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<Material> _runtimeMemoryMaterialInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<Material>();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<Shader> _runtimeMemoryShaderInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<Shader>();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<AnimationClip> _runtimeMemoryAnimationClipInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<AnimationClip>();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<AudioClip> _runtimeMemoryAudioClipInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<AudioClip>();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<Font> _runtimeMemoryFontInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<Font>();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<TextAsset> _runtimeMemoryTextAssetInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<TextAsset>();

	private readonly DebuggerWindow.RuntimeMemoryInformationWindow<ScriptableObject> _runtimeMemoryScriptableObjectInformationWindow = new DebuggerWindow.RuntimeMemoryInformationWindow<ScriptableObject>();

	private readonly DebuggerWindow.NetworkInformationWindow _networkInformationWindow = new DebuggerWindow.NetworkInformationWindow();

	private readonly DebuggerWindow.SettingsWindow _settingsWindow = new DebuggerWindow.SettingsWindow();

	private readonly DebuggerWindow.OperationsWindow _operationsWindow = new DebuggerWindow.OperationsWindow();

	public DebuggerWindow DebuggerWindow { get; private set; }

	public DebuggerWindow.DebuggerWindowGroup DebuggerWindowRoot { get; private set; }

	public DebuggerWindow.ConsoleWindow ConsoleWindow => _consoleWindow;

	private void Awake()
	{
		DebuggerWindowRoot = new DebuggerWindow.DebuggerWindowGroup();
	}

	private void OnDestroy()
	{
		DebuggerWindowRoot.Shutdown();
	}

	public void Startup()
	{
		DebuggerWindow = new DebuggerWindow();
		RegisterDebuggerWindow("Console", _consoleWindow);
		RegisterDebuggerWindow("Information/System", _systemInformationWindow);
		RegisterDebuggerWindow("Information/Environment", _environmentInformationWindow);
		RegisterDebuggerWindow("Information/Screen", _screenInformationWindow);
		RegisterDebuggerWindow("Information/Graphics", _graphicsInformationWindow);
		RegisterDebuggerWindow("Information/Input/Summary", _inputSummaryInformationWindow);
		RegisterDebuggerWindow("Information/Input/Touch", _inputTouchInformationWindow);
		RegisterDebuggerWindow("Information/Input/Acceleration", _inputAccelerationInformationWindow);
		RegisterDebuggerWindow("Information/Input/Gyroscope", _inputGyroscopeInformationWindow);
		RegisterDebuggerWindow("Information/Input/Compass", _inputCompassInformationWindow);
		RegisterDebuggerWindow("Information/Other/Scene", _sceneInformationWindow);
		RegisterDebuggerWindow("Information/Other/Path", _pathInformationWindow);
		RegisterDebuggerWindow("Information/Other/Time", _timeInformationWindow);
		RegisterDebuggerWindow("Information/Other/Quality", _qualityInformationWindow);
		RegisterDebuggerWindow("Information/Other/Web Player", _webPlayerInformationWindow);
		RegisterDebuggerWindow("Profiler/Summary", _profilerInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/Summary", _runtimeMemorySummaryWindow);
		RegisterDebuggerWindow("Profiler/Memory/All", _runtimeMemoryAllInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/Texture", _runtimeMemoryTextureInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/Mesh", _runtimeMemoryMeshInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/Material", _runtimeMemoryMaterialInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/Shader", _runtimeMemoryShaderInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/AnimationClip", _runtimeMemoryAnimationClipInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/AudioClip", _runtimeMemoryAudioClipInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/Font", _runtimeMemoryFontInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/TextAsset", _runtimeMemoryTextAssetInformationWindow);
		RegisterDebuggerWindow("Profiler/Memory/ScriptableObject", _runtimeMemoryScriptableObjectInformationWindow);
		RegisterDebuggerWindow("Profiler/Network", _networkInformationWindow);
		RegisterDebuggerWindow("Other/Settings", _settingsWindow);
		RegisterDebuggerWindow("Other/Operations", _operationsWindow);
		DebuggerWindow.Start();
	}

	public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow)
	{
		DebuggerWindowRoot.RegisterDebuggerWindow(path, debuggerWindow);
		debuggerWindow.Initialize();
	}

	public bool UnregisterDebuggerWindow(string path)
	{
		return DebuggerWindowRoot.UnregisterDebuggerWindow(path);
	}

	public IDebuggerWindow GetDebuggerWindow(string path)
	{
		return DebuggerWindowRoot.GetDebuggerWindow(path);
	}

	public bool SelectDebuggerWindow(string path)
	{
		return DebuggerWindowRoot.SelectDebuggerWindow(path);
	}

	public void GetRecentLogs(List<DebuggerWindow.LogNode> results)
	{
		_consoleWindow.GetRecentLogs(results);
	}

	public void GetRecentLogs(List<DebuggerWindow.LogNode> results, int count)
	{
		_consoleWindow.GetRecentLogs(results, count);
	}

	private void Update()
	{
		DebuggerWindow?.Update();
	}

	private void OnGUI()
	{
		DebuggerWindow?.OnGUI();
	}
}
