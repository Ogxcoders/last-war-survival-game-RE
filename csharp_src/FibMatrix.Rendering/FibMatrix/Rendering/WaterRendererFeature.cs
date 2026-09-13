using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

namespace FibMatrix.Rendering;

[ExcludeFromPreset]
public class WaterRendererFeature : RenderObjects
{
	public bool smallColorRTBlit = true;

	[Range(0.1f, 1f)]
	public float destinationScale = 0.5f;

	public bool readDepthInsteadOfCopy = true;

	public const string WaterColorTextureName = "_WaterColorTexture";

	private WaterColorPass m_WaterColorPass;

	private RenderQueueType m_RenderQueueType;

	public RenderQueueType renderQueueType
	{
		get
		{
			return m_RenderQueueType;
		}
		set
		{
			if (m_RenderQueueType != value)
			{
				settings.filterSettings.RenderQueueType = value;
				settings.Event = ((value == RenderQueueType.Opaque) ? RenderPassEvent.BeforeRenderingOpaques : RenderPassEvent.BeforeRenderingTransparents);
				m_RenderQueueType = value;
				Create();
			}
		}
	}

	public override void Create()
	{
		base.Create();
		FilterSettings filterSettings = settings.filterSettings;
		m_WaterColorPass = new WaterColorPass(settings.passTag, settings.Event, filterSettings.PassNames, filterSettings.RenderQueueType, filterSettings.LayerMask, settings.cameraSettings, "_WaterColorTexture");
		m_WaterColorPass.smallColorRTBlit = smallColorRTBlit;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base)
		{
			bool customReadDepth = readDepthInsteadOfCopy && renderer.cameraColorTarget != RenderTargetHandle.CameraTarget.Identifier() && renderingData.cameraData.cameraType == CameraType.Game;
			bool supportsCameraDepthTexture = UniversalRenderPipeline.asset.supportsCameraDepthTexture;
			CopyDepthPass.CustomReadDepth = customReadDepth;
			m_WaterColorPass.Setup(destinationScale, supportsCameraDepthTexture, customReadDepth, renderingData.cameraData.isHdrEnabled);
			renderer.EnqueuePass(m_WaterColorPass);
		}
	}
}
