using System;
using UnityEngine.TextCore;

namespace TMPro;

[Serializable]
public class TMP_Character : TMP_TextElement
{
	public TMP_Character()
	{
		m_ElementType = TextElementType.Character;
		base.scale = 1f;
	}

	public TMP_Character(uint unicode, Glyph glyph)
	{
		m_ElementType = TextElementType.Character;
		base.unicode = unicode;
		base.textAsset = null;
		base.glyph = glyph;
		base.glyphIndex = glyph.index;
		base.scale = 1f;
	}

	public TMP_Character(uint unicode, TMP_FontAsset fontAsset, Glyph glyph)
	{
		m_ElementType = TextElementType.Character;
		base.unicode = unicode;
		base.textAsset = fontAsset;
		base.glyph = glyph;
		base.glyphIndex = glyph.index;
		base.scale = 1f;
	}

	internal TMP_Character(uint unicode, uint glyphIndex)
	{
		m_ElementType = TextElementType.Character;
		base.unicode = unicode;
		base.textAsset = null;
		base.glyph = null;
		base.glyphIndex = glyphIndex;
		base.scale = 1f;
	}
}
