using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomDepthRenderFeature : ScriptableRendererFeature
{
	[Serializable]
	public class CustomDepthPassSettings
	{
		public RenderPassEvent passEvent = RenderPassEvent.AfterRenderingOpaques;
	}

	private class CustomDepthPass : ScriptableRenderPass
	{
		private ShaderTagId shaderTagId = new ShaderTagId("CustomDepthPass");

		private FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.all);

		private string profilerTag = "Custom Depth Pass";

		public CustomDepthPass(RenderPassEvent passEvent)
		{
			base.renderPassEvent = passEvent;
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get(profilerTag);
			using (new ProfilingSample(commandBuffer, profilerTag))
			{
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				DrawingSettings drawingSettings = CreateDrawingSettings(shaderTagId, ref renderingData, SortingCriteria.CommonOpaque);
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}
	}

	[SerializeField]
	public CustomDepthPassSettings settings = new CustomDepthPassSettings();

	private CustomDepthPass customDepthPass;

	public override void Create()
	{
		customDepthPass = new CustomDepthPass(settings.passEvent);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(customDepthPass);
	}
}
