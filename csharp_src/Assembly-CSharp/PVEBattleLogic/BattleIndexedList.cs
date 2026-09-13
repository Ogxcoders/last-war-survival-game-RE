using System.Collections.Generic;

namespace PVEBattleLogic;

public sealed class BattleIndexedList<T> where T : class
{
	private class LinearListNode
	{
		public int index;

		public T obj;
	}

	private List<LinearListNode> list;

	private LinearListNode head;

	private int count;

	public T this[int i]
	{
		get
		{
			if (i > 0 && i < count)
			{
				return list[i].obj;
			}
			return null;
		}
	}

	public int Count => count;

	public BattleIndexedList(int capacity = 15)
	{
		list = new List<LinearListNode>(capacity);
		head = new LinearListNode
		{
			index = 0,
			obj = null
		};
		list.Add(head);
		list.Add(new LinearListNode
		{
			index = 1,
			obj = null
		});
		count = list.Count;
	}

	public void Clear()
	{
		list.Clear();
		head = new LinearListNode
		{
			index = 0,
			obj = null
		};
		list.Add(head);
		list.Add(new LinearListNode
		{
			index = 1,
			obj = null
		});
		count = list.Count;
	}

	public int Add(T obj)
	{
		int num = -1;
		if (head.index != 0)
		{
			num = head.index;
			list[num].obj = obj;
			head.index = list[num].index;
		}
		else
		{
			num = list.Count;
			list.Add(new LinearListNode
			{
				index = num,
				obj = obj
			});
			count = num + 1;
		}
		return num;
	}

	public T TryGetValue(int index)
	{
		if (index > 0 && index < count)
		{
			return list[index].obj;
		}
		return null;
	}

	public T Remove(int pos)
	{
		if (pos > 0 && pos < count)
		{
			T obj = list[pos].obj;
			if (obj == null)
			{
				return obj;
			}
			list[pos].obj = null;
			list[pos].index = head.index;
			head.index = pos;
			return obj;
		}
		return null;
	}

	public T Replace(int pos, T obj)
	{
		if (pos > 0 && pos < count)
		{
			T obj2 = list[pos].obj;
			list[pos].obj = obj;
			return obj2;
		}
		return null;
	}
}
