using System.ComponentModel.Composition.Primitives;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

public static class ScopingExtensions
{
	public static bool Exports(this ComposablePartDefinition part, string contractName)
	{
		Requires.NotNull(part, "part");
		Requires.NotNull(contractName, "contractName");
		foreach (ExportDefinition exportDefinition in part.ExportDefinitions)
		{
			if (StringComparers.ContractName.Equals(contractName, exportDefinition.ContractName))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Imports(this ComposablePartDefinition part, string contractName)
	{
		Requires.NotNull(part, "part");
		Requires.NotNull(contractName, "contractName");
		foreach (ImportDefinition importDefinition in part.ImportDefinitions)
		{
			if (StringComparers.ContractName.Equals(contractName, importDefinition.ContractName))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Imports(this ComposablePartDefinition part, string contractName, ImportCardinality importCardinality)
	{
		Requires.NotNull(part, "part");
		Requires.NotNull(contractName, "contractName");
		foreach (ImportDefinition importDefinition in part.ImportDefinitions)
		{
			if (StringComparers.ContractName.Equals(contractName, importDefinition.ContractName) && importDefinition.Cardinality == importCardinality)
			{
				return true;
			}
		}
		return false;
	}

	public static bool ContainsPartMetadataWithKey(this ComposablePartDefinition part, string key)
	{
		Requires.NotNull(part, "part");
		Requires.NotNull(key, "key");
		return part.Metadata.ContainsKey(key);
	}

	public static bool ContainsPartMetadata<T>(this ComposablePartDefinition part, string key, T value)
	{
		Requires.NotNull(part, "part");
		Requires.NotNull(key, "key");
		object value2 = null;
		if (part.Metadata.TryGetValue(key, out value2))
		{
			return value?.Equals(value2) ?? (value2 == null);
		}
		return false;
	}

	public static FilteredCatalog Filter(this ComposablePartCatalog catalog, Func<ComposablePartDefinition, bool> filter)
	{
		Requires.NotNull(catalog, "catalog");
		Requires.NotNull(filter, "filter");
		return new FilteredCatalog(catalog, filter);
	}
}
