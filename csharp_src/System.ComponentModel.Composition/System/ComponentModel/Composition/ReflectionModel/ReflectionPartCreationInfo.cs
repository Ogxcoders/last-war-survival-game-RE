using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionPartCreationInfo : IReflectionPartCreationInfo, ICompositionElement
{
	private readonly Lazy<Type> _partType;

	private readonly Lazy<IEnumerable<ImportDefinition>> _imports;

	private readonly Lazy<IEnumerable<ExportDefinition>> _exports;

	private readonly Lazy<IDictionary<string, object>> _metadata;

	private readonly ICompositionElement _origin;

	private ConstructorInfo _constructor;

	private bool _isDisposalRequired;

	public bool IsDisposalRequired => _isDisposalRequired;

	public string DisplayName => GetPartType().GetDisplayName();

	public ICompositionElement Origin => _origin;

	public ReflectionPartCreationInfo(Lazy<Type> partType, bool isDisposalRequired, Lazy<IEnumerable<ImportDefinition>> imports, Lazy<IEnumerable<ExportDefinition>> exports, Lazy<IDictionary<string, object>> metadata, ICompositionElement origin)
	{
		Assumes.NotNull(partType);
		_partType = partType;
		_isDisposalRequired = isDisposalRequired;
		_imports = imports;
		_exports = exports;
		_metadata = metadata;
		_origin = origin;
	}

	public Type GetPartType()
	{
		return _partType.GetNotNullValue("type");
	}

	public Lazy<Type> GetLazyPartType()
	{
		return _partType;
	}

	public ConstructorInfo GetConstructor()
	{
		if (_constructor == null)
		{
			ConstructorInfo[] array = null;
			array = (from parameterImport in GetImports().OfType<ReflectionParameterImportDefinition>()
				select parameterImport.ImportingLazyParameter.Value.Member).OfType<ConstructorInfo>().Distinct().ToArray();
			if (array.Length == 1)
			{
				_constructor = array[0];
			}
			else if (array.Length == 0)
			{
				_constructor = GetPartType().GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			}
		}
		return _constructor;
	}

	public IDictionary<string, object> GetMetadata()
	{
		if (_metadata == null)
		{
			return null;
		}
		return _metadata.Value;
	}

	public IEnumerable<ExportDefinition> GetExports()
	{
		if (_exports == null)
		{
			yield break;
		}
		IEnumerable<ExportDefinition> value = _exports.Value;
		if (value == null)
		{
			yield break;
		}
		foreach (ExportDefinition item in value)
		{
			if (!(item is ReflectionMemberExportDefinition reflectionMemberExportDefinition))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidExportDefinition, item.GetType()));
			}
			yield return reflectionMemberExportDefinition;
		}
	}

	public IEnumerable<ImportDefinition> GetImports()
	{
		if (_imports == null)
		{
			yield break;
		}
		IEnumerable<ImportDefinition> value = _imports.Value;
		if (value == null)
		{
			yield break;
		}
		foreach (ImportDefinition item in value)
		{
			if (!(item is ReflectionImportDefinition reflectionImportDefinition))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidMemberImportDefinition, item.GetType()));
			}
			yield return reflectionImportDefinition;
		}
	}
}
