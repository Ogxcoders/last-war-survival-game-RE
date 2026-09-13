using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class Import : DocumentableItem
{
	private string location;

	private string ns;

	private ServiceDescription serviceDescription;

	private ServiceDescriptionFormatExtensionCollection extensions;

	[XmlAttribute("location")]
	public string Location
	{
		get
		{
			return location;
		}
		set
		{
			location = value;
		}
	}

	[XmlAttribute("namespace")]
	public string Namespace
	{
		get
		{
			return ns;
		}
		set
		{
			ns = value;
		}
	}

	public ServiceDescription ServiceDescription => serviceDescription;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public Import()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
		location = string.Empty;
		ns = string.Empty;
		serviceDescription = null;
	}

	internal void SetParent(ServiceDescription serviceDescription)
	{
		this.serviceDescription = serviceDescription;
	}
}
