namespace System.Runtime.Serialization;

internal class HexBinaryDataContract : StringDataContract
{
	internal HexBinaryDataContract()
		: base(DictionaryGlobals.hexBinaryLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
