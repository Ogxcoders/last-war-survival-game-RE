using System.Collections;

namespace System.Diagnostics;

public class TraceListenerCollection : IList, ICollection, IEnumerable
{
	private ArrayList list;

	public TraceListener this[int i]
	{
		get
		{
			return (TraceListener)list[i];
		}
		set
		{
			InitializeListener(value);
			list[i] = value;
		}
	}

	public TraceListener this[string name]
	{
		get
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					TraceListener traceListener = (TraceListener)enumerator.Current;
					if (traceListener.Name == name)
					{
						return traceListener;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return null;
		}
	}

	public int Count => list.Count;

	object IList.this[int index]
	{
		get
		{
			return list[index];
		}
		set
		{
			if (!(value is TraceListener traceListener))
			{
				throw new ArgumentException(global::SR.GetString("Only TraceListeners can be added to a TraceListenerCollection."), "value");
			}
			InitializeListener(traceListener);
			list[index] = traceListener;
		}
	}

	bool IList.IsReadOnly => false;

	bool IList.IsFixedSize => false;

	object ICollection.SyncRoot => this;

	bool ICollection.IsSynchronized => true;

	internal TraceListenerCollection()
	{
		list = new ArrayList(1);
	}

	public int Add(TraceListener listener)
	{
		InitializeListener(listener);
		lock (TraceInternal.critSec)
		{
			return list.Add(listener);
		}
	}

	public void AddRange(TraceListener[] value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		for (int i = 0; i < value.Length; i++)
		{
			Add(value[i]);
		}
	}

	public void AddRange(TraceListenerCollection value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		int count = value.Count;
		for (int i = 0; i < count; i++)
		{
			Add(value[i]);
		}
	}

	public void Clear()
	{
		list = new ArrayList();
	}

	public bool Contains(TraceListener listener)
	{
		return ((IList)this).Contains((object)listener);
	}

	public void CopyTo(TraceListener[] listeners, int index)
	{
		((ICollection)this).CopyTo((Array)listeners, index);
	}

	public IEnumerator GetEnumerator()
	{
		return list.GetEnumerator();
	}

	internal void InitializeListener(TraceListener listener)
	{
		if (listener == null)
		{
			throw new ArgumentNullException("listener");
		}
		listener.IndentSize = TraceInternal.IndentSize;
		listener.IndentLevel = TraceInternal.IndentLevel;
	}

	public int IndexOf(TraceListener listener)
	{
		return ((IList)this).IndexOf((object)listener);
	}

	public void Insert(int index, TraceListener listener)
	{
		InitializeListener(listener);
		lock (TraceInternal.critSec)
		{
			list.Insert(index, listener);
		}
	}

	public void Remove(TraceListener listener)
	{
		((IList)this).Remove((object)listener);
	}

	public void Remove(string name)
	{
		TraceListener traceListener = this[name];
		if (traceListener != null)
		{
			((IList)this).Remove((object)traceListener);
		}
	}

	public void RemoveAt(int index)
	{
		lock (TraceInternal.critSec)
		{
			list.RemoveAt(index);
		}
	}

	int IList.Add(object value)
	{
		if (!(value is TraceListener listener))
		{
			throw new ArgumentException(global::SR.GetString("Only TraceListeners can be added to a TraceListenerCollection."), "value");
		}
		InitializeListener(listener);
		lock (TraceInternal.critSec)
		{
			return list.Add(value);
		}
	}

	bool IList.Contains(object value)
	{
		return list.Contains(value);
	}

	int IList.IndexOf(object value)
	{
		return list.IndexOf(value);
	}

	void IList.Insert(int index, object value)
	{
		if (!(value is TraceListener listener))
		{
			throw new ArgumentException(global::SR.GetString("Only TraceListeners can be added to a TraceListenerCollection."), "value");
		}
		InitializeListener(listener);
		lock (TraceInternal.critSec)
		{
			list.Insert(index, value);
		}
	}

	void IList.Remove(object value)
	{
		lock (TraceInternal.critSec)
		{
			list.Remove(value);
		}
	}

	void ICollection.CopyTo(Array array, int index)
	{
		lock (TraceInternal.critSec)
		{
			list.CopyTo(array, index);
		}
	}
}
