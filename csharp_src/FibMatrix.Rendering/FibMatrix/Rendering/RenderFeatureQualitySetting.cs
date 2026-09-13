using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public class RenderFeatureQualitySetting<T> : QualitySettingBase where T : ScriptableRendererFeature
{
	public override void Switch(EnQualityLevel level)
	{
		base.Switch(level);
	}
}
