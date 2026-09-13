using System;

namespace Unity.Collections;

public static class NativeHashMapExtensions
{
	public static int Unique<T>(this NativeArray<T> array) where T : struct, IEquatable<T>
	{
		int num = 0;
		int length = array.Length;
		int num2 = num;
		while (++num != length)
		{
			if (!array[num2].Equals(array[num]))
			{
				array[++num2] = array[num];
			}
		}
		return ++num2;
	}

	public static Tuple<NativeArray<TKey>, int> GetUniqueKeyArray<TKey, TValue>(this NativeMultiHashMap<TKey, TValue> hashMap, Allocator allocator) where TKey : struct, IEquatable<TKey>, IComparable<TKey> where TValue : struct
	{
		NativeArray<TKey> keyArray = hashMap.GetKeyArray(allocator);
		keyArray.Sort();
		int item = keyArray.Unique();
		return new Tuple<NativeArray<TKey>, int>(keyArray, item);
	}
}
