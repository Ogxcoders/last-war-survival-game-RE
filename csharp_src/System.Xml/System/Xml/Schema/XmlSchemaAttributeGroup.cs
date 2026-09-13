using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAttributeGroup : XmlSchemaAnnotated
{
	private string name;

	private XmlSchemaObjectCollection attributes = new XmlSchemaObjectCollection();

	private XmlSchemaAnyAttribute anyAttribute;

	private XmlQualifiedName qname = XmlQualifiedName.Empty;

	private XmlSchemaAttributeGroup redefined;

	private XmlSchemaObjectTable attributeUses;

	private XmlSchemaAnyAttribute attributeWildcard;

	private int selfReferenceCount;

	[XmlAttribute("name")]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	[XmlElement("attribute", typeof(XmlSchemaAttribute))]
	[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
	public XmlSchemaObjectCollection Attributes => attributes;

	[XmlElement("anyAttribute")]
	public XmlSchemaAnyAttribute AnyAttribute
	{
		get
		{
			return anyAttribute;
		}
		set
		{
			anyAttribute = value;
		}
	}

	[XmlIgnore]
	public XmlQualifiedName QualifiedName => qname;

	[XmlIgnore]
	internal XmlSchemaObjectTable AttributeUses
	{
		get
		{
			if (attributeUses == null)
			{
				attributeUses = new XmlSchemaObjectTable();
			}
			return attributeUses;
		}
	}

	[XmlIgnore]
	internal XmlSchemaAnyAttribute AttributeWildcard
	{
		get
		{
			return attributeWildcard;
		}
		set
		{
			attributeWildcard = value;
		}
	}

	[XmlIgnore]
	public XmlSchemaAttributeGroup RedefinedAttributeGroup => redefined;

	[XmlIgnore]
	internal XmlSchemaAttributeGroup Redefined
	{
		get
		{
			return redefined;
		}
		set
		{
			redefined = value;
		}
	}

	[XmlIgnore]
	internal int SelfReferenceCount
	{
		get
		{
			return selfReferenceCount;
		}
		set
		{
			selfReferenceCount = value;
		}
	}

	[XmlIgnore]
	internal override string NameAttribute
	{
		get
		{
			return Name;
		}
		set
		{
			Name = value;
		}
	}

	internal void SetQualifiedName(XmlQualifiedName value)
	{
		qname = value;
	}

	internal override XmlSchemaObject Clone()
	{
		XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)MemberwiseClone();
		if (XmlSchemaComplexType.HasAttributeQNameRef(attributes))
		{
			xmlSchemaAttributeGroup.attributes = XmlSchemaComplexType.CloneAttributes(attributes);
			xmlSchemaAttributeGroup.attributeUses = null;
		}
		return xmlSchemaAttributeGroup;
	}
}
