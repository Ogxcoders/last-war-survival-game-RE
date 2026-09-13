using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public class CloudRendererFeature : RenderObjects
{
	private class CloudRendererPass : RenderObjectsPass
	{
		public const string CloudColorTexture = "_CloudColorTexture";

		public const string CloudHeight = "_CloudHeight";

		public const string CloudBlitFlipY = "_CloudBlitFlipY";

		private RenderTexture destination;

		private float renderTextureScale;

		private float cloudHeight;

		public CloudRendererPass(string profilerTag, RenderPassEvent renderPassEvent, string[] shaderTags, RenderQueueType renderQueueType, int layerMask, CustomCameraSettings cameraSettings)
			: base(profilerTag, renderPassEvent, shaderTags, renderQueueType, layerMask, cameraSettings)
		{
		}

		public void SetupScale(float renderTextureScale)
		{
			this.renderTextureScale = renderTextureScale;
		}

		public void SetupHeight(float cloudHeight)
		{
			this.cloudHeight = cloudHeight;
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			destination = RenderTexture.GetTemporary(Mathf.Max(4, (int)((float)cameraTextureDescriptor.width * renderTextureScale) >> 2 << 2), Mathf.Max(4, (int)((float)cameraTextureDescriptor.height * renderTextureScale) >> 2 << 2), 0, RenderTextureFormat.ARGB32);
			destination.hideFlags = HideFlags.HideAndDontSave;
			destination.name = "_CloudColorTexture";
			ConfigureTarget(new RenderTargetIdentifier(destination));
			ConfigureClear(ClearFlag.Color, Color.black);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get("CloudRenderPass");
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			base.Execute(context, ref renderingData);
			commandBuffer.SetGlobalTexture("_CloudColorTexture", destination);
			commandBuffer.SetGlobalFloat("_CloudHeight", cloudHeight);
			commandBuffer.SetGlobalFloat("_CloudBlitFlipY", renderingData.cameraData.IsCameraProjectionMatrixFlipped() ? (-1f) : 1f);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			CommandBufferPool.Release(commandBuffer);
		}

		public override void FrameCleanup(CommandBuffer cmd)
		{
			if (destination != null)
			{
				RenderTexture.ReleaseTemporary(destination);
				destination = null;
			}
		}
	}

	private CloudRendererPass m_CloudColorPass;

	public float renderTextureScale = 1f;

	public float cloudHeight = 1f;

	public override void Create()
	{
		FilterSettings filterSettings = settings.filterSettings;
		m_CloudColorPass = new CloudRendererPass(settings.passTag, settings.Event, filterSettings.PassNames, filterSettings.RenderQueueType, filterSettings.LayerMask, settings.cameraSettings);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base)
		{
			m_CloudColorPass.SetupScale(renderTextureScale);
			m_CloudColorPass.SetupHeight(cloudHeight);
			renderer.EnqueuePass(m_CloudColorPass);
		}
	}
}
