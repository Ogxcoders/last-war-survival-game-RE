namespace System.Runtime.Serialization;

internal class ByteArrayDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteBase64";

	internal override string ReadMethodName => "ReadElementContentAsBase64";

	internal ByteArrayDataContract()
		: base(typeof(byte[]), DictionaryGlobals.ByteArrayLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteBase64((byte[])obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context == null)
		{
			if (!TryReadNullAtTopLevel(reader))
			{
				return reader.ReadElementContentAsBase64();
			}
			return null;
		}
		return HandleReadValue(reader.ReadElementContentAsBase64(), context);
	}
}
