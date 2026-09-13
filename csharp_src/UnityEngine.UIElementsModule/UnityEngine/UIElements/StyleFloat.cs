using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

public struct StyleFloat : IStyleValue<float>, IEquatable<StyleFloat>
{
	private StyleKeyword m_Keyword;

	private float m_Value;

	private int m_Specificity;

	public float value
	{
		get
		{
			return (m_Keyword == StyleKeyword.Undefined) ? m_Value : 0f;
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

	int IStyleValue<float>.specificity
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

	public StyleFloat(float v)
		: this(v, StyleKeyword.Undefined)
	{
	}

	public StyleFloat(StyleKeyword keyword)
		: this(0f, keyword)
	{
	}

	internal StyleFloat(float v, StyleKeyword keyword)
	{
		m_Specificity = 0;
		m_Keyword = keyword;
		m_Value = v;
	}

	internal bool Apply<U>(U other, StylePropertyApplyMode mode) where U : IStyleValue<float>
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

	bool IStyleValue<float>.Apply<U>(U other, StylePropertyApplyMode mode)
	{
		return Apply(other, mode);
	}

	public static bool operator ==(StyleFloat lhs, StyleFloat rhs)
	{
		return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
	}

	public static bool operator !=(StyleFloat lhs, StyleFloat rhs)
	{
		return !(lhs == rhs);
	}

	public static implicit operator StyleFloat(StyleKeyword keyword)
	{
		return new StyleFloat(keyword);
	}

	public static implicit operator StyleFloat(float v)
	{
		return new StyleFloat(v);
	}

	public bool Equals(StyleFloat other)
	{
		return other == this;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StyleFloat styleFloat))
		{
			return false;
		}
		return styleFloat == this;
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
