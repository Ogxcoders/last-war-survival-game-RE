using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Internal;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Strings
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("Microsoft.Internal.Strings", typeof(Strings).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string Argument_AssemblyReflectionOnly => ResourceManager.GetString("Argument_AssemblyReflectionOnly", resourceCulture);

	internal static string Argument_ElementReflectionOnlyType => ResourceManager.GetString("Argument_ElementReflectionOnlyType", resourceCulture);

	internal static string Argument_ExportsEmpty => ResourceManager.GetString("Argument_ExportsEmpty", resourceCulture);

	internal static string Argument_ExportsTooMany => ResourceManager.GetString("Argument_ExportsTooMany", resourceCulture);

	internal static string Argument_NullElement => ResourceManager.GetString("Argument_NullElement", resourceCulture);

	internal static string Argument_ReflectionContextReturnsReflectionOnlyType => ResourceManager.GetString("Argument_ReflectionContextReturnsReflectionOnlyType", resourceCulture);

	internal static string ArgumentException_EmptyString => ResourceManager.GetString("ArgumentException_EmptyString", resourceCulture);

	internal static string ArgumentOutOfRange_InvalidEnum => ResourceManager.GetString("ArgumentOutOfRange_InvalidEnum", resourceCulture);

	internal static string ArgumentOutOfRange_InvalidEnumInSet => ResourceManager.GetString("ArgumentOutOfRange_InvalidEnumInSet", resourceCulture);

	internal static string ArgumentValueType => ResourceManager.GetString("ArgumentValueType", resourceCulture);

	internal static string AssemblyFileNotFoundOrWrongType => ResourceManager.GetString("AssemblyFileNotFoundOrWrongType", resourceCulture);

	internal static string AtomicComposition_AlreadyCompleted => ResourceManager.GetString("AtomicComposition_AlreadyCompleted", resourceCulture);

	internal static string AtomicComposition_AlreadyNested => ResourceManager.GetString("AtomicComposition_AlreadyNested", resourceCulture);

	internal static string AtomicComposition_PartOfAnotherAtomicComposition => ResourceManager.GetString("AtomicComposition_PartOfAnotherAtomicComposition", resourceCulture);

	internal static string CardinalityMismatch_NoExports => ResourceManager.GetString("CardinalityMismatch_NoExports", resourceCulture);

	internal static string CardinalityMismatch_TooManyExports => ResourceManager.GetString("CardinalityMismatch_TooManyExports", resourceCulture);

	internal static string CatalogMutation_Invalid => ResourceManager.GetString("CatalogMutation_Invalid", resourceCulture);

	internal static string CompositionElement_UnknownOrigin => ResourceManager.GetString("CompositionElement_UnknownOrigin", resourceCulture);

	internal static string CompositionException_ChangesRejected => ResourceManager.GetString("CompositionException_ChangesRejected", resourceCulture);

	internal static string CompositionException_ElementPrefix => ResourceManager.GetString("CompositionException_ElementPrefix", resourceCulture);

	internal static string CompositionException_ErrorPrefix => ResourceManager.GetString("CompositionException_ErrorPrefix", resourceCulture);

	internal static string CompositionException_MetadataViewInvalidConstructor => ResourceManager.GetString("CompositionException_MetadataViewInvalidConstructor", resourceCulture);

	internal static string CompositionException_MultipleErrorsWithMultiplePaths => ResourceManager.GetString("CompositionException_MultipleErrorsWithMultiplePaths", resourceCulture);

	internal static string CompositionException_OriginFormat => ResourceManager.GetString("CompositionException_OriginFormat", resourceCulture);

	internal static string CompositionException_OriginSeparator => ResourceManager.GetString("CompositionException_OriginSeparator", resourceCulture);

	internal static string CompositionException_PathsCountSeparator => ResourceManager.GetString("CompositionException_PathsCountSeparator", resourceCulture);

	internal static string CompositionException_ReviewErrorProperty => ResourceManager.GetString("CompositionException_ReviewErrorProperty", resourceCulture);

	internal static string CompositionException_SingleErrorWithMultiplePaths => ResourceManager.GetString("CompositionException_SingleErrorWithMultiplePaths", resourceCulture);

	internal static string CompositionException_SingleErrorWithSinglePath => ResourceManager.GetString("CompositionException_SingleErrorWithSinglePath", resourceCulture);

	internal static string CompositionTrace_Discovery_AssemblyLoadFailed => ResourceManager.GetString("CompositionTrace_Discovery_AssemblyLoadFailed", resourceCulture);

	internal static string CompositionTrace_Discovery_DefinitionContainsNoExports => ResourceManager.GetString("CompositionTrace_Discovery_DefinitionContainsNoExports", resourceCulture);

	internal static string CompositionTrace_Discovery_DefinitionMarkedWithPartNotDiscoverableAttribute => ResourceManager.GetString("CompositionTrace_Discovery_DefinitionMarkedWithPartNotDiscoverableAttribute", resourceCulture);

	internal static string CompositionTrace_Discovery_DefinitionMismatchedExportArity => ResourceManager.GetString("CompositionTrace_Discovery_DefinitionMismatchedExportArity", resourceCulture);

	internal static string CompositionTrace_Discovery_MemberMarkedWithMultipleImportAndImportMany => ResourceManager.GetString("CompositionTrace_Discovery_MemberMarkedWithMultipleImportAndImportMany", resourceCulture);

	internal static string CompositionTrace_Rejection_DefinitionRejected => ResourceManager.GetString("CompositionTrace_Rejection_DefinitionRejected", resourceCulture);

	internal static string CompositionTrace_Rejection_DefinitionResurrected => ResourceManager.GetString("CompositionTrace_Rejection_DefinitionResurrected", resourceCulture);

	internal static string ContractMismatch_ExportedValueCannotBeCastToT => ResourceManager.GetString("ContractMismatch_ExportedValueCannotBeCastToT", resourceCulture);

	internal static string ContractMismatch_InvalidCastOnMetadataField => ResourceManager.GetString("ContractMismatch_InvalidCastOnMetadataField", resourceCulture);

	internal static string ContractMismatch_MetadataViewImplementationCanNotBeNull => ResourceManager.GetString("ContractMismatch_MetadataViewImplementationCanNotBeNull", resourceCulture);

	internal static string ContractMismatch_MetadataViewImplementationDoesNotImplementViewInterface => ResourceManager.GetString("ContractMismatch_MetadataViewImplementationDoesNotImplementViewInterface", resourceCulture);

	internal static string ContractMismatch_NullReferenceOnMetadataField => ResourceManager.GetString("ContractMismatch_NullReferenceOnMetadataField", resourceCulture);

	internal static string DirectoryNotFound => ResourceManager.GetString("DirectoryNotFound", resourceCulture);

	internal static string Discovery_DuplicateMetadataNameValues => ResourceManager.GetString("Discovery_DuplicateMetadataNameValues", resourceCulture);

	internal static string Discovery_MetadataContainsValueWithInvalidType => ResourceManager.GetString("Discovery_MetadataContainsValueWithInvalidType", resourceCulture);

	internal static string Discovery_ReservedMetadataNameUsed => ResourceManager.GetString("Discovery_ReservedMetadataNameUsed", resourceCulture);

	internal static string ExportDefinitionNotOnThisComposablePart => ResourceManager.GetString("ExportDefinitionNotOnThisComposablePart", resourceCulture);

	internal static string ExportFactory_TooManyGenericParameters => ResourceManager.GetString("ExportFactory_TooManyGenericParameters", resourceCulture);

	internal static string ExportNotValidOnIndexers => ResourceManager.GetString("ExportNotValidOnIndexers", resourceCulture);

	internal static string ImportDefinitionNotOnThisComposablePart => ResourceManager.GetString("ImportDefinitionNotOnThisComposablePart", resourceCulture);

	internal static string ImportEngine_ComposeTookTooManyIterations => ResourceManager.GetString("ImportEngine_ComposeTookTooManyIterations", resourceCulture);

	internal static string ImportEngine_InvalidStateForRecomposition => ResourceManager.GetString("ImportEngine_InvalidStateForRecomposition", resourceCulture);

	internal static string ImportEngine_PartCannotActivate => ResourceManager.GetString("ImportEngine_PartCannotActivate", resourceCulture);

	internal static string ImportEngine_PartCannotGetExportedValue => ResourceManager.GetString("ImportEngine_PartCannotGetExportedValue", resourceCulture);

	internal static string ImportEngine_PartCannotSetImport => ResourceManager.GetString("ImportEngine_PartCannotSetImport", resourceCulture);

	internal static string ImportEngine_PartCycle => ResourceManager.GetString("ImportEngine_PartCycle", resourceCulture);

	internal static string ImportEngine_PreventedByExistingImport => ResourceManager.GetString("ImportEngine_PreventedByExistingImport", resourceCulture);

	internal static string ImportNotSetOnPart => ResourceManager.GetString("ImportNotSetOnPart", resourceCulture);

	internal static string ImportNotValidOnIndexers => ResourceManager.GetString("ImportNotValidOnIndexers", resourceCulture);

	internal static string InternalExceptionMessage => ResourceManager.GetString("InternalExceptionMessage", resourceCulture);

	internal static string InvalidArgument_ReflectionContext => ResourceManager.GetString("InvalidArgument_ReflectionContext", resourceCulture);

	internal static string InvalidMetadataValue => ResourceManager.GetString("InvalidMetadataValue", resourceCulture);

	internal static string InvalidMetadataView => ResourceManager.GetString("InvalidMetadataView", resourceCulture);

	internal static string InvalidOperation_DefinitionCannotBeRecomposed => ResourceManager.GetString("InvalidOperation_DefinitionCannotBeRecomposed", resourceCulture);

	internal static string InvalidOperation_GetExportedValueBeforePrereqImportSet => ResourceManager.GetString("InvalidOperation_GetExportedValueBeforePrereqImportSet", resourceCulture);

	internal static string InvalidOperationReentrantCompose => ResourceManager.GetString("InvalidOperationReentrantCompose", resourceCulture);

	internal static string InvalidPartCreationPolicyOnImport => ResourceManager.GetString("InvalidPartCreationPolicyOnImport", resourceCulture);

	internal static string InvalidPartCreationPolicyOnPart => ResourceManager.GetString("InvalidPartCreationPolicyOnPart", resourceCulture);

	internal static string InvalidSetterOnMetadataField => ResourceManager.GetString("InvalidSetterOnMetadataField", resourceCulture);

	internal static string LazyMemberInfo_AccessorsNull => ResourceManager.GetString("LazyMemberInfo_AccessorsNull", resourceCulture);

	internal static string LazyMemberInfo_InvalidAccessorOnSimpleMember => ResourceManager.GetString("LazyMemberInfo_InvalidAccessorOnSimpleMember", resourceCulture);

	internal static string LazyMemberinfo_InvalidEventAccessors_AccessorType => ResourceManager.GetString("LazyMemberinfo_InvalidEventAccessors_AccessorType", resourceCulture);

	internal static string LazyMemberInfo_InvalidEventAccessors_Cardinality => ResourceManager.GetString("LazyMemberInfo_InvalidEventAccessors_Cardinality", resourceCulture);

	internal static string LazyMemberinfo_InvalidPropertyAccessors_AccessorType => ResourceManager.GetString("LazyMemberinfo_InvalidPropertyAccessors_AccessorType", resourceCulture);

	internal static string LazyMemberInfo_InvalidPropertyAccessors_Cardinality => ResourceManager.GetString("LazyMemberInfo_InvalidPropertyAccessors_Cardinality", resourceCulture);

	internal static string LazyMemberInfo_NoAccessors => ResourceManager.GetString("LazyMemberInfo_NoAccessors", resourceCulture);

	internal static string LazyServices_LazyResolvesToNull => ResourceManager.GetString("LazyServices_LazyResolvesToNull", resourceCulture);

	internal static string MetadataItemNotSupported => ResourceManager.GetString("MetadataItemNotSupported", resourceCulture);

	internal static string NotImplemented_NotOverriddenByDerived => ResourceManager.GetString("NotImplemented_NotOverriddenByDerived", resourceCulture);

	internal static string NotSupportedCatalogChanges => ResourceManager.GetString("NotSupportedCatalogChanges", resourceCulture);

	internal static string NotSupportedInterfaceMetadataView => ResourceManager.GetString("NotSupportedInterfaceMetadataView", resourceCulture);

	internal static string NotSupportedReadOnlyDictionary => ResourceManager.GetString("NotSupportedReadOnlyDictionary", resourceCulture);

	internal static string ObjectAlreadyInitialized => ResourceManager.GetString("ObjectAlreadyInitialized", resourceCulture);

	internal static string ObjectMustBeInitialized => ResourceManager.GetString("ObjectMustBeInitialized", resourceCulture);

	internal static string ReentrantCompose => ResourceManager.GetString("ReentrantCompose", resourceCulture);

	internal static string ReflectionContext_Requires_DefaultConstructor => ResourceManager.GetString("ReflectionContext_Requires_DefaultConstructor", resourceCulture);

	internal static string ReflectionContext_Type_Required => ResourceManager.GetString("ReflectionContext_Type_Required", resourceCulture);

	internal static string ReflectionModel_ExportNotReadable => ResourceManager.GetString("ReflectionModel_ExportNotReadable", resourceCulture);

	internal static string ReflectionModel_ExportThrewException => ResourceManager.GetString("ReflectionModel_ExportThrewException", resourceCulture);

	internal static string ReflectionModel_ImportCollectionAddThrewException => ResourceManager.GetString("ReflectionModel_ImportCollectionAddThrewException", resourceCulture);

	internal static string ReflectionModel_ImportCollectionClearThrewException => ResourceManager.GetString("ReflectionModel_ImportCollectionClearThrewException", resourceCulture);

	internal static string ReflectionModel_ImportCollectionConstructionThrewException => ResourceManager.GetString("ReflectionModel_ImportCollectionConstructionThrewException", resourceCulture);

	internal static string ReflectionModel_ImportCollectionGetThrewException => ResourceManager.GetString("ReflectionModel_ImportCollectionGetThrewException", resourceCulture);

	internal static string ReflectionModel_ImportCollectionIsReadOnlyThrewException => ResourceManager.GetString("ReflectionModel_ImportCollectionIsReadOnlyThrewException", resourceCulture);

	internal static string ReflectionModel_ImportCollectionNotWritable => ResourceManager.GetString("ReflectionModel_ImportCollectionNotWritable", resourceCulture);

	internal static string ReflectionModel_ImportCollectionNull => ResourceManager.GetString("ReflectionModel_ImportCollectionNull", resourceCulture);

	internal static string ReflectionModel_ImportManyOnParameterCanOnlyBeAssigned => ResourceManager.GetString("ReflectionModel_ImportManyOnParameterCanOnlyBeAssigned", resourceCulture);

	internal static string ReflectionModel_ImportNotAssignableFromExport => ResourceManager.GetString("ReflectionModel_ImportNotAssignableFromExport", resourceCulture);

	internal static string ReflectionModel_ImportNotWritable => ResourceManager.GetString("ReflectionModel_ImportNotWritable", resourceCulture);

	internal static string ReflectionModel_ImportThrewException => ResourceManager.GetString("ReflectionModel_ImportThrewException", resourceCulture);

	internal static string ReflectionModel_InvalidExportDefinition => ResourceManager.GetString("ReflectionModel_InvalidExportDefinition", resourceCulture);

	internal static string ReflectionModel_InvalidImportDefinition => ResourceManager.GetString("ReflectionModel_InvalidImportDefinition", resourceCulture);

	internal static string ReflectionModel_InvalidMemberImportDefinition => ResourceManager.GetString("ReflectionModel_InvalidMemberImportDefinition", resourceCulture);

	internal static string ReflectionModel_InvalidParameterImportDefinition => ResourceManager.GetString("ReflectionModel_InvalidParameterImportDefinition", resourceCulture);

	internal static string ReflectionModel_InvalidPartDefinition => ResourceManager.GetString("ReflectionModel_InvalidPartDefinition", resourceCulture);

	internal static string ReflectionModel_PartConstructorMissing => ResourceManager.GetString("ReflectionModel_PartConstructorMissing", resourceCulture);

	internal static string ReflectionModel_PartConstructorThrewException => ResourceManager.GetString("ReflectionModel_PartConstructorThrewException", resourceCulture);

	internal static string ReflectionModel_PartOnImportsSatisfiedThrewException => ResourceManager.GetString("ReflectionModel_PartOnImportsSatisfiedThrewException", resourceCulture);

	internal static string TypeCatalog_DisplayNameFormat => ResourceManager.GetString("TypeCatalog_DisplayNameFormat", resourceCulture);

	internal static string TypeCatalog_Empty => ResourceManager.GetString("TypeCatalog_Empty", resourceCulture);

	internal Strings()
	{
	}
}
