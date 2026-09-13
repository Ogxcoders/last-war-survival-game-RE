using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

public class ComposablePartExportProvider : ExportProvider, IDisposable
{
	private List<ComposablePart> _parts = new List<ComposablePart>();

	private volatile bool _isDisposed;

	private volatile bool _isRunning;

	private CompositionLock _lock;

	private ExportProvider _sourceProvider;

	private ImportEngine _importEngine;

	private volatile bool _currentlyComposing;

	private CompositionOptions _compositionOptions;

	public ExportProvider SourceProvider
	{
		get
		{
			ThrowIfDisposed();
			return _sourceProvider;
		}
		set
		{
			ThrowIfDisposed();
			Requires.NotNull(value, "value");
			using (_lock.LockStateForWrite())
			{
				EnsureCanSet(_sourceProvider);
				_sourceProvider = value;
			}
		}
	}

	private ImportEngine ImportEngine
	{
		get
		{
			if (_importEngine == null)
			{
				Assumes.NotNull(_sourceProvider);
				ImportEngine importEngine = new ImportEngine(_sourceProvider, _compositionOptions);
				using (_lock.LockStateForWrite())
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
			return _importEngine;
		}
	}

	public ComposablePartExportProvider()
		: this(isThreadSafe: false)
	{
	}

	public ComposablePartExportProvider(bool isThreadSafe)
		: this(isThreadSafe ? CompositionOptions.IsThreadSafe : CompositionOptions.Default)
	{
	}

	public ComposablePartExportProvider(CompositionOptions compositionOptions)
	{
		if (compositionOptions > (CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe | CompositionOptions.ExportCompositionService))
		{
			throw new ArgumentOutOfRangeException("compositionOptions");
		}
		_compositionOptions = compositionOptions;
		_lock = new CompositionLock(compositionOptions.HasFlag(CompositionOptions.IsThreadSafe));
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
		bool flag = false;
		ImportEngine importEngine = null;
		try
		{
			using (_lock.LockStateForWrite())
			{
				if (!_isDisposed)
				{
					importEngine = _importEngine;
					_importEngine = null;
					_sourceProvider = null;
					_isDisposed = true;
					flag = true;
				}
			}
		}
		finally
		{
			importEngine?.Dispose();
			if (flag)
			{
				_lock.Dispose();
			}
		}
	}

	protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
	{
		ThrowIfDisposed();
		EnsureRunning();
		List<ComposablePart> list = null;
		using (_lock.LockStateForRead())
		{
			list = atomicComposition.GetValueAllowNull(this, _parts);
		}
		if (list.Count == 0)
		{
			return null;
		}
		List<Export> list2 = new List<Export>();
		foreach (ComposablePart item in list)
		{
			foreach (ExportDefinition exportDefinition in item.ExportDefinitions)
			{
				if (definition.IsConstraintSatisfiedBy(exportDefinition))
				{
					list2.Add(CreateExport(item, exportDefinition));
				}
			}
		}
		return list2;
	}

	public void Compose(CompositionBatch batch)
	{
		ThrowIfDisposed();
		EnsureRunning();
		Requires.NotNull(batch, "batch");
		if (batch.PartsToAdd.Count == 0 && batch.PartsToRemove.Count == 0)
		{
			return;
		}
		CompositionResult compositionResult = CompositionResult.SucceededResult;
		List<ComposablePart> updatedPartsList = GetUpdatedPartsList(ref batch);
		using (AtomicComposition atomicComposition = new AtomicComposition())
		{
			if (_currentlyComposing)
			{
				throw new InvalidOperationException(Strings.ReentrantCompose);
			}
			_currentlyComposing = true;
			try
			{
				atomicComposition.SetValue(this, updatedPartsList);
				Recompose(batch, atomicComposition);
				foreach (ComposablePart item in batch.PartsToAdd)
				{
					try
					{
						ImportEngine.PreviewImports(item, atomicComposition);
					}
					catch (ChangeRejectedException ex)
					{
						compositionResult = compositionResult.MergeResult(new CompositionResult(ex.Errors));
					}
				}
				compositionResult.ThrowOnErrors(atomicComposition);
				using (_lock.LockStateForWrite())
				{
					_parts = updatedPartsList;
				}
				atomicComposition.Complete();
			}
			finally
			{
				_currentlyComposing = false;
			}
		}
		foreach (ComposablePart part in batch.PartsToAdd)
		{
			compositionResult = compositionResult.MergeResult(CompositionServices.TryInvoke(delegate
			{
				ImportEngine.SatisfyImports(part);
			}));
		}
		compositionResult.ThrowOnErrors();
	}

	private List<ComposablePart> GetUpdatedPartsList(ref CompositionBatch batch)
	{
		Assumes.NotNull(batch);
		List<ComposablePart> list = null;
		using (_lock.LockStateForRead())
		{
			list = _parts.ToList();
		}
		foreach (ComposablePart item in batch.PartsToAdd)
		{
			list.Add(item);
		}
		List<ComposablePart> list2 = null;
		foreach (ComposablePart item2 in batch.PartsToRemove)
		{
			if (list.Remove(item2))
			{
				if (list2 == null)
				{
					list2 = new List<ComposablePart>();
				}
				list2.Add(item2);
			}
		}
		batch = new CompositionBatch(batch.PartsToAdd, list2);
		return list;
	}

	private void Recompose(CompositionBatch batch, AtomicComposition atomicComposition)
	{
		Assumes.NotNull(batch);
		foreach (ComposablePart item in batch.PartsToRemove)
		{
			ImportEngine.ReleaseImports(item, atomicComposition);
		}
		IEnumerable<ExportDefinition> addedExports = ((batch.PartsToAdd.Count != 0) ? batch.PartsToAdd.SelectMany((ComposablePart part) => part.ExportDefinitions).ToArray() : new ExportDefinition[0]);
		IEnumerable<ExportDefinition> removedExports = ((batch.PartsToRemove.Count != 0) ? batch.PartsToRemove.SelectMany((ComposablePart part) => part.ExportDefinitions).ToArray() : new ExportDefinition[0]);
		OnExportsChanging(new ExportsChangeEventArgs(addedExports, removedExports, atomicComposition));
		atomicComposition.AddCompleteAction(delegate
		{
			OnExportsChanged(new ExportsChangeEventArgs(addedExports, removedExports, null));
		});
	}

	private Export CreateExport(ComposablePart part, ExportDefinition export)
	{
		return new Export(export, () => GetExportedValue(part, export));
	}

	private object GetExportedValue(ComposablePart part, ExportDefinition export)
	{
		ThrowIfDisposed();
		EnsureRunning();
		return CompositionServices.GetExportedValueFromComposedPart(ImportEngine, part, export);
	}

	[DebuggerStepThrough]
	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
	}

	[DebuggerStepThrough]
	private void EnsureCanRun()
	{
		if (_sourceProvider == null)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.ObjectMustBeInitialized, "SourceProvider"));
		}
	}

	[DebuggerStepThrough]
	private void EnsureRunning()
	{
		if (_isRunning)
		{
			return;
		}
		using (_lock.LockStateForWrite())
		{
			if (!_isRunning)
			{
				EnsureCanRun();
				_isRunning = true;
			}
		}
	}

	[DebuggerStepThrough]
	private void EnsureCanSet<T>(T currentValue) where T : class
	{
		if (_isRunning || currentValue != null)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.ObjectAlreadyInitialized));
		}
	}
}
