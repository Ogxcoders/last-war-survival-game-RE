using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting;

public class FilteredCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
{
	internal class DependenciesTraversal : IComposablePartCatalogTraversal
	{
		private IEnumerable<ComposablePartDefinition> _parts;

		private Func<ImportDefinition, bool> _importFilter;

		private Dictionary<string, List<ComposablePartDefinition>> _exportersIndex;

		public DependenciesTraversal(FilteredCatalog catalog, Func<ImportDefinition, bool> importFilter)
		{
			Assumes.NotNull(catalog);
			Assumes.NotNull(importFilter);
			_parts = catalog._innerCatalog;
			_importFilter = importFilter;
		}

		public void Initialize()
		{
			BuildExportersIndex();
		}

		private void BuildExportersIndex()
		{
			_exportersIndex = new Dictionary<string, List<ComposablePartDefinition>>();
			foreach (ComposablePartDefinition part in _parts)
			{
				foreach (ExportDefinition exportDefinition in part.ExportDefinitions)
				{
					AddToExportersIndex(exportDefinition.ContractName, part);
				}
			}
		}

		private void AddToExportersIndex(string contractName, ComposablePartDefinition part)
		{
			List<ComposablePartDefinition> value = null;
			if (!_exportersIndex.TryGetValue(contractName, out value))
			{
				value = new List<ComposablePartDefinition>();
				_exportersIndex.Add(contractName, value);
			}
			value.Add(part);
		}

		public bool TryTraverse(ComposablePartDefinition part, out IEnumerable<ComposablePartDefinition> reachableParts)
		{
			reachableParts = null;
			List<ComposablePartDefinition> list = null;
			foreach (ImportDefinition item in part.ImportDefinitions.Where(_importFilter))
			{
				List<ComposablePartDefinition> value = null;
				foreach (string candidateContractName in item.GetCandidateContractNames(part))
				{
					if (!_exportersIndex.TryGetValue(candidateContractName, out value))
					{
						continue;
					}
					foreach (ComposablePartDefinition item2 in value)
					{
						foreach (ExportDefinition exportDefinition in item2.ExportDefinitions)
						{
							if (item.IsImportDependentOnPart(item2, exportDefinition, part.IsGeneric() != item2.IsGeneric()))
							{
								if (list == null)
								{
									list = new List<ComposablePartDefinition>();
								}
								list.Add(item2);
							}
						}
					}
				}
			}
			reachableParts = list;
			return reachableParts != null;
		}
	}

	internal class DependentsTraversal : IComposablePartCatalogTraversal
	{
		private IEnumerable<ComposablePartDefinition> _parts;

		private Func<ImportDefinition, bool> _importFilter;

		private Dictionary<string, List<ComposablePartDefinition>> _importersIndex;

		public DependentsTraversal(FilteredCatalog catalog, Func<ImportDefinition, bool> importFilter)
		{
			Assumes.NotNull(catalog);
			Assumes.NotNull(importFilter);
			_parts = catalog._innerCatalog;
			_importFilter = importFilter;
		}

		public void Initialize()
		{
			BuildImportersIndex();
		}

		private void BuildImportersIndex()
		{
			_importersIndex = new Dictionary<string, List<ComposablePartDefinition>>();
			foreach (ComposablePartDefinition part in _parts)
			{
				foreach (ImportDefinition importDefinition in part.ImportDefinitions)
				{
					foreach (string candidateContractName in importDefinition.GetCandidateContractNames(part))
					{
						AddToImportersIndex(candidateContractName, part);
					}
				}
			}
		}

		private void AddToImportersIndex(string contractName, ComposablePartDefinition part)
		{
			List<ComposablePartDefinition> value = null;
			if (!_importersIndex.TryGetValue(contractName, out value))
			{
				value = new List<ComposablePartDefinition>();
				_importersIndex.Add(contractName, value);
			}
			value.Add(part);
		}

		public bool TryTraverse(ComposablePartDefinition part, out IEnumerable<ComposablePartDefinition> reachableParts)
		{
			reachableParts = null;
			List<ComposablePartDefinition> list = null;
			foreach (ExportDefinition exportDefinition in part.ExportDefinitions)
			{
				List<ComposablePartDefinition> value = null;
				if (!_importersIndex.TryGetValue(exportDefinition.ContractName, out value))
				{
					continue;
				}
				foreach (ComposablePartDefinition item in value)
				{
					foreach (ImportDefinition item2 in item.ImportDefinitions.Where(_importFilter))
					{
						if (item2.IsImportDependentOnPart(part, exportDefinition, part.IsGeneric() != item.IsGeneric()))
						{
							if (list == null)
							{
								list = new List<ComposablePartDefinition>();
							}
							list.Add(item);
						}
					}
				}
			}
			reachableParts = list;
			return reachableParts != null;
		}
	}

	internal interface IComposablePartCatalogTraversal
	{
		void Initialize();

		bool TryTraverse(ComposablePartDefinition part, out IEnumerable<ComposablePartDefinition> reachableParts);
	}

	private Func<ComposablePartDefinition, bool> _filter;

	private ComposablePartCatalog _innerCatalog;

	private FilteredCatalog _complement;

	private object _lock = new object();

	private volatile bool _isDisposed;

	public FilteredCatalog Complement
	{
		get
		{
			ThrowIfDisposed();
			if (_complement == null)
			{
				FilteredCatalog filteredCatalog = new FilteredCatalog(_innerCatalog, (ComposablePartDefinition p) => !_filter(p), this);
				lock (_lock)
				{
					if (_complement == null)
					{
						Thread.MemoryBarrier();
						_complement = filteredCatalog;
						filteredCatalog = null;
					}
				}
				filteredCatalog?.Dispose();
			}
			return _complement;
		}
	}

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

	public FilteredCatalog IncludeDependencies()
	{
		return IncludeDependencies((ImportDefinition i) => i.Cardinality == ImportCardinality.ExactlyOne);
	}

	public FilteredCatalog IncludeDependencies(Func<ImportDefinition, bool> importFilter)
	{
		Requires.NotNull(importFilter, "importFilter");
		ThrowIfDisposed();
		return Traverse(new DependenciesTraversal(this, importFilter));
	}

	public FilteredCatalog IncludeDependents()
	{
		return IncludeDependents((ImportDefinition i) => i.Cardinality == ImportCardinality.ExactlyOne);
	}

	public FilteredCatalog IncludeDependents(Func<ImportDefinition, bool> importFilter)
	{
		Requires.NotNull(importFilter, "importFilter");
		ThrowIfDisposed();
		return Traverse(new DependentsTraversal(this, importFilter));
	}

	private FilteredCatalog Traverse(IComposablePartCatalogTraversal traversal)
	{
		Assumes.NotNull(traversal);
		FreezeInnerCatalog();
		try
		{
			traversal.Initialize();
			HashSet<ComposablePartDefinition> traversalClosure = GetTraversalClosure(_innerCatalog.Where(_filter), traversal);
			return new FilteredCatalog(_innerCatalog, (ComposablePartDefinition p) => traversalClosure.Contains(p));
		}
		finally
		{
			UnfreezeInnerCatalog();
		}
	}

	private static HashSet<ComposablePartDefinition> GetTraversalClosure(IEnumerable<ComposablePartDefinition> parts, IComposablePartCatalogTraversal traversal)
	{
		Assumes.NotNull(traversal);
		HashSet<ComposablePartDefinition> hashSet = new HashSet<ComposablePartDefinition>();
		GetTraversalClosure(parts, hashSet, traversal);
		return hashSet;
	}

	private static void GetTraversalClosure(IEnumerable<ComposablePartDefinition> parts, HashSet<ComposablePartDefinition> traversedParts, IComposablePartCatalogTraversal traversal)
	{
		foreach (ComposablePartDefinition part in parts)
		{
			if (traversedParts.Add(part))
			{
				IEnumerable<ComposablePartDefinition> reachableParts = null;
				if (traversal.TryTraverse(part, out reachableParts))
				{
					GetTraversalClosure(reachableParts, traversedParts, traversal);
				}
			}
		}
	}

	private void FreezeInnerCatalog()
	{
		if (_innerCatalog is INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged)
		{
			notifyComposablePartCatalogChanged.Changing += ThrowOnRecomposition;
		}
	}

	private void UnfreezeInnerCatalog()
	{
		if (_innerCatalog is INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged)
		{
			notifyComposablePartCatalogChanged.Changing -= ThrowOnRecomposition;
		}
	}

	private static void ThrowOnRecomposition(object sender, ComposablePartCatalogChangeEventArgs e)
	{
		throw new ChangeRejectedException();
	}

	public FilteredCatalog(ComposablePartCatalog catalog, Func<ComposablePartDefinition, bool> filter)
		: this(catalog, filter, null)
	{
	}

	internal FilteredCatalog(ComposablePartCatalog catalog, Func<ComposablePartDefinition, bool> filter, FilteredCatalog complement)
	{
		Requires.NotNull(catalog, "catalog");
		Requires.NotNull(filter, "filter");
		_innerCatalog = catalog;
		_filter = (ComposablePartDefinition p) => filter(p.GetGenericPartDefinition() ?? p);
		_complement = complement;
		if (_innerCatalog is INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged)
		{
			notifyComposablePartCatalogChanged.Changed += OnChangedInternal;
			notifyComposablePartCatalogChanged.Changing += OnChangingInternal;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!disposing || _isDisposed)
			{
				return;
			}
			INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged = null;
			try
			{
				lock (_lock)
				{
					if (!_isDisposed)
					{
						_isDisposed = true;
						notifyComposablePartCatalogChanged = _innerCatalog as INotifyComposablePartCatalogChanged;
						_innerCatalog = null;
					}
				}
			}
			finally
			{
				if (notifyComposablePartCatalogChanged != null)
				{
					notifyComposablePartCatalogChanged.Changed -= OnChangedInternal;
					notifyComposablePartCatalogChanged.Changing -= OnChangingInternal;
				}
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		return _innerCatalog.Where(_filter).GetEnumerator();
	}

	public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		ThrowIfDisposed();
		Requires.NotNull(definition, "definition");
		List<Tuple<ComposablePartDefinition, ExportDefinition>> list = new List<Tuple<ComposablePartDefinition, ExportDefinition>>();
		foreach (Tuple<ComposablePartDefinition, ExportDefinition> export in _innerCatalog.GetExports(definition))
		{
			if (_filter(export.Item1))
			{
				list.Add(export);
			}
		}
		return list;
	}

	protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e)
	{
		this.Changed?.Invoke(this, e);
	}

	protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e)
	{
		this.Changing?.Invoke(this, e);
	}

	private void OnChangedInternal(object sender, ComposablePartCatalogChangeEventArgs e)
	{
		ComposablePartCatalogChangeEventArgs e2 = ProcessEventArgs(e);
		if (e2 != null)
		{
			OnChanged(ProcessEventArgs(e2));
		}
	}

	private void OnChangingInternal(object sender, ComposablePartCatalogChangeEventArgs e)
	{
		ComposablePartCatalogChangeEventArgs e2 = ProcessEventArgs(e);
		if (e2 != null)
		{
			OnChanging(ProcessEventArgs(e2));
		}
	}

	private ComposablePartCatalogChangeEventArgs ProcessEventArgs(ComposablePartCatalogChangeEventArgs e)
	{
		ComposablePartCatalogChangeEventArgs e2 = new ComposablePartCatalogChangeEventArgs(e.AddedDefinitions.Where(_filter), e.RemovedDefinitions.Where(_filter), e.AtomicComposition);
		if (e2.AddedDefinitions.FastAny() || e2.RemovedDefinitions.FastAny())
		{
			return e2;
		}
		return null;
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
