using System.Xml;

namespace System.Runtime.Serialization;

internal class StringDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteString";

	internal override string ReadMethodName => "ReadElementContentAsString";

	internal StringDataContract()
		: this(DictionaryGlobals.StringLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	internal StringDataContract(XmlDictionaryString name, XmlDictionaryString ns)
		: base(typeof(string), name, ns)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteString((string)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context == null)
		{
			if (!TryReadNullAtTopLevel(reader))
			{
				return reader.ReadElementContentAsString();
			}
			return null;
		}
		return HandleReadValue(reader.ReadElementContentAsString(), context);
	}
}
