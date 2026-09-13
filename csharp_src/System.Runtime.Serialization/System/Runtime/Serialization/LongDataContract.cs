using System.Xml;

namespace System.Runtime.Serialization;

internal class LongDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteLong";

	internal override string ReadMethodName => "ReadElementContentAsLong";

	internal LongDataContract()
		: this(DictionaryGlobals.LongLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	internal LongDataContract(XmlDictionaryString name, XmlDictionaryString ns)
		: base(typeof(long), name, ns)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteLong((long)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsLong(), context);
		}
		return reader.ReadElementContentAsLong();
	}
}
