using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition;

internal interface IAttributedImport
{
	string ContractName { get; }

	Type ContractType { get; }

	bool AllowRecomposition { get; }

	CreationPolicy RequiredCreationPolicy { get; }

	ImportCardinality Cardinality { get; }

	ImportSource Source { get; }
}
