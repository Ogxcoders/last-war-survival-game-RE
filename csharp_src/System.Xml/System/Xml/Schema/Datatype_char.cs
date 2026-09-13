namespace System.Xml.Schema;

internal class Datatype_char : Datatype_anySimpleType
{
	private static readonly Type atomicValueType = typeof(char);

	private static readonly Type listValueType = typeof(char[]);

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override RestrictionFlags ValidRestrictionFlags => (RestrictionFlags)0;

	internal override int Compare(object value1, object value2)
	{
		return ((char)value1).CompareTo(value2);
	}

	public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
	{
		try
		{
			return XmlConvert.ToChar(s);
		}
		catch (XmlSchemaException ex)
		{
			throw ex;
		}
		catch (Exception innerException)
		{
			throw new XmlSchemaException(Res.GetString("The value '{0}' is invalid according to its data type.", s), innerException);
		}
	}

	internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
	{
		typedValue = null;
		char result;
		Exception ex = XmlConvert.TryToChar(s, out result);
		if (ex == null)
		{
			typedValue = result;
			return null;
		}
		return ex;
	}
}
