using System.Xml.Serialization;

namespace System.Web.Services.Description;

internal sealed class definitionsSerializer : BaseXmlSerializer
{
	protected override void Serialize(object obj, XmlSerializationWriter writer)
	{
		((ServiceDescriptionWriterBase)writer).WriteRoot_ServiceDescription(obj);
	}

	protected override object Deserialize(XmlSerializationReader reader)
	{
		return ((ServiceDescriptionReaderBase)reader).ReadRoot_ServiceDescription();
	}
}
