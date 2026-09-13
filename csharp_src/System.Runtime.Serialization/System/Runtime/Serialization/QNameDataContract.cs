using System.Xml;

namespace System.Runtime.Serialization;

internal class QNameDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteQName";

	internal override string ReadMethodName => "ReadElementContentAsQName";

	internal override bool IsPrimitive => false;

	internal QNameDataContract()
		: base(typeof(XmlQualifiedName), DictionaryGlobals.QNameLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
		writer.WriteQName((XmlQualifiedName)obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		if (context == null)
		{
			if (!TryReadNullAtTopLevel(reader))
			{
				return reader.ReadElementContentAsQName();
			}
			return null;
		}
		return HandleReadValue(reader.ReadElementContentAsQName(), context);
	}

	internal override void WriteRootElement(XmlWriterDelegator writer, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (ns == DictionaryGlobals.SerializationNamespace)
		{
			writer.WriteStartElement("z", name, ns);
		}
		else if (ns != null && ns.Value != null && ns.Value.Length > 0)
		{
			writer.WriteStartElement("q", name, ns);
		}
		else
		{
			writer.WriteStartElement(name, ns);
		}
	}
}
