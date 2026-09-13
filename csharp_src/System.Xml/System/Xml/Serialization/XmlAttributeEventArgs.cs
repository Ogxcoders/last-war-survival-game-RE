namespace System.Xml.Serialization;

public class XmlAttributeEventArgs : EventArgs
{
	private object o;

	private XmlAttribute attr;

	private string qnames;

	private int lineNumber;

	private int linePosition;

	public object ObjectBeingDeserialized => o;

	public XmlAttribute Attr => attr;

	public int LineNumber => lineNumber;

	public int LinePosition => linePosition;

	public string ExpectedAttributes
	{
		get
		{
			if (qnames != null)
			{
				return qnames;
			}
			return string.Empty;
		}
	}

	internal XmlAttributeEventArgs(XmlAttribute attr, int lineNumber, int linePosition, object o, string qnames)
	{
		this.attr = attr;
		this.o = o;
		this.qnames = qnames;
		this.lineNumber = lineNumber;
		this.linePosition = linePosition;
	}
}
