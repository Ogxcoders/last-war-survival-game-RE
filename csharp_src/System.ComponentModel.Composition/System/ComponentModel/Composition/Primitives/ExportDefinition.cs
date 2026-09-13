using System.Collections.Generic;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives;

public class ExportDefinition
{
	private readonly IDictionary<string, object> _metadata = MetadataServices.EmptyMetadata;

	private readonly string _contractName;

	public virtual string ContractName
	{
		get
		{
			if (_contractName != null)
			{
				return _contractName;
			}
			throw ExceptionBuilder.CreateNotOverriddenByDerived("ContractName");
		}
	}

	public virtual IDictionary<string, object> Metadata => _metadata;

	protected ExportDefinition()
	{
	}

	public ExportDefinition(string contractName, IDictionary<string, object> metadata)
	{
		Requires.NotNullOrEmpty(contractName, "contractName");
		_contractName = contractName;
		if (metadata != null)
		{
			_metadata = metadata.AsReadOnly();
		}
	}

	public override string ToString()
	{
		return ContractName;
	}
}
