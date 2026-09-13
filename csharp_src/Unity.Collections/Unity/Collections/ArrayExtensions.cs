using System;

namespace Unity.Collections;

public static class ArrayExtensions
{
	public static int IndexOf<T>(this NativeArray<T> array, T value) where T : struct, IComparable<T>
	{
		for (int i = 0; i != array.Length; i++)
		{
			if (array[i].CompareTo(value) == 0)
			{
				return i;
			}
		}
		return -1;
	}
}
