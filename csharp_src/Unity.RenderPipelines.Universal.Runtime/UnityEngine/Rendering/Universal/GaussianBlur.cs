using System;

namespace UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("CustomVolume/GaussianVolume")]
public class GaussianBlur : VolumeComponent, IPostProcessComponent
{
	[Serializable]
	public sealed class IntParameter : VolumeParameter<int>
	{
		public IntParameter(int value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}

	[Serializable]
	public sealed class MaterialParameter : VolumeParameter<Material>
	{
		public MaterialParameter(Material value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}

	[Range(1f, 8f)]
	public IntParameter downSample = new IntParameter(1, overrideState: true);

	[Range(1f, 32f)]
	public IntParameter blurCount = new IntParameter(1, overrideState: true);

	[Range(0f, 0.005f)]
	public IntParameter intensity = new IntParameter(1, overrideState: true);

	public MaterialParameter material = new MaterialParameter(null, overrideState: true);

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
