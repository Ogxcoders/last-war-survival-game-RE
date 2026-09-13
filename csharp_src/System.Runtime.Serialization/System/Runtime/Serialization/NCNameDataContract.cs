namespace System.Runtime.Serialization;

internal class NCNameDataContract : StringDataContract
{
	internal NCNameDataContract()
		: base(DictionaryGlobals.NCNameLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
