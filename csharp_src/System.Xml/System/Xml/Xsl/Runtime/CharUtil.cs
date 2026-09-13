using System.Globalization;

namespace System.Xml.Xsl.Runtime;

internal static class CharUtil
{
	public static bool IsAlphaNumeric(char ch)
	{
		int unicodeCategory = (int)char.GetUnicodeCategory(ch);
		if (unicodeCategory > 4)
		{
			if (unicodeCategory <= 10)
			{
				return unicodeCategory >= 8;
			}
			return false;
		}
		return true;
	}

	public static bool IsDecimalDigitOne(char ch)
	{
		if (char.GetUnicodeCategory(ch = (char)(ch - 1)) == UnicodeCategory.DecimalDigitNumber)
		{
			return char.GetNumericValue(ch) == 0.0;
		}
		return false;
	}
}
