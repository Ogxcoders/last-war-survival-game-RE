using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.XR;

namespace UnityEngine.Rendering.Universal;

public sealed class UniversalRenderPipeline : RenderPipeline
{
	internal static class PerFrameBuffer
	{
		public static int _GlossyEnvironmentColor;

		public static int _SubtractiveShadowColor;

		public static int _Time;

		public static int _SinTime;

		public static int _CosTime;

		public static int unity_DeltaTime;

		public static int _TimeParameters;
	}

	public const string k_ShaderTagName = "UniversalPipeline";

	private const string k_RenderCameraTag = "Render Camera";

	private static ProfilingSampler _CameraProfilingSampler = new ProfilingSampler("Render Camera");

	private const int k_MaxVisibleAdditionalLightsMobile = 32;

	private const int k_MaxVisibleAdditionalLightsNonMobile = 256;

	private static List<XRDisplaySubsystem> xrDisplayList = new List<XRDisplaySubsystem>();

	private static bool xrSkipRender = false;

	private List<Camera> uiCameraList = new List<Camera>();

	private List<UniversalAdditionalCameraData> uiCameraAdditionalDataList = new List<UniversalAdditionalCameraData>();

	private static List<XRDisplaySubsystem> displaySubsystemList = new List<XRDisplaySubsystem>();

	private static List<Vector4> m_ShadowBiasData = new List<Vector4>();

	private Comparison<Camera> cameraComparison = (Camera camera1, Camera camera2) => (int)camera1.depth - (int)camera2.depth;

	private static Lightmapping.RequestLightsDelegate lightsDelegate = delegate(Light[] requests, NativeArray<LightDataGI> lightsOutput)
	{
		LightDataGI value = default(LightDataGI);
		for (int i = 0; i < requests.Length; i++)
		{
			Light light = requests[i];
			value.InitNoBake(light.GetInstanceID());
			lightsOutput[i] = value;
		}
		Debug.LogWarning("Realtime GI is not supported in Universal Pipeline.");
	};

	public static float maxShadowBias => 10f;

	public static float minRenderScale => 0.1f;

	public static float maxRenderScale => 2f;

	public static int maxPerObjectLights
	{
		get
		{
			if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2)
			{
				return 8;
			}
			return 4;
		}
	}

	public static int maxVisibleAdditionalLights
	{
		get
		{
			if (!Application.isMobilePlatform && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3)
			{
				return 256;
			}
			return 32;
		}
	}

	internal static int maxScriptableRenderers => 8;

	public static UniversalRenderPipelineAsset asset => GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;

	public UniversalRenderPipeline(UniversalRenderPipelineAsset asset)
	{
		SetSupportedRenderingFeatures();
		PerFrameBuffer._GlossyEnvironmentColor = Shader.PropertyToID("_GlossyEnvironmentColor");
		PerFrameBuffer._SubtractiveShadowColor = Shader.PropertyToID("_SubtractiveShadowColor");
		PerFrameBuffer._Time = Shader.PropertyToID("_Time");
		PerFrameBuffer._SinTime = Shader.PropertyToID("_SinTime");
		PerFrameBuffer._CosTime = Shader.PropertyToID("_CosTime");
		PerFrameBuffer.unity_DeltaTime = Shader.PropertyToID("unity_DeltaTime");
		PerFrameBuffer._TimeParameters = Shader.PropertyToID("_TimeParameters");
		if (((QualitySettings.antiAliasing <= 0) ? 1 : QualitySettings.antiAliasing) != asset.msaaSampleCount)
		{
			QualitySettings.antiAliasing = asset.msaaSampleCount;
			XRDevice.UpdateEyeTextureMSAASetting();
		}
		XRGraphics.eyeTextureResolutionScale = asset.renderScale;
		Shader.globalRenderPipeline = "UniversalPipeline,LightweightPipeline";
		Lightmapping.SetDelegate(lightsDelegate);
		CameraCaptureBridge.enabled = true;
		RenderingUtils.ClearSystemInfoCache();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		Shader.globalRenderPipeline = "";
		SupportedRenderingFeatures.active = new SupportedRenderingFeatures();
		ShaderData.instance.Dispose();
		Lightmapping.ResetDelegate();
		CameraCaptureBridge.enabled = false;
	}

	internal void SetupXRStates()
	{
		SubsystemManager.GetInstances(xrDisplayList);
		if (xrDisplayList.Count <= 0)
		{
			return;
		}
		if (xrDisplayList.Count > 1)
		{
			throw new NotImplementedException("Only 1 XR display is supported.");
		}
		if (xrDisplayList[0].GetRenderPassCount() == 0)
		{
			if (!xrSkipRender)
			{
				xrSkipRender = true;
				Debug.Log("XR display is not ready. Skip XR rendering.");
			}
		}
		else if (xrSkipRender)
		{
			xrSkipRender = false;
			Debug.Log("XR display is ready. Start XR rendering.");
		}
	}

	protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
	{
		RenderPipeline.BeginFrameRendering(renderContext, cameras);
		GraphicsSettings.lightsUseLinearIntensity = QualitySettings.activeColorSpace == ColorSpace.Linear;
		GraphicsSettings.useScriptableRenderPipelineBatching = asset.useSRPBatcher;
		SetupPerFrameShaderConstants();
		SetupXRStates();
		SortCameras(cameras);
		uiCameraList.Clear();
		uiCameraAdditionalDataList.Clear();
		foreach (Camera camera in cameras)
		{
			if (IsStereoEnabled(camera) && xrSkipRender)
			{
				continue;
			}
			if (IsGameCamera(camera))
			{
				camera.TryGetComponent<UniversalAdditionalCameraData>(out var component);
				if (!(component != null) || !component.disableRender)
				{
					if (component != null && component.renderType == CameraRenderType.UI)
					{
						uiCameraList.Add(camera);
						uiCameraAdditionalDataList.Add(component);
					}
					else
					{
						RenderCameraStack(renderContext, camera, component);
					}
				}
			}
			else
			{
				RenderPipeline.BeginCameraRendering(renderContext, camera);
				UpdateVolumeFramework(camera, null);
				RenderSingleCamera(renderContext, camera);
				RenderPipeline.EndCameraRendering(renderContext, camera);
			}
		}
		for (int j = 0; j < uiCameraList.Count; j++)
		{
			Camera camera2 = uiCameraList[j];
			UniversalAdditionalCameraData additionalCameraData = uiCameraAdditionalDataList[j];
			RenderPipeline.BeginCameraRendering(renderContext, camera2);
			RenderSingleCamera(renderContext, camera2, additionalCameraData);
			RenderPipeline.EndCameraRendering(renderContext, camera2);
		}
		RenderPipeline.EndFrameRendering(renderContext, cameras);
	}

	public static void RenderSingleCamera(ScriptableRenderContext context, Camera camera, UniversalAdditionalCameraData additionalCameraData)
	{
		InitializeCameraData(camera, additionalCameraData, resolveFinalTarget: true, out var cameraData);
		RenderSingleCamera(context, cameraData, cameraData.postProcessEnabled);
	}

	public static void RenderSingleCamera(ScriptableRenderContext context, Camera camera)
	{
		UniversalAdditionalCameraData component = null;
		if (IsGameCamera(camera))
		{
			camera.gameObject.TryGetComponent<UniversalAdditionalCameraData>(out component);
		}
		if (component != null && component.renderType != CameraRenderType.Base)
		{
			Debug.LogWarning("Only Base cameras can be rendered with standalone RenderSingleCamera. Camera will be skipped.");
			return;
		}
		InitializeCameraData(camera, component, resolveFinalTarget: true, out var cameraData);
		RenderSingleCamera(context, cameraData, cameraData.postProcessEnabled);
	}

	private static void RenderSingleCamera(ScriptableRenderContext context, CameraData cameraData, bool anyPostProcessingEnabled)
	{
		Camera camera = cameraData.camera;
		ScriptableRenderer renderer = cameraData.renderer;
		ScriptableCullingParameters cullingParameters;
		if (renderer == null)
		{
			Debug.LogWarning($"Trying to render {camera.name} with an invalid renderer. Camera rendering will be skipped.");
		}
		else if (camera.TryGetCullingParameters(IsStereoEnabled(camera), out cullingParameters))
		{
			ScriptableRenderer.current = renderer;
			ProfilingSampler profilingSampler = ((asset.debugLevel >= PipelineDebugLevel.Profiling) ? new ProfilingSampler(camera.name) : _CameraProfilingSampler);
			CommandBuffer commandBuffer = CommandBufferPool.Get(profilingSampler.name);
			using (new ProfilingScope(commandBuffer, profilingSampler))
			{
				renderer.Clear(cameraData.renderType);
				renderer.SetupCullingParameters(ref cullingParameters, ref cameraData);
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				CullingResults cullResults = context.Cull(ref cullingParameters);
				InitializeRenderingData(asset, ref cameraData, ref cullResults, anyPostProcessingEnabled, out var renderingData);
				renderer.Setup(context, ref renderingData);
				renderer.Execute(context, ref renderingData);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
			context.Submit();
			ScriptableRenderer.current = null;
		}
	}

	private static void RenderCameraStack(ScriptableRenderContext context, Camera baseCamera, UniversalAdditionalCameraData baseCameraAdditionalData)
	{
		if (baseCameraAdditionalData != null && (baseCameraAdditionalData.renderType == CameraRenderType.Overlay || baseCameraAdditionalData.renderType == CameraRenderType.UI))
		{
			return;
		}
		ScriptableRenderer scriptableRenderer = baseCameraAdditionalData?.scriptableRenderer;
		List<Camera> list = ((scriptableRenderer == null || !scriptableRenderer.supportedRenderingFeatures.cameraStacking) ? null : baseCameraAdditionalData?.cameraStack);
		bool flag = baseCameraAdditionalData != null && baseCameraAdditionalData.renderPostProcessing;
		flag &= SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2;
		flag &= asset.supportsPostProcess;
		int num = -1;
		if (list != null && list.Count > 0)
		{
			if (!IsMultiPassStereoEnabled(baseCamera))
			{
				Type type = baseCameraAdditionalData?.scriptableRenderer.GetType();
				bool flag2 = false;
				for (int i = 0; i < list.Count; i++)
				{
					Camera camera = list[i];
					if (camera == null)
					{
						flag2 = true;
					}
					else
					{
						if (!camera.isActiveAndEnabled)
						{
							continue;
						}
						camera.TryGetComponent<UniversalAdditionalCameraData>(out var component);
						if (component == null || component.renderType != CameraRenderType.Overlay)
						{
							Debug.LogWarning($"Stack can only contain Overlay cameras. {camera.name} will skip rendering.");
							continue;
						}
						Type type2 = component?.scriptableRenderer.GetType();
						if (type2 != type)
						{
							Type typeFromHandle = typeof(Renderer2D);
							if (type2 != typeFromHandle && type != typeFromHandle)
							{
								Debug.LogWarning($"Only cameras with compatible renderer types can be stacked. {camera.name} will skip rendering");
								continue;
							}
						}
						flag |= component.renderPostProcessing;
						num = i;
					}
				}
				if (flag2)
				{
					baseCameraAdditionalData.UpdateCameraStack();
				}
			}
			else
			{
				Debug.LogWarning("Multi pass stereo mode doesn't support Camera Stacking. Overlay cameras will skip rendering.");
			}
		}
		bool flag3 = num != -1;
		RenderPipeline.BeginCameraRendering(context, baseCamera);
		UpdateVolumeFramework(baseCamera, baseCameraAdditionalData);
		InitializeCameraData(baseCamera, baseCameraAdditionalData, !flag3, out var cameraData);
		RenderSingleCamera(context, cameraData, flag);
		RenderPipeline.EndCameraRendering(context, baseCamera);
		if (!flag3)
		{
			return;
		}
		for (int j = 0; j < list.Count; j++)
		{
			Camera camera2 = list[j];
			if (camera2.isActiveAndEnabled)
			{
				camera2.TryGetComponent<UniversalAdditionalCameraData>(out var component2);
				if (component2 != null)
				{
					CameraData cameraData2 = cameraData;
					bool resolveFinalTarget = j == num;
					RenderPipeline.BeginCameraRendering(context, camera2);
					UpdateVolumeFramework(camera2, component2);
					InitializeAdditionalCameraData(camera2, component2, resolveFinalTarget, ref cameraData2);
					RenderSingleCamera(context, cameraData2, flag);
					RenderPipeline.EndCameraRendering(context, camera2);
				}
			}
		}
	}

	private static void UpdateVolumeFramework(Camera camera, UniversalAdditionalCameraData additionalCameraData)
	{
		LayerMask layerMask = 1;
		Transform transform = camera.transform;
		if (additionalCameraData != null)
		{
			layerMask = additionalCameraData.volumeLayerMask;
			transform = ((additionalCameraData.volumeTrigger != null) ? additionalCameraData.volumeTrigger : transform);
		}
		else if (camera.cameraType == CameraType.SceneView)
		{
			Camera main = Camera.main;
			UniversalAdditionalCameraData component = null;
			if (main != null && main.TryGetComponent<UniversalAdditionalCameraData>(out component))
			{
				layerMask = component.volumeLayerMask;
			}
			transform = ((component != null && component.volumeTrigger != null) ? component.volumeTrigger : transform);
		}
		VolumeManager.instance.Update(transform, layerMask);
	}

	private static bool CheckPostProcessForDepth(in CameraData cameraData)
	{
		if (!cameraData.postProcessEnabled)
		{
			return false;
		}
		if (cameraData.antialiasing == AntialiasingMode.SubpixelMorphologicalAntiAliasing)
		{
			return true;
		}
		VolumeStack stack = VolumeManager.instance.stack;
		if (stack.GetComponent<DepthOfField>().IsActive())
		{
			return true;
		}
		if (stack.GetComponent<MotionBlur>().IsActive())
		{
			return true;
		}
		return false;
	}

	private static void SetSupportedRenderingFeatures()
	{
	}

	private static void InitializeCameraData(Camera camera, UniversalAdditionalCameraData additionalCameraData, bool resolveFinalTarget, out CameraData cameraData)
	{
		cameraData = default(CameraData);
		InitializeStackedCameraData(camera, additionalCameraData, ref cameraData);
		InitializeAdditionalCameraData(camera, additionalCameraData, resolveFinalTarget, ref cameraData);
	}

	private static bool CanXRSDKUseSinglePass(Camera camera)
	{
		XRDisplaySubsystem xRDisplaySubsystem = null;
		SubsystemManager.GetInstances(displaySubsystemList);
		if (displaySubsystemList.Count > 0)
		{
			xRDisplaySubsystem = displaySubsystemList[0];
			if (xRDisplaySubsystem.GetRenderPassCount() > 0)
			{
				xRDisplaySubsystem.GetRenderPass(0, out var renderPass);
				if (renderPass.renderTargetDesc.dimension != TextureDimension.Tex2DArray)
				{
					return false;
				}
				if (renderPass.GetRenderParameterCount() != 2 || renderPass.renderTargetDesc.volumeDepth != 2)
				{
					return false;
				}
				renderPass.GetRenderParameter(camera, 0, out var renderParameter);
				renderPass.GetRenderParameter(camera, 1, out var renderParameter2);
				if (renderParameter.textureArraySlice != 0 || renderParameter2.textureArraySlice != 1)
				{
					return false;
				}
				if (renderParameter.viewport != renderParameter2.viewport)
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}

	private static void InitializeStackedCameraData(Camera baseCamera, UniversalAdditionalCameraData baseAdditionalCameraData, ref CameraData cameraData)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = asset;
		cameraData.targetTexture = baseCamera.targetTexture;
		cameraData.isStereoEnabled = IsStereoEnabled(baseCamera);
		cameraData.cameraType = baseCamera.cameraType;
		cameraData.isSceneViewCamera = cameraData.cameraType == CameraType.SceneView;
		cameraData.numberOfXRPasses = 1;
		cameraData.isXRMultipass = false;
		bool isSceneViewCamera = cameraData.isSceneViewCamera;
		if (cameraData.isStereoEnabled && !isSceneViewCamera && !CanXRSDKUseSinglePass(baseCamera) && XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.MultiPass)
		{
			cameraData.numberOfXRPasses = 2;
			cameraData.isXRMultipass = true;
		}
		cameraData.doFxaaInUberPost = universalRenderPipelineAsset.doFxaaInUberPost;
		if (isSceneViewCamera)
		{
			cameraData.volumeLayerMask = 1;
			cameraData.volumeTrigger = null;
			cameraData.isStopNaNEnabled = false;
			cameraData.isDitheringEnabled = false;
			cameraData.antialiasing = AntialiasingMode.None;
			cameraData.antialiasingQuality = AntialiasingQuality.High;
		}
		else if (baseAdditionalCameraData != null)
		{
			cameraData.volumeLayerMask = baseAdditionalCameraData.volumeLayerMask;
			cameraData.volumeTrigger = ((baseAdditionalCameraData.volumeTrigger == null) ? baseCamera.transform : baseAdditionalCameraData.volumeTrigger);
			cameraData.isStopNaNEnabled = baseAdditionalCameraData.stopNaN && SystemInfo.graphicsShaderLevel >= 35;
			cameraData.isDitheringEnabled = baseAdditionalCameraData.dithering;
			cameraData.antialiasing = ((baseAdditionalCameraData.antialiasing != AntialiasingMode.UsePipelineSettings) ? baseAdditionalCameraData.antialiasing : asset.antialiasingMode);
			cameraData.antialiasingQuality = ((baseAdditionalCameraData.antialiasing != AntialiasingMode.UsePipelineSettings) ? baseAdditionalCameraData.antialiasingQuality : asset.antialiasingQuality);
		}
		else
		{
			cameraData.volumeLayerMask = 1;
			cameraData.volumeTrigger = null;
			cameraData.isStopNaNEnabled = false;
			cameraData.isDitheringEnabled = false;
			cameraData.antialiasing = AntialiasingMode.None;
			cameraData.antialiasingQuality = AntialiasingQuality.High;
		}
		int msaaSamples = 1;
		if (baseCamera.allowMSAA && universalRenderPipelineAsset.msaaSampleCount > 1)
		{
			msaaSamples = ((baseCamera.targetTexture != null) ? baseCamera.targetTexture.antiAliasing : universalRenderPipelineAsset.msaaSampleCount);
		}
		cameraData.isHdrEnabled = baseCamera.allowHDR && universalRenderPipelineAsset.supportsHDR;
		Rect rect = baseCamera.rect;
		cameraData.pixelRect = baseCamera.pixelRect;
		cameraData.pixelWidth = baseCamera.pixelWidth;
		cameraData.pixelHeight = baseCamera.pixelHeight;
		cameraData.aspectRatio = (float)cameraData.pixelWidth / (float)cameraData.pixelHeight;
		cameraData.isDefaultViewport = !(Math.Abs(rect.x) > 0f) && !(Math.Abs(rect.y) > 0f) && !(Math.Abs(rect.width) < 1f) && !(Math.Abs(rect.height) < 1f);
		float num = (XRGraphics.enabled ? XRGraphics.eyeTextureResolutionScale : universalRenderPipelineAsset.renderScale);
		cameraData.renderScale = ((Mathf.Abs(1f - num) < 0.05f) ? 1f : num);
		if (cameraData.renderType == CameraRenderType.Base)
		{
			int num2 = Mathf.Min(cameraData.pixelWidth, cameraData.pixelHeight);
			int num3 = Mathf.Max(cameraData.pixelWidth, cameraData.pixelHeight);
			float num4 = (float)num2 * cameraData.renderScale;
			int num5 = Mathf.Min(universalRenderPipelineAsset.minRenderResolution, num2);
			if (num4 < (float)num5)
			{
				num4 = num5;
			}
			if (num4 > (float)universalRenderPipelineAsset.maxRenderResolution)
			{
				num4 = universalRenderPipelineAsset.maxRenderResolution;
			}
			bool flag = num2 > universalRenderPipelineAsset.foldableResolution && num3 > universalRenderPipelineAsset.foldableResolution && (float)num2 / (float)num3 >= universalRenderPipelineAsset.foldableAspectRatio;
			cameraData.renderScale = (flag ? universalRenderPipelineAsset.foldableRenderScale : (num4 / (float)num2));
		}
		SortingCriteria sortingCriteria = SortingCriteria.CommonOpaque;
		SortingCriteria sortingCriteria2 = SortingCriteria.SortingLayer | SortingCriteria.RenderQueue | SortingCriteria.OptimizeStateChanges | SortingCriteria.CanvasOrder;
		bool hasHiddenSurfaceRemovalOnGPU = SystemInfo.hasHiddenSurfaceRemovalOnGPU;
		bool flag2 = (baseCamera.opaqueSortMode == OpaqueSortMode.Default && hasHiddenSurfaceRemovalOnGPU) || baseCamera.opaqueSortMode == OpaqueSortMode.NoDistanceSort;
		cameraData.defaultOpaqueSortFlags = (flag2 ? sortingCriteria2 : sortingCriteria);
		cameraData.captureActions = CameraCaptureBridge.GetCaptureActions(baseCamera);
		bool preserveFramebufferAlpha = Graphics.preserveFramebufferAlpha;
		cameraData.cameraTargetDescriptor = CreateRenderTextureDescriptor(baseCamera, cameraData.renderScale, cameraData.isStereoEnabled, cameraData.isHdrEnabled, msaaSamples, preserveFramebufferAlpha);
	}

	private static void InitializeAdditionalCameraData(Camera camera, UniversalAdditionalCameraData additionalCameraData, bool resolveFinalTarget, ref CameraData cameraData)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = asset;
		cameraData.camera = camera;
		bool flag = universalRenderPipelineAsset.supportsMainLightShadows || universalRenderPipelineAsset.supportsAdditionalLightShadows;
		cameraData.maxShadowDistance = Mathf.Min(universalRenderPipelineAsset.shadowDistance, camera.farClipPlane);
		cameraData.maxShadowDistance = ((flag && cameraData.maxShadowDistance >= camera.nearClipPlane) ? cameraData.maxShadowDistance : 0f);
		bool isSceneViewCamera = cameraData.isSceneViewCamera;
		if (isSceneViewCamera)
		{
			cameraData.renderType = CameraRenderType.Base;
			cameraData.clearDepth = true;
			cameraData.postProcessEnabled = CoreUtils.ArePostProcessesEnabled(camera);
			cameraData.requiresDepthTexture = universalRenderPipelineAsset.supportsCameraDepthTexture || universalRenderPipelineAsset.allowSoftParticles;
			cameraData.requiresOpaqueTexture = universalRenderPipelineAsset.supportsCameraOpaqueTexture;
			cameraData.renderer = asset.scriptableRenderer;
		}
		else if (additionalCameraData != null)
		{
			cameraData.renderType = additionalCameraData.renderType;
			cameraData.clearDepth = additionalCameraData.renderType == CameraRenderType.Base || additionalCameraData.clearDepth;
			cameraData.postProcessEnabled = additionalCameraData.renderType != CameraRenderType.UI && additionalCameraData.renderPostProcessing;
			cameraData.maxShadowDistance = ((additionalCameraData.renderType != CameraRenderType.UI && additionalCameraData.renderShadows) ? cameraData.maxShadowDistance : 0f);
			cameraData.requiresDepthTexture = additionalCameraData.requiresDepthTexture;
			cameraData.requiresOpaqueTexture = additionalCameraData.requiresColorTexture;
			cameraData.renderer = additionalCameraData.scriptableRenderer;
		}
		else
		{
			cameraData.renderType = CameraRenderType.Base;
			cameraData.clearDepth = true;
			cameraData.postProcessEnabled = false;
			cameraData.requiresDepthTexture = universalRenderPipelineAsset.supportsCameraDepthTexture || universalRenderPipelineAsset.allowSoftParticles;
			cameraData.requiresOpaqueTexture = universalRenderPipelineAsset.supportsCameraOpaqueTexture;
			cameraData.renderer = asset.scriptableRenderer;
		}
		int num;
		if (cameraData.renderType != CameraRenderType.Overlay)
		{
			num = ((cameraData.renderType == CameraRenderType.UI) ? 1 : 0);
			if (num == 0)
			{
				goto IL_01b5;
			}
		}
		else
		{
			num = 1;
		}
		cameraData.requiresDepthTexture = false;
		cameraData.requiresOpaqueTexture = false;
		goto IL_01b5;
		IL_01b5:
		cameraData.postProcessEnabled &= SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2;
		cameraData.postProcessEnabled &= universalRenderPipelineAsset.supportsPostProcess;
		bool flag2 = CheckPostProcessForDepth(in cameraData);
		cameraData.requiresDepthTexture |= isSceneViewCamera || flag2;
		cameraData.resolveFinalTarget = resolveFinalTarget;
		Matrix4x4 projectionMatrix = camera.projectionMatrix;
		if (num != 0 && !camera.orthographic && !cameraData.isStereoEnabled && cameraData.pixelRect != camera.pixelRect)
		{
			float m = camera.projectionMatrix.m00 * camera.aspect / cameraData.aspectRatio;
			projectionMatrix.m00 = m;
		}
		cameraData.SetViewAndProjectionMatrix(camera.worldToCameraMatrix, projectionMatrix);
	}

	private static void InitializeRenderingData(UniversalRenderPipelineAsset settings, ref CameraData cameraData, ref CullingResults cullResults, bool anyPostProcessingEnabled, out RenderingData renderingData)
	{
		NativeArray<VisibleLight> visibleLights = cullResults.visibleLights;
		int mainLightIndex = GetMainLightIndex(settings, visibleLights);
		bool mainLightCastShadows = false;
		bool flag = false;
		if (cameraData.maxShadowDistance > 0f)
		{
			mainLightCastShadows = mainLightIndex != -1 && visibleLights[mainLightIndex].light != null && visibleLights[mainLightIndex].light.shadows != LightShadows.None;
			if (settings.additionalLightsRenderingMode == LightRenderingMode.PerPixel)
			{
				for (int i = 0; i < visibleLights.Length; i++)
				{
					if (i != mainLightIndex)
					{
						Light light = visibleLights[i].light;
						if (visibleLights[i].lightType == LightType.Spot && light != null && light.shadows != LightShadows.None)
						{
							flag = true;
							break;
						}
					}
				}
			}
		}
		renderingData.cullResults = cullResults;
		renderingData.cameraData = cameraData;
		InitializeLightData(settings, visibleLights, mainLightIndex, out renderingData.lightData);
		InitializeShadowData(settings, visibleLights, mainLightCastShadows, flag && !renderingData.lightData.shadeAdditionalLightsPerVertex, out renderingData.shadowData);
		InitializePostProcessingData(settings, out renderingData.postProcessingData);
		renderingData.supportsDynamicBatching = settings.supportsDynamicBatching;
		renderingData.perObjectData = GetPerObjectLightFlags(renderingData.lightData.additionalLightsCount);
		renderingData.postProcessingEnabled = anyPostProcessingEnabled;
		renderingData.killAlphaInFinalBlit = false;
	}

	private static void InitializeShadowData(UniversalRenderPipelineAsset settings, NativeArray<VisibleLight> visibleLights, bool mainLightCastShadows, bool additionalLightsCastShadows, out ShadowData shadowData)
	{
		m_ShadowBiasData.Clear();
		for (int i = 0; i < visibleLights.Length; i++)
		{
			Light light = visibleLights[i].light;
			UniversalAdditionalLightData component = null;
			if (light != null)
			{
				light.gameObject.TryGetComponent<UniversalAdditionalLightData>(out component);
			}
			if ((bool)component && !component.usePipelineSettings)
			{
				m_ShadowBiasData.Add(new Vector4(light.shadowBias, light.shadowNormalBias, 0f, 0f));
			}
			else
			{
				m_ShadowBiasData.Add(new Vector4(settings.shadowDepthBias, settings.shadowNormalBias, 0f, 0f));
			}
		}
		shadowData.bias = m_ShadowBiasData;
		shadowData.supportsMainLightShadows = SystemInfo.supportsShadows && settings.supportsMainLightShadows && mainLightCastShadows;
		shadowData.requiresScreenSpaceShadowResolve = false;
		shadowData.mainLightShadowCascadesCount = settings.shadowCascadeOption switch
		{
			ShadowCascadesOption.FourCascades => 4, 
			ShadowCascadesOption.TwoCascades => 2, 
			_ => 1, 
		};
		shadowData.mainLightShadowmapWidth = settings.mainLightShadowmapResolution;
		shadowData.mainLightShadowmapHeight = settings.mainLightShadowmapResolution;
		switch (shadowData.mainLightShadowCascadesCount)
		{
		case 1:
			shadowData.mainLightShadowCascadesSplit = new Vector3(1f, 0f, 0f);
			break;
		case 2:
			shadowData.mainLightShadowCascadesSplit = new Vector3(settings.cascade2Split, 1f, 0f);
			break;
		default:
			shadowData.mainLightShadowCascadesSplit = settings.cascade4Split;
			break;
		}
		shadowData.supportsAdditionalLightShadows = SystemInfo.supportsShadows && settings.supportsAdditionalLightShadows && additionalLightsCastShadows;
		shadowData.additionalLightsShadowmapWidth = (shadowData.additionalLightsShadowmapHeight = settings.additionalLightsShadowmapResolution);
		shadowData.supportsSoftShadows = settings.supportsSoftShadows && (shadowData.supportsMainLightShadows || shadowData.supportsAdditionalLightShadows);
		shadowData.shadowmapDepthBufferBits = 16;
	}

	private static void InitializePostProcessingData(UniversalRenderPipelineAsset settings, out PostProcessingData postProcessingData)
	{
		postProcessingData.gradingMode = (settings.supportsHDR ? settings.colorGradingMode : ColorGradingMode.LowDynamicRange);
		postProcessingData.lutSize = settings.colorGradingLutSize;
		postProcessingData.enableFxDistortion = settings.enableFxDistortionInPost;
	}

	private static void InitializeLightData(UniversalRenderPipelineAsset settings, NativeArray<VisibleLight> visibleLights, int mainLightIndex, out LightData lightData)
	{
		int val = maxPerObjectLights;
		int val2 = maxVisibleAdditionalLights;
		lightData.mainLightIndex = mainLightIndex;
		if (settings.additionalLightsRenderingMode != LightRenderingMode.Disabled)
		{
			lightData.additionalLightsCount = Math.Min((mainLightIndex != -1) ? (visibleLights.Length - 1) : visibleLights.Length, val2);
			lightData.maxPerObjectAdditionalLightsCount = Math.Min(settings.maxAdditionalLightsCount, val);
		}
		else
		{
			lightData.additionalLightsCount = 0;
			lightData.maxPerObjectAdditionalLightsCount = 0;
		}
		lightData.shadeAdditionalLightsPerVertex = settings.additionalLightsRenderingMode == LightRenderingMode.PerVertex;
		lightData.visibleLights = visibleLights;
		lightData.supportsMixedLighting = settings.supportsMixedLighting;
	}

	private static PerObjectData GetPerObjectLightFlags(int additionalLightsCount)
	{
		PerObjectData perObjectData = PerObjectData.LightProbe | PerObjectData.ReflectionProbes | PerObjectData.Lightmaps | PerObjectData.LightData | PerObjectData.OcclusionProbe;
		if (additionalLightsCount > 0)
		{
			perObjectData |= PerObjectData.LightData;
			if (!RenderingUtils.useStructuredBuffer)
			{
				perObjectData |= PerObjectData.LightIndices;
			}
		}
		return perObjectData;
	}

	private static int GetMainLightIndex(UniversalRenderPipelineAsset settings, NativeArray<VisibleLight> visibleLights)
	{
		int length = visibleLights.Length;
		if (length == 0 || settings.mainLightRenderingMode != LightRenderingMode.PerPixel)
		{
			return -1;
		}
		Light sun = RenderSettings.sun;
		int result = -1;
		float num = 0f;
		for (int i = 0; i < length; i++)
		{
			VisibleLight visibleLight = visibleLights[i];
			Light light = visibleLight.light;
			if (light == null)
			{
				break;
			}
			if (visibleLight.lightType == LightType.Directional)
			{
				if (light == sun)
				{
					return i;
				}
				if (light.intensity > num)
				{
					num = light.intensity;
					result = i;
				}
			}
		}
		return result;
	}

	private static void SetupPerFrameShaderConstants()
	{
		SphericalHarmonicsL2 ambientProbe = RenderSettings.ambientProbe;
		Color color = CoreUtils.ConvertLinearToActiveColorSpace(new Color(ambientProbe[0, 0], ambientProbe[1, 0], ambientProbe[2, 0]) * RenderSettings.reflectionIntensity);
		Shader.SetGlobalVector(PerFrameBuffer._GlossyEnvironmentColor, color);
		Shader.SetGlobalVector(PerFrameBuffer._SubtractiveShadowColor, CoreUtils.ConvertSRGBToActiveColorSpace(RenderSettings.subtractiveShadowColor));
	}

	public static bool IsGameCamera(Camera camera)
	{
		if (camera == null)
		{
			throw new ArgumentNullException("camera");
		}
		if (camera.cameraType != CameraType.Game)
		{
			return camera.cameraType == CameraType.VR;
		}
		return true;
	}

	public static bool IsStereoEnabled(Camera camera)
	{
		if (camera == null)
		{
			throw new ArgumentNullException("camera");
		}
		bool flag = IsGameCamera(camera);
		bool flag2 = true;
		flag2 &= !camera.targetTexture || camera.targetTexture.dimension == XRSettings.deviceEyeTextureDimension;
		return XRGraphics.enabled && flag && camera.stereoTargetEye == StereoTargetEyeMask.Both && flag2;
	}

	private static bool IsMultiPassStereoEnabled(Camera camera)
	{
		if (camera == null)
		{
			throw new ArgumentNullException("camera");
		}
		if (IsStereoEnabled(camera) && !CanXRSDKUseSinglePass(camera))
		{
			return XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.MultiPass;
		}
		return false;
	}

	private static XRDisplaySubsystem GetXRDisplaySubsystem()
	{
		XRDisplaySubsystem result = null;
		SubsystemManager.GetInstances(displaySubsystemList);
		if (displaySubsystemList.Count > 0)
		{
			result = displaySubsystemList[0];
		}
		return result;
	}

	internal static bool IsRunningHololens(Camera camera)
	{
		return false;
	}

	private void SortCameras(Camera[] cameras)
	{
		if (cameras.Length > 1)
		{
			Array.Sort(cameras, cameraComparison);
		}
	}

	private static RenderTextureDescriptor CreateRenderTextureDescriptor(Camera camera, float renderScale, bool isStereoEnabled, bool isHdrEnabled, int msaaSamples, bool needsAlpha)
	{
		GraphicsFormat graphicsFormat = SystemInfo.GetGraphicsFormat(DefaultFormat.LDR);
		RenderTextureDescriptor result;
		if (isStereoEnabled)
		{
			result = XRGraphics.eyeTextureDesc;
			graphicsFormat = result.graphicsFormat;
		}
		else if (camera.targetTexture == null)
		{
			result = new RenderTextureDescriptor(camera.pixelWidth, camera.pixelHeight);
			result.width = Mathf.Max(1, Mathf.RoundToInt((float)result.width * renderScale) >> 2 << 2);
			result.height = Mathf.Max(1, Mathf.RoundToInt((float)result.height * renderScale) >> 2 << 2);
			GraphicsFormat graphicsFormat2 = ((!needsAlpha && RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend)) ? GraphicsFormat.B10G11R11_UFloatPack32 : ((!needsAlpha || !RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R16G16B16A16_SFloat, FormatUsage.Blend)) ? SystemInfo.GetGraphicsFormat(DefaultFormat.LDR) : GraphicsFormat.R16G16B16A16_SFloat));
			result.graphicsFormat = (isHdrEnabled ? graphicsFormat2 : graphicsFormat);
			result.depthBufferBits = 32;
			result.msaaSamples = msaaSamples;
			result.sRGB = QualitySettings.activeColorSpace == ColorSpace.Linear;
		}
		else
		{
			result = camera.targetTexture.descriptor;
		}
		result.enableRandomWrite = false;
		result.bindMS = false;
		result.useDynamicScale = camera.allowDynamicResolution;
		return result;
	}
}
