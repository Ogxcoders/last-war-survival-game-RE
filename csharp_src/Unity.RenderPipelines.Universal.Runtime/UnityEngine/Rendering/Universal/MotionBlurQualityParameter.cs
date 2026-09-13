using System;

namespace UnityEngine.Rendering.Universal;

[Serializable]
public sealed class MotionBlurQualityParameter : VolumeParameter<MotionBlurQuality>
{
	public MotionBlurQualityParameter(MotionBlurQuality value, bool overrideState = false)
		: base(value, overrideState)
	{
	}
}
