using System;
using System.Text.RegularExpressions;

public static class StringUtilsExt
{
	public static bool IsNullOrEmpty(this string str)
	{
		return string.IsNullOrEmpty(str);
	}

	public static bool IsInt(this string value)
	{
		return Regex.IsMatch(value, "^[+-]?\\d*$");
	}

	public static string FixNewLine(this string str)
	{
		return str.Replace("\\n", "\n");
	}

	public static string GetFileNameNoExtension(this string path, char separator = '.')
	{
		if (path.IsNullOrEmpty())
		{
			return "";
		}
		return path.Substring(0, path.LastIndexOf(separator));
	}

	public static string GetFileName(this string path, char separator = '/')
	{
		if (path.IsNullOrEmpty())
		{
			return "";
		}
		return path.Substring(path.LastIndexOf(separator) + 1);
	}

	public static string GetFormattedSeperatorNum(int value)
	{
		string text = "";
		if (value < 0)
		{
			value = -value;
			text = "-";
		}
		return text + value.ToString("N0");
	}

	public static int GetLengthByChar(this string sourceStr)
	{
		return Regex.Replace(sourceStr, "[一-龥]", "aa", RegexOptions.IgnoreCase).Length;
	}

	public static string SubstringEx(this string sourceStr, int len)
	{
		if (sourceStr.IsNullOrEmpty())
		{
			return sourceStr;
		}
		int lengthByChar = sourceStr.GetLengthByChar();
		if (lengthByChar <= len)
		{
			return sourceStr;
		}
		string result = string.Empty;
		for (int i = Convert.ToInt32(Math.Floor(Convert.ToDouble(len / 2))); i <= sourceStr.Length; i++)
		{
			string text = sourceStr.Substring(0, i);
			int lengthByChar2 = text.GetLengthByChar();
			if (lengthByChar2 >= len)
			{
				result = text + ((lengthByChar2 < lengthByChar) ? "..." : "");
				break;
			}
		}
		return result;
	}
}
