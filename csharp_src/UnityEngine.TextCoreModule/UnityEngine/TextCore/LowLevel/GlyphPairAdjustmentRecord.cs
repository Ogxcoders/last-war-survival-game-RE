using System;
using System.Diagnostics;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel;

[Serializable]
[DebuggerDisplay("First glyphIndex = {m_FirstAdjustmentRecord.m_GlyphIndex},  Second glyphIndex = {m_SecondAdjustmentRecord.m_GlyphIndex}")]
[UsedByNativeCode]
public struct GlyphPairAdjustmentRecord
{
	[SerializeField]
	[NativeName("firstAdjustmentRecord")]
	private GlyphAdjustmentRecord m_FirstAdjustmentRecord;

	[SerializeField]
	[NativeName("secondAdjustmentRecord")]
	private GlyphAdjustmentRecord m_SecondAdjustmentRecord;

	[SerializeField]
	private FontFeatureLookupFlags m_FeatureLookupFlags;

	public GlyphAdjustmentRecord firstAdjustmentRecord
	{
		get
		{
			return m_FirstAdjustmentRecord;
		}
		set
		{
			m_FirstAdjustmentRecord = value;
		}
	}

	public GlyphAdjustmentRecord secondAdjustmentRecord
	{
		get
		{
			return m_SecondAdjustmentRecord;
		}
		set
		{
			m_SecondAdjustmentRecord = value;
		}
	}

	public FontFeatureLookupFlags featureLookupFlags
	{
		get
		{
			return m_FeatureLookupFlags;
		}
		set
		{
			m_FeatureLookupFlags = value;
		}
	}

	public GlyphPairAdjustmentRecord(GlyphAdjustmentRecord firstAdjustmentRecord, GlyphAdjustmentRecord secondAdjustmentRecord)
	{
		m_FirstAdjustmentRecord = firstAdjustmentRecord;
		m_SecondAdjustmentRecord = secondAdjustmentRecord;
		m_FeatureLookupFlags = FontFeatureLookupFlags.None;
	}
}
