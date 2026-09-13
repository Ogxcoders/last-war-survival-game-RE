using System.Xml;

namespace System.Runtime.Serialization;

internal class CharDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteChar";

	internal override string ReadMethodName => "ReadElementContentAsChar";

	internal CharDataContract()
		: this(DictionaryGlobals.CharLocalName, DictionaryGlobals.SerializationNamespace)
	{
	}

	internal CharDataContract(XmlDictionaryString name, XmlDictionaryString ns)
		: base(typeof(char), name, ns)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteChar((char)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsChar(), context);
		}
		return reader.ReadElementContentAsChar();
	}
}
