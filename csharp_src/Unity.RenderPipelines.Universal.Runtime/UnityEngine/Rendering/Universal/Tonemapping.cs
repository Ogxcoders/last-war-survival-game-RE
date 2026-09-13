using System;

namespace UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Post-processing/Tonemapping")]
public sealed class Tonemapping : VolumeComponent, IPostProcessComponent
{
	[Tooltip("Select a tonemapping algorithm to use for the color grading process.")]
	public TonemappingModeParameter mode = new TonemappingModeParameter(TonemappingMode.None);

	public ClampedFloatParameter neutralBlackIn = new ClampedFloatParameter(0f, -0.1f, 0.1f);

	public ClampedFloatParameter neutralWhiteIn = new ClampedFloatParameter(0f, 1f, 20f);

	public ClampedFloatParameter neutralBlackOut = new ClampedFloatParameter(0f, -0.09f, 0.1f);

	public ClampedFloatParameter neutralWhiteOutx = new ClampedFloatParameter(0f, 1f, 20f);

	public ClampedFloatParameter neutralWhiteLevel = new ClampedFloatParameter(0f, 0.1f, 20f);

	public ClampedFloatParameter neutralWhiteClip = new ClampedFloatParameter(0f, 1f, 10f);

	public bool IsActive()
	{
		return mode.value != TonemappingMode.None;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
