using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class Fault
{
	private static XmlSerializer serializer;

	[XmlElement(Namespace = "")]
	public XmlQualifiedName faultcode;

	[XmlElement(Namespace = "")]
	public string faultstring;

	[XmlElement(Namespace = "")]
	public string faultactor;

	[SoapIgnore]
	public XmlNode detail;

	public static XmlSerializer Serializer => serializer;

	static Fault()
	{
		serializer = new FaultSerializer();
	}

	public Fault()
	{
	}

	public Fault(SoapException ex)
	{
		faultcode = ex.Code;
		faultstring = ex.Message;
		faultactor = ex.Actor;
		detail = ex.Detail;
	}
}
