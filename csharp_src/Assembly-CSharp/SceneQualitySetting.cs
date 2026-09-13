using System;
using System.Collections.Generic;
using System.Reflection;
using FibMatrix.Rendering;
using FibMatrix.RenderingHotUpdatable;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public static class SceneQualitySetting
{
	private static int pixelHeightMax = 1080;

	public static void ChangeQualitySetting()
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.ChangeQualitySetting();
		}
		SetResolutionQuality();
	}

	public static float GetScale()
	{
		return 1f;
	}

	private static bool CheckLowMemory()
	{
		if (SystemInfo.systemMemorySize <= 5000)
		{
			return true;
		}
		return false;
	}

	public static void SetResolutionQuality()
	{
		float num = 1f;
		if (Screen.height > pixelHeightMax)
		{
			num = (float)pixelHeightMax / (float)Screen.height;
		}
		int num2 = GameEntry.Setting.GetInt("QualitySetting.Resolution", 3);
		UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;
		if (CheckLowMemory())
		{
			universalRenderPipelineAsset.supportsHDR = false;
		}
		switch (num2)
		{
		case 1:
			if (SystemInfo.systemMemorySize <= 4000)
			{
				int width = (int)Math.Round((float)Screen.width * num);
				int height = (int)Math.Round((float)Screen.height * num);
				Screen.SetResolution(width, height, fullscreen: true);
			}
			universalRenderPipelineAsset.renderScale = num * GetScale();
			break;
		case 3:
			universalRenderPipelineAsset.renderScale = num * GetScale();
			break;
		}
	}

	public static void SetPixelHeightMax(int heightMax)
	{
		pixelHeightMax = heightMax;
	}

	public static int GetGraphicLevel()
	{
		int num = GameEntry.Setting.GetInt("GAME_QUALITY_CONFIG_KEY", -1);
		if (num >= 1 && num <= 3)
		{
			return num;
		}
		return GameEntry.Setting.GetInt("SCENE_GRAPHIC_LEVEL", 2);
	}

	public static void ApplySavedGraphicsLevel()
	{
		TryChangeFeatureToGpuSkinCompatable();
		ApplyGraphicsLevelNotSaved(GetGraphicLevel());
	}

	public static void TryChangeFeatureToGpuSkinCompatable()
	{
		if (!Debug.isDebugBuild && !ClientSwitch.IsOn(35))
		{
			return;
		}
		List<CustomRenderObjectsFeatureHotUpdatable> list = new List<CustomRenderObjectsFeatureHotUpdatable>();
		foreach (ScriptableRendererFeature rendererFeatures in RenderQualitySetting.ScriptableRenderer.GetRendererFeaturesList())
		{
			if (rendererFeatures.GetType() == typeof(CustomRenderObjectsFeatureHotUpdatable))
			{
				continue;
			}
			RenderObjects renderObjects = rendererFeatures as RenderObjects;
			if (!(renderObjects == null))
			{
				string text = rendererFeatures.name.ToLower();
				if ((text.Contains("shadow") && !text.EndsWith("GpuSkinCompatable") && !text.EndsWith("deprecated")) || (text.Contains("outline") && !text.EndsWith("GpuSkinCompatable") && !text.EndsWith("deprecated")))
				{
					CustomRenderObjectsFeatureHotUpdatable customRenderObjectsFeatureHotUpdatable = ScriptableObject.CreateInstance<CustomRenderObjectsFeatureHotUpdatable>();
					customRenderObjectsFeatureHotUpdatable.name = rendererFeatures.name + "GpuSkinCompatable";
					customRenderObjectsFeatureHotUpdatable.settings.Event = renderObjects.settings.Event;
					RenderObjects.FilterSettings filterSettings = renderObjects.settings.filterSettings;
					string[] passNames = ((!text.Contains("shadow")) ? new string[1] { "OutLineGPUSkin" } : new string[1] { "PlanarShadowGPUSkin" });
					customRenderObjectsFeatureHotUpdatable.settings.filterSettings = new CustomRenderObjectsFeatureHotUpdatable.FilterSettings
					{
						LayerMask = filterSettings.LayerMask,
						RenderQueueType = filterSettings.RenderQueueType,
						PassNames = passNames
					};
					customRenderObjectsFeatureHotUpdatable.settings.overrideMaterial = renderObjects.settings.overrideMaterial;
					customRenderObjectsFeatureHotUpdatable.settings.overrideGlobalProperties = true;
					list.Add(customRenderObjectsFeatureHotUpdatable);
					renderObjects.name = rendererFeatures.name + "deprecated";
					renderObjects.SetActive(active: false);
				}
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
		ScriptableRendererData[] obj = (ScriptableRendererData[])universalRenderPipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(universalRenderPipelineAsset);
		ScriptableRendererData scriptableRendererData = ((obj != null) ? obj[0] : null);
		if (!(scriptableRendererData != null))
		{
			return;
		}
		foreach (CustomRenderObjectsFeatureHotUpdatable item in list)
		{
			scriptableRendererData.rendererFeatures.Add(item);
		}
		scriptableRendererData.SetDirty();
	}

	internal static void ApplyGraphicsLevelNotSaved(int level)
	{
		if (level == 3)
		{
			Shader.EnableKeyword("_LOD_HIGH");
		}
		else
		{
			Shader.DisableKeyword("_LOD_HIGH");
		}
		foreach (ScriptableRendererFeature rendererFeatures in RenderQualitySetting.ScriptableRenderer.GetRendererFeaturesList())
		{
			string text = rendererFeatures.name.ToLower();
			if (!text.EndsWith("deprecated") && (text.Contains("shadow") || text.Contains("outline")))
			{
				rendererFeatures.SetActive(level != 1);
			}
		}
		QualitySettings.SetQualityLevel(level - 1);
		UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
		if (universalRenderPipelineAsset != null)
		{
			universalRenderPipelineAsset.supportsDynamicBatching = true;
		}
	}

	public static int GetTerrainLevel()
	{
		return GameEntry.Setting.GetInt("QualitySetting.Terrain", 1);
	}
}
