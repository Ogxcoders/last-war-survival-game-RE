using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

[XmlRoot("discovery", Namespace = "http://schemas.xmlsoap.org/disco/")]
public sealed class DiscoveryDocument
{
	public const string Namespace = "http://schemas.xmlsoap.org/disco/";

	[XmlElement(typeof(ContractReference), Namespace = "http://schemas.xmlsoap.org/disco/scl/")]
	[XmlElement(typeof(DiscoveryDocumentReference))]
	[XmlElement(typeof(SchemaReference))]
	internal ArrayList references = new ArrayList();

	[XmlElement(typeof(SoapBinding), ElementName = "soap", Namespace = "http://schemas/xmlsoap.org/disco/schema/soap/")]
	internal ArrayList additionalInfo = new ArrayList();

	[XmlIgnore]
	public IList References => references;

	[XmlIgnore]
	internal IList AdditionalInfo => additionalInfo;

	public static bool CanRead(XmlReader xmlReader)
	{
		xmlReader.MoveToContent();
		if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "discovery")
		{
			return xmlReader.NamespaceURI == "http://schemas.xmlsoap.org/disco/";
		}
		return false;
	}

	public static DiscoveryDocument Read(Stream stream)
	{
		return Read(new XmlTextReader(stream));
	}

	public static DiscoveryDocument Read(TextReader textReader)
	{
		return Read(new XmlTextReader(textReader));
	}

	public static DiscoveryDocument Read(XmlReader xmlReader)
	{
		return (DiscoveryDocument)new DiscoveryDocumentSerializer().Deserialize(xmlReader);
	}

	public void Write(Stream stream)
	{
		new DiscoveryDocumentSerializer().Serialize(stream, this, GetNamespaceList());
	}

	public void Write(TextWriter textWriter)
	{
		new DiscoveryDocumentSerializer().Serialize(textWriter, this, GetNamespaceList());
	}

	public void Write(XmlWriter xmlWriter)
	{
		new DiscoveryDocumentSerializer().Serialize(xmlWriter, this, GetNamespaceList());
	}

	private XmlSerializerNamespaces GetNamespaceList()
	{
		XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
		xmlSerializerNamespaces.Add("scl", "http://schemas.xmlsoap.org/disco/scl/");
		return xmlSerializerNamespaces;
	}
}
