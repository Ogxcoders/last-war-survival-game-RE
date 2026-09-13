using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Primitives;

[DebuggerTypeProxy(typeof(ComposablePartCatalogDebuggerProxy))]
public abstract class ComposablePartCatalog : IEnumerable<ComposablePartDefinition>, IEnumerable, IDisposable
{
	private bool _isDisposed;

	private volatile IQueryable<ComposablePartDefinition> _queryableParts;

	private static readonly List<Tuple<ComposablePartDefinition, ExportDefinition>> _EmptyExportsList = new List<Tuple<ComposablePartDefinition, ExportDefinition>>();

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual IQueryable<ComposablePartDefinition> Parts
	{
		get
		{
			ThrowIfDisposed();
			if (_queryableParts == null)
			{
				IQueryable<ComposablePartDefinition> value = this.AsQueryable();
				Interlocked.CompareExchange(ref _queryableParts, value, null);
				Assumes.NotNull(_queryableParts);
			}
			return _queryableParts;
		}
	}

	public virtual IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		ThrowIfDisposed();
		Requires.NotNull(definition, "definition");
		List<Tuple<ComposablePartDefinition, ExportDefinition>> list = null;
		IEnumerable<ComposablePartDefinition> candidateParts = GetCandidateParts(definition);
		if (candidateParts != null)
		{
			foreach (ComposablePartDefinition item in candidateParts)
			{
				IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> exports = item.GetExports(definition);
				if (exports != ComposablePartDefinition._EmptyExports)
				{
					list = list.FastAppendToListAllowNulls(exports);
				}
			}
		}
		return list ?? _EmptyExportsList;
	}

	internal virtual IEnumerable<ComposablePartDefinition> GetCandidateParts(ImportDefinition definition)
	{
		return this;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		_isDisposed = true;
	}

	[DebuggerStepThrough]
	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}

	public virtual IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		IQueryable<ComposablePartDefinition> parts = Parts;
		if (parts == _queryableParts)
		{
			return Enumerable.Empty<ComposablePartDefinition>().GetEnumerator();
		}
		return parts.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
