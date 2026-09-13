using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class Binding : NamedItem
{
	private ServiceDescriptionFormatExtensionCollection extensions;

	private OperationBindingCollection operations;

	private ServiceDescription serviceDescription;

	private XmlQualifiedName type;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	[XmlElement("operation")]
	public OperationBindingCollection Operations => operations;

	public ServiceDescription ServiceDescription => serviceDescription;

	[XmlAttribute("type")]
	public XmlQualifiedName Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public Binding()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
		operations = new OperationBindingCollection(this);
		serviceDescription = null;
		type = XmlQualifiedName.Empty;
	}

	internal void SetParent(ServiceDescription serviceDescription)
	{
		this.serviceDescription = serviceDescription;
	}
}
