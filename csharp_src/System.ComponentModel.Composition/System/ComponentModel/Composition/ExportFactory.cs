using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition;

public class ExportFactory<T>
{
	private Func<Tuple<T, Action>> _exportLifetimeContextCreator;

	public ExportFactory(Func<Tuple<T, Action>> exportLifetimeContextCreator)
	{
		if (exportLifetimeContextCreator == null)
		{
			throw new ArgumentNullException("exportLifetimeContextCreator");
		}
		_exportLifetimeContextCreator = exportLifetimeContextCreator;
	}

	public ExportLifetimeContext<T> CreateExport()
	{
		Tuple<T, Action> tuple = _exportLifetimeContextCreator();
		return new ExportLifetimeContext<T>(tuple.Item1, tuple.Item2);
	}

	internal bool IncludeInScopedCatalog(ComposablePartDefinition composablePartDefinition)
	{
		return OnFilterScopedCatalog(composablePartDefinition);
	}

	protected virtual bool OnFilterScopedCatalog(ComposablePartDefinition composablePartDefinition)
	{
		return true;
	}
}
public class ExportFactory<T, TMetadata> : ExportFactory<T>
{
	private readonly TMetadata _metadata;

	public TMetadata Metadata => _metadata;

	public ExportFactory(Func<Tuple<T, Action>> exportLifetimeContextCreator, TMetadata metadata)
		: base(exportLifetimeContextCreator)
	{
		_metadata = metadata;
	}
}
