using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GaussianBlurRenderPassFeature : ScriptableRendererFeature
{
	[Serializable]
	public class BlurSettings
	{
		public LayerMask opaqueLayerMask;

		public LayerMask transparentLayerMask;

		public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

		public Material m_VerticalBlurMat;

		public Material m_HorizontalBlurMat;

		public string textureId = "_ScreenTexture";

		[Range(1f, 8f)]
		public int downSample = 1;

		[Range(1f, 32f)]
		public int blurCount = 1;

		[Range(0f, 0.005f)]
		public float intensity;
	}

	public BlurSettings blurSettings = new BlurSettings();

	private RenderTargetHandle m_renderTargetHandle;

	private GaussianBlurRenderPass m_gaussianBlurRenderPass;

	public override void Create()
	{
		m_gaussianBlurRenderPass = new GaussianBlurRenderPass(blurSettings.renderPassEvent, blurSettings.m_VerticalBlurMat, blurSettings.m_HorizontalBlurMat, base.name, blurSettings.downSample, blurSettings.blurCount, blurSettings.intensity);
		m_renderTargetHandle.Init(blurSettings.textureId);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (!renderingData.cameraData.isSceneViewCamera)
		{
			RenderTargetIdentifier cameraColorTarget = renderer.cameraColorTarget;
			RenderTargetHandle renderTargetHandle = m_renderTargetHandle;
			if (blurSettings.m_VerticalBlurMat == null)
			{
				Debug.Log("Missing VerticalBlurMat");
				return;
			}
			if (blurSettings.m_HorizontalBlurMat == null)
			{
				Debug.Log("Missing HorizontalBlurMat");
				return;
			}
			m_gaussianBlurRenderPass.Setup(cameraColorTarget, renderTargetHandle);
			renderer.EnqueuePass(m_gaussianBlurRenderPass);
		}
	}
}
