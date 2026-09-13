using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public static class RenderQualitySetting
{
	private static RenderQualityGM RenderQualityGM;

	public static EnQualityLevel CurrentLevel { get; private set; } = EnQualityLevel.Unknown;

	public static int DeviceLevel => QualitySettings.GetQualityLevel() + 1;

	public static bool IsLowDevice => DeviceLevel <= 1;

	internal static UniversalRenderPipelineAsset RenderPipelineAsset => UniversalRenderPipeline.asset;

	public static ScriptableRenderer ScriptableRenderer => RenderPipelineAsset.scriptableRenderer;

	public static bool RequireDepthTexture => DepthTextureStateRegister.Valid;

	public static bool RequireDepthNormalTexture => DepthNormalTextureStateRegister.Valid;

	public static bool RequireOpaqueTexture => OpaqueTextureStateRegister.Valid;

	public static bool RequirePostProcess => PostProcessStateRegister.Valid;

	public static RenderStateRegister DepthTextureStateRegister => RenderStateRegisterCenter.DepthTextureStateRegister;

	public static RenderStateRegister DepthNormalTextureStateRegister => RenderStateRegisterCenter.DepthNormalTextureStateRegister;

	public static RenderStateRegister OpaqueTextureStateRegister => RenderStateRegisterCenter.OpaqueTextureStateRegister;

	public static RenderStateRegister PostProcessStateRegister => RenderStateRegisterCenter.PostProcessStateRegister;

	public static void SwitchQualityLevel(EnQualityLevel level, bool applyExpensiveChanges = true)
	{
		QualitySettings.SetQualityLevel((int)level, applyExpensiveChanges);
		QualitySettingSwitcherCenter.Switch(level);
		CurrentLevel = level;
	}

	public static void ToggleRenderQualityGM()
	{
		ToggleRenderQualityGMInternal();
	}

	private static void ToggleRenderQualityGMInternal()
	{
		if (RenderQualityGM == null)
		{
			RenderQualityGM[] array = Object.FindObjectsOfType<RenderQualityGM>();
			if (array.Length != 0)
			{
				RenderQualityGM = array[0];
			}
			for (int i = 1; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
		}
		if (RenderQualityGM == null)
		{
			RenderQualityGM = new GameObject().AddComponent<RenderQualityGM>();
			RenderQualityGM.enabled = false;
		}
		if (RenderQualityGM != null)
		{
			RenderQualityGM.enabled = !RenderQualityGM.enabled;
		}
	}

	public static void UpdateMainCameraState()
	{
		if (Camera.main != null)
		{
			UniversalAdditionalCameraData component = Camera.main.GetComponent<UniversalAdditionalCameraData>();
			if (component != null)
			{
				component.requiresDepthOption = ((RenderPipelineAsset.supportsCameraDepthTexture && RequireDepthTexture) ? CameraOverrideOption.On : CameraOverrideOption.Off);
				component.requiresColorOption = ((RenderPipelineAsset.supportsCameraOpaqueTexture && RequireOpaqueTexture) ? CameraOverrideOption.On : CameraOverrideOption.Off);
				component.renderPostProcessing = RenderPipelineAsset.supportsPostProcess && RequirePostProcess;
			}
		}
	}
}
