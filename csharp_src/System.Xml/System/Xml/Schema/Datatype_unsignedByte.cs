namespace System.Xml.Schema;

internal class Datatype_unsignedByte : Datatype_unsignedShort
{
	private static readonly Type atomicValueType = typeof(byte);

	private static readonly Type listValueType = typeof(byte[]);

	private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(0m, 255m);

	internal override FacetsChecker FacetsChecker => numeric10FacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.UnsignedByte;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override int Compare(object value1, object value2)
	{
		return ((byte)value1).CompareTo(value2);
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		Exception ex = numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
		if (ex == null)
		{
			ex = XmlConvert.TryToByte(s, out var result);
			if (ex == null)
			{
				ex = numeric10FacetsChecker.CheckValueFacets((short)result, (XmlSchemaDatatype)this);
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
