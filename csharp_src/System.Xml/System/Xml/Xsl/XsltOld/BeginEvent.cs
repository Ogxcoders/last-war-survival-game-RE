using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class BeginEvent : Event
{
	private XPathNodeType nodeType;

	private string namespaceUri;

	private string name;

	private string prefix;

	private bool empty;

	private object htmlProps;

	public BeginEvent(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		nodeType = input.NodeType;
		namespaceUri = input.NamespaceURI;
		name = input.LocalName;
		prefix = input.Prefix;
		empty = input.IsEmptyTag;
		if (nodeType == XPathNodeType.Element)
		{
			htmlProps = HtmlElementProps.GetProps(name);
		}
		else if (nodeType == XPathNodeType.Attribute)
		{
			htmlProps = HtmlAttributeProps.GetProps(name);
		}
	}

	public override void ReplaceNamespaceAlias(Compiler compiler)
	{
		if (nodeType == XPathNodeType.Attribute && namespaceUri.Length == 0)
		{
			return;
		}
		NamespaceInfo namespaceInfo = compiler.FindNamespaceAlias(namespaceUri);
		if (namespaceInfo != null)
		{
			namespaceUri = namespaceInfo.nameSpace;
			if (namespaceInfo.prefix != null)
			{
				prefix = namespaceInfo.prefix;
			}
		}
	}

	public override bool Output(Processor processor, ActionFrame frame)
	{
		return processor.BeginEvent(nodeType, prefix, name, namespaceUri, empty, htmlProps, search: false);
	}
}
