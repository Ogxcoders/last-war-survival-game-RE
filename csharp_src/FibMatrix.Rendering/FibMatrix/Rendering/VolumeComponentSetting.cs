using System;
using UnityEngine.Rendering;

namespace FibMatrix.Rendering;

[Serializable]
public class VolumeComponentSetting<T> : VolumeComponentSettingBase where T : VolumeComponent
{
	public override void Switch(EnQualityLevel level)
	{
		if (base.Volumes != null && base.Volumes.TryGetTarget(out var target) && target != null)
		{
			foreach (Volume item in target)
			{
				if (item.sharedProfile != null && item.sharedProfile.TryGet<T>(out var component))
				{
					component.active = base[level];
				}
			}
		}
		base.Switch(level);
	}
}
