using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace GameKit.Base;

public sealed class DebuggerWindow
{
	public sealed class ConsoleWindow : IDebuggerWindow
	{
		private readonly Queue<LogNode> _logNodes = new Queue<LogNode>();

		private Vector2 _logScrollPosition = Vector2.zero;

		private Vector2 _stackScrollPosition = Vector2.zero;

		private LogNode _selectedNode;

		private bool _lastLockScroll = true;

		private bool _lastInfoFilter = true;

		private bool _lastWarningFilter = true;

		private bool _lastErrorFilter = true;

		private bool _LastExceptionFilter = true;

		public bool LockScroll { get; set; } = true;

		public int MaxLine { get; set; } = 100;

		public bool InfoFilter { get; set; } = true;

		public bool WarningFilter { get; set; } = true;

		public bool ErrorFilter { get; set; } = true;

		public bool ExceptionFilter { get; set; } = true;

		public int InfoCount { get; private set; }

		public int WarningCount { get; private set; }

		public int ErrorCount { get; private set; }

		public int ExceptionCount { get; private set; }

		public Color32 InfoColor { get; set; } = Color.white;

		public Color32 WarningColor { get; set; } = Color.yellow;

		public Color32 ErrorColor { get; set; } = Color.red;

		public Color32 ExceptionColor { get; set; } = new Color(0.7f, 0.2f, 0.2f);

		public void Initialize(params object[] args)
		{
			Application.logMessageReceived += OnLogMessageReceived;
		}

		public void Shutdown()
		{
			Application.logMessageReceived -= OnLogMessageReceived;
			Clear();
		}

		public void OnEnter()
		{
		}

		public void OnLeave()
		{
		}

		public void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			if (_lastLockScroll != LockScroll)
			{
				_lastLockScroll = LockScroll;
			}
			if (_lastInfoFilter != InfoFilter)
			{
				_lastInfoFilter = InfoFilter;
			}
			if (_lastWarningFilter != WarningFilter)
			{
				_lastWarningFilter = WarningFilter;
			}
			if (_lastErrorFilter != ErrorFilter)
			{
				_lastErrorFilter = ErrorFilter;
			}
			if (_LastExceptionFilter != ExceptionFilter)
			{
				_LastExceptionFilter = ExceptionFilter;
			}
		}

		public void OnDraw()
		{
			RefreshCount();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Clear All", GUILayout.Width(100f)))
			{
				Clear();
			}
			LockScroll = GUILayout.Toggle(LockScroll, "Lock Scroll", GUILayout.Width(90f));
			GUILayout.FlexibleSpace();
			InfoFilter = GUILayout.Toggle(InfoFilter, $"Info ({InfoCount.ToString()})", GUILayout.Width(90f));
			WarningFilter = GUILayout.Toggle(WarningFilter, $"Warning ({WarningCount.ToString()})", GUILayout.Width(90f));
			ErrorFilter = GUILayout.Toggle(ErrorFilter, $"Error ({ErrorCount.ToString()})", GUILayout.Width(90f));
			ExceptionFilter = GUILayout.Toggle(ExceptionFilter, $"Exception ({ExceptionCount.ToString()})", GUILayout.Width(90f));
			GUILayout.EndHorizontal();
			GUILayout.BeginVertical("box");
			if (LockScroll)
			{
				_logScrollPosition.y = float.MaxValue;
			}
			_logScrollPosition = GUILayout.BeginScrollView(_logScrollPosition);
			bool flag = false;
			foreach (LogNode logNode in _logNodes)
			{
				if (logNode == null)
				{
					continue;
				}
				switch (logNode.LogType)
				{
				case LogType.Log:
					if (!InfoFilter)
					{
						continue;
					}
					break;
				case LogType.Warning:
					if (!WarningFilter)
					{
						continue;
					}
					break;
				case LogType.Error:
					if (!ErrorFilter)
					{
						continue;
					}
					break;
				case LogType.Exception:
					if (!ExceptionFilter)
					{
						continue;
					}
					break;
				}
				if (GUILayout.Toggle(_selectedNode == logNode, GetLogString(logNode)))
				{
					flag = true;
					if (_selectedNode != logNode)
					{
						_selectedNode = logNode;
						_stackScrollPosition = Vector2.zero;
					}
				}
			}
			if (!flag)
			{
				_selectedNode = null;
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			_stackScrollPosition = GUILayout.BeginScrollView(_stackScrollPosition, GUILayout.Height(100f));
			if (_selectedNode != null)
			{
				Color32 logStringColor = GetLogStringColor(_selectedNode.LogType);
				if (GUILayout.Button(string.Format("<color=#{0}{1}{2}{3}><b>{4}</b></color>{6}{6}{5}", logStringColor.r.ToString("x2"), logStringColor.g.ToString("x2"), logStringColor.b.ToString("x2"), logStringColor.a.ToString("x2"), _selectedNode.LogMessage, _selectedNode.StackTrack, Environment.NewLine), "label"))
				{
					CopyToClipboard(string.Format("{0}{2}{2}{1}", _selectedNode.LogMessage, _selectedNode.StackTrack, Environment.NewLine));
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
		}

		private void Clear()
		{
			_logNodes.Clear();
		}

		public void RefreshCount()
		{
			InfoCount = 0;
			WarningCount = 0;
			ErrorCount = 0;
			ExceptionCount = 0;
			using Queue<LogNode>.Enumerator enumerator = _logNodes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				switch (enumerator.Current.LogType)
				{
				case LogType.Log:
					InfoCount++;
					break;
				case LogType.Warning:
					WarningCount++;
					break;
				case LogType.Error:
					ErrorCount++;
					break;
				case LogType.Exception:
					ExceptionCount++;
					break;
				}
			}
		}

		public void GetRecentLogs(List<LogNode> results)
		{
			if (results == null)
			{
				throw new Exception("Results is invalid.");
			}
			results.Clear();
			foreach (LogNode logNode in _logNodes)
			{
				results.Add(logNode);
			}
		}

		public void GetRecentLogs(List<LogNode> results, int count)
		{
			if (results == null)
			{
				throw new Exception("Results is invalid.");
			}
			if (count <= 0)
			{
				throw new Exception("Results is invalid.");
			}
			int num = _logNodes.Count - count;
			if (num < 0)
			{
				num = 0;
			}
			int num2 = 0;
			results.Clear();
			foreach (LogNode logNode in _logNodes)
			{
				if (num2++ >= num)
				{
					results.Add(logNode);
				}
			}
		}

		private void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
		{
			if (logType == LogType.Assert)
			{
				logType = LogType.Error;
			}
			_logNodes.Enqueue(LogNode.Create(logType, logMessage, stackTrace));
			while (_logNodes.Count > MaxLine)
			{
				LogNode.Release(_logNodes.Dequeue());
			}
		}

		private string GetLogString(LogNode logNode)
		{
			Color32 logStringColor = GetLogStringColor(logNode.LogType);
			return string.Format("<color=#{0}{1}{2}{3}>[{4}][{5}] {6}</color>", logStringColor.r.ToString("x2"), logStringColor.g.ToString("x2"), logStringColor.b.ToString("x2"), logStringColor.a.ToString("x2"), logNode.LogTime.ToLocalTime().ToString("HH:mm:ss.fff"), logNode.LogFrameCount.ToString(), logNode.LogMessage);
		}

		internal Color32 GetLogStringColor(LogType logType)
		{
			Color32 result = Color.white;
			switch (logType)
			{
			case LogType.Log:
				result = InfoColor;
				break;
			case LogType.Warning:
				result = WarningColor;
				break;
			case LogType.Error:
				result = ErrorColor;
				break;
			case LogType.Exception:
				result = ExceptionColor;
				break;
			}
			return result;
		}
	}

	public class DebuggerWindowGroup : IDebuggerWindow
	{
		private readonly List<KeyValuePair<string, IDebuggerWindow>> _debuggerWindows;

		private string[] _debuggerWindowNames;

		public int DebuggerWindowCount => _debuggerWindows.Count;

		public int SelectedIndex { get; set; }

		public IDebuggerWindow SelectedWindow
		{
			get
			{
				if (SelectedIndex >= _debuggerWindows.Count)
				{
					return null;
				}
				return _debuggerWindows[SelectedIndex].Value;
			}
		}

		public DebuggerWindowGroup()
		{
			_debuggerWindows = new List<KeyValuePair<string, IDebuggerWindow>>();
			_debuggerWindowNames = null;
			SelectedIndex = 0;
		}

		public void Initialize(params object[] args)
		{
			throw new NotImplementedException();
		}

		public void Shutdown()
		{
			foreach (KeyValuePair<string, IDebuggerWindow> debuggerWindow in _debuggerWindows)
			{
				debuggerWindow.Value.Shutdown();
			}
			_debuggerWindows.Clear();
		}

		public void OnEnter()
		{
			SelectedWindow.OnEnter();
		}

		public void OnLeave()
		{
			SelectedWindow.OnLeave();
		}

		public void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			SelectedWindow.OnUpdate(elapseSeconds, realElapseSeconds);
		}

		public void OnDraw()
		{
		}

		private void RefreshDebuggerWindowNames()
		{
			int num = 0;
			_debuggerWindowNames = new string[_debuggerWindows.Count];
			foreach (KeyValuePair<string, IDebuggerWindow> debuggerWindow in _debuggerWindows)
			{
				_debuggerWindowNames[num++] = debuggerWindow.Key;
			}
		}

		public string[] GetDebuggerWindowNames()
		{
			return _debuggerWindowNames;
		}

		public IDebuggerWindow GetDebuggerWindow(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			int num = path.IndexOf('/');
			if (num < 0 || num >= path.Length - 1)
			{
				return InternalGetDebuggerWindow(path);
			}
			string name = path.Substring(0, num);
			string path2 = path.Substring(num + 1);
			return ((DebuggerWindowGroup)InternalGetDebuggerWindow(name))?.GetDebuggerWindow(path2);
		}

		public bool SelectDebuggerWindow(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			int num = path.IndexOf('/');
			if (num < 0 || num >= path.Length - 1)
			{
				return InternalSelectDebuggerWindow(path);
			}
			string name = path.Substring(0, num);
			string path2 = path.Substring(num + 1);
			DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetDebuggerWindow(name);
			if (debuggerWindowGroup == null || !InternalSelectDebuggerWindow(name))
			{
				return false;
			}
			return debuggerWindowGroup.SelectDebuggerWindow(path2);
		}

		public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new Exception("Path is invalid.");
			}
			int num = path.IndexOf('/');
			if (num < 0 || num >= path.Length - 1)
			{
				if (InternalGetDebuggerWindow(path) != null)
				{
					throw new Exception("Debugger window has been registered.");
				}
				_debuggerWindows.Add(new KeyValuePair<string, IDebuggerWindow>(path, debuggerWindow));
				RefreshDebuggerWindowNames();
				return;
			}
			string text = path.Substring(0, num);
			string path2 = path.Substring(num + 1);
			DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetDebuggerWindow(text);
			if (debuggerWindowGroup == null)
			{
				if (InternalGetDebuggerWindow(text) != null)
				{
					throw new Exception("Debugger window has been registered, can not create debugger window group.");
				}
				debuggerWindowGroup = new DebuggerWindowGroup();
				_debuggerWindows.Add(new KeyValuePair<string, IDebuggerWindow>(text, debuggerWindowGroup));
				RefreshDebuggerWindowNames();
			}
			debuggerWindowGroup.RegisterDebuggerWindow(path2, debuggerWindow);
		}

		public bool UnregisterDebuggerWindow(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			int num = path.IndexOf('/');
			if (num < 0 || num >= path.Length - 1)
			{
				IDebuggerWindow debuggerWindow = InternalGetDebuggerWindow(path);
				bool result = _debuggerWindows.Remove(new KeyValuePair<string, IDebuggerWindow>(path, debuggerWindow));
				debuggerWindow.Shutdown();
				RefreshDebuggerWindowNames();
				return result;
			}
			string name = path.Substring(0, num);
			string path2 = path.Substring(num + 1);
			return ((DebuggerWindowGroup)InternalGetDebuggerWindow(name))?.UnregisterDebuggerWindow(path2) ?? false;
		}

		private IDebuggerWindow InternalGetDebuggerWindow(string name)
		{
			foreach (KeyValuePair<string, IDebuggerWindow> debuggerWindow in _debuggerWindows)
			{
				if (debuggerWindow.Key == name)
				{
					return debuggerWindow.Value;
				}
			}
			return null;
		}

		private bool InternalSelectDebuggerWindow(string name)
		{
			for (int i = 0; i < _debuggerWindows.Count; i++)
			{
				if (_debuggerWindows[i].Key == name)
				{
					SelectedIndex = i;
					return true;
				}
			}
			return false;
		}
	}

	public sealed class EnvironmentInformationWindow : ScrollableDebuggerWindowBase
	{
		public override void Initialize(params object[] args)
		{
		}

		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Environment Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Product Name", Application.productName);
			ScrollableDebuggerWindowBase.DrawItem("Company Name", Application.companyName);
			ScrollableDebuggerWindowBase.DrawItem("Game Identifier", Application.identifier);
			ScrollableDebuggerWindowBase.DrawItem("Application Version", Application.version);
			ScrollableDebuggerWindowBase.DrawItem("Unity Version", Application.unityVersion);
			ScrollableDebuggerWindowBase.DrawItem("Platform", Application.platform.ToString());
			ScrollableDebuggerWindowBase.DrawItem("System Language", Application.systemLanguage.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Cloud Project Id", Application.cloudProjectId);
			ScrollableDebuggerWindowBase.DrawItem("Build Guid", Application.buildGUID);
			ScrollableDebuggerWindowBase.DrawItem("Target Frame Rate", Application.targetFrameRate.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Internet Reachability", Application.internetReachability.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Background Loading Priority", Application.backgroundLoadingPriority.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Is Playing", Application.isPlaying.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Splash Screen Is Finished", SplashScreen.isFinished.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Run In Background", Application.runInBackground.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Install Name", Application.installerName);
			ScrollableDebuggerWindowBase.DrawItem("Install Mode", Application.installMode.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Sandbox Type", Application.sandboxType.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Is Mobile Platform", Application.isMobilePlatform.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Is Console Platform", Application.isConsolePlatform.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Is Editor", Application.isEditor.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Is Debug Build", Debug.isDebugBuild.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Is Focused", Application.isFocused.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Is Batch Mode", Application.isBatchMode.ToString());
			GUILayout.EndVertical();
		}
	}

	public sealed class FpsCounter
	{
		private float _updateInterval;

		private float _currentFps;

		private int _frames;

		private float _accumulator;

		private float _timeLeft;

		public float UpdateInterval
		{
			get
			{
				return _updateInterval;
			}
			set
			{
				if (value <= 0f)
				{
					throw new Exception("Update interval is invalid.");
				}
				_updateInterval = value;
				Reset();
			}
		}

		public float CurrentFps => _currentFps;

		public FpsCounter(float updateInterval)
		{
			if (updateInterval <= 0f)
			{
				throw new Exception("Update interval is invalid.");
			}
			_updateInterval = updateInterval;
			Reset();
		}

		public void Update(float elapseSeconds, float realElapseSeconds)
		{
			_frames++;
			_accumulator += realElapseSeconds;
			_timeLeft -= realElapseSeconds;
			if (_timeLeft <= 0f)
			{
				_currentFps = ((_accumulator > 0f) ? ((float)_frames / _accumulator) : 0f);
				_frames = 0;
				_accumulator = 0f;
				_timeLeft += _updateInterval;
			}
		}

		private void Reset()
		{
			_currentFps = 0f;
			_frames = 0;
			_accumulator = 0f;
			_timeLeft = 0f;
		}
	}

	public sealed class GraphicsInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Graphics Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Device ID", SystemInfo.graphicsDeviceID.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Device Name", SystemInfo.graphicsDeviceName);
			ScrollableDebuggerWindowBase.DrawItem("Device Vendor ID", SystemInfo.graphicsDeviceVendorID.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Device Vendor", SystemInfo.graphicsDeviceVendor);
			ScrollableDebuggerWindowBase.DrawItem("Device Type", SystemInfo.graphicsDeviceType.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Device Version", SystemInfo.graphicsDeviceVersion);
			ScrollableDebuggerWindowBase.DrawItem("Memory Size", $"{SystemInfo.graphicsMemorySize.ToString()} MB");
			ScrollableDebuggerWindowBase.DrawItem("Multi Threaded", SystemInfo.graphicsMultiThreaded.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Rendering Threading Mode", SystemInfo.renderingThreadingMode.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Shader Level", GetShaderLevelString(SystemInfo.graphicsShaderLevel));
			ScrollableDebuggerWindowBase.DrawItem("Global Maximum LOD", Shader.globalMaximumLOD.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Global Render Pipeline", Shader.globalRenderPipeline);
			ScrollableDebuggerWindowBase.DrawItem("Active Tier", Graphics.activeTier.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Active Color Gamut", Graphics.activeColorGamut.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Preserve Frame Buffer Alpha", Graphics.preserveFramebufferAlpha.ToString());
			ScrollableDebuggerWindowBase.DrawItem("NPOT Support", SystemInfo.npotSupport.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Texture Size", SystemInfo.maxTextureSize.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supported Render Target Count", SystemInfo.supportedRenderTargetCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supported Random Write Target Count", SystemInfo.supportedRandomWriteTargetCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Copy Texture Support", SystemInfo.copyTextureSupport.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Uses Reversed ZBuffer", SystemInfo.usesReversedZBuffer.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Cubemap Size", SystemInfo.maxCubemapSize.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Graphics UV Starts At Top", SystemInfo.graphicsUVStartsAtTop.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Min Constant Buffer Offset Alignment", SystemInfo.minConstantBufferOffsetAlignment.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Has Hidden Surface Removal On GPU", SystemInfo.hasHiddenSurfaceRemovalOnGPU.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Has Dynamic Uniform Array Indexing In Fragment Shaders", SystemInfo.hasDynamicUniformArrayIndexingInFragmentShaders.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Has Mip Max Level", SystemInfo.hasMipMaxLevel.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Uses Load Store Actions", SystemInfo.usesLoadStoreActions.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Buffer Inputs Compute", SystemInfo.maxComputeBufferInputsCompute.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Buffer Inputs Domain", SystemInfo.maxComputeBufferInputsDomain.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Buffer Inputs Fragment", SystemInfo.maxComputeBufferInputsFragment.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Buffer Inputs Geometry", SystemInfo.maxComputeBufferInputsGeometry.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Buffer Inputs Hull", SystemInfo.maxComputeBufferInputsHull.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Buffer Inputs Vertex", SystemInfo.maxComputeBufferInputsVertex.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Work Group Size", SystemInfo.maxComputeWorkGroupSize.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Work Group Size X", SystemInfo.maxComputeWorkGroupSizeX.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Work Group Size Y", SystemInfo.maxComputeWorkGroupSizeY.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Compute Work Group Size Z", SystemInfo.maxComputeWorkGroupSizeZ.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Sparse Textures", SystemInfo.supportsSparseTextures.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports 3D Textures", SystemInfo.supports3DTextures.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Shadows", SystemInfo.supportsShadows.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Raw Shadow Depth Sampling", SystemInfo.supportsRawShadowDepthSampling.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Compute Shader", SystemInfo.supportsComputeShaders.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Instancing", SystemInfo.supportsInstancing.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports 2D Array Textures", SystemInfo.supports2DArrayTextures.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Motion Vectors", SystemInfo.supportsMotionVectors.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Cubemap Array Textures", SystemInfo.supportsCubemapArrayTextures.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports 3D Render Textures", SystemInfo.supports3DRenderTextures.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Texture Wrap Mirror Once", SystemInfo.supportsTextureWrapMirrorOnce.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Graphics Fence", SystemInfo.supportsGraphicsFence.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Async Compute", SystemInfo.supportsAsyncCompute.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Multi-sampled Textures", SystemInfo.supportsMultisampledTextures.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Async GPU Readback", SystemInfo.supportsAsyncGPUReadback.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports 32bits Index Buffer", SystemInfo.supports32bitsIndexBuffer.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Hardware Quad Topology", SystemInfo.supportsHardwareQuadTopology.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Mip Streaming", SystemInfo.supportsMipStreaming.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Multi-sample Auto Resolve", SystemInfo.supportsMultisampleAutoResolve.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Separated Render Targets Blend", SystemInfo.supportsSeparatedRenderTargetsBlend.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Set Constant Buffer", SystemInfo.supportsSetConstantBuffer.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Geometry Shaders", SystemInfo.supportsGeometryShaders.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Ray Tracing", SystemInfo.supportsRayTracing.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Tessellation Shaders", SystemInfo.supportsTessellationShaders.ToString());
			GUILayout.EndVertical();
		}

		private string GetShaderLevelString(int shaderLevel)
		{
			return $"Shader Model {(shaderLevel / 10).ToString()}.{(shaderLevel % 10).ToString()}";
		}
	}

	public sealed class InputAccelerationInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Input Acceleration Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Acceleration", Input.acceleration.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Acceleration Event Count", Input.accelerationEventCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Acceleration Events", GetAccelerationEventsString(Input.accelerationEvents));
			GUILayout.EndVertical();
		}

		private string GetAccelerationEventString(AccelerationEvent accelerationEvent)
		{
			return $"{accelerationEvent.acceleration.ToString()}, {accelerationEvent.deltaTime.ToString()}";
		}

		private string GetAccelerationEventsString(AccelerationEvent[] accelerationEvents)
		{
			string[] array = new string[accelerationEvents.Length];
			for (int i = 0; i < accelerationEvents.Length; i++)
			{
				array[i] = GetAccelerationEventString(accelerationEvents[i]);
			}
			return string.Join("; ", array);
		}
	}

	public sealed class InputCompassInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Input Compass Information</b>");
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Enable", GUILayout.Height(30f)))
			{
				Input.compass.enabled = true;
			}
			if (GUILayout.Button("Disable", GUILayout.Height(30f)))
			{
				Input.compass.enabled = false;
			}
			GUILayout.EndHorizontal();
			ScrollableDebuggerWindowBase.DrawItem("Enabled", Input.compass.enabled.ToString());
			if (Input.compass.enabled)
			{
				ScrollableDebuggerWindowBase.DrawItem("Heading Accuracy", Input.compass.headingAccuracy.ToString());
				ScrollableDebuggerWindowBase.DrawItem("Magnetic Heading", Input.compass.magneticHeading.ToString());
				ScrollableDebuggerWindowBase.DrawItem("Raw Vector", Input.compass.rawVector.ToString());
				ScrollableDebuggerWindowBase.DrawItem("Timestamp", Input.compass.timestamp.ToString());
				ScrollableDebuggerWindowBase.DrawItem("True Heading", Input.compass.trueHeading.ToString());
			}
			GUILayout.EndVertical();
		}
	}

	public sealed class InputGyroscopeInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Input Gyroscope Information</b>");
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Enable", GUILayout.Height(30f)))
			{
				Input.gyro.enabled = true;
			}
			if (GUILayout.Button("Disable", GUILayout.Height(30f)))
			{
				Input.gyro.enabled = false;
			}
			GUILayout.EndHorizontal();
			ScrollableDebuggerWindowBase.DrawItem("Enabled", Input.gyro.enabled.ToString());
			if (Input.gyro.enabled)
			{
				ScrollableDebuggerWindowBase.DrawItem("Update Interval", Input.gyro.updateInterval.ToString());
				ScrollableDebuggerWindowBase.DrawItem("Attitude", Input.gyro.attitude.eulerAngles.ToString());
				ScrollableDebuggerWindowBase.DrawItem("Gravity", Input.gyro.gravity.ToString());
				ScrollableDebuggerWindowBase.DrawItem("Rotation Rate", Input.gyro.rotationRate.ToString());
				ScrollableDebuggerWindowBase.DrawItem("Rotation Rate Unbiased", Input.gyro.rotationRateUnbiased.ToString());
				ScrollableDebuggerWindowBase.DrawItem("User Acceleration", Input.gyro.userAcceleration.ToString());
			}
			GUILayout.EndVertical();
		}
	}

	public sealed class InputSummaryInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Input Summary Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Back Button Leaves App", Input.backButtonLeavesApp.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Device Orientation", Input.deviceOrientation.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Mouse Present", Input.mousePresent.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Mouse Position", Input.mousePosition.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Mouse Scroll Delta", Input.mouseScrollDelta.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Any Key", Input.anyKey.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Any Key Down", Input.anyKeyDown.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Input String", Input.inputString);
			ScrollableDebuggerWindowBase.DrawItem("IME Is Selected", Input.imeIsSelected.ToString());
			ScrollableDebuggerWindowBase.DrawItem("IME Composition Mode", Input.imeCompositionMode.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Compensate Sensors", Input.compensateSensors.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Composition Cursor Position", Input.compositionCursorPos.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Composition String", Input.compositionString);
			GUILayout.EndVertical();
		}
	}

	public sealed class InputTouchInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Input Touch Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Touch Supported", Input.touchSupported.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Touch Pressure Supported", Input.touchPressureSupported.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Stylus Touch Supported", Input.stylusTouchSupported.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Simulate Mouse With Touches", Input.simulateMouseWithTouches.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Multi Touch Enabled", Input.multiTouchEnabled.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Touch Count", Input.touchCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Touches", GetTouchesString(Input.touches));
			GUILayout.EndVertical();
		}

		private string GetTouchString(Touch touch)
		{
			return $"{touch.position.ToString()}, {touch.deltaPosition.ToString()}, {touch.rawPosition.ToString()}, {touch.pressure.ToString()}, {touch.phase.ToString()}";
		}

		private string GetTouchesString(Touch[] touches)
		{
			string[] array = new string[touches.Length];
			for (int i = 0; i < touches.Length; i++)
			{
				array[i] = GetTouchString(touches[i]);
			}
			return string.Join("; ", array);
		}
	}

	public sealed class LogNode
	{
		private DateTime _logTime;

		private int _logFrameCount;

		private LogType _logType;

		private string _logMessage;

		private string _stackTrack;

		public DateTime LogTime => _logTime;

		public int LogFrameCount => _logFrameCount;

		public LogType LogType => _logType;

		public string LogMessage => _logMessage;

		public string StackTrack => _stackTrack;

		private static ObjectPool<LogNode> Pool { get; } = GenericPool<LogNode>.Init(_OnInit, _OnDispose);

		public LogNode()
		{
			_logTime = default(DateTime);
			_logFrameCount = 0;
			_logType = LogType.Error;
			_logMessage = null;
			_stackTrack = null;
		}

		public static LogNode Create(LogType logType, string logMessage, string stackTrack)
		{
			LogNode logNode = Pool.Get();
			logNode._logTime = DateTime.UtcNow;
			logNode._logFrameCount = Time.frameCount;
			logNode._logType = logType;
			logNode._logMessage = logMessage;
			logNode._stackTrack = stackTrack;
			return logNode;
		}

		public static void Release(LogNode logNode)
		{
			Pool.Release(logNode);
		}

		private static void _OnInit(LogNode logNode)
		{
		}

		private static void _OnDispose(LogNode logNode)
		{
			logNode._logTime = default(DateTime);
			logNode._logFrameCount = 0;
			logNode._logType = LogType.Error;
			logNode._logMessage = null;
			logNode._stackTrack = null;
		}
	}

	public sealed class NetworkInformationWindow : ScrollableDebuggerWindowBase
	{
		public override void Initialize(params object[] args)
		{
		}

		protected override void OnDrawScrollableWindow()
		{
		}

		private void DrawNetworkChannel()
		{
		}
	}

	public sealed class OperationsWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Operations</b>");
			GUILayout.BeginVertical("box");
			if (GUILayout.Button("Shutdown Game", GUILayout.Height(30f)))
			{
				Application.Quit();
			}
			GUILayout.EndVertical();
		}
	}

	public sealed class PathInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Path Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Current Directory", Environment.CurrentDirectory);
			ScrollableDebuggerWindowBase.DrawItem("Data Path", Application.dataPath);
			ScrollableDebuggerWindowBase.DrawItem("Persistent Data Path", Application.persistentDataPath);
			ScrollableDebuggerWindowBase.DrawItem("Streaming Assets Path", Application.streamingAssetsPath);
			ScrollableDebuggerWindowBase.DrawItem("Temporary Cache Path", Application.temporaryCachePath);
			ScrollableDebuggerWindowBase.DrawItem("Console Log Path", Application.consoleLogPath);
			GUILayout.EndVertical();
		}
	}

	public sealed class ProfilerInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Profiler Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Supported", Profiler.supported.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Enabled", Profiler.enabled.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Enable Binary Log", Profiler.enableBinaryLog ? $"True, {Profiler.logFile}" : "False");
			ScrollableDebuggerWindowBase.DrawItem("Enable Allocation Callstacks", Profiler.enableAllocationCallstacks.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Area Count", Profiler.areaCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Used Memory", ScrollableDebuggerWindowBase.GetByteLengthString(Profiler.maxUsedMemory));
			ScrollableDebuggerWindowBase.DrawItem("Mono Used Size", ScrollableDebuggerWindowBase.GetByteLengthString(Profiler.GetMonoUsedSizeLong()));
			ScrollableDebuggerWindowBase.DrawItem("Mono Heap Size", ScrollableDebuggerWindowBase.GetByteLengthString(Profiler.GetMonoHeapSizeLong()));
			ScrollableDebuggerWindowBase.DrawItem("Used Heap Size", ScrollableDebuggerWindowBase.GetByteLengthString(Profiler.usedHeapSizeLong));
			ScrollableDebuggerWindowBase.DrawItem("Total Allocated Memory", ScrollableDebuggerWindowBase.GetByteLengthString(Profiler.GetTotalAllocatedMemoryLong()));
			ScrollableDebuggerWindowBase.DrawItem("Total Reserved Memory", ScrollableDebuggerWindowBase.GetByteLengthString(Profiler.GetTotalReservedMemoryLong()));
			ScrollableDebuggerWindowBase.DrawItem("Total Unused Reserved Memory", ScrollableDebuggerWindowBase.GetByteLengthString(Profiler.GetTotalUnusedReservedMemoryLong()));
			ScrollableDebuggerWindowBase.DrawItem("Allocated Memory For Graphics Driver", ScrollableDebuggerWindowBase.GetByteLengthString(Profiler.GetAllocatedMemoryForGraphicsDriver()));
			ScrollableDebuggerWindowBase.DrawItem("Temp Allocator Size", ScrollableDebuggerWindowBase.GetByteLengthString(Profiler.GetTempAllocatorSize()));
			GUILayout.EndVertical();
		}
	}

	public sealed class QualityInformationWindow : ScrollableDebuggerWindowBase
	{
		private bool _applyExpensiveChanges;

		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Quality Level</b>");
			GUILayout.BeginVertical("box");
			int qualityLevel = QualitySettings.GetQualityLevel();
			ScrollableDebuggerWindowBase.DrawItem("Current Quality Level", QualitySettings.names[qualityLevel]);
			_applyExpensiveChanges = GUILayout.Toggle(_applyExpensiveChanges, "Apply expensive changes on quality level change.");
			int num = GUILayout.SelectionGrid(qualityLevel, QualitySettings.names, 3, "toggle");
			if (num != qualityLevel)
			{
				QualitySettings.SetQualityLevel(num, _applyExpensiveChanges);
			}
			GUILayout.EndVertical();
			GUILayout.Label("<b>Rendering Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Active Color Space", QualitySettings.activeColorSpace.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Desired Color Space", QualitySettings.desiredColorSpace.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Max Queued Frames", QualitySettings.maxQueuedFrames.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Pixel Light Count", QualitySettings.pixelLightCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Master Texture Limit", QualitySettings.masterTextureLimit.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Anisotropic Filtering", QualitySettings.anisotropicFiltering.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Anti Aliasing", QualitySettings.antiAliasing.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Soft Particles", QualitySettings.softParticles.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Soft Vegetation", QualitySettings.softVegetation.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Realtime Reflection Probes", QualitySettings.realtimeReflectionProbes.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Billboards Face Camera Position", QualitySettings.billboardsFaceCameraPosition.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Resolution Scaling Fixed DPI Factor", QualitySettings.resolutionScalingFixedDPIFactor.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Texture Streaming Enabled", QualitySettings.streamingMipmapsActive.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Texture Streaming Add All Cameras", QualitySettings.streamingMipmapsAddAllCameras.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Texture Streaming Memory Budget", QualitySettings.streamingMipmapsMemoryBudget.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Texture Streaming Renderers Per Frame", QualitySettings.streamingMipmapsRenderersPerFrame.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Texture Streaming Max Level Reduction", QualitySettings.streamingMipmapsMaxLevelReduction.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Texture Streaming Max File IO Requests", QualitySettings.streamingMipmapsMaxFileIORequests.ToString());
			GUILayout.EndVertical();
			GUILayout.Label("<b>Shadows Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Shadowmask Mode", QualitySettings.shadowmaskMode.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Shadow Quality", QualitySettings.shadows.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Shadow Resolution", QualitySettings.shadowResolution.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Shadow Projection", QualitySettings.shadowProjection.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Shadow Distance", QualitySettings.shadowDistance.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Shadow Near Plane Offset", QualitySettings.shadowNearPlaneOffset.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Shadow Cascades", QualitySettings.shadowCascades.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Shadow Cascade 2 Split", QualitySettings.shadowCascade2Split.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Shadow Cascade 4 Split", QualitySettings.shadowCascade4Split.ToString());
			GUILayout.EndVertical();
			GUILayout.Label("<b>Other Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Skin Weights", QualitySettings.skinWeights.ToString());
			ScrollableDebuggerWindowBase.DrawItem("VSync Count", QualitySettings.vSyncCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("LOD Bias", QualitySettings.lodBias.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Maximum LOD Level", QualitySettings.maximumLODLevel.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Particle Raycast Budget", QualitySettings.particleRaycastBudget.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Async Upload Time Slice", $"{QualitySettings.asyncUploadTimeSlice.ToString()} ms");
			ScrollableDebuggerWindowBase.DrawItem("Async Upload Buffer Size", $"{QualitySettings.asyncUploadBufferSize.ToString()} MB");
			ScrollableDebuggerWindowBase.DrawItem("Async Upload Persistent Buffer", QualitySettings.asyncUploadPersistentBuffer.ToString());
			GUILayout.EndVertical();
		}
	}

	public sealed class RuntimeMemoryInformationWindow<T> : ScrollableDebuggerWindowBase where T : UnityEngine.Object
	{
		private sealed class Sample
		{
			public string Name { get; private set; }

			public string Type { get; private set; }

			public long Size { get; private set; }

			public bool Highlight { get; set; }

			public Sample(string name, string type, long size)
			{
				Name = name;
				Type = type;
				Size = size;
				Highlight = false;
			}
		}

		private const int ShowSampleCount = 300;

		private readonly List<Sample> _samples = new List<Sample>();

		private readonly Comparison<Sample> _sampleComparer = SampleComparer;

		private DateTime _sampleTime = DateTime.MinValue;

		private long _sampleSize;

		private long _duplicateSampleSize;

		private int _duplicateSimpleCount;

		protected override void OnDrawScrollableWindow()
		{
			string name = typeof(T).Name;
			GUILayout.Label($"<b>{name} Runtime Memory Information</b>");
			GUILayout.BeginVertical("box");
			if (GUILayout.Button($"Take Sample for {name}", GUILayout.Height(30f)))
			{
				TakeSample();
			}
			if (_sampleTime <= DateTime.MinValue)
			{
				GUILayout.Label($"<b>Please take sample for {name} first.</b>");
			}
			else
			{
				if (_duplicateSimpleCount > 0)
				{
					GUILayout.Label(string.Format("<b>{0} {1}s ({2}) obtained at {3}, while {4} {1}s ({5}) might be duplicated.</b>", _samples.Count.ToString(), name, ScrollableDebuggerWindowBase.GetByteLengthString(_sampleSize), _sampleTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"), _duplicateSimpleCount.ToString(), ScrollableDebuggerWindowBase.GetByteLengthString(_duplicateSampleSize)));
				}
				else
				{
					GUILayout.Label(string.Format("<b>{0} {1}s ({2}) obtained at {3}.</b>", _samples.Count.ToString(), name, ScrollableDebuggerWindowBase.GetByteLengthString(_sampleSize), _sampleTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")));
				}
				if (_samples.Count > 0)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label($"<b>{name} Name</b>");
					GUILayout.Label("<b>Type</b>", GUILayout.Width(240f));
					GUILayout.Label("<b>Size</b>", GUILayout.Width(80f));
					GUILayout.EndHorizontal();
				}
				int num = 0;
				for (int i = 0; i < _samples.Count; i++)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(_samples[i].Highlight ? $"<color=yellow>{_samples[i].Name}</color>" : _samples[i].Name);
					GUILayout.Label(_samples[i].Highlight ? $"<color=yellow>{_samples[i].Type}</color>" : _samples[i].Type, GUILayout.Width(240f));
					GUILayout.Label(_samples[i].Highlight ? $"<color=yellow>{ScrollableDebuggerWindowBase.GetByteLengthString(_samples[i].Size)}</color>" : ScrollableDebuggerWindowBase.GetByteLengthString(_samples[i].Size), GUILayout.Width(80f));
					GUILayout.EndHorizontal();
					num++;
					if (num >= 300)
					{
						break;
					}
				}
			}
			GUILayout.EndVertical();
		}

		private void TakeSample()
		{
			_sampleTime = DateTime.UtcNow;
			_sampleSize = 0L;
			_duplicateSampleSize = 0L;
			_duplicateSimpleCount = 0;
			_samples.Clear();
			T[] array = Resources.FindObjectsOfTypeAll<T>();
			for (int i = 0; i < array.Length; i++)
			{
				long num = 0L;
				num = Profiler.GetRuntimeMemorySizeLong(array[i]);
				_sampleSize += num;
				_samples.Add(new Sample(array[i].name, array[i].GetType().Name, num));
			}
			_samples.Sort(_sampleComparer);
			for (int j = 1; j < _samples.Count; j++)
			{
				if (_samples[j].Name == _samples[j - 1].Name && _samples[j].Type == _samples[j - 1].Type && _samples[j].Size == _samples[j - 1].Size)
				{
					_samples[j].Highlight = true;
					_duplicateSampleSize += _samples[j].Size;
					_duplicateSimpleCount++;
				}
			}
		}

		private static int SampleComparer(Sample a, Sample b)
		{
			int num = b.Size.CompareTo(a.Size);
			if (num != 0)
			{
				return num;
			}
			num = a.Type.CompareTo(b.Type);
			if (num != 0)
			{
				return num;
			}
			return a.Name.CompareTo(b.Name);
		}
	}

	public sealed class RuntimeMemorySummaryWindow : ScrollableDebuggerWindowBase
	{
		private sealed class Record
		{
			public string Name { get; private set; }

			public int Count { get; set; }

			public long Size { get; set; }

			public Record(string name)
			{
				Name = name;
				Count = 0;
				Size = 0L;
			}
		}

		private readonly List<Record> _records = new List<Record>();

		private readonly Comparison<Record> _recordComparer = RecordComparer;

		private DateTime _sampleTime = DateTime.MinValue;

		private int _sampleCount;

		private long _sampleSize;

		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Runtime Memory Summary</b>");
			GUILayout.BeginVertical("box");
			if (GUILayout.Button("Take Sample", GUILayout.Height(30f)))
			{
				TakeSample();
			}
			if (_sampleTime <= DateTime.MinValue)
			{
				GUILayout.Label("<b>Please take sample first.</b>");
			}
			else
			{
				GUILayout.Label(string.Format("<b>{0} Objects ({1}) obtained at {2}.</b>", _sampleCount.ToString(), ScrollableDebuggerWindowBase.GetByteLengthString(_sampleSize), _sampleTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")));
				GUILayout.BeginHorizontal();
				GUILayout.Label("<b>Type</b>");
				GUILayout.Label("<b>Count</b>", GUILayout.Width(120f));
				GUILayout.Label("<b>Size</b>", GUILayout.Width(120f));
				GUILayout.EndHorizontal();
				for (int i = 0; i < _records.Count; i++)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(_records[i].Name);
					GUILayout.Label(_records[i].Count.ToString(), GUILayout.Width(120f));
					GUILayout.Label(ScrollableDebuggerWindowBase.GetByteLengthString(_records[i].Size), GUILayout.Width(120f));
					GUILayout.EndHorizontal();
				}
			}
			GUILayout.EndVertical();
		}

		private void TakeSample()
		{
			_records.Clear();
			_sampleTime = DateTime.UtcNow;
			_sampleCount = 0;
			_sampleSize = 0L;
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll<UnityEngine.Object>();
			for (int i = 0; i < array.Length; i++)
			{
				long num = 0L;
				num = Profiler.GetRuntimeMemorySizeLong(array[i]);
				string name = array[i].GetType().Name;
				_sampleCount++;
				_sampleSize += num;
				Record record = null;
				foreach (Record record2 in _records)
				{
					if (record2.Name == name)
					{
						record = record2;
						break;
					}
				}
				if (record == null)
				{
					record = new Record(name);
					_records.Add(record);
				}
				record.Count++;
				record.Size += num;
			}
			_records.Sort(_recordComparer);
		}

		private static int RecordComparer(Record a, Record b)
		{
			int num = b.Size.CompareTo(a.Size);
			if (num != 0)
			{
				return num;
			}
			num = a.Count.CompareTo(b.Count);
			if (num != 0)
			{
				return num;
			}
			return a.Name.CompareTo(b.Name);
		}
	}

	public sealed class SceneInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Scene Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Scene Count", UnityEngine.SceneManagement.SceneManager.sceneCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Scene Count In Build Settings", UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings.ToString());
			Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
			ScrollableDebuggerWindowBase.DrawItem("Active Scene Handle", activeScene.handle.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Active Scene Name", activeScene.name);
			ScrollableDebuggerWindowBase.DrawItem("Active Scene Path", activeScene.path);
			ScrollableDebuggerWindowBase.DrawItem("Active Scene Build Index", activeScene.buildIndex.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Active Scene Is Dirty", activeScene.isDirty.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Active Scene Is Loaded", activeScene.isLoaded.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Active Scene Is Valid", activeScene.IsValid().ToString());
			ScrollableDebuggerWindowBase.DrawItem("Active Scene Root Count", activeScene.rootCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Active Scene Is Sub Scene", activeScene.isSubScene.ToString());
			GUILayout.EndVertical();
		}
	}

	public sealed class ScreenInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Screen Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Current Resolution", GetResolutionString(Screen.currentResolution));
			ScrollableDebuggerWindowBase.DrawItem("Screen Width", string.Format("{0} px / {1} in / {2} cm", Screen.width.ToString(), ((float)Screen.width / Screen.dpi).ToString("F2"), (2.54 * (double)Screen.width / (double)Screen.dpi).ToString("F2")));
			ScrollableDebuggerWindowBase.DrawItem("Screen Height", string.Format("{0} px / {1} in / {2} cm", Screen.height.ToString(), ((float)Screen.height / Screen.dpi).ToString("F2"), (2.54 * (double)Screen.height / (double)Screen.dpi).ToString("F2")));
			ScrollableDebuggerWindowBase.DrawItem("Screen DPI", Screen.dpi.ToString("F2"));
			ScrollableDebuggerWindowBase.DrawItem("Screen Orientation", Screen.orientation.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Is Full Screen", Screen.fullScreen.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Full Screen Mode", Screen.fullScreenMode.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Sleep Timeout", GetSleepTimeoutDescription(Screen.sleepTimeout));
			ScrollableDebuggerWindowBase.DrawItem("Brightness", Screen.brightness.ToString("F2"));
			ScrollableDebuggerWindowBase.DrawItem("Cursor Visible", Cursor.visible.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Cursor Lock State", Cursor.lockState.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Auto Landscape Left", Screen.autorotateToLandscapeLeft.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Auto Landscape Right", Screen.autorotateToLandscapeRight.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Auto Portrait", Screen.autorotateToPortrait.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Auto Portrait Upside Down", Screen.autorotateToPortraitUpsideDown.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Safe Area", Screen.safeArea.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Cutouts", GetCutoutsString(Screen.cutouts));
			ScrollableDebuggerWindowBase.DrawItem("Support Resolutions", GetResolutionsString(Screen.resolutions));
			GUILayout.EndVertical();
		}

		private string GetSleepTimeoutDescription(int sleepTimeout)
		{
			return sleepTimeout switch
			{
				-1 => "Never Sleep", 
				-2 => "System Setting", 
				_ => sleepTimeout.ToString(), 
			};
		}

		private string GetResolutionString(Resolution resolution)
		{
			return $"{resolution.width.ToString()} x {resolution.height.ToString()} @ {resolution.refreshRate.ToString()}Hz";
		}

		private string GetCutoutsString(Rect[] cutouts)
		{
			string[] array = new string[cutouts.Length];
			for (int i = 0; i < cutouts.Length; i++)
			{
				array[i] = cutouts[i].ToString();
			}
			return string.Join("; ", array);
		}

		private string GetResolutionsString(Resolution[] resolutions)
		{
			string[] array = new string[resolutions.Length];
			for (int i = 0; i < resolutions.Length; i++)
			{
				array[i] = GetResolutionString(resolutions[i]);
			}
			return string.Join("; ", array);
		}
	}

	public abstract class ScrollableDebuggerWindowBase : IDebuggerWindow
	{
		private const float TitleWidth = 240f;

		private Vector2 _scrollPosition = Vector2.zero;

		public virtual void Initialize(params object[] args)
		{
		}

		public virtual void Shutdown()
		{
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnLeave()
		{
		}

		public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
		}

		public void OnDraw()
		{
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
			OnDrawScrollableWindow();
			GUILayout.EndScrollView();
		}

		protected abstract void OnDrawScrollableWindow();

		protected static void DrawItem(string title, string content)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(title, GUILayout.Width(240f));
			if (GUILayout.Button(content, "label"))
			{
				CopyToClipboard(content);
			}
			GUILayout.EndHorizontal();
		}

		protected static string GetByteLengthString(long byteLength)
		{
			if (byteLength < 1024)
			{
				return $"{byteLength.ToString()} Bytes";
			}
			if (byteLength < 1048576)
			{
				return string.Format("{0} KB", ((float)byteLength / 1024f).ToString("F2"));
			}
			if (byteLength < 1073741824)
			{
				return string.Format("{0} MB", ((float)byteLength / 1048576f).ToString("F2"));
			}
			if (byteLength < 1099511627776L)
			{
				return string.Format("{0} GB", ((float)byteLength / 1.0737418E+09f).ToString("F2"));
			}
			if (byteLength < 1125899906842624L)
			{
				return string.Format("{0} TB", ((float)byteLength / 1.0995116E+12f).ToString("F2"));
			}
			if (byteLength < 1152921504606846976L)
			{
				return string.Format("{0} PB", ((float)byteLength / 1.1258999E+15f).ToString("F2"));
			}
			return string.Format("{0} EB", ((float)byteLength / 1.1529215E+18f).ToString("F2"));
		}
	}

	public sealed class SettingsWindow : ScrollableDebuggerWindowBase
	{
		private DebuggerWindow _debuggerWindow;

		private float _lastIconX;

		private float _lastIconY;

		private float _lastWindowX;

		private float _lastWindowY;

		private float _lastWindowWidth;

		private float _lastWindowHeight;

		private float _lastWindowScale;

		public override void Initialize(params object[] args)
		{
			_debuggerWindow = SingletonBehaviour<DebuggerService>.Instance.Debugger.DebuggerWindow;
			_lastIconX = _debuggerWindow.IconRect.x;
			_lastIconY = _debuggerWindow.IconRect.y;
			_lastWindowX = _debuggerWindow.WindowRect.x;
			_lastWindowY = _debuggerWindow.WindowRect.y;
			_lastWindowWidth = _debuggerWindow.WindowRect.width;
			_lastWindowHeight = _debuggerWindow.WindowRect.height;
			_debuggerWindow.WindowScale = (_lastWindowScale = _debuggerWindow.WindowScale);
			_debuggerWindow.IconRect = new Rect(_lastIconX, _lastIconY, DefaultIconRect.width, DefaultIconRect.height);
			_debuggerWindow.WindowRect = new Rect(_lastWindowX, _lastWindowY, _lastWindowWidth, _lastWindowHeight);
		}

		public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			if ((double)Math.Abs(_lastIconX - _debuggerWindow.IconRect.x) > 0.1)
			{
				_lastIconX = _debuggerWindow.IconRect.x;
			}
			if ((double)Math.Abs(_lastIconY - _debuggerWindow.IconRect.y) > 0.1)
			{
				_lastIconY = _debuggerWindow.IconRect.y;
			}
			if ((double)Math.Abs(_lastWindowX - _debuggerWindow.WindowRect.x) > 0.1)
			{
				_lastWindowX = _debuggerWindow.WindowRect.x;
			}
			if ((double)Math.Abs(_lastWindowY - _debuggerWindow.WindowRect.y) > 0.1)
			{
				_lastWindowY = _debuggerWindow.WindowRect.y;
			}
			if ((double)Math.Abs(_lastWindowWidth - _debuggerWindow.WindowRect.width) > 0.1)
			{
				_lastWindowWidth = _debuggerWindow.WindowRect.width;
			}
			if ((double)Math.Abs(_lastWindowHeight - _debuggerWindow.WindowRect.height) > 0.1)
			{
				_lastWindowHeight = _debuggerWindow.WindowRect.height;
			}
			if ((double)Math.Abs(_lastWindowScale - _debuggerWindow.WindowScale) > 0.1)
			{
				_lastWindowScale = _debuggerWindow.WindowScale;
			}
		}

		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Window Settings</b>");
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Position:", GUILayout.Width(60f));
			GUILayout.Label("Drag window caption to move position.");
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			float num = _debuggerWindow.WindowRect.width;
			GUILayout.Label("Width:", GUILayout.Width(60f));
			if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
			{
				num -= 1f;
			}
			num = GUILayout.HorizontalSlider(num, 100f, (float)Screen.width - 20f);
			if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
			{
				num += 1f;
			}
			num = Mathf.Clamp(num, 100f, (float)Screen.width - 20f);
			if ((double)Math.Abs(num - _debuggerWindow.WindowRect.width) > 0.1)
			{
				_debuggerWindow.WindowRect = new Rect(_debuggerWindow.WindowRect.x, _debuggerWindow.WindowRect.y, num, _debuggerWindow.WindowRect.height);
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			float num2 = _debuggerWindow.WindowRect.height;
			GUILayout.Label("Height:", GUILayout.Width(60f));
			if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
			{
				num2 -= 1f;
			}
			num2 = GUILayout.HorizontalSlider(num2, 100f, (float)Screen.height - 20f);
			if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
			{
				num2 += 1f;
			}
			num2 = Mathf.Clamp(num2, 100f, (float)Screen.height - 20f);
			if ((double)Math.Abs(num2 - _debuggerWindow.WindowRect.height) > 0.1)
			{
				_debuggerWindow.WindowRect = new Rect(_debuggerWindow.WindowRect.x, _debuggerWindow.WindowRect.y, _debuggerWindow.WindowRect.width, num2);
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			float num3 = _debuggerWindow.WindowScale;
			GUILayout.Label("Scale:", GUILayout.Width(60f));
			if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
			{
				num3 -= 0.01f;
			}
			num3 = GUILayout.HorizontalSlider(num3, 0.5f, 4f);
			if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
			{
				num3 += 0.01f;
			}
			num3 = Mathf.Clamp(num3, 0.5f, 4f);
			if (num3 != _debuggerWindow.WindowScale)
			{
				_debuggerWindow.WindowScale = num3;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("0.5x", GUILayout.Height(60f)))
			{
				_debuggerWindow.WindowScale = 0.5f;
			}
			if (GUILayout.Button("1.0x", GUILayout.Height(60f)))
			{
				_debuggerWindow.WindowScale = 1f;
			}
			if (GUILayout.Button("1.2x", GUILayout.Height(60f)))
			{
				_debuggerWindow.WindowScale = 1.2f;
			}
			if (GUILayout.Button("1.5x", GUILayout.Height(60f)))
			{
				_debuggerWindow.WindowScale = 1.5f;
			}
			if (GUILayout.Button("1.8x", GUILayout.Height(60f)))
			{
				_debuggerWindow.WindowScale = 1.8f;
			}
			if (GUILayout.Button("2.0x", GUILayout.Height(60f)))
			{
				_debuggerWindow.WindowScale = 2f;
			}
			if (GUILayout.Button("2.5x", GUILayout.Height(60f)))
			{
				_debuggerWindow.WindowScale = 2.5f;
			}
			if (GUILayout.Button("3.0x", GUILayout.Height(60f)))
			{
				_debuggerWindow.WindowScale = 3f;
			}
			GUILayout.EndHorizontal();
			if (GUILayout.Button("Reset Layout", GUILayout.Height(30f)))
			{
				_debuggerWindow.ResetLayout();
			}
			GUILayout.EndVertical();
		}
	}

	public sealed class SystemInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>System Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Device Unique ID", SystemInfo.deviceUniqueIdentifier);
			ScrollableDebuggerWindowBase.DrawItem("Device Name", SystemInfo.deviceName);
			ScrollableDebuggerWindowBase.DrawItem("Device Type", SystemInfo.deviceType.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Device Model", SystemInfo.deviceModel);
			ScrollableDebuggerWindowBase.DrawItem("Processor Type", SystemInfo.processorType);
			ScrollableDebuggerWindowBase.DrawItem("Processor Count", SystemInfo.processorCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Processor Frequency", $"{SystemInfo.processorFrequency.ToString()} MHz");
			ScrollableDebuggerWindowBase.DrawItem("System Memory Size", $"{SystemInfo.systemMemorySize.ToString()} MB");
			ScrollableDebuggerWindowBase.DrawItem("Operating System Family", SystemInfo.operatingSystemFamily.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Operating System", SystemInfo.operatingSystem);
			ScrollableDebuggerWindowBase.DrawItem("Battery Status", SystemInfo.batteryStatus.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Battery Level", GetBatteryLevelString(SystemInfo.batteryLevel));
			ScrollableDebuggerWindowBase.DrawItem("Supports Audio", SystemInfo.supportsAudio.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Accelerometer", SystemInfo.supportsAccelerometer.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Gyroscope", SystemInfo.supportsGyroscope.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Supports Vibration", SystemInfo.supportsVibration.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Genuine", Application.genuine.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Genuine Check Available", Application.genuineCheckAvailable.ToString());
			GUILayout.EndVertical();
		}

		private string GetBatteryLevelString(float batteryLevel)
		{
			if (batteryLevel < 0f)
			{
				return "Unavailable";
			}
			return batteryLevel.ToString("P0");
		}
	}

	public sealed class TimeInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Time Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Time Scale", $"{Time.timeScale.ToString()} [{GetTimeScaleDescription(Time.timeScale)}]");
			ScrollableDebuggerWindowBase.DrawItem("Realtime Since Startup", Time.realtimeSinceStartup.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Time Since Level Load", Time.timeSinceLevelLoad.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Time", Time.time.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Fixed Time", Time.fixedTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Unscaled Time", Time.unscaledTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Fixed Unscaled Time", Time.fixedUnscaledTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Delta Time", Time.deltaTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Fixed Delta Time", Time.fixedDeltaTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Unscaled Delta Time", Time.unscaledDeltaTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Fixed Unscaled Delta Time", Time.fixedUnscaledDeltaTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Smooth Delta Time", Time.smoothDeltaTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Maximum Delta Time", Time.maximumDeltaTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Maximum Particle Delta Time", Time.maximumParticleDeltaTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Frame Count", Time.frameCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Rendered Frame Count", Time.renderedFrameCount.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Capture Framerate", Time.captureFramerate.ToString());
			ScrollableDebuggerWindowBase.DrawItem("Capture Delta Time", Time.captureDeltaTime.ToString());
			ScrollableDebuggerWindowBase.DrawItem("In Fixed Time Step", Time.inFixedTimeStep.ToString());
			GUILayout.EndVertical();
		}

		private string GetTimeScaleDescription(float timeScale)
		{
			if (timeScale <= 0f)
			{
				return "Pause";
			}
			if (timeScale < 1f)
			{
				return "Slower";
			}
			if (timeScale > 1f)
			{
				return "Faster";
			}
			return "Normal";
		}
	}

	public sealed class WebPlayerInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Web Player Information</b>");
			GUILayout.BeginVertical("box");
			ScrollableDebuggerWindowBase.DrawItem("Absolute URL", Application.absoluteURL);
			GUILayout.EndVertical();
		}
	}

	private static readonly Rect DefaultIconRect = new Rect(10f, 10f, 60f, 60f);

	private static readonly Rect DefaultWindowRect = new Rect(10f, 10f, Screen.width - 20, Screen.height - 20);

	private const float DefaultWindowScale = 1f;

	[SerializeField]
	private readonly DebuggerActiveWindowType _activeWindow;

	private static readonly TextEditor TextEditor = new TextEditor();

	private readonly Rect _dragRect = new Rect(0f, 0f, float.MaxValue, 25f);

	[SerializeField]
	private readonly GUISkin _skin;

	private FpsCounter _fpsCounter;

	private Debugger debugger => SingletonBehaviour<DebuggerService>.Instance.Debugger;

	public bool ActiveWindow { get; set; }

	public bool ShowFullWindow { get; set; }

	public Rect IconRect { get; set; } = DefaultIconRect;

	public Rect WindowRect { get; set; } = DefaultWindowRect;

	public float WindowScale { get; set; } = 1f;

	public void Start()
	{
		_fpsCounter = new FpsCounter(0.5f);
		switch (_activeWindow)
		{
		case DebuggerActiveWindowType.AlwaysOpen:
			ActiveWindow = true;
			break;
		case DebuggerActiveWindowType.OnlyOpenWhenDevelopment:
			ActiveWindow = Debug.isDebugBuild;
			break;
		case DebuggerActiveWindowType.OnlyOpenInEditor:
			ActiveWindow = Application.isEditor;
			break;
		default:
			ActiveWindow = false;
			break;
		}
	}

	public void Update()
	{
		_fpsCounter.Update(Time.deltaTime, Time.unscaledDeltaTime);
	}

	public void OnGUI()
	{
		GUISkin skin = GUI.skin;
		Matrix4x4 matrix = GUI.matrix;
		GUI.skin = _skin;
		GUI.matrix = Matrix4x4.Scale(new Vector3(WindowScale, WindowScale, 1f));
		if (ShowFullWindow)
		{
			WindowRect = GUILayout.Window(0, WindowRect, DrawWindow, "<b>GAME DEBUGGER</b>");
		}
		else
		{
			IconRect = GUILayout.Window(0, IconRect, DrawDebuggerWindowIcon, "<b>DEBUGGER</b>");
		}
		GUI.matrix = matrix;
		GUI.skin = skin;
	}

	public void ResetLayout()
	{
		IconRect = DefaultIconRect;
		WindowRect = DefaultWindowRect;
		WindowScale = 1f;
	}

	public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow)
	{
		debugger.RegisterDebuggerWindow(path, debuggerWindow);
	}

	public bool UnregisterDebuggerWindow(string path)
	{
		return debugger.UnregisterDebuggerWindow(path);
	}

	public IDebuggerWindow GetDebuggerWindow(string path)
	{
		return debugger.GetDebuggerWindow(path);
	}

	public bool SelectDebuggerWindow(string path)
	{
		return debugger.SelectDebuggerWindow(path);
	}

	private void DrawWindow(int windowId)
	{
		GUI.DragWindow(_dragRect);
		DrawDebuggerWindowGroup(debugger.DebuggerWindowRoot);
	}

	private void DrawDebuggerWindowGroup(DebuggerWindowGroup debuggerWindowGroup)
	{
		if (debuggerWindowGroup == null)
		{
			return;
		}
		List<string> list = new List<string>();
		string[] debuggerWindowNames = debuggerWindowGroup.GetDebuggerWindowNames();
		for (int i = 0; i < debuggerWindowNames.Length; i++)
		{
			list.Add($"<b>{debuggerWindowNames[i]}</b>");
		}
		if (debuggerWindowGroup == debugger.DebuggerWindowRoot)
		{
			list.Add("<b>Close</b>");
		}
		int num = GUILayout.Toolbar(debuggerWindowGroup.SelectedIndex, list.ToArray(), GUILayout.Height(30f), GUILayout.MaxWidth(Screen.width));
		if (num >= debuggerWindowGroup.DebuggerWindowCount)
		{
			ShowFullWindow = false;
		}
		else if (debuggerWindowGroup.SelectedWindow != null)
		{
			if (debuggerWindowGroup.SelectedIndex != num)
			{
				debuggerWindowGroup.SelectedWindow.OnLeave();
				debuggerWindowGroup.SelectedIndex = num;
				debuggerWindowGroup.SelectedWindow.OnEnter();
			}
			if (debuggerWindowGroup.SelectedWindow is DebuggerWindowGroup debuggerWindowGroup2)
			{
				DrawDebuggerWindowGroup(debuggerWindowGroup2);
			}
			debuggerWindowGroup.SelectedWindow.OnDraw();
		}
	}

	private void DrawDebuggerWindowIcon(int windowId)
	{
		GUI.DragWindow(_dragRect);
		GUILayout.Space(5f);
		Color32 color = Color.white;
		ConsoleWindow consoleWindow = debugger.ConsoleWindow;
		consoleWindow.RefreshCount();
		color = ((consoleWindow.ExceptionCount > 0) ? consoleWindow.GetLogStringColor(LogType.Exception) : ((consoleWindow.ErrorCount > 0) ? consoleWindow.GetLogStringColor(LogType.Error) : ((consoleWindow.WarningCount <= 0) ? consoleWindow.GetLogStringColor(LogType.Log) : consoleWindow.GetLogStringColor(LogType.Warning))));
		if (GUILayout.Button(string.Format("<color=#{0}{1}{2}{3}><b>FPS: {4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), _fpsCounter.CurrentFps.ToString("F2")), GUILayout.Width(100f), GUILayout.Height(40f)))
		{
			ShowFullWindow = true;
		}
	}

	private static void CopyToClipboard(string content)
	{
		TextEditor.text = content;
		TextEditor.OnFocus();
		TextEditor.Copy();
		TextEditor.text = string.Empty;
	}
}
