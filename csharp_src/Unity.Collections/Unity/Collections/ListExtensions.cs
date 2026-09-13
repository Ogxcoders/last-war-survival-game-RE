using System.Collections.Generic;

namespace Unity.Collections;

public static class ListExtensions
{
	public static void RemoveAtSwapBack<T>(this List<T> list, int index)
	{
		int index2 = list.Count - 1;
		list[index] = list[index2];
		list.RemoveAt(index2);
	}

	public static void RemoveSwapBack<T>(this List<T> list, T value)
	{
		int num = list.IndexOf(value);
		if (num != -1)
		{
			list.RemoveAtSwapBack(num);
		}
	}
}
