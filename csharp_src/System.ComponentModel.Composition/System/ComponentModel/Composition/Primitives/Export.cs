using System.Collections.Generic;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives;

public class Export
{
	private readonly ExportDefinition _definition;

	private readonly Func<object> _exportedValueGetter;

	private static readonly object _EmptyValue = new object();

	private volatile object _exportedValue = _EmptyValue;

	public virtual ExportDefinition Definition
	{
		get
		{
			if (_definition != null)
			{
				return _definition;
			}
			throw ExceptionBuilder.CreateNotOverriddenByDerived("Definition");
		}
	}

	public IDictionary<string, object> Metadata => Definition.Metadata;

	public object Value
	{
		get
		{
			if (_exportedValue == _EmptyValue)
			{
				object exportedValueCore = GetExportedValueCore();
				Interlocked.CompareExchange(ref _exportedValue, exportedValueCore, _EmptyValue);
			}
			return _exportedValue;
		}
	}

	protected Export()
	{
	}

	public Export(string contractName, Func<object> exportedValueGetter)
		: this(new ExportDefinition(contractName, null), exportedValueGetter)
	{
	}

	public Export(string contractName, IDictionary<string, object> metadata, Func<object> exportedValueGetter)
		: this(new ExportDefinition(contractName, metadata), exportedValueGetter)
	{
	}

	public Export(ExportDefinition definition, Func<object> exportedValueGetter)
	{
		Requires.NotNull(definition, "definition");
		Requires.NotNull(exportedValueGetter, "exportedValueGetter");
		_definition = definition;
		_exportedValueGetter = exportedValueGetter;
	}

	protected virtual object GetExportedValueCore()
	{
		if (_exportedValueGetter != null)
		{
			return _exportedValueGetter();
		}
		throw ExceptionBuilder.CreateNotOverriddenByDerived("GetExportedValueCore");
	}
}
