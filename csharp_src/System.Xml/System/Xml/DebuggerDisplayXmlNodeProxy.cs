using System.Diagnostics;

namespace System.Xml;

[DebuggerDisplay("{ToString()}")]
internal struct DebuggerDisplayXmlNodeProxy
{
	private XmlNode node;

	public DebuggerDisplayXmlNodeProxy(XmlNode node)
	{
		this.node = node;
	}

	public override string ToString()
	{
		XmlNodeType nodeType = node.NodeType;
		string text = nodeType.ToString();
		switch (nodeType)
		{
		case XmlNodeType.Element:
		case XmlNodeType.EntityReference:
			text = text + ", Name=\"" + node.Name + "\"";
			break;
		case XmlNodeType.Attribute:
		case XmlNodeType.ProcessingInstruction:
			text = text + ", Name=\"" + node.Name + "\", Value=\"" + XmlConvert.EscapeValueForDebuggerDisplay(node.Value) + "\"";
			break;
		case XmlNodeType.Text:
		case XmlNodeType.CDATA:
		case XmlNodeType.Comment:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
		case XmlNodeType.XmlDeclaration:
			text = text + ", Value=\"" + XmlConvert.EscapeValueForDebuggerDisplay(node.Value) + "\"";
			break;
		case XmlNodeType.DocumentType:
		{
			XmlDocumentType xmlDocumentType = (XmlDocumentType)node;
			text = text + ", Name=\"" + xmlDocumentType.Name + "\", SYSTEM=\"" + xmlDocumentType.SystemId + "\", PUBLIC=\"" + xmlDocumentType.PublicId + "\", Value=\"" + XmlConvert.EscapeValueForDebuggerDisplay(xmlDocumentType.InternalSubset) + "\"";
			break;
		}
		}
		return text;
	}
}
