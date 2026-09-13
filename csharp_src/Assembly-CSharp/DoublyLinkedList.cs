using System;

public class DoublyLinkedList
{
	private AutoDisposePool _head;

	private AutoDisposePool _tail;

	public AutoDisposePool Head => _head;

	public AutoDisposePool Tail => _tail;

	public void AddFirst(AutoDisposePool node)
	{
		if (node != null && node != _head)
		{
			if (_head == null)
			{
				_head = (_tail = node);
				return;
			}
			node.next = _head;
			_head.prev = node;
			_head = node;
		}
	}

	public void AddLast(AutoDisposePool node)
	{
		if (node != null && node != _tail)
		{
			if (_head == null)
			{
				_head = (_tail = node);
				return;
			}
			_tail.next = node;
			node.prev = _tail;
			_tail = node;
		}
	}

	public void Remove(AutoDisposePool node)
	{
		if (node != null)
		{
			if (node == _head)
			{
				_head = node.next;
			}
			if (node == _tail)
			{
				_tail = node.prev;
			}
			if (node.prev != null)
			{
				node.prev.next = node.next;
			}
			if (node.next != null)
			{
				node.next.prev = node.prev;
			}
			node.prev = null;
			node.next = null;
		}
	}

	public void MoveToHead(AutoDisposePool node)
	{
		if (node != null && node != _head)
		{
			if (node == _tail)
			{
				_tail = node.prev;
				_tail.next = null;
			}
			else
			{
				node.prev.next = node.next;
				node.next.prev = node.prev;
			}
			node.prev = null;
			node.next = _head;
			_head.prev = node;
			_head = node;
		}
	}

	public void TraverseForward(Action<AutoDisposePool> cb)
	{
		AutoDisposePool autoDisposePool = _head;
		while (autoDisposePool != null)
		{
			AutoDisposePool next = autoDisposePool.next;
			cb(autoDisposePool);
			autoDisposePool = next;
		}
	}

	public void TraverseBackward(Func<AutoDisposePool, bool> cb)
	{
		AutoDisposePool autoDisposePool = _tail;
		while (autoDisposePool != null)
		{
			AutoDisposePool prev = autoDisposePool.prev;
			if (cb(autoDisposePool))
			{
				autoDisposePool = prev;
				continue;
			}
			break;
		}
	}
}
