using System;

namespace UnityEngine.Rendering.Universal;

[Serializable]
public sealed class MotionBlurModeParameter : VolumeParameter<MotionBlurMode>
{
	public MotionBlurModeParameter(MotionBlurMode value, bool overrideState = false)
		: base(value, overrideState)
	{
	}
}
