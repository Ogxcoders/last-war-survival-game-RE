namespace UnityEngine.UIElements.StyleSheets;

internal class StyleValuePropertyReader : IStylePropertyReader
{
	private StyleValue m_CurrentStyleValue;

	private StyleCursor m_CurrentCursor;

	public StylePropertyID propertyID { get; private set; }

	public int specificity { get; private set; }

	public int valueCount => 1;

	public void Set(StylePropertyID id, StyleValue value, int spec)
	{
		propertyID = id;
		m_CurrentStyleValue = value;
		specificity = spec;
	}

	public void Set(StyleCursor cursor, int spec)
	{
		propertyID = StylePropertyID.Cursor;
		m_CurrentCursor = cursor;
		specificity = spec;
	}

	public bool IsValueType(int index, StyleValueType type)
	{
		if (type == StyleValueType.Keyword)
		{
			return m_CurrentStyleValue.keyword != StyleKeyword.Undefined;
		}
		return m_CurrentStyleValue.keyword == StyleKeyword.Undefined;
	}

	public bool IsKeyword(int index, StyleValueKeyword keyword)
	{
		if (m_CurrentStyleValue.keyword == StyleKeyword.Undefined)
		{
			return false;
		}
		return m_CurrentStyleValue.keyword == keyword.ToStyleKeyword();
	}

	public string ReadAsString(int index)
	{
		if (m_CurrentStyleValue.keyword != StyleKeyword.Undefined)
		{
			return m_CurrentStyleValue.keyword.ToString();
		}
		return m_CurrentStyleValue.number.ToString();
	}

	public StyleLength ReadStyleLength(int index)
	{
		StyleLength result = new StyleLength(m_CurrentStyleValue.length, m_CurrentStyleValue.keyword);
		result.specificity = specificity;
		return result;
	}

	public StyleFloat ReadStyleFloat(int index)
	{
		StyleFloat result = new StyleFloat(m_CurrentStyleValue.number, m_CurrentStyleValue.keyword);
		result.specificity = specificity;
		return result;
	}

	public StyleInt ReadStyleInt(int index)
	{
		StyleInt result = new StyleInt((int)m_CurrentStyleValue.number, m_CurrentStyleValue.keyword);
		result.specificity = specificity;
		return result;
	}

	public StyleColor ReadStyleColor(int index)
	{
		StyleColor result = new StyleColor(m_CurrentStyleValue.color, m_CurrentStyleValue.keyword);
		result.specificity = specificity;
		return result;
	}

	public StyleInt ReadStyleEnum<T>(int index)
	{
		StyleInt result = new StyleInt((int)m_CurrentStyleValue.number, m_CurrentStyleValue.keyword);
		result.specificity = specificity;
		return result;
	}

	public StyleFont ReadStyleFont(int index)
	{
		Font v = null;
		if (m_CurrentStyleValue.resource.IsAllocated)
		{
			v = m_CurrentStyleValue.resource.Target as Font;
		}
		StyleFont result = new StyleFont(v, m_CurrentStyleValue.keyword);
		result.specificity = specificity;
		return result;
	}

	public StyleBackground ReadStyleBackground(int index)
	{
		StyleBackground result = new StyleBackground(m_CurrentStyleValue.keyword);
		if (m_CurrentStyleValue.resource.IsAllocated)
		{
			Texture2D texture2D = m_CurrentStyleValue.resource.Target as Texture2D;
			if (texture2D != null)
			{
				result = new StyleBackground(texture2D, m_CurrentStyleValue.keyword);
			}
			else
			{
				VectorImage vectorImage = m_CurrentStyleValue.resource.Target as VectorImage;
				if (vectorImage != null)
				{
					result = new StyleBackground(vectorImage, m_CurrentStyleValue.keyword);
				}
			}
		}
		result.specificity = specificity;
		return result;
	}

	public StyleCursor ReadStyleCursor(int index)
	{
		StyleCursor result = new StyleCursor(m_CurrentCursor.value, m_CurrentCursor.keyword);
		result.specificity = specificity;
		return result;
	}
}
