using System;

namespace VEngine;

public static class StringExtensions
{
	public static int[] IntArrayValue(this string s, string split = ",")
	{
		string[] array = s.Split(new string[1] { split }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 0)
		{
			return Array.ConvertAll(array, int.Parse);
		}
		return new int[0];
	}

	public static ulong ULongValue(this string s)
	{
		ulong.TryParse(s, out var result);
		return result;
	}

	public static int IntValue(this string s)
	{
		int.TryParse(s, out var result);
		return result;
	}

	public static byte ByteValue(this string s)
	{
		byte.TryParse(s, out var result);
		return result;
	}

	public static uint UIntValue(this string s)
	{
		uint.TryParse(s, out var result);
		return result;
	}

	public static string Join<T>(string separator, T[] array)
	{
		string[] array2 = new string[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			T val = array[i];
			array2[i] = val.ToString();
		}
		return string.Join(separator, array2);
	}
}
