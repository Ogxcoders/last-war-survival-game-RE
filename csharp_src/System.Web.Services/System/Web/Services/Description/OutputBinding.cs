using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class OutputBinding : MessageBinding
{
	private ServiceDescriptionFormatExtensionCollection extensions;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public OutputBinding()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
	}
}
