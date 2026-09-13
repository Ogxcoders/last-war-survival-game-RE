using System.Globalization;
using System.Text;

namespace System.Xml.Xsl.Runtime;

internal class DecimalFormatter
{
	private NumberFormatInfo posFormatInfo;

	private NumberFormatInfo negFormatInfo;

	private string posFormat;

	private string negFormat;

	private char zeroDigit;

	private const string ClrSpecialChars = "0#.,%‰Ee\\'\";";

	private const char EscChar = '\a';

	public DecimalFormatter(string formatPicture, DecimalFormat decimalFormat)
	{
		if (formatPicture.Length == 0)
		{
			throw XsltException.Create("Format cannot be empty.");
		}
		zeroDigit = decimalFormat.zeroDigit;
		posFormatInfo = (NumberFormatInfo)decimalFormat.info.Clone();
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		char c = posFormatInfo.NumberDecimalSeparator[0];
		char c2 = posFormatInfo.NumberGroupSeparator[0];
		char c3 = posFormatInfo.PercentSymbol[0];
		char c4 = posFormatInfo.PerMilleSymbol[0];
		int num = 0;
		int num2 = 0;
		int num3 = -1;
		int num4 = -1;
		for (int i = 0; i < formatPicture.Length; i++)
		{
			char c5 = formatPicture[i];
			if (c5 == decimalFormat.digit)
			{
				if (flag3 && flag)
				{
					throw XsltException.Create("Format '{0}' cannot have digit symbol after zero digit symbol before a decimal point.", formatPicture);
				}
				num4 = stringBuilder.Length;
				flag4 = (flag6 = true);
				stringBuilder.Append('#');
			}
			else if (c5 == decimalFormat.zeroDigit)
			{
				if (flag4 && !flag)
				{
					throw XsltException.Create("Format '{0}' cannot have zero digit symbol after digit symbol after decimal point.", formatPicture);
				}
				num4 = stringBuilder.Length;
				flag3 = (flag6 = true);
				stringBuilder.Append('0');
			}
			else if (c5 == decimalFormat.patternSeparator)
			{
				if (!flag6)
				{
					throw XsltException.Create("Format string should have at least one digit or zero digit.");
				}
				if (flag2)
				{
					throw XsltException.Create("Format '{0}' has two pattern separators.", formatPicture);
				}
				flag2 = true;
				if (num3 < 0)
				{
					num3 = num4 + 1;
				}
				num2 = RemoveTrailingComma(stringBuilder, num, num3);
				if (num2 > 9)
				{
					num2 = 0;
				}
				posFormatInfo.NumberGroupSizes = new int[1] { num2 };
				if (!flag5)
				{
					posFormatInfo.NumberDecimalDigits = 0;
				}
				posFormat = stringBuilder.ToString();
				stringBuilder.Length = 0;
				num3 = -1;
				num4 = -1;
				num = 0;
				flag4 = (flag3 = (flag6 = false));
				flag5 = false;
				flag = true;
				negFormatInfo = (NumberFormatInfo)decimalFormat.info.Clone();
				negFormatInfo.NegativeSign = string.Empty;
			}
			else if (c5 == c)
			{
				if (flag5)
				{
					throw XsltException.Create("Format '{0}' cannot have two decimal separators.", formatPicture);
				}
				num3 = stringBuilder.Length;
				flag5 = true;
				flag4 = (flag3 = (flag = false));
				stringBuilder.Append('.');
			}
			else if (c5 == c2)
			{
				num = stringBuilder.Length;
				num4 = num;
				stringBuilder.Append(',');
			}
			else if (c5 == c3)
			{
				stringBuilder.Append('%');
			}
			else if (c5 == c4)
			{
				stringBuilder.Append('‰');
			}
			else if (c5 == '\'')
			{
				int num5 = formatPicture.IndexOf('\'', i + 1);
				if (num5 < 0)
				{
					num5 = formatPicture.Length - 1;
				}
				stringBuilder.Append(formatPicture, i, num5 - i + 1);
				i = num5;
			}
			else
			{
				if ((('0' <= c5 && c5 <= '9') || c5 == '\a') && decimalFormat.zeroDigit != '0')
				{
					stringBuilder.Append('\a');
				}
				if ("0#.,%‰Ee\\'\";".IndexOf(c5) >= 0)
				{
					stringBuilder.Append('\\');
				}
				stringBuilder.Append(c5);
			}
		}
		if (!flag6)
		{
			throw XsltException.Create("Format string should have at least one digit or zero digit.");
		}
		NumberFormatInfo numberFormatInfo = (flag2 ? negFormatInfo : posFormatInfo);
		if (num3 < 0)
		{
			num3 = num4 + 1;
		}
		num2 = RemoveTrailingComma(stringBuilder, num, num3);
		if (num2 > 9)
		{
			num2 = 0;
		}
		numberFormatInfo.NumberGroupSizes = new int[1] { num2 };
		if (!flag5)
		{
			numberFormatInfo.NumberDecimalDigits = 0;
		}
		if (flag2)
		{
			negFormat = stringBuilder.ToString();
		}
		else
		{
			posFormat = stringBuilder.ToString();
		}
	}

	private static int RemoveTrailingComma(StringBuilder builder, int commaIndex, int decimalIndex)
	{
		if (commaIndex > 0 && commaIndex == decimalIndex - 1)
		{
			builder.Remove(decimalIndex - 1, 1);
		}
		else if (decimalIndex > commaIndex)
		{
			return decimalIndex - commaIndex - 1;
		}
		return 0;
	}

	public string Format(double value)
	{
		NumberFormatInfo numberFormatInfo;
		string text;
		if (value < 0.0 && negFormatInfo != null)
		{
			numberFormatInfo = negFormatInfo;
			text = negFormat;
		}
		else
		{
			numberFormatInfo = posFormatInfo;
			text = posFormat;
		}
		string text2 = value.ToString(text, numberFormatInfo);
		if (zeroDigit != '0')
		{
			StringBuilder stringBuilder = new StringBuilder(text2.Length);
			int num = zeroDigit - 48;
			for (int i = 0; i < text2.Length; i++)
			{
				char c = text2[i];
				switch (c)
				{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					c = (char)(c + (ushort)num);
					break;
				case '\a':
					c = text2[++i];
					break;
				}
				stringBuilder.Append(c);
			}
			text2 = stringBuilder.ToString();
		}
		return text2;
	}

	public static string Format(double value, string formatPicture, DecimalFormat decimalFormat)
	{
		return new DecimalFormatter(formatPicture, decimalFormat).Format(value);
	}
}
