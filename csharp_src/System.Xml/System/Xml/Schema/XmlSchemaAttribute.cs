using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAttribute : XmlSchemaAnnotated
{
	private string defaultValue;

	private string fixedValue;

	private string name;

	private XmlSchemaForm form;

	private XmlSchemaUse use;

	private XmlQualifiedName refName = XmlQualifiedName.Empty;

	private XmlQualifiedName typeName = XmlQualifiedName.Empty;

	private XmlQualifiedName qualifiedName = XmlQualifiedName.Empty;

	private XmlSchemaSimpleType type;

	private XmlSchemaSimpleType attributeType;

	private SchemaAttDef attDef;

	[XmlAttribute("default")]
	[DefaultValue(null)]
	public string DefaultValue
	{
		get
		{
			return defaultValue;
		}
		set
		{
			defaultValue = value;
		}
	}

	[XmlAttribute("fixed")]
	[DefaultValue(null)]
	public string FixedValue
	{
		get
		{
			return fixedValue;
		}
		set
		{
			fixedValue = value;
		}
	}

	[XmlAttribute("form")]
	[DefaultValue(XmlSchemaForm.None)]
	public XmlSchemaForm Form
	{
		get
		{
			return form;
		}
		set
		{
			form = value;
		}
	}

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

	[XmlAttribute("ref")]
	public XmlQualifiedName RefName
	{
		get
		{
			return refName;
		}
		set
		{
			refName = ((value == null) ? XmlQualifiedName.Empty : value);
		}
	}

	[XmlAttribute("type")]
	public XmlQualifiedName SchemaTypeName
	{
		get
		{
			return typeName;
		}
		set
		{
			typeName = ((value == null) ? XmlQualifiedName.Empty : value);
		}
	}

	[XmlElement("simpleType")]
	public XmlSchemaSimpleType SchemaType
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	[XmlAttribute("use")]
	[DefaultValue(XmlSchemaUse.None)]
	public XmlSchemaUse Use
	{
		get
		{
			return use;
		}
		set
		{
			use = value;
		}
	}

	[XmlIgnore]
	public XmlQualifiedName QualifiedName => qualifiedName;

	[XmlIgnore]
	[Obsolete("This property has been deprecated. Please use AttributeSchemaType property that returns a strongly typed attribute type. http://go.microsoft.com/fwlink/?linkid=14202")]
	public object AttributeType
	{
		get
		{
			if (attributeType == null)
			{
				return null;
			}
			if (attributeType.QualifiedName.Namespace == "http://www.w3.org/2001/XMLSchema")
			{
				return attributeType.Datatype;
			}
			return attributeType;
		}
	}

	[XmlIgnore]
	public XmlSchemaSimpleType AttributeSchemaType => attributeType;

	[XmlIgnore]
	internal XmlSchemaDatatype Datatype
	{
		get
		{
			if (attributeType != null)
			{
				return attributeType.Datatype;
			}
			return null;
		}
	}

	internal SchemaAttDef AttDef
	{
		get
		{
			return attDef;
		}
		set
		{
			attDef = value;
		}
	}

	internal bool HasDefault => defaultValue != null;

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

	internal XmlReader Validate(XmlReader reader, XmlResolver resolver, XmlSchemaSet schemaSet, ValidationEventHandler valEventHandler)
	{
		if (schemaSet != null)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			xmlReaderSettings.Schemas = schemaSet;
			xmlReaderSettings.ValidationEventHandler += valEventHandler;
			return new XsdValidatingReader(reader, resolver, xmlReaderSettings, this);
		}
		return null;
	}

	internal void SetQualifiedName(XmlQualifiedName value)
	{
		qualifiedName = value;
	}

	internal void SetAttributeType(XmlSchemaSimpleType value)
	{
		attributeType = value;
	}

	internal override XmlSchemaObject Clone()
	{
		XmlSchemaAttribute obj = (XmlSchemaAttribute)MemberwiseClone();
		obj.refName = refName.Clone();
		obj.typeName = typeName.Clone();
		obj.qualifiedName = qualifiedName.Clone();
		return obj;
	}
}
