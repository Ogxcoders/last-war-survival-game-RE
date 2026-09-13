namespace System.Runtime.Serialization;

internal class SignedByteDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteSignedByte";

	internal override string ReadMethodName => "ReadElementContentAsSignedByte";

	internal SignedByteDataContract()
		: base(typeof(sbyte), DictionaryGlobals.SignedByteLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteSignedByte((sbyte)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsSignedByte(), context);
		}
		return reader.ReadElementContentAsSignedByte();
	}
}
