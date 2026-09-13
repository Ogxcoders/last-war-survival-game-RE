namespace System.Xml.Schema;

internal sealed class XmlSchemaCollectionNode
{
	private string namespaceUri;

	private SchemaInfo schemaInfo;

	private XmlSchema schema;

	internal string NamespaceURI
	{
		get
		{
			return namespaceUri;
		}
		set
		{
			namespaceUri = value;
		}
	}

	internal SchemaInfo SchemaInfo
	{
		get
		{
			return schemaInfo;
		}
		set
		{
			schemaInfo = value;
		}
	}

	internal XmlSchema Schema
	{
		get
		{
			return schema;
		}
		set
		{
			schema = value;
		}
	}
}
