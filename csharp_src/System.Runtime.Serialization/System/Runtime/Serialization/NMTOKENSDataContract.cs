namespace System.Runtime.Serialization;

internal class NMTOKENSDataContract : StringDataContract
{
	internal NMTOKENSDataContract()
		: base(DictionaryGlobals.NMTOKENSLocalName, DictionaryGlobals.SchemaNamespace)
	{
	}
}
