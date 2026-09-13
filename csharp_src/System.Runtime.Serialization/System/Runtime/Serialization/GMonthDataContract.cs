namespace System.Runtime.Serialization;

internal class GMonthDataContract : StringDataContract
{
	internal GMonthDataContract()
		: base(DictionaryGlobals.gMonthLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
