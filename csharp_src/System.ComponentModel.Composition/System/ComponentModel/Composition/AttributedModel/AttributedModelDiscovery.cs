using System.ComponentModel.Composition.Diagnostics;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Globalization;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.AttributedModel;

internal static class AttributedModelDiscovery
{
	public static ComposablePartDefinition CreatePartDefinitionIfDiscoverable(Type type, ICompositionElement origin)
	{
		AttributedPartCreationInfo attributedPartCreationInfo = new AttributedPartCreationInfo(type, null, ignoreConstructorImports: false, origin);
		if (!attributedPartCreationInfo.IsPartDiscoverable())
		{
			return null;
		}
		return new ReflectionComposablePartDefinition(attributedPartCreationInfo);
	}

	public static ReflectionComposablePartDefinition CreatePartDefinition(Type type, PartCreationPolicyAttribute partCreationPolicy, bool ignoreConstructorImports, ICompositionElement origin)
	{
		Assumes.NotNull(type);
		return new ReflectionComposablePartDefinition(new AttributedPartCreationInfo(type, partCreationPolicy, ignoreConstructorImports, origin));
	}

	public static ReflectionComposablePart CreatePart(object attributedPart)
	{
		Assumes.NotNull(attributedPart);
		return new ReflectionComposablePart(CreatePartDefinition(attributedPart.GetType(), PartCreationPolicyAttribute.Shared, ignoreConstructorImports: true, null), attributedPart);
	}

	public static ReflectionComposablePart CreatePart(object attributedPart, ReflectionContext reflectionContext)
	{
		Assumes.NotNull(attributedPart);
		Assumes.NotNull(reflectionContext);
		TypeInfo typeInfo = reflectionContext.MapType(attributedPart.GetType().GetTypeInfo());
		if (typeInfo.Assembly.ReflectionOnly)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Argument_ReflectionContextReturnsReflectionOnlyType, "reflectionContext"), "reflectionContext");
		}
		return CreatePart(CreatePartDefinition(typeInfo, PartCreationPolicyAttribute.Shared, ignoreConstructorImports: true, null), attributedPart);
	}

	public static ReflectionComposablePart CreatePart(ComposablePartDefinition partDefinition, object attributedPart)
	{
		Assumes.NotNull(partDefinition);
		Assumes.NotNull(attributedPart);
		return new ReflectionComposablePart((ReflectionComposablePartDefinition)partDefinition, attributedPart);
	}

	public static ReflectionParameterImportDefinition CreateParameterImportDefinition(ParameterInfo parameter, ICompositionElement origin)
	{
		Requires.NotNull(parameter, "parameter");
		ReflectionParameter reflectionParameter = parameter.ToReflectionParameter();
		IAttributedImport attributedImport = GetAttributedImport(reflectionParameter, parameter);
		ImportType importType = new ImportType(reflectionParameter.ReturnType, attributedImport.Cardinality);
		if (importType.IsPartCreator)
		{
			return new PartCreatorParameterImportDefinition(new Lazy<ParameterInfo>(() => parameter), origin, new ContractBasedImportDefinition(attributedImport.GetContractNameFromImport(importType), attributedImport.GetTypeIdentityFromImport(importType), CompositionServices.GetRequiredMetadata(importType.MetadataViewType), attributedImport.Cardinality, isRecomposable: false, isPrerequisite: true, (attributedImport.RequiredCreationPolicy != CreationPolicy.NewScope) ? CreationPolicy.NonShared : CreationPolicy.NewScope, CompositionServices.GetImportMetadata(importType, attributedImport)));
		}
		if (attributedImport.RequiredCreationPolicy == CreationPolicy.NewScope)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidPartCreationPolicyOnImport, attributedImport.RequiredCreationPolicy), origin);
		}
		return new ReflectionParameterImportDefinition(new Lazy<ParameterInfo>(() => parameter), attributedImport.GetContractNameFromImport(importType), attributedImport.GetTypeIdentityFromImport(importType), CompositionServices.GetRequiredMetadata(importType.MetadataViewType), attributedImport.Cardinality, attributedImport.RequiredCreationPolicy, CompositionServices.GetImportMetadata(importType, attributedImport), origin);
	}

	public static ReflectionMemberImportDefinition CreateMemberImportDefinition(MemberInfo member, ICompositionElement origin)
	{
		Requires.NotNull(member, "member");
		ReflectionWritableMember reflectionWritableMember = member.ToReflectionWritableMember();
		IAttributedImport attributedImport = GetAttributedImport(reflectionWritableMember, member);
		ImportType importType = new ImportType(reflectionWritableMember.ReturnType, attributedImport.Cardinality);
		if (importType.IsPartCreator)
		{
			return new PartCreatorMemberImportDefinition(new LazyMemberInfo(member), origin, new ContractBasedImportDefinition(attributedImport.GetContractNameFromImport(importType), attributedImport.GetTypeIdentityFromImport(importType), CompositionServices.GetRequiredMetadata(importType.MetadataViewType), attributedImport.Cardinality, attributedImport.AllowRecomposition, isPrerequisite: false, (attributedImport.RequiredCreationPolicy != CreationPolicy.NewScope) ? CreationPolicy.NonShared : CreationPolicy.NewScope, CompositionServices.GetImportMetadata(importType, attributedImport)));
		}
		if (attributedImport.RequiredCreationPolicy == CreationPolicy.NewScope)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidPartCreationPolicyOnImport, attributedImport.RequiredCreationPolicy), origin);
		}
		bool isPrerequisite = member.GetAttributes<ExportAttribute>().Length != 0;
		return new ReflectionMemberImportDefinition(new LazyMemberInfo(member), attributedImport.GetContractNameFromImport(importType), attributedImport.GetTypeIdentityFromImport(importType), CompositionServices.GetRequiredMetadata(importType.MetadataViewType), attributedImport.Cardinality, attributedImport.AllowRecomposition, isPrerequisite, attributedImport.RequiredCreationPolicy, CompositionServices.GetImportMetadata(importType, attributedImport), origin);
	}

	private static IAttributedImport GetAttributedImport(ReflectionItem item, ICustomAttributeProvider attributeProvider)
	{
		IAttributedImport[] attributes = attributeProvider.GetAttributes<IAttributedImport>(inherit: false);
		if (attributes.Length == 0)
		{
			return new ImportAttribute();
		}
		if (attributes.Length > 1)
		{
			CompositionTrace.MemberMarkedWithMultipleImportAndImportMany(item);
		}
		return attributes[0];
	}
}
