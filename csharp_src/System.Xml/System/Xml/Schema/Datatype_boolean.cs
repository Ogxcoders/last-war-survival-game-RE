namespace System.Xml.Schema;

internal class Datatype_boolean : Datatype_anySimpleType
{
	private static readonly Type atomicValueType = typeof(bool);

	private static readonly Type listValueType = typeof(bool[]);

	internal override FacetsChecker FacetsChecker => DatatypeImplementation.miscFacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.Boolean;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet => XmlSchemaWhiteSpace.Collapse;

	internal override RestrictionFlags ValidRestrictionFlags => RestrictionFlags.Pattern | RestrictionFlags.WhiteSpace;

	internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
	{
		return XmlBooleanConverter.Create(schemaType);
	}

	internal override int Compare(object value1, object value2)
	{
		return ((bool)value1).CompareTo(value2);
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		Exception ex = DatatypeImplementation.miscFacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			ex = XmlConvert.TryToBoolean(s, out var result);
			if (ex == null)
			{
				typedValue = result;
				return null;
			}
		}
		return ex;
	}
}
