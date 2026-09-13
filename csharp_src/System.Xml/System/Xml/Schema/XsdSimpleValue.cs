namespace System.Xml.Schema;

internal class XsdSimpleValue
{
	private XmlSchemaSimpleType xmlType;

	private object typedValue;

	public XmlSchemaSimpleType XmlType => xmlType;

	public object TypedValue => typedValue;

	public XsdSimpleValue(XmlSchemaSimpleType st, object value)
	{
		xmlType = st;
		typedValue = value;
	}
}
