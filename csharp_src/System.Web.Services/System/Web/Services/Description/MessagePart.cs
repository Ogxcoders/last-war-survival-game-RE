using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class MessagePart : NamedItem
{
	private XmlQualifiedName element;

	private Message message;

	private XmlQualifiedName type;

	private ServiceDescriptionFormatExtensionCollection extensions;

	[XmlAttribute("element")]
	public XmlQualifiedName Element
	{
		get
		{
			return element;
		}
		set
		{
			element = value;
		}
	}

	public Message Message => message;

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

	internal bool DefinedByType
	{
		get
		{
			if (type != null)
			{
				return type != XmlQualifiedName.Empty;
			}
			return false;
		}
	}

	internal bool DefinedByElement
	{
		get
		{
			if (element != null)
			{
				return element != XmlQualifiedName.Empty;
			}
			return false;
		}
	}

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public MessagePart()
	{
		element = XmlQualifiedName.Empty;
		message = null;
		type = XmlQualifiedName.Empty;
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
	}

	internal void SetParent(Message message)
	{
		this.message = message;
	}
}
