using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Experimental.Rendering.Universal;

internal class Render2DLightingPass : ScriptableRenderPass
{
	private static SortingLayer[] s_SortingLayers;

	private Renderer2DData m_Renderer2DData;

	private static readonly ShaderTagId k_CombinedRenderingPassNameOld = new ShaderTagId("Lightweight2D");

	private static readonly ShaderTagId k_CombinedRenderingPassName = new ShaderTagId("Universal2D");

	private static readonly ShaderTagId k_NormalsRenderingPassName = new ShaderTagId("NormalsRendering");

	private static readonly ShaderTagId k_LegacyPassName = new ShaderTagId("SRPDefaultUnlit");

	private static readonly List<ShaderTagId> k_ShaderTags = new List<ShaderTagId> { k_LegacyPassName, k_CombinedRenderingPassName, k_CombinedRenderingPassNameOld };

	public Render2DLightingPass(Renderer2DData rendererData)
	{
		if (s_SortingLayers == null)
		{
			s_SortingLayers = SortingLayer.layers;
		}
		m_Renderer2DData = rendererData;
	}

	public void GetTransparencySortingMode(Camera camera, ref SortingSettings sortingSettings)
	{
		TransparencySortMode transparencySortMode = camera.transparencySortMode;
		if (transparencySortMode == TransparencySortMode.Default)
		{
			transparencySortMode = m_Renderer2DData.transparencySortMode;
			if (transparencySortMode == TransparencySortMode.Default)
			{
				transparencySortMode = ((!camera.orthographic) ? TransparencySortMode.Perspective : TransparencySortMode.Orthographic);
			}
		}
		switch (transparencySortMode)
		{
		case TransparencySortMode.Perspective:
			sortingSettings.distanceMetric = DistanceMetric.Perspective;
			break;
		case TransparencySortMode.Orthographic:
			sortingSettings.distanceMetric = DistanceMetric.Orthographic;
			break;
		default:
			sortingSettings.distanceMetric = DistanceMetric.CustomAxis;
			sortingSettings.customAxis = m_Renderer2DData.transparencySortAxis;
			break;
		}
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		bool flag = true;
		Camera camera = renderingData.cameraData.camera;
		FilteringSettings filteringSettings = new FilteringSettings
		{
			renderQueueRange = RenderQueueRange.all,
			layerMask = -1,
			renderingLayerMask = uint.MaxValue,
			sortingLayerRange = SortingLayerRange.all
		};
		if (Light2D.IsSceneLit(camera))
		{
			RendererLighting.Setup(renderingData, m_Renderer2DData);
			CommandBuffer commandBuffer = CommandBufferPool.Get("Render 2D Lighting");
			commandBuffer.Clear();
			RendererLighting.CreateNormalMapRenderTexture(commandBuffer);
			commandBuffer.SetGlobalFloat("_HDREmulationScale", m_Renderer2DData.hdrEmulationScale);
			commandBuffer.SetGlobalFloat("_InverseHDREmulationScale", 1f / m_Renderer2DData.hdrEmulationScale);
			commandBuffer.SetGlobalFloat("_UseSceneLighting", flag ? 1f : 0f);
			commandBuffer.SetGlobalColor("_RendererColor", Color.white);
			RendererLighting.SetShapeLightShaderGlobals(commandBuffer);
			context.ExecuteCommandBuffer(commandBuffer);
			DrawingSettings drawingSettings = CreateDrawingSettings(k_ShaderTags, ref renderingData, SortingCriteria.CommonTransparent);
			DrawingSettings drawSettings = CreateDrawingSettings(k_NormalsRenderingPassName, ref renderingData, SortingCriteria.CommonTransparent);
			SortingSettings sortingSettings = drawingSettings.sortingSettings;
			GetTransparencySortingMode(camera, ref sortingSettings);
			drawingSettings.sortingSettings = sortingSettings;
			drawingSettings.sortingSettings = sortingSettings;
			bool[] array = new bool[4];
			for (int i = 0; i < s_SortingLayers.Length; i++)
			{
				short num = (short)s_SortingLayers[i].value;
				short lowerBound = ((i == 0) ? short.MinValue : num);
				short upperBound = ((i == s_SortingLayers.Length - 1) ? short.MaxValue : num);
				filteringSettings.sortingLayerRange = new SortingLayerRange(lowerBound, upperBound);
				int id = s_SortingLayers[i].id;
				Light2D.LightStats lightStatsByLayer = Light2D.GetLightStatsByLayer(id, camera);
				commandBuffer.Clear();
				for (int j = 0; j < 4; j++)
				{
					uint num2 = (uint)(1 << j);
					bool flag2 = (lightStatsByLayer.blendStylesUsed & num2) != 0;
					if (flag2 && !array[j])
					{
						RendererLighting.CreateBlendStyleRenderTexture(commandBuffer, j);
						array[j] = true;
					}
					RendererLighting.EnableBlendStyle(commandBuffer, j, flag2);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				if (lightStatsByLayer.totalNormalMapUsage > 0)
				{
					RendererLighting.RenderNormals(context, renderingData.cullResults, drawSettings, filteringSettings, base.depthAttachment);
				}
				commandBuffer.Clear();
				if (lightStatsByLayer.totalLights > 0)
				{
					RendererLighting.RenderLights(camera, commandBuffer, id, lightStatsByLayer.blendStylesUsed);
				}
				else
				{
					RendererLighting.ClearDirtyLighting(commandBuffer, lightStatsByLayer.blendStylesUsed);
				}
				CoreUtils.SetRenderTarget(commandBuffer, base.colorAttachment, base.depthAttachment, ClearFlag.None, Color.white);
				context.ExecuteCommandBuffer(commandBuffer);
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
				if (lightStatsByLayer.totalVolumetricUsage > 0)
				{
					commandBuffer.Clear();
					RendererLighting.RenderLightVolumes(camera, commandBuffer, id, base.colorAttachment, base.depthAttachment, lightStatsByLayer.blendStylesUsed);
					context.ExecuteCommandBuffer(commandBuffer);
					commandBuffer.Clear();
				}
			}
			commandBuffer.Clear();
			RendererLighting.ReleaseRenderTextures(commandBuffer);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
			filteringSettings.sortingLayerRange = SortingLayerRange.all;
		}
		else
		{
			CommandBuffer commandBuffer2 = CommandBufferPool.Get("Render Unlit");
			DrawingSettings drawingSettings2 = CreateDrawingSettings(k_ShaderTags, ref renderingData, SortingCriteria.CommonTransparent);
			CoreUtils.SetRenderTarget(commandBuffer2, base.colorAttachment, base.depthAttachment, ClearFlag.None, Color.white);
			commandBuffer2.SetGlobalTexture("_ShapeLightTexture0", Texture2D.blackTexture);
			commandBuffer2.SetGlobalTexture("_ShapeLightTexture1", Texture2D.blackTexture);
			commandBuffer2.SetGlobalTexture("_ShapeLightTexture2", Texture2D.blackTexture);
			commandBuffer2.SetGlobalTexture("_ShapeLightTexture3", Texture2D.blackTexture);
			commandBuffer2.SetGlobalFloat("_UseSceneLighting", flag ? 1f : 0f);
			commandBuffer2.SetGlobalColor("_RendererColor", Color.white);
			commandBuffer2.EnableShaderKeyword("USE_SHAPE_LIGHT_TYPE_0");
			context.ExecuteCommandBuffer(commandBuffer2);
			CommandBufferPool.Release(commandBuffer2);
			context.DrawRenderers(renderingData.cullResults, ref drawingSettings2, ref filteringSettings);
		}
	}
}
