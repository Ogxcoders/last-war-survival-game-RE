using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace System.Collections.Concurrent;

[Serializable]
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(IProducerConsumerCollectionDebugView<>))]
public class ConcurrentStack<T> : IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
{
	[Serializable]
	private class Node
	{
		internal readonly T _value;

		internal Node _next;

		internal Node(T value)
		{
			_value = value;
			_next = null;
		}
	}

	private volatile Node _head;

	private const int BACKOFF_MAX_YIELDS = 8;

	public bool IsEmpty => _head == null;

	public int Count
	{
		get
		{
			int num = 0;
			for (Node node = _head; node != null; node = node._next)
			{
				num++;
			}
			return num;
		}
	}

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot
	{
		get
		{
			throw new NotSupportedException("The SyncRoot property may not be used for the synchronization of concurrent collections.");
		}
	}

	public ConcurrentStack()
	{
	}

	public ConcurrentStack(IEnumerable<T> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		InitializeFromCollection(collection);
	}

	private void InitializeFromCollection(IEnumerable<T> collection)
	{
		Node node = null;
		using (IEnumerator<T> enumerator = collection.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				node = new Node(enumerator.Current)
				{
					_next = node
				};
			}
		}
		_head = node;
	}

	public void Clear()
	{
		_head = null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		((ICollection)ToList()).CopyTo(array, index);
	}

	public void CopyTo(T[] array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		ToList().CopyTo(array, index);
	}

	public void Push(T item)
	{
		Node node = new Node(item);
		node._next = _head;
		if (Interlocked.CompareExchange(ref _head, node, node._next) != node._next)
		{
			PushCore(node, node);
		}
	}

	public void PushRange(T[] items)
	{
		if (items == null)
		{
			throw new ArgumentNullException("items");
		}
		PushRange(items, 0, items.Length);
	}

	public void PushRange(T[] items, int startIndex, int count)
	{
		ValidatePushPopRangeInput(items, startIndex, count);
		if (count != 0)
		{
			Node node2;
			Node node = (node2 = new Node(items[startIndex]));
			for (int i = startIndex + 1; i < startIndex + count; i++)
			{
				node = new Node(items[i])
				{
					_next = node
				};
			}
			node2._next = _head;
			if (Interlocked.CompareExchange(ref _head, node, node2._next) != node2._next)
			{
				PushCore(node, node2);
			}
		}
	}

	private void PushCore(Node head, Node tail)
	{
		SpinWait spinWait = default(SpinWait);
		do
		{
			spinWait.SpinOnce();
			tail._next = _head;
		}
		while (Interlocked.CompareExchange(ref _head, head, tail._next) != tail._next);
		if (CDSCollectionETWBCLProvider.Log.IsEnabled())
		{
			CDSCollectionETWBCLProvider.Log.ConcurrentStack_FastPushFailed(spinWait.Count);
		}
	}

	private static void ValidatePushPopRangeInput(T[] items, int startIndex, int count)
	{
		if (items == null)
		{
			throw new ArgumentNullException("items");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "The count argument must be greater than or equal to zero.");
		}
		int num = items.Length;
		if (startIndex >= num || startIndex < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex", "The startIndex argument must be greater than or equal to zero.");
		}
		if (num - count < startIndex)
		{
			throw new ArgumentException("The sum of the startIndex and count arguments must be less than or equal to the collection's Count.");
		}
	}

	bool IProducerConsumerCollection<T>.TryAdd(T item)
	{
		Push(item);
		return true;
	}

	public bool TryPeek(out T result)
	{
		Node head = _head;
		if (head == null)
		{
			result = default(T);
			return false;
		}
		result = head._value;
		return true;
	}

	public bool TryPop(out T result)
	{
		Node head = _head;
		if (head == null)
		{
			result = default(T);
			return false;
		}
		if (Interlocked.CompareExchange(ref _head, head._next, head) == head)
		{
			result = head._value;
			return true;
		}
		return TryPopCore(out result);
	}

	public int TryPopRange(T[] items)
	{
		if (items == null)
		{
			throw new ArgumentNullException("items");
		}
		return TryPopRange(items, 0, items.Length);
	}

	public int TryPopRange(T[] items, int startIndex, int count)
	{
		ValidatePushPopRangeInput(items, startIndex, count);
		if (count == 0)
		{
			return 0;
		}
		Node poppedHead;
		int num = TryPopCore(count, out poppedHead);
		if (num > 0)
		{
			CopyRemovedItems(poppedHead, items, startIndex, num);
		}
		return num;
	}

	private bool TryPopCore(out T result)
	{
		if (TryPopCore(1, out var poppedHead) == 1)
		{
			result = poppedHead._value;
			return true;
		}
		result = default(T);
		return false;
	}

	private int TryPopCore(int count, out Node poppedHead)
	{
		SpinWait spinWait = default(SpinWait);
		int num = 1;
		Random random = null;
		Node head;
		int i;
		while (true)
		{
			head = _head;
			if (head == null)
			{
				if (count == 1 && CDSCollectionETWBCLProvider.Log.IsEnabled())
				{
					CDSCollectionETWBCLProvider.Log.ConcurrentStack_FastPopFailed(spinWait.Count);
				}
				poppedHead = null;
				return 0;
			}
			Node node = head;
			for (i = 1; i < count; i++)
			{
				if (node._next == null)
				{
					break;
				}
				node = node._next;
			}
			if (Interlocked.CompareExchange(ref _head, node._next, head) == head)
			{
				break;
			}
			for (int j = 0; j < num; j++)
			{
				spinWait.SpinOnce();
			}
			if (spinWait.NextSpinWillYield)
			{
				if (random == null)
				{
					random = new Random();
				}
				num = random.Next(1, 8);
			}
			else
			{
				num *= 2;
			}
		}
		if (count == 1 && CDSCollectionETWBCLProvider.Log.IsEnabled())
		{
			CDSCollectionETWBCLProvider.Log.ConcurrentStack_FastPopFailed(spinWait.Count);
		}
		poppedHead = head;
		return i;
	}

	private static void CopyRemovedItems(Node head, T[] collection, int startIndex, int nodesCount)
	{
		Node node = head;
		for (int i = startIndex; i < startIndex + nodesCount; i++)
		{
			collection[i] = node._value;
			node = node._next;
		}
	}

	bool IProducerConsumerCollection<T>.TryTake(out T item)
	{
		return TryPop(out item);
	}

	public T[] ToArray()
	{
		Node head = _head;
		if (head != null)
		{
			return ToList(head).ToArray();
		}
		return Array.Empty<T>();
	}

	private List<T> ToList()
	{
		return ToList(_head);
	}

	private List<T> ToList(Node curr)
	{
		List<T> list = new List<T>();
		while (curr != null)
		{
			list.Add(curr._value);
			curr = curr._next;
		}
		return list;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return GetEnumerator(_head);
	}

	private IEnumerator<T> GetEnumerator(Node head)
	{
		for (Node current = head; current != null; current = current._next)
		{
			yield return current._value;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<T>)this).GetEnumerator();
	}
}
