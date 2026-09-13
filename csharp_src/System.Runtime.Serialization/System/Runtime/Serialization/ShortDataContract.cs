namespace System.Runtime.Serialization;

internal class ShortDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteShort";

	internal override string ReadMethodName => "ReadElementContentAsShort";

	internal ShortDataContract()
		: base(typeof(short), DictionaryGlobals.ShortLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteShort((short)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsShort(), context);
		}
		return reader.ReadElementContentAsShort();
	}
}
