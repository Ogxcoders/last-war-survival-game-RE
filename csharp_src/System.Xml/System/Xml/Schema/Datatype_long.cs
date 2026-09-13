namespace System.Xml.Schema;

internal class Datatype_long : Datatype_integer
{
	private static readonly Type atomicValueType = typeof(long);

	private static readonly Type listValueType = typeof(long[]);

	private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(-9223372036854775808m, 9223372036854775807m);

	internal override FacetsChecker FacetsChecker => numeric10FacetsChecker;

	internal override bool HasValueFacets => true;

	public override XmlTypeCode TypeCode => XmlTypeCode.Long;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override int Compare(object value1, object value2)
	{
		return ((long)value1).CompareTo(value2);
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		Exception ex = numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			ex = XmlConvert.TryToInt64(s, out var result);
			if (ex == null)
			{
				ex = numeric10FacetsChecker.CheckValueFacets(result, this);
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
