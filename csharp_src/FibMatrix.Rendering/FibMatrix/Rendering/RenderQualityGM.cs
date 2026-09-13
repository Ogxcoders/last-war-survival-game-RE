using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace FibMatrix.Rendering;

public class RenderQualityGM : MonoBehaviour
{
	private class ScaleSettingItem
	{
		public int min;

		public int max;

		public float scale;
	}

	private class ScaleSetting
	{
		public ScaleSettingItem org;

		public ScaleSettingItem now;
	}

	private class KeywordsToggleString
	{
		public string on;

		public string off;
	}

	private int size = 48;

	private int oldScreenWidth;

	private string sizeStr = 48.ToString();

	private GUIStyle m_Style;

	private GUILayoutOption[] m_SliderOption;

	private GUILayoutOption[] m_ThreeLineOption;

	private EnQualityLevel m_Level = EnQualityLevel.Unknown;

	private const int TimesCount = 64;

	private Queue<float> m_Times = new Queue<float>(64);

	private RenderPipelineCallback m_RenderPipelineCallback;

	private Stopwatch m_Stopwatch;

	private Dictionary<Camera, Queue<double>> m_CameraDurations = new Dictionary<Camera, Queue<double>>();

	private StringBuilder m_SharedStringBuilder = new StringBuilder(64);

	private bool m_GMToggle;

	private Dictionary<string, VolumeComponent> m_PostProcesses;

	private bool m_PostProcessToggle;

	private Dictionary<string, ScriptableRendererFeature> m_Features;

	private bool m_FeaturesToggle;

	private bool m_RenderingLayerMaskToggle;

	private Dictionary<UniversalRenderPipelineAsset, ScaleSetting> m_ScaleSettingDictionary = new Dictionary<UniversalRenderPipelineAsset, ScaleSetting>(5);

	private bool m_RenderingScaleToggle;

	private Dictionary<string, KeywordsToggleString> m_WaterShaderKeywords = new Dictionary<string, KeywordsToggleString>
	{
		{
			"_PERFORMANCE_CAUSTICS_OFF",
			new KeywordsToggleString
			{
				on = "关闭焦散",
				off = "开启焦散"
			}
		},
		{
			"_PERFORMANCE_DISTORTION_OFF",
			new KeywordsToggleString
			{
				on = "关闭扰动",
				off = "开启扰动"
			}
		},
		{
			"_PERFORMANCE_FOAM_OFF",
			new KeywordsToggleString
			{
				on = "关闭白沫",
				off = "开启白沫"
			}
		},
		{
			"_PERFORMANCE_FRESNEL_OFF",
			new KeywordsToggleString
			{
				on = "关闭菲涅尔",
				off = "开启菲涅尔"
			}
		},
		{
			"_PERFORMANCE_SPECULAR_OFF",
			new KeywordsToggleString
			{
				on = "关闭高光",
				off = "开启高光"
			}
		},
		{
			"_PERFORMANCE_SSS_OFF",
			new KeywordsToggleString
			{
				on = "关闭次表面散射",
				off = "开启次表面散射"
			}
		}
	};

	private bool m_WaterShaderKeywordsToggle;

	private GameObject _runtimeInspectorHierarchy;

	private void Start()
	{
		Application.targetFrameRate = 999;
	}

	private void OnEnable()
	{
		RefreshPostProcessSetting();
		RefreshRenderFeatureSetting();
		RefreshRenderTextureScaleSetting();
		RenderPipelineCallbackUtilities.GetOrCreateRenderPipelineCallback(base.gameObject, ref m_RenderPipelineCallback);
		m_RenderPipelineCallback.ActionBeginCameraRendering -= ActionBeforeRender;
		m_RenderPipelineCallback.ActionBeginCameraRendering += ActionBeforeRender;
		m_RenderPipelineCallback.ActionEndCameraRendering -= ActionAfterRender;
		m_RenderPipelineCallback.ActionEndCameraRendering += ActionAfterRender;
	}

	private void RefreshPostProcessSetting()
	{
		if (m_PostProcesses == null)
		{
			m_PostProcesses = new Dictionary<string, VolumeComponent>(4);
		}
		m_PostProcesses.Clear();
		if (!(typeof(VolumeManager).GetField("m_Volumes", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(VolumeManager.instance) is List<Volume> { Count: >0 } list))
		{
			return;
		}
		foreach (VolumeComponent component in list[list.Count - 1].sharedProfile.components)
		{
			m_PostProcesses[component.name] = component;
		}
	}

	private void RefreshRenderFeatureSetting()
	{
		if (m_Features == null)
		{
			m_Features = new Dictionary<string, ScriptableRendererFeature>(8);
		}
		m_Features.Clear();
		foreach (ScriptableRendererFeature rendererFeatures in RenderQualitySetting.ScriptableRenderer.GetRendererFeaturesList())
		{
			m_Features[rendererFeatures.name] = rendererFeatures;
		}
	}

	private void RefreshRenderTextureScaleSetting()
	{
		if (!m_ScaleSettingDictionary.Any())
		{
			for (int i = 0; i < 5; i++)
			{
				UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.GetRenderPipelineAssetAt(i) as UniversalRenderPipelineAsset;
				m_ScaleSettingDictionary[universalRenderPipelineAsset] = new ScaleSetting
				{
					org = new ScaleSettingItem
					{
						min = universalRenderPipelineAsset.minRenderResolution,
						max = universalRenderPipelineAsset.maxRenderResolution,
						scale = universalRenderPipelineAsset.renderScale
					},
					now = new ScaleSettingItem
					{
						min = universalRenderPipelineAsset.minRenderResolution,
						max = universalRenderPipelineAsset.maxRenderResolution,
						scale = universalRenderPipelineAsset.renderScale
					}
				};
			}
		}
	}

	private void OnDisable()
	{
		m_Features?.Clear();
		m_RenderPipelineCallback.ActionBeginCameraRendering -= ActionBeforeRender;
		m_RenderPipelineCallback.ActionEndCameraRendering -= ActionAfterRender;
		foreach (KeyValuePair<string, KeywordsToggleString> waterShaderKeyword in m_WaterShaderKeywords)
		{
			Shader.DisableKeyword(waterShaderKeyword.Key);
		}
	}

	private void Update()
	{
		OnInspectorHierarchyUpdate();
	}

	private void ActionBeforeRender(ScriptableRenderContext context, Camera camera)
	{
		m_Stopwatch = Stopwatch.StartNew();
	}

	private void ActionAfterRender(ScriptableRenderContext context, Camera camera)
	{
		if (!m_CameraDurations.TryGetValue(camera, out var value))
		{
			value = new Queue<double>();
			m_CameraDurations.Add(camera, value);
		}
		value.Enqueue(m_Stopwatch.Elapsed.TotalMilliseconds);
	}

	private void InitGUI()
	{
		if (oldScreenWidth != Screen.width)
		{
			oldScreenWidth = Screen.width;
			size = (int)(2f / 45f * (float)oldScreenWidth);
			ResetGUI();
		}
		if (m_Style == null)
		{
			m_Style = new GUIStyle(GUI.skin.button);
			m_Style.fontSize = size;
		}
		if (m_SliderOption == null)
		{
			m_SliderOption = new GUILayoutOption[2]
			{
				GUILayout.Height(size),
				GUILayout.Width(size * 16)
			};
		}
		if (m_ThreeLineOption == null)
		{
			m_ThreeLineOption = new GUILayoutOption[1] { GUILayout.Height(size * 3) };
		}
	}

	private void ResetGUI()
	{
		m_Style = null;
		m_SliderOption = null;
		m_ThreeLineOption = null;
	}

	private void OnGUI()
	{
		InitGUI();
		if (m_Level == EnQualityLevel.Unknown)
		{
			m_Level = (EnQualityLevel)QualitySettings.GetQualityLevel();
			RenderQualitySetting.SwitchQualityLevel(m_Level);
		}
		GUILayout.BeginVertical();
		GUILayout.Space(size * 2);
		GUILayout.BeginHorizontal();
		m_GMToggle = GUILayout.Toggle(m_GMToggle, "GM", m_Style);
		if (!m_GMToggle)
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			return;
		}
		if (GUILayout.Button(m_Level.ToString(), m_Style))
		{
			if (++m_Level > EnQualityLevel.High)
			{
				m_Level = EnQualityLevel.Low;
			}
			RenderQualitySetting.SwitchQualityLevel(m_Level);
		}
		if (GUILayout.Button("Hierarchy", m_Style))
		{
			OnInspectorHierarchy();
		}
		if (GUILayout.Button("RenderTiming", m_Style))
		{
			if (RenderTiming.instance == null)
			{
				base.gameObject.AddComponent<RenderTiming>();
			}
			else
			{
				RenderTiming.instance.enabled = !RenderTiming.instance.enabled;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (RenderTiming.instance != null && RenderTiming.instance.isSupported)
		{
			if (RenderTiming.instance.deltaTime != float.NaN)
			{
				float item = RenderTiming.instance.deltaTime * 1000f;
				if (m_Times.Count < 64)
				{
					m_Times.Enqueue(item);
				}
				else
				{
					m_Times.Dequeue();
					m_Times.Enqueue(item);
				}
			}
			GUILayout.Label($"gpu {(m_Times.Any() ? m_Times.Average() : 0f),8:f3}", m_Style);
		}
		GUILayout.EndHorizontal();
		if (GUILayout.Button("post processes", m_Style))
		{
			m_PostProcessToggle = !m_PostProcessToggle;
			RefreshPostProcessSetting();
		}
		if (m_PostProcessToggle && RenderQualitySetting.RequirePostProcess)
		{
			GUILayout.BeginVertical();
			foreach (KeyValuePair<string, VolumeComponent> postProcess in m_PostProcesses)
			{
				GUILayout.BeginHorizontal();
				GUILayout.TextArea(postProcess.Value.active ? "√" : "×", m_Style);
				if (GUILayout.Button(postProcess.Key, m_Style))
				{
					postProcess.Value.active = !postProcess.Value.active;
				}
				GUILayout.EndVertical();
			}
			TiltShift tiltShift = m_PostProcesses.FirstOrDefault((KeyValuePair<string, VolumeComponent> kv) => kv.Value is TiltShift).Value as TiltShift;
			if (tiltShift != null && tiltShift.active)
			{
				int value = tiltShift.debug.value;
				int num = GUILayout.SelectionGrid(tiltShift.debug.value, new GUIContent[3]
				{
					new GUIContent("mesh"),
					new GUIContent("viewport"),
					new GUIContent("scissor")
				}, 3, m_Style);
				tiltShift.debug.value = num;
				tiltShift.debug.overrideState |= value != num;
				tiltShift.blurStart.value = GUILayout.HorizontalSlider(tiltShift.blurStart.value, 0f, 1f, m_SliderOption);
			}
			GUILayout.EndVertical();
		}
		if (GUILayout.Button("renderer features", m_Style))
		{
			m_FeaturesToggle = !m_FeaturesToggle;
		}
		if (m_FeaturesToggle)
		{
			GUILayout.BeginVertical();
			foreach (KeyValuePair<string, ScriptableRendererFeature> feature in m_Features)
			{
				GUILayout.BeginHorizontal();
				GUILayout.TextArea(feature.Value.isActive ? "√" : "×", m_Style);
				if (GUILayout.Button(feature.Key, m_Style))
				{
					feature.Value.SetActive(!feature.Value.isActive);
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();
		}
		if (GUILayout.Button("rendering layer mask", m_Style))
		{
			m_RenderingLayerMaskToggle = !m_RenderingLayerMaskToggle;
		}
		if (m_RenderingLayerMaskToggle)
		{
			GUILayout.BeginVertical();
			for (int num2 = 0; num2 < 32; num2++)
			{
				GUILayout.Label($"{num2}: {RenderingLayerMask.LayerToName(num2)}", m_Style);
			}
			GUILayout.EndVertical();
		}
		if (GUILayout.Button("rendering scale", m_Style))
		{
			m_RenderingScaleToggle = !m_RenderingScaleToggle;
		}
		if (m_RenderingScaleToggle)
		{
			ScaleSettingItem org = m_ScaleSettingDictionary[RenderQualitySetting.RenderPipelineAsset].org;
			ScaleSettingItem now = m_ScaleSettingDictionary[RenderQualitySetting.RenderPipelineAsset].now;
			GUILayout.BeginHorizontal();
			GUILayout.Label(32f.ToString(), m_Style, m_ThreeLineOption);
			GUILayout.BeginVertical();
			now.min = Mathf.Min(now.max, (int)GUILayout.HorizontalSlider(now.min, 32f, Screen.width, m_SliderOption));
			now.max = Mathf.Max(now.min, (int)GUILayout.HorizontalSlider(now.max, 32f, Screen.width, m_SliderOption));
			now.scale = GUILayout.HorizontalSlider(now.scale, 0.01f, 1f, m_SliderOption);
			GUILayout.EndVertical();
			GUILayout.Label(Screen.width.ToString(), m_Style, m_ThreeLineOption);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("重置", m_Style))
			{
				RenderQualitySetting.RenderPipelineAsset.minRenderResolution = org.min;
				RenderQualitySetting.RenderPipelineAsset.maxRenderResolution = org.max;
				RenderQualitySetting.RenderPipelineAsset.renderScale = org.scale;
				now.min = org.min;
				now.max = org.max;
				now.scale = org.scale;
			}
			GUILayout.Label(Mathf.Clamp((float)Screen.width * now.scale, now.min, now.max).ToString(), m_Style);
			if (GUILayout.Button("确认", m_Style))
			{
				RenderQualitySetting.RenderPipelineAsset.minRenderResolution = now.min;
				RenderQualitySetting.RenderPipelineAsset.maxRenderResolution = now.max;
				RenderQualitySetting.RenderPipelineAsset.renderScale = now.scale;
			}
			GUILayout.EndHorizontal();
		}
		m_WaterShaderKeywordsToggle = GUILayout.Toggle(m_WaterShaderKeywordsToggle, "water performance", m_Style);
		if (m_WaterShaderKeywordsToggle)
		{
			foreach (KeyValuePair<string, KeywordsToggleString> waterShaderKeyword in m_WaterShaderKeywords)
			{
				bool num3 = Shader.IsKeywordEnabled(waterShaderKeyword.Key);
				if (GUILayout.Toggle(num3, num3 ? waterShaderKeyword.Value.off : waterShaderKeyword.Value.on, m_Style))
				{
					Shader.EnableKeyword(waterShaderKeyword.Key);
				}
				else
				{
					Shader.DisableKeyword(waterShaderKeyword.Key);
				}
			}
		}
		GUILayout.EndVertical();
	}

	private void OnInspectorHierarchy()
	{
		if (_runtimeInspectorHierarchy == null)
		{
			GameObject gameObject = Object.FindObjectOfType<CanvasScaler>()?.gameObject;
			_ = gameObject != null;
			if (gameObject != null)
			{
				_runtimeInspectorHierarchy = Object.Instantiate(Resources.Load("RuntimeInspectorHierarchy"), gameObject.transform) as GameObject;
			}
		}
		else
		{
			_runtimeInspectorHierarchy.SetActive(!_runtimeInspectorHierarchy.activeSelf);
		}
	}

	private void OnInspectorHierarchyUpdate()
	{
		if (_runtimeInspectorHierarchy != null && _runtimeInspectorHierarchy.transform.GetSiblingIndex() != _runtimeInspectorHierarchy.transform.parent.childCount - 1)
		{
			_runtimeInspectorHierarchy.transform.SetAsLastSibling();
		}
	}
}
