using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

public struct StyleInt : IStyleValue<int>, IEquatable<StyleInt>
{
	private StyleKeyword m_Keyword;

	private int m_Value;

	private int m_Specificity;

	public int value
	{
		get
		{
			return (m_Keyword == StyleKeyword.Undefined) ? m_Value : 0;
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

	int IStyleValue<int>.specificity
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

	public StyleInt(int v)
		: this(v, StyleKeyword.Undefined)
	{
	}

	public StyleInt(StyleKeyword keyword)
		: this(0, keyword)
	{
	}

	internal StyleInt(int v, StyleKeyword keyword)
	{
		m_Specificity = 0;
		m_Keyword = keyword;
		m_Value = v;
	}

	internal bool Apply<U>(U other, StylePropertyApplyMode mode) where U : IStyleValue<int>
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

	bool IStyleValue<int>.Apply<U>(U other, StylePropertyApplyMode mode)
	{
		return Apply(other, mode);
	}

	public static bool operator ==(StyleInt lhs, StyleInt rhs)
	{
		return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
	}

	public static bool operator !=(StyleInt lhs, StyleInt rhs)
	{
		return !(lhs == rhs);
	}

	public static implicit operator StyleInt(StyleKeyword keyword)
	{
		return new StyleInt(keyword);
	}

	public static implicit operator StyleInt(int v)
	{
		return new StyleInt(v);
	}

	public bool Equals(StyleInt other)
	{
		return other == this;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StyleInt styleInt))
		{
			return false;
		}
		return styleInt == this;
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
