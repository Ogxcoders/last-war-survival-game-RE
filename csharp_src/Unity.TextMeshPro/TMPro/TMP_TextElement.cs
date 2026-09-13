using System;
using UnityEngine;
using UnityEngine.TextCore;

namespace TMPro;

[Serializable]
public class TMP_TextElement
{
	[SerializeField]
	protected TextElementType m_ElementType;

	[SerializeField]
	internal uint m_Unicode;

	internal TMP_Asset m_TextAsset;

	internal Glyph m_Glyph;

	[SerializeField]
	internal uint m_GlyphIndex;

	[SerializeField]
	internal float m_Scale;

	public TextElementType elementType => m_ElementType;

	public uint unicode
	{
		get
		{
			return m_Unicode;
		}
		set
		{
			m_Unicode = value;
		}
	}

	public TMP_Asset textAsset
	{
		get
		{
			return m_TextAsset;
		}
		set
		{
			m_TextAsset = value;
		}
	}

	public Glyph glyph
	{
		get
		{
			return m_Glyph;
		}
		set
		{
			m_Glyph = value;
		}
	}

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
}
