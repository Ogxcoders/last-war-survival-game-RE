using System.Xml.XPath;

namespace MS.Internal.Xml.XPath;

internal class Variable : AstNode
{
	private string _localname;

	private string _prefix;

	public override AstType Type => AstType.Variable;

	public override XPathResultType ReturnType => XPathResultType.Any;

	public string Localname => _localname;

	public string Prefix => _prefix;

	public Variable(string name, string prefix)
	{
		_localname = name;
		_prefix = prefix;
	}
}
