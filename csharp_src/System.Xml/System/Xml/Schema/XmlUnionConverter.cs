namespace System.Xml.Schema;

internal class XmlUnionConverter : XmlBaseConverter
{
	private XmlValueConverter[] converters;

	private bool hasAtomicMember;

	private bool hasListMember;

	protected XmlUnionConverter(XmlSchemaType schemaType)
		: base(schemaType)
	{
		while (schemaType.DerivedBy == XmlSchemaDerivationMethod.Restriction)
		{
			schemaType = schemaType.BaseXmlSchemaType;
		}
		XmlSchemaSimpleType[] baseMemberTypes = ((XmlSchemaSimpleTypeUnion)((XmlSchemaSimpleType)schemaType).Content).BaseMemberTypes;
		converters = new XmlValueConverter[baseMemberTypes.Length];
		for (int i = 0; i < baseMemberTypes.Length; i++)
		{
			converters[i] = baseMemberTypes[i].ValueConverter;
			if (baseMemberTypes[i].Datatype.Variety == XmlSchemaDatatypeVariety.List)
			{
				hasListMember = true;
			}
			else if (baseMemberTypes[i].Datatype.Variety == XmlSchemaDatatypeVariety.Atomic)
			{
				hasAtomicMember = true;
			}
		}
	}

	public static XmlValueConverter Create(XmlSchemaType schemaType)
	{
		return new XmlUnionConverter(schemaType);
	}

	public override object ChangeType(object value, Type destinationType, IXmlNamespaceResolver nsResolver)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		Type type = value.GetType();
		if (type == XmlBaseConverter.XmlAtomicValueType && hasAtomicMember)
		{
			return ((XmlAtomicValue)value).ValueAs(destinationType, nsResolver);
		}
		if (type == XmlBaseConverter.XmlAtomicValueArrayType && hasListMember)
		{
			return XmlAnyListConverter.ItemList.ChangeType(value, destinationType, nsResolver);
		}
		if (type == XmlBaseConverter.StringType)
		{
			if (destinationType == XmlBaseConverter.StringType)
			{
				return value;
			}
			return ((XsdSimpleValue)base.SchemaType.Datatype.ParseValue((string)value, new NameTable(), nsResolver, createAtomicValue: true)).XmlType.ValueConverter.ChangeType((string)value, destinationType, nsResolver);
		}
		throw CreateInvalidClrMappingException(type, destinationType);
	}
}
