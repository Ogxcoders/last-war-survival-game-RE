namespace System.Runtime.Serialization;

internal class NormalizedStringDataContract : StringDataContract
{
	internal NormalizedStringDataContract()
		: base(DictionaryGlobals.normalizedStringLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
