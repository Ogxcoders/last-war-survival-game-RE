using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting;

public class ImportEngine : ICompositionService, IDisposable
{
	private class EngineContext
	{
		private ImportEngine _importEngine;

		private List<PartManager> _addedPartManagers = new List<PartManager>();

		private List<PartManager> _removedPartManagers = new List<PartManager>();

		private EngineContext _parentEngineContext;

		public EngineContext(ImportEngine importEngine, EngineContext parentEngineContext)
		{
			_importEngine = importEngine;
			_parentEngineContext = parentEngineContext;
		}

		public void AddPartManager(PartManager part)
		{
			Assumes.NotNull(part);
			if (!_removedPartManagers.Remove(part))
			{
				_addedPartManagers.Add(part);
			}
		}

		public void RemovePartManager(PartManager part)
		{
			Assumes.NotNull(part);
			if (!_addedPartManagers.Remove(part))
			{
				_removedPartManagers.Add(part);
			}
		}

		public IEnumerable<PartManager> GetAddedPartManagers()
		{
			if (_parentEngineContext != null)
			{
				return _addedPartManagers.ConcatAllowingNull(_parentEngineContext.GetAddedPartManagers());
			}
			return _addedPartManagers;
		}

		public IEnumerable<PartManager> GetRemovedPartManagers()
		{
			if (_parentEngineContext != null)
			{
				return _removedPartManagers.ConcatAllowingNull(_parentEngineContext.GetRemovedPartManagers());
			}
			return _removedPartManagers;
		}

		public void Complete()
		{
			foreach (PartManager addedPartManager in _addedPartManagers)
			{
				_importEngine.StartSatisfyingImports(addedPartManager, null);
			}
			foreach (PartManager removedPartManager in _removedPartManagers)
			{
				_importEngine.StopSatisfyingImports(removedPartManager, null);
			}
		}
	}

	private class PartManager
	{
		private Dictionary<ImportDefinition, List<IDisposable>> _importedDisposableExports;

		private Dictionary<ImportDefinition, Export[]> _importCache;

		private string[] _importedContractNames;

		private ComposablePart _part;

		private ImportState _state;

		private readonly ImportEngine _importEngine;

		public ComposablePart Part => _part;

		public ImportState State
		{
			get
			{
				using (_importEngine._lock.LockStateForRead())
				{
					return _state;
				}
			}
			set
			{
				using (_importEngine._lock.LockStateForWrite())
				{
					_state = value;
				}
			}
		}

		public bool TrackingImports { get; set; }

		public PartManager(ImportEngine importEngine, ComposablePart part)
		{
			_importEngine = importEngine;
			_part = part;
		}

		public IEnumerable<string> GetImportedContractNames()
		{
			if (Part == null)
			{
				return Enumerable.Empty<string>();
			}
			if (_importedContractNames == null)
			{
				_importedContractNames = Part.ImportDefinitions.Select((ImportDefinition import) => import.ContractName ?? ImportDefinition.EmptyContractName).Distinct().ToArray();
			}
			return _importedContractNames;
		}

		public CompositionResult TrySetImport(ImportDefinition import, IEnumerable<Export> exports)
		{
			try
			{
				Part.SetImport(import, exports);
				UpdateDisposableDependencies(import, exports);
				return CompositionResult.SucceededResult;
			}
			catch (CompositionException innerException)
			{
				return new CompositionResult(ErrorBuilder.CreatePartCannotSetImport(Part, import, innerException));
			}
			catch (ComposablePartException innerException2)
			{
				return new CompositionResult(ErrorBuilder.CreatePartCannotSetImport(Part, import, innerException2));
			}
		}

		public void SetSavedImport(ImportDefinition import, Export[] exports, AtomicComposition atomicComposition)
		{
			if (atomicComposition != null)
			{
				Export[] savedExports = GetSavedImport(import);
				atomicComposition.AddRevertAction(delegate
				{
					SetSavedImport(import, savedExports, null);
				});
			}
			if (_importCache == null)
			{
				_importCache = new Dictionary<ImportDefinition, Export[]>();
			}
			_importCache[import] = exports;
		}

		public Export[] GetSavedImport(ImportDefinition import)
		{
			Export[] value = null;
			if (_importCache != null)
			{
				_importCache.TryGetValue(import, out value);
			}
			return value;
		}

		public void ClearSavedImports()
		{
			_importCache = null;
		}

		public CompositionResult TryOnComposed()
		{
			try
			{
				Part.Activate();
				return CompositionResult.SucceededResult;
			}
			catch (ComposablePartException innerException)
			{
				return new CompositionResult(ErrorBuilder.CreatePartCannotActivate(Part, innerException));
			}
		}

		public void UpdateDisposableDependencies(ImportDefinition import, IEnumerable<Export> exports)
		{
			List<IDisposable> list = null;
			foreach (IDisposable item in exports.OfType<IDisposable>())
			{
				if (list == null)
				{
					list = new List<IDisposable>();
				}
				list.Add(item);
			}
			List<IDisposable> value = null;
			if (_importedDisposableExports != null && _importedDisposableExports.TryGetValue(import, out value))
			{
				value.ForEach(delegate(IDisposable disposable)
				{
					disposable.Dispose();
				});
				if (list == null)
				{
					_importedDisposableExports.Remove(import);
					if (!_importedDisposableExports.FastAny())
					{
						_importedDisposableExports = null;
					}
					return;
				}
			}
			if (list != null)
			{
				if (_importedDisposableExports == null)
				{
					_importedDisposableExports = new Dictionary<ImportDefinition, List<IDisposable>>();
				}
				_importedDisposableExports[import] = list;
			}
		}

		public void DisposeAllDependencies()
		{
			if (_importedDisposableExports != null)
			{
				IEnumerable<IDisposable> source = _importedDisposableExports.Values.SelectMany((List<IDisposable> exports) => exports);
				_importedDisposableExports = null;
				source.ForEach(delegate(IDisposable disposableExport)
				{
					disposableExport.Dispose();
				});
			}
		}
	}

	private class RecompositionManager
	{
		private WeakReferenceCollection<PartManager> _partsToIndex = new WeakReferenceCollection<PartManager>();

		private WeakReferenceCollection<PartManager> _partsToUnindex = new WeakReferenceCollection<PartManager>();

		private Dictionary<string, WeakReferenceCollection<PartManager>> _partManagerIndex = new Dictionary<string, WeakReferenceCollection<PartManager>>();

		public void AddPartToIndex(PartManager partManager)
		{
			_partsToIndex.Add(partManager);
		}

		public void AddPartToUnindex(PartManager partManager)
		{
			_partsToUnindex.Add(partManager);
		}

		public IEnumerable<PartManager> GetAffectedParts(IEnumerable<string> changedContractNames)
		{
			UpdateImportIndex();
			List<PartManager> list = new List<PartManager>();
			list.AddRange(GetPartsImporting(ImportDefinition.EmptyContractName));
			foreach (string changedContractName in changedContractNames)
			{
				list.AddRange(GetPartsImporting(changedContractName));
			}
			return list;
		}

		public static IEnumerable<ImportDefinition> GetAffectedImports(ComposablePart part, IEnumerable<ExportDefinition> changedExports)
		{
			return part.ImportDefinitions.Where((ImportDefinition import) => IsAffectedImport(import, changedExports));
		}

		private static bool IsAffectedImport(ImportDefinition import, IEnumerable<ExportDefinition> changedExports)
		{
			foreach (ExportDefinition changedExport in changedExports)
			{
				if (import.IsConstraintSatisfiedBy(changedExport))
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<PartManager> GetPartsImporting(string contractName)
		{
			if (!_partManagerIndex.TryGetValue(contractName, out var value))
			{
				return Enumerable.Empty<PartManager>();
			}
			return value.AliveItemsToList();
		}

		private void AddIndexEntries(PartManager partManager)
		{
			foreach (string importedContractName in partManager.GetImportedContractNames())
			{
				if (!_partManagerIndex.TryGetValue(importedContractName, out var value))
				{
					value = new WeakReferenceCollection<PartManager>();
					_partManagerIndex.Add(importedContractName, value);
				}
				if (!value.Contains(partManager))
				{
					value.Add(partManager);
				}
			}
		}

		private void RemoveIndexEntries(PartManager partManager)
		{
			foreach (string importedContractName in partManager.GetImportedContractNames())
			{
				if (_partManagerIndex.TryGetValue(importedContractName, out var value))
				{
					value.Remove(partManager);
					if (value.AliveItemsToList().Count == 0)
					{
						_partManagerIndex.Remove(importedContractName);
					}
				}
			}
		}

		private void UpdateImportIndex()
		{
			List<PartManager> list = _partsToIndex.AliveItemsToList();
			_partsToIndex.Clear();
			List<PartManager> list2 = _partsToUnindex.AliveItemsToList();
			_partsToUnindex.Clear();
			if (list.Count == 0 && list2.Count == 0)
			{
				return;
			}
			foreach (PartManager item in list)
			{
				int num = list2.IndexOf(item);
				if (num >= 0)
				{
					list2[num] = null;
				}
				else
				{
					AddIndexEntries(item);
				}
			}
			foreach (PartManager item2 in list2)
			{
				if (item2 != null)
				{
					RemoveIndexEntries(item2);
				}
			}
		}
	}

	private enum ImportState
	{
		NoImportsSatisfied,
		ImportsPreviewing,
		ImportsPreviewed,
		PreExportImportsSatisfying,
		PreExportImportsSatisfied,
		PostExportImportsSatisfying,
		PostExportImportsSatisfied,
		ComposedNotifying,
		Composed
	}

	private const int MaximumNumberOfCompositionIterations = 100;

	private volatile bool _isDisposed;

	private ExportProvider _sourceProvider;

	private Stack<PartManager> _recursionStateStack = new Stack<PartManager>();

	private ConditionalWeakTable<ComposablePart, PartManager> _partManagers = new ConditionalWeakTable<ComposablePart, PartManager>();

	private RecompositionManager _recompositionManager = new RecompositionManager();

	private readonly CompositionLock _lock;

	private readonly CompositionOptions _compositionOptions;

	public ImportEngine(ExportProvider sourceProvider)
		: this(sourceProvider, CompositionOptions.Default)
	{
	}

	public ImportEngine(ExportProvider sourceProvider, bool isThreadSafe)
		: this(sourceProvider, isThreadSafe ? CompositionOptions.IsThreadSafe : CompositionOptions.Default)
	{
	}

	public ImportEngine(ExportProvider sourceProvider, CompositionOptions compositionOptions)
	{
		Requires.NotNull(sourceProvider, "sourceProvider");
		_compositionOptions = compositionOptions;
		_sourceProvider = sourceProvider;
		_sourceProvider.ExportsChanging += OnExportsChanging;
		_lock = new CompositionLock(compositionOptions.HasFlag(CompositionOptions.IsThreadSafe));
	}

	public void PreviewImports(ComposablePart part, AtomicComposition atomicComposition)
	{
		ThrowIfDisposed();
		Requires.NotNull(part, "part");
		if (_compositionOptions.HasFlag(CompositionOptions.DisableSilentRejection))
		{
			return;
		}
		IDisposable compositionLockHolder = (_lock.IsThreadSafe ? _lock.LockComposition() : null);
		bool flag = compositionLockHolder != null;
		try
		{
			if (flag)
			{
				atomicComposition?.AddRevertAction(delegate
				{
					compositionLockHolder.Dispose();
				});
			}
			PartManager partManager = GetPartManager(part, createIfNotpresent: true);
			TryPreviewImportsStateMachine(partManager, part, atomicComposition).ThrowOnErrors(atomicComposition);
			StartSatisfyingImports(partManager, atomicComposition);
			if (flag)
			{
				atomicComposition?.AddCompleteAction(delegate
				{
					compositionLockHolder.Dispose();
				});
			}
		}
		finally
		{
			if (flag && atomicComposition == null)
			{
				compositionLockHolder.Dispose();
			}
		}
	}

	public void SatisfyImports(ComposablePart part)
	{
		ThrowIfDisposed();
		Requires.NotNull(part, "part");
		PartManager partManager = GetPartManager(part, createIfNotpresent: true);
		if (partManager.State == ImportState.Composed)
		{
			return;
		}
		using (_lock.LockComposition())
		{
			TrySatisfyImports(partManager, part, shouldTrackImports: true).ThrowOnErrors();
		}
	}

	public void SatisfyImportsOnce(ComposablePart part)
	{
		ThrowIfDisposed();
		Requires.NotNull(part, "part");
		PartManager partManager = GetPartManager(part, createIfNotpresent: true);
		if (partManager.State == ImportState.Composed)
		{
			return;
		}
		using (_lock.LockComposition())
		{
			TrySatisfyImports(partManager, part, shouldTrackImports: false).ThrowOnErrors();
		}
	}

	public void ReleaseImports(ComposablePart part, AtomicComposition atomicComposition)
	{
		ThrowIfDisposed();
		Requires.NotNull(part, "part");
		using (_lock.LockComposition())
		{
			PartManager partManager = GetPartManager(part, createIfNotpresent: false);
			if (partManager != null)
			{
				StopSatisfyingImports(partManager, atomicComposition);
			}
		}
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
		ExportProvider exportProvider = null;
		using (_lock.LockStateForWrite())
		{
			if (!_isDisposed)
			{
				exportProvider = _sourceProvider;
				_sourceProvider = null;
				_recompositionManager = null;
				_partManagers = null;
				_isDisposed = true;
				flag = true;
			}
		}
		if (exportProvider != null)
		{
			exportProvider.ExportsChanging -= OnExportsChanging;
		}
		if (flag)
		{
			_lock.Dispose();
		}
	}

	private CompositionResult TryPreviewImportsStateMachine(PartManager partManager, ComposablePart part, AtomicComposition atomicComposition)
	{
		CompositionResult result = CompositionResult.SucceededResult;
		if (partManager.State == ImportState.ImportsPreviewing)
		{
			return new CompositionResult(ErrorBuilder.CreatePartCycle(part));
		}
		if (partManager.State == ImportState.NoImportsSatisfied)
		{
			partManager.State = ImportState.ImportsPreviewing;
			IEnumerable<ImportDefinition> imports = part.ImportDefinitions.Where(IsRequiredImportForPreview);
			atomicComposition.AddRevertActionAllowNull(delegate
			{
				partManager.State = ImportState.NoImportsSatisfied;
			});
			result = result.MergeResult(TrySatisfyImportSubset(partManager, imports, atomicComposition));
			if (!result.Succeeded)
			{
				partManager.State = ImportState.NoImportsSatisfied;
				return result;
			}
			partManager.State = ImportState.ImportsPreviewed;
		}
		return result;
	}

	private CompositionResult TrySatisfyImportsStateMachine(PartManager partManager, ComposablePart part)
	{
		CompositionResult result = CompositionResult.SucceededResult;
		while (partManager.State < ImportState.Composed)
		{
			ImportState state = partManager.State;
			switch (partManager.State)
			{
			case ImportState.NoImportsSatisfied:
			case ImportState.ImportsPreviewed:
			{
				partManager.State = ImportState.PreExportImportsSatisfying;
				IEnumerable<ImportDefinition> imports2 = part.ImportDefinitions.Where((ImportDefinition import) => import.IsPrerequisite);
				result = result.MergeResult(TrySatisfyImportSubset(partManager, imports2, null));
				partManager.State = ImportState.PreExportImportsSatisfied;
				break;
			}
			case ImportState.PreExportImportsSatisfied:
			{
				partManager.State = ImportState.PostExportImportsSatisfying;
				IEnumerable<ImportDefinition> imports = part.ImportDefinitions.Where((ImportDefinition import) => !import.IsPrerequisite);
				result = result.MergeResult(TrySatisfyImportSubset(partManager, imports, null));
				partManager.State = ImportState.PostExportImportsSatisfied;
				break;
			}
			case ImportState.PostExportImportsSatisfied:
				partManager.State = ImportState.ComposedNotifying;
				partManager.ClearSavedImports();
				result = result.MergeResult(partManager.TryOnComposed());
				partManager.State = ImportState.Composed;
				break;
			case ImportState.ImportsPreviewing:
				return new CompositionResult(ErrorBuilder.CreatePartCycle(part));
			case ImportState.PreExportImportsSatisfying:
			case ImportState.PostExportImportsSatisfying:
				if (InPrerequisiteLoop())
				{
					return result.MergeError(ErrorBuilder.CreatePartCycle(part));
				}
				return result;
			case ImportState.ComposedNotifying:
				return result;
			}
			if (!result.Succeeded)
			{
				partManager.State = state;
				return result;
			}
		}
		return result;
	}

	private CompositionResult TrySatisfyImports(PartManager partManager, ComposablePart part, bool shouldTrackImports)
	{
		Assumes.NotNull(part);
		CompositionResult result = CompositionResult.SucceededResult;
		if (partManager.State == ImportState.Composed)
		{
			return result;
		}
		if (_recursionStateStack.Count >= 100)
		{
			return result.MergeError(ErrorBuilder.ComposeTookTooManyIterations(100));
		}
		_recursionStateStack.Push(partManager);
		try
		{
			result = result.MergeResult(TrySatisfyImportsStateMachine(partManager, part));
		}
		finally
		{
			_recursionStateStack.Pop();
		}
		if (shouldTrackImports)
		{
			StartSatisfyingImports(partManager, null);
		}
		return result;
	}

	private CompositionResult TrySatisfyImportSubset(PartManager partManager, IEnumerable<ImportDefinition> imports, AtomicComposition atomicComposition)
	{
		CompositionResult result = CompositionResult.SucceededResult;
		ComposablePart part = partManager.Part;
		foreach (ImportDefinition import in imports)
		{
			Export[] array = partManager.GetSavedImport(import);
			if (array == null)
			{
				CompositionResult<IEnumerable<Export>> compositionResult = TryGetExports(_sourceProvider, part, import, atomicComposition);
				if (!compositionResult.Succeeded)
				{
					result = result.MergeResult(compositionResult.ToResult());
					continue;
				}
				array = compositionResult.Value.AsArray();
			}
			if (atomicComposition == null)
			{
				result = result.MergeResult(partManager.TrySetImport(import, array));
			}
			else
			{
				partManager.SetSavedImport(import, array, atomicComposition);
			}
		}
		return result;
	}

	private void OnExportsChanging(object sender, ExportsChangeEventArgs e)
	{
		CompositionResult compositionResult = CompositionResult.SucceededResult;
		AtomicComposition atomicComposition = e.AtomicComposition;
		IEnumerable<PartManager> enumerable = _recompositionManager.GetAffectedParts(e.ChangedContractNames);
		if (atomicComposition != null && atomicComposition.TryGetValue<EngineContext>(this, out var value))
		{
			enumerable = enumerable.ConcatAllowingNull(value.GetAddedPartManagers()).Except(value.GetRemovedPartManagers());
		}
		IEnumerable<ExportDefinition> changedExports = e.AddedExports.ConcatAllowingNull(e.RemovedExports);
		foreach (PartManager item in enumerable)
		{
			compositionResult = compositionResult.MergeResult(TryRecomposeImports(item, changedExports, atomicComposition));
		}
		compositionResult.ThrowOnErrors(atomicComposition);
	}

	private CompositionResult TryRecomposeImports(PartManager partManager, IEnumerable<ExportDefinition> changedExports, AtomicComposition atomicComposition)
	{
		CompositionResult result = CompositionResult.SucceededResult;
		ImportState state = partManager.State;
		if (state != ImportState.ImportsPreviewed && state != ImportState.Composed)
		{
			return new CompositionResult(ErrorBuilder.InvalidStateForRecompposition(partManager.Part));
		}
		IEnumerable<ImportDefinition> affectedImports = RecompositionManager.GetAffectedImports(partManager.Part, changedExports);
		bool flag = partManager.State == ImportState.Composed;
		bool flag2 = false;
		foreach (ImportDefinition item in affectedImports)
		{
			result = result.MergeResult(TryRecomposeImport(partManager, flag, item, atomicComposition));
			flag2 = true;
		}
		if (result.Succeeded && flag2 && flag)
		{
			if (atomicComposition == null)
			{
				result = result.MergeResult(partManager.TryOnComposed());
			}
			else
			{
				atomicComposition.AddCompleteAction(delegate
				{
					partManager.TryOnComposed().ThrowOnErrors();
				});
			}
		}
		return result;
	}

	private CompositionResult TryRecomposeImport(PartManager partManager, bool partComposed, ImportDefinition import, AtomicComposition atomicComposition)
	{
		if (partComposed && !import.IsRecomposable)
		{
			return new CompositionResult(ErrorBuilder.PreventedByExistingImport(partManager.Part, import));
		}
		CompositionResult<IEnumerable<Export>> compositionResult = TryGetExports(_sourceProvider, partManager.Part, import, atomicComposition);
		if (!compositionResult.Succeeded)
		{
			return compositionResult.ToResult();
		}
		Export[] exports = compositionResult.Value.AsArray();
		if (partComposed)
		{
			if (atomicComposition == null)
			{
				return partManager.TrySetImport(import, exports);
			}
			atomicComposition.AddCompleteAction(delegate
			{
				partManager.TrySetImport(import, exports).ThrowOnErrors();
			});
		}
		else
		{
			partManager.SetSavedImport(import, exports, atomicComposition);
		}
		return CompositionResult.SucceededResult;
	}

	private void StartSatisfyingImports(PartManager partManager, AtomicComposition atomicComposition)
	{
		if (atomicComposition == null)
		{
			if (!partManager.TrackingImports)
			{
				partManager.TrackingImports = true;
				_recompositionManager.AddPartToIndex(partManager);
			}
		}
		else
		{
			GetEngineContext(atomicComposition).AddPartManager(partManager);
		}
	}

	private void StopSatisfyingImports(PartManager partManager, AtomicComposition atomicComposition)
	{
		if (atomicComposition == null)
		{
			_partManagers.Remove(partManager.Part);
			partManager.DisposeAllDependencies();
			if (partManager.TrackingImports)
			{
				partManager.TrackingImports = false;
				_recompositionManager.AddPartToUnindex(partManager);
			}
		}
		else
		{
			GetEngineContext(atomicComposition).RemovePartManager(partManager);
		}
	}

	private PartManager GetPartManager(ComposablePart part, bool createIfNotpresent)
	{
		PartManager value = null;
		using (_lock.LockStateForRead())
		{
			if (_partManagers.TryGetValue(part, out value))
			{
				return value;
			}
		}
		if (createIfNotpresent)
		{
			using (_lock.LockStateForWrite())
			{
				if (!_partManagers.TryGetValue(part, out value))
				{
					value = new PartManager(this, part);
					_partManagers.Add(part, value);
				}
			}
		}
		return value;
	}

	private EngineContext GetEngineContext(AtomicComposition atomicComposition)
	{
		Assumes.NotNull(atomicComposition);
		if (!atomicComposition.TryGetValue<EngineContext>(this, localAtomicCompositionOnly: true, out var value))
		{
			atomicComposition.TryGetValue<EngineContext>(this, localAtomicCompositionOnly: false, out var value2);
			value = new EngineContext(this, value2);
			atomicComposition.SetValue(this, value);
			atomicComposition.AddCompleteAction(value.Complete);
		}
		return value;
	}

	private bool InPrerequisiteLoop()
	{
		PartManager partManager = _recursionStateStack.First();
		PartManager partManager2 = null;
		foreach (PartManager item in _recursionStateStack.Skip(1))
		{
			if (item.State == ImportState.PreExportImportsSatisfying)
			{
				return true;
			}
			if (item == partManager)
			{
				partManager2 = item;
				break;
			}
		}
		Assumes.IsTrue(partManager2 == partManager);
		return false;
	}

	[DebuggerStepThrough]
	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}

	private static CompositionResult<IEnumerable<Export>> TryGetExports(ExportProvider provider, ComposablePart part, ImportDefinition definition, AtomicComposition atomicComposition)
	{
		try
		{
			return new CompositionResult<IEnumerable<Export>>(provider.GetExports(definition, atomicComposition).AsArray());
		}
		catch (ImportCardinalityMismatchException exception)
		{
			CompositionException innerException = new CompositionException(ErrorBuilder.CreateImportCardinalityMismatch(exception, definition));
			return new CompositionResult<IEnumerable<Export>>(ErrorBuilder.CreatePartCannotSetImport(part, definition, innerException));
		}
	}

	internal static bool IsRequiredImportForPreview(ImportDefinition import)
	{
		return import.Cardinality == ImportCardinality.ExactlyOne;
	}
}
