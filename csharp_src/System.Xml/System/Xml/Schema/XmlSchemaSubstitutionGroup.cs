using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema;

internal class XmlSchemaSubstitutionGroup : XmlSchemaObject
{
	private ArrayList membersList = new ArrayList();

	private XmlQualifiedName examplar = XmlQualifiedName.Empty;

	[XmlIgnore]
	internal ArrayList Members => membersList;

	[XmlIgnore]
	internal XmlQualifiedName Examplar
	{
		get
		{
			return examplar;
		}
		set
		{
			examplar = value;
		}
	}
}
