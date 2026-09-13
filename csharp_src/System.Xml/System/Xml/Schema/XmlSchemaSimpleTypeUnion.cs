using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleTypeUnion : XmlSchemaSimpleTypeContent
{
	private XmlSchemaObjectCollection baseTypes = new XmlSchemaObjectCollection();

	private XmlQualifiedName[] memberTypes;

	private XmlSchemaSimpleType[] baseMemberTypes;

	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaObjectCollection BaseTypes => baseTypes;

	[XmlAttribute("memberTypes")]
	public XmlQualifiedName[] MemberTypes
	{
		get
		{
			return memberTypes;
		}
		set
		{
			memberTypes = value;
		}
	}

	[XmlIgnore]
	public XmlSchemaSimpleType[] BaseMemberTypes => baseMemberTypes;

	internal void SetBaseMemberTypes(XmlSchemaSimpleType[] baseMemberTypes)
	{
		this.baseMemberTypes = baseMemberTypes;
	}

	internal override XmlSchemaObject Clone()
	{
		if (memberTypes != null && memberTypes.Length != 0)
		{
			XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = (XmlSchemaSimpleTypeUnion)MemberwiseClone();
			XmlQualifiedName[] array = new XmlQualifiedName[memberTypes.Length];
			for (int i = 0; i < memberTypes.Length; i++)
			{
				array[i] = memberTypes[i].Clone();
			}
			xmlSchemaSimpleTypeUnion.MemberTypes = array;
			return xmlSchemaSimpleTypeUnion;
		}
		return this;
	}
}
