using System.Collections.Generic;

namespace System.Runtime;

internal class DuplicateDetector<T> where T : class
{
	private LinkedList<T> fifoList;

	private Dictionary<T, LinkedListNode<T>> items;

	private int capacity;

	private object thisLock;

	public DuplicateDetector(int capacity)
	{
		this.capacity = capacity;
		items = new Dictionary<T, LinkedListNode<T>>();
		fifoList = new LinkedList<T>();
		thisLock = new object();
	}

	public bool AddIfNotDuplicate(T value)
	{
		bool result = false;
		lock (thisLock)
		{
			if (!items.ContainsKey(value))
			{
				Add(value);
				result = true;
			}
		}
		return result;
	}

	private void Add(T value)
	{
		if (items.Count == capacity)
		{
			LinkedListNode<T> last = fifoList.Last;
			items.Remove(last.Value);
			fifoList.Remove(last);
		}
		items.Add(value, fifoList.AddFirst(value));
	}

	public bool Remove(T value)
	{
		bool result = false;
		lock (thisLock)
		{
			if (items.TryGetValue(value, out var value2))
			{
				items.Remove(value);
				fifoList.Remove(value2);
				result = true;
			}
		}
		return result;
	}

	public void Clear()
	{
		lock (thisLock)
		{
			fifoList.Clear();
			items.Clear();
		}
	}
}
