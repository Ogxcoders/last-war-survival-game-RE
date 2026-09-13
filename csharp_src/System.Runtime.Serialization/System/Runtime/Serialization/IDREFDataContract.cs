namespace System.Runtime.Serialization;

internal class IDREFDataContract : StringDataContract
{
	internal IDREFDataContract()
		: base(DictionaryGlobals.IDREFLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
