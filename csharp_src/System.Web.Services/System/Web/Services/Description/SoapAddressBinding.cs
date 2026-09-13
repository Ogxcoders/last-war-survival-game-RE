using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("address", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(Port))]
public class SoapAddressBinding : ServiceDescriptionFormatExtension
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

	public SoapAddressBinding()
	{
		location = string.Empty;
	}
}
