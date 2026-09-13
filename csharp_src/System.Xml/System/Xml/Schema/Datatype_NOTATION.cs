namespace System.Xml.Schema;

internal class Datatype_NOTATION : Datatype_anySimpleType
{
	private static readonly Type atomicValueType = typeof(XmlQualifiedName);

	private static readonly Type listValueType = typeof(XmlQualifiedName[]);

	internal override FacetsChecker FacetsChecker => DatatypeImplementation.qnameFacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.Notation;

	public override XmlTokenizedType TokenizedType => XmlTokenizedType.NOTATION;

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

	internal override void VerifySchemaValid(XmlSchemaObjectTable notations, XmlSchemaObject caller)
	{
		for (Datatype_NOTATION datatype_NOTATION = this; datatype_NOTATION != null; datatype_NOTATION = (Datatype_NOTATION)datatype_NOTATION.Base)
		{
			if (datatype_NOTATION.Restriction != null && (datatype_NOTATION.Restriction.Flags & RestrictionFlags.Enumeration) != 0)
			{
				for (int i = 0; i < datatype_NOTATION.Restriction.Enumeration.Count; i++)
				{
					XmlQualifiedName name = (XmlQualifiedName)datatype_NOTATION.Restriction.Enumeration[i];
					if (!notations.Contains(name))
					{
						throw new XmlSchemaException("NOTATION cannot be used directly in a schema; only data types derived from NOTATION by specifying an enumeration value can be used in a schema. All enumeration facet values must match the name of a notation declared in the current schema.", caller);
					}
				}
				return;
			}
		}
		throw new XmlSchemaException("NOTATION cannot be used directly in a schema; only data types derived from NOTATION by specifying an enumeration value can be used in a schema. All enumeration facet values must match the name of a notation declared in the current schema.", caller);
	}
}
