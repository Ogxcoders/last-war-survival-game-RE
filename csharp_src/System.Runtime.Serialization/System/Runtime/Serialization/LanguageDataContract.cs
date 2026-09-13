namespace System.Runtime.Serialization;

internal class LanguageDataContract : StringDataContract
{
	internal LanguageDataContract()
		: base(DictionaryGlobals.languageLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
