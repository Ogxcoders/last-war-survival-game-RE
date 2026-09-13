namespace UnityEngine.UIElements.StyleSheets.Syntax;

internal struct ExpressionMultiplier
{
	public const int Infinity = 10;

	private ExpressionMultiplierType m_Type;

	public int min;

	public int max;

	public ExpressionMultiplierType type
	{
		get
		{
			return m_Type;
		}
		set
		{
			SetType(value);
		}
	}

	public ExpressionMultiplier(ExpressionMultiplierType type = ExpressionMultiplierType.None)
	{
		m_Type = type;
		min = (max = 1);
		SetType(type);
	}

	private void SetType(ExpressionMultiplierType value)
	{
		m_Type = value;
		switch (value)
		{
		case ExpressionMultiplierType.ZeroOrMore:
			min = 0;
			max = 10;
			break;
		case ExpressionMultiplierType.ZeroOrOne:
			min = 0;
			max = 1;
			break;
		case ExpressionMultiplierType.OneOrMore:
		case ExpressionMultiplierType.OneOrMoreComma:
		case ExpressionMultiplierType.GroupAtLeastOne:
			min = 1;
			max = 10;
			break;
		default:
			min = (max = 1);
			break;
		}
	}
}
