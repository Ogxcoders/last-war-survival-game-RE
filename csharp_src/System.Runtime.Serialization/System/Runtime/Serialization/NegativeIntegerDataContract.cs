namespace System.Runtime.Serialization;

internal class NegativeIntegerDataContract : LongDataContract
{
	internal NegativeIntegerDataContract()
		: base(DictionaryGlobals.negativeIntegerLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
