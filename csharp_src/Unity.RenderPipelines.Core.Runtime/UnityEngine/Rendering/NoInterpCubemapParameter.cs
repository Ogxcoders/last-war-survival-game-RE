using System;
using System.Diagnostics;

namespace UnityEngine.Rendering;

[Serializable]
[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
public class NoInterpCubemapParameter : VolumeParameter<Cubemap>
{
	public NoInterpCubemapParameter(Cubemap value, bool overrideState = false)
		: base(value, overrideState)
	{
	}
}
