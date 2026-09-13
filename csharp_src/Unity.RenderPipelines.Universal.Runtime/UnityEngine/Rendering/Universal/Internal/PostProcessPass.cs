using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal.Internal;

public class PostProcessPass : ScriptableRenderPass
{
	private class MaterialLibrary
	{
		public readonly Material stopNaN;

		public readonly Material subpixelMorphologicalAntialiasing;

		public readonly Material tiltShift;

		public readonly Material gaussianDepthOfField;

		public readonly Material bokehDepthOfField;

		public readonly Material cameraMotionBlur;

		public readonly Material paniniProjection;

		public readonly Material bloom;

		public readonly Material uber;

		public readonly Material finalPass;

		public MaterialLibrary(PostProcessData data)
		{
			stopNaN = Load(data.shaders.stopNanPS);
			subpixelMorphologicalAntialiasing = Load(data.shaders.subpixelMorphologicalAntialiasingPS);
			tiltShift = Load(data.shaders.tiltShiftPS);
			gaussianDepthOfField = Load(data.shaders.gaussianDepthOfFieldPS);
			bokehDepthOfField = Load(data.shaders.bokehDepthOfFieldPS);
			cameraMotionBlur = Load(data.shaders.cameraMotionBlurPS);
			paniniProjection = Load(data.shaders.paniniProjectionPS);
			bloom = Load(data.shaders.bloomPS);
			uber = Load(data.shaders.uberPostPS);
			finalPass = Load(data.shaders.finalPostPassPS);
		}

		private Material Load(Shader shader)
		{
			if (shader == null)
			{
				Debug.LogErrorFormat("Missing shader. " + GetType().DeclaringType.Name + " render pass will not execute. Check for missing reference in the renderer resources.");
				return null;
			}
			if (!shader.isSupported)
			{
				return null;
			}
			return CoreUtils.CreateEngineMaterial(shader);
		}

		internal void Cleanup()
		{
			CoreUtils.Destroy(stopNaN);
			CoreUtils.Destroy(subpixelMorphologicalAntialiasing);
			CoreUtils.Destroy(gaussianDepthOfField);
			CoreUtils.Destroy(bokehDepthOfField);
			CoreUtils.Destroy(cameraMotionBlur);
			CoreUtils.Destroy(paniniProjection);
			CoreUtils.Destroy(bloom);
			CoreUtils.Destroy(uber);
			CoreUtils.Destroy(finalPass);
		}
	}

	private static class ShaderConstants
	{
		public static readonly int _TempTarget = Shader.PropertyToID("_TempTarget");

		public static readonly int _TempTarget2 = Shader.PropertyToID("_TempTarget2");

		public static readonly int _StencilRef = Shader.PropertyToID("_StencilRef");

		public static readonly int _StencilMask = Shader.PropertyToID("_StencilMask");

		public static readonly int _TiltShiftBlur0 = Shader.PropertyToID("_TiltShiftBlur0");

		public static readonly int _TiltShiftBlur1 = Shader.PropertyToID("_TiltShiftBlur1");

		public static readonly int _TiltShiftBlurSize = Shader.PropertyToID("_TiltShiftBlurSize");

		public static readonly int _TiltShiftStart = Shader.PropertyToID("_TiltShiftStart");

		public static readonly int _TiltShiftEnd = Shader.PropertyToID("_TiltShiftEnd");

		public static readonly int _TiltShiftSmooth = Shader.PropertyToID("_TiltShiftSmooth");

		public static readonly int _FullCoCTexture = Shader.PropertyToID("_FullCoCTexture");

		public static readonly int _HalfCoCTexture = Shader.PropertyToID("_HalfCoCTexture");

		public static readonly int _DofTexture = Shader.PropertyToID("_DofTexture");

		public static readonly int _CoCParams = Shader.PropertyToID("_CoCParams");

		public static readonly int _BokehKernel = Shader.PropertyToID("_BokehKernel");

		public static readonly int _PongTexture = Shader.PropertyToID("_PongTexture");

		public static readonly int _PingTexture = Shader.PropertyToID("_PingTexture");

		public static readonly int _Metrics = Shader.PropertyToID("_Metrics");

		public static readonly int _AreaTexture = Shader.PropertyToID("_AreaTexture");

		public static readonly int _SearchTexture = Shader.PropertyToID("_SearchTexture");

		public static readonly int _EdgeTexture = Shader.PropertyToID("_EdgeTexture");

		public static readonly int _BlendTexture = Shader.PropertyToID("_BlendTexture");

		public static readonly int _ColorTexture = Shader.PropertyToID("_ColorTexture");

		public static readonly int _Params = Shader.PropertyToID("_Params");

		public static readonly int _MainTexLowMip = Shader.PropertyToID("_MainTexLowMip");

		public static readonly int _Bloom_Params = Shader.PropertyToID("_Bloom_Params");

		public static readonly int _Bloom_RGBM = Shader.PropertyToID("_Bloom_RGBM");

		public static readonly int _Bloom_Texture = Shader.PropertyToID("_Bloom_Texture");

		public static readonly int _LensDirt_Texture = Shader.PropertyToID("_LensDirt_Texture");

		public static readonly int _LensDirt_Params = Shader.PropertyToID("_LensDirt_Params");

		public static readonly int _LensDirt_Intensity = Shader.PropertyToID("_LensDirt_Intensity");

		public static readonly int _Distortion_Params1 = Shader.PropertyToID("_Distortion_Params1");

		public static readonly int _Distortion_Params2 = Shader.PropertyToID("_Distortion_Params2");

		public static readonly int _Chroma_Params = Shader.PropertyToID("_Chroma_Params");

		public static readonly int _Vignette_Params1 = Shader.PropertyToID("_Vignette_Params1");

		public static readonly int _Vignette_Params2 = Shader.PropertyToID("_Vignette_Params2");

		public static readonly int _Lut_Params = Shader.PropertyToID("_Lut_Params");

		public static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");

		public static readonly int _InternalLut = Shader.PropertyToID("_InternalLut");

		public static readonly int _UserLut = Shader.PropertyToID("_UserLut");

		public static readonly int _Toe = Shader.PropertyToID("_Toe");

		public static readonly int _Shoulder = Shader.PropertyToID("_Shoulder");

		public static readonly int FilmSlope = Shader.PropertyToID("FilmSlope");

		public static readonly int FilmToe = Shader.PropertyToID("FilmToe");

		public static readonly int FilmShoulder = Shader.PropertyToID("FilmShoulder");

		public static readonly int FilmBlackClip = Shader.PropertyToID("FilmBlackClip");

		public static readonly int FilmWhiteClip = Shader.PropertyToID("FilmWhiteClip");

		public static readonly int _FullscreenProjMat = Shader.PropertyToID("_FullscreenProjMat");

		public static readonly int _GrayscaleWeight = Shader.PropertyToID("_GrayscaleWeight");

		public static int[] _BloomMipUp;

		public static int[] _BloomMipDown;
	}

	private RenderTextureDescriptor m_Descriptor;

	private RenderTargetHandle m_Source;

	private RenderTargetHandle m_Destination;

	private RenderTargetHandle m_Depth;

	private RenderTargetHandle m_InternalLut;

	private const string k_RenderPostProcessingTag = "Render PostProcessing Effects";

	private const string k_RenderFinalPostProcessingTag = "Render Final PostProcessing Pass";

	private MaterialLibrary m_Materials;

	private PostProcessData m_Data;

	private DepthOfField m_DepthOfField;

	private MotionBlur m_MotionBlur;

	private PaniniProjection m_PaniniProjection;

	private Bloom m_Bloom;

	private LensDistortion m_LensDistortion;

	private ChromaticAberration m_ChromaticAberration;

	private Vignette m_Vignette;

	private ColorLookup m_ColorLookup;

	private ColorAdjustments m_ColorAdjustments;

	private TiltShift m_TiltShift;

	private Tonemapping m_Tonemapping;

	private FilmGrain m_FilmGrain;

	private Grayscale m_Grayscale;

	private const int k_MaxPyramidSize = 16;

	private readonly GraphicsFormat m_DefaultHDRFormat;

	private bool m_UseRGBM;

	private readonly GraphicsFormat m_SMAAEdgeFormat;

	private readonly GraphicsFormat m_GaussianCoCFormat;

	private Matrix4x4 m_PrevViewProjM = Matrix4x4.identity;

	private bool m_ResetHistory;

	private int m_DitheringTextureIndex;

	private RenderTargetIdentifier[] m_MRT2;

	private Vector4[] m_BokehKernel;

	private int m_BokehHash;

	private bool m_IsStereo;

	private bool m_IsFinalPass;

	private bool m_HasFinalPass;

	private bool m_EnableSRGBConversionIfNeeded;

	private Material m_BlitMaterial;

	private static int blurOffset = Shader.PropertyToID("_Offset");

	public PostProcessPass(RenderPassEvent evt, PostProcessData data, Material blitMaterial = null)
	{
		base.renderPassEvent = evt;
		m_Data = data;
		m_Materials = new MaterialLibrary(data);
		m_BlitMaterial = blitMaterial;
		if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend))
		{
			m_DefaultHDRFormat = GraphicsFormat.B10G11R11_UFloatPack32;
			m_UseRGBM = false;
		}
		else
		{
			m_DefaultHDRFormat = ((QualitySettings.activeColorSpace == ColorSpace.Linear) ? GraphicsFormat.R8G8B8A8_SRGB : GraphicsFormat.R8G8B8A8_UNorm);
			m_UseRGBM = true;
		}
		if (SystemInfo.IsFormatSupported(GraphicsFormat.R8G8_UNorm, FormatUsage.Render) && SystemInfo.graphicsDeviceVendor.ToLowerInvariant().Contains("arm"))
		{
			m_SMAAEdgeFormat = GraphicsFormat.R8G8_UNorm;
		}
		else
		{
			m_SMAAEdgeFormat = GraphicsFormat.R8G8B8A8_UNorm;
		}
		if (SystemInfo.IsFormatSupported(GraphicsFormat.R16_UNorm, FormatUsage.Blend))
		{
			m_GaussianCoCFormat = GraphicsFormat.R16_UNorm;
		}
		else if (SystemInfo.IsFormatSupported(GraphicsFormat.R16_SFloat, FormatUsage.Blend))
		{
			m_GaussianCoCFormat = GraphicsFormat.R16_SFloat;
		}
		else
		{
			m_GaussianCoCFormat = GraphicsFormat.R8_UNorm;
		}
		ShaderConstants._BloomMipUp = new int[16];
		ShaderConstants._BloomMipDown = new int[16];
		for (int i = 0; i < 16; i++)
		{
			ShaderConstants._BloomMipUp[i] = Shader.PropertyToID("_BloomMipUp" + i);
			ShaderConstants._BloomMipDown[i] = Shader.PropertyToID("_BloomMipDown" + i);
		}
		m_MRT2 = new RenderTargetIdentifier[2];
		m_ResetHistory = true;
	}

	public void Cleanup()
	{
		m_Materials.Cleanup();
	}

	public void Setup(in RenderTextureDescriptor baseDescriptor, in RenderTargetHandle source, in RenderTargetHandle destination, in RenderTargetHandle depth, in RenderTargetHandle internalLut, bool hasFinalPass, bool enableSRGBConversion)
	{
		m_Descriptor = baseDescriptor;
		m_Descriptor.useMipMap = false;
		m_Descriptor.autoGenerateMips = false;
		m_Source = source;
		m_Destination = destination;
		m_Depth = depth;
		m_InternalLut = internalLut;
		m_IsFinalPass = false;
		m_HasFinalPass = hasFinalPass;
		m_EnableSRGBConversionIfNeeded = enableSRGBConversion;
	}

	public void SetupFinalPass(in RenderTextureDescriptor baseDescriptor, in RenderTargetHandle source)
	{
		m_Descriptor = baseDescriptor;
		m_Source = source;
		m_Destination = RenderTargetHandle.CameraTarget;
		m_IsFinalPass = true;
		m_HasFinalPass = false;
		m_EnableSRGBConversionIfNeeded = true;
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		if (!(m_Destination == RenderTargetHandle.CameraTarget))
		{
			RenderTextureDescriptor desc = cameraTextureDescriptor;
			desc.msaaSamples = 1;
			desc.depthBufferBits = 0;
			cmd.GetTemporaryRT(m_Destination.id, desc, FilterMode.Bilinear);
		}
	}

	public void ResetHistory()
	{
		m_ResetHistory = true;
	}

	public bool CanRunOnTile()
	{
		return false;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		VolumeStack stack = VolumeManager.instance.stack;
		m_DepthOfField = stack.GetComponent<DepthOfField>();
		m_MotionBlur = stack.GetComponent<MotionBlur>();
		m_PaniniProjection = stack.GetComponent<PaniniProjection>();
		m_Bloom = stack.GetComponent<Bloom>();
		m_LensDistortion = stack.GetComponent<LensDistortion>();
		m_ChromaticAberration = stack.GetComponent<ChromaticAberration>();
		m_Vignette = stack.GetComponent<Vignette>();
		m_ColorLookup = stack.GetComponent<ColorLookup>();
		m_ColorAdjustments = stack.GetComponent<ColorAdjustments>();
		m_TiltShift = stack.GetComponent<TiltShift>();
		m_Tonemapping = stack.GetComponent<Tonemapping>();
		m_FilmGrain = stack.GetComponent<FilmGrain>();
		m_Grayscale = stack.GetComponent<Grayscale>();
		if (m_IsFinalPass)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get("Render Final PostProcessing Pass");
			RenderFinalPass(commandBuffer, ref renderingData);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}
		else if (!CanRunOnTile())
		{
			CommandBuffer commandBuffer2 = CommandBufferPool.Get("Render PostProcessing Effects");
			Render(commandBuffer2, ref renderingData);
			context.ExecuteCommandBuffer(commandBuffer2);
			CommandBufferPool.Release(commandBuffer2);
		}
		m_ResetHistory = false;
	}

	private RenderTextureDescriptor GetStereoCompatibleDescriptor()
	{
		return GetStereoCompatibleDescriptor(m_Descriptor.width, m_Descriptor.height, m_Descriptor.graphicsFormat, m_Descriptor.depthBufferBits);
	}

	private RenderTextureDescriptor GetStereoCompatibleDescriptor(int width, int height, GraphicsFormat format, int depthBufferBits = 0)
	{
		RenderTextureDescriptor descriptor = m_Descriptor;
		descriptor.depthBufferBits = depthBufferBits;
		descriptor.msaaSamples = 1;
		descriptor.width = width;
		descriptor.height = height;
		descriptor.graphicsFormat = format;
		return descriptor;
	}

	private bool RequireSRGBConversionBlitToBackBuffer(CameraData cameraData)
	{
		bool result = Display.main.requiresSrgbBlitToBackbuffer;
		if (cameraData.isStereoEnabled)
		{
			result = !XRGraphics.eyeTextureDesc.sRGB;
		}
		return result;
	}

	private void Render(CommandBuffer cmd, ref RenderingData renderingData)
	{
		ref CameraData cameraData = ref renderingData.cameraData;
		m_IsStereo = renderingData.cameraData.isStereoEnabled;
		bool tempTargetUsed = false;
		bool tempTarget2Used = false;
		int source = m_Source.id;
		int destination = -1;
		bool isSceneViewCamera = cameraData.isSceneViewCamera;
		cmd.SetGlobalMatrix(ShaderConstants._FullscreenProjMat, GL.GetGPUProjectionMatrix(Matrix4x4.identity, renderIntoTexture: true));
		if (cameraData.isStopNaNEnabled && m_Materials.stopNaN != null)
		{
			using (new ProfilingScope(cmd, ProfilingSampler.Get(URPProfileId.StopNaNs)))
			{
				cmd.Blit(GetSource(), BlitDstDiscardContent(cmd, GetDestination()), m_Materials.stopNaN);
				Swap();
			}
		}
		if (cameraData.antialiasing == AntialiasingMode.SubpixelMorphologicalAntiAliasing && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2)
		{
			using (new ProfilingScope(cmd, ProfilingSampler.Get(URPProfileId.SMAA)))
			{
				DoSubpixelMorphologicalAntialiasing(ref cameraData, cmd, GetSource(), GetDestination());
				Swap();
			}
		}
		if (m_TiltShift.IsActive() && !m_TiltShift.deffered.value && !isSceneViewCamera)
		{
			using (new ProfilingScope(cmd, ProfilingSampler.Get(URPProfileId.TiltShift)))
			{
				DoTiltShift(cameraData.camera, cmd, GetSource(), GetSource(), cameraData.pixelRect);
			}
		}
		if (m_DepthOfField.IsActive() && !isSceneViewCamera)
		{
			URPProfileId marker = ((m_DepthOfField.mode.value == DepthOfFieldMode.Gaussian) ? URPProfileId.GaussianDepthOfField : URPProfileId.BokehDepthOfField);
			using (new ProfilingScope(cmd, ProfilingSampler.Get(marker)))
			{
				DoDepthOfField(cameraData.camera, cmd, GetSource(), GetDestination(), cameraData.pixelRect);
				Swap();
			}
		}
		if (m_MotionBlur.IsActive() && !isSceneViewCamera)
		{
			using (new ProfilingScope(cmd, ProfilingSampler.Get(URPProfileId.MotionBlur)))
			{
				DoMotionBlur(cameraData.camera, cmd, GetSource(), GetDestination());
				Swap();
			}
		}
		if (m_PaniniProjection.IsActive() && !isSceneViewCamera)
		{
			using (new ProfilingScope(cmd, ProfilingSampler.Get(URPProfileId.PaniniProjection)))
			{
				DoPaniniProjection(cameraData.camera, cmd, GetSource(), GetDestination());
				Swap();
			}
		}
		using (new ProfilingScope(cmd, ProfilingSampler.Get(URPProfileId.UberPostProcess)))
		{
			m_Materials.uber.shaderKeywords = null;
			bool flag = m_Bloom.IsActive();
			int nameID = ShaderConstants._BloomMipUp[0];
			if (flag)
			{
				using (new ProfilingScope(cmd, ProfilingSampler.Get(URPProfileId.Bloom)))
				{
					SetupBloom(cmd, GetSource(), m_Materials.uber);
				}
			}
			SetupLensDistortion(m_Materials.uber, isSceneViewCamera);
			SetupChromaticAberration(m_Materials.uber);
			SetupVignette(m_Materials.uber);
			SetupColorGrading(cmd, ref renderingData, m_Materials.uber);
			SetupGrain(in cameraData, m_Materials.uber);
			SetupDithering(in cameraData, m_Materials.uber);
			SetupGrayscale(m_Materials.uber);
			if (cameraData.antialiasing == AntialiasingMode.FastApproximateAntialiasing && cameraData.doFxaaInUberPost)
			{
				m_Materials.uber.EnableKeyword(ShaderKeywordStrings.Fxaa);
			}
			if (RequireSRGBConversionBlitToBackBuffer(cameraData) && m_EnableSRGBConversionIfNeeded)
			{
				m_Materials.uber.EnableKeyword(ShaderKeywordStrings.LinearToSRGBConversion);
			}
			cmd.SetGlobalTexture("_BlitTex", GetSource());
			RenderBufferLoadAction colorLoadAction = RenderBufferLoadAction.DontCare;
			if (m_Destination == RenderTargetHandle.CameraTarget && !cameraData.isDefaultViewport)
			{
				colorLoadAction = RenderBufferLoadAction.Load;
			}
			RenderTargetIdentifier renderTargetIdentifier = ((cameraData.targetTexture != null) ? new RenderTargetIdentifier(cameraData.targetTexture) : ((RenderTargetIdentifier)BuiltinRenderTextureType.CameraTarget));
			renderTargetIdentifier = ((m_Destination == RenderTargetHandle.CameraTarget) ? renderTargetIdentifier : m_Destination.Identifier());
			cmd.SetRenderTarget(renderTargetIdentifier, colorLoadAction, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
			bool flag2 = cameraData.resolveFinalTarget || m_Destination == RenderTargetHandle.CameraTarget || m_HasFinalPass;
			if (m_IsStereo)
			{
				Blit(cmd, GetSource(), BuiltinRenderTextureType.CurrentActive, m_Materials.uber);
				if (!flag2)
				{
					cmd.SetGlobalTexture("_BlitTex", renderTargetIdentifier);
					Blit(cmd, BuiltinRenderTextureType.CurrentActive, m_Source.id, m_BlitMaterial);
				}
			}
			else
			{
				cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
				if (m_Destination == RenderTargetHandle.CameraTarget)
				{
					cmd.SetViewport(cameraData.pixelRect);
				}
				cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_Materials.uber);
				if (!flag2)
				{
					cmd.SetGlobalTexture("_BlitTex", renderTargetIdentifier);
					cmd.SetRenderTarget(m_Source.id, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
					cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_BlitMaterial);
				}
				cmd.SetViewProjectionMatrices(cameraData.camera.worldToCameraMatrix, cameraData.camera.projectionMatrix);
			}
			if (flag)
			{
				cmd.ReleaseTemporaryRT(nameID);
			}
			if (tempTargetUsed)
			{
				cmd.ReleaseTemporaryRT(ShaderConstants._TempTarget);
			}
			if (tempTarget2Used)
			{
				cmd.ReleaseTemporaryRT(ShaderConstants._TempTarget2);
			}
		}
		int GetDestination()
		{
			if (destination == -1)
			{
				cmd.GetTemporaryRT(ShaderConstants._TempTarget, GetStereoCompatibleDescriptor(), FilterMode.Bilinear);
				destination = ShaderConstants._TempTarget;
				tempTargetUsed = true;
			}
			else if (destination == m_Source.id && m_Descriptor.msaaSamples > 1)
			{
				cmd.GetTemporaryRT(ShaderConstants._TempTarget2, GetStereoCompatibleDescriptor(), FilterMode.Bilinear);
				destination = ShaderConstants._TempTarget2;
				tempTarget2Used = true;
			}
			return destination;
		}
		int GetSource()
		{
			return source;
		}
		void Swap()
		{
			CoreUtils.Swap(ref source, ref destination);
		}
	}

	private BuiltinRenderTextureType BlitDstDiscardContent(CommandBuffer cmd, RenderTargetIdentifier rt)
	{
		cmd.SetRenderTarget(new RenderTargetIdentifier(rt, 0, CubemapFace.Unknown, -1), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
		return BuiltinRenderTextureType.CurrentActive;
	}

	private void DoSubpixelMorphologicalAntialiasing(ref CameraData cameraData, CommandBuffer cmd, int source, int destination)
	{
		Camera camera = cameraData.camera;
		Rect pixelRect = cameraData.pixelRect;
		Material subpixelMorphologicalAntialiasing = m_Materials.subpixelMorphologicalAntialiasing;
		subpixelMorphologicalAntialiasing.SetVector(ShaderConstants._Metrics, new Vector4(1f / (float)m_Descriptor.width, 1f / (float)m_Descriptor.height, m_Descriptor.width, m_Descriptor.height));
		subpixelMorphologicalAntialiasing.SetTexture(ShaderConstants._AreaTexture, m_Data.textures.smaaAreaTex);
		subpixelMorphologicalAntialiasing.SetTexture(ShaderConstants._SearchTexture, m_Data.textures.smaaSearchTex);
		subpixelMorphologicalAntialiasing.SetInt(ShaderConstants._StencilRef, 64);
		subpixelMorphologicalAntialiasing.SetInt(ShaderConstants._StencilMask, 64);
		subpixelMorphologicalAntialiasing.shaderKeywords = null;
		switch (cameraData.antialiasingQuality)
		{
		case AntialiasingQuality.Low:
			subpixelMorphologicalAntialiasing.EnableKeyword(ShaderKeywordStrings.SmaaLow);
			break;
		case AntialiasingQuality.Medium:
			subpixelMorphologicalAntialiasing.EnableKeyword(ShaderKeywordStrings.SmaaMedium);
			break;
		case AntialiasingQuality.High:
			subpixelMorphologicalAntialiasing.EnableKeyword(ShaderKeywordStrings.SmaaHigh);
			break;
		}
		RenderTargetIdentifier depth;
		int depthBufferBits;
		if (m_Depth == RenderTargetHandle.CameraTarget || m_Descriptor.msaaSamples > 1)
		{
			depth = ShaderConstants._EdgeTexture;
			depthBufferBits = 24;
		}
		else
		{
			depth = m_Depth.Identifier();
			depthBufferBits = 0;
		}
		cmd.GetTemporaryRT(ShaderConstants._EdgeTexture, GetStereoCompatibleDescriptor(m_Descriptor.width, m_Descriptor.height, m_SMAAEdgeFormat, depthBufferBits), FilterMode.Point);
		cmd.GetTemporaryRT(ShaderConstants._BlendTexture, GetStereoCompatibleDescriptor(m_Descriptor.width, m_Descriptor.height, GraphicsFormat.R8G8B8A8_UNorm), FilterMode.Point);
		cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
		cmd.SetViewport(pixelRect);
		cmd.SetRenderTarget(new RenderTargetIdentifier(ShaderConstants._EdgeTexture, 0, CubemapFace.Unknown, -1), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, depth, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
		cmd.ClearRenderTarget(clearDepth: true, clearColor: true, Color.clear);
		cmd.SetGlobalTexture(ShaderConstants._ColorTexture, source);
		cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, subpixelMorphologicalAntialiasing, 0, 0);
		cmd.SetRenderTarget(new RenderTargetIdentifier(ShaderConstants._BlendTexture, 0, CubemapFace.Unknown, -1), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, depth, RenderBufferLoadAction.Load, RenderBufferStoreAction.DontCare);
		cmd.ClearRenderTarget(clearDepth: false, clearColor: true, Color.clear);
		cmd.SetGlobalTexture(ShaderConstants._ColorTexture, ShaderConstants._EdgeTexture);
		cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, subpixelMorphologicalAntialiasing, 0, 1);
		cmd.SetRenderTarget(new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
		cmd.SetGlobalTexture(ShaderConstants._ColorTexture, source);
		cmd.SetGlobalTexture(ShaderConstants._BlendTexture, ShaderConstants._BlendTexture);
		cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, subpixelMorphologicalAntialiasing, 0, 2);
		cmd.ReleaseTemporaryRT(ShaderConstants._EdgeTexture);
		cmd.ReleaseTemporaryRT(ShaderConstants._BlendTexture);
		cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
	}

	private void DoTiltShift(Camera camera, CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Rect pixelRect)
	{
		DoTiltShiftBlur(camera, cmd, source, destination, pixelRect);
		DoTiltShiftCombile(camera, cmd, source, destination, pixelRect);
	}

	private void DoTiltShiftBlur(Camera camera, CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Rect pixelRect)
	{
		cmd.SetGlobalFloat(ShaderConstants._TiltShiftStart, m_TiltShift.blurStart.value);
		float num = 0.5f;
		Vector2Int vector2Int = new Vector2Int((int)((float)m_Descriptor.width * num), (int)Math.Max(4f, (float)m_Descriptor.height * (1f - m_TiltShift.blurStart.value) * num));
		cmd.GetTemporaryRT(ShaderConstants._TiltShiftBlur0, GetStereoCompatibleDescriptor(vector2Int.x, vector2Int.y, m_DefaultHDRFormat), FilterMode.Bilinear);
		cmd.GetTemporaryRT(ShaderConstants._TiltShiftBlur1, GetStereoCompatibleDescriptor(vector2Int.x, vector2Int.y, m_DefaultHDRFormat), FilterMode.Bilinear);
		float num2 = m_TiltShift.radius.value * ((float)m_Descriptor.width / 1080f);
		cmd.SetGlobalVector(ShaderConstants._TiltShiftBlurSize, new Vector2(num2, num2 * 1f));
		cmd.SetGlobalTexture(ShaderConstants._ColorTexture, source);
		cmd.Blit(source, BlitDstDiscardContent(cmd, ShaderConstants._TiltShiftBlur0), m_Materials.tiltShift, 0);
		cmd.SetGlobalVector(ShaderConstants._TiltShiftBlurSize, new Vector2(num2, num2 * num));
		for (int i = 0; i < m_TiltShift.iteration.value; i++)
		{
			cmd.SetGlobalTexture(ShaderConstants._ColorTexture, ShaderConstants._TiltShiftBlur0);
			cmd.Blit(ShaderConstants._TiltShiftBlur0, BlitDstDiscardContent(cmd, ShaderConstants._TiltShiftBlur1), m_Materials.tiltShift, 1);
			cmd.SetGlobalTexture(ShaderConstants._ColorTexture, ShaderConstants._TiltShiftBlur1);
			cmd.Blit(ShaderConstants._TiltShiftBlur1, BlitDstDiscardContent(cmd, ShaderConstants._TiltShiftBlur0), m_Materials.tiltShift, 2);
		}
	}

	private void DoTiltShiftCombile(Camera camera, CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Rect pixelRect, int overrideDrawMode = -1)
	{
		cmd.SetGlobalTexture(ShaderConstants._ColorTexture, ShaderConstants._TiltShiftBlur0);
		cmd.SetRenderTarget(destination, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
		switch ((overrideDrawMode == -1) ? m_TiltShift.debug.value : Mathf.Clamp(overrideDrawMode, 0, 2))
		{
		case 0:
		{
			Mesh customScreenMesh = RenderingUtils.GetCustomScreenMesh(new Vector2(1f, 1f - m_TiltShift.blurStart.value), new Vector2(0f, m_TiltShift.blurStart.value));
			cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
			cmd.DrawMesh(customScreenMesh, Matrix4x4.identity, m_Materials.tiltShift, 0, 4);
			cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
			break;
		}
		case 1:
			cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
			cmd.SetViewport(Rect.MinMaxRect(0f, (float)m_Descriptor.height * m_TiltShift.blurStart.value, m_Descriptor.width, m_Descriptor.height));
			cmd.EnableScissorRect(Rect.MinMaxRect(0f, (float)m_Descriptor.height * m_TiltShift.blurStart.value, m_Descriptor.width, m_Descriptor.height));
			cmd.DisableScissorRect();
			cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_Materials.tiltShift, 0, 4);
			cmd.SetViewport(pixelRect);
			cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
			break;
		case 2:
			cmd.EnableScissorRect(Rect.MinMaxRect(0f, (float)m_Descriptor.height * m_TiltShift.blurStart.value, m_Descriptor.width, m_Descriptor.height));
			cmd.Blit(ShaderConstants._TiltShiftBlur0, destination, m_Materials.tiltShift, 3);
			cmd.DisableScissorRect();
			break;
		}
		cmd.ReleaseTemporaryRT(ShaderConstants._TiltShiftBlur0);
		cmd.ReleaseTemporaryRT(ShaderConstants._TiltShiftBlur1);
	}

	private void DoDepthOfField(Camera camera, CommandBuffer cmd, int source, int destination, Rect pixelRect)
	{
		if (m_DepthOfField.mode.value == DepthOfFieldMode.Gaussian)
		{
			DoGaussianDepthOfField(camera, cmd, source, destination, pixelRect);
		}
		else if (m_DepthOfField.mode.value == DepthOfFieldMode.Bokeh)
		{
			DoBokehDepthOfField(cmd, source, destination, pixelRect);
		}
	}

	private void DoGaussianDepthOfField(Camera camera, CommandBuffer cmd, int source, int destination, Rect pixelRect)
	{
		Material gaussianDepthOfField = m_Materials.gaussianDepthOfField;
		int num = m_Descriptor.width / 2;
		int height = m_Descriptor.height / 2;
		float value = m_DepthOfField.gaussianStart.value;
		float y = Mathf.Max(value, m_DepthOfField.gaussianEnd.value);
		float a = m_DepthOfField.gaussianMaxRadius.value * ((float)num / 1080f);
		a = Mathf.Min(a, 2f);
		CoreUtils.SetKeyword(gaussianDepthOfField, ShaderKeywordStrings.HighQualitySampling, m_DepthOfField.highQualitySampling.value);
		gaussianDepthOfField.SetVector(ShaderConstants._CoCParams, new Vector3(value, y, a));
		cmd.GetTemporaryRT(ShaderConstants._FullCoCTexture, GetStereoCompatibleDescriptor(m_Descriptor.width, m_Descriptor.height, m_GaussianCoCFormat), FilterMode.Bilinear);
		cmd.GetTemporaryRT(ShaderConstants._HalfCoCTexture, GetStereoCompatibleDescriptor(num, height, m_GaussianCoCFormat), FilterMode.Bilinear);
		cmd.GetTemporaryRT(ShaderConstants._PingTexture, GetStereoCompatibleDescriptor(num, height, m_DefaultHDRFormat), FilterMode.Bilinear);
		cmd.GetTemporaryRT(ShaderConstants._PongTexture, GetStereoCompatibleDescriptor(num, height, m_DefaultHDRFormat), FilterMode.Bilinear);
		cmd.Blit(source, ShaderConstants._FullCoCTexture, gaussianDepthOfField, 0);
		m_MRT2[0] = ShaderConstants._HalfCoCTexture;
		m_MRT2[1] = ShaderConstants._PingTexture;
		cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
		cmd.SetViewport(pixelRect);
		cmd.SetGlobalTexture(ShaderConstants._ColorTexture, source);
		cmd.SetGlobalTexture(ShaderConstants._FullCoCTexture, ShaderConstants._FullCoCTexture);
		cmd.SetRenderTarget(m_MRT2, ShaderConstants._HalfCoCTexture, 0, CubemapFace.Unknown, -1);
		cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, gaussianDepthOfField, 0, 1);
		cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
		cmd.SetGlobalTexture(ShaderConstants._HalfCoCTexture, ShaderConstants._HalfCoCTexture);
		cmd.Blit(ShaderConstants._PingTexture, ShaderConstants._PongTexture, gaussianDepthOfField, 2);
		cmd.Blit(ShaderConstants._PongTexture, BlitDstDiscardContent(cmd, ShaderConstants._PingTexture), gaussianDepthOfField, 3);
		cmd.SetGlobalTexture(ShaderConstants._ColorTexture, ShaderConstants._PingTexture);
		cmd.SetGlobalTexture(ShaderConstants._FullCoCTexture, ShaderConstants._FullCoCTexture);
		cmd.Blit(source, BlitDstDiscardContent(cmd, destination), gaussianDepthOfField, 4);
		cmd.ReleaseTemporaryRT(ShaderConstants._FullCoCTexture);
		cmd.ReleaseTemporaryRT(ShaderConstants._HalfCoCTexture);
		cmd.ReleaseTemporaryRT(ShaderConstants._PingTexture);
		cmd.ReleaseTemporaryRT(ShaderConstants._PongTexture);
	}

	private void PrepareBokehKernel()
	{
		if (m_BokehKernel == null)
		{
			m_BokehKernel = new Vector4[42];
		}
		int num = 0;
		float num2 = m_DepthOfField.bladeCount.value;
		float p = 1f - m_DepthOfField.bladeCurvature.value;
		float num3 = m_DepthOfField.bladeRotation.value * (MathF.PI / 180f);
		for (int i = 1; i < 4; i++)
		{
			float num4 = 1f / 7f;
			float num5 = ((float)i + num4) / (3f + num4);
			int num6 = i * 7;
			for (int j = 0; j < num6; j++)
			{
				float num7 = MathF.PI * 2f * (float)j / (float)num6;
				float num8 = Mathf.Cos(MathF.PI / num2);
				float num9 = Mathf.Cos(num7 - MathF.PI * 2f / num2 * Mathf.Floor((num2 * num7 + MathF.PI) / (MathF.PI * 2f)));
				float num10 = num5 * Mathf.Pow(num8 / num9, p);
				float x = num10 * Mathf.Cos(num7 - num3);
				float y = num10 * Mathf.Sin(num7 - num3);
				m_BokehKernel[num] = new Vector4(x, y);
				num++;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float GetMaxBokehRadiusInPixels(float viewportHeight)
	{
		return Mathf.Min(0.05f, 14f / viewportHeight);
	}

	private void DoBokehDepthOfField(CommandBuffer cmd, int source, int destination, Rect pixelRect)
	{
		Material bokehDepthOfField = m_Materials.bokehDepthOfField;
		int num = m_Descriptor.width / 2;
		int num2 = m_Descriptor.height / 2;
		float num3 = m_DepthOfField.focalLength.value / 1000f;
		float num4 = m_DepthOfField.focalLength.value / m_DepthOfField.aperture.value;
		float value = m_DepthOfField.focusDistance.value;
		float y = num4 * num3 / (value - num3);
		float maxBokehRadiusInPixels = GetMaxBokehRadiusInPixels(m_Descriptor.height);
		float w = 1f / ((float)num / (float)num2);
		cmd.SetGlobalVector(ShaderConstants._CoCParams, new Vector4(value, y, maxBokehRadiusInPixels, w));
		int hashCode = m_DepthOfField.GetHashCode();
		if (hashCode != m_BokehHash)
		{
			m_BokehHash = hashCode;
			PrepareBokehKernel();
		}
		cmd.SetGlobalVectorArray(ShaderConstants._BokehKernel, m_BokehKernel);
		cmd.GetTemporaryRT(ShaderConstants._FullCoCTexture, GetStereoCompatibleDescriptor(m_Descriptor.width, m_Descriptor.height, GraphicsFormat.R8_UNorm), FilterMode.Bilinear);
		cmd.GetTemporaryRT(ShaderConstants._PingTexture, GetStereoCompatibleDescriptor(num, num2, GraphicsFormat.R16G16B16A16_SFloat), FilterMode.Bilinear);
		cmd.GetTemporaryRT(ShaderConstants._PongTexture, GetStereoCompatibleDescriptor(num, num2, GraphicsFormat.R16G16B16A16_SFloat), FilterMode.Bilinear);
		cmd.Blit(source, ShaderConstants._FullCoCTexture, bokehDepthOfField, 0);
		cmd.SetGlobalTexture(ShaderConstants._FullCoCTexture, ShaderConstants._FullCoCTexture);
		cmd.Blit(source, ShaderConstants._PingTexture, bokehDepthOfField, 1);
		cmd.Blit(ShaderConstants._PingTexture, ShaderConstants._PongTexture, bokehDepthOfField, 2);
		cmd.Blit(ShaderConstants._PongTexture, BlitDstDiscardContent(cmd, ShaderConstants._PingTexture), bokehDepthOfField, 3);
		cmd.SetGlobalTexture(ShaderConstants._DofTexture, ShaderConstants._PingTexture);
		cmd.Blit(source, BlitDstDiscardContent(cmd, destination), bokehDepthOfField, 4);
		cmd.ReleaseTemporaryRT(ShaderConstants._FullCoCTexture);
		cmd.ReleaseTemporaryRT(ShaderConstants._PingTexture);
		cmd.ReleaseTemporaryRT(ShaderConstants._PongTexture);
	}

	private void DoMotionBlur(Camera camera, CommandBuffer cmd, int source, int destination)
	{
		Material cameraMotionBlur = m_Materials.cameraMotionBlur;
		Matrix4x4 nonJitteredProjectionMatrix = camera.nonJitteredProjectionMatrix;
		Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
		Matrix4x4 matrix4x = nonJitteredProjectionMatrix * worldToCameraMatrix;
		cameraMotionBlur.SetMatrix("_ViewProjM", matrix4x);
		if (m_ResetHistory)
		{
			cameraMotionBlur.SetMatrix("_PrevViewProjM", matrix4x);
		}
		else
		{
			cameraMotionBlur.SetMatrix("_PrevViewProjM", m_PrevViewProjM);
		}
		cameraMotionBlur.SetFloat("_Intensity", m_MotionBlur.intensity.value);
		cameraMotionBlur.SetFloat("_Clamp", m_MotionBlur.clamp.value);
		cmd.Blit(source, BlitDstDiscardContent(cmd, destination), cameraMotionBlur, (int)m_MotionBlur.quality.value);
		m_PrevViewProjM = matrix4x;
	}

	private void DoPaniniProjection(Camera camera, CommandBuffer cmd, int source, int destination)
	{
		float value = m_PaniniProjection.distance.value;
		Vector2 vector = CalcViewExtents(camera);
		Vector2 vector2 = CalcCropExtents(camera, value);
		float a = vector2.x / vector.x;
		float b = vector2.y / vector.y;
		float value2 = Mathf.Min(a, b);
		float num = value;
		float w = Mathf.Lerp(1f, Mathf.Clamp01(value2), m_PaniniProjection.cropToFit.value);
		Material paniniProjection = m_Materials.paniniProjection;
		paniniProjection.SetVector(ShaderConstants._Params, new Vector4(vector.x, vector.y, num, w));
		paniniProjection.EnableKeyword((1f - Mathf.Abs(num) > float.Epsilon) ? ShaderKeywordStrings.PaniniGeneric : ShaderKeywordStrings.PaniniUnitDistance);
		cmd.Blit(source, BlitDstDiscardContent(cmd, destination), paniniProjection);
	}

	private Vector2 CalcViewExtents(Camera camera)
	{
		float num = camera.fieldOfView * (MathF.PI / 180f);
		float num2 = (float)m_Descriptor.width / (float)m_Descriptor.height;
		float num3 = Mathf.Tan(0.5f * num);
		return new Vector2(num2 * num3, num3);
	}

	private Vector2 CalcCropExtents(Camera camera, float d)
	{
		float num = 1f + d;
		Vector2 vector = CalcViewExtents(camera);
		float num2 = Mathf.Sqrt(vector.x * vector.x + 1f);
		float num3 = 1f / num2;
		float num4 = num3 + d;
		return vector * num3 * (num / num4);
	}

	private void GaussianCalculation(CommandBuffer cmd, Material bloomMaterial, int mipCount, int tw, int th, RenderTextureDescriptor desc)
	{
		int num = ShaderConstants._BloomMipDown[0];
		for (int i = 1; i < mipCount; i++)
		{
			tw = Mathf.Max(1, tw >> 1);
			th = Mathf.Max(1, th >> 1);
			int num2 = ShaderConstants._BloomMipDown[i];
			int num3 = ShaderConstants._BloomMipUp[i];
			desc.width = tw;
			desc.height = th;
			cmd.GetTemporaryRT(num2, desc, FilterMode.Bilinear);
			cmd.GetTemporaryRT(num3, desc, FilterMode.Bilinear);
			cmd.Blit(num, num3, bloomMaterial, 1);
			cmd.Blit(num3, num2, bloomMaterial, 2);
			num = num2;
		}
		for (int num4 = mipCount - 2; num4 >= 0; num4--)
		{
			int num5 = ((num4 == mipCount - 2) ? ShaderConstants._BloomMipDown[num4 + 1] : ShaderConstants._BloomMipUp[num4 + 1]);
			int num6 = ShaderConstants._BloomMipDown[num4];
			int num7 = ShaderConstants._BloomMipUp[num4];
			cmd.SetGlobalTexture(ShaderConstants._MainTexLowMip, num5);
			cmd.Blit(num6, BlitDstDiscardContent(cmd, num7), bloomMaterial, 3);
		}
		for (int j = 0; j < mipCount; j++)
		{
			cmd.ReleaseTemporaryRT(ShaderConstants._BloomMipDown[j]);
			if (j > 0)
			{
				cmd.ReleaseTemporaryRT(ShaderConstants._BloomMipUp[j]);
			}
		}
	}

	private void FastBloomCalculation(CommandBuffer cmd, Material bloomMaterial)
	{
		float num = m_Descriptor.width;
		float num2 = m_Descriptor.height;
		int value = m_Bloom._downsample.value;
		int width = Mathf.RoundToInt(num / (float)value);
		int height = Mathf.RoundToInt(num2 / (float)value);
		int num3 = Shader.PropertyToID("_BloomTemp1");
		int num4 = Shader.PropertyToID("_BloomTemp2");
		cmd.GetTemporaryRT(num3, width, height, 0, FilterMode.Bilinear);
		cmd.GetTemporaryRT(num4, width, height, 0, FilterMode.Bilinear);
		cmd.Blit(ShaderConstants._BloomMipDown[0], num3);
		int value2 = m_Bloom._blurIterations.value;
		for (int i = 0; i < value2; i++)
		{
			float num5 = (float)i * 1f;
			cmd.SetGlobalFloat(blurOffset, num5 / (float)value + m_Bloom._blursize.value);
			cmd.Blit(num3, num4, bloomMaterial, 4);
			cmd.Blit(num4, num3, bloomMaterial, 4);
		}
		cmd.Blit(num3, ShaderConstants._BloomMipUp[0]);
		cmd.ReleaseTemporaryRT(num3);
		cmd.ReleaseTemporaryRT(num4);
		cmd.ReleaseTemporaryRT(ShaderConstants._BloomMipDown[0]);
	}

	private void SetupBloom(CommandBuffer cmd, int source, Material uberMaterial)
	{
		int num = m_Descriptor.width >> 1;
		int num2 = m_Descriptor.height >> 1;
		Mathf.FloorToInt(Mathf.Log(Mathf.Max(num, num2), 2f) - 1f);
		float value = m_Bloom.clamp.value;
		float num3 = Mathf.GammaToLinearSpace(m_Bloom.threshold.value);
		float w = num3 * 0.5f;
		float x = Mathf.Lerp(0.05f, 0.95f, m_Bloom.scatter.value);
		Material bloom = m_Materials.bloom;
		bloom.SetVector(ShaderConstants._Params, new Vector4(x, value, num3, w));
		CoreUtils.SetKeyword(bloom, ShaderKeywordStrings.BloomHQ, m_Bloom.highQualityFiltering.value);
		CoreUtils.SetKeyword(bloom, ShaderKeywordStrings.UseRGBM, m_UseRGBM);
		RenderTextureDescriptor stereoCompatibleDescriptor = GetStereoCompatibleDescriptor(num, num2, m_DefaultHDRFormat);
		cmd.GetTemporaryRT(ShaderConstants._BloomMipDown[0], stereoCompatibleDescriptor, FilterMode.Bilinear);
		cmd.GetTemporaryRT(ShaderConstants._BloomMipUp[0], stereoCompatibleDescriptor, FilterMode.Bilinear);
		cmd.Blit(source, ShaderConstants._BloomMipDown[0], bloom, 0);
		FastBloomCalculation(cmd, bloom);
		Color color = m_Bloom.tint.value.linear;
		float num4 = ColorUtils.Luminance(in color);
		color = ((num4 > 0f) ? (color * (1f / num4)) : Color.white);
		uberMaterial.SetVector(value: new Vector4(m_Bloom.intensity.value, color.r, color.g, color.b), nameID: ShaderConstants._Bloom_Params);
		uberMaterial.SetFloat(ShaderConstants._Bloom_RGBM, m_UseRGBM ? 1f : 0f);
		cmd.SetGlobalTexture(ShaderConstants._Bloom_Texture, ShaderConstants._BloomMipUp[0]);
		Texture texture = ((m_Bloom.dirtTexture.value == null) ? Texture2D.blackTexture : m_Bloom.dirtTexture.value);
		float num5 = (float)texture.width / (float)texture.height;
		float num6 = (float)m_Descriptor.width / (float)m_Descriptor.height;
		Vector4 value2 = new Vector4(1f, 1f, 0f, 0f);
		float value3 = m_Bloom.dirtIntensity.value;
		if (num5 > num6)
		{
			value2.x = num6 / num5;
			value2.z = (1f - value2.x) * 0.5f;
		}
		else if (num6 > num5)
		{
			value2.y = num5 / num6;
			value2.w = (1f - value2.y) * 0.5f;
		}
		uberMaterial.SetVector(ShaderConstants._LensDirt_Params, value2);
		uberMaterial.SetFloat(ShaderConstants._LensDirt_Intensity, value3);
		uberMaterial.SetTexture(ShaderConstants._LensDirt_Texture, texture);
		if (m_Bloom.highQualityFiltering.value)
		{
			uberMaterial.EnableKeyword((value3 > 0f) ? ShaderKeywordStrings.BloomHQDirt : ShaderKeywordStrings.BloomHQ);
		}
		else
		{
			uberMaterial.EnableKeyword((value3 > 0f) ? ShaderKeywordStrings.BloomLQDirt : ShaderKeywordStrings.BloomLQ);
		}
	}

	private void SetupLensDistortion(Material material, bool isSceneView)
	{
		float b = 1.6f * Mathf.Max(Mathf.Abs(m_LensDistortion.intensity.value * 100f), 1f);
		float num = MathF.PI / 180f * Mathf.Min(160f, b);
		float y = 2f * Mathf.Tan(num * 0.5f);
		Vector2 vector = m_LensDistortion.center.value * 2f - Vector2.one;
		Vector4 value = new Vector4(vector.x, vector.y, Mathf.Max(m_LensDistortion.xMultiplier.value, 0.0001f), Mathf.Max(m_LensDistortion.yMultiplier.value, 0.0001f));
		Vector4 value2 = new Vector4((m_LensDistortion.intensity.value >= 0f) ? num : (1f / num), y, 1f / m_LensDistortion.scale.value, m_LensDistortion.intensity.value * 100f);
		material.SetVector(ShaderConstants._Distortion_Params1, value);
		material.SetVector(ShaderConstants._Distortion_Params2, value2);
		if (m_LensDistortion.IsActive() && !isSceneView)
		{
			material.EnableKeyword(ShaderKeywordStrings.Distortion);
		}
	}

	private void SetupChromaticAberration(Material material)
	{
		material.SetFloat(ShaderConstants._Chroma_Params, m_ChromaticAberration.intensity.value * 0.05f);
		if (m_ChromaticAberration.IsActive())
		{
			material.EnableKeyword(ShaderKeywordStrings.ChromaticAberration);
		}
	}

	private void SetupVignette(Material material)
	{
		Color value = m_Vignette.color.value;
		Vector2 value2 = m_Vignette.center.value;
		float num = (float)m_Descriptor.width / (float)m_Descriptor.height;
		if (m_IsStereo && XRGraphics.stereoRenderingMode == XRGraphics.StereoRenderingMode.SinglePass)
		{
			num *= 0.5f;
		}
		Vector4 value3 = new Vector4(value.r, value.g, value.b, m_Vignette.rounded.value ? num : 1f);
		Vector4 value4 = new Vector4(value2.x, value2.y, m_Vignette.intensity.value * 3f, m_Vignette.smoothness.value * 5f);
		material.SetVector(ShaderConstants._Vignette_Params1, value3);
		material.SetVector(ShaderConstants._Vignette_Params2, value4);
	}

	private void SetupColorGrading(CommandBuffer cmd, ref RenderingData renderingData, Material material)
	{
		ref PostProcessingData postProcessingData = ref renderingData.postProcessingData;
		bool flag = postProcessingData.gradingMode == ColorGradingMode.HighDynamicRange;
		int lutSize = postProcessingData.lutSize;
		int num = lutSize * lutSize;
		float w = Mathf.Pow(2f, m_ColorAdjustments.postExposure.value);
		cmd.SetGlobalTexture(ShaderConstants._InternalLut, m_InternalLut.Identifier());
		material.SetVector(ShaderConstants._Lut_Params, new Vector4(1f / (float)num, 1f / (float)lutSize, (float)lutSize - 1f, w));
		material.SetTexture(ShaderConstants._UserLut, m_ColorLookup.texture.value);
		material.SetVector(ShaderConstants._UserLut_Params, (!m_ColorLookup.IsActive()) ? Vector4.zero : new Vector4(1f / (float)m_ColorLookup.texture.value.width, 1f / (float)m_ColorLookup.texture.value.height, (float)m_ColorLookup.texture.value.height - 1f, m_ColorLookup.contribution.value));
		if (flag)
		{
			material.EnableKeyword(ShaderKeywordStrings.HDRGrading);
			return;
		}
		switch (m_Tonemapping.mode.value)
		{
		case TonemappingMode.Neutral:
		{
			material.EnableKeyword(ShaderKeywordStrings.TonemapNeutral);
			float num2 = m_Tonemapping.neutralBlackIn.value * 20f + 1f;
			float num3 = m_Tonemapping.neutralBlackOut.value * 10f + 1f;
			float num4 = m_Tonemapping.neutralWhiteIn.value / 20f;
			float num5 = 1f - m_Tonemapping.neutralWhiteOutx.value / 20f;
			float t = num2 / num3;
			float t2 = num4 / num5;
			float y = Mathf.Max(0f, Mathf.LerpUnclamped(0.57f, 0.37f, t));
			float z = Mathf.LerpUnclamped(0.01f, 0.24f, t2);
			float w2 = Mathf.Max(0f, Mathf.LerpUnclamped(0.02f, 0.2f, t));
			Shader.SetGlobalVector(ShaderConstants._Toe, new Vector4(0.2f, y, z, w2));
			Shader.SetGlobalVector(ShaderConstants._Shoulder, new Vector4(0.02f, 0.3f, m_Tonemapping.neutralWhiteLevel.value, m_Tonemapping.neutralWhiteClip.value / 10f));
			break;
		}
		case TonemappingMode.ACES:
			material.EnableKeyword(ShaderKeywordStrings.TonemapACES);
			break;
		case TonemappingMode.UE4_TONEMAP:
			material.EnableKeyword(ShaderKeywordStrings.UE4_TONEMAP);
			break;
		}
	}

	private void SetupGrain(in CameraData cameraData, Material material)
	{
		if (!m_HasFinalPass && m_FilmGrain.IsActive())
		{
			material.EnableKeyword(ShaderKeywordStrings.FilmGrain);
			PostProcessUtils.ConfigureFilmGrain(m_Data, m_FilmGrain, cameraData.pixelWidth, cameraData.pixelHeight, material);
		}
	}

	private void SetupDithering(in CameraData cameraData, Material material)
	{
		if (!m_HasFinalPass && cameraData.isDitheringEnabled)
		{
			material.EnableKeyword(ShaderKeywordStrings.Dithering);
			m_DitheringTextureIndex = PostProcessUtils.ConfigureDithering(m_Data, m_DitheringTextureIndex, cameraData.pixelWidth, cameraData.pixelHeight, material);
		}
	}

	private void SetupGrayscale(Material material)
	{
		material.SetFloat(ShaderConstants._GrayscaleWeight, m_Grayscale.weight.value);
		if (m_Grayscale.IsActive())
		{
			material.EnableKeyword(ShaderKeywordStrings.GrayScale);
		}
	}

	private void RenderFinalPass(CommandBuffer cmd, ref RenderingData renderingData)
	{
		ref CameraData cameraData = ref renderingData.cameraData;
		Material finalPass = m_Materials.finalPass;
		finalPass.shaderKeywords = null;
		if (cameraData.antialiasing == AntialiasingMode.FastApproximateAntialiasing && !cameraData.doFxaaInUberPost)
		{
			finalPass.EnableKeyword(ShaderKeywordStrings.Fxaa);
		}
		SetupGrain(in cameraData, finalPass);
		SetupDithering(in cameraData, finalPass);
		if (RequireSRGBConversionBlitToBackBuffer(cameraData) && m_EnableSRGBConversionIfNeeded)
		{
			finalPass.EnableKeyword(ShaderKeywordStrings.LinearToSRGBConversion);
		}
		cmd.SetGlobalTexture("_BlitTex", m_Source.Identifier());
		RenderBufferLoadAction colorLoadAction = (cameraData.isDefaultViewport ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
		RenderTargetIdentifier renderTargetIdentifier = ((cameraData.targetTexture != null) ? new RenderTargetIdentifier(cameraData.targetTexture) : ((RenderTargetIdentifier)BuiltinRenderTextureType.CameraTarget));
		if (m_TiltShift.IsActive() && m_TiltShift.deffered.value && !cameraData.isSceneViewCamera)
		{
			using (new ProfilingScope(cmd, ProfilingSampler.Get(URPProfileId.TiltShift)))
			{
				DoTiltShiftBlur(cameraData.camera, cmd, m_Source.Identifier(), renderTargetIdentifier, cameraData.pixelRect);
			}
		}
		cmd.SetRenderTarget(renderTargetIdentifier, colorLoadAction, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
		if (cameraData.isStereoEnabled)
		{
			Blit(cmd, m_Source.Identifier(), BuiltinRenderTextureType.CurrentActive, finalPass);
		}
		else
		{
			cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
			cmd.SetViewport(cameraData.pixelRect);
			cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, finalPass);
			cmd.SetViewProjectionMatrices(cameraData.camera.worldToCameraMatrix, cameraData.camera.projectionMatrix);
		}
		if (m_TiltShift.IsActive() && m_TiltShift.deffered.value && !cameraData.isSceneViewCamera)
		{
			DoTiltShiftCombile(cameraData.camera, cmd, m_Source.Identifier(), renderTargetIdentifier, cameraData.pixelRect, 0);
		}
	}

	public bool IsFinalPostProcessingRequired()
	{
		if (m_TiltShift != null && m_TiltShift.IsActive())
		{
			return m_TiltShift.deffered.value;
		}
		return false;
	}
}
