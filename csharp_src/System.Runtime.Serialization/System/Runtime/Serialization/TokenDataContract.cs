namespace System.Runtime.Serialization;

internal class TokenDataContract : StringDataContract
{
	internal TokenDataContract()
		: base(DictionaryGlobals.tokenLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
