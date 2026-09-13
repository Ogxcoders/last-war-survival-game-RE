namespace System.Xml.Schema;

internal class Datatype_doubleXdr : Datatype_double
{
	public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
	{
		double num;
		try
		{
			num = XmlConvert.ToDouble(s);
		}
		catch (Exception innerException)
		{
			throw new XmlSchemaException(Res.GetString("The value '{0}' is invalid according to its data type.", s), innerException);
		}
		if (double.IsInfinity(num) || double.IsNaN(num))
		{
			throw new XmlSchemaException("The value '{0}' is invalid according to its data type.", s);
		}
		return num;
	}
}
