namespace System.Runtime.Serialization;

internal class IDDataContract : StringDataContract
{
	internal IDDataContract()
		: base(DictionaryGlobals.XSDIDLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
