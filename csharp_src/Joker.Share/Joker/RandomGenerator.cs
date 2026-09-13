using System;
using System.Collections.Generic;

namespace Joker;

public static class RandomGenerator
{
	[ThreadStatic]
	private static Random random;

	private static Random GetRandom()
	{
		if (random != null)
		{
			return random;
		}
		random = new Random(Guid.NewGuid().GetHashCode() ^ Environment.TickCount);
		return random;
	}

	public static ulong RandUInt64()
	{
		int num = RandInt32();
		int num2 = RandInt32();
		return (ulong)(((long)num << 32) | (uint)num2);
	}

	public static int RandInt32()
	{
		return GetRandom().Next();
	}

	public static uint RandUInt32()
	{
		return (uint)GetRandom().Next();
	}

	public static long RandInt64()
	{
		uint num = RandUInt32();
		uint num2 = RandUInt32();
		return (long)(((ulong)num << 32) | num2);
	}

	public static int RandomNumber(int lower, int upper)
	{
		return GetRandom().Next(lower, upper);
	}

	public static bool RandomBool()
	{
		return GetRandom().Next(2) == 0;
	}

	public static T RandomArray<T>(T[] array)
	{
		return array[RandomNumber(0, array.Length)];
	}

	public static T RandomArray<T>(List<T> array)
	{
		return array[RandomNumber(0, array.Count)];
	}

	public static void BreakRank<T>(List<T> arr)
	{
		if (arr != null && arr.Count >= 2)
		{
			for (int i = 0; i < arr.Count; i++)
			{
				int num = GetRandom().Next(0, arr.Count);
				int index = num;
				int index2 = i;
				T val = arr[i];
				T val2 = arr[num];
				T val3 = (arr[index] = val);
				val3 = (arr[index2] = val2);
			}
		}
	}

	public static float RandFloat01()
	{
		return (float)RandomNumber(0, 1000000) / 1000000f;
	}
}
