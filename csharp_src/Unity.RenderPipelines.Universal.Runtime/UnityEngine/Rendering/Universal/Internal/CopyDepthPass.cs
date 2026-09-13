using System;

namespace UnityEngine.Rendering.Universal.Internal;

public class CopyDepthPass : ScriptableRenderPass
{
	public static bool CustomReadDepth;

	private Material m_CopyDepthMaterial;

	private const string m_ProfilerTag = "Copy Depth";

	private const string m_ProfilerTagUpdateReadDepthParameter = "Update Read Depth Parameter";

	private int m_ScaleBiasId = Shader.PropertyToID("_ScaleBiasRT");

	private RenderTargetHandle source { get; set; }

	private RenderTargetHandle destination { get; set; }

	public CopyDepthPass(RenderPassEvent evt, Material copyDepthMaterial)
	{
		m_CopyDepthMaterial = copyDepthMaterial;
		base.renderPassEvent = evt;
	}

	public void Setup(RenderTargetHandle source, RenderTargetHandle destination)
	{
		this.source = source;
		this.destination = destination;
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		if (!CustomReadDepth)
		{
			RenderTextureDescriptor desc = cameraTextureDescriptor;
			desc.colorFormat = RenderTextureFormat.Depth;
			desc.depthBufferBits = 32;
			desc.msaaSamples = 1;
			desc.width /= 2;
			desc.height /= 2;
			cmd.GetTemporaryRT(destination.id, desc, FilterMode.Point);
			ConfigureTarget(destination.Identifier());
		}
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (m_CopyDepthMaterial == null)
		{
			Debug.LogErrorFormat("Missing {0}. {1} render pass will not execute. Check for missing reference in the renderer resources.", m_CopyDepthMaterial, GetType().Name);
			return;
		}
		CommandBuffer commandBuffer = CommandBufferPool.Get(CustomReadDepth ? "Update Read Depth Parameter" : "Copy Depth");
		source.Identifier();
		destination.Identifier();
		RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
		int msaaSamples = cameraTargetDescriptor.msaaSamples;
		CameraData cameraData = renderingData.cameraData;
		switch (msaaSamples)
		{
		case 8:
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.DepthMsaa2);
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.DepthMsaa4);
			commandBuffer.EnableShaderKeyword(ShaderKeywordStrings.DepthMsaa8);
			break;
		case 4:
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.DepthMsaa2);
			commandBuffer.EnableShaderKeyword(ShaderKeywordStrings.DepthMsaa4);
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.DepthMsaa8);
			break;
		case 2:
			commandBuffer.EnableShaderKeyword(ShaderKeywordStrings.DepthMsaa2);
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.DepthMsaa4);
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.DepthMsaa8);
			break;
		default:
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.DepthMsaa2);
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.DepthMsaa4);
			commandBuffer.DisableShaderKeyword(ShaderKeywordStrings.DepthMsaa8);
			break;
		}
		commandBuffer.SetGlobalTexture("_CameraDepthAttachment", source.Identifier());
		float num = (cameraData.IsCameraProjectionMatrixFlipped() ? (-1f) : 1f);
		Vector4 value = ((num < 0f) ? new Vector4(num, 1f, -1f, 1f) : new Vector4(num, 0f, 1f, 1f));
		commandBuffer.SetGlobalVector(m_ScaleBiasId, value);
		if (!CustomReadDepth)
		{
			commandBuffer.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_CopyDepthMaterial);
		}
		CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.AllowSoftParticles, UniversalRenderPipeline.asset.allowSoftParticles);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (!CustomReadDepth)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}
			cmd.ReleaseTemporaryRT(destination.id);
			destination = RenderTargetHandle.CameraTarget;
		}
	}
}
