using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class ImportAttribute : Attribute, IAttributedImport
{
	public string ContractName { get; private set; }

	public Type ContractType { get; private set; }

	public bool AllowDefault { get; set; }

	public bool AllowRecomposition { get; set; }

	public CreationPolicy RequiredCreationPolicy { get; set; }

	public ImportSource Source { get; set; }

	ImportCardinality IAttributedImport.Cardinality
	{
		get
		{
			if (AllowDefault)
			{
				return ImportCardinality.ZeroOrOne;
			}
			return ImportCardinality.ExactlyOne;
		}
	}

	public ImportAttribute()
		: this((string)null)
	{
	}

	public ImportAttribute(Type contractType)
		: this(null, contractType)
	{
	}

	public ImportAttribute(string contractName)
		: this(contractName, null)
	{
	}

	public ImportAttribute(string contractName, Type contractType)
	{
		ContractName = contractName;
		ContractType = contractType;
	}
}
