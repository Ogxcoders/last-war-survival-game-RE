using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public static class CompositionConstants
{
	private const string CompositionNamespace = "System.ComponentModel.Composition";

	public const string PartCreationPolicyMetadataName = "System.ComponentModel.Composition.CreationPolicy";

	public const string ImportSourceMetadataName = "System.ComponentModel.Composition.ImportSource";

	public const string IsGenericPartMetadataName = "System.ComponentModel.Composition.IsGenericPart";

	public const string GenericContractMetadataName = "System.ComponentModel.Composition.GenericContractName";

	public const string GenericParametersMetadataName = "System.ComponentModel.Composition.GenericParameters";

	public const string ExportTypeIdentityMetadataName = "ExportTypeIdentity";

	internal const string GenericImportParametersOrderMetadataName = "System.ComponentModel.Composition.GenericImportParametersOrderMetadataName";

	internal const string GenericExportParametersOrderMetadataName = "System.ComponentModel.Composition.GenericExportParametersOrderMetadataName";

	internal const string GenericPartArityMetadataName = "System.ComponentModel.Composition.GenericPartArity";

	internal const string GenericParameterConstraintsMetadataName = "System.ComponentModel.Composition.GenericParameterConstraints";

	internal const string GenericParameterAttributesMetadataName = "System.ComponentModel.Composition.GenericParameterAttributes";

	internal const string ProductDefinitionMetadataName = "ProductDefinition";

	internal const string PartCreatorContractName = "System.ComponentModel.Composition.Contracts.ExportFactory";

	internal static readonly string PartCreatorTypeIdentity = AttributedModelServices.GetTypeIdentity(typeof(ComposablePartDefinition));
}
