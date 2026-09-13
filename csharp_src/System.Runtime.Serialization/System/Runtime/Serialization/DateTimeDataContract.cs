namespace System.Runtime.Serialization;

internal class DateTimeDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteDateTime";

	internal override string ReadMethodName => "ReadElementContentAsDateTime";

	internal DateTimeDataContract()
		: base(typeof(DateTime), DictionaryGlobals.DateTimeLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteDateTime((DateTime)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsDateTime(), context);
		}
		return reader.ReadElementContentAsDateTime();
	}
}
