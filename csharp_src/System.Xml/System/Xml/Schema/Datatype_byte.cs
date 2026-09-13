namespace System.Xml.Schema;

internal class Datatype_byte : Datatype_short
{
	private static readonly Type atomicValueType = typeof(sbyte);

	private static readonly Type listValueType = typeof(sbyte[]);

	private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(-128m, 127m);

	internal override FacetsChecker FacetsChecker => numeric10FacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.Byte;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override int Compare(object value1, object value2)
	{
		return ((sbyte)value1).CompareTo(value2);
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		Exception ex = numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			ex = XmlConvert.TryToSByte(s, out var result);
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
