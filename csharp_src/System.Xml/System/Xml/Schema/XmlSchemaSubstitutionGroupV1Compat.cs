using System.Xml.Serialization;

namespace System.Xml.Schema;

internal class XmlSchemaSubstitutionGroupV1Compat : XmlSchemaSubstitutionGroup
{
	private XmlSchemaChoice choice = new XmlSchemaChoice();

	[XmlIgnore]
	internal XmlSchemaChoice Choice => choice;
}
