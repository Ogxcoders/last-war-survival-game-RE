using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.XR;

namespace UnityEngine.Rendering.Universal;

public sealed class ForwardRenderer : ScriptableRenderer
{
	private const int k_DepthStencilBufferBits = 32;

	private const string k_CreateCameraTextures = "Create Camera Texture";

	private ColorGradingLutPass m_ColorGradingLutPass;

	private DepthOnlyPass m_DepthPrepass;

	private MainLightShadowCasterPass m_MainLightShadowCasterPass;

	private AdditionalLightsShadowCasterPass m_AdditionalLightsShadowCasterPass;

	private DrawObjectsPass m_RenderOpaqueForwardPass;

	private DrawSkyboxPass m_DrawSkyboxPass;

	private CopyDepthPass m_CopyDepthPass;

	private CopyColorPass m_CopyColorPass;

	private TransparentSettingsPass m_TransparentSettingsPass;

	private DrawObjectsPass m_RenderTransparentForwardPass;

	private InvokeOnRenderObjectCallbackPass m_OnRenderObjectCallbackPass;

	private PostProcessPass m_PostProcessPass;

	private PostProcessPass m_FinalPostProcessPass;

	private FinalBlitPass m_FinalBlitPass;

	private CapturePass m_CapturePass;

	private RenderTargetHandle m_ActiveCameraColorAttachment;

	private RenderTargetHandle m_ActiveCameraDepthAttachment;

	private RenderTargetHandle m_CameraColorAttachment;

	private RenderTargetHandle m_CameraDepthAttachment;

	private RenderTargetHandle m_DepthTexture;

	private RenderTargetHandle m_OpaqueColor;

	private RenderTargetHandle m_AfterPostProcessColor;

	private RenderTargetHandle m_ColorGradingLut;

	private ForwardLights m_ForwardLights;

	private StencilState m_DefaultStencilState;

	private Material m_BlitMaterial;

	private Material m_CopyDepthMaterial;

	private Material m_SamplingMaterial;

	private Material m_ScreenspaceShadowsMaterial;

	private Camera mainCamera;

	public ForwardRenderer(ForwardRendererData data)
		: base(data)
	{
		m_BlitMaterial = CoreUtils.CreateEngineMaterial(data.shaders.blitPS);
		m_CopyDepthMaterial = CoreUtils.CreateEngineMaterial(data.shaders.copyDepthPS);
		m_SamplingMaterial = CoreUtils.CreateEngineMaterial(data.shaders.samplingPS);
		m_ScreenspaceShadowsMaterial = CoreUtils.CreateEngineMaterial(data.shaders.screenSpaceShadowPS);
		StencilStateData defaultStencilState = data.defaultStencilState;
		m_DefaultStencilState = StencilState.defaultValue;
		m_DefaultStencilState.enabled = defaultStencilState.overrideStencilState;
		m_DefaultStencilState.SetCompareFunction(defaultStencilState.stencilCompareFunction);
		m_DefaultStencilState.SetPassOperation(defaultStencilState.passOperation);
		m_DefaultStencilState.SetFailOperation(defaultStencilState.failOperation);
		m_DefaultStencilState.SetZFailOperation(defaultStencilState.zFailOperation);
		m_MainLightShadowCasterPass = new MainLightShadowCasterPass(RenderPassEvent.BeforeRenderingShadows);
		m_AdditionalLightsShadowCasterPass = new AdditionalLightsShadowCasterPass(RenderPassEvent.BeforeRenderingShadows);
		m_DepthPrepass = new DepthOnlyPass(RenderPassEvent.BeforeRenderingPrepasses, RenderQueueRange.opaque, data.opaqueLayerMask);
		m_ColorGradingLutPass = new ColorGradingLutPass((RenderPassEvent)160, data.postProcessData);
		m_RenderOpaqueForwardPass = new DrawObjectsPass("Render Opaques", opaque: true, RenderPassEvent.BeforeRenderingOpaques, RenderQueueRange.opaque, data.opaqueLayerMask, m_DefaultStencilState, defaultStencilState.stencilReference);
		m_CopyDepthPass = new CopyDepthPass(RenderPassEvent.AfterRenderingSkybox, m_CopyDepthMaterial);
		m_DrawSkyboxPass = new DrawSkyboxPass(RenderPassEvent.BeforeRenderingSkybox);
		m_CopyColorPass = new CopyColorPass(RenderPassEvent.AfterRenderingSkybox, m_SamplingMaterial);
		m_TransparentSettingsPass = new TransparentSettingsPass(RenderPassEvent.BeforeRenderingTransparents, data.shadowTransparentReceive);
		m_RenderTransparentForwardPass = new DrawObjectsPass("Render Transparents", opaque: false, RenderPassEvent.BeforeRenderingTransparents, RenderQueueRange.transparent, data.transparentLayerMask, m_DefaultStencilState, defaultStencilState.stencilReference);
		m_OnRenderObjectCallbackPass = new InvokeOnRenderObjectCallbackPass(RenderPassEvent.BeforeRenderingPostProcessing);
		m_PostProcessPass = new PostProcessPass(RenderPassEvent.BeforeRenderingPostProcessing, data.postProcessData, m_BlitMaterial);
		m_FinalPostProcessPass = new PostProcessPass((RenderPassEvent)1001, data.postProcessData, m_BlitMaterial);
		m_CapturePass = new CapturePass(RenderPassEvent.AfterRendering);
		m_FinalBlitPass = new FinalBlitPass((RenderPassEvent)1001, m_BlitMaterial);
		m_CameraColorAttachment.Init("_CameraColorTexture");
		m_CameraDepthAttachment.Init("_CameraDepthAttachment");
		m_DepthTexture.Init("_CameraDepthTexture");
		m_OpaqueColor.Init("_CameraOpaqueTexture");
		m_AfterPostProcessColor.Init("_AfterPostProcessTexture");
		m_ColorGradingLut.Init("_InternalGradingLut");
		m_ForwardLights = new ForwardLights();
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
		CoreUtils.Destroy(m_CopyDepthMaterial);
		CoreUtils.Destroy(m_SamplingMaterial);
		CoreUtils.Destroy(m_ScreenspaceShadowsMaterial);
	}

	public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		Camera camera = renderingData.cameraData.camera;
		ref CameraData cameraData = ref renderingData.cameraData;
		RenderTextureDescriptor baseDescriptor = renderingData.cameraData.cameraTargetDescriptor;
		if (cameraData.targetTexture != null && cameraData.targetTexture.format == RenderTextureFormat.Depth)
		{
			ConfigureCameraTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);
			for (int i = 0; i < base.rendererFeatures.Count; i++)
			{
				if (base.rendererFeatures[i].isActive)
				{
					base.rendererFeatures[i].AddRenderPasses(this, ref renderingData);
				}
			}
			EnqueuePass(m_RenderOpaqueForwardPass);
			EnqueuePass(m_DrawSkyboxPass);
			EnqueuePass(m_RenderTransparentForwardPass);
			return;
		}
		bool postProcessEnabled = cameraData.postProcessEnabled;
		bool postProcessingEnabled = renderingData.postProcessingEnabled;
		_ = UniversalRenderPipeline.asset.postProcessingFeatureSet;
		bool postProcessEnabled2 = cameraData.postProcessEnabled;
		bool isSceneViewCamera = cameraData.isSceneViewCamera;
		bool isPreviewCamera = cameraData.isPreviewCamera;
		bool requiresDepthTexture = cameraData.requiresDepthTexture;
		bool isStereoEnabled = cameraData.isStereoEnabled;
		bool flag = m_MainLightShadowCasterPass.Setup(ref renderingData);
		bool flag2 = m_AdditionalLightsShadowCasterPass.Setup(ref renderingData);
		bool flag3 = m_TransparentSettingsPass.Setup(ref renderingData);
		bool flag4 = requiresDepthTexture && !CanCopyDepth(ref renderingData.cameraData);
		flag4 = flag4 || isSceneViewCamera;
		flag4 = flag4 || isPreviewCamera;
		m_CopyDepthPass.renderPassEvent = ((!requiresDepthTexture && (postProcessEnabled || isSceneViewCamera)) ? RenderPassEvent.AfterRenderingTransparents : RenderPassEvent.AfterRenderingOpaques);
		if (isStereoEnabled && requiresDepthTexture)
		{
			flag4 = true;
		}
		UniversalRenderPipeline.IsRunningHololens(camera);
		bool flag5 = RequiresIntermediateColorTexture(ref cameraData);
		flag5 = flag5 && !isPreviewCamera;
		bool flag6 = cameraData.requiresDepthTexture && !flag4;
		flag6 |= cameraData.renderType == CameraRenderType.Base && !cameraData.resolveFinalTarget;
		if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Vulkan)
		{
			flag5 = flag5 || flag6;
		}
		if (cameraData.renderType == CameraRenderType.Base)
		{
			m_ActiveCameraColorAttachment = (flag5 ? m_CameraColorAttachment : RenderTargetHandle.CameraTarget);
			m_ActiveCameraDepthAttachment = (flag6 ? m_CameraDepthAttachment : RenderTargetHandle.CameraTarget);
			bool num = flag5 || flag6;
			if (num)
			{
				CreateCameraRenderTarget(context, ref renderingData.cameraData);
			}
			int msaaSamples = (num ? 1 : baseDescriptor.msaaSamples);
			if (mainCamera == null)
			{
				mainCamera = Camera.main;
			}
			if (mainCamera == camera && camera.cameraType == CameraType.Game && cameraData.targetTexture == null)
			{
				SetupBackbufferFormat(msaaSamples, isStereoEnabled);
			}
		}
		else
		{
			m_ActiveCameraColorAttachment = ((cameraData.renderType == CameraRenderType.UI) ? RenderTargetHandle.CameraTarget : m_CameraColorAttachment);
			m_ActiveCameraDepthAttachment = ((cameraData.renderType == CameraRenderType.UI) ? RenderTargetHandle.CameraTarget : m_CameraDepthAttachment);
		}
		ConfigureCameraTarget(m_ActiveCameraColorAttachment.Identifier(), m_ActiveCameraDepthAttachment.Identifier());
		for (int j = 0; j < base.rendererFeatures.Count; j++)
		{
			if (base.rendererFeatures[j].isActive)
			{
				base.rendererFeatures[j].AddRenderPasses(this, ref renderingData);
			}
		}
		for (int num2 = base.activeRenderPassQueue.Count - 1; num2 >= 0; num2--)
		{
			if (base.activeRenderPassQueue[num2] == null)
			{
				base.activeRenderPassQueue.RemoveAt(num2);
			}
		}
		bool flag7 = base.activeRenderPassQueue.Find((ScriptableRenderPass x) => x.renderPassEvent == RenderPassEvent.AfterRendering) != null;
		if (flag)
		{
			EnqueuePass(m_MainLightShadowCasterPass);
		}
		if (flag2)
		{
			EnqueuePass(m_AdditionalLightsShadowCasterPass);
		}
		if (flag4)
		{
			m_DepthPrepass.Setup(baseDescriptor, m_DepthTexture);
			EnqueuePass(m_DepthPrepass);
		}
		if (postProcessEnabled2)
		{
			m_ColorGradingLutPass.Setup(in m_ColorGradingLut);
			EnqueuePass(m_ColorGradingLutPass);
		}
		EnqueuePass(m_RenderOpaqueForwardPass);
		cameraData.camera.TryGetComponent<Skybox>(out var component);
		bool flag8 = cameraData.renderType == CameraRenderType.Overlay || cameraData.renderType == CameraRenderType.UI;
		if (camera.clearFlags == CameraClearFlags.Skybox && (RenderSettings.skybox != null || component?.material != null) && !flag8)
		{
			EnqueuePass(m_DrawSkyboxPass);
		}
		if (!flag4 && renderingData.cameraData.requiresDepthTexture && flag6)
		{
			m_CopyDepthPass.Setup(m_ActiveCameraDepthAttachment, m_DepthTexture);
			EnqueuePass(m_CopyDepthPass);
		}
		if (renderingData.cameraData.requiresOpaqueTexture)
		{
			Downsampling opaqueDownsampling = UniversalRenderPipeline.asset.opaqueDownsampling;
			m_CopyColorPass.Setup(m_ActiveCameraColorAttachment.Identifier(), m_OpaqueColor, opaqueDownsampling);
			EnqueuePass(m_CopyColorPass);
		}
		if (flag3)
		{
			EnqueuePass(m_TransparentSettingsPass);
		}
		EnqueuePass(m_RenderTransparentForwardPass);
		EnqueuePass(m_OnRenderObjectCallbackPass);
		bool resolveFinalTarget = cameraData.resolveFinalTarget;
		bool num3 = renderingData.cameraData.captureActions != null && resolveFinalTarget;
		bool flag9 = postProcessingEnabled && resolveFinalTarget && ((renderingData.cameraData.antialiasing == AntialiasingMode.FastApproximateAntialiasing && !cameraData.doFxaaInUberPost) || m_PostProcessPass.IsFinalPostProcessingRequired());
		bool flag10 = !num3 && !flag7 && !flag9;
		if (resolveFinalTarget)
		{
			if (postProcessEnabled)
			{
				RenderTargetHandle destination = (flag10 ? RenderTargetHandle.CameraTarget : m_AfterPostProcessColor);
				bool enableSRGBConversion = flag10;
				m_PostProcessPass.Setup(in baseDescriptor, in m_ActiveCameraColorAttachment, in destination, in m_ActiveCameraDepthAttachment, in m_ColorGradingLut, flag9, enableSRGBConversion);
				EnqueuePass(m_PostProcessPass);
			}
			if (renderingData.cameraData.captureActions != null)
			{
				m_CapturePass.Setup(m_ActiveCameraColorAttachment);
				EnqueuePass(m_CapturePass);
			}
			RenderTargetHandle source = (postProcessEnabled ? m_AfterPostProcessColor : m_ActiveCameraColorAttachment);
			if (flag9)
			{
				m_FinalPostProcessPass.SetupFinalPass(in baseDescriptor, in source);
				EnqueuePass(m_FinalPostProcessPass);
			}
			if (!flag9 && (!postProcessEnabled || flag7) && !(m_ActiveCameraColorAttachment == RenderTargetHandle.CameraTarget))
			{
				if (!postProcessingEnabled)
				{
					m_FinalBlitPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
				}
				m_FinalBlitPass.Setup(baseDescriptor, source);
				EnqueuePass(m_FinalBlitPass);
			}
		}
		else if (postProcessEnabled)
		{
			m_PostProcessPass.Setup(in baseDescriptor, in m_ActiveCameraColorAttachment, in m_AfterPostProcessColor, in m_ActiveCameraDepthAttachment, in m_ColorGradingLut, hasFinalPass: false, enableSRGBConversion: false);
			EnqueuePass(m_PostProcessPass);
		}
	}

	public override void SetupLights(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		m_ForwardLights.Setup(context, ref renderingData);
	}

	public override void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData)
	{
		bool num = !UniversalRenderPipeline.asset.supportsMainLightShadows && !UniversalRenderPipeline.asset.supportsAdditionalLightShadows;
		bool flag = Mathf.Approximately(cameraData.maxShadowDistance, 0f);
		if (num || flag)
		{
			cullingParameters.cullingOptions &= ~CullingOptions.ShadowCasters;
		}
		cullingParameters.maximumVisibleLights = UniversalRenderPipeline.maxVisibleAdditionalLights + 1;
		cullingParameters.shadowDistance = cameraData.maxShadowDistance;
	}

	public override void FinishRendering(CommandBuffer cmd)
	{
		if (m_ActiveCameraColorAttachment != RenderTargetHandle.CameraTarget)
		{
			cmd.ReleaseTemporaryRT(m_ActiveCameraColorAttachment.id);
			m_ActiveCameraColorAttachment = RenderTargetHandle.CameraTarget;
		}
		if (m_ActiveCameraDepthAttachment != RenderTargetHandle.CameraTarget)
		{
			cmd.ReleaseTemporaryRT(m_ActiveCameraDepthAttachment.id);
			m_ActiveCameraDepthAttachment = RenderTargetHandle.CameraTarget;
		}
	}

	private void CreateCameraRenderTarget(ScriptableRenderContext context, ref CameraData cameraData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("Create Camera Texture");
		RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;
		_ = cameraTargetDescriptor.msaaSamples;
		if (m_ActiveCameraColorAttachment != RenderTargetHandle.CameraTarget)
		{
			bool flag = m_ActiveCameraDepthAttachment == RenderTargetHandle.CameraTarget;
			RenderTextureDescriptor desc = cameraTargetDescriptor;
			desc.useMipMap = false;
			desc.autoGenerateMips = false;
			desc.depthBufferBits = (flag ? 32 : 0);
			commandBuffer.GetTemporaryRT(m_ActiveCameraColorAttachment.id, desc, FilterMode.Bilinear);
		}
		if (m_ActiveCameraDepthAttachment != RenderTargetHandle.CameraTarget)
		{
			RenderTextureDescriptor desc2 = cameraTargetDescriptor;
			desc2.useMipMap = false;
			desc2.autoGenerateMips = false;
			desc2.colorFormat = RenderTextureFormat.Depth;
			desc2.depthBufferBits = 32;
			commandBuffer.GetTemporaryRT(m_ActiveCameraDepthAttachment.id, desc2, FilterMode.Point);
		}
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	private void SetupBackbufferFormat(int msaaSamples, bool stereo)
	{
		if (stereo)
		{
			bool flag = false;
			int antiAliasing = QualitySettings.antiAliasing;
			if (antiAliasing != msaaSamples && (antiAliasing != 0 || msaaSamples != 1))
			{
				flag = true;
			}
			if (flag)
			{
				QualitySettings.antiAliasing = msaaSamples;
				XRDevice.UpdateEyeTextureMSAASetting();
			}
		}
	}

	private bool PlatformRequiresExplicitMsaaResolve()
	{
		if (!SystemInfo.supportsMultisampleAutoResolve)
		{
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal)
			{
				return !Application.isMobilePlatform;
			}
			return true;
		}
		return false;
	}

	private bool RequiresIntermediateColorTexture(ref CameraData cameraData)
	{
		if (cameraData.renderType == CameraRenderType.Base && !cameraData.resolveFinalTarget)
		{
			return true;
		}
		bool isSceneViewCamera = cameraData.isSceneViewCamera;
		RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;
		int msaaSamples = cameraTargetDescriptor.msaaSamples;
		bool isStereoEnabled = cameraData.isStereoEnabled;
		bool flag = !Mathf.Approximately(cameraData.renderScale, 1f) && !cameraData.isStereoEnabled;
		bool flag2 = cameraTargetDescriptor.dimension == TextureDimension.Tex2D;
		bool flag3 = msaaSamples > 1 && PlatformRequiresExplicitMsaaResolve();
		bool num = cameraData.targetTexture != null && !isSceneViewCamera;
		bool flag4 = cameraData.captureActions != null;
		if (isStereoEnabled)
		{
			flag2 = XRSettings.deviceEyeTextureDimension == cameraTargetDescriptor.dimension;
		}
		bool flag5 = cameraData.postProcessEnabled || cameraData.requiresOpaqueTexture || flag3 || !cameraData.isDefaultViewport;
		if (num)
		{
			return flag5;
		}
		if (!(flag5 || isSceneViewCamera || flag || cameraData.isHdrEnabled || !flag2 || flag4))
		{
			if (Display.main.requiresBlitToBackbuffer)
			{
				return !isStereoEnabled;
			}
			return false;
		}
		return true;
	}

	private bool CanCopyDepth(ref CameraData cameraData)
	{
		bool num = cameraData.cameraTargetDescriptor.msaaSamples > 1;
		bool flag = SystemInfo.copyTextureSupport != CopyTextureSupport.None;
		bool flag2 = RenderingUtils.SupportsRenderTextureFormat(RenderTextureFormat.Depth);
		bool num2 = !num && (flag2 || flag);
		bool flag3 = false;
		return num2 || flag3;
	}
}
