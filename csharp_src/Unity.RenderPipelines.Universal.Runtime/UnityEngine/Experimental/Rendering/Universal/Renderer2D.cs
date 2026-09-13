using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Experimental.Rendering.Universal;

internal class Renderer2D : ScriptableRenderer
{
	private ColorGradingLutPass m_ColorGradingLutPass;

	private Render2DLightingPass m_Render2DLightingPass;

	private PostProcessPass m_PostProcessPass;

	private FinalBlitPass m_FinalBlitPass;

	private PostProcessPass m_FinalPostProcessPass;

	private bool m_UseDepthStencilBuffer = true;

	private bool m_CreateColorTexture;

	private bool m_CreateDepthTexture;

	private readonly RenderTargetHandle k_ColorTextureHandle;

	private readonly RenderTargetHandle k_DepthTextureHandle;

	private readonly RenderTargetHandle k_AfterPostProcessColorHandle;

	private readonly RenderTargetHandle k_ColorGradingLutHandle;

	private Material m_BlitMaterial;

	private Renderer2DData m_Renderer2DData;

	internal bool createColorTexture => m_CreateColorTexture;

	internal bool createDepthTexture => m_CreateDepthTexture;

	public Renderer2D(Renderer2DData data)
		: base(data)
	{
		m_BlitMaterial = CoreUtils.CreateEngineMaterial(data.blitShader);
		m_ColorGradingLutPass = new ColorGradingLutPass(RenderPassEvent.BeforeRenderingOpaques, data.postProcessData);
		m_Render2DLightingPass = new Render2DLightingPass(data);
		m_PostProcessPass = new PostProcessPass(RenderPassEvent.BeforeRenderingPostProcessing, data.postProcessData, m_BlitMaterial);
		m_FinalPostProcessPass = new PostProcessPass(RenderPassEvent.AfterRenderingPostProcessing, data.postProcessData, m_BlitMaterial);
		m_FinalBlitPass = new FinalBlitPass((RenderPassEvent)1001, m_BlitMaterial);
		m_UseDepthStencilBuffer = data.useDepthStencilBuffer;
		k_ColorTextureHandle.Init("_CameraColorTexture");
		k_DepthTextureHandle.Init("_CameraDepthAttachment");
		k_AfterPostProcessColorHandle.Init("_AfterPostProcessTexture");
		k_ColorGradingLutHandle.Init("_InternalGradingLut");
		m_Renderer2DData = data;
		base.supportedRenderingFeatures = new RenderingFeatures
		{
			cameraStacking = true
		};
	}

	protected override void Dispose(bool disposing)
	{
		m_PostProcessPass.Cleanup();
		m_FinalPostProcessPass.Cleanup();
		m_ColorGradingLutPass.Cleanup();
		CoreUtils.Destroy(m_BlitMaterial);
	}

	public Renderer2DData GetRenderer2DData()
	{
		return m_Renderer2DData;
	}

	private void CreateRenderTextures(ref CameraData cameraData, bool forceCreateColorTexture, FilterMode colorTextureFilterMode, CommandBuffer cmd, out RenderTargetHandle colorTargetHandle, out RenderTargetHandle depthTargetHandle)
	{
		ref RenderTextureDescriptor cameraTargetDescriptor = ref cameraData.cameraTargetDescriptor;
		if (cameraData.renderType == CameraRenderType.Base)
		{
			m_CreateColorTexture = forceCreateColorTexture || cameraData.postProcessEnabled || cameraData.isHdrEnabled || cameraData.isSceneViewCamera || !cameraData.isDefaultViewport || !m_UseDepthStencilBuffer || !cameraData.resolveFinalTarget;
			m_CreateDepthTexture = !cameraData.resolveFinalTarget && m_UseDepthStencilBuffer;
			colorTargetHandle = (m_CreateColorTexture ? k_ColorTextureHandle : RenderTargetHandle.CameraTarget);
			depthTargetHandle = (m_CreateDepthTexture ? k_DepthTextureHandle : colorTargetHandle);
			if (m_CreateColorTexture)
			{
				RenderTextureDescriptor desc = cameraTargetDescriptor;
				desc.depthBufferBits = ((!m_CreateDepthTexture && m_UseDepthStencilBuffer) ? 32 : 0);
				cmd.GetTemporaryRT(k_ColorTextureHandle.id, desc, colorTextureFilterMode);
			}
			if (m_CreateDepthTexture)
			{
				RenderTextureDescriptor desc2 = cameraTargetDescriptor;
				desc2.colorFormat = RenderTextureFormat.Depth;
				desc2.depthBufferBits = 32;
				desc2.bindMS = desc2.msaaSamples > 1 && !SystemInfo.supportsMultisampleAutoResolve && SystemInfo.supportsMultisampledTextures != 0;
				cmd.GetTemporaryRT(k_DepthTextureHandle.id, desc2, FilterMode.Point);
			}
		}
		else
		{
			m_CreateColorTexture = true;
			m_CreateDepthTexture = true;
			colorTargetHandle = k_ColorTextureHandle;
			depthTargetHandle = k_DepthTextureHandle;
		}
	}

	public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		ref CameraData cameraData = ref renderingData.cameraData;
		ref RenderTextureDescriptor cameraTargetDescriptor = ref cameraData.cameraTargetDescriptor;
		bool postProcessingEnabled = renderingData.postProcessingEnabled;
		bool resolveFinalTarget = cameraData.resolveFinalTarget;
		FilterMode colorTextureFilterMode = FilterMode.Bilinear;
		bool num = UniversalRenderPipeline.asset.postProcessingFeatureSet == PostProcessingFeatureSet.PostProcessingV2;
		PixelPerfectCamera component = null;
		bool forceCreateColorTexture = false;
		bool flag = false;
		if (cameraData.renderType == CameraRenderType.Base && resolveFinalTarget)
		{
			cameraData.camera.TryGetComponent<PixelPerfectCamera>(out component);
			if (component != null)
			{
				if (component.offscreenRTSize != Vector2Int.zero)
				{
					forceCreateColorTexture = true;
					cameraTargetDescriptor.width = component.offscreenRTSize.x;
					cameraTargetDescriptor.height = component.offscreenRTSize.y;
				}
				colorTextureFilterMode = component.finalBlitFilterMode;
				flag = component.upscaleRT && component.isRunning;
			}
		}
		CommandBuffer commandBuffer = CommandBufferPool.Get("Create Camera Textures");
		CreateRenderTextures(ref cameraData, forceCreateColorTexture, colorTextureFilterMode, commandBuffer, out var colorTargetHandle, out var depthTargetHandle);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
		ConfigureCameraTarget(colorTargetHandle.Identifier(), depthTargetHandle.Identifier());
		if (!num && postProcessingEnabled && cameraData.renderType == CameraRenderType.Base)
		{
			m_ColorGradingLutPass.Setup(in k_ColorGradingLutHandle);
			EnqueuePass(m_ColorGradingLutPass);
		}
		m_Render2DLightingPass.ConfigureTarget(colorTargetHandle.Identifier(), depthTargetHandle.Identifier());
		EnqueuePass(m_Render2DLightingPass);
		bool flag2 = !num && resolveFinalTarget && !flag && postProcessingEnabled && cameraData.antialiasing == AntialiasingMode.FastApproximateAntialiasing && !cameraData.doFxaaInUberPost;
		if (cameraData.postProcessEnabled)
		{
			RenderTargetHandle destination = ((resolveFinalTarget && !flag && !flag2) ? RenderTargetHandle.CameraTarget : k_AfterPostProcessColorHandle);
			m_PostProcessPass.Setup(in cameraTargetDescriptor, in colorTargetHandle, in destination, in depthTargetHandle, in k_ColorGradingLutHandle, flag2, destination == RenderTargetHandle.CameraTarget);
			EnqueuePass(m_PostProcessPass);
			colorTargetHandle = destination;
		}
		if (flag2)
		{
			m_FinalPostProcessPass.SetupFinalPass(in cameraTargetDescriptor, in colorTargetHandle);
			EnqueuePass(m_FinalPostProcessPass);
		}
		else if (resolveFinalTarget && colorTargetHandle != RenderTargetHandle.CameraTarget)
		{
			m_FinalBlitPass.Setup(cameraTargetDescriptor, colorTargetHandle);
			EnqueuePass(m_FinalBlitPass);
		}
	}

	public override void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData)
	{
		cullingParameters.cullingOptions = CullingOptions.None;
		cullingParameters.isOrthographic = cameraData.camera.orthographic;
		cullingParameters.shadowDistance = 0f;
	}

	public override void FinishRendering(CommandBuffer cmd)
	{
		if (m_CreateColorTexture)
		{
			cmd.ReleaseTemporaryRT(k_ColorTextureHandle.id);
		}
		if (m_CreateDepthTexture)
		{
			cmd.ReleaseTemporaryRT(k_DepthTextureHandle.id);
		}
	}
}
