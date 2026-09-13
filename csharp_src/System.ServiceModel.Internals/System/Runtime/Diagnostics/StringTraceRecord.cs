using System.Xml;

namespace System.Runtime.Diagnostics;

internal class StringTraceRecord : TraceRecord
{
	private string elementName;

	private string content;

	internal override string EventId => BuildEventId("String");

	internal StringTraceRecord(string elementName, string content)
	{
		this.elementName = elementName;
		this.content = content;
	}

	internal override void WriteTo(XmlWriter writer)
	{
		writer.WriteElementString(elementName, content);
	}
}
