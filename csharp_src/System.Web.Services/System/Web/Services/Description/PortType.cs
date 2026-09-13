using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class PortType : NamedItem
{
	private OperationCollection operations;

	private ServiceDescription serviceDescription;

	private ServiceDescriptionFormatExtensionCollection extensions;

	[XmlElement("operation")]
	public OperationCollection Operations => operations;

	public ServiceDescription ServiceDescription => serviceDescription;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public PortType()
	{
		operations = new OperationCollection(this);
		serviceDescription = null;
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
	}

	internal void SetParent(ServiceDescription serviceDescription)
	{
		this.serviceDescription = serviceDescription;
	}
}
