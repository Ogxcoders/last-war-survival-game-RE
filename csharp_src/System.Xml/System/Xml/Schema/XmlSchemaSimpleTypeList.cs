using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleTypeList : XmlSchemaSimpleTypeContent
{
	private XmlQualifiedName itemTypeName = XmlQualifiedName.Empty;

	private XmlSchemaSimpleType itemType;

	private XmlSchemaSimpleType baseItemType;

	[XmlAttribute("itemType")]
	public XmlQualifiedName ItemTypeName
	{
		get
		{
			return itemTypeName;
		}
		set
		{
			itemTypeName = ((value == null) ? XmlQualifiedName.Empty : value);
		}
	}

	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaSimpleType ItemType
	{
		get
		{
			return itemType;
		}
		set
		{
			itemType = value;
		}
	}

	[XmlIgnore]
	public XmlSchemaSimpleType BaseItemType
	{
		get
		{
			return baseItemType;
		}
		set
		{
			baseItemType = value;
		}
	}

	internal override XmlSchemaObject Clone()
	{
		XmlSchemaSimpleTypeList obj = (XmlSchemaSimpleTypeList)MemberwiseClone();
		obj.ItemTypeName = itemTypeName.Clone();
		return obj;
	}
}
