namespace System.Runtime.Serialization;

internal class GDayDataContract : StringDataContract
{
	internal GDayDataContract()
		: base(DictionaryGlobals.gDayLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
