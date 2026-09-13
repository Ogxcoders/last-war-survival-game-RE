using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class Service : NamedItem
{
	private ServiceDescriptionFormatExtensionCollection extensions;

	private PortCollection ports;

	private ServiceDescription serviceDescription;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	[XmlElement("port")]
	public PortCollection Ports => ports;

	public ServiceDescription ServiceDescription => serviceDescription;

	public Service()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
		ports = new PortCollection(this);
		serviceDescription = null;
	}

	internal void SetParent(ServiceDescription serviceDescription)
	{
		this.serviceDescription = serviceDescription;
	}
}
