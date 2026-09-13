using System.Xml.XPath;

namespace MS.Internal.Xml.XPath;

internal class Filter : AstNode
{
	private AstNode _input;

	private AstNode _condition;

	public override AstType Type => AstType.Filter;

	public override XPathResultType ReturnType => XPathResultType.NodeSet;

	public AstNode Input => _input;

	public AstNode Condition => _condition;

	public Filter(AstNode input, AstNode condition)
	{
		_input = input;
		_condition = condition;
	}
}
