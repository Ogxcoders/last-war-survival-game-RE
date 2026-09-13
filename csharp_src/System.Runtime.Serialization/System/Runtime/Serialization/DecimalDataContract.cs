namespace System.Runtime.Serialization;

internal class DecimalDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteDecimal";

	internal override string ReadMethodName => "ReadElementContentAsDecimal";

	internal DecimalDataContract()
		: base(typeof(decimal), DictionaryGlobals.DecimalLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteDecimal((decimal)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsDecimal(), context);
		}
		return reader.ReadElementContentAsDecimal();
	}
}
