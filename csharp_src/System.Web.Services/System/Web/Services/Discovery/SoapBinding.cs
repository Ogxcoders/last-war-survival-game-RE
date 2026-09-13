using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

[XmlRoot("soap", Namespace = "http://schemas.xmlsoap.org/disco/soap/", IsNullable = true)]
public sealed class SoapBinding
{
	public const string Namespace = "http://schemas.xmlsoap.org/disco/soap/";

	private string address;

	private XmlQualifiedName binding;

	[XmlAttribute("address")]
	public string Address
	{
		get
		{
			return address;
		}
		set
		{
			address = value;
		}
	}

	[XmlAttribute("binding")]
	public XmlQualifiedName Binding
	{
		get
		{
			return binding;
		}
		set
		{
			binding = value;
		}
	}
}
