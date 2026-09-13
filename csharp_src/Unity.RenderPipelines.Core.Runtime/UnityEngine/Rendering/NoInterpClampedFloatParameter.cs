using System;
using System.Diagnostics;

namespace UnityEngine.Rendering;

[Serializable]
[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
public class NoInterpClampedFloatParameter : VolumeParameter<float>
{
	public float min;

	public float max;

	public override float value
	{
		get
		{
			return m_Value;
		}
		set
		{
			m_Value = Mathf.Clamp(value, min, max);
		}
	}

	public NoInterpClampedFloatParameter(float value, float min, float max, bool overrideState = false)
		: base(value, overrideState)
	{
		this.min = min;
		this.max = max;
	}
}
