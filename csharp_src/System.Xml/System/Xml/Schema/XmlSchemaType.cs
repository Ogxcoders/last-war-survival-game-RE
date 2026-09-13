using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaType : XmlSchemaAnnotated
{
	private string name;

	private XmlSchemaDerivationMethod final = XmlSchemaDerivationMethod.None;

	private XmlSchemaDerivationMethod derivedBy;

	private XmlSchemaType baseSchemaType;

	private XmlSchemaDatatype datatype;

	private XmlSchemaDerivationMethod finalResolved;

	private volatile SchemaElementDecl elementDecl;

	private volatile XmlQualifiedName qname = XmlQualifiedName.Empty;

	private XmlSchemaType redefined;

	private XmlSchemaContentType contentType;

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

	[XmlIgnore]
	public XmlQualifiedName QualifiedName => qname;

	[XmlIgnore]
	public XmlSchemaDerivationMethod FinalResolved => finalResolved;

	[XmlIgnore]
	[Obsolete("This property has been deprecated. Please use BaseXmlSchemaType property that returns a strongly typed base schema type. http://go.microsoft.com/fwlink/?linkid=14202")]
	public object BaseSchemaType
	{
		get
		{
			if (baseSchemaType == null)
			{
				return null;
			}
			if (baseSchemaType.QualifiedName.Namespace == "http://www.w3.org/2001/XMLSchema")
			{
				return baseSchemaType.Datatype;
			}
			return baseSchemaType;
		}
	}

	[XmlIgnore]
	public XmlSchemaType BaseXmlSchemaType => baseSchemaType;

	[XmlIgnore]
	public XmlSchemaDerivationMethod DerivedBy => derivedBy;

	[XmlIgnore]
	public XmlSchemaDatatype Datatype => datatype;

	[XmlIgnore]
	public virtual bool IsMixed
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[XmlIgnore]
	public XmlTypeCode TypeCode
	{
		get
		{
			if (this == XmlSchemaComplexType.AnyType)
			{
				return XmlTypeCode.Item;
			}
			if (datatype == null)
			{
				return XmlTypeCode.None;
			}
			return datatype.TypeCode;
		}
	}

	[XmlIgnore]
	internal XmlValueConverter ValueConverter
	{
		get
		{
			if (datatype == null)
			{
				return XmlUntypedConverter.Untyped;
			}
			return datatype.ValueConverter;
		}
	}

	internal XmlSchemaContentType SchemaContentType => contentType;

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
	internal XmlSchemaType Redefined
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

	internal virtual XmlQualifiedName DerivedFrom => XmlQualifiedName.Empty;

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

	public static XmlSchemaSimpleType GetBuiltInSimpleType(XmlQualifiedName qualifiedName)
	{
		if (qualifiedName == null)
		{
			throw new ArgumentNullException("qualifiedName");
		}
		return DatatypeImplementation.GetSimpleTypeFromXsdType(qualifiedName);
	}

	public static XmlSchemaSimpleType GetBuiltInSimpleType(XmlTypeCode typeCode)
	{
		return DatatypeImplementation.GetSimpleTypeFromTypeCode(typeCode);
	}

	public static XmlSchemaComplexType GetBuiltInComplexType(XmlTypeCode typeCode)
	{
		if (typeCode == XmlTypeCode.Item)
		{
			return XmlSchemaComplexType.AnyType;
		}
		return null;
	}

	public static XmlSchemaComplexType GetBuiltInComplexType(XmlQualifiedName qualifiedName)
	{
		if (qualifiedName == null)
		{
			throw new ArgumentNullException("qualifiedName");
		}
		if (qualifiedName.Equals(XmlSchemaComplexType.AnyType.QualifiedName))
		{
			return XmlSchemaComplexType.AnyType;
		}
		if (qualifiedName.Equals(XmlSchemaComplexType.UntypedAnyType.QualifiedName))
		{
			return XmlSchemaComplexType.UntypedAnyType;
		}
		return null;
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
		qname = value;
	}

	internal void SetFinalResolved(XmlSchemaDerivationMethod value)
	{
		finalResolved = value;
	}

	internal void SetBaseSchemaType(XmlSchemaType value)
	{
		baseSchemaType = value;
	}

	internal void SetDerivedBy(XmlSchemaDerivationMethod value)
	{
		derivedBy = value;
	}

	internal void SetDatatype(XmlSchemaDatatype value)
	{
		datatype = value;
	}

	internal void SetContentType(XmlSchemaContentType value)
	{
		contentType = value;
	}

	public static bool IsDerivedFrom(XmlSchemaType derivedType, XmlSchemaType baseType, XmlSchemaDerivationMethod except)
	{
		if (derivedType == null || baseType == null)
		{
			return false;
		}
		if (derivedType == baseType)
		{
			return true;
		}
		if (baseType == XmlSchemaComplexType.AnyType)
		{
			return true;
		}
		do
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = derivedType as XmlSchemaSimpleType;
			if (baseType is XmlSchemaSimpleType xmlSchemaSimpleType2 && xmlSchemaSimpleType != null)
			{
				if (xmlSchemaSimpleType2 == DatatypeImplementation.AnySimpleType)
				{
					return true;
				}
				if ((except & derivedType.DerivedBy) != XmlSchemaDerivationMethod.Empty || !xmlSchemaSimpleType.Datatype.IsDerivedFrom(xmlSchemaSimpleType2.Datatype))
				{
					return false;
				}
				return true;
			}
			if ((except & derivedType.DerivedBy) != XmlSchemaDerivationMethod.Empty)
			{
				return false;
			}
			derivedType = derivedType.BaseXmlSchemaType;
			if (derivedType == baseType)
			{
				return true;
			}
		}
		while (derivedType != null);
		return false;
	}

	internal static bool IsDerivedFromDatatype(XmlSchemaDatatype derivedDataType, XmlSchemaDatatype baseDataType, XmlSchemaDerivationMethod except)
	{
		if (DatatypeImplementation.AnySimpleType.Datatype == baseDataType)
		{
			return true;
		}
		return derivedDataType.IsDerivedFrom(baseDataType);
	}
}
