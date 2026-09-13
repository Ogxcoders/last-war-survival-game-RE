namespace System.Xml.Schema;

internal class Datatype_floatXdr : Datatype_float
{
	public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
	{
		float num;
		try
		{
			num = XmlConvert.ToSingle(s);
		}
		catch (Exception innerException)
		{
			throw new XmlSchemaException(Res.GetString("The value '{0}' is invalid according to its data type.", s), innerException);
		}
		if (float.IsInfinity(num) || float.IsNaN(num))
		{
			throw new XmlSchemaException("The value '{0}' is invalid according to its data type.", s);
		}
		return num;
	}
}
