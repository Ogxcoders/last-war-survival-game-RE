using System.Globalization;

namespace Microsoft.CSharp;

internal class CodeGenerator
{
	public static bool IsValidLanguageIndependentIdentifier(string value)
	{
		return IsValidTypeNameOrIdentifier(value, isTypeName: false);
	}

	private static bool IsValidTypeNameOrIdentifier(string value, bool isTypeName)
	{
		bool nextMustBeStartChar = true;
		if (value.Length == 0)
		{
			return false;
		}
		foreach (char c in value)
		{
			switch (char.GetUnicodeCategory(c))
			{
			case UnicodeCategory.UppercaseLetter:
			case UnicodeCategory.LowercaseLetter:
			case UnicodeCategory.TitlecaseLetter:
			case UnicodeCategory.ModifierLetter:
			case UnicodeCategory.OtherLetter:
			case UnicodeCategory.LetterNumber:
				nextMustBeStartChar = false;
				break;
			case UnicodeCategory.NonSpacingMark:
			case UnicodeCategory.SpacingCombiningMark:
			case UnicodeCategory.DecimalDigitNumber:
			case UnicodeCategory.ConnectorPunctuation:
				if (nextMustBeStartChar && c != '_')
				{
					return false;
				}
				nextMustBeStartChar = false;
				break;
			default:
				if (!isTypeName || !IsSpecialTypeChar(c, ref nextMustBeStartChar))
				{
					return false;
				}
				break;
			}
		}
		return true;
	}

	private static bool IsSpecialTypeChar(char ch, ref bool nextMustBeStartChar)
	{
		switch (ch)
		{
		case '$':
		case '&':
		case '*':
		case '+':
		case ',':
		case '-':
		case '.':
		case ':':
		case '<':
		case '>':
		case '[':
		case ']':
			nextMustBeStartChar = true;
			return true;
		case '`':
			return true;
		default:
			return false;
		}
	}
}
