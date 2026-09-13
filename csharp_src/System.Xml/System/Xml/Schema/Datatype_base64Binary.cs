namespace System.Xml.Schema;

internal class Datatype_base64Binary : Datatype_anySimpleType
{
	private static readonly Type atomicValueType = typeof(byte[]);

	private static readonly Type listValueType = typeof(byte[][]);

	internal override FacetsChecker FacetsChecker => DatatypeImplementation.binaryFacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.Base64Binary;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet => XmlSchemaWhiteSpace.Collapse;

	internal override RestrictionFlags ValidRestrictionFlags => RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace;

	internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
	{
		return XmlMiscConverter.Create(schemaType);
	}

	internal override int Compare(object value1, object value2)
	{
		return Compare((byte[])value1, (byte[])value2);
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		Exception ex = DatatypeImplementation.binaryFacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			byte[] array = null;
			try
			{
				array = Convert.FromBase64String(s);
			}
			catch (ArgumentException ex2)
			{
				ex = ex2;
				goto IL_003c;
			}
			catch (FormatException ex3)
			{
				ex = ex3;
				goto IL_003c;
			}
			ex = DatatypeImplementation.binaryFacetsChecker.CheckValueFacets(array, this);
			if (ex == null)
			{
				typedValue = array;
				return null;
			}
		}
		goto IL_003c;
		IL_003c:
		return ex;
	}
}
