using System.Globalization;
using System.Text;
using Microsoft.CSharp;

namespace System.Xml.Serialization;

public class CodeIdentifier
{
	internal static CodeDomProvider csharp = new CSharpCodeProvider();

	internal const int MaxIdentifierLength = 511;

	[Obsolete("This class should never get constructed as it contains only static methods.")]
	public CodeIdentifier()
	{
	}

	public static string MakePascal(string identifier)
	{
		identifier = MakeValid(identifier);
		if (identifier.Length <= 2)
		{
			return identifier.ToUpper(CultureInfo.InvariantCulture);
		}
		if (char.IsLower(identifier[0]))
		{
			return char.ToUpper(identifier[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) + identifier.Substring(1);
		}
		return identifier;
	}

	public static string MakeCamel(string identifier)
	{
		identifier = MakeValid(identifier);
		if (identifier.Length <= 2)
		{
			return identifier.ToLower(CultureInfo.InvariantCulture);
		}
		if (char.IsUpper(identifier[0]))
		{
			return char.ToLower(identifier[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) + identifier.Substring(1);
		}
		return identifier;
	}

	public static string MakeValid(string identifier)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < identifier.Length; i++)
		{
			if (stringBuilder.Length >= 511)
			{
				break;
			}
			char c = identifier[i];
			if (IsValid(c))
			{
				if (stringBuilder.Length == 0 && !IsValidStart(c))
				{
					stringBuilder.Append("Item");
				}
				stringBuilder.Append(c);
			}
		}
		if (stringBuilder.Length == 0)
		{
			return "Item";
		}
		return stringBuilder.ToString();
	}

	internal static string MakeValidInternal(string identifier)
	{
		if (identifier.Length > 30)
		{
			return "Item";
		}
		return MakeValid(identifier);
	}

	private static bool IsValidStart(char c)
	{
		if (char.GetUnicodeCategory(c) == UnicodeCategory.DecimalDigitNumber)
		{
			return false;
		}
		return true;
	}

	private static bool IsValid(char c)
	{
		switch (char.GetUnicodeCategory(c))
		{
		case UnicodeCategory.EnclosingMark:
		case UnicodeCategory.LetterNumber:
		case UnicodeCategory.OtherNumber:
		case UnicodeCategory.SpaceSeparator:
		case UnicodeCategory.LineSeparator:
		case UnicodeCategory.ParagraphSeparator:
		case UnicodeCategory.Control:
		case UnicodeCategory.Format:
		case UnicodeCategory.Surrogate:
		case UnicodeCategory.PrivateUse:
		case UnicodeCategory.DashPunctuation:
		case UnicodeCategory.OpenPunctuation:
		case UnicodeCategory.ClosePunctuation:
		case UnicodeCategory.InitialQuotePunctuation:
		case UnicodeCategory.FinalQuotePunctuation:
		case UnicodeCategory.OtherPunctuation:
		case UnicodeCategory.MathSymbol:
		case UnicodeCategory.CurrencySymbol:
		case UnicodeCategory.ModifierSymbol:
		case UnicodeCategory.OtherSymbol:
		case UnicodeCategory.OtherNotAssigned:
			return false;
		default:
			return false;
		case UnicodeCategory.UppercaseLetter:
		case UnicodeCategory.LowercaseLetter:
		case UnicodeCategory.TitlecaseLetter:
		case UnicodeCategory.ModifierLetter:
		case UnicodeCategory.OtherLetter:
		case UnicodeCategory.NonSpacingMark:
		case UnicodeCategory.SpacingCombiningMark:
		case UnicodeCategory.DecimalDigitNumber:
		case UnicodeCategory.ConnectorPunctuation:
			return true;
		}
	}

	internal static void CheckValidIdentifier(string ident)
	{
		if (!CodeGenerator.IsValidLanguageIndependentIdentifier(ident))
		{
			throw new ArgumentException(Res.GetString("Identifier '{0}' is not CLS-compliant.", ident), "ident");
		}
	}

	internal static string GetCSharpName(string name)
	{
		return EscapeKeywords(name.Replace('+', '.'), csharp);
	}

	private static int GetCSharpName(Type t, Type[] parameters, int index, StringBuilder sb)
	{
		if (t.DeclaringType != null && t.DeclaringType != t)
		{
			index = GetCSharpName(t.DeclaringType, parameters, index, sb);
			sb.Append(".");
		}
		string name = t.Name;
		int num = name.IndexOf('`');
		if (num < 0)
		{
			num = name.IndexOf('!');
		}
		if (num > 0)
		{
			EscapeKeywords(name.Substring(0, num), csharp, sb);
			sb.Append("<");
			int num2 = int.Parse(name.Substring(num + 1), CultureInfo.InvariantCulture) + index;
			while (index < num2)
			{
				sb.Append(GetCSharpName(parameters[index]));
				if (index < num2 - 1)
				{
					sb.Append(",");
				}
				index++;
			}
			sb.Append(">");
		}
		else
		{
			EscapeKeywords(name, csharp, sb);
		}
		return index;
	}

	internal static string GetCSharpName(Type t)
	{
		int num = 0;
		while (t.IsArray)
		{
			t = t.GetElementType();
			num++;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("global::");
		string text = t.Namespace;
		if (text != null && text.Length > 0)
		{
			string[] array = text.Split(new char[1] { '.' });
			for (int i = 0; i < array.Length; i++)
			{
				EscapeKeywords(array[i], csharp, stringBuilder);
				stringBuilder.Append(".");
			}
		}
		Type[] parameters = ((t.IsGenericType || t.ContainsGenericParameters) ? t.GetGenericArguments() : new Type[0]);
		GetCSharpName(t, parameters, 0, stringBuilder);
		for (int j = 0; j < num; j++)
		{
			stringBuilder.Append("[]");
		}
		return stringBuilder.ToString();
	}

	private static void EscapeKeywords(string identifier, CodeDomProvider codeProvider, StringBuilder sb)
	{
		if (identifier != null && identifier.Length != 0)
		{
			int num = 0;
			while (identifier.EndsWith("[]", StringComparison.Ordinal))
			{
				num++;
				identifier = identifier.Substring(0, identifier.Length - 2);
			}
			if (identifier.Length > 0)
			{
				CheckValidIdentifier(identifier);
				identifier = codeProvider.CreateEscapedIdentifier(identifier);
				sb.Append(identifier);
			}
			for (int i = 0; i < num; i++)
			{
				sb.Append("[]");
			}
		}
	}

	private static string EscapeKeywords(string identifier, CodeDomProvider codeProvider)
	{
		if (identifier == null || identifier.Length == 0)
		{
			return identifier;
		}
		string[] array = identifier.Split('.', ',', '<', '>');
		StringBuilder stringBuilder = new StringBuilder();
		int num = -1;
		for (int i = 0; i < array.Length; i++)
		{
			if (num >= 0)
			{
				stringBuilder.Append(identifier.Substring(num, 1));
			}
			num++;
			num += array[i].Length;
			EscapeKeywords(array[i].Trim(), codeProvider, stringBuilder);
		}
		if (stringBuilder.Length == identifier.Length)
		{
			return identifier;
		}
		return stringBuilder.ToString();
	}
}
