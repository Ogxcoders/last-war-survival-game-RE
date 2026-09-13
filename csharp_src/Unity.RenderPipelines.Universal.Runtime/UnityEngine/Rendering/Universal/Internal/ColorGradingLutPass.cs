using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal.Internal;

public class ColorGradingLutPass : ScriptableRenderPass
{
	private static class ShaderConstants
	{
		public static readonly int _Lut_Params = Shader.PropertyToID("_Lut_Params");

		public static readonly int _ColorBalance = Shader.PropertyToID("_ColorBalance");

		public static readonly int _ColorFilter = Shader.PropertyToID("_ColorFilter");

		public static readonly int _ChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");

		public static readonly int _ChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");

		public static readonly int _ChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");

		public static readonly int _HueSatCon = Shader.PropertyToID("_HueSatCon");

		public static readonly int _Lift = Shader.PropertyToID("_Lift");

		public static readonly int _Gamma = Shader.PropertyToID("_Gamma");

		public static readonly int _Gain = Shader.PropertyToID("_Gain");

		public static readonly int _Shadows = Shader.PropertyToID("_Shadows");

		public static readonly int _Midtones = Shader.PropertyToID("_Midtones");

		public static readonly int _Highlights = Shader.PropertyToID("_Highlights");

		public static readonly int _ShaHiLimits = Shader.PropertyToID("_ShaHiLimits");

		public static readonly int _SplitShadows = Shader.PropertyToID("_SplitShadows");

		public static readonly int _SplitHighlights = Shader.PropertyToID("_SplitHighlights");

		public static readonly int _CurveMaster = Shader.PropertyToID("_CurveMaster");

		public static readonly int _CurveRed = Shader.PropertyToID("_CurveRed");

		public static readonly int _CurveGreen = Shader.PropertyToID("_CurveGreen");

		public static readonly int _CurveBlue = Shader.PropertyToID("_CurveBlue");

		public static readonly int _CurveHueVsHue = Shader.PropertyToID("_CurveHueVsHue");

		public static readonly int _CurveHueVsSat = Shader.PropertyToID("_CurveHueVsSat");

		public static readonly int _CurveLumVsSat = Shader.PropertyToID("_CurveLumVsSat");

		public static readonly int _CurveSatVsSat = Shader.PropertyToID("_CurveSatVsSat");
	}

	private const string k_ProfilerTag = "Color Grading LUT";

	private readonly Material m_LutBuilderLdr;

	private readonly Material m_LutBuilderHdr;

	private readonly GraphicsFormat m_HdrLutFormat;

	private readonly GraphicsFormat m_LdrLutFormat;

	private RenderTargetHandle m_InternalLut;

	public ColorGradingLutPass(RenderPassEvent evt, PostProcessData data)
	{
		base.renderPassEvent = evt;
		base.overrideCameraTarget = true;
		m_LutBuilderLdr = Load(data.shaders.lutBuilderLdrPS);
		m_LutBuilderHdr = Load(data.shaders.lutBuilderHdrPS);
		if (SystemInfo.IsFormatSupported(GraphicsFormat.R16G16B16A16_SFloat, FormatUsage.Blend))
		{
			m_HdrLutFormat = GraphicsFormat.R16G16B16A16_SFloat;
		}
		else if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend))
		{
			m_HdrLutFormat = GraphicsFormat.B10G11R11_UFloatPack32;
		}
		else
		{
			m_HdrLutFormat = GraphicsFormat.R8G8B8A8_UNorm;
		}
		m_LdrLutFormat = GraphicsFormat.R8G8B8A8_UNorm;
		Material Load(Shader shader)
		{
			if (shader == null)
			{
				Debug.LogError("Missing shader. " + GetType().DeclaringType.Name + " render pass will not execute. Check for missing reference in the renderer resources.");
				return null;
			}
			return CoreUtils.CreateEngineMaterial(shader);
		}
	}

	public void Setup(in RenderTargetHandle internalLut)
	{
		m_InternalLut = internalLut;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer commandBuffer = CommandBufferPool.Get("Color Grading LUT");
		VolumeStack stack = VolumeManager.instance.stack;
		ChannelMixer component = stack.GetComponent<ChannelMixer>();
		ColorAdjustments component2 = stack.GetComponent<ColorAdjustments>();
		ColorCurves component3 = stack.GetComponent<ColorCurves>();
		LiftGammaGain component4 = stack.GetComponent<LiftGammaGain>();
		ShadowsMidtonesHighlights component5 = stack.GetComponent<ShadowsMidtonesHighlights>();
		SplitToning component6 = stack.GetComponent<SplitToning>();
		Tonemapping component7 = stack.GetComponent<Tonemapping>();
		WhiteBalance component8 = stack.GetComponent<WhiteBalance>();
		ref PostProcessingData postProcessingData = ref renderingData.postProcessingData;
		bool flag = postProcessingData.gradingMode == ColorGradingMode.HighDynamicRange;
		int lutSize = postProcessingData.lutSize;
		int num = lutSize * lutSize;
		GraphicsFormat colorFormat = (flag ? m_HdrLutFormat : m_LdrLutFormat);
		Material material = (flag ? m_LutBuilderHdr : m_LutBuilderLdr);
		RenderTextureDescriptor desc = new RenderTextureDescriptor(num, lutSize, colorFormat, 0);
		desc.vrUsage = VRTextureUsage.None;
		commandBuffer.GetTemporaryRT(m_InternalLut.id, desc, FilterMode.Bilinear);
		Vector3 vector = ColorUtils.ColorBalanceToLMSCoeffs(component8.temperature.value, component8.tint.value);
		Vector4 value = new Vector4(component2.hueShift.value / 360f, component2.saturation.value / 100f + 1f, component2.contrast.value / 100f + 1f, 0f);
		Vector4 value2 = new Vector4(component.redOutRedIn.value / 100f, component.redOutGreenIn.value / 100f, component.redOutBlueIn.value / 100f, 0f);
		Vector4 value3 = new Vector4(component.greenOutRedIn.value / 100f, component.greenOutGreenIn.value / 100f, component.greenOutBlueIn.value / 100f, 0f);
		Vector4 value4 = new Vector4(component.blueOutRedIn.value / 100f, component.blueOutGreenIn.value / 100f, component.blueOutBlueIn.value / 100f, 0f);
		Vector4 value5 = new Vector4(component5.shadowsStart.value, component5.shadowsEnd.value, component5.highlightsStart.value, component5.highlightsEnd.value);
		var (value6, value7, value8) = ColorUtils.PrepareShadowsMidtonesHighlights(component5.shadows.value, component5.midtones.value, component5.highlights.value);
		var (value9, value10, value11) = ColorUtils.PrepareLiftGammaGain(component4.lift.value, component4.gamma.value, component4.gain.value);
		var (value12, value13) = ColorUtils.PrepareSplitToning((Vector4)component6.shadows.value, (Vector4)component6.highlights.value, component6.balance.value);
		material.SetVector(value: new Vector4(lutSize, 0.5f / (float)num, 0.5f / (float)lutSize, (float)lutSize / ((float)lutSize - 1f)), nameID: ShaderConstants._Lut_Params);
		material.SetVector(ShaderConstants._ColorBalance, vector);
		material.SetVector(ShaderConstants._ColorFilter, component2.colorFilter.value.linear);
		material.SetVector(ShaderConstants._ChannelMixerRed, value2);
		material.SetVector(ShaderConstants._ChannelMixerGreen, value3);
		material.SetVector(ShaderConstants._ChannelMixerBlue, value4);
		material.SetVector(ShaderConstants._HueSatCon, value);
		material.SetVector(ShaderConstants._Lift, value9);
		material.SetVector(ShaderConstants._Gamma, value10);
		material.SetVector(ShaderConstants._Gain, value11);
		material.SetVector(ShaderConstants._Shadows, value6);
		material.SetVector(ShaderConstants._Midtones, value7);
		material.SetVector(ShaderConstants._Highlights, value8);
		material.SetVector(ShaderConstants._ShaHiLimits, value5);
		material.SetVector(ShaderConstants._SplitShadows, value12);
		material.SetVector(ShaderConstants._SplitHighlights, value13);
		material.SetTexture(ShaderConstants._CurveMaster, component3.master.value.GetTexture());
		material.SetTexture(ShaderConstants._CurveRed, component3.red.value.GetTexture());
		material.SetTexture(ShaderConstants._CurveGreen, component3.green.value.GetTexture());
		material.SetTexture(ShaderConstants._CurveBlue, component3.blue.value.GetTexture());
		material.SetTexture(ShaderConstants._CurveHueVsHue, component3.hueVsHue.value.GetTexture());
		material.SetTexture(ShaderConstants._CurveHueVsSat, component3.hueVsSat.value.GetTexture());
		material.SetTexture(ShaderConstants._CurveLumVsSat, component3.lumVsSat.value.GetTexture());
		material.SetTexture(ShaderConstants._CurveSatVsSat, component3.satVsSat.value.GetTexture());
		if (flag)
		{
			material.shaderKeywords = null;
			switch (component7.mode.value)
			{
			case TonemappingMode.Neutral:
				material.EnableKeyword(ShaderKeywordStrings.TonemapNeutral);
				break;
			case TonemappingMode.ACES:
				material.EnableKeyword(ShaderKeywordStrings.TonemapACES);
				break;
			case TonemappingMode.UE4_TONEMAP:
				material.EnableKeyword(ShaderKeywordStrings.UE4_TONEMAP);
				break;
			}
		}
		Blit(commandBuffer, m_InternalLut.id, m_InternalLut.id, material);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	internal override void OnFinishCameraStackRendering(CommandBuffer cmd)
	{
		cmd.ReleaseTemporaryRT(m_InternalLut.id);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
	}

	internal void Cleanup()
	{
		CoreUtils.Destroy(m_LutBuilderLdr);
		CoreUtils.Destroy(m_LutBuilderHdr);
	}
}
