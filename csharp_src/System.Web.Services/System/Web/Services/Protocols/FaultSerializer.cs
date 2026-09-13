using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class FaultSerializer : XmlSerializer
{
	protected override void Serialize(object o, XmlSerializationWriter writer)
	{
		(writer as FaultWriter).WriteRoot_Fault(o);
	}

	protected override object Deserialize(XmlSerializationReader reader)
	{
		return (reader as FaultReader).ReadRoot_Fault();
	}

	protected override XmlSerializationWriter CreateWriter()
	{
		return new FaultWriter();
	}

	protected override XmlSerializationReader CreateReader()
	{
		return new FaultReader();
	}
}
