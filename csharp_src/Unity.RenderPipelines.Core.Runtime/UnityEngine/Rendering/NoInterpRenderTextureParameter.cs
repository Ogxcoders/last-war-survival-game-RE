using System;
using System.Diagnostics;

namespace UnityEngine.Rendering;

[Serializable]
[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
public class NoInterpRenderTextureParameter : VolumeParameter<RenderTexture>
{
	public NoInterpRenderTextureParameter(RenderTexture value, bool overrideState = false)
		: base(value, overrideState)
	{
	}
}
