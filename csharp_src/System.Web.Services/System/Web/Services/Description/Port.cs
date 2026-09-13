using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class Port : NamedItem
{
	private XmlQualifiedName binding;

	private ServiceDescriptionFormatExtensionCollection extensions;

	private Service service;

	[XmlAttribute("binding")]
	public XmlQualifiedName Binding
	{
		get
		{
			return binding;
		}
		set
		{
			binding = value;
		}
	}

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public Service Service => service;

	public Port()
	{
		binding = null;
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
		service = null;
	}

	internal void SetParent(Service service)
	{
		this.service = service;
	}
}
