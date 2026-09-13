using System.Text.RegularExpressions;
using UnityEngine.UIElements.StyleSheets.Syntax;

namespace UnityEngine.UIElements.StyleSheets;

internal class StyleMatcher : BaseStyleMatcher
{
	private StylePropertyValueParser m_Parser = new StylePropertyValueParser();

	private string[] m_PropertyParts;

	private static readonly Regex s_NumberRegex = new Regex("^[+-]?\\d+(?:\\.\\d+)?$", RegexOptions.Compiled);

	private static readonly Regex s_IntegerRegex = new Regex("^[+-]?\\d+$", RegexOptions.Compiled);

	private static readonly Regex s_ZeroRegex = new Regex("^0(?:\\.0+)?$", RegexOptions.Compiled);

	private static readonly Regex s_LengthRegex = new Regex("^[+-]?\\d+(?:\\.\\d+)?(?:px)$", RegexOptions.Compiled);

	private static readonly Regex s_PercentRegex = new Regex("^[+-]?\\d+(?:\\.\\d+)?(?:%)$", RegexOptions.Compiled);

	private static readonly Regex s_HexColorRegex = new Regex("^#[a-fA-F0-9]{3}(?:[a-fA-F0-9]{3})?$", RegexOptions.Compiled);

	private static readonly Regex s_RgbRegex = new Regex("^rgb\\(\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*(\\d+)\\s*\\)$", RegexOptions.Compiled);

	private static readonly Regex s_RgbaRegex = new Regex("rgba\\(\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*([\\d.]+)\\s*\\)$", RegexOptions.Compiled);

	private static readonly Regex s_VarFunctionRegex = new Regex("^var\\(.+\\)$", RegexOptions.Compiled);

	private static readonly Regex s_ResourceRegex = new Regex("^resource\\((.+)\\)$", RegexOptions.Compiled);

	private static readonly Regex s_UrlRegex = new Regex("^url\\((.+)\\)$", RegexOptions.Compiled);

	private string current => base.hasCurrent ? m_PropertyParts[m_CurrentIndex] : null;

	public override int valueCount => m_PropertyParts.Length;

	public override bool isVariable => base.hasCurrent && current.StartsWith("var(");

	private void Initialize(string propertyValue)
	{
		Initialize();
		m_PropertyParts = m_Parser.Parse(propertyValue);
	}

	public MatchResult Match(Expression exp, string propertyValue)
	{
		MatchResult result = new MatchResult
		{
			errorCode = MatchResultErrorCode.None
		};
		if (string.IsNullOrEmpty(propertyValue))
		{
			result.errorCode = MatchResultErrorCode.EmptyValue;
			return result;
		}
		bool flag = false;
		Initialize(propertyValue);
		string text = current;
		if (text == "initial" || text.StartsWith("env("))
		{
			MoveNext();
			flag = true;
		}
		else
		{
			flag = Match(exp);
		}
		if (!flag)
		{
			result.errorCode = MatchResultErrorCode.Syntax;
			result.errorValue = current;
		}
		else if (base.hasCurrent)
		{
			result.errorCode = MatchResultErrorCode.ExpectedEndOfValue;
			result.errorValue = current;
		}
		return result;
	}

	protected override bool MatchKeyword(string keyword)
	{
		return current != null && keyword == current.ToLower();
	}

	protected override bool MatchNumber()
	{
		string input = current;
		Match match = s_NumberRegex.Match(input);
		return match.Success;
	}

	protected override bool MatchInteger()
	{
		string input = current;
		Match match = s_IntegerRegex.Match(input);
		return match.Success;
	}

	protected override bool MatchLength()
	{
		string input = current;
		Match match = s_LengthRegex.Match(input);
		if (match.Success)
		{
			return true;
		}
		match = s_ZeroRegex.Match(input);
		if (match.Success)
		{
			return true;
		}
		return false;
	}

	protected override bool MatchPercentage()
	{
		string input = current;
		Match match = s_PercentRegex.Match(input);
		if (match.Success)
		{
			return true;
		}
		match = s_ZeroRegex.Match(input);
		if (match.Success)
		{
			return true;
		}
		return false;
	}

	protected override bool MatchColor()
	{
		string text = current;
		Match match = s_HexColorRegex.Match(text);
		if (match.Success)
		{
			return true;
		}
		match = s_RgbRegex.Match(text);
		if (match.Success)
		{
			return true;
		}
		match = s_RgbaRegex.Match(text);
		if (match.Success)
		{
			return true;
		}
		Color color = Color.clear;
		if (StyleSheetColor.TryGetColor(text, out color))
		{
			return true;
		}
		return false;
	}

	protected override bool MatchResource()
	{
		string input = current;
		Match match = s_ResourceRegex.Match(input);
		if (!match.Success)
		{
			return false;
		}
		string input2 = match.Groups[1].Value.Trim();
		match = s_VarFunctionRegex.Match(input2);
		return !match.Success;
	}

	protected override bool MatchUrl()
	{
		string input = current;
		Match match = s_UrlRegex.Match(input);
		if (!match.Success)
		{
			return false;
		}
		string input2 = match.Groups[1].Value.Trim();
		match = s_VarFunctionRegex.Match(input2);
		return !match.Success;
	}
}
