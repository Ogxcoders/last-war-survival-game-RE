using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

[XmlType("Text", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
internal class Soap12FaultReasonText
{
	[XmlAttribute("lang", Namespace = "http://www.w3.org/XML/1998/namespace")]
	public string XmlLang;

	[XmlText]
	public string Value;
}
