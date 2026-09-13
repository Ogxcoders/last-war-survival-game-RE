namespace System.Runtime.Serialization;

internal class ENTITYDataContract : StringDataContract
{
	internal ENTITYDataContract()
		: base(DictionaryGlobals.ENTITYLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
