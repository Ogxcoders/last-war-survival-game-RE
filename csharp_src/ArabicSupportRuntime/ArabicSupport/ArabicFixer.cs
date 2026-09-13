using System;
using System.Collections.Generic;
using System.Text;

namespace ArabicSupport;

public class ArabicFixer
{
	public delegate UGUITextLines[] GetLinesInfo(string input);

	public const float ArabicJudgeRate = 0.15f;

	private static StringBuilder _stringBuilder = new StringBuilder(50);

	private static bool _lock = false;

	private static string _compareValue = "";

	private static bool _needNewString = false;

	private static string[] Holder = new string[5];

	private static string[] FixedText = new string[30];

	private static int originalStartIndex;

	private static int originalEndIndex;

	private static int originalLength;

	private static string TextHolder;

	public static string Fix(string str)
	{
		return Fix(str, showTashkeel: false, useHinduNumbers: true);
	}

	public static string Fix(string str, bool rtl)
	{
		if (rtl)
		{
			return Fix(str);
		}
		string[] array = str.Split(new char[1] { ' ' });
		string text = "";
		string text2 = "";
		string[] array2 = array;
		foreach (string text3 in array2)
		{
			if (char.IsLower(text3.ToLower()[text3.Length / 2]))
			{
				text = text + Fix(text2) + text3 + " ";
				text2 = "";
			}
			else
			{
				text2 = text2 + text3 + " ";
			}
		}
		if (text2 != "")
		{
			text += Fix(text2);
		}
		return text;
	}

	public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
	{
		ArabicFixerTool.showTashkeel = showTashkeel;
		ArabicFixerTool.useHinduNumbers = useHinduNumbers;
		if (str.Contains("\n"))
		{
			str = str.Replace("\n", Environment.NewLine);
		}
		if (str.Contains(Environment.NewLine))
		{
			string[] separator = new string[1] { Environment.NewLine };
			string[] array = str.Split(separator, StringSplitOptions.None);
			if (array.Length == 0)
			{
				return ArabicFixerTool.FixLine(str);
			}
			if (array.Length == 1)
			{
				return ArabicFixerTool.FixLine(str);
			}
			string text = ArabicFixerTool.FixLine(array[0]);
			int i = 1;
			if (array.Length > 1)
			{
				for (; i < array.Length; i++)
				{
					text = text + Environment.NewLine + ArabicFixerTool.FixLine(array[i]);
				}
			}
			return text;
		}
		return ArabicFixerTool.FixLine(str);
	}

	public static string ReverseString(string input)
	{
		List<char> list = new List<char>(input.Length);
		for (int num = input.Length - 1; num >= 0; num--)
		{
			char c = input[num];
			if (char.IsLowSurrogate(c) && num > 0 && char.IsHighSurrogate(input[num - 1]))
			{
				list.Add(input[num - 1]);
				list.Add(c);
				num--;
			}
			else
			{
				list.Add(c);
			}
		}
		return new string(list.ToArray());
	}

	public static string FixText(string value, GetLinesInfo callback)
	{
		string text = "";
		_lock = false;
		_stringBuilder.Clear();
		_compareValue = "";
		for (int i = 0; i < value.Length; i++)
		{
			if (value[i] == '<' && (i == value.Length - 1 || value[i + 1] != '-'))
			{
				_lock = true;
			}
			else if (value[i] == '>' && (i == 0 || value[i - 1] != '-'))
			{
				_lock = false;
			}
			else if (!_lock)
			{
				_stringBuilder.Append(value[i]);
			}
		}
		Holder = _stringBuilder.ToString().Split(new char[1] { '\n' });
		for (int j = 0; j < Holder.Length; j++)
		{
			int num = 0;
			text = Fix(Holder[j], showTashkeel: true, useHinduNumbers: false);
			text = FixTextHelper(text);
			string input = ReverseString(text);
			UGUITextLines[] array = callback?.Invoke(input);
			for (int k = 0; k < FixedText.Length; k++)
			{
				FixedText[k] = "";
			}
			bool flag = false;
			if (array.Length != 0 && array.Length < FixedText.Length)
			{
				for (int l = 0; l < array.Length; l++)
				{
					originalStartIndex = array[l].startIndex;
					originalEndIndex = array[l].endIndex;
					originalLength = originalEndIndex - originalStartIndex + 1;
					if (originalLength > 2)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				int num2 = 0;
				int num3 = array.Length;
				for (int m = 0; m < num3; m++)
				{
					originalStartIndex = array[m].startIndex;
					originalEndIndex = array[m].endIndex;
					originalLength = originalEndIndex - originalStartIndex + 1;
					int num4 = Math.Min(text.Length - num2, originalLength);
					if (num4 > 0)
					{
						if (m < num3 - 1)
						{
							num2 += num4;
							num2 = Math.Min(num2, text.Length);
							int startIndex = text.Length - num2;
							FixedText[m] = text.Substring(startIndex, num4);
							if (FixedText[m][0] == ' ')
							{
								FixedText[m] = FixedText[m].Substring(1) + " ";
							}
						}
						else
						{
							FixedText[m] = text.Substring(0, text.Length - num2);
						}
					}
					num = m;
				}
				text = "";
				if (j == Holder.Length - 1)
				{
					for (int n = 0; n < FixedText.Length; n++)
					{
						if (FixedText[n] != "" && FixedText[n] != "\n" && FixedText[n] != null)
						{
							if (num == 0)
							{
								TextHolder += FixedText[n];
							}
							else if (num != 0)
							{
								TextHolder = TextHolder + FixedText[n] + "\n";
								num--;
							}
							else
							{
								TextHolder += FixedText[n];
							}
						}
					}
					continue;
				}
				for (int num5 = 0; num5 < FixedText.Length; num5++)
				{
					if (FixedText[num5] != "" && FixedText[num5] != "\n" && FixedText[num5] != null)
					{
						TextHolder = TextHolder + FixedText[num5] + "\n";
					}
				}
			}
			else
			{
				TextHolder += text;
			}
		}
		text = TextHolder;
		TextHolder = "";
		return text;
	}

	private static string FixTextHelper(string originStr)
	{
		StringBuilder stringBuilder = new StringBuilder(50);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < originStr.Length; i++)
		{
			switch (originStr[i])
			{
			case '(':
			case ')':
				if (num > 0)
				{
					stringBuilder.Append(')');
					num--;
				}
				else
				{
					stringBuilder.Append('(');
					num++;
				}
				break;
			case '[':
			case ']':
				if (num2 > 0)
				{
					stringBuilder.Append(']');
					num2--;
				}
				else
				{
					stringBuilder.Append('[');
					num2++;
				}
				break;
			case '{':
			case '}':
				if (num3 > 0)
				{
					stringBuilder.Append('{');
					num3--;
				}
				else
				{
					stringBuilder.Append('}');
					num3++;
				}
				break;
			default:
				stringBuilder.Append(originStr[i]);
				break;
			}
		}
		return stringBuilder.ToString();
	}
}
