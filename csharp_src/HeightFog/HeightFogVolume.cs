using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Dejavu/HeightFog")]
public class HeightFogVolume : VolumeComponent, IPostProcessComponent
{
	[Tooltip("\ufffdǷ\ufffd\ufffd\ufffdЧ\ufffd\ufffd")]
	public BoolParameter EnableEffect = new BoolParameter(value: false);

	[Tooltip("\ufffd\ufffd\ufffd\ufffdʼ\ufffd߶\ufffd")]
	public FloatParameter FogStartHeight = new FloatParameter(0f);

	[Tooltip("\ufffd\ufffd߶\ufffd")]
	public FloatParameter FogHeight = new FloatParameter(10f);

	[Range(0f, 1f)]
	[Tooltip("\ufffd\ufffdǿ\ufffd\ufffd")]
	public FloatParameter FogIntensity = new FloatParameter(0.5f);

	[Range(0f, 1f)]
	[Tooltip("\ufffd\ufffd\ufffd\ufffdɫ")]
	public ColorParameter FogColor = new ColorParameter(Color.white);

	public bool IsActive()
	{
		return EnableEffect == rhs: true;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
