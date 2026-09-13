using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPrefix("mime", "http://schemas.xmlsoap.org/wsdl/mime/")]
[XmlFormatExtension("content", "http://schemas.xmlsoap.org/wsdl/mime/", typeof(InputBinding), typeof(OutputBinding))]
public sealed class MimeContentBinding : ServiceDescriptionFormatExtension
{
	public const string Namespace = "http://schemas.xmlsoap.org/wsdl/mime/";

	private string part;

	private string type;

	[XmlAttribute("part")]
	public string Part
	{
		get
		{
			return part;
		}
		set
		{
			part = value;
		}
	}

	[XmlAttribute("type")]
	public string Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public MimeContentBinding()
	{
		part = string.Empty;
		type = string.Empty;
	}
}
