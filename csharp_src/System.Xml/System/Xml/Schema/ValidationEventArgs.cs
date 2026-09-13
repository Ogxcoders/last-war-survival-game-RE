namespace System.Xml.Schema;

public class ValidationEventArgs : EventArgs
{
	private XmlSchemaException ex;

	private XmlSeverityType severity;

	public XmlSeverityType Severity => severity;

	public XmlSchemaException Exception => ex;

	public string Message => ex.Message;

	internal ValidationEventArgs(XmlSchemaException ex)
	{
		this.ex = ex;
		severity = XmlSeverityType.Error;
	}

	internal ValidationEventArgs(XmlSchemaException ex, XmlSeverityType severity)
	{
		this.ex = ex;
		this.severity = severity;
	}
}
