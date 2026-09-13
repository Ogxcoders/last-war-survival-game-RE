using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal sealed class Fault12Serializer : Soap12FaultBaseSerializer
{
	protected override void Serialize(object obj, XmlSerializationWriter writer)
	{
		((Soap12FaultWriter)writer).WriteRoot_Soap12Fault(obj);
	}

	protected override object Deserialize(XmlSerializationReader reader)
	{
		return ((Soap12FaultReader)reader).ReadRoot_Soap12Fault();
	}
}
