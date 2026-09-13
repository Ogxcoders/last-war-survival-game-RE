using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels;

[ComVisible(true)]
public abstract class BaseChannelObjectWithProperties : IDictionary, ICollection, IEnumerable
{
	private Hashtable table;

	public virtual int Count => table.Count;

	public virtual bool IsFixedSize => true;

	public virtual bool IsReadOnly => false;

	public virtual bool IsSynchronized => false;

	public virtual object this[object key]
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public virtual ICollection Keys => table.Keys;

	public virtual IDictionary Properties => this;

	public virtual object SyncRoot => this;

	public virtual ICollection Values => table.Values;

	protected BaseChannelObjectWithProperties()
	{
		table = new Hashtable();
	}

	public virtual void Add(object key, object value)
	{
		throw new NotSupportedException();
	}

	public virtual void Clear()
	{
		throw new NotSupportedException();
	}

	public virtual bool Contains(object key)
	{
		return table.Contains(key);
	}

	public virtual void CopyTo(Array array, int index)
	{
		throw new NotSupportedException();
	}

	public virtual IDictionaryEnumerator GetEnumerator()
	{
		return table.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return table.GetEnumerator();
	}

	public virtual void Remove(object key)
	{
		throw new NotSupportedException();
	}
}
