using System.Collections;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class Message : NamedItem
{
	private MessagePartCollection parts;

	private ServiceDescription serviceDescription;

	private ServiceDescriptionFormatExtensionCollection extensions;

	[XmlElement("part")]
	public MessagePartCollection Parts => parts;

	public ServiceDescription ServiceDescription => serviceDescription;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public Message()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
		parts = new MessagePartCollection(this);
		serviceDescription = null;
	}

	public MessagePart FindPartByName(string partName)
	{
		return parts[partName];
	}

	public MessagePart[] FindPartsByName(string[] partNames)
	{
		ArrayList arrayList = new ArrayList();
		foreach (string partName in partNames)
		{
			arrayList.Add(FindPartByName(partName));
		}
		int count = arrayList.Count;
		if (count == 0)
		{
			throw new ArgumentException();
		}
		MessagePart[] array = new MessagePart[count];
		arrayList.CopyTo(array);
		return array;
	}

	internal void SetParent(ServiceDescription serviceDescription)
	{
		this.serviceDescription = serviceDescription;
	}
}
