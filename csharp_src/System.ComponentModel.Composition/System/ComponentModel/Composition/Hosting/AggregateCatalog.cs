using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

public class AggregateCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
{
	private ComposablePartCatalogCollection _catalogs;

	private volatile int _isDisposed;

	public ICollection<ComposablePartCatalog> Catalogs
	{
		get
		{
			ThrowIfDisposed();
			return _catalogs;
		}
	}

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed
	{
		add
		{
			_catalogs.Changed += value;
		}
		remove
		{
			_catalogs.Changed -= value;
		}
	}

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing
	{
		add
		{
			_catalogs.Changing += value;
		}
		remove
		{
			_catalogs.Changing -= value;
		}
	}

	public AggregateCatalog()
		: this((IEnumerable<ComposablePartCatalog>)null)
	{
	}

	public AggregateCatalog(params ComposablePartCatalog[] catalogs)
		: this((IEnumerable<ComposablePartCatalog>)catalogs)
	{
	}

	public AggregateCatalog(IEnumerable<ComposablePartCatalog> catalogs)
	{
		Requires.NullOrNotNullElements(catalogs, "catalogs");
		_catalogs = new ComposablePartCatalogCollection(catalogs, OnChanged, OnChanging);
	}

	public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		ThrowIfDisposed();
		Requires.NotNull(definition, "definition");
		List<Tuple<ComposablePartDefinition, ExportDefinition>> list = new List<Tuple<ComposablePartDefinition, ExportDefinition>>();
		foreach (ComposablePartCatalog catalog in _catalogs)
		{
			foreach (Tuple<ComposablePartDefinition, ExportDefinition> export in catalog.GetExports(definition))
			{
				list.Add(export);
			}
		}
		return list;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 0)
			{
				_catalogs.Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		return _catalogs.SelectMany((ComposablePartCatalog catalog) => catalog).GetEnumerator();
	}

	protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e)
	{
		_catalogs.OnChanged(this, e);
	}

	protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e)
	{
		_catalogs.OnChanging(this, e);
	}

	[DebuggerStepThrough]
	private void ThrowIfDisposed()
	{
		if (_isDisposed == 1)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}
}
