using System;

namespace UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("CustomVolume/CustomToneMapping")]
public sealed class CustToneMapping : VolumeComponent, IPostProcessComponent
{
	public MaterialParameter material = new MaterialParameter(null, overrideState: true);

	public ColorParameter color = new ColorParameter(Color.white);

	public bool IsActive()
	{
		if (material.value == null)
		{
			return false;
		}
		return true;
	}

	public bool IsTileCompatible()
	{
		return false;
	}

	public override void Override(VolumeComponent state, float interpFactor)
	{
		base.Override(state, interpFactor);
	}
}
