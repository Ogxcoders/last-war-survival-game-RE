using System.Xml.XPath;

namespace MS.Internal.Xml.XPath;

internal class Operand : AstNode
{
	private XPathResultType _type;

	private object _val;

	public override AstType Type => AstType.ConstantOperand;

	public override XPathResultType ReturnType => _type;

	public object OperandValue => _val;

	public Operand(string val)
	{
		_type = XPathResultType.String;
		_val = val;
	}

	public Operand(double val)
	{
		_type = XPathResultType.Number;
		_val = val;
	}
}
