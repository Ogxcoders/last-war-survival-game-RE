namespace System.Runtime.Serialization;

internal class BooleanDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteBoolean";

	internal override string ReadMethodName => "ReadElementContentAsBoolean";

	internal BooleanDataContract()
		: base(typeof(bool), DictionaryGlobals.BooleanLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteBoolean((bool)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsBoolean(), context);
		}
		return reader.ReadElementContentAsBoolean();
	}
}
