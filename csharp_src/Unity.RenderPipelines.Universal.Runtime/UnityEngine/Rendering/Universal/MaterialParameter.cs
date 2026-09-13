using System;

namespace UnityEngine.Rendering.Universal;

[Serializable]
public sealed class MaterialParameter : VolumeParameter<Material>
{
	public MaterialParameter(Material value, bool overrideState = false)
		: base(value, overrideState)
	{
	}
}
