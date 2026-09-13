namespace System.Runtime.Serialization;

internal class UnsignedIntDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteUnsignedInt";

	internal override string ReadMethodName => "ReadElementContentAsUnsignedInt";

	internal UnsignedIntDataContract()
		: base(typeof(uint), DictionaryGlobals.UnsignedIntLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteUnsignedInt((uint)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsUnsignedInt(), context);
		}
		return reader.ReadElementContentAsUnsignedInt();
	}
}
