using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class NamespaceEvent : Event
{
	private string namespaceUri;

	private string name;

	public NamespaceEvent(NavigatorInput input)
	{
		namespaceUri = input.Value;
		name = input.LocalName;
	}

	public override void ReplaceNamespaceAlias(Compiler compiler)
	{
		if (namespaceUri.Length == 0)
		{
			return;
		}
		NamespaceInfo namespaceInfo = compiler.FindNamespaceAlias(namespaceUri);
		if (namespaceInfo != null)
		{
			namespaceUri = namespaceInfo.nameSpace;
			if (namespaceInfo.prefix != null)
			{
				name = namespaceInfo.prefix;
			}
		}
	}

	public override bool Output(Processor processor, ActionFrame frame)
	{
		processor.BeginEvent(XPathNodeType.Namespace, null, name, namespaceUri, empty: false);
		processor.EndEvent(XPathNodeType.Namespace);
		return true;
	}
}
