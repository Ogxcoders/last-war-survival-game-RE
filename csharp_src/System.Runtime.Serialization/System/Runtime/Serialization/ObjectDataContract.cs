using System.Xml;

namespace System.Runtime.Serialization;

internal class ObjectDataContract : PrimitiveDataContract
{
	internal override string WriteMethodName => "WriteAnyType";

	internal override string ReadMethodName => "ReadElementContentAsAnyType";

	internal override bool CanContainReferences => true;

	internal override bool IsPrimitive => false;

	internal ObjectDataContract()
		: base(typeof(object), DictionaryGlobals.ObjectLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}

	public override void WriteXmlValue(XmlWriterDelegator writer, object obj, XmlObjectSerializerWriteContext context)
	{
	}

	public override object ReadXmlValue(XmlReaderDelegator reader, XmlObjectSerializerReadContext context)
	{
		object obj;
		if (reader.IsEmptyElement)
		{
			reader.Skip();
			obj = new object();
		}
		else
		{
			string localName = reader.LocalName;
			string namespaceURI = reader.NamespaceURI;
			reader.Read();
			try
			{
				reader.ReadEndElement();
				obj = new object();
			}
			catch (XmlException innerException)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Element {0} from namespace {1} cannot have child contents to be deserialized as an object. Please use XElement to deserialize this pattern of XML.", localName, namespaceURI), innerException));
			}
		}
		if (context != null)
		{
			return HandleReadValue(obj, context);
		}
		return obj;
	}
}
