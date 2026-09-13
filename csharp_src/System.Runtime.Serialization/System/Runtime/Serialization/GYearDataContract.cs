namespace System.Runtime.Serialization;

internal class GYearDataContract : StringDataContract
{
	internal GYearDataContract()
		: base(DictionaryGlobals.gYearLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
