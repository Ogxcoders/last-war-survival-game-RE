using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaElement : XmlSchemaParticle
{
	private bool isAbstract;

	private bool hasAbstractAttribute;

	private bool isNillable;

	private bool hasNillableAttribute;

	private bool isLocalTypeDerivationChecked;

	private XmlSchemaDerivationMethod block = XmlSchemaDerivationMethod.None;

	private XmlSchemaDerivationMethod final = XmlSchemaDerivationMethod.None;

	private XmlSchemaForm form;

	private string defaultValue;

	private string fixedValue;

	private string name;

	private XmlQualifiedName refName = XmlQualifiedName.Empty;

	private XmlQualifiedName substitutionGroup = XmlQualifiedName.Empty;

	private XmlQualifiedName typeName = XmlQualifiedName.Empty;

	private XmlSchemaType type;

	private XmlQualifiedName qualifiedName = XmlQualifiedName.Empty;

	private XmlSchemaType elementType;

	private XmlSchemaDerivationMethod blockResolved;

	private XmlSchemaDerivationMethod finalResolved;

	private XmlSchemaObjectCollection constraints;

	private SchemaElementDecl elementDecl;

	[XmlAttribute("abstract")]
	[DefaultValue(false)]
	public bool IsAbstract
	{
		get
		{
			return isAbstract;
		}
		set
		{
			isAbstract = value;
			hasAbstractAttribute = true;
		}
	}

	[XmlAttribute("block")]
	[DefaultValue(XmlSchemaDerivationMethod.None)]
	public XmlSchemaDerivationMethod Block
	{
		get
		{
			return block;
		}
		set
		{
			block = value;
		}
	}

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

	[XmlAttribute("final")]
	[DefaultValue(XmlSchemaDerivationMethod.None)]
	public XmlSchemaDerivationMethod Final
	{
		get
		{
			return final;
		}
		set
		{
			final = value;
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
	[DefaultValue("")]
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

	[XmlAttribute("nillable")]
	[DefaultValue(false)]
	public bool IsNillable
	{
		get
		{
			return isNillable;
		}
		set
		{
			isNillable = value;
			hasNillableAttribute = true;
		}
	}

	[XmlIgnore]
	internal bool HasNillableAttribute => hasNillableAttribute;

	[XmlIgnore]
	internal bool HasAbstractAttribute => hasAbstractAttribute;

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

	[XmlAttribute("substitutionGroup")]
	public XmlQualifiedName SubstitutionGroup
	{
		get
		{
			return substitutionGroup;
		}
		set
		{
			substitutionGroup = ((value == null) ? XmlQualifiedName.Empty : value);
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

	[XmlElement("complexType", typeof(XmlSchemaComplexType))]
	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	public XmlSchemaType SchemaType
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

	[XmlElement("key", typeof(XmlSchemaKey))]
	[XmlElement("keyref", typeof(XmlSchemaKeyref))]
	[XmlElement("unique", typeof(XmlSchemaUnique))]
	public XmlSchemaObjectCollection Constraints
	{
		get
		{
			if (constraints == null)
			{
				constraints = new XmlSchemaObjectCollection();
			}
			return constraints;
		}
	}

	[XmlIgnore]
	public XmlQualifiedName QualifiedName => qualifiedName;

	[XmlIgnore]
	[Obsolete("This property has been deprecated. Please use ElementSchemaType property that returns a strongly typed element type. http://go.microsoft.com/fwlink/?linkid=14202")]
	public object ElementType
	{
		get
		{
			if (elementType == null)
			{
				return null;
			}
			if (elementType.QualifiedName.Namespace == "http://www.w3.org/2001/XMLSchema")
			{
				return elementType.Datatype;
			}
			return elementType;
		}
	}

	[XmlIgnore]
	public XmlSchemaType ElementSchemaType => elementType;

	[XmlIgnore]
	public XmlSchemaDerivationMethod BlockResolved => blockResolved;

	[XmlIgnore]
	public XmlSchemaDerivationMethod FinalResolved => finalResolved;

	[XmlIgnore]
	internal bool HasDefault
	{
		get
		{
			if (defaultValue != null)
			{
				return defaultValue.Length > 0;
			}
			return false;
		}
	}

	internal bool HasConstraints
	{
		get
		{
			if (constraints != null)
			{
				return constraints.Count > 0;
			}
			return false;
		}
	}

	internal bool IsLocalTypeDerivationChecked
	{
		get
		{
			return isLocalTypeDerivationChecked;
		}
		set
		{
			isLocalTypeDerivationChecked = value;
		}
	}

	internal SchemaElementDecl ElementDecl
	{
		get
		{
			return elementDecl;
		}
		set
		{
			elementDecl = value;
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

	[XmlIgnore]
	internal override string NameString => qualifiedName.ToString();

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

	internal void SetElementType(XmlSchemaType value)
	{
		elementType = value;
	}

	internal void SetBlockResolved(XmlSchemaDerivationMethod value)
	{
		blockResolved = value;
	}

	internal void SetFinalResolved(XmlSchemaDerivationMethod value)
	{
		finalResolved = value;
	}

	internal override XmlSchemaObject Clone()
	{
		return Clone(null);
	}

	internal XmlSchemaObject Clone(XmlSchema parentSchema)
	{
		XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)MemberwiseClone();
		xmlSchemaElement.refName = refName.Clone();
		xmlSchemaElement.substitutionGroup = substitutionGroup.Clone();
		xmlSchemaElement.typeName = typeName.Clone();
		xmlSchemaElement.qualifiedName = qualifiedName.Clone();
		if (type is XmlSchemaComplexType xmlSchemaComplexType && xmlSchemaComplexType.QualifiedName.IsEmpty)
		{
			xmlSchemaElement.type = (XmlSchemaType)xmlSchemaComplexType.Clone(parentSchema);
		}
		xmlSchemaElement.constraints = null;
		return xmlSchemaElement;
	}
}
