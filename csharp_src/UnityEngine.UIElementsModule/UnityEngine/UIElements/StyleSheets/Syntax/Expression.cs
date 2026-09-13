namespace UnityEngine.UIElements.StyleSheets.Syntax;

internal class Expression
{
	public ExpressionType type;

	public ExpressionMultiplier multiplier;

	public DataType dataType;

	public ExpressionCombinator combinator;

	public Expression[] subExpressions;

	public string keyword;

	public Expression(ExpressionType type)
	{
		this.type = type;
		combinator = ExpressionCombinator.None;
		multiplier = new ExpressionMultiplier(ExpressionMultiplierType.None);
		subExpressions = null;
		keyword = null;
	}
}
