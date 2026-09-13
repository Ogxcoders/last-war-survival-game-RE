using System.Xml;

namespace System.Runtime.Diagnostics;

[Serializable]
internal class TraceRecord
{
	protected const string EventIdBase = "http://schemas.microsoft.com/2006/08/ServiceModel/";

	protected const string NamespaceSuffix = "TraceRecord";

	internal virtual string EventId => BuildEventId("Empty");

	internal virtual void WriteTo(XmlWriter writer)
	{
	}

	protected string BuildEventId(string eventId)
	{
		return "http://schemas.microsoft.com/2006/08/ServiceModel/" + eventId + "TraceRecord";
	}

	protected string XmlEncode(string text)
	{
		return DiagnosticTraceBase.XmlEncode(text);
	}
}
