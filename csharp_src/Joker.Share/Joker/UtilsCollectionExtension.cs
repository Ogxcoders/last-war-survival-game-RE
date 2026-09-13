using System.Collections.Generic;

namespace Joker;

public static class UtilsCollectionExtension
{
	public static bool RemoveSwapBack<T>(this List<T> data, T obj)
	{
		int num = data.IndexOf(obj);
		if (num != -1)
		{
			data.RemoveSwapBackAt(num);
		}
		return num != -1;
	}

	public static void RemoveSwapBackAt<T>(this List<T> data, int idx)
	{
		int index = data.Count - 1;
		data[idx] = data[index];
		data.RemoveAt(index);
	}

	public static V GetValue<K, V>(this Dictionary<K, V> dict, K key, V def)
	{
		if (!dict.TryGetValue(key, out var value))
		{
			return def;
		}
		return value;
	}

	public static void AddRange<K, V>(this Dictionary<K, V> self, IEnumerable<(K, V)> datas)
	{
		foreach (var data in datas)
		{
			self.Add(data.Item1, data.Item2);
		}
	}
}
