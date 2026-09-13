using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal abstract class ImportingItem
{
	private readonly ContractBasedImportDefinition _definition;

	private readonly ImportType _importType;

	public ContractBasedImportDefinition Definition => _definition;

	public ImportType ImportType => _importType;

	protected ImportingItem(ContractBasedImportDefinition definition, ImportType importType)
	{
		Assumes.NotNull(definition);
		_definition = definition;
		_importType = importType;
	}

	public object CastExportsToImportType(Export[] exports)
	{
		if (Definition.Cardinality == ImportCardinality.ZeroOrMore)
		{
			return CastExportsToCollectionImportType(exports);
		}
		return CastExportsToSingleImportType(exports);
	}

	private object CastExportsToCollectionImportType(Export[] exports)
	{
		Assumes.NotNull(exports);
		Type type = ImportType.ElementType ?? typeof(object);
		Array array = Array.CreateInstance(type, exports.Length);
		for (int i = 0; i < array.Length; i++)
		{
			object value = CastSingleExportToImportType(type, exports[i]);
			array.SetValue(value, i);
		}
		return array;
	}

	private object CastExportsToSingleImportType(Export[] exports)
	{
		Assumes.NotNull(exports);
		Assumes.IsTrue(exports.Length < 2);
		if (exports.Length == 0)
		{
			return null;
		}
		return CastSingleExportToImportType(ImportType.ActualType, exports[0]);
	}

	private object CastSingleExportToImportType(Type type, Export export)
	{
		if (ImportType.CastExport != null)
		{
			return ImportType.CastExport(export);
		}
		return Cast(type, export);
	}

	private object Cast(Type type, Export export)
	{
		object value = export.Value;
		if (!ContractServices.TryCast(type, value, out var result))
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportNotAssignableFromExport, export.ToElement().DisplayName, type.FullName), Definition.ToElement());
		}
		return result;
	}
}
