namespace System.Xml.Schema;

internal class Datatype_short : Datatype_int
{
	private static readonly Type atomicValueType = typeof(short);

	private static readonly Type listValueType = typeof(short[]);

	private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(-32768m, 32767m);

	internal override FacetsChecker FacetsChecker => numeric10FacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.Short;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override int Compare(object value1, object value2)
	{
		return ((short)value1).CompareTo(value2);
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		Exception ex = numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			ex = XmlConvert.TryToInt16(s, out var result);
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
