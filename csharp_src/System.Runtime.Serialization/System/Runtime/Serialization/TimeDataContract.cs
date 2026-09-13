namespace System.Runtime.Serialization;

internal class TimeDataContract : StringDataContract
{
	internal TimeDataContract()
		: base(DictionaryGlobals.timeLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
