using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class InputBinding : MessageBinding
{
	private ServiceDescriptionFormatExtensionCollection extensions;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public InputBinding()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
	}
}
