namespace System.Runtime.Serialization;

internal class ENTITIESDataContract : StringDataContract
{
	internal ENTITIESDataContract()
		: base(DictionaryGlobals.ENTITIESLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
