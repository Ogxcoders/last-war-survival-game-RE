using System.Collections.Generic;
using System.Text;

namespace RTLTMPro;

public static class ArabicPreprocessor
{
	private enum CharType
	{
		Arabic,
		LTRToken,
		CJK,
		Neutral,
		Other
	}

	private static readonly HashSet<char> NeutralChars = new HashSet<char>("()[]{}\"'“”‘’<>/\\-.,:;!?_~…–—|@#$%^&*+=«»。，、：；？！“”‘’（）《》「」『』……——～【】،؛؟٪٫٬");

	private static readonly StringBuilder Sb = new StringBuilder(256);

	private const char TAB_PUA = '\ue102';

	public static bool IsArabic(char c)
	{
		if ((c < '\u0600' || c > 'ۿ') && (c < 'ݐ' || c > 'ݿ') && (c < 'ﭐ' || c > 'ﰿ'))
		{
			if (c >= 'ﹰ')
			{
				return c <= 'ﻼ';
			}
			return false;
		}
		return true;
	}

	private static CharType GetCharType(char c)
	{
		if (NeutralChars.Contains(c))
		{
			return CharType.Neutral;
		}
		if (IsArabic(c))
		{
			return CharType.Arabic;
		}
		if ((c >= '一' && c <= '鿿') || (c >= '㐀' && c <= '䶿') || (c >= '豈' && c <= '\ufaff'))
		{
			return CharType.CJK;
		}
		return CharType.Other;
	}

	public static string FixSpaces(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		Sb.Clear();
		Sb.EnsureCapacity(input.Length + 8);
		int length = input.Length;
		int i = 0;
		while (i < length)
		{
			char c = input[i];
			if (c != ' ')
			{
				Sb.Append(c);
				i++;
				continue;
			}
			int num = i;
			for (; i < length && input[i] == ' '; i++)
			{
			}
			int num2 = i - num;
			char c2 = ((num - 1 >= 0) ? input[num - 1] : ' ');
			char c3 = ((i < length) ? input[i] : ' ');
			CharType charType = GetCharType(c2);
			CharType charType2 = GetCharType(c3);
			bool flag = false;
			bool flag2 = false;
			if ((charType == CharType.Neutral && charType2 != CharType.Arabic) || (charType2 == CharType.Neutral && charType != CharType.Arabic))
			{
				flag = true;
			}
			else if ((IsArabic(c2) && !IsArabic(c3)) || (!IsArabic(c2) && IsArabic(c3)))
			{
				flag2 = num2 > 1;
			}
			for (int j = 0; j < num2; j++)
			{
				Sb.Append(' ');
				if (flag && (j == 0 || j == num2 - 1))
				{
					Sb.Append('\u200e');
				}
				if (flag2 && j == num2 - 1)
				{
					Sb.Append('\u200e');
				}
			}
		}
		return Sb.ToString();
	}

	public static string FixTabForRtl(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		return text.Replace('\t', '\ue102');
	}
}
