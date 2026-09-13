using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace System.Collections.ObjectModel;

[Serializable]
[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
[DebuggerDisplay("Count = {Count}")]
public class ReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
	event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
	{
		add
		{
			CollectionChanged += value;
		}
		remove
		{
			CollectionChanged -= value;
		}
	}

	protected virtual event NotifyCollectionChangedEventHandler CollectionChanged;

	event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
	{
		add
		{
			PropertyChanged += value;
		}
		remove
		{
			PropertyChanged -= value;
		}
	}

	protected virtual event PropertyChangedEventHandler PropertyChanged;

	public ReadOnlyObservableCollection(ObservableCollection<T> list)
		: base((IList<T>)list)
	{
		((INotifyCollectionChanged)base.Items).CollectionChanged += HandleCollectionChanged;
		((INotifyPropertyChanged)base.Items).PropertyChanged += HandlePropertyChanged;
	}

	protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
	{
		if (this.CollectionChanged != null)
		{
			this.CollectionChanged(this, args);
		}
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, args);
		}
	}

	private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		OnCollectionChanged(e);
	}

	private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		OnPropertyChanged(e);
	}
}
