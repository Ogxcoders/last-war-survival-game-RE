using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class EndEvent : Event
{
	private XPathNodeType nodeType;

	internal EndEvent(XPathNodeType nodeType)
	{
		this.nodeType = nodeType;
	}

	public override bool Output(Processor processor, ActionFrame frame)
	{
		return processor.EndEvent(nodeType);
	}
}
