using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FibMatrix;
using FibMatrix.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class WorldInstancingRenderers
{
	private static List<IWorldGPUInstancingRenderer> allRenderers;

	public static WorldGPUInstancingRenderFeature RenderFeature { get; private set; }

	public static bool DeviceSupportInstancing { get; private set; }

	public static List<IWorldGPUInstancingRenderer> AllRenderers => allRenderers;

	public static LittleSmartGizmos GizmosHelper { get; private set; }

	public static bool ActiveGizmos => GizmosHelper != null;

	public static void Init()
	{
		if (RenderFeature != null)
		{
			RenderFeature.SetActive(active: true);
			return;
		}
		DeviceSupportInstancing = SystemInfo.supportsInstancing;
		foreach (ScriptableRendererFeature rendererFeatures in RenderQualitySetting.ScriptableRenderer.GetRendererFeaturesList())
		{
			if (rendererFeatures is WorldGPUInstancingRenderFeature)
			{
				RenderFeature = rendererFeatures as WorldGPUInstancingRenderFeature;
				break;
			}
		}
		if (RenderFeature == null)
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
			ScriptableRendererData[] obj = (ScriptableRendererData[])universalRenderPipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(universalRenderPipelineAsset);
			ScriptableRendererData scriptableRendererData = ((obj != null) ? obj[0] : null);
			if (scriptableRendererData != null)
			{
				WorldGPUInstancingRenderFeature worldGPUInstancingRenderFeature = ScriptableObject.CreateInstance<WorldGPUInstancingRenderFeature>();
				if (worldGPUInstancingRenderFeature != null)
				{
					worldGPUInstancingRenderFeature.name = "GPUInstancingRender[Dynamic]";
					scriptableRendererData.rendererFeatures.Insert(0, worldGPUInstancingRenderFeature);
					RenderFeature = worldGPUInstancingRenderFeature;
				}
			}
		}
		if ((bool)RenderFeature)
		{
			RenderFeature.SetActive(active: true);
		}
	}

	public static void Dispose()
	{
		if ((bool)RenderFeature)
		{
			RenderFeature.ClearAllPass();
			RenderFeature.SetActive(active: false);
		}
		allRenderers?.Clear();
	}

	public static void AddRenderer(IWorldGPUInstancingRenderer renderer)
	{
		if (!DeviceSupportInstancing)
		{
			FibMatrix.Logger.Warning("Add renderer failed. Device not support instancing!");
			return;
		}
		if (RenderFeature == null)
		{
			FibMatrix.Logger.Warning("Add renderer failed. Feature is null");
			return;
		}
		allRenderers = allRenderers ?? new List<IWorldGPUInstancingRenderer>();
		if (!allRenderers.Contains(renderer))
		{
			allRenderers.Add(renderer);
			RenderFeature.AddRenderer(renderer);
		}
		else
		{
			FibMatrix.Logger.Error("WorldInstancingRenderers.AddRenderer " + renderer.Name + " failed. Already exist.");
		}
	}

	public static void RemoveRenderer(IWorldGPUInstancingRenderer renderer)
	{
		if (renderer != null && allRenderers.Remove(renderer))
		{
			RenderFeature?.RemoveRenderer(renderer);
		}
	}

	public static bool SwitchGizmosVisible()
	{
		return false;
	}

	public static string Description()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("---WorldInstancingRenderers---");
		stringBuilder.AppendLine($"设备是否支持:{DeviceSupportInstancing}");
		stringBuilder.AppendLine($"Feature就绪:{RenderFeature != null}");
		stringBuilder.AppendLine($"已注册渲染器数量为{allRenderers?.Count}");
		List<IWorldGPUInstancingRenderer> list = allRenderers;
		if (list != null && list.Count > 0)
		{
			stringBuilder.AppendLine("----------");
			foreach (IWorldGPUInstancingRenderer allRenderer in allRenderers)
			{
				stringBuilder.AppendLine(allRenderer.Description());
			}
		}
		if (RenderFeature != null)
		{
			stringBuilder.AppendLine("查看Feature信息");
			stringBuilder.AppendLine(RenderFeature.Description());
		}
		return stringBuilder.ToString();
	}

	private static void OnGizmos()
	{
		if (allRenderers == null)
		{
			return;
		}
		foreach (IWorldGPUInstancingRenderer allRenderer in allRenderers)
		{
			allRenderer?.OnGizmos();
		}
	}

	private static void OnGizmosSelected()
	{
		if (allRenderers == null)
		{
			return;
		}
		foreach (IWorldGPUInstancingRenderer allRenderer in allRenderers)
		{
			allRenderer?.OnGizmosSelected();
		}
	}
}
