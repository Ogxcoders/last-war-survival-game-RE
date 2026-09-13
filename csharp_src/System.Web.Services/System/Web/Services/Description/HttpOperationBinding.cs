using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("operation", "http://schemas.xmlsoap.org/wsdl/http/", typeof(OperationBinding))]
public sealed class HttpOperationBinding : ServiceDescriptionFormatExtension
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

	public HttpOperationBinding()
	{
		location = string.Empty;
	}
}
