using System;
using System.Diagnostics;

namespace UnityEngine.Rendering;

[Serializable]
[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
public class TextureParameter : VolumeParameter<Texture>
{
	public TextureParameter(Texture value, bool overrideState = false)
		: base(value, overrideState)
	{
	}
}
