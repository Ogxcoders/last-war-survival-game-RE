using System;
using System.Collections.Generic;

public static class ArrayHelper
{
	public static void OrderBy<T, TKey>(T[] arr, SelectHandler<T, TKey> handler) where TKey : IComparable, IComparable<TKey>
	{
		for (int i = 0; i < arr.Length - 1; i++)
		{
			for (int j = i + 1; j < arr.Length; j++)
			{
				if (handler(arr[i]).CompareTo(handler(arr[j])) > 0)
				{
					T val = arr[i];
					arr[i] = arr[j];
					arr[j] = val;
				}
			}
		}
	}

	public static void OrderByDescending<T, TKey>(T[] arr, SelectHandler<T, TKey> handler) where TKey : IComparable, IComparable<TKey>
	{
		for (int i = 0; i < arr.Length - 1; i++)
		{
			for (int j = i + 1; j < arr.Length; j++)
			{
				if (handler(arr[i]).CompareTo(handler(arr[j])) < 0)
				{
					T val = arr[i];
					arr[i] = arr[j];
					arr[j] = val;
				}
			}
		}
	}

	public static T Min<T, TKey>(T[] arr, SelectHandler<T, TKey> handler) where TKey : IComparable, IComparable<TKey>
	{
		T val = default(T);
		val = arr[0];
		for (int i = 1; i < arr.Length; i++)
		{
			if (handler(val).CompareTo(handler(arr[i])) > 0)
			{
				val = arr[i];
			}
		}
		return val;
	}

	public static T Max<T, TKey>(T[] arr, SelectHandler<T, TKey> handler) where TKey : IComparable, IComparable<TKey>
	{
		T val = default(T);
		val = arr[0];
		for (int i = 1; i < arr.Length; i++)
		{
			if (arr[i] != null && handler(val).CompareTo(handler(arr[i])) < 0)
			{
				val = arr[i];
			}
		}
		return val;
	}

	public static TKey[] Select<T, TKey>(T[] arr, SelectHandler<T, TKey> handler)
	{
		TKey[] array = new TKey[arr.Length];
		for (int i = 0; i < arr.Length; i++)
		{
			array[i] = handler(arr[i]);
		}
		return array;
	}

	public static T Find<T>(T[] arr, FindHandler<T> handler)
	{
		T result = default(T);
		for (int i = 0; i < arr.Length; i++)
		{
			if (handler(arr[i]))
			{
				return arr[i];
			}
		}
		return result;
	}

	public static T[] FindAll<T>(T[] arr, FindHandler<T> handler)
	{
		List<T> list = new List<T>();
		for (int i = 0; i < arr.Length; i++)
		{
			if (handler(arr[i]))
			{
				T item = arr[i];
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	public static T[] Disrupted<T>(T[] arr)
	{
		Random random = new Random(ListHelper.GetRandomSeed());
		for (int i = 0; i < arr.Length; i++)
		{
			int num = random.Next(0, arr.Length);
			if (num != i)
			{
				T val = arr[i];
				arr[i] = arr[num];
				arr[num] = val;
			}
		}
		return arr;
	}
}
