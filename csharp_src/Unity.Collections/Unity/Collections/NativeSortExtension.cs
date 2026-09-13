using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Collections;

public static class NativeSortExtension
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct DefaultComparer<T> : IComparer<T> where T : IComparable<T>
	{
		public int Compare(T x, T y)
		{
			return x.CompareTo(y);
		}
	}

	private const int k_IntrosortSizeThreshold = 16;

	public static void Sort<T>(this NativeArray<T> array) where T : struct, IComparable<T>
	{
		array.Sort(default(DefaultComparer<T>));
	}

	public unsafe static void Sort<T, U>(this NativeArray<T> array, U comp) where T : struct where U : IComparer<T>
	{
		IntroSort<T, U>(array.GetUnsafePtr(), 0, array.Length - 1, 2 * math_2.log2_floor(array.Length), comp);
	}

	public static void Sort<T>(this NativeSlice<T> slice) where T : struct, IComparable<T>
	{
		slice.Sort(default(DefaultComparer<T>));
	}

	public unsafe static void Sort<T, U>(this NativeSlice<T> slice, U comp) where T : struct where U : IComparer<T>
	{
		IntroSort<T, U>(slice.GetUnsafePtr(), 0, slice.Length - 1, 2 * math_2.log2_floor(slice.Length), comp);
	}

	private unsafe static void IntroSort<T, U>(void* array, int lo, int hi, int depth, U comp) where T : struct where U : IComparer<T>
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
					SwapIfGreaterWithItems<T, U>(array, lo, hi, comp);
					break;
				case 3:
					SwapIfGreaterWithItems<T, U>(array, lo, hi - 1, comp);
					SwapIfGreaterWithItems<T, U>(array, lo, hi, comp);
					SwapIfGreaterWithItems<T, U>(array, hi - 1, hi, comp);
					break;
				default:
					InsertionSort<T, U>(array, lo, hi, comp);
					break;
				}
				break;
			}
			if (depth == 0)
			{
				HeapSort<T, U>(array, lo, hi, comp);
				break;
			}
			depth--;
			int num2 = Partition<T, U>(array, lo, hi, comp);
			IntroSort<T, U>(array, num2 + 1, hi, depth, comp);
			hi = num2 - 1;
		}
	}

	private unsafe static void InsertionSort<T, U>(void* array, int lo, int hi, U comp) where T : struct where U : IComparer<T>
	{
		for (int i = lo; i < hi; i++)
		{
			int num = i;
			T val = UnsafeUtility.ReadArrayElement<T>(array, i + 1);
			while (num >= lo && comp.Compare(val, UnsafeUtility.ReadArrayElement<T>(array, num)) < 0)
			{
				UnsafeUtility.WriteArrayElement(array, num + 1, UnsafeUtility.ReadArrayElement<T>(array, num));
				num--;
			}
			UnsafeUtility.WriteArrayElement(array, num + 1, val);
		}
	}

	private unsafe static int Partition<T, U>(void* array, int lo, int hi, U comp) where T : struct where U : IComparer<T>
	{
		int num = lo + (hi - lo) / 2;
		SwapIfGreaterWithItems<T, U>(array, lo, num, comp);
		SwapIfGreaterWithItems<T, U>(array, lo, hi, comp);
		SwapIfGreaterWithItems<T, U>(array, num, hi, comp);
		T x = UnsafeUtility.ReadArrayElement<T>(array, num);
		Swap<T>(array, num, hi - 1);
		int num2 = lo;
		int num3 = hi - 1;
		while (num2 < num3)
		{
			T y;
			do
			{
				y = UnsafeUtility.ReadArrayElement<T>(array, ++num2);
			}
			while (comp.Compare(x, y) > 0);
			T y2;
			do
			{
				y2 = UnsafeUtility.ReadArrayElement<T>(array, --num3);
			}
			while (comp.Compare(x, y2) < 0);
			if (num2 >= num3)
			{
				break;
			}
			Swap<T>(array, num2, num3);
		}
		Swap<T>(array, num2, hi - 1);
		return num2;
	}

	private unsafe static void HeapSort<T, U>(void* array, int lo, int hi, U comp) where T : struct where U : IComparer<T>
	{
		int num = hi - lo + 1;
		for (int num2 = num / 2; num2 >= 1; num2--)
		{
			Heapify<T, U>(array, num2, num, lo, comp);
		}
		for (int num3 = num; num3 > 1; num3--)
		{
			Swap<T>(array, lo, lo + num3 - 1);
			Heapify<T, U>(array, 1, num3 - 1, lo, comp);
		}
	}

	private unsafe static void Heapify<T, U>(void* array, int i, int n, int lo, U comp) where T : struct where U : IComparer<T>
	{
		T val = UnsafeUtility.ReadArrayElement<T>(array, lo + i - 1);
		while (i <= n / 2)
		{
			int num = 2 * i;
			if (num < n)
			{
				T x = UnsafeUtility.ReadArrayElement<T>(array, lo + num - 1);
				T y = UnsafeUtility.ReadArrayElement<T>(array, lo + num);
				if (comp.Compare(x, y) < 0)
				{
					num++;
				}
			}
			T x2 = UnsafeUtility.ReadArrayElement<T>(array, lo + num - 1);
			if (comp.Compare(x2, val) < 0)
			{
				break;
			}
			UnsafeUtility.WriteArrayElement(array, lo + i - 1, UnsafeUtility.ReadArrayElement<T>(array, lo + num - 1));
			i = num;
		}
		UnsafeUtility.WriteArrayElement(array, lo + i - 1, val);
	}

	private unsafe static void Swap<T>(void* array, int lhs, int rhs) where T : struct
	{
		T value = UnsafeUtility.ReadArrayElement<T>(array, lhs);
		UnsafeUtility.WriteArrayElement(array, lhs, UnsafeUtility.ReadArrayElement<T>(array, rhs));
		UnsafeUtility.WriteArrayElement(array, rhs, value);
	}

	private unsafe static void SwapIfGreaterWithItems<T, U>(void* array, int lhs, int rhs, U comp) where T : struct where U : IComparer<T>
	{
		if (lhs != rhs && comp.Compare(UnsafeUtility.ReadArrayElement<T>(array, lhs), UnsafeUtility.ReadArrayElement<T>(array, rhs)) > 0)
		{
			Swap<T>(array, lhs, rhs);
		}
	}
}
