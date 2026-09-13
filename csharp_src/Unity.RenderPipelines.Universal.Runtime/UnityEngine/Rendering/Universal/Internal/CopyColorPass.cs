using System;

namespace UnityEngine.Rendering.Universal.Internal;

public class CopyColorPass : ScriptableRenderPass
{
	private int m_SampleOffsetShaderHandle;

	private Material m_SamplingMaterial;

	private Downsampling m_DownsamplingMethod;

	private const string m_ProfilerTag = "Copy Color";

	private RenderTargetIdentifier source { get; set; }

	private RenderTargetHandle destination { get; set; }

	public CopyColorPass(RenderPassEvent evt, Material samplingMaterial)
	{
		m_SamplingMaterial = samplingMaterial;
		m_SampleOffsetShaderHandle = Shader.PropertyToID("_SampleOffset");
		base.renderPassEvent = evt;
		m_DownsamplingMethod = Downsampling.None;
	}

	public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination, Downsampling downsampling)
	{
		this.source = source;
		this.destination = destination;
		m_DownsamplingMethod = downsampling;
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescripor)
	{
		RenderTextureDescriptor desc = cameraTextureDescripor;
		desc.msaaSamples = 1;
		desc.depthBufferBits = 0;
		if (m_DownsamplingMethod == Downsampling._2xBilinear)
		{
			desc.width /= 2;
			desc.height /= 2;
		}
		else if (m_DownsamplingMethod == Downsampling._4xBox || m_DownsamplingMethod == Downsampling._4xBilinear)
		{
			desc.width /= 4;
			desc.height /= 4;
		}
		cmd.GetTemporaryRT(destination.id, desc, (m_DownsamplingMethod != Downsampling.None) ? FilterMode.Bilinear : FilterMode.Point);
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (m_SamplingMaterial == null)
		{
			Debug.LogErrorFormat("Missing {0}. {1} render pass will not execute. Check for missing reference in the renderer resources.", m_SamplingMaterial, GetType().Name);
			return;
		}
		CommandBuffer commandBuffer = CommandBufferPool.Get("Copy Color");
		RenderTargetIdentifier renderTargetIdentifier = destination.Identifier();
		switch (m_DownsamplingMethod)
		{
		case Downsampling.None:
			Blit(commandBuffer, source, renderTargetIdentifier);
			break;
		case Downsampling._2xBilinear:
			Blit(commandBuffer, source, renderTargetIdentifier);
			break;
		case Downsampling._4xBox:
			m_SamplingMaterial.SetFloat(m_SampleOffsetShaderHandle, 2f);
			Blit(commandBuffer, source, renderTargetIdentifier, m_SamplingMaterial);
			break;
		case Downsampling._4xBilinear:
			Blit(commandBuffer, source, renderTargetIdentifier);
			break;
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		if (destination != RenderTargetHandle.CameraTarget)
		{
			cmd.ReleaseTemporaryRT(destination.id);
			destination = RenderTargetHandle.CameraTarget;
		}
	}
}
