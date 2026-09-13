using System.Collections.Generic;

namespace BestHTTP.SignalRCore;

public class StreamItemContainer<T>
{
	public readonly long id;

	public bool IsCanceled;

	public List<T> Items { get; private set; }

	public T LastAdded { get; private set; }

	public StreamItemContainer(long _id)
	{
		id = _id;
		Items = new List<T>();
	}

	public void AddItem(T item)
	{
		if (Items == null)
		{
			Items = new List<T>();
		}
		Items.Add(item);
		LastAdded = item;
	}
}
