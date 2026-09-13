namespace System.Xml.Serialization;

public class XmlNodeEventArgs : EventArgs
{
	private object o;

	private XmlNode xmlNode;

	private int lineNumber;

	private int linePosition;

	public object ObjectBeingDeserialized => o;

	public XmlNodeType NodeType => xmlNode.NodeType;

	public string Name => xmlNode.Name;

	public string LocalName => xmlNode.LocalName;

	public string NamespaceURI => xmlNode.NamespaceURI;

	public string Text => xmlNode.Value;

	public int LineNumber => lineNumber;

	public int LinePosition => linePosition;

	internal XmlNodeEventArgs(XmlNode xmlNode, int lineNumber, int linePosition, object o)
	{
		this.o = o;
		this.xmlNode = xmlNode;
		this.lineNumber = lineNumber;
		this.linePosition = linePosition;
	}
}
