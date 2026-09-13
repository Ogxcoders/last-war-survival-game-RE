using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class Soap12FaultBaseSerializer : XmlSerializer
{
	protected override XmlSerializationReader CreateReader()
	{
		return new Soap12FaultReader();
	}

	protected override XmlSerializationWriter CreateWriter()
	{
		return new Soap12FaultWriter();
	}

	public override bool CanDeserialize(XmlReader xmlReader)
	{
		return true;
	}
}
