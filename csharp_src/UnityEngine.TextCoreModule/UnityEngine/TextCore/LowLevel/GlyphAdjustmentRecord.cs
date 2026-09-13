using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel;

[Serializable]
[UsedByNativeCode]
public struct GlyphAdjustmentRecord
{
	[SerializeField]
	[NativeName("glyphIndex")]
	private uint m_GlyphIndex;

	[SerializeField]
	[NativeName("glyphValueRecord")]
	private GlyphValueRecord m_GlyphValueRecord;

	public uint glyphIndex
	{
		get
		{
			return m_GlyphIndex;
		}
		set
		{
			m_GlyphIndex = value;
		}
	}

	public GlyphValueRecord glyphValueRecord
	{
		get
		{
			return m_GlyphValueRecord;
		}
		set
		{
			m_GlyphValueRecord = value;
		}
	}

	public GlyphAdjustmentRecord(uint glyphIndex, GlyphValueRecord glyphValueRecord)
	{
		m_GlyphIndex = glyphIndex;
		m_GlyphValueRecord = glyphValueRecord;
	}
}
