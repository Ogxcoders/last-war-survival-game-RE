using System.Xml.XPath;

namespace MS.Internal.Xml.XPath;

internal class Group : AstNode
{
	private AstNode _groupNode;

	public override AstType Type => AstType.Group;

	public override XPathResultType ReturnType => XPathResultType.NodeSet;

	public AstNode GroupNode => _groupNode;

	public Group(AstNode groupNode)
	{
		_groupNode = groupNode;
	}
}
