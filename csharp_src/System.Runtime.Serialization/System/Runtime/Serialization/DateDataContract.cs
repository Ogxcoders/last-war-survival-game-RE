namespace System.Runtime.Serialization;

internal class DateDataContract : StringDataContract
{
	internal DateDataContract()
		: base(DictionaryGlobals.dateLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
