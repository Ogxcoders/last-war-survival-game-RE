using System;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

internal struct NativeHashMapBase<TKey, TValue> where TKey : struct, IEquatable<TKey> where TValue : struct
{
	public unsafe static void Clear(NativeHashMapData* data)
	{
		int* buckets = (int*)data->buckets;
		for (int i = 0; i <= data->bucketCapacityMask; i++)
		{
			buckets[i] = -1;
		}
		int* next = (int*)data->next;
		for (int j = 0; j < data->keyCapacity; j++)
		{
			next[j] = -1;
		}
		for (int k = 0; k < 128; k++)
		{
			data->firstFreeTLS[k * 16] = -1;
		}
		data->allocatedIndexLength = 0;
	}

	private unsafe static int AllocEntry(NativeHashMapData* data, int threadIndex)
	{
		int* next = (int*)data->next;
		int num;
		do
		{
			num = data->firstFreeTLS[threadIndex * 16];
			if (num < 0)
			{
				Interlocked.Exchange(ref data->firstFreeTLS[threadIndex * 16], -2);
				if (data->allocatedIndexLength < data->keyCapacity)
				{
					num = Interlocked.Add(ref data->allocatedIndexLength, 16) - 16;
					if (num < data->keyCapacity - 1)
					{
						int num2 = Math.Min(16, data->keyCapacity - num);
						for (int i = 1; i < num2; i++)
						{
							next[num + i] = num + i + 1;
						}
						next[num + num2 - 1] = -1;
						next[num] = -1;
						Interlocked.Exchange(ref data->firstFreeTLS[threadIndex * 16], num + 1);
						return num;
					}
					if (num == data->keyCapacity - 1)
					{
						Interlocked.Exchange(ref data->firstFreeTLS[threadIndex * 16], -1);
						return num;
					}
				}
				Interlocked.Exchange(ref data->firstFreeTLS[threadIndex * 16], -1);
				bool flag = true;
				while (flag)
				{
					flag = false;
					for (int num3 = (threadIndex + 1) % 128; num3 != threadIndex; num3 = (num3 + 1) % 128)
					{
						do
						{
							num = data->firstFreeTLS[num3 * 16];
						}
						while (num >= 0 && Interlocked.CompareExchange(ref data->firstFreeTLS[num3 * 16], next[num], num) != num);
						if (num == -2)
						{
							flag = true;
						}
						else if (num >= 0)
						{
							next[num] = -1;
							return num;
						}
					}
				}
				throw new InvalidOperationException("HashMap is full");
			}
			if (num >= data->keyCapacity)
			{
				throw new InvalidOperationException($"nextPtr idx {num} beyond capacity {data->keyCapacity}");
			}
		}
		while (Interlocked.CompareExchange(ref data->firstFreeTLS[threadIndex * 16], next[num], num) != num);
		next[num] = -1;
		return num;
	}

	public unsafe static bool TryAddAtomic(NativeHashMapData* data, TKey key, TValue item, int threadIndex)
	{
		if (TryGetFirstValueAtomic(data, key, out var item2, out var it))
		{
			return false;
		}
		int num = AllocEntry(data, threadIndex);
		UnsafeUtility.WriteArrayElement(data->keys, num, key);
		UnsafeUtility.WriteArrayElement(data->values, num, item);
		int num2 = key.GetHashCode() & data->bucketCapacityMask;
		int* buckets = (int*)data->buckets;
		if (Interlocked.CompareExchange(ref buckets[num2], num, -1) != -1)
		{
			int* next = (int*)data->next;
			do
			{
				next[num] = buckets[num2];
				if (TryGetFirstValueAtomic(data, key, out item2, out it))
				{
					do
					{
						next[num] = data->firstFreeTLS[threadIndex * 16];
					}
					while (Interlocked.CompareExchange(ref data->firstFreeTLS[threadIndex * 16], num, next[num]) != next[num]);
					return false;
				}
			}
			while (Interlocked.CompareExchange(ref buckets[num2], num, next[num]) != next[num]);
		}
		return true;
	}

	public unsafe static void AddAtomicMulti(NativeHashMapData* data, TKey key, TValue item, int threadIndex)
	{
		int num = AllocEntry(data, threadIndex);
		UnsafeUtility.WriteArrayElement(data->keys, num, key);
		UnsafeUtility.WriteArrayElement(data->values, num, item);
		int num2 = key.GetHashCode() & data->bucketCapacityMask;
		int* buckets = (int*)data->buckets;
		int* next = (int*)data->next;
		int num3;
		do
		{
			num3 = (next[num] = buckets[num2]);
		}
		while (Interlocked.CompareExchange(ref buckets[num2], num, num3) != num3);
	}

	public unsafe static bool TryAdd(NativeHashMapData* data, TKey key, TValue item, bool isMultiHashMap, Allocator allocation)
	{
		if (!isMultiHashMap && TryGetFirstValueAtomic(data, key, out var _, out var _))
		{
			return false;
		}
		int num;
		int* next;
		if (data->allocatedIndexLength >= data->keyCapacity && *data->firstFreeTLS < 0)
		{
			for (int i = 1; i < 128; i++)
			{
				if (data->firstFreeTLS[i * 16] >= 0)
				{
					num = data->firstFreeTLS[i * 16];
					next = (int*)data->next;
					data->firstFreeTLS[i * 16] = next[num];
					next[num] = -1;
					*data->firstFreeTLS = num;
					break;
				}
			}
			if (*data->firstFreeTLS < 0)
			{
				int num2 = NativeHashMapData.GrowCapacity(data->keyCapacity);
				NativeHashMapData.ReallocateHashMap<TKey, TValue>(data, num2, NativeHashMapData.GetBucketSize(num2), allocation);
			}
		}
		num = *data->firstFreeTLS;
		if (num >= 0)
		{
			int* firstFreeTLS = data->firstFreeTLS;
			*firstFreeTLS = ((int*)data->next)[num];
		}
		else
		{
			num = data->allocatedIndexLength++;
		}
		if (num < 0 || num >= data->keyCapacity)
		{
			throw new InvalidOperationException("Internal HashMap error");
		}
		UnsafeUtility.WriteArrayElement(data->keys, num, key);
		UnsafeUtility.WriteArrayElement(data->values, num, item);
		int num3 = key.GetHashCode() & data->bucketCapacityMask;
		int* buckets = (int*)data->buckets;
		next = (int*)data->next;
		next[num] = buckets[num3];
		buckets[num3] = num;
		return true;
	}

	public unsafe static void Remove(NativeHashMapData* data, TKey key, bool isMultiHashMap)
	{
		int* buckets = (int*)data->buckets;
		int* next = (int*)data->next;
		int num = key.GetHashCode() & data->bucketCapacityMask;
		int num2 = -1;
		int num3 = buckets[num];
		while (num3 >= 0 && num3 < data->keyCapacity)
		{
			if (UnsafeUtility.ReadArrayElement<TKey>(data->keys, num3).Equals(key))
			{
				if (num2 < 0)
				{
					buckets[num] = next[num3];
				}
				else
				{
					next[num2] = next[num3];
				}
				int num4 = next[num3];
				next[num3] = *data->firstFreeTLS;
				*data->firstFreeTLS = num3;
				num3 = num4;
				if (!isMultiHashMap)
				{
					break;
				}
			}
			else
			{
				num2 = num3;
				num3 = next[num3];
			}
		}
	}

	public unsafe static void Remove(NativeHashMapData* data, NativeMultiHashMapIterator<TKey> it)
	{
		int* buckets = (int*)data->buckets;
		int* next = (int*)data->next;
		int num = it.key.GetHashCode() & data->bucketCapacityMask;
		int num2 = buckets[num];
		if (num2 == it.EntryIndex)
		{
			buckets[num] = next[num2];
		}
		else
		{
			while (num2 >= 0 && next[num2] != it.EntryIndex)
			{
				num2 = next[num2];
			}
			if (num2 < 0)
			{
				throw new InvalidOperationException("Invalid iterator passed to HashMap remove");
			}
			next[num2] = next[it.EntryIndex];
		}
		next[it.EntryIndex] = *data->firstFreeTLS;
		int* firstFreeTLS = data->firstFreeTLS;
		*firstFreeTLS = it.EntryIndex;
	}

	public unsafe static bool TryGetFirstValueAtomic(NativeHashMapData* data, TKey key, out TValue item, out NativeMultiHashMapIterator<TKey> it)
	{
		it.key = key;
		if (data->allocatedIndexLength <= 0)
		{
			it.EntryIndex = (it.NextEntryIndex = -1);
			item = default(TValue);
			return false;
		}
		int* buckets = (int*)data->buckets;
		int num = key.GetHashCode() & data->bucketCapacityMask;
		it.EntryIndex = (it.NextEntryIndex = buckets[num]);
		return TryGetNextValueAtomic(data, out item, ref it);
	}

	public unsafe static bool TryGetNextValueAtomic(NativeHashMapData* data, out TValue item, ref NativeMultiHashMapIterator<TKey> it)
	{
		int num = it.NextEntryIndex;
		it.NextEntryIndex = -1;
		it.EntryIndex = -1;
		item = default(TValue);
		if (num < 0 || num >= data->keyCapacity)
		{
			return false;
		}
		int* next = (int*)data->next;
		while (!UnsafeUtility.ReadArrayElement<TKey>(data->keys, num).Equals(it.key))
		{
			num = next[num];
			if (num < 0 || num >= data->keyCapacity)
			{
				return false;
			}
		}
		it.NextEntryIndex = next[num];
		it.EntryIndex = num;
		item = UnsafeUtility.ReadArrayElement<TValue>(data->values, num);
		return true;
	}

	public unsafe static bool SetValue(NativeHashMapData* data, ref NativeMultiHashMapIterator<TKey> it, ref TValue item)
	{
		int entryIndex = it.EntryIndex;
		if (entryIndex < 0 || entryIndex >= data->keyCapacity)
		{
			return false;
		}
		UnsafeUtility.WriteArrayElement(data->values, entryIndex, item);
		return true;
	}
}
