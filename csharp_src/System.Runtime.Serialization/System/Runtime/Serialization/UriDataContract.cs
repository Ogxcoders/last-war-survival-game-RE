namespace System.Runtime.Serialization;

internal class UriDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteUri";

	internal override string ReadMethodName => "ReadElementContentAsUri";

	internal UriDataContract()
		: base(typeof(Uri), DictionaryGlobals.UriLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteUri((Uri)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context == null)
		{
			if (!TryReadNullAtTopLevel(reader))
			{
				return reader.ReadElementContentAsUri();
			}
			return null;
		}
		return HandleReadValue(reader.ReadElementContentAsUri(), context);
	}
}
