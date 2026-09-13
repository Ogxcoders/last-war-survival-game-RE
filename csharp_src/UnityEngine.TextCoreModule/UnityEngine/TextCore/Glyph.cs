using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.TextCore.LowLevel;

namespace UnityEngine.TextCore;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
[UsedByNativeCode]
public class Glyph
{
	[SerializeField]
	[NativeName("index")]
	private uint m_Index;

	[SerializeField]
	[NativeName("metrics")]
	private GlyphMetrics m_Metrics;

	[NativeName("glyphRect")]
	[SerializeField]
	private GlyphRect m_GlyphRect;

	[NativeName("scale")]
	[SerializeField]
	private float m_Scale;

	[SerializeField]
	[NativeName("atlasIndex")]
	private int m_AtlasIndex;

	public uint index
	{
		get
		{
			return m_Index;
		}
		set
		{
			m_Index = value;
		}
	}

	public GlyphMetrics metrics
	{
		get
		{
			return m_Metrics;
		}
		set
		{
			m_Metrics = value;
		}
	}

	public GlyphRect glyphRect
	{
		get
		{
			return m_GlyphRect;
		}
		set
		{
			m_GlyphRect = value;
		}
	}

	public float scale
	{
		get
		{
			return m_Scale;
		}
		set
		{
			m_Scale = value;
		}
	}

	public int atlasIndex
	{
		get
		{
			return m_AtlasIndex;
		}
		set
		{
			m_AtlasIndex = value;
		}
	}

	public Glyph()
	{
		m_Index = 0u;
		m_Metrics = default(GlyphMetrics);
		m_GlyphRect = default(GlyphRect);
		m_Scale = 1f;
		m_AtlasIndex = 0;
	}

	public Glyph(Glyph glyph)
	{
		m_Index = glyph.index;
		m_Metrics = glyph.metrics;
		m_GlyphRect = glyph.glyphRect;
		m_Scale = glyph.scale;
		m_AtlasIndex = glyph.atlasIndex;
	}

	internal Glyph(GlyphMarshallingStruct glyphStruct)
	{
		m_Index = glyphStruct.index;
		m_Metrics = glyphStruct.metrics;
		m_GlyphRect = glyphStruct.glyphRect;
		m_Scale = glyphStruct.scale;
		m_AtlasIndex = glyphStruct.atlasIndex;
	}

	public Glyph(uint index, GlyphMetrics metrics, GlyphRect glyphRect)
	{
		m_Index = index;
		m_Metrics = metrics;
		m_GlyphRect = glyphRect;
		m_Scale = 1f;
		m_AtlasIndex = 0;
	}

	public Glyph(uint index, GlyphMetrics metrics, GlyphRect glyphRect, float scale, int atlasIndex)
	{
		m_Index = index;
		m_Metrics = metrics;
		m_GlyphRect = glyphRect;
		m_Scale = scale;
		m_AtlasIndex = atlasIndex;
	}

	public bool Compare(Glyph other)
	{
		return index == other.index && metrics == other.metrics && glyphRect == other.glyphRect && scale == other.scale && atlasIndex == other.atlasIndex;
	}
}
