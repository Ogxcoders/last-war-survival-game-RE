using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class OperationOutput : OperationMessage
{
	private ServiceDescriptionFormatExtensionCollection extensions;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public OperationOutput()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
	}
}
