using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("address", "http://schemas.xmlsoap.org/wsdl/http/", typeof(Port))]
public sealed class HttpAddressBinding : ServiceDescriptionFormatExtension
{
	private string location;

	[XmlAttribute("location")]
	public string Location
	{
		get
		{
			return location;
		}
		set
		{
			location = value;
		}
	}

	public HttpAddressBinding()
	{
		location = string.Empty;
	}
}
