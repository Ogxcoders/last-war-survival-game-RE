using System;

namespace UnityEngine.TextCore;

[Serializable]
internal class SpriteCharacter : TextElement
{
	[SerializeField]
	private string m_Name;

	[SerializeField]
	private int m_HashCode;

	public string name
	{
		get
		{
			return m_Name;
		}
		set
		{
			if (!(value == m_Name))
			{
				m_Name = value;
				m_HashCode = TextUtilities.GetHashCodeCaseInSensitive(m_Name);
			}
		}
	}

	public int hashCode => m_HashCode;

	public SpriteCharacter()
	{
		m_ElementType = TextElementType.Sprite;
	}

	public SpriteCharacter(uint unicode, SpriteGlyph glyph)
	{
		m_ElementType = TextElementType.Sprite;
		base.unicode = unicode;
		base.glyphIndex = glyph.index;
		base.glyph = glyph;
		base.scale = 1f;
	}
}
