using System.Xml.Serialization;

namespace System.Web.Services.Description;

public enum ServiceDescriptionImportStyle
{
	[XmlEnum("client")]
	Client,
	[XmlEnum("server")]
	Server,
	[XmlEnum("serverInterface")]
	ServerInterface
}
