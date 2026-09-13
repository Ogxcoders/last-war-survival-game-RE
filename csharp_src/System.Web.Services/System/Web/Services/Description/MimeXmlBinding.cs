using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("mimeXml", "http://schemas.xmlsoap.org/wsdl/mime/", typeof(MimePart), typeof(InputBinding), typeof(OutputBinding))]
public sealed class MimeXmlBinding : ServiceDescriptionFormatExtension
{
	private string part;

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

	public MimeXmlBinding()
	{
		part = string.Empty;
	}
}
