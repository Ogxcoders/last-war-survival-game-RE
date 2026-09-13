using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

[XmlType("Detail", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
internal class Soap12FaultDetail
{
	[XmlAnyAttribute]
	public XmlAttribute[] Attributes;

	[XmlAnyElement]
	public XmlElement[] Children;

	[XmlText]
	public string Text;
}
