using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public class OverlayBackgroundRendererFeature : ScriptableRendererFeature
{
	private class OverlayBackgroundRenderObjectsPass : CustomRenderObjectsPass
	{
		public bool clearDepth { get; set; } = true;

		public OverlayBackgroundRenderObjectsPass(string tag, CustomRenderObjectsFeature.RenderObjectsSettings settings)
			: base(tag, settings)
		{
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			ConfigureClear(clearDepth ? ClearFlag.Depth : ClearFlag.None, Color.clear);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (!(base.overrideMaterial == null))
			{
				CommandBuffer commandBuffer = CommandBufferPool.Get("OverlayBackgroundRendererFeature");
				commandBuffer.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, base.overrideMaterial);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}
		}

		public override void FrameCleanup(CommandBuffer cmd)
		{
		}
	}

	public string renderingLayerMaskName = "OverlayBackground";

	public const string OverlayBackgroundSettingsTag = "OverlayBackgroundSettings";

	public const string OpaqueSettingsTag = "OpaqueSettings";

	public const string TransparentSettingsTag = "TransparentSettings";

	[Header("OverlayBackgroundSettings")]
	public CustomRenderObjectsFeature.RenderObjectsSettings overlayBackgroundSettings = new CustomRenderObjectsFeature.RenderObjectsSettings();

	[Header("OpaqueSettings")]
	public CustomRenderObjectsFeature.RenderObjectsSettings opaqueSettings = new CustomRenderObjectsFeature.RenderObjectsSettings();

	[Header("TransparentSettings")]
	public CustomRenderObjectsFeature.RenderObjectsSettings transparentSettings = new CustomRenderObjectsFeature.RenderObjectsSettings();

	private OverlayBackgroundRenderObjectsPass m_OverlayBackgroundRenderPass;

	private CustomRenderObjectsPass m_OpaqueRenderPass;

	private CustomRenderObjectsPass m_TransparentRenderPass;

	public Material overrideMaterial
	{
		get
		{
			return overlayBackgroundSettings.overrideMaterial;
		}
		set
		{
			overlayBackgroundSettings.overrideMaterial = value;
		}
	}

	public bool clearDepth
	{
		get
		{
			return m_OverlayBackgroundRenderPass.clearDepth;
		}
		set
		{
			m_OverlayBackgroundRenderPass.clearDepth = value;
		}
	}

	public override void Create()
	{
		int num = 1 << LayerMask.NameToLayer("Default");
		int num2 = ((!string.IsNullOrEmpty(renderingLayerMaskName)) ? RenderingLayerMask.NameToMask(renderingLayerMaskName) : 0);
		if ((int)opaqueSettings.filterSettings.LayerMask == 0)
		{
			opaqueSettings.filterSettings.LayerMask = num;
		}
		opaqueSettings.filterSettings.RenderQueueType = RenderQueueType.Opaque;
		opaqueSettings.filterSettings.RenderingLayerMask |= num2;
		if ((int)transparentSettings.filterSettings.LayerMask == 0)
		{
			transparentSettings.filterSettings.LayerMask = num;
		}
		transparentSettings.filterSettings.RenderQueueType = RenderQueueType.Transparent;
		transparentSettings.filterSettings.RenderingLayerMask |= num2;
		m_OverlayBackgroundRenderPass = new OverlayBackgroundRenderObjectsPass("OverlayBackgroundSettings", overlayBackgroundSettings);
		m_OpaqueRenderPass = new CustomRenderObjectsPass("OpaqueSettings", opaqueSettings);
		m_TransparentRenderPass = new CustomRenderObjectsPass("TransparentSettings", transparentSettings);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base)
		{
			renderer.EnqueuePass(m_OverlayBackgroundRenderPass);
			renderer.EnqueuePass(m_OpaqueRenderPass);
			renderer.EnqueuePass(m_TransparentRenderPass);
		}
	}
}
