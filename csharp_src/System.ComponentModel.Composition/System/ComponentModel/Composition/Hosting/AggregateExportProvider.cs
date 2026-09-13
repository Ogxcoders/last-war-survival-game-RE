using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting;

public class AggregateExportProvider : ExportProvider, IDisposable
{
	private readonly ReadOnlyCollection<ExportProvider> _readOnlyProviders;

	private readonly ExportProvider[] _providers;

	private volatile int _isDisposed;

	public ReadOnlyCollection<ExportProvider> Providers
	{
		get
		{
			ThrowIfDisposed();
			return _readOnlyProviders;
		}
	}

	public AggregateExportProvider(params ExportProvider[] providers)
	{
		ExportProvider[] array = null;
		if (providers != null)
		{
			array = new ExportProvider[providers.Length];
			for (int i = 0; i < providers.Length; i++)
			{
				ExportProvider exportProvider = providers[i];
				if (exportProvider == null)
				{
					throw ExceptionBuilder.CreateContainsNullElement("providers");
				}
				array[i] = exportProvider;
				exportProvider.ExportsChanged += OnExportChangedInternal;
				exportProvider.ExportsChanging += OnExportChangingInternal;
			}
		}
		else
		{
			array = new ExportProvider[0];
		}
		_providers = array;
		_readOnlyProviders = new ReadOnlyCollection<ExportProvider>(_providers);
	}

	public AggregateExportProvider(IEnumerable<ExportProvider> providers)
		: this(providers?.AsArray())
	{
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 0)
		{
			ExportProvider[] providers = _providers;
			foreach (ExportProvider obj in providers)
			{
				obj.ExportsChanged -= OnExportChangedInternal;
				obj.ExportsChanging -= OnExportChangingInternal;
			}
		}
	}

	protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
	{
		ThrowIfDisposed();
		ExportProvider[] providers;
		if (definition.Cardinality == ImportCardinality.ZeroOrMore)
		{
			List<Export> list = new List<Export>();
			providers = _providers;
			for (int i = 0; i < providers.Length; i++)
			{
				foreach (Export export in providers[i].GetExports(definition, atomicComposition))
				{
					list.Add(export);
				}
			}
			return list;
		}
		IEnumerable<Export> enumerable = null;
		providers = _providers;
		for (int i = 0; i < providers.Length; i++)
		{
			IEnumerable<Export> exports;
			bool num = providers[i].TryGetExports(definition, atomicComposition, out exports);
			bool flag = exports.FastAny();
			if (num && flag)
			{
				return exports;
			}
			if (flag)
			{
				enumerable = ((enumerable != null) ? enumerable.Concat(exports) : exports);
			}
		}
		return enumerable;
	}

	private void OnExportChangedInternal(object sender, ExportsChangeEventArgs e)
	{
		OnExportsChanged(e);
	}

	private void OnExportChangingInternal(object sender, ExportsChangeEventArgs e)
	{
		OnExportsChanging(e);
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
