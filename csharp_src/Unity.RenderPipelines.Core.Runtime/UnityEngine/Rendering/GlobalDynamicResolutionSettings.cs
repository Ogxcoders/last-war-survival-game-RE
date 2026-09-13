using System;

namespace UnityEngine.Rendering;

[Serializable]
public struct GlobalDynamicResolutionSettings
{
	public bool enabled;

	public float maxPercentage;

	public float minPercentage;

	public DynamicResolutionType dynResType;

	public DynamicResUpscaleFilter upsampleFilter;

	public bool forceResolution;

	public float forcedPercentage;

	public static GlobalDynamicResolutionSettings NewDefault()
	{
		return new GlobalDynamicResolutionSettings
		{
			maxPercentage = 100f,
			minPercentage = 100f,
			dynResType = DynamicResolutionType.Hardware,
			upsampleFilter = DynamicResUpscaleFilter.CatmullRom,
			forcedPercentage = 100f
		};
	}
}
