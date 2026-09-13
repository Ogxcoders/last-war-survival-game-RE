using System;

namespace UnityEngine.TextCore;

[Serializable]
internal class Character : TextElement
{
	public Character()
	{
		m_ElementType = TextElementType.Character;
		base.scale = 1f;
	}

	public Character(uint unicode, Glyph glyph)
	{
		m_ElementType = TextElementType.Character;
		base.unicode = unicode;
		base.glyph = glyph;
		base.glyphIndex = glyph.index;
		base.scale = 1f;
	}
}
