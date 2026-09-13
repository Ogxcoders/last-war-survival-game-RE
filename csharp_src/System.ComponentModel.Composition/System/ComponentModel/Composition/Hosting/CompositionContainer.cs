using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

public class CompositionContainer : ExportProvider, ICompositionService, IDisposable
{
	private class CompositionServiceShim : ICompositionService
	{
		private CompositionContainer _innerContainer;

		public CompositionServiceShim(CompositionContainer innerContainer)
		{
			Assumes.NotNull(innerContainer);
			_innerContainer = innerContainer;
		}

		void ICompositionService.SatisfyImportsOnce(ComposablePart part)
		{
			_innerContainer.SatisfyImportsOnce(part);
		}
	}

	private CompositionOptions _compositionOptions;

	private ImportEngine _importEngine;

	private ComposablePartExportProvider _partExportProvider;

	private ExportProvider _rootProvider;

	private CatalogExportProvider _catalogExportProvider;

	private AggregateExportProvider _localExportProvider;

	private AggregateExportProvider _ancestorExportProvider;

	private readonly ReadOnlyCollection<ExportProvider> _providers;

	private volatile bool _isDisposed;

	private object _lock = new object();

	private static ReadOnlyCollection<ExportProvider> EmptyProviders = new ReadOnlyCollection<ExportProvider>(new ExportProvider[0]);

	internal CompositionOptions CompositionOptions
	{
		get
		{
			ThrowIfDisposed();
			return _compositionOptions;
		}
	}

	public ComposablePartCatalog Catalog
	{
		get
		{
			ThrowIfDisposed();
			if (_catalogExportProvider == null)
			{
				return null;
			}
			return _catalogExportProvider.Catalog;
		}
	}

	internal CatalogExportProvider CatalogExportProvider
	{
		get
		{
			ThrowIfDisposed();
			return _catalogExportProvider;
		}
	}

	public ReadOnlyCollection<ExportProvider> Providers
	{
		get
		{
			ThrowIfDisposed();
			return _providers;
		}
	}

	public CompositionContainer()
		: this(null, Array.Empty<ExportProvider>())
	{
	}

	public CompositionContainer(params ExportProvider[] providers)
		: this(null, providers)
	{
	}

	public CompositionContainer(CompositionOptions compositionOptions, params ExportProvider[] providers)
		: this(null, compositionOptions, providers)
	{
	}

	public CompositionContainer(ComposablePartCatalog catalog, params ExportProvider[] providers)
		: this(catalog, isThreadSafe: false, providers)
	{
	}

	public CompositionContainer(ComposablePartCatalog catalog, bool isThreadSafe, params ExportProvider[] providers)
		: this(catalog, isThreadSafe ? CompositionOptions.IsThreadSafe : CompositionOptions.Default, providers)
	{
	}

	public CompositionContainer(ComposablePartCatalog catalog, CompositionOptions compositionOptions, params ExportProvider[] providers)
	{
		if (compositionOptions > (CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe | CompositionOptions.ExportCompositionService))
		{
			throw new ArgumentOutOfRangeException("compositionOptions");
		}
		_compositionOptions = compositionOptions;
		_partExportProvider = new ComposablePartExportProvider(compositionOptions);
		_partExportProvider.SourceProvider = this;
		if (catalog != null || providers.Length != 0)
		{
			if (catalog != null)
			{
				_catalogExportProvider = new CatalogExportProvider(catalog, compositionOptions);
				_catalogExportProvider.SourceProvider = this;
				_localExportProvider = new AggregateExportProvider(_partExportProvider, _catalogExportProvider);
			}
			else
			{
				_localExportProvider = new AggregateExportProvider(_partExportProvider);
			}
			if (providers != null && providers.Length != 0)
			{
				_ancestorExportProvider = new AggregateExportProvider(providers);
				_rootProvider = new AggregateExportProvider(_localExportProvider, _ancestorExportProvider);
			}
			else
			{
				_rootProvider = _localExportProvider;
			}
		}
		else
		{
			_rootProvider = _partExportProvider;
		}
		if (compositionOptions.HasFlag(CompositionOptions.ExportCompositionService))
		{
			this.ComposeExportedValue((ICompositionService)new CompositionServiceShim(this));
		}
		_rootProvider.ExportsChanged += OnExportsChangedInternal;
		_rootProvider.ExportsChanging += OnExportsChangingInternal;
		_providers = ((providers != null) ? new ReadOnlyCollection<ExportProvider>((ExportProvider[])providers.Clone()) : EmptyProviders);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposing || _isDisposed)
		{
			return;
		}
		ExportProvider exportProvider = null;
		AggregateExportProvider aggregateExportProvider = null;
		AggregateExportProvider aggregateExportProvider2 = null;
		ComposablePartExportProvider composablePartExportProvider = null;
		CatalogExportProvider catalogExportProvider = null;
		ImportEngine importEngine = null;
		lock (_lock)
		{
			if (!_isDisposed)
			{
				exportProvider = _rootProvider;
				_rootProvider = null;
				aggregateExportProvider2 = _localExportProvider;
				_localExportProvider = null;
				aggregateExportProvider = _ancestorExportProvider;
				_ancestorExportProvider = null;
				composablePartExportProvider = _partExportProvider;
				_partExportProvider = null;
				catalogExportProvider = _catalogExportProvider;
				_catalogExportProvider = null;
				importEngine = _importEngine;
				_importEngine = null;
				_isDisposed = true;
			}
		}
		if (exportProvider != null)
		{
			exportProvider.ExportsChanged -= OnExportsChangedInternal;
			exportProvider.ExportsChanging -= OnExportsChangingInternal;
		}
		aggregateExportProvider?.Dispose();
		aggregateExportProvider2?.Dispose();
		catalogExportProvider?.Dispose();
		composablePartExportProvider?.Dispose();
		importEngine?.Dispose();
	}

	public void Compose(CompositionBatch batch)
	{
		Requires.NotNull(batch, "batch");
		ThrowIfDisposed();
		_partExportProvider.Compose(batch);
	}

	public void ReleaseExport(Export export)
	{
		Requires.NotNull(export, "export");
		if (export is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

	public void ReleaseExport<T>(Lazy<T> export)
	{
		Requires.NotNull(export, "export");
		if (export is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

	public void ReleaseExports(IEnumerable<Export> exports)
	{
		Requires.NotNullOrNullElements(exports, "exports");
		foreach (Export export in exports)
		{
			ReleaseExport(export);
		}
	}

	public void ReleaseExports<T>(IEnumerable<Lazy<T>> exports)
	{
		Requires.NotNullOrNullElements(exports, "exports");
		foreach (Lazy<T> export in exports)
		{
			ReleaseExport(export);
		}
	}

	public void ReleaseExports<T, TMetadataView>(IEnumerable<Lazy<T, TMetadataView>> exports)
	{
		Requires.NotNullOrNullElements(exports, "exports");
		foreach (Lazy<T, TMetadataView> export in exports)
		{
			ReleaseExport(export);
		}
	}

	public void SatisfyImportsOnce(ComposablePart part)
	{
		ThrowIfDisposed();
		if (_importEngine == null)
		{
			ImportEngine importEngine = new ImportEngine(this, _compositionOptions);
			lock (_lock)
			{
				if (_importEngine == null)
				{
					Thread.MemoryBarrier();
					_importEngine = importEngine;
					importEngine = null;
				}
			}
			importEngine?.Dispose();
		}
		_importEngine.SatisfyImportsOnce(part);
	}

	internal void OnExportsChangedInternal(object sender, ExportsChangeEventArgs e)
	{
		OnExportsChanged(e);
	}

	internal void OnExportsChangingInternal(object sender, ExportsChangeEventArgs e)
	{
		OnExportsChanging(e);
	}

	protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
	{
		ThrowIfDisposed();
		IEnumerable<Export> exports = null;
		if (!definition.Metadata.TryGetValue("System.ComponentModel.Composition.ImportSource", out var value))
		{
			value = ImportSource.Any;
		}
		switch ((ImportSource)value)
		{
		case ImportSource.Any:
			Assumes.NotNull(_rootProvider);
			_rootProvider.TryGetExports(definition, atomicComposition, out exports);
			break;
		case ImportSource.Local:
			Assumes.NotNull(_localExportProvider);
			_localExportProvider.TryGetExports(definition.RemoveImportSource(), atomicComposition, out exports);
			break;
		case ImportSource.NonLocal:
			if (_ancestorExportProvider != null)
			{
				_ancestorExportProvider.TryGetExports(definition.RemoveImportSource(), atomicComposition, out exports);
			}
			break;
		}
		return exports;
	}

	[DebuggerStepThrough]
	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}
}
