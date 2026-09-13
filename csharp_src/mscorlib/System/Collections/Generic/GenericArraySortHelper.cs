using System.Threading;

namespace System.Collections.Generic;

internal class GenericArraySortHelper<T> where T : IComparable<T>
{
	public void Sort(T[] keys, int index, int length, IComparer<T> comparer)
	{
		try
		{
			if (comparer == null || comparer == Comparer<T>.Default)
			{
				IntrospectiveSort(keys, index, length);
			}
			else
			{
				ArraySortHelper<T>.IntrospectiveSort(keys, index, length, comparer.Compare);
			}
		}
		catch (IndexOutOfRangeException)
		{
			IntrospectiveSortUtilities.ThrowOrIgnoreBadComparer(comparer);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new InvalidOperationException("Failed to compare two elements in the array.", innerException);
		}
	}

	public int BinarySearch(T[] array, int index, int length, T value, IComparer<T> comparer)
	{
		try
		{
			if (comparer == null || comparer == Comparer<T>.Default)
			{
				return BinarySearch(array, index, length, value);
			}
			return ArraySortHelper<T>.InternalBinarySearch(array, index, length, value, comparer);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new InvalidOperationException("Failed to compare two elements in the array.", innerException);
		}
	}

	private static int BinarySearch(T[] array, int index, int length, T value)
	{
		int num = index;
		int num2 = index + length - 1;
		while (num <= num2)
		{
			int num3 = num + (num2 - num >> 1);
			int num4 = ((array[num3] != null) ? array[num3].CompareTo(value) : ((value != null) ? (-1) : 0));
			if (num4 == 0)
			{
				return num3;
			}
			if (num4 < 0)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3 - 1;
			}
		}
		return ~num;
	}

	private static void SwapIfGreaterWithItems(T[] keys, int a, int b)
	{
		if (a != b && keys[a] != null)
		{
			ref readonly T reference = ref keys[a];
			T other = keys[b];
			if (reference.CompareTo(other) > 0)
			{
				T val = keys[a];
				keys[a] = keys[b];
				keys[b] = val;
			}
		}
	}

	private static void Swap(T[] a, int i, int j)
	{
		if (i != j)
		{
			T val = a[i];
			a[i] = a[j];
			a[j] = val;
		}
	}

	internal static void IntrospectiveSort(T[] keys, int left, int length)
	{
		if (length >= 2)
		{
			IntroSort(keys, left, length + left - 1, 2 * IntrospectiveSortUtilities.FloorLog2PlusOne(length));
		}
	}

	private static void IntroSort(T[] keys, int lo, int hi, int depthLimit)
	{
		while (hi > lo)
		{
			int num = hi - lo + 1;
			if (num <= 16)
			{
				switch (num)
				{
				case 1:
					break;
				case 2:
					SwapIfGreaterWithItems(keys, lo, hi);
					break;
				case 3:
					SwapIfGreaterWithItems(keys, lo, hi - 1);
					SwapIfGreaterWithItems(keys, lo, hi);
					SwapIfGreaterWithItems(keys, hi - 1, hi);
					break;
				default:
					InsertionSort(keys, lo, hi);
					break;
				}
				break;
			}
			if (depthLimit == 0)
			{
				Heapsort(keys, lo, hi);
				break;
			}
			depthLimit--;
			int num2 = PickPivotAndPartition(keys, lo, hi);
			IntroSort(keys, num2 + 1, hi, depthLimit);
			hi = num2 - 1;
		}
	}

	private static int PickPivotAndPartition(T[] keys, int lo, int hi)
	{
		int num = lo + (hi - lo) / 2;
		SwapIfGreaterWithItems(keys, lo, num);
		SwapIfGreaterWithItems(keys, lo, hi);
		SwapIfGreaterWithItems(keys, num, hi);
		T val = keys[num];
		Swap(keys, num, hi - 1);
		int num2 = lo;
		int num3 = hi - 1;
		while (num2 < num3)
		{
			if (val == null)
			{
				while (num2 < hi - 1 && keys[++num2] == null)
				{
				}
				while (num3 > lo && keys[--num3] != null)
				{
				}
			}
			else
			{
				T other;
				do
				{
					other = keys[++num2];
				}
				while (val.CompareTo(other) > 0);
				T other2;
				do
				{
					other2 = keys[--num3];
				}
				while (val.CompareTo(other2) < 0);
			}
			if (num2 >= num3)
			{
				break;
			}
			Swap(keys, num2, num3);
		}
		Swap(keys, num2, hi - 1);
		return num2;
	}

	private static void Heapsort(T[] keys, int lo, int hi)
	{
		int num = hi - lo + 1;
		for (int num2 = num / 2; num2 >= 1; num2--)
		{
			DownHeap(keys, num2, num, lo);
		}
		for (int num3 = num; num3 > 1; num3--)
		{
			Swap(keys, lo, lo + num3 - 1);
			DownHeap(keys, 1, num3 - 1, lo);
		}
	}

	private static void DownHeap(T[] keys, int i, int n, int lo)
	{
		T val = keys[lo + i - 1];
		while (i <= n / 2)
		{
			int num = 2 * i;
			if (num < n)
			{
				if (keys[lo + num - 1] != null)
				{
					ref readonly T reference = ref keys[lo + num - 1];
					T other = keys[lo + num];
					if (reference.CompareTo(other) >= 0)
					{
						goto IL_0053;
					}
				}
				num++;
			}
			goto IL_0053;
			IL_0053:
			if (keys[lo + num - 1] == null || keys[lo + num - 1].CompareTo(val) < 0)
			{
				break;
			}
			keys[lo + i - 1] = keys[lo + num - 1];
			i = num;
		}
		keys[lo + i - 1] = val;
	}

	private static void InsertionSort(T[] keys, int lo, int hi)
	{
		for (int i = lo; i < hi; i++)
		{
			int num = i;
			T val = keys[i + 1];
			while (num >= lo && (val == null || val.CompareTo(keys[num]) < 0))
			{
				keys[num + 1] = keys[num];
				num--;
			}
			keys[num + 1] = val;
		}
	}
}
internal class GenericArraySortHelper<TKey, TValue> where TKey : IComparable<TKey>
{
	public void Sort(TKey[] keys, TValue[] values, int index, int length, IComparer<TKey> comparer)
	{
		try
		{
			if (comparer == null || comparer == Comparer<TKey>.Default)
			{
				IntrospectiveSort(keys, values, index, length);
			}
			else
			{
				ArraySortHelper<TKey, TValue>.IntrospectiveSort(keys, values, index, length, comparer);
			}
		}
		catch (IndexOutOfRangeException)
		{
			IntrospectiveSortUtilities.ThrowOrIgnoreBadComparer(comparer);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new InvalidOperationException("Failed to compare two elements in the array.", innerException);
		}
	}

	private static void SwapIfGreaterWithItems(TKey[] keys, TValue[] values, int a, int b)
	{
		if (a != b && keys[a] != null)
		{
			ref readonly TKey reference = ref keys[a];
			TKey other = keys[b];
			if (reference.CompareTo(other) > 0)
			{
				TKey val = keys[a];
				keys[a] = keys[b];
				keys[b] = val;
				TValue val2 = values[a];
				values[a] = values[b];
				values[b] = val2;
			}
		}
	}

	private static void Swap(TKey[] keys, TValue[] values, int i, int j)
	{
		if (i != j)
		{
			TKey val = keys[i];
			keys[i] = keys[j];
			keys[j] = val;
			TValue val2 = values[i];
			values[i] = values[j];
			values[j] = val2;
		}
	}

	internal static void IntrospectiveSort(TKey[] keys, TValue[] values, int left, int length)
	{
		if (length >= 2)
		{
			IntroSort(keys, values, left, length + left - 1, 2 * IntrospectiveSortUtilities.FloorLog2PlusOne(length));
		}
	}

	private static void IntroSort(TKey[] keys, TValue[] values, int lo, int hi, int depthLimit)
	{
		while (hi > lo)
		{
			int num = hi - lo + 1;
			if (num <= 16)
			{
				switch (num)
				{
				case 1:
					break;
				case 2:
					SwapIfGreaterWithItems(keys, values, lo, hi);
					break;
				case 3:
					SwapIfGreaterWithItems(keys, values, lo, hi - 1);
					SwapIfGreaterWithItems(keys, values, lo, hi);
					SwapIfGreaterWithItems(keys, values, hi - 1, hi);
					break;
				default:
					InsertionSort(keys, values, lo, hi);
					break;
				}
				break;
			}
			if (depthLimit == 0)
			{
				Heapsort(keys, values, lo, hi);
				break;
			}
			depthLimit--;
			int num2 = PickPivotAndPartition(keys, values, lo, hi);
			IntroSort(keys, values, num2 + 1, hi, depthLimit);
			hi = num2 - 1;
		}
	}

	private static int PickPivotAndPartition(TKey[] keys, TValue[] values, int lo, int hi)
	{
		int num = lo + (hi - lo) / 2;
		SwapIfGreaterWithItems(keys, values, lo, num);
		SwapIfGreaterWithItems(keys, values, lo, hi);
		SwapIfGreaterWithItems(keys, values, num, hi);
		TKey val = keys[num];
		Swap(keys, values, num, hi - 1);
		int num2 = lo;
		int num3 = hi - 1;
		while (num2 < num3)
		{
			if (val == null)
			{
				while (num2 < hi - 1 && keys[++num2] == null)
				{
				}
				while (num3 > lo && keys[--num3] != null)
				{
				}
			}
			else
			{
				TKey other;
				do
				{
					other = keys[++num2];
				}
				while (val.CompareTo(other) > 0);
				TKey other2;
				do
				{
					other2 = keys[--num3];
				}
				while (val.CompareTo(other2) < 0);
			}
			if (num2 >= num3)
			{
				break;
			}
			Swap(keys, values, num2, num3);
		}
		Swap(keys, values, num2, hi - 1);
		return num2;
	}

	private static void Heapsort(TKey[] keys, TValue[] values, int lo, int hi)
	{
		int num = hi - lo + 1;
		for (int num2 = num / 2; num2 >= 1; num2--)
		{
			DownHeap(keys, values, num2, num, lo);
		}
		for (int num3 = num; num3 > 1; num3--)
		{
			Swap(keys, values, lo, lo + num3 - 1);
			DownHeap(keys, values, 1, num3 - 1, lo);
		}
	}

	private static void DownHeap(TKey[] keys, TValue[] values, int i, int n, int lo)
	{
		TKey val = keys[lo + i - 1];
		TValue val2 = values[lo + i - 1];
		while (i <= n / 2)
		{
			int num = 2 * i;
			if (num < n)
			{
				if (keys[lo + num - 1] != null)
				{
					ref readonly TKey reference = ref keys[lo + num - 1];
					TKey other = keys[lo + num];
					if (reference.CompareTo(other) >= 0)
					{
						goto IL_0064;
					}
				}
				num++;
			}
			goto IL_0064;
			IL_0064:
			if (keys[lo + num - 1] == null || keys[lo + num - 1].CompareTo(val) < 0)
			{
				break;
			}
			keys[lo + i - 1] = keys[lo + num - 1];
			values[lo + i - 1] = values[lo + num - 1];
			i = num;
		}
		keys[lo + i - 1] = val;
		values[lo + i - 1] = val2;
	}

	private static void InsertionSort(TKey[] keys, TValue[] values, int lo, int hi)
	{
		for (int i = lo; i < hi; i++)
		{
			int num = i;
			TKey val = keys[i + 1];
			TValue val2 = values[i + 1];
			while (num >= lo && (val == null || val.CompareTo(keys[num]) < 0))
			{
				keys[num + 1] = keys[num];
				values[num + 1] = values[num];
				num--;
			}
			keys[num + 1] = val;
			values[num + 1] = val2;
		}
	}
}
