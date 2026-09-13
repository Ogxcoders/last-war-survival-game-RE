using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.ReflectionModel;

internal abstract class ReflectionImportDefinition : ContractBasedImportDefinition, ICompositionElement
{
	private readonly ICompositionElement _origin;

	string ICompositionElement.DisplayName => GetDisplayName();

	ICompositionElement ICompositionElement.Origin => _origin;

	public ReflectionImportDefinition(string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy, IDictionary<string, object> metadata, ICompositionElement origin)
		: base(contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable, isPrerequisite, requiredCreationPolicy, metadata)
	{
		_origin = origin;
	}

	public abstract ImportingItem ToImportingItem();

	protected abstract string GetDisplayName();
}
