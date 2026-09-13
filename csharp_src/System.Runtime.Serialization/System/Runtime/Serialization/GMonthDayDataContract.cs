namespace System.Runtime.Serialization;

internal class GMonthDayDataContract : StringDataContract
{
	internal GMonthDayDataContract()
		: base(DictionaryGlobals.gMonthDayLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
