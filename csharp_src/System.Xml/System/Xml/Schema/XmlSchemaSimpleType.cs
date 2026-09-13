using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaSimpleType : XmlSchemaType
{
	private XmlSchemaSimpleTypeContent content;

	[XmlElement("restriction", typeof(XmlSchemaSimpleTypeRestriction))]
	[XmlElement("list", typeof(XmlSchemaSimpleTypeList))]
	[XmlElement("union", typeof(XmlSchemaSimpleTypeUnion))]
	public XmlSchemaSimpleTypeContent Content
	{
		get
		{
			return content;
		}
		set
		{
			content = value;
		}
	}

	internal override XmlQualifiedName DerivedFrom
	{
		get
		{
			if (content == null)
			{
				return XmlQualifiedName.Empty;
			}
			if (content is XmlSchemaSimpleTypeRestriction)
			{
				return ((XmlSchemaSimpleTypeRestriction)content).BaseTypeName;
			}
			return XmlQualifiedName.Empty;
		}
	}

	internal override XmlSchemaObject Clone()
	{
		XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)MemberwiseClone();
		if (content != null)
		{
			xmlSchemaSimpleType.Content = (XmlSchemaSimpleTypeContent)content.Clone();
		}
		return xmlSchemaSimpleType;
	}
}
