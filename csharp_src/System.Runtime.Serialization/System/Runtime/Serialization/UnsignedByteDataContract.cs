namespace System.Runtime.Serialization;

internal class UnsignedByteDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteUnsignedByte";

	internal override string ReadMethodName => "ReadElementContentAsUnsignedByte";

	internal UnsignedByteDataContract()
		: base(typeof(byte), DictionaryGlobals.UnsignedByteLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteUnsignedByte((byte)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsUnsignedByte(), context);
		}
		return reader.ReadElementContentAsUnsignedByte();
	}
}
