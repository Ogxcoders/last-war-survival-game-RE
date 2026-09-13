namespace System.Runtime.Serialization;

internal class AsmxCharDataContract : CharDataContract
{
	internal AsmxCharDataContract()
		: base(DictionaryGlobals.CharLocalName, DictionaryGlobals.AsmxTypesNamespace)
	{
	}
}
