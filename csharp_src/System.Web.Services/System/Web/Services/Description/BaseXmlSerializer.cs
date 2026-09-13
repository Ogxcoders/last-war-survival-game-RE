using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

internal class BaseXmlSerializer : XmlSerializer
{
	protected override XmlSerializationReader CreateReader()
	{
		return new ServiceDescriptionReaderBase();
	}

	protected override XmlSerializationWriter CreateWriter()
	{
		return new ServiceDescriptionWriterBase();
	}

	public override bool CanDeserialize(XmlReader xmlReader)
	{
		return true;
	}
}
