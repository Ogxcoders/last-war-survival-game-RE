using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class FaultBinding : MessageBinding
{
	private ServiceDescriptionFormatExtensionCollection extensions;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public FaultBinding()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
	}
}
