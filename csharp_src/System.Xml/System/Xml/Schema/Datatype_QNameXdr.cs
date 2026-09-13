namespace System.Xml.Schema;

internal class Datatype_QNameXdr : Datatype_anySimpleType
{
	private static readonly Type atomicValueType = typeof(XmlQualifiedName);

	private static readonly Type listValueType = typeof(XmlQualifiedName[]);

	public override XmlTokenizedType TokenizedType => XmlTokenizedType.QName;

	public override Type ValueType => atomicValueType;

	internal override Type ListValueType => listValueType;

	public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
	{
		if (s == null || s.Length == 0)
		{
			throw new XmlSchemaException("The attribute value cannot be empty.", string.Empty);
		}
		if (nsmgr == null)
		{
			throw new ArgumentNullException("nsmgr");
		}
		try
		{
			string prefix;
			return XmlQualifiedName.Parse(s.Trim(), nsmgr, out prefix);
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
}
