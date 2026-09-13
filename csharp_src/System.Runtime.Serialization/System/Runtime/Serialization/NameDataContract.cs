namespace System.Runtime.Serialization;

internal class NameDataContract : StringDataContract
{
	internal NameDataContract()
		: base(DictionaryGlobals.NameLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
