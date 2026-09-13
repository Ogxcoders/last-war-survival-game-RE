using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FibMatrix.PerfTools;

[ExecuteAlways]
public class OverdrawMonitorManager : MonoBehaviour
{
	private List<OverdrawMonitor> m_Monitors = new List<OverdrawMonitor>();

	private List<Canvas> m_RootCanvases = new List<Canvas>();

	private StringBuilder m_TextBuilder = new StringBuilder(512);

	private List<ProfilingChartRenderer_CameraOverdraw> m_ChartRendererOverdraws = new List<ProfilingChartRenderer_CameraOverdraw>();

	private ProfilingChartRenderer_SummaryOverdraw m_ChartRendererOverdrawSummary;

	private ProfilingChartRenderer_Particle m_ChartRendererParticle;

	private ParticleMonitor m_ParticleMonitor;

	private bool m_ChartFoldout = true;

	public static bool DisableGUIOnGameView;

	public static bool ForceGetODResultInSyncMode;

	private float m_RecordIntervalTimeAcc;

	private static OverdrawMonitorManager s_Instance;

	private const string k_GoName = "OverdrawMonitorManagerGo";

	internal const float DisabledCameraOverdrawValue = -1f;

	public static OverdrawMonitorManager Instance
	{
		get
		{
			if (!Application.isEditor)
			{
				Debug.LogError("overdraw monitor can only be used in editor");
				return null;
			}
			if (s_Instance == null)
			{
				GameObject gameObject = GameObject.Find("OverdrawMonitorManagerGo");
				if (gameObject == null)
				{
					gameObject = new GameObject("OverdrawMonitorManagerGo");
					if (Application.isPlaying)
					{
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
					else
					{
						gameObject.hideFlags = HideFlags.DontSave;
					}
				}
				OverdrawMonitorManager overdrawMonitorManager = gameObject.GetComponent<OverdrawMonitorManager>();
				if (overdrawMonitorManager == null)
				{
					overdrawMonitorManager = gameObject.AddComponent<OverdrawMonitorManager>();
				}
				s_Instance = overdrawMonitorManager;
			}
			return s_Instance;
		}
	}

	public static string SupportedRTFormat
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int value in Enum.GetValues(typeof(RenderTextureFormat)))
			{
				string arg = Enum.GetName(typeof(RenderTextureFormat), value);
				bool flag = SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat)value);
				stringBuilder.AppendFormat("{0}:{1} \n", arg, flag);
			}
			return stringBuilder.ToString();
		}
	}

	private void OnDestroy()
	{
		m_RootCanvases?.Clear();
		if (m_ChartRendererOverdraws != null)
		{
			foreach (ProfilingChartRenderer_CameraOverdraw chartRendererOverdraw in m_ChartRendererOverdraws)
			{
				chartRendererOverdraw.UninitializeRenderer();
			}
			m_ChartRendererOverdraws.Clear();
		}
		if (m_ChartRendererOverdrawSummary != null)
		{
			m_ChartRendererOverdrawSummary.UninitializeRenderer();
			m_ChartRendererOverdrawSummary = null;
		}
		if (m_ChartRendererParticle != null)
		{
			m_ChartRendererParticle.UninitializeRenderer();
			m_ChartRendererParticle = null;
		}
		if (m_Monitors != null)
		{
			foreach (OverdrawMonitor monitor in m_Monitors)
			{
				monitor.Dispose();
			}
			m_Monitors.Clear();
		}
		if (m_ParticleMonitor != null)
		{
			m_ParticleMonitor.Dispose();
			m_ParticleMonitor = null;
		}
	}

	public void Update()
	{
		UpdateUIRootCanvas();
		for (int num = m_Monitors.Count - 1; num >= 0; num--)
		{
			if (m_Monitors[num] == null)
			{
				m_Monitors.RemoveAt(num);
			}
			else if (m_Monitors[num].IsDead())
			{
				string text = m_Monitors[num].name;
				m_Monitors[num].Dispose();
				m_Monitors.RemoveAt(num);
				ReleaseAndRemoveProfilingChartFromList(text, m_ChartRendererOverdraws);
			}
		}
		for (int num2 = m_ChartRendererOverdraws.Count - 1; num2 >= 0; num2--)
		{
			if (!m_ChartRendererOverdraws[num2].IsMonitorValid())
			{
				m_ChartRendererOverdraws.RemoveAt(num2);
			}
		}
		OverdrawMonitorCfg overdrawMonitorConfig = PerfToolsRuntimeCfg.Instance.overdrawMonitorConfig;
		Camera[] allCameras = Camera.allCameras;
		foreach (Camera cam in allCameras)
		{
			if (!OverdrawMonitor.DoesCameraSkipRendering(cam))
			{
				OverdrawMonitor monitor = m_Monitors.Find((OverdrawMonitor m) => m.IsTargetCamera(cam) || m.IsSelfCamera(cam));
				if (monitor == null)
				{
					monitor = OverdrawMonitor.GetOverdrawMonitor(cam, overdrawMonitorConfig);
					monitor.transform.SetParent(base.transform, worldPositionStays: true);
					m_Monitors.Add(monitor);
				}
				if (m_ChartRendererOverdraws.Find((ProfilingChartRenderer_CameraOverdraw x) => x.HasSameMontor(monitor)) == null)
				{
					CDOverdrawChart config = ProfilingChartRenderer_CameraOverdraw.FetchConfig(overdrawMonitorConfig.overdrawChartDefault, overdrawMonitorConfig.overdrawChartCustom, monitor.GetTargetCameraName());
					ProfilingChartRenderer_CameraOverdraw profilingChartRenderer_CameraOverdraw = new ProfilingChartRenderer_CameraOverdraw(monitor.name, config, overdrawMonitorConfig.chartAppearance, monitor);
					profilingChartRenderer_CameraOverdraw.InitializeRenderer();
					m_ChartRendererOverdraws.Add(profilingChartRenderer_CameraOverdraw);
				}
			}
		}
		if (m_ChartRendererOverdrawSummary == null)
		{
			string text2 = "OverdrawSummary";
			m_ChartRendererOverdrawSummary = new ProfilingChartRenderer_SummaryOverdraw(text2, overdrawMonitorConfig.overdrawSummaryChart, overdrawMonitorConfig.chartAppearance, m_Monitors);
			m_ChartRendererOverdrawSummary.InitializeRenderer();
		}
		if (m_ParticleMonitor == null)
		{
			m_ParticleMonitor = ParticleMonitor.GetMonitor(base.transform, overdrawMonitorConfig.particleMonitor);
		}
		if (m_ChartRendererParticle == null)
		{
			m_ChartRendererParticle = new ProfilingChartRenderer_Particle(m_ParticleMonitor.name, overdrawMonitorConfig.particleChart, overdrawMonitorConfig.chartAppearance, m_ParticleMonitor);
			m_ChartRendererParticle.InitializeRenderer();
		}
	}

	private void LateUpdate()
	{
		UpdateProfilingChartFrame();
	}

	private void UpdateUIRootCanvas()
	{
		List<string> rootUICanvasPath = PerfToolsRuntimeCfg.Instance.overdrawMonitorConfig.rootUICanvasPath;
		if (rootUICanvasPath != null && rootUICanvasPath.Count != m_RootCanvases.Count)
		{
			m_RootCanvases.Clear();
			foreach (string item in rootUICanvasPath)
			{
				m_RootCanvases.Add(GameObject.Find(item)?.GetComponent<Canvas>());
			}
		}
		for (int i = 0; i < m_RootCanvases.Count; i++)
		{
			Canvas canvas = m_RootCanvases[i];
			if (canvas == null)
			{
				canvas = GameObject.Find(rootUICanvasPath[i])?.GetComponent<Canvas>();
				m_RootCanvases[i] = canvas;
			}
		}
	}

	public bool TryGetUIRootCanvas(Camera targetCamera, out Canvas canvas)
	{
		canvas = null;
		if (m_RootCanvases == null)
		{
			return false;
		}
		bool result = false;
		int num = 0;
		for (int i = 0; i < m_RootCanvases.Count; i++)
		{
			Canvas canvas2 = m_RootCanvases[i];
			if (!(canvas2 == null) && canvas2.renderMode != RenderMode.WorldSpace && !(canvas2.worldCamera != targetCamera))
			{
				canvas = canvas2;
				result = true;
				num++;
			}
		}
		if (num > 1)
		{
			Debug.LogError("multiple root canvas for one camera is not supported, camera:" + targetCamera.name);
		}
		return result;
	}

	private bool ReleaseAndRemoveProfilingChartFromList<T>(string name, List<T> chartRenderers) where T : ProfilingChartRenderer
	{
		bool result = false;
		int count = chartRenderers.Count;
		for (int i = 0; i < count; i++)
		{
			ProfilingChartRenderer profilingChartRenderer = chartRenderers[i];
			if (profilingChartRenderer.isSelfName(name))
			{
				chartRenderers.RemoveAt(i);
				profilingChartRenderer.UninitializeRenderer();
				result = true;
				break;
			}
		}
		return result;
	}

	private void UpdateProfilingChartFrame()
	{
		OverdrawMonitorCfg overdrawMonitorConfig = PerfToolsRuntimeCfg.Instance.overdrawMonitorConfig;
		float chartUpdateInterval = overdrawMonitorConfig.chartUpdateInterval;
		bool flag = false;
		m_RecordIntervalTimeAcc += Time.deltaTime;
		if (m_RecordIntervalTimeAcc > chartUpdateInterval)
		{
			m_RecordIntervalTimeAcc -= chartUpdateInterval;
			flag = true;
		}
		if (overdrawMonitorConfig.forceUpdateEveryFrame)
		{
			flag = true;
		}
		if (m_ChartRendererOverdraws != null)
		{
			foreach (ProfilingChartRenderer_CameraOverdraw chartRendererOverdraw in m_ChartRendererOverdraws)
			{
				chartRendererOverdraw.DoUpdateFrame();
				if (flag)
				{
					chartRendererOverdraw.DoRecord();
					chartRendererOverdraw.DoRender();
				}
			}
		}
		if (m_ChartRendererOverdrawSummary != null)
		{
			m_ChartRendererOverdrawSummary.DoUpdateFrame();
			if (flag)
			{
				m_ChartRendererOverdrawSummary.DoRecord();
				m_ChartRendererOverdrawSummary.DoRender();
			}
		}
		if (m_ChartRendererParticle != null)
		{
			m_ChartRendererParticle.DoUpdateFrame();
			if (flag)
			{
				m_ChartRendererParticle.DoRecord();
				m_ChartRendererParticle.DoRender();
			}
		}
	}

	public void SetAllOverdrawAsTransparent()
	{
		foreach (OverdrawMonitor monitor in m_Monitors)
		{
			if (!monitor.IsDead())
			{
				monitor.SetReplacementTag(null);
			}
		}
	}

	public void SetAllOverdrawAsDefault()
	{
		foreach (OverdrawMonitor monitor in m_Monitors)
		{
			if (!monitor.IsDead())
			{
				monitor.ResetReplacementTag();
			}
		}
	}

	public bool IsOverdrawUsingTransparentTag()
	{
		foreach (OverdrawMonitor monitor in m_Monitors)
		{
			if (!monitor.IsDead())
			{
				return monitor.GetCurReplacementTag() == null;
			}
		}
		return false;
	}

	public string GetShowText()
	{
		if (m_TextBuilder == null)
		{
			m_TextBuilder = new StringBuilder(512);
		}
		m_TextBuilder.Clear();
		if (m_ParticleMonitor != null)
		{
			m_TextBuilder.AppendLine($"P-Particle total: {m_ParticleMonitor.particleCount}");
		}
		foreach (OverdrawMonitor monitor in m_Monitors)
		{
			if (!monitor.IsDead())
			{
				m_TextBuilder.AppendLine($"O-{monitor.GetTargetCameraName()}: {monitor.LastFrameOverdraw:F1}");
			}
		}
		return m_TextBuilder.ToString();
	}

	public OverdrawMonitor FindOverdrawMonitor(Camera tergetCamera)
	{
		if (tergetCamera == null)
		{
			return null;
		}
		OverdrawMonitor result = null;
		foreach (OverdrawMonitor monitor in m_Monitors)
		{
			if (monitor.IsTargetCamera(tergetCamera))
			{
				result = monitor;
				break;
			}
		}
		return result;
	}

	public void RenderGUI()
	{
		bool disableGUIOnGameView = DisableGUIOnGameView;
		GUILayout.BeginVertical();
		GUILayout.BeginVertical("粒子", "box");
		GUILayout.Space(12f);
		if (m_ChartRendererParticle != null)
		{
			m_ChartRendererParticle.DoGUI(disableGUIOnGameView, m_ChartFoldout);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical("Overdraw", "box");
		GUILayout.Space(12f);
		if (m_ChartRendererOverdrawSummary != null)
		{
			m_ChartRendererOverdrawSummary.DoGUI(disableGUIOnGameView, m_ChartFoldout);
		}
		if (m_ChartRendererOverdraws != null)
		{
			foreach (ProfilingChartRenderer_CameraOverdraw chartRendererOverdraw in m_ChartRendererOverdraws)
			{
				chartRendererOverdraw.DoGUI(disableGUIOnGameView, m_ChartFoldout);
			}
		}
		GUILayout.EndVertical();
		if (GUILayout.Button(m_ChartFoldout ? "+" : "-"))
		{
			m_ChartFoldout = !m_ChartFoldout;
		}
		GUILayout.EndVertical();
	}

	public static bool IsMonitoring()
	{
		if (s_Instance != null)
		{
			return s_Instance.isActiveAndEnabled;
		}
		return false;
	}

	public static bool EnableMonitering()
	{
		bool flag = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Default);
		bool supportsComputeShaders = SystemInfo.supportsComputeShaders;
		if (!flag || !supportsComputeShaders)
		{
			Debug.LogError($"支持overdrawRT格式:{flag}, 支持compute shader:{supportsComputeShaders}");
			return false;
		}
		Instance.gameObject.SetActive(value: true);
		return true;
	}

	public static void DisableMonitering()
	{
		if (!(s_Instance == null))
		{
			Instance.gameObject.SetActive(value: false);
		}
	}

	public static float GetAllCameraTotalOverdraw()
	{
		if (IsMonitoring())
		{
			float num = 0f;
			{
				foreach (OverdrawMonitor monitor in s_Instance.m_Monitors)
				{
					num += monitor.LastFrameOverdraw;
				}
				return num;
			}
		}
		return -1f;
	}

	public static float GetMainCameraOverdraw()
	{
		if (IsMonitoring())
		{
			foreach (OverdrawMonitor monitor in s_Instance.m_Monitors)
			{
				if (monitor.IsTargetCamera(Camera.main))
				{
					return monitor.LastFrameOverdraw;
				}
			}
		}
		return -1f;
	}

	public static float GetCameraOverdrawByName(string camName)
	{
		if (IsMonitoring())
		{
			foreach (OverdrawMonitor monitor in s_Instance.m_Monitors)
			{
				if (monitor.GetTargetCameraName() == camName)
				{
					return monitor.LastFrameOverdraw;
				}
			}
		}
		return -1f;
	}

	public static void DestroyInstance()
	{
		OverdrawMonitorManager[] array = Resources.FindObjectsOfTypeAll(typeof(OverdrawMonitorManager)) as OverdrawMonitorManager[];
		foreach (OverdrawMonitorManager overdrawMonitorManager in array)
		{
			if (true)
			{
				UnityEngine.Object.DestroyImmediate(overdrawMonitorManager.gameObject);
			}
		}
		s_Instance = null;
	}

	public static bool IsOverdrawUsingTransparent()
	{
		return Instance.IsOverdrawUsingTransparentTag();
	}

	public static void SetOverdrawUsingTransparent(bool isUse)
	{
		if (isUse)
		{
			Instance.SetAllOverdrawAsTransparent();
		}
		else
		{
			Instance.SetAllOverdrawAsDefault();
		}
	}
}
