using System;
using System.Linq;
using System.Text;

namespace UnityEngine.UIElements;

internal static class StringUtilsExtensions
{
	private static readonly char NoDelimiter = '\0';

	private static readonly char[] WordDelimiters = new char[3] { ' ', '-', '_' };

	public static string ToPascalCase(this string text)
	{
		return ConvertCase(text, NoDelimiter, char.ToUpperInvariant, char.ToUpperInvariant);
	}

	public static string ToCamelCase(this string text)
	{
		return ConvertCase(text, NoDelimiter, char.ToLowerInvariant, char.ToUpperInvariant);
	}

	public static string ToKebabCase(this string text)
	{
		return ConvertCase(text, '-', char.ToLowerInvariant, char.ToLowerInvariant);
	}

	public static string ToTrainCase(this string text)
	{
		return ConvertCase(text, '-', char.ToUpperInvariant, char.ToUpperInvariant);
	}

	public static string ToSnakeCase(this string text)
	{
		return ConvertCase(text, '_', char.ToLowerInvariant, char.ToLowerInvariant);
	}

	private static string ConvertCase(string text, char outputWordDelimiter, Func<char, char> startOfStringCaseHandler, Func<char, char> middleStringCaseHandler)
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		bool flag2 = true;
		bool flag3 = true;
		foreach (char c in text)
		{
			if (WordDelimiters.Contains(c))
			{
				if (c == outputWordDelimiter)
				{
					stringBuilder.Append(outputWordDelimiter);
					flag3 = false;
				}
				flag2 = true;
			}
			else if (!char.IsLetterOrDigit(c))
			{
				flag = true;
				flag2 = true;
			}
			else if (flag2 || char.IsUpper(c))
			{
				if (flag)
				{
					stringBuilder.Append(startOfStringCaseHandler(c));
				}
				else
				{
					if (flag3 && outputWordDelimiter != NoDelimiter)
					{
						stringBuilder.Append(outputWordDelimiter);
					}
					stringBuilder.Append(middleStringCaseHandler(c));
					flag3 = true;
				}
				flag = false;
				flag2 = false;
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}
}
