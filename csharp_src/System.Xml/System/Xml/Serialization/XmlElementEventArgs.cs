namespace System.Xml.Serialization;

public class XmlElementEventArgs : EventArgs
{
	private object o;

	private XmlElement elem;

	private string qnames;

	private int lineNumber;

	private int linePosition;

	public object ObjectBeingDeserialized => o;

	public XmlElement Element => elem;

	public int LineNumber => lineNumber;

	public int LinePosition => linePosition;

	public string ExpectedElements
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

	internal XmlElementEventArgs(XmlElement elem, int lineNumber, int linePosition, object o, string qnames)
	{
		this.elem = elem;
		this.o = o;
		this.qnames = qnames;
		this.lineNumber = lineNumber;
		this.linePosition = linePosition;
	}
}
