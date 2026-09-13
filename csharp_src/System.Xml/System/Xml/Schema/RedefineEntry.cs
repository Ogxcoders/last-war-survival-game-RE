namespace System.Xml.Schema;

internal class RedefineEntry
{
	internal XmlSchemaRedefine redefine;

	internal XmlSchema schemaToUpdate;

	public RedefineEntry(XmlSchemaRedefine external, XmlSchema schema)
	{
		redefine = external;
		schemaToUpdate = schema;
	}
}
