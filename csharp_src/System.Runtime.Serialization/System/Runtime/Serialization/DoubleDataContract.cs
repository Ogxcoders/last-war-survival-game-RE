namespace System.Runtime.Serialization;

internal class DoubleDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteDouble";

	internal override string ReadMethodName => "ReadElementContentAsDouble";

	internal DoubleDataContract()
		: base(typeof(double), DictionaryGlobals.DoubleLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteDouble((double)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsDouble(), context);
		}
		return reader.ReadElementContentAsDouble();
	}
}
