namespace System.Runtime.Serialization;

internal class IntDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteInt";

	internal override string ReadMethodName => "ReadElementContentAsInt";

	internal IntDataContract()
		: base(typeof(int), DictionaryGlobals.IntLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteInt((int)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsInt(), context);
		}
		return reader.ReadElementContentAsInt();
	}
}
