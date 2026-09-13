using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

[XmlType("Reason", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
internal class Soap12FaultReason
{
	[XmlElement("Text", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
	public Soap12FaultReasonText[] Texts;
}
