namespace System.Runtime.Serialization;

internal class UnsignedShortDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteUnsignedShort";

	internal override string ReadMethodName => "ReadElementContentAsUnsignedShort";

	internal UnsignedShortDataContract()
		: base(typeof(ushort), DictionaryGlobals.UnsignedShortLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteUnsignedShort((ushort)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsUnsignedShort(), context);
		}
		return reader.ReadElementContentAsUnsignedShort();
	}
}
