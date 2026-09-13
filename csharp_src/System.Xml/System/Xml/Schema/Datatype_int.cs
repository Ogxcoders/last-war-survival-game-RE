namespace System.Xml.Schema;

internal class Datatype_int : Datatype_long
{
	private static readonly Type atomicValueType = typeof(int);

	private static readonly Type listValueType = typeof(int[]);

	private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(-2147483648m, 2147483647m);

	internal override FacetsChecker FacetsChecker => numeric10FacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.Int;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override int Compare(object value1, object value2)
	{
		return ((int)value1).CompareTo(value2);
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		Exception ex = numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			ex = XmlConvert.TryToInt32(s, out var result);
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
