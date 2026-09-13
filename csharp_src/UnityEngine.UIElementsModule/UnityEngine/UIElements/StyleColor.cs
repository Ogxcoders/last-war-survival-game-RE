using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

public struct StyleColor : IStyleValue<Color>, IEquatable<StyleColor>
{
	private StyleKeyword m_Keyword;

	private Color m_Value;

	private int m_Specificity;

	public Color value
	{
		get
		{
			return (m_Keyword == StyleKeyword.Undefined) ? m_Value : Color.clear;
		}
		set
		{
			m_Value = value;
			m_Keyword = StyleKeyword.Undefined;
		}
	}

	internal int specificity
	{
		get
		{
			return m_Specificity;
		}
		set
		{
			m_Specificity = value;
		}
	}

	int IStyleValue<Color>.specificity
	{
		get
		{
			return specificity;
		}
		set
		{
			specificity = value;
		}
	}

	public StyleKeyword keyword
	{
		get
		{
			return m_Keyword;
		}
		set
		{
			m_Keyword = value;
		}
	}

	public StyleColor(Color v)
		: this(v, StyleKeyword.Undefined)
	{
	}

	public StyleColor(StyleKeyword keyword)
		: this(Color.clear, keyword)
	{
	}

	internal StyleColor(Color v, StyleKeyword keyword)
	{
		m_Specificity = 0;
		m_Keyword = keyword;
		m_Value = v;
	}

	internal bool Apply<U>(U other, StylePropertyApplyMode mode) where U : IStyleValue<Color>
	{
		if (StyleValueExtensions.CanApply(specificity, other.specificity, mode))
		{
			value = other.value;
			keyword = other.keyword;
			specificity = other.specificity;
			return true;
		}
		return false;
	}

	bool IStyleValue<Color>.Apply<U>(U other, StylePropertyApplyMode mode)
	{
		return Apply(other, mode);
	}

	public static bool operator ==(StyleColor lhs, StyleColor rhs)
	{
		return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
	}

	public static bool operator !=(StyleColor lhs, StyleColor rhs)
	{
		return !(lhs == rhs);
	}

	public static bool operator ==(StyleColor lhs, Color rhs)
	{
		StyleColor styleColor = new StyleColor(rhs);
		return lhs == styleColor;
	}

	public static bool operator !=(StyleColor lhs, Color rhs)
	{
		return !(lhs == rhs);
	}

	public static implicit operator StyleColor(StyleKeyword keyword)
	{
		return new StyleColor(keyword);
	}

	public static implicit operator StyleColor(Color v)
	{
		return new StyleColor(v);
	}

	public bool Equals(StyleColor other)
	{
		return other == this;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StyleColor styleColor))
		{
			return false;
		}
		return styleColor == this;
	}

	public override int GetHashCode()
	{
		int num = 917506989;
		num = num * -1521134295 + m_Keyword.GetHashCode();
		num = num * -1521134295 + m_Value.GetHashCode();
		return num * -1521134295 + m_Specificity.GetHashCode();
	}

	public override string ToString()
	{
		return this.DebugString();
	}
}
