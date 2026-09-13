using System.Web.Services.Configuration;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class Types : DocumentableItem
{
	private ServiceDescriptionFormatExtensionCollection extensions;

	private XmlSchemas schemas;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	[XmlElement("schema", typeof(XmlSchema), Namespace = "http://www.w3.org/2001/XMLSchema")]
	public XmlSchemas Schemas => schemas;

	public Types()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
		schemas = new XmlSchemas();
	}
}
