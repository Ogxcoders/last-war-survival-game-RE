namespace System.Runtime.Serialization;

internal class NonNegativeIntegerDataContract : LongDataContract
{
	internal NonNegativeIntegerDataContract()
		: base(DictionaryGlobals.nonNegativeIntegerLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
