using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaRedefine : XmlSchemaExternal
{
	private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();

	private XmlSchemaObjectTable attributeGroups = new XmlSchemaObjectTable();

	private XmlSchemaObjectTable types = new XmlSchemaObjectTable();

	private XmlSchemaObjectTable groups = new XmlSchemaObjectTable();

	[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
	[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroup))]
	[XmlElement("complexType", typeof(XmlSchemaComplexType))]
	[XmlElement("group", typeof(XmlSchemaGroup))]
	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaObjectCollection Items => items;

	[XmlIgnore]
	public XmlSchemaObjectTable AttributeGroups => attributeGroups;

	[XmlIgnore]
	public XmlSchemaObjectTable SchemaTypes => types;

	[XmlIgnore]
	public XmlSchemaObjectTable Groups => groups;

	public XmlSchemaRedefine()
	{
		base.Compositor = Compositor.Redefine;
	}

	internal override void AddAnnotation(XmlSchemaAnnotation annotation)
	{
		items.Add(annotation);
	}
}
