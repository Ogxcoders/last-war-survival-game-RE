using System;
using System.Diagnostics;

namespace UnityEngine.Rendering;

[Serializable]
[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
public class CubemapParameter : VolumeParameter<Cubemap>
{
	public CubemapParameter(Cubemap value, bool overrideState = false)
		: base(value, overrideState)
	{
	}
}
