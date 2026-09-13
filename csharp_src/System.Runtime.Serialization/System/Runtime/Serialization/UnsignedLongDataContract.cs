namespace System.Runtime.Serialization;

internal class UnsignedLongDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteUnsignedLong";

	internal override string ReadMethodName => "ReadElementContentAsUnsignedLong";

	internal UnsignedLongDataContract()
		: base(typeof(ulong), DictionaryGlobals.UnsignedLongLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteUnsignedLong((ulong)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsUnsignedLong(), context);
		}
		return reader.ReadElementContentAsUnsignedLong();
	}
}
