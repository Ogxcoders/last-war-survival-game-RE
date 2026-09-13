using System.Collections.Generic;
using System.ComponentModel.Composition.AttributedModel;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition;

public static class AttributedModelServices
{
	public static TMetadataView GetMetadataView<TMetadataView>(IDictionary<string, object> metadata)
	{
		Requires.NotNull(metadata, "metadata");
		return MetadataViewProvider.GetMetadataView<TMetadataView>(metadata);
	}

	public static ComposablePart CreatePart(object attributedPart)
	{
		Requires.NotNull(attributedPart, "attributedPart");
		return AttributedModelDiscovery.CreatePart(attributedPart);
	}

	public static ComposablePart CreatePart(object attributedPart, ReflectionContext reflectionContext)
	{
		Requires.NotNull(attributedPart, "attributedPart");
		Requires.NotNull(reflectionContext, "reflectionContext");
		return AttributedModelDiscovery.CreatePart(attributedPart, reflectionContext);
	}

	public static ComposablePart CreatePart(ComposablePartDefinition partDefinition, object attributedPart)
	{
		Requires.NotNull(partDefinition, "partDefinition");
		Requires.NotNull(attributedPart, "attributedPart");
		return AttributedModelDiscovery.CreatePart((partDefinition as ReflectionComposablePartDefinition) ?? throw ExceptionBuilder.CreateReflectionModelInvalidPartDefinition("partDefinition", partDefinition.GetType()), attributedPart);
	}

	public static ComposablePartDefinition CreatePartDefinition(Type type, ICompositionElement origin)
	{
		Requires.NotNull(type, "type");
		return CreatePartDefinition(type, origin, ensureIsDiscoverable: false);
	}

	public static ComposablePartDefinition CreatePartDefinition(Type type, ICompositionElement origin, bool ensureIsDiscoverable)
	{
		Requires.NotNull(type, "type");
		if (ensureIsDiscoverable)
		{
			return AttributedModelDiscovery.CreatePartDefinitionIfDiscoverable(type, origin);
		}
		return AttributedModelDiscovery.CreatePartDefinition(type, null, ignoreConstructorImports: false, origin);
	}

	public static string GetTypeIdentity(Type type)
	{
		Requires.NotNull(type, "type");
		return ContractNameServices.GetTypeIdentity(type);
	}

	public static string GetTypeIdentity(MethodInfo method)
	{
		Requires.NotNull(method, "method");
		return ContractNameServices.GetTypeIdentityFromMethod(method);
	}

	public static string GetContractName(Type type)
	{
		Requires.NotNull(type, "type");
		return GetTypeIdentity(type);
	}

	public static ComposablePart AddExportedValue<T>(this CompositionBatch batch, T exportedValue)
	{
		Requires.NotNull(batch, "batch");
		string contractName = GetContractName(typeof(T));
		return batch.AddExportedValue(contractName, exportedValue);
	}

	public static void ComposeExportedValue<T>(this CompositionContainer container, T exportedValue)
	{
		Requires.NotNull(container, "container");
		CompositionBatch batch = new CompositionBatch();
		batch.AddExportedValue(exportedValue);
		container.Compose(batch);
	}

	public static ComposablePart AddExportedValue<T>(this CompositionBatch batch, string contractName, T exportedValue)
	{
		Requires.NotNull(batch, "batch");
		string typeIdentity = GetTypeIdentity(typeof(T));
		IDictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("ExportTypeIdentity", typeIdentity);
		return batch.AddExport(new Export(contractName, dictionary, () => exportedValue));
	}

	public static void ComposeExportedValue<T>(this CompositionContainer container, string contractName, T exportedValue)
	{
		Requires.NotNull(container, "container");
		CompositionBatch batch = new CompositionBatch();
		batch.AddExportedValue(contractName, exportedValue);
		container.Compose(batch);
	}

	public static ComposablePart AddPart(this CompositionBatch batch, object attributedPart)
	{
		Requires.NotNull(batch, "batch");
		Requires.NotNull(attributedPart, "attributedPart");
		ComposablePart composablePart = CreatePart(attributedPart);
		batch.AddPart(composablePart);
		return composablePart;
	}

	public static void ComposeParts(this CompositionContainer container, params object[] attributedParts)
	{
		Requires.NotNull(container, "container");
		Requires.NotNullOrNullElements(attributedParts, "attributedParts");
		CompositionBatch batch = new CompositionBatch(attributedParts.Select((object attributedPart) => CreatePart(attributedPart)).ToArray(), Enumerable.Empty<ComposablePart>());
		container.Compose(batch);
	}

	public static ComposablePart SatisfyImportsOnce(this ICompositionService compositionService, object attributedPart)
	{
		Requires.NotNull(compositionService, "compositionService");
		Requires.NotNull(attributedPart, "attributedPart");
		ComposablePart composablePart = CreatePart(attributedPart);
		compositionService.SatisfyImportsOnce(composablePart);
		return composablePart;
	}

	public static ComposablePart SatisfyImportsOnce(this ICompositionService compositionService, object attributedPart, ReflectionContext reflectionContext)
	{
		Requires.NotNull(compositionService, "compositionService");
		Requires.NotNull(attributedPart, "attributedPart");
		Requires.NotNull(reflectionContext, "reflectionContext");
		ComposablePart composablePart = CreatePart(attributedPart, reflectionContext);
		compositionService.SatisfyImportsOnce(composablePart);
		return composablePart;
	}

	public static bool Exports(this ComposablePartDefinition part, Type contractType)
	{
		Requires.NotNull(part, "part");
		Requires.NotNull(contractType, "contractType");
		return part.Exports(GetContractName(contractType));
	}

	public static bool Exports<T>(this ComposablePartDefinition part)
	{
		Requires.NotNull(part, "part");
		return part.Exports(typeof(T));
	}

	public static bool Imports(this ComposablePartDefinition part, Type contractType)
	{
		Requires.NotNull(part, "part");
		Requires.NotNull(contractType, "contractType");
		return part.Imports(GetContractName(contractType));
	}

	public static bool Imports<T>(this ComposablePartDefinition part)
	{
		Requires.NotNull(part, "part");
		return part.Imports(typeof(T));
	}

	public static bool Imports(this ComposablePartDefinition part, Type contractType, ImportCardinality importCardinality)
	{
		Requires.NotNull(part, "part");
		Requires.NotNull(contractType, "contractType");
		return part.Imports(GetContractName(contractType), importCardinality);
	}

	public static bool Imports<T>(this ComposablePartDefinition part, ImportCardinality importCardinality)
	{
		Requires.NotNull(part, "part");
		return part.Imports(typeof(T), importCardinality);
	}
}
