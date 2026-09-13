using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

internal class StyleValueCollection
{
	internal List<StyleValue> m_Values = new List<StyleValue>();

	public StyleLength GetStyleLength(StylePropertyID id)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value))
		{
			return new StyleLength(value.length, value.keyword);
		}
		return StyleKeyword.Null;
	}

	public StyleFloat GetStyleFloat(StylePropertyID id)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value))
		{
			return new StyleFloat(value.number, value.keyword);
		}
		return StyleKeyword.Null;
	}

	public StyleInt GetStyleInt(StylePropertyID id)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value))
		{
			return new StyleInt((int)value.number, value.keyword);
		}
		return StyleKeyword.Null;
	}

	public StyleColor GetStyleColor(StylePropertyID id)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value))
		{
			return new StyleColor(value.color, value.keyword);
		}
		return StyleKeyword.Null;
	}

	public StyleBackground GetStyleBackground(StylePropertyID id)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value))
		{
			Texture2D v = (value.resource.IsAllocated ? (value.resource.Target as Texture2D) : null);
			return new StyleBackground(v, value.keyword);
		}
		return StyleKeyword.Null;
	}

	public StyleFont GetStyleFont(StylePropertyID id)
	{
		StyleValue value = default(StyleValue);
		if (TryGetStyleValue(id, ref value))
		{
			Font v = (value.resource.IsAllocated ? (value.resource.Target as Font) : null);
			return new StyleFont(v, value.keyword);
		}
		return StyleKeyword.Null;
	}

	public bool TryGetStyleValue(StylePropertyID id, ref StyleValue value)
	{
		value.id = StylePropertyID.Unknown;
		foreach (StyleValue value2 in m_Values)
		{
			if (value2.id == id)
			{
				value = value2;
				return true;
			}
		}
		return false;
	}

	public void SetStyleValue(StyleValue value)
	{
		for (int i = 0; i < m_Values.Count; i++)
		{
			if (m_Values[i].id == value.id)
			{
				m_Values[i] = value;
				return;
			}
		}
		m_Values.Add(value);
	}
}
