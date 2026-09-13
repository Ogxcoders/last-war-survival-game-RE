namespace System.Runtime.Serialization;

internal class IDREFSDataContract : StringDataContract
{
	internal IDREFSDataContract()
		: base(DictionaryGlobals.IDREFSLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
