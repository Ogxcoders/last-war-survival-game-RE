using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

[DebuggerTypeProxy(typeof(CompositionScopeDefinitionDebuggerProxy))]
public class CompositionScopeDefinition : ComposablePartCatalog, INotifyComposablePartCatalogChanged
{
	private ComposablePartCatalog _catalog;

	private IEnumerable<ExportDefinition> _publicSurface;

	private IEnumerable<CompositionScopeDefinition> _children = Enumerable.Empty<CompositionScopeDefinition>();

	private volatile int _isDisposed;

	public virtual IEnumerable<CompositionScopeDefinition> Children
	{
		get
		{
			ThrowIfDisposed();
			return _children;
		}
	}

	public virtual IEnumerable<ExportDefinition> PublicSurface
	{
		get
		{
			ThrowIfDisposed();
			if (_publicSurface == null)
			{
				return this.SelectMany((ComposablePartDefinition p) => p.ExportDefinitions);
			}
			return _publicSurface;
		}
	}

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

	protected CompositionScopeDefinition()
	{
	}

	public CompositionScopeDefinition(ComposablePartCatalog catalog, IEnumerable<CompositionScopeDefinition> children)
	{
		Requires.NotNull(catalog, "catalog");
		Requires.NullOrNotNullElements(children, "children");
		InitializeCompositionScopeDefinition(catalog, children, null);
	}

	public CompositionScopeDefinition(ComposablePartCatalog catalog, IEnumerable<CompositionScopeDefinition> children, IEnumerable<ExportDefinition> publicSurface)
	{
		Requires.NotNull(catalog, "catalog");
		Requires.NullOrNotNullElements(children, "children");
		Requires.NullOrNotNullElements(publicSurface, "publicSurface");
		InitializeCompositionScopeDefinition(catalog, children, publicSurface);
	}

	private void InitializeCompositionScopeDefinition(ComposablePartCatalog catalog, IEnumerable<CompositionScopeDefinition> children, IEnumerable<ExportDefinition> publicSurface)
	{
		_catalog = catalog;
		if (children != null)
		{
			_children = children.ToArray();
		}
		if (publicSurface != null)
		{
			_publicSurface = publicSurface;
		}
		if (_catalog is INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged)
		{
			notifyComposablePartCatalogChanged.Changed += OnChangedInternal;
			notifyComposablePartCatalogChanged.Changing += OnChangingInternal;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 0 && _catalog is INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged)
			{
				notifyComposablePartCatalogChanged.Changed -= OnChangedInternal;
				notifyComposablePartCatalogChanged.Changing -= OnChangingInternal;
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		return _catalog.GetEnumerator();
	}

	public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		ThrowIfDisposed();
		return _catalog.GetExports(definition);
	}

	internal IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExportsFromPublicSurface(ImportDefinition definition)
	{
		Assumes.NotNull(definition, "definition");
		List<Tuple<ComposablePartDefinition, ExportDefinition>> list = new List<Tuple<ComposablePartDefinition, ExportDefinition>>();
		foreach (ExportDefinition item in PublicSurface)
		{
			if (!definition.IsConstraintSatisfiedBy(item))
			{
				continue;
			}
			foreach (Tuple<ComposablePartDefinition, ExportDefinition> export in GetExports(definition))
			{
				if (export.Item2 == item)
				{
					list.Add(export);
					break;
				}
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
		OnChanged(e);
	}

	private void OnChangingInternal(object sender, ComposablePartCatalogChangeEventArgs e)
	{
		OnChanging(e);
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
