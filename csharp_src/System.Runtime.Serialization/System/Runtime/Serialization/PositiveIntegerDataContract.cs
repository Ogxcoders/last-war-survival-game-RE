namespace System.Runtime.Serialization;

internal class PositiveIntegerDataContract : LongDataContract
{
	internal PositiveIntegerDataContract()
		: base(DictionaryGlobals.positiveIntegerLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
