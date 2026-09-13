using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace VolumetricFogAndMist2;

public class DepthRenderPrePassFeature : ScriptableRendererFeature
{
	public class DepthRenderPass : ScriptableRenderPass
	{
		private const string m_ProfilerTag = "CustomDepthPrePass";

		private const string SKW_DEPTH_PREPASS = "VF2_DEPTH_PREPASS";

		public static int layerMask;

		private FilteringSettings m_FilteringSettings;

		private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

		private RenderTargetHandle m_Depth;

		private Material depthOnlyMaterial;

		public DepthRenderPass()
		{
			m_Depth.Init("_CustomDepthTexture");
			m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
			m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
			m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));
			m_FilteringSettings = new FilteringSettings(RenderQueueRange.transparent, 0);
			Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
			if (shader != null)
			{
				depthOnlyMaterial = new Material(shader);
			}
			SetupKeywords();
		}

		private void SetupKeywords()
		{
			if (layerMask != 0)
			{
				Shader.EnableKeyword("VF2_DEPTH_PREPASS");
			}
			else
			{
				Shader.DisableKeyword("VF2_DEPTH_PREPASS");
			}
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			if (layerMask != m_FilteringSettings.layerMask)
			{
				m_FilteringSettings = new FilteringSettings(RenderQueueRange.transparent, layerMask);
				SetupKeywords();
			}
			RenderTextureDescriptor desc = cameraTextureDescriptor;
			desc.colorFormat = RenderTextureFormat.Depth;
			desc.depthBufferBits = 32;
			desc.msaaSamples = 1;
			cmd.GetTemporaryRT(m_Depth.id, desc, FilterMode.Point);
			cmd.SetGlobalTexture("_CustomDepthTexture", m_Depth.Identifier());
			ConfigureTarget(m_Depth.Identifier());
			ConfigureClear(ClearFlag.All, Color.black);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (layerMask != 0)
			{
				CommandBuffer commandBuffer = CommandBufferPool.Get("CustomDepthPrePass");
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				SortingCriteria sortingCriteria = SortingCriteria.CommonTransparent;
				DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
				drawingSettings.perObjectData = PerObjectData.None;
				drawingSettings.overrideMaterial = depthOnlyMaterial;
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}
		}

		public override void FrameCleanup(CommandBuffer cmd)
		{
			cmd?.ReleaseTemporaryRT(m_Depth.id);
		}
	}

	private DepthRenderPass m_ScriptablePass;

	public static bool installed;

	public override void Create()
	{
		m_ScriptablePass = new DepthRenderPass
		{
			renderPassEvent = RenderPassEvent.AfterRenderingOpaques
		};
	}

	private void OnDestroy()
	{
		installed = false;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		installed = true;
		renderer.EnqueuePass(m_ScriptablePass);
	}
}
