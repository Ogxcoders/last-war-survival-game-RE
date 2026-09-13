using System;
using System.Diagnostics;

namespace UnityEngine.Rendering;

[Serializable]
[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
public class RenderTextureParameter : VolumeParameter<RenderTexture>
{
	public RenderTextureParameter(RenderTexture value, bool overrideState = false)
		: base(value, overrideState)
	{
	}
}
