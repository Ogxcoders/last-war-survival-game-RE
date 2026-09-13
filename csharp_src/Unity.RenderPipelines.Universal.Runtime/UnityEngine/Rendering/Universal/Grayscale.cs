using System;

namespace UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("PostProcessingDebugger/Grayscale")]
public class Grayscale : VolumeComponent, IPostProcessComponent
{
	[Tooltip("grayscale weight")]
	public ClampedFloatParameter weight = new ClampedFloatParameter(0f, 0f, 1f);

	public bool IsActive()
	{
		return weight.value > 0f;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
