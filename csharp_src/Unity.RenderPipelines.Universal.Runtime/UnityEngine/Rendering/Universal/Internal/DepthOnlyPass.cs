using System;

namespace UnityEngine.Rendering.Universal.Internal;

public class DepthOnlyPass : ScriptableRenderPass
{
	private int kDepthBufferBits = 32;

	private FilteringSettings m_FilteringSettings;

	private const string m_ProfilerTag = "Depth Prepass";

	private ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Depth Prepass");

	private ShaderTagId m_ShaderTagId = new ShaderTagId("DepthOnly");

	private RenderTargetHandle depthAttachmentHandle { get; set; }

	internal RenderTextureDescriptor descriptor { get; private set; }

	public DepthOnlyPass(RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask)
	{
		m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
		base.renderPassEvent = evt;
	}

	public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle depthAttachmentHandle)
	{
		this.depthAttachmentHandle = depthAttachmentHandle;
		baseDescriptor.colorFormat = RenderTextureFormat.Depth;
		baseDescriptor.depthBufferBits = kDepthBufferBits;
		baseDescriptor.msaaSamples = 1;
		descriptor = baseDescriptor;
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		cmd.GetTemporaryRT(depthAttachmentHandle.id, descriptor, FilterMode.Point);
		ConfigureTarget(depthAttachmentHandle.Identifier());
		ConfigureClear(ClearFlag.All, Color.black);
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("Depth Prepass");
		using (new ProfilingScope(commandBuffer, m_ProfilingSampler))
		{
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			SortingCriteria defaultOpaqueSortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
			DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagId, ref renderingData, defaultOpaqueSortFlags);
			drawingSettings.perObjectData = PerObjectData.None;
			ref CameraData cameraData = ref renderingData.cameraData;
			Camera camera = cameraData.camera;
			if (cameraData.isStereoEnabled)
			{
				context.StartMultiEye(camera, base.eyeIndex);
			}
			context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings);
		}
		CoreUtils.SetKeyword(commandBuffer, ShaderKeywordStrings.AllowSoftParticles, UniversalRenderPipeline.asset.allowSoftParticles);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		if (cmd == null)
		{
			throw new ArgumentNullException("cmd");
		}
		if (depthAttachmentHandle != RenderTargetHandle.CameraTarget)
		{
			cmd.ReleaseTemporaryRT(depthAttachmentHandle.id);
			depthAttachmentHandle = RenderTargetHandle.CameraTarget;
		}
	}
}
