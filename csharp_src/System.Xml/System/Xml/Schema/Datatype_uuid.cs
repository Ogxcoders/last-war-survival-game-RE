namespace System.Xml.Schema;

internal class Datatype_uuid : Datatype_anySimpleType
{
	private static readonly Type atomicValueType = typeof(Guid);

	private static readonly Type listValueType = typeof(Guid[]);

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	internal override RestrictionFlags ValidRestrictionFlags => (RestrictionFlags)0;

	internal override int Compare(object value1, object value2)
	{
		if (!((Guid)value1/*cast due to .constrained prefix*/).Equals(value2))
		{
			return -1;
		}
		return 0;
	}

	public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
	{
		try
		{
			return XmlConvert.ToGuid(s);
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
		Guid result;
		Exception ex = XmlConvert.TryToGuid(s, out result);
		if (ex == null)
		{
			typedValue = result;
			return null;
		}
		return ex;
	}
}
