using System.Collections.Generic;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;

namespace System.ComponentModel.Composition.Primitives;

internal static class PrimitivesServices
{
	public static bool IsGeneric(this ComposablePartDefinition part)
	{
		return part.Metadata.GetValue<bool>("System.ComponentModel.Composition.IsGenericPart");
	}

	public static ImportDefinition GetProductImportDefinition(this ImportDefinition import)
	{
		if (import is IPartCreatorImportDefinition partCreatorImportDefinition)
		{
			return partCreatorImportDefinition.ProductImportDefinition;
		}
		return import;
	}

	internal static IEnumerable<string> GetCandidateContractNames(this ImportDefinition import, ComposablePartDefinition part)
	{
		import = import.GetProductImportDefinition();
		string text = import.ContractName;
		string genericContractName = import.Metadata.GetValue<string>("System.ComponentModel.Composition.GenericContractName");
		int[] value = import.Metadata.GetValue<int[]>("System.ComponentModel.Composition.GenericImportParametersOrderMetadataName");
		if (value != null)
		{
			int value2 = part.Metadata.GetValue<int>("System.ComponentModel.Composition.GenericPartArity");
			if (value2 > 0)
			{
				text = GenericServices.GetGenericName(text, value, value2);
			}
		}
		yield return text;
		if (!string.IsNullOrEmpty(genericContractName))
		{
			yield return genericContractName;
		}
	}

	internal static bool IsImportDependentOnPart(this ImportDefinition import, ComposablePartDefinition part, ExportDefinition export, bool expandGenerics)
	{
		import = import.GetProductImportDefinition();
		if (expandGenerics)
		{
			return part.GetExports(import).Any();
		}
		return TranslateImport(import, part).IsConstraintSatisfiedBy(export);
	}

	private static ImportDefinition TranslateImport(ImportDefinition import, ComposablePartDefinition part)
	{
		if (!(import is ContractBasedImportDefinition contractBasedImportDefinition))
		{
			return import;
		}
		int[] value = contractBasedImportDefinition.Metadata.GetValue<int[]>("System.ComponentModel.Composition.GenericImportParametersOrderMetadataName");
		if (value == null)
		{
			return import;
		}
		int value2 = part.Metadata.GetValue<int>("System.ComponentModel.Composition.GenericPartArity");
		if (value2 == 0)
		{
			return import;
		}
		string genericName = GenericServices.GetGenericName(contractBasedImportDefinition.ContractName, value, value2);
		string genericName2 = GenericServices.GetGenericName(contractBasedImportDefinition.RequiredTypeIdentity, value, value2);
		return new ContractBasedImportDefinition(genericName, genericName2, contractBasedImportDefinition.RequiredMetadata, contractBasedImportDefinition.Cardinality, contractBasedImportDefinition.IsRecomposable, isPrerequisite: false, contractBasedImportDefinition.RequiredCreationPolicy, contractBasedImportDefinition.Metadata);
	}
}
