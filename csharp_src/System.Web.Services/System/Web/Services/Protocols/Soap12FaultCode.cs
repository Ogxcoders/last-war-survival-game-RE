using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

[XmlType("Code", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
internal class Soap12FaultCode
{
	public XmlQualifiedName Value;

	public Soap12FaultCode Subcode;
}
