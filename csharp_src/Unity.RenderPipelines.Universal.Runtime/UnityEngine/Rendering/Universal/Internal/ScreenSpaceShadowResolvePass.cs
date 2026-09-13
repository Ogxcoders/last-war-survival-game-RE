using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal.Internal;

public class ScreenSpaceShadowResolvePass : ScriptableRenderPass
{
	private Material m_ScreenSpaceShadowsMaterial;

	private RenderTargetHandle m_ScreenSpaceShadowmap;

	private RenderTextureDescriptor m_RenderTextureDescriptor;

	private const string m_ProfilerTag = "Resolve Shadows";

	public ScreenSpaceShadowResolvePass(RenderPassEvent evt, Material screenspaceShadowsMaterial)
	{
		m_ScreenSpaceShadowsMaterial = screenspaceShadowsMaterial;
		m_ScreenSpaceShadowmap.Init("_ScreenSpaceShadowmapTexture");
		base.renderPassEvent = evt;
	}

	public void Setup(RenderTextureDescriptor baseDescriptor)
	{
		m_RenderTextureDescriptor = baseDescriptor;
		m_RenderTextureDescriptor.depthBufferBits = 0;
		m_RenderTextureDescriptor.msaaSamples = 1;
		m_RenderTextureDescriptor.graphicsFormat = (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R8_UNorm, FormatUsage.Blend) ? GraphicsFormat.R8_UNorm : GraphicsFormat.B8G8R8A8_UNorm);
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		cmd.GetTemporaryRT(m_ScreenSpaceShadowmap.id, m_RenderTextureDescriptor, FilterMode.Bilinear);
		RenderTargetIdentifier renderTargetIdentifier = m_ScreenSpaceShadowmap.Identifier();
		ConfigureTarget(renderTargetIdentifier);
		ConfigureClear(ClearFlag.All, Color.white);
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (m_ScreenSpaceShadowsMaterial == null)
		{
			Debug.LogErrorFormat("Missing {0}. {1} render pass will not execute. Check for missing reference in the renderer resources.", m_ScreenSpaceShadowsMaterial, GetType().Name);
		}
		else if (renderingData.lightData.mainLightIndex != -1)
		{
			Camera camera = renderingData.cameraData.camera;
			bool isStereoEnabled = renderingData.cameraData.isStereoEnabled;
			CommandBuffer commandBuffer = CommandBufferPool.Get("Resolve Shadows");
			if (!isStereoEnabled)
			{
				commandBuffer.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
				commandBuffer.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_ScreenSpaceShadowsMaterial);
				commandBuffer.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
			}
			else
			{
				RenderTargetIdentifier renderTargetIdentifier = m_ScreenSpaceShadowmap.Identifier();
				Blit(commandBuffer, renderTargetIdentifier, renderTargetIdentifier, m_ScreenSpaceShadowsMaterial);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		cmd.ReleaseTemporaryRT(m_ScreenSpaceShadowmap.id);
	}
}
