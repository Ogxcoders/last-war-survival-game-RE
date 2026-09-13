using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

public abstract class DocumentableItem
{
	private XmlElement docElement;

	private XmlAttribute[] extAttributes;

	private XmlSerializerNamespaces namespaces;

	[XmlIgnore]
	public string Documentation
	{
		get
		{
			if (docElement == null)
			{
				return "";
			}
			return docElement.InnerText;
		}
		set
		{
			if (value == null || value.Length == 0)
			{
				docElement = null;
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			docElement = xmlDocument.CreateElement("wsdl", "documentation", "http://schemas.xmlsoap.org/wsdl/");
			docElement.InnerText = value;
		}
	}

	[ComVisible(false)]
	[XmlAnyElement(Name = "documentation", Namespace = "http://schemas.xmlsoap.org/wsdl/")]
	public XmlElement DocumentationElement
	{
		get
		{
			return docElement;
		}
		set
		{
			docElement = value;
		}
	}

	[XmlAnyAttribute]
	public XmlAttribute[] ExtensibleAttributes
	{
		get
		{
			return extAttributes;
		}
		set
		{
			extAttributes = value;
		}
	}

	[XmlIgnore]
	public abstract ServiceDescriptionFormatExtensionCollection Extensions { get; }

	[XmlNamespaceDeclarations]
	public XmlSerializerNamespaces Namespaces
	{
		get
		{
			if (namespaces == null)
			{
				namespaces = new XmlSerializerNamespaces();
			}
			return namespaces;
		}
		set
		{
			namespaces = value;
		}
	}
}
