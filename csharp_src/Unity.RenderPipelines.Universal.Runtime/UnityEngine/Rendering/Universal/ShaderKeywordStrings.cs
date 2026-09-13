using System;

namespace UnityEngine.Rendering.Universal;

public static class ShaderKeywordStrings
{
	public static readonly string MainLightShadows = "_MAIN_LIGHT_SHADOWS";

	public static readonly string MainLightShadowCascades = "_MAIN_LIGHT_SHADOWS_CASCADE";

	public static readonly string AdditionalLightsVertex = "_ADDITIONAL_LIGHTS_VERTEX";

	public static readonly string AdditionalLightsPixel = "_ADDITIONAL_LIGHTS";

	public static readonly string AdditionalLightShadows = "_ADDITIONAL_LIGHT_SHADOWS";

	public static readonly string SoftShadows = "_SHADOWS_SOFT";

	public static readonly string MixedLightingSubtractive = "_MIXED_LIGHTING_SUBTRACTIVE";

	public static readonly string MainLightShadows3SD = "_MAIN_LIGHT_SHADOWS3SD";

	public static readonly string AllowSoftParticles = "_GLOBAL_ALLOW_SOFT_PARTICLES";

	public static readonly string DepthNoMsaa = "_DEPTH_NO_MSAA";

	public static readonly string DepthMsaa2 = "_DEPTH_MSAA_2";

	public static readonly string DepthMsaa4 = "_DEPTH_MSAA_4";

	public static readonly string DepthMsaa8 = "_DEPTH_MSAA_8";

	public static readonly string LinearToSRGBConversion = "_LINEAR_TO_SRGB_CONVERSION";

	[Obsolete("The _KILL_ALPHA shader keyword is deprecated in the Universal Render Pipeline.")]
	public static readonly string KillAlpha = "_KILL_ALPHA";

	public static readonly string SmaaLow = "_SMAA_PRESET_LOW";

	public static readonly string SmaaMedium = "_SMAA_PRESET_MEDIUM";

	public static readonly string SmaaHigh = "_SMAA_PRESET_HIGH";

	public static readonly string PaniniGeneric = "_GENERIC";

	public static readonly string PaniniUnitDistance = "_UNIT_DISTANCE";

	public static readonly string BloomLQ = "_BLOOM_LQ";

	public static readonly string BloomHQ = "_BLOOM_HQ";

	public static readonly string BloomLQDirt = "_BLOOM_LQ_DIRT";

	public static readonly string BloomHQDirt = "_BLOOM_HQ_DIRT";

	public static readonly string UseRGBM = "_USE_RGBM";

	public static readonly string Distortion = "_DISTORTION";

	public static readonly string FX_Distortion = "_FX_DISTORTION";

	public static readonly string ChromaticAberration = "_CHROMATIC_ABERRATION";

	public static readonly string HDRGrading = "_HDR_GRADING";

	public static readonly string TonemapACES = "_TONEMAP_ACES";

	public static readonly string TonemapNeutral = "_TONEMAP_NEUTRAL";

	public static readonly string UE4_TONEMAP = "_UE4_TONEMAP";

	public static readonly string FilmGrain = "_FILM_GRAIN";

	public static readonly string Fxaa = "_FXAA";

	public static readonly string Dithering = "_DITHERING";

	public static readonly string _Toe = "_Toe";

	public static readonly string _Shoulder = "_Shoulder";

	public static readonly string GrayScale = "_GRAYSCALE";

	public static readonly string HighQualitySampling = "_HIGH_QUALITY_SAMPLING";
}
