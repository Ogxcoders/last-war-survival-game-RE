using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public class RenderFeatureStateRegister<T> : RenderStateRegister where T : ScriptableRendererFeature
{
	public RenderFeatureStateRegister()
	{
		OnDisable();
	}

	public override void OnEnable()
	{
		RenderQualitySetting.ScriptableRenderer.ActiveRendererFeature<T>(enabled: true);
	}

	public override void OnDisable()
	{
		RenderQualitySetting.ScriptableRenderer.ActiveRendererFeature<T>(enabled: false);
	}
}
