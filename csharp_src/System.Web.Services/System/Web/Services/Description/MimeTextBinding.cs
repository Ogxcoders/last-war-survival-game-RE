using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("text", "http://microsoft.com/wsdl/mime/textMatching/", typeof(InputBinding), typeof(OutputBinding), typeof(MimePart))]
[XmlFormatExtensionPrefix("tm", "http://microsoft.com/wsdl/mime/textMatching/")]
public sealed class MimeTextBinding : ServiceDescriptionFormatExtension
{
	public const string Namespace = "http://microsoft.com/wsdl/mime/textMatching/";

	private MimeTextMatchCollection matches;

	[XmlElement("match", typeof(MimeTextMatch))]
	public MimeTextMatchCollection Matches => matches;

	public MimeTextBinding()
	{
		matches = new MimeTextMatchCollection();
	}
}
