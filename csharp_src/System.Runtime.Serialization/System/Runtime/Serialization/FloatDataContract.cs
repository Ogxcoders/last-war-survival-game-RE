namespace System.Runtime.Serialization;

internal class FloatDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteFloat";

	internal override string ReadMethodName => "ReadElementContentAsFloat";

	internal FloatDataContract()
		: base(typeof(float), DictionaryGlobals.FloatLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteFloat((float)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsFloat(), context);
		}
		return reader.ReadElementContentAsFloat();
	}
}
