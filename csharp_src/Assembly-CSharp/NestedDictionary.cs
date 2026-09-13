using System;
using System.Collections.Generic;

public class NestedDictionary<TK1, TK2, TValue>
{
	protected Dictionary<TK1, Dictionary<TK2, TValue>> mDict = new Dictionary<TK1, Dictionary<TK2, TValue>>();

	public int Count => mDict.Count;

	public TValue this[TK1 key1, TK2 key2]
	{
		get
		{
			if (mDict.TryGetValue(key1, out var value) && value.TryGetValue(key2, out var value2))
			{
				return value2;
			}
			return default(TValue);
		}
		set
		{
			if (!mDict.ContainsKey(key1))
			{
				mDict[key1] = new Dictionary<TK2, TValue>();
			}
			mDict[key1][key2] = value;
		}
	}

	public void Clear()
	{
		mDict.Clear();
	}

	public void Add(TK1 key1, TK2 key2, TValue value)
	{
		if (!mDict.ContainsKey(key1))
		{
			mDict[key1] = new Dictionary<TK2, TValue>();
		}
		mDict[key1][key2] = value;
	}

	public bool Remove(TK1 key1, TK2 key2)
	{
		bool flag = false;
		if (mDict.TryGetValue(key1, out var value))
		{
			flag = value.Remove(key2);
			if (flag && value.Count == 0)
			{
				mDict.Remove(key1);
			}
		}
		return flag;
	}

	public bool ContainsKey(TK1 key1, TK2 key2)
	{
		if (mDict.TryGetValue(key1, out var value))
		{
			return value.ContainsKey(key2);
		}
		return false;
	}

	public bool TryGetValue(TK1 key1, TK2 key2, out TValue value)
	{
		if (mDict.TryGetValue(key1, out var value2))
		{
			return value2.TryGetValue(key2, out value);
		}
		value = default(TValue);
		return false;
	}

	public int CloneTo(NestedDictionary<TK1, TK2, TValue> target)
	{
		if (target == null)
		{
			return 0;
		}
		int num = 0;
		foreach (KeyValuePair<TK1, Dictionary<TK2, TValue>> item in mDict)
		{
			foreach (KeyValuePair<TK2, TValue> item2 in item.Value)
			{
				target[item.Key, item2.Key] = item2.Value;
				num++;
			}
		}
		return num;
	}

	public void ForEach(Action<TK1, TK2, TValue> action)
	{
		if (action == null)
		{
			return;
		}
		foreach (KeyValuePair<TK1, Dictionary<TK2, TValue>> item in mDict)
		{
			Dictionary<TK2, TValue> value = item.Value;
			if (value == null)
			{
				continue;
			}
			foreach (KeyValuePair<TK2, TValue> item2 in value)
			{
				TValue value2 = item2.Value;
				if (value2 != null)
				{
					action(item.Key, item2.Key, value2);
				}
			}
		}
	}
}
