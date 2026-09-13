using System.Xml;

namespace System.Runtime.Serialization;

internal class GuidDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteGuid";

	internal override string ReadMethodName => "ReadElementContentAsGuid";

	internal GuidDataContract()
		: this(DictionaryGlobals.GuidLocalName, DictionaryGlobals.SerializationNamespace)
	{
	}

	internal GuidDataContract(XmlDictionaryString name, XmlDictionaryString ns)
		: base(typeof(Guid), name, ns)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteGuid((Guid)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsGuid(), context);
		}
		return reader.ReadElementContentAsGuid();
	}
}
