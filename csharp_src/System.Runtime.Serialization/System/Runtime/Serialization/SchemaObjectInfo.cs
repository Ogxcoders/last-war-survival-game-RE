using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Runtime.Serialization;

internal class SchemaObjectInfo
{
	internal XmlSchemaType type;

	internal XmlSchemaElement element;

	internal XmlSchema schema;

	internal List<XmlSchemaType> knownTypes;

	internal SchemaObjectInfo(XmlSchemaType type, XmlSchemaElement element, XmlSchema schema, List<XmlSchemaType> knownTypes)
	{
		this.type = type;
		this.element = element;
		this.schema = schema;
		this.knownTypes = knownTypes;
	}
}
