namespace System.Runtime.Serialization;

internal class IntegerDataContract : LongDataContract
{
	internal IntegerDataContract()
		: base(DictionaryGlobals.integerLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
