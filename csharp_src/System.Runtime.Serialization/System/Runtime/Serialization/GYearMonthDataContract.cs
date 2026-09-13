namespace System.Runtime.Serialization;

internal class GYearMonthDataContract : StringDataContract
{
	internal GYearMonthDataContract()
		: base(DictionaryGlobals.gYearMonthLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
