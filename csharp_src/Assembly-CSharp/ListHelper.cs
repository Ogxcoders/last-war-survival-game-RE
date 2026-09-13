using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public static class ListHelper
{
	public static void OrderBy<T, TKey>(List<T> list, SelectHandler<T, TKey> handler) where TKey : IComparable, IComparable<TKey>
	{
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count - 1; i++)
		{
			for (int j = i + 1; j < list.Count; j++)
			{
				if (handler(list[i]).CompareTo(handler(list[j])) > 0)
				{
					T value = list[i];
					list[i] = list[j];
					list[j] = value;
				}
			}
		}
	}

	public static void OrderByDescending<T, TKey>(List<T> list, SelectHandler<T, TKey> handler) where TKey : IComparable, IComparable<TKey>
	{
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count - 1; i++)
		{
			for (int j = i + 1; j < list.Count; j++)
			{
				if (handler(list[i]).CompareTo(handler(list[j])) < 0)
				{
					T value = list[i];
					list[i] = list[j];
					list[j] = value;
				}
			}
		}
	}

	public static List<T> RandomSortList<T>(List<T> ListT)
	{
		System.Random random = new System.Random();
		List<T> list = new List<T>();
		foreach (T item in ListT)
		{
			list.Insert(random.Next(list.Count + 1), item);
		}
		Debug.Log(list.Count);
		return list;
	}

	public static T Min<T, TKey>(List<T> list, SelectHandler<T, TKey> handler) where TKey : IComparable, IComparable<TKey>
	{
		T val = default(T);
		val = list[0];
		for (int i = 1; i < list.Count; i++)
		{
			if (list[i] != null && handler(val).CompareTo(handler(list[i])) > 0)
			{
				val = list[i];
			}
		}
		return val;
	}

	public static T Max<T, TKey>(List<T> list, SelectHandler<T, TKey> handler) where TKey : IComparable, IComparable<TKey>
	{
		T val = default(T);
		val = list[0];
		for (int i = 1; i < list.Count; i++)
		{
			if (list[i] != null && handler(val).CompareTo(handler(list[i])) < 0)
			{
				val = list[i];
			}
		}
		return val;
	}

	public static T Find<T>(List<T> list, FindHandler<T> handler)
	{
		T result = default(T);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == null && handler(list[i]))
			{
				return list[i];
			}
		}
		return result;
	}

	public static List<T> FindAll<T>(List<T> list, FindHandler<T> handler)
	{
		List<T> result = new List<T>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == null && handler(list[i]))
			{
				T item = list[i];
				list.Add(item);
			}
		}
		return result;
	}

	public static Dictionary<TKey, List<T>> ToDictionary<TKey, T>(List<T> list, SelectHandler<T, TKey> handler)
	{
		Dictionary<TKey, List<T>> dictionary = new Dictionary<TKey, List<T>>();
		for (int i = 0; i < list.Count; i++)
		{
			if (dictionary.ContainsKey(handler(list[i])))
			{
				dictionary[handler(list[i])].Add(list[i]);
				continue;
			}
			List<T> list2 = new List<T>();
			list2.Add(list[i]);
			dictionary[handler(list[i])] = list2;
		}
		return dictionary;
	}

	public static List<T> GetRandomList<T>(List<T> list, int count = 1) where T : iRandomObject
	{
		if (list == null || list.Count <= count || count <= 0)
		{
			return list;
		}
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			num += list[i].Weight + 1;
		}
		System.Random random = new System.Random(GetRandomSeed());
		List<KeyValuePair<int, int>> list2 = new List<KeyValuePair<int, int>>();
		for (int j = 0; j < list.Count; j++)
		{
			int value = list[j].Weight + 1 + random.Next(0, num);
			list2.Add(new KeyValuePair<int, int>(j, value));
		}
		list2.Sort((KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2) => kvp2.Value - kvp1.Value);
		List<T> list3 = new List<T>();
		for (int num2 = 0; num2 < count; num2++)
		{
			T item = list[list2[num2].Key];
			list3.Add(item);
		}
		return list3;
	}

	public static int GetRandomSeed()
	{
		byte[] array = new byte[4];
		new RNGCryptoServiceProvider().GetBytes(array);
		return BitConverter.ToInt32(array, 0);
	}

	public static T GetRandomItem<T>(List<T> list)
	{
		if (list == null || list.Count == 0)
		{
			return default(T);
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		return list[index];
	}
}
