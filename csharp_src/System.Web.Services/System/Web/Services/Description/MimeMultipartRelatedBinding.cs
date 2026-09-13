using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("multipartRelated", "http://schemas.xmlsoap.org/wsdl/mime/", typeof(InputBinding), typeof(OutputBinding))]
public sealed class MimeMultipartRelatedBinding : ServiceDescriptionFormatExtension
{
	private MimePartCollection parts;

	[XmlElement("part")]
	public MimePartCollection Parts => parts;

	public MimeMultipartRelatedBinding()
	{
		parts = new MimePartCollection();
	}
}
