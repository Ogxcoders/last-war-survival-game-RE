namespace System.Runtime.Serialization;

internal class NMTOKENDataContract : StringDataContract
{
	internal NMTOKENDataContract()
		: base(DictionaryGlobals.NMTOKENLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
