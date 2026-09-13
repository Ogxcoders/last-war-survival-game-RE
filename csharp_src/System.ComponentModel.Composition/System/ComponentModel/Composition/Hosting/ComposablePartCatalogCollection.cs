using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Threading;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting;

internal class ComposablePartCatalogCollection : ICollection<ComposablePartCatalog>, IEnumerable<ComposablePartCatalog>, IEnumerable, INotifyComposablePartCatalogChanged, IDisposable
{
	private readonly Microsoft.Internal.Lock _lock = new Microsoft.Internal.Lock();

	private Action<ComposablePartCatalogChangeEventArgs> _onChanged;

	private Action<ComposablePartCatalogChangeEventArgs> _onChanging;

	private List<ComposablePartCatalog> _catalogs = new List<ComposablePartCatalog>();

	private volatile bool _isCopyNeeded;

	private volatile bool _isDisposed;

	private bool _hasChanged;

	public int Count
	{
		get
		{
			ThrowIfDisposed();
			using (new ReadLock(_lock))
			{
				return _catalogs.Count;
			}
		}
	}

	public bool IsReadOnly
	{
		get
		{
			ThrowIfDisposed();
			return false;
		}
	}

	internal bool HasChanged
	{
		get
		{
			ThrowIfDisposed();
			using (new ReadLock(_lock))
			{
				return _hasChanged;
			}
		}
	}

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;

	public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

	public ComposablePartCatalogCollection(IEnumerable<ComposablePartCatalog> catalogs, Action<ComposablePartCatalogChangeEventArgs> onChanged, Action<ComposablePartCatalogChangeEventArgs> onChanging)
	{
		catalogs = catalogs ?? Enumerable.Empty<ComposablePartCatalog>();
		_catalogs = new List<ComposablePartCatalog>(catalogs);
		_onChanged = onChanged;
		_onChanging = onChanging;
		SubscribeToCatalogNotifications(catalogs);
	}

	public void Add(ComposablePartCatalog item)
	{
		Requires.NotNull(item, "item");
		ThrowIfDisposed();
		Lazy<IEnumerable<ComposablePartDefinition>> addedDefinitions = new Lazy<IEnumerable<ComposablePartDefinition>>(() => item.ToArray(), LazyThreadSafetyMode.PublicationOnly);
		using (AtomicComposition atomicComposition = new AtomicComposition())
		{
			RaiseChangingEvent(addedDefinitions, null, atomicComposition);
			using (new WriteLock(_lock))
			{
				if (_isCopyNeeded)
				{
					_catalogs = new List<ComposablePartCatalog>(_catalogs);
					_isCopyNeeded = false;
				}
				_hasChanged = true;
				_catalogs.Add(item);
			}
			SubscribeToCatalogNotifications(item);
			atomicComposition.Complete();
		}
		RaiseChangedEvent(addedDefinitions, null);
	}

	public void Clear()
	{
		ThrowIfDisposed();
		ComposablePartCatalog[] catalogs = null;
		using (new ReadLock(_lock))
		{
			if (_catalogs.Count == 0)
			{
				return;
			}
			catalogs = _catalogs.ToArray();
		}
		Lazy<IEnumerable<ComposablePartDefinition>> removedDefinitions = new Lazy<IEnumerable<ComposablePartDefinition>>(() => catalogs.SelectMany((ComposablePartCatalog catalog) => catalog).ToArray(), LazyThreadSafetyMode.PublicationOnly);
		using (AtomicComposition atomicComposition = new AtomicComposition())
		{
			RaiseChangingEvent(null, removedDefinitions, atomicComposition);
			UnsubscribeFromCatalogNotifications(catalogs);
			using (new WriteLock(_lock))
			{
				_catalogs = new List<ComposablePartCatalog>();
				_isCopyNeeded = false;
				_hasChanged = true;
			}
			atomicComposition.Complete();
		}
		RaiseChangedEvent(null, removedDefinitions);
	}

	public bool Contains(ComposablePartCatalog item)
	{
		Requires.NotNull(item, "item");
		ThrowIfDisposed();
		using (new ReadLock(_lock))
		{
			return _catalogs.Contains(item);
		}
	}

	public void CopyTo(ComposablePartCatalog[] array, int arrayIndex)
	{
		ThrowIfDisposed();
		using (new ReadLock(_lock))
		{
			_catalogs.CopyTo(array, arrayIndex);
		}
	}

	public bool Remove(ComposablePartCatalog item)
	{
		Requires.NotNull(item, "item");
		ThrowIfDisposed();
		using (new ReadLock(_lock))
		{
			if (!_catalogs.Contains(item))
			{
				return false;
			}
		}
		bool flag = false;
		Lazy<IEnumerable<ComposablePartDefinition>> removedDefinitions = new Lazy<IEnumerable<ComposablePartDefinition>>(() => item.ToArray(), LazyThreadSafetyMode.PublicationOnly);
		using (AtomicComposition atomicComposition = new AtomicComposition())
		{
			RaiseChangingEvent(null, removedDefinitions, atomicComposition);
			using (new WriteLock(_lock))
			{
				if (_isCopyNeeded)
				{
					_catalogs = new List<ComposablePartCatalog>(_catalogs);
					_isCopyNeeded = false;
				}
				flag = _catalogs.Remove(item);
				if (flag)
				{
					_hasChanged = true;
				}
			}
			UnsubscribeFromCatalogNotifications(item);
			atomicComposition.Complete();
		}
		RaiseChangedEvent(null, removedDefinitions);
		return flag;
	}

	public IEnumerator<ComposablePartCatalog> GetEnumerator()
	{
		ThrowIfDisposed();
		using (new WriteLock(_lock))
		{
			object result = _catalogs.GetEnumerator();
			_isCopyNeeded = true;
			return (IEnumerator<ComposablePartCatalog>)result;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
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
		IEnumerable<ComposablePartCatalog> enumerable = null;
		try
		{
			using (new WriteLock(_lock))
			{
				if (!_isDisposed)
				{
					flag = true;
					enumerable = _catalogs;
					_catalogs = null;
					_isDisposed = true;
				}
			}
		}
		finally
		{
			if (enumerable != null)
			{
				UnsubscribeFromCatalogNotifications(enumerable);
				enumerable.ForEach(delegate(ComposablePartCatalog catalog)
				{
					catalog.Dispose();
				});
			}
			if (flag)
			{
				_lock.Dispose();
			}
		}
	}

	private void RaiseChangedEvent(Lazy<IEnumerable<ComposablePartDefinition>> addedDefinitions, Lazy<IEnumerable<ComposablePartDefinition>> removedDefinitions)
	{
		if (_onChanged != null && this.Changed != null)
		{
			IEnumerable<ComposablePartDefinition> addedDefinitions2 = ((addedDefinitions == null) ? Enumerable.Empty<ComposablePartDefinition>() : addedDefinitions.Value);
			IEnumerable<ComposablePartDefinition> removedDefinitions2 = ((removedDefinitions == null) ? Enumerable.Empty<ComposablePartDefinition>() : removedDefinitions.Value);
			_onChanged(new ComposablePartCatalogChangeEventArgs(addedDefinitions2, removedDefinitions2, null));
		}
	}

	public void OnChanged(object sender, ComposablePartCatalogChangeEventArgs e)
	{
		this.Changed?.Invoke(sender, e);
	}

	private void RaiseChangingEvent(Lazy<IEnumerable<ComposablePartDefinition>> addedDefinitions, Lazy<IEnumerable<ComposablePartDefinition>> removedDefinitions, AtomicComposition atomicComposition)
	{
		if (_onChanging != null && this.Changing != null)
		{
			IEnumerable<ComposablePartDefinition> addedDefinitions2 = ((addedDefinitions == null) ? Enumerable.Empty<ComposablePartDefinition>() : addedDefinitions.Value);
			IEnumerable<ComposablePartDefinition> removedDefinitions2 = ((removedDefinitions == null) ? Enumerable.Empty<ComposablePartDefinition>() : removedDefinitions.Value);
			_onChanging(new ComposablePartCatalogChangeEventArgs(addedDefinitions2, removedDefinitions2, atomicComposition));
		}
	}

	public void OnChanging(object sender, ComposablePartCatalogChangeEventArgs e)
	{
		this.Changing?.Invoke(sender, e);
	}

	private void OnContainedCatalogChanged(object sender, ComposablePartCatalogChangeEventArgs e)
	{
		if (_onChanged != null && this.Changed != null)
		{
			_onChanged(e);
		}
	}

	private void OnContainedCatalogChanging(object sender, ComposablePartCatalogChangeEventArgs e)
	{
		if (_onChanging != null && this.Changing != null)
		{
			_onChanging(e);
		}
	}

	private void SubscribeToCatalogNotifications(ComposablePartCatalog catalog)
	{
		if (catalog is INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged)
		{
			notifyComposablePartCatalogChanged.Changed += OnContainedCatalogChanged;
			notifyComposablePartCatalogChanged.Changing += OnContainedCatalogChanging;
		}
	}

	private void SubscribeToCatalogNotifications(IEnumerable<ComposablePartCatalog> catalogs)
	{
		foreach (ComposablePartCatalog catalog in catalogs)
		{
			SubscribeToCatalogNotifications(catalog);
		}
	}

	private void UnsubscribeFromCatalogNotifications(ComposablePartCatalog catalog)
	{
		if (catalog is INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged)
		{
			notifyComposablePartCatalogChanged.Changed -= OnContainedCatalogChanged;
			notifyComposablePartCatalogChanged.Changing -= OnContainedCatalogChanging;
		}
	}

	private void UnsubscribeFromCatalogNotifications(IEnumerable<ComposablePartCatalog> catalogs)
	{
		foreach (ComposablePartCatalog catalog in catalogs)
		{
			UnsubscribeFromCatalogNotifications(catalog);
		}
	}

	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}
}
