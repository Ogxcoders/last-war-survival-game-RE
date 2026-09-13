namespace System.Xml.Schema;

internal class Datatype_unsignedLong : Datatype_nonNegativeInteger
{
	private static readonly Type atomicValueType = typeof(ulong);

	private static readonly Type listValueType = typeof(ulong[]);

	private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(0m, 18446744073709551615m);

	internal override FacetsChecker FacetsChecker => numeric10FacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.UnsignedLong;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override int Compare(object value1, object value2)
	{
		return ((ulong)value1).CompareTo(value2);
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		Exception ex = numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			ex = XmlConvert.TryToUInt64(s, out var result);
			if (ex == null)
			{
				ex = numeric10FacetsChecker.CheckValueFacets((decimal)result, (XmlSchemaDatatype)this);
				if (ex == null)
				{
					typedValue = result;
					return null;
				}
			}
		}
		return ex;
	}
}
