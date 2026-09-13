namespace System.Runtime.Serialization;

internal class NonPositiveIntegerDataContract : LongDataContract
{
	internal NonPositiveIntegerDataContract()
		: base(DictionaryGlobals.nonPositiveIntegerLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
