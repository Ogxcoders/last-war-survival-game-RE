namespace System.Runtime.Serialization;

internal class AsmxGuidDataContract : GuidDataContract
{
	internal AsmxGuidDataContract()
		: base(DictionaryGlobals.GuidLocalName, DictionaryGlobals.AsmxTypesNamespace)
	{
	}
}
