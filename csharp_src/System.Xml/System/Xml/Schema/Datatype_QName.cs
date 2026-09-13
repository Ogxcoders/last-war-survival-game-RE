namespace System.Xml.Schema;

internal class Datatype_QName : Datatype_anySimpleType
{
	private static readonly Type atomicValueType = typeof(XmlQualifiedName);

	private static readonly Type listValueType = typeof(XmlQualifiedName[]);

	internal override FacetsChecker FacetsChecker => DatatypeImplementation.qnameFacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.QName;

	public override XmlTokenizedType TokenizedType => XmlTokenizedType.QName;

	internal override RestrictionFlags ValidRestrictionFlags => RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet => XmlSchemaWhiteSpace.Collapse;

	internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
	{
		return XmlMiscConverter.Create(schemaType);
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		if (s == null || s.Length == 0)
		{
			return new XmlSchemaException("The attribute value cannot be empty.", string.Empty);
		}
		Exception ex = DatatypeImplementation.qnameFacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			XmlQualifiedName xmlQualifiedName = null;
			try
			{
				xmlQualifiedName = XmlQualifiedName.Parse(s, nsmgr, out var _);
			}
			catch (ArgumentException ex2)
			{
				ex = ex2;
				goto IL_005a;
			}
			catch (XmlException ex3)
			{
				ex = ex3;
				goto IL_005a;
			}
			ex = DatatypeImplementation.qnameFacetsChecker.CheckValueFacets(xmlQualifiedName, this);
			if (ex == null)
			{
				typedValue = xmlQualifiedName;
				return null;
			}
		}
		goto IL_005a;
		IL_005a:
		return ex;
	}
}
