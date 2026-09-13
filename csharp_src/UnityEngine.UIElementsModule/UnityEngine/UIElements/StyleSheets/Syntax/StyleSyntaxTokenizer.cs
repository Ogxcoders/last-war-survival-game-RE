#define UNITY_ASSERTIONS
using System.Collections.Generic;

namespace UnityEngine.UIElements.StyleSheets.Syntax;

internal class StyleSyntaxTokenizer
{
	private List<StyleSyntaxToken> m_Tokens = new List<StyleSyntaxToken>();

	private int m_CurrentTokenIndex = -1;

	public StyleSyntaxToken current
	{
		get
		{
			if (m_CurrentTokenIndex < 0 || m_CurrentTokenIndex >= m_Tokens.Count)
			{
				return new StyleSyntaxToken(StyleSyntaxTokenType.Unknown);
			}
			return m_Tokens[m_CurrentTokenIndex];
		}
	}

	public StyleSyntaxToken MoveNext()
	{
		StyleSyntaxToken result = current;
		if (result.type == StyleSyntaxTokenType.Unknown)
		{
			return result;
		}
		m_CurrentTokenIndex++;
		result = current;
		if (m_CurrentTokenIndex == m_Tokens.Count)
		{
			m_CurrentTokenIndex = -1;
		}
		return result;
	}

	public StyleSyntaxToken PeekNext()
	{
		int num = m_CurrentTokenIndex + 1;
		if (m_CurrentTokenIndex < 0 || num >= m_Tokens.Count)
		{
			return new StyleSyntaxToken(StyleSyntaxTokenType.Unknown);
		}
		return m_Tokens[num];
	}

	public void Tokenize(string syntax)
	{
		m_Tokens.Clear();
		m_CurrentTokenIndex = 0;
		syntax = syntax.Trim(new char[1] { ' ' }).ToLower();
		for (int i = 0; i < syntax.Length; i++)
		{
			char c = syntax[i];
			switch (c)
			{
			case ' ':
				i = GlobCharacter(syntax, i, ' ');
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.Space));
				continue;
			case '|':
				if (IsNextCharacter(syntax, i, '|'))
				{
					m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.DoubleBar));
					i++;
				}
				else
				{
					m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.SingleBar));
				}
				continue;
			case '&':
				if (!IsNextCharacter(syntax, i, '&'))
				{
					string text = ((i + 1 < syntax.Length) ? syntax[i + 1].ToString() : "EOF");
					Debug.LogAssertionFormat("Expected '&' got '{0}'", text);
					m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.Unknown));
				}
				else
				{
					m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.DoubleAmpersand));
					i++;
				}
				continue;
			case ',':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.Comma));
				continue;
			case '\'':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.SingleQuote));
				continue;
			case '*':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.Asterisk));
				continue;
			case '+':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.Plus));
				continue;
			case '?':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.QuestionMark));
				continue;
			case '#':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.HashMark));
				continue;
			case '!':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.ExclamationPoint));
				continue;
			case '[':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.OpenBracket));
				continue;
			case ']':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.CloseBracket));
				continue;
			case '{':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.OpenBrace));
				continue;
			case '}':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.CloseBrace));
				continue;
			case '<':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.LessThan));
				continue;
			case '>':
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.GreaterThan));
				continue;
			}
			if (char.IsNumber(c))
			{
				int startIndex = i;
				int num = 1;
				while (IsNextNumber(syntax, i))
				{
					i++;
					num++;
				}
				string s = syntax.Substring(startIndex, num);
				int number = int.Parse(s);
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.Number, number));
			}
			else if (char.IsLetter(c))
			{
				int startIndex2 = i;
				int num2 = 1;
				while (IsNextLetterOrDash(syntax, i))
				{
					i++;
					num2++;
				}
				string text2 = syntax.Substring(startIndex2, num2);
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.String, text2));
			}
			else
			{
				Debug.LogAssertionFormat("Expected letter or number got '{0}'", c);
				m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.Unknown));
			}
		}
		m_Tokens.Add(new StyleSyntaxToken(StyleSyntaxTokenType.End));
	}

	private static bool IsNextCharacter(string s, int index, char c)
	{
		return index + 1 < s.Length && s[index + 1] == c;
	}

	private static bool IsNextLetterOrDash(string s, int index)
	{
		return index + 1 < s.Length && (char.IsLetter(s[index + 1]) || s[index + 1] == '-');
	}

	private static bool IsNextNumber(string s, int index)
	{
		return index + 1 < s.Length && char.IsNumber(s[index + 1]);
	}

	private static int GlobCharacter(string s, int index, char c)
	{
		while (IsNextCharacter(s, index, c))
		{
			index++;
		}
		return index;
	}
}
