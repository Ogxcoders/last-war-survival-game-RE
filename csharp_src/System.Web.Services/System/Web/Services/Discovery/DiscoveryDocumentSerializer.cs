using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

internal class DiscoveryDocumentSerializer : XmlSerializer
{
	protected override void Serialize(object o, XmlSerializationWriter writer)
	{
		(writer as DiscoveryDocumentWriter).WriteRoot_DiscoveryDocument(o);
	}

	protected override object Deserialize(XmlSerializationReader reader)
	{
		return (reader as DiscoveryDocumentReader).ReadRoot_DiscoveryDocument();
	}

	protected override XmlSerializationWriter CreateWriter()
	{
		return new DiscoveryDocumentWriter();
	}

	protected override XmlSerializationReader CreateReader()
	{
		return new DiscoveryDocumentReader();
	}
}
