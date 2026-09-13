using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace FibMatrix.Rendering;

[Serializable]
public abstract class VolumeComponentSettingBase : QualitySettingBase
{
	public override bool showDescription => false;

	public WeakReference<List<Volume>> Volumes { get; set; }
}
