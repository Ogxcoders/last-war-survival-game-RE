using System.Xml;

namespace System.Runtime.Serialization;

internal class TimeSpanDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteTimeSpan";

	internal override string ReadMethodName => "ReadElementContentAsTimeSpan";

	internal TimeSpanDataContract()
		: this(DictionaryGlobals.TimeSpanLocalName, DictionaryGlobals.SerializationNamespace)
	{
	}

	internal TimeSpanDataContract(XmlDictionaryString name, XmlDictionaryString ns)
		: base(typeof(TimeSpan), name, ns)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteTimeSpan((TimeSpan)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context != null)
		{
			return HandleReadValue(reader.ReadElementContentAsTimeSpan(), context);
		}
		return reader.ReadElementContentAsTimeSpan();
	}
}
