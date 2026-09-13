using System.Collections;

namespace System.ComponentModel;

public class ListSortDescriptionCollection : IList, ICollection, IEnumerable
{
	private ArrayList _sorts = new ArrayList();

	public ListSortDescription this[int index]
	{
		get
		{
			return (ListSortDescription)_sorts[index];
		}
		set
		{
			throw new InvalidOperationException("Once a ListSortDescriptionCollection has been created it can't be modified.");
		}
	}

	bool IList.IsFixedSize => true;

	bool IList.IsReadOnly => true;

	object IList.this[int index]
	{
		get
		{
			return this[index];
		}
		set
		{
			throw new InvalidOperationException("Once a ListSortDescriptionCollection has been created it can't be modified.");
		}
	}

	public int Count => _sorts.Count;

	bool ICollection.IsSynchronized => true;

	object ICollection.SyncRoot => this;

	public ListSortDescriptionCollection()
	{
	}

	public ListSortDescriptionCollection(ListSortDescription[] sorts)
	{
		if (sorts != null)
		{
			for (int i = 0; i < sorts.Length; i++)
			{
				_sorts.Add(sorts[i]);
			}
		}
	}

	int IList.Add(object value)
	{
		throw new InvalidOperationException("Once a ListSortDescriptionCollection has been created it can't be modified.");
	}

	void IList.Clear()
	{
		throw new InvalidOperationException("Once a ListSortDescriptionCollection has been created it can't be modified.");
	}

	public bool Contains(object value)
	{
		return ((IList)_sorts).Contains(value);
	}

	public int IndexOf(object value)
	{
		return ((IList)_sorts).IndexOf(value);
	}

	void IList.Insert(int index, object value)
	{
		throw new InvalidOperationException("Once a ListSortDescriptionCollection has been created it can't be modified.");
	}

	void IList.Remove(object value)
	{
		throw new InvalidOperationException("Once a ListSortDescriptionCollection has been created it can't be modified.");
	}

	void IList.RemoveAt(int index)
	{
		throw new InvalidOperationException("Once a ListSortDescriptionCollection has been created it can't be modified.");
	}

	public void CopyTo(Array array, int index)
	{
		_sorts.CopyTo(array, index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _sorts.GetEnumerator();
	}
}
