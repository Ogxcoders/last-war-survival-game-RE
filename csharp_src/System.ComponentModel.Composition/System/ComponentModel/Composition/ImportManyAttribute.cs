using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class ImportManyAttribute : Attribute, IAttributedImport
{
	public string ContractName { get; private set; }

	public Type ContractType { get; private set; }

	public bool AllowRecomposition { get; set; }

	public CreationPolicy RequiredCreationPolicy { get; set; }

	public ImportSource Source { get; set; }

	ImportCardinality IAttributedImport.Cardinality => ImportCardinality.ZeroOrMore;

	public ImportManyAttribute()
		: this((string)null)
	{
	}

	public ImportManyAttribute(Type contractType)
		: this(null, contractType)
	{
	}

	public ImportManyAttribute(string contractName)
		: this(contractName, null)
	{
	}

	public ImportManyAttribute(string contractName, Type contractType)
	{
		ContractName = contractName;
		ContractType = contractType;
	}
}
