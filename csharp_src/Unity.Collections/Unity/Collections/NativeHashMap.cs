using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

[NativeContainer]
[DebuggerTypeProxy(typeof(NativeHashMapDebuggerTypeProxy<, >))]
public struct NativeHashMap<TKey, TValue> : IDisposable where TKey : struct, IEquatable<TKey> where TValue : struct
{
	[NativeContainer]
	[NativeContainerIsAtomicWriteOnly]
	public struct Concurrent
	{
		[NativeDisableUnsafePtrRestriction]
		internal unsafe NativeHashMapData* m_Buffer;

		[NativeSetThreadIndex]
		internal int m_ThreadIndex;

		public unsafe int Capacity => m_Buffer->keyCapacity;

		public unsafe bool TryAdd(TKey key, TValue item)
		{
			return NativeHashMapBase<TKey, TValue>.TryAddAtomic(m_Buffer, key, item, m_ThreadIndex);
		}
	}

	[NativeDisableUnsafePtrRestriction]
	private unsafe NativeHashMapData* m_Buffer;

	private Allocator m_AllocatorLabel;

	public unsafe int Length
	{
		get
		{
			NativeHashMapData* buffer = m_Buffer;
			int* next = (int*)buffer->next;
			int num = 0;
			for (int i = 0; i < 128; i++)
			{
				for (int num2 = buffer->firstFreeTLS[i * 16]; num2 >= 0; num2 = next[num2])
				{
					num++;
				}
			}
			return Math.Min(buffer->keyCapacity, buffer->allocatedIndexLength) - num;
		}
	}

	public unsafe int Capacity
	{
		get
		{
			return m_Buffer->keyCapacity;
		}
		set
		{
			NativeHashMapData.ReallocateHashMap<TKey, TValue>(m_Buffer, value, NativeHashMapData.GetBucketSize(value), m_AllocatorLabel);
		}
	}

	public TValue this[TKey key]
	{
		get
		{
			TryGetValue(key, out var item);
			return item;
		}
	}

	public unsafe bool IsCreated => m_Buffer != null;

	public unsafe NativeHashMap(int capacity, Allocator label)
	{
		m_AllocatorLabel = label;
		NativeHashMapData.AllocateHashMap<TKey, TValue>(capacity, capacity * 2, label, out m_Buffer);
		Clear();
	}

	public unsafe void Clear()
	{
		NativeHashMapBase<TKey, TValue>.Clear(m_Buffer);
	}

	public unsafe bool TryAdd(TKey key, TValue item)
	{
		return NativeHashMapBase<TKey, TValue>.TryAdd(m_Buffer, key, item, isMultiHashMap: false, m_AllocatorLabel);
	}

	public unsafe void Remove(TKey key)
	{
		NativeHashMapBase<TKey, TValue>.Remove(m_Buffer, key, isMultiHashMap: false);
	}

	public unsafe bool TryGetValue(TKey key, out TValue item)
	{
		NativeMultiHashMapIterator<TKey> it;
		return NativeHashMapBase<TKey, TValue>.TryGetFirstValueAtomic(m_Buffer, key, out item, out it);
	}

	public unsafe void Dispose()
	{
		NativeHashMapData.DeallocateHashMap(m_Buffer, m_AllocatorLabel);
		m_Buffer = null;
	}

	public unsafe Concurrent ToConcurrent()
	{
		Concurrent result = default(Concurrent);
		result.m_ThreadIndex = 0;
		result.m_Buffer = m_Buffer;
		return result;
	}

	public unsafe NativeArray<TKey> GetKeyArray(Allocator allocator)
	{
		NativeArray<TKey> result = new NativeArray<TKey>(Length, allocator, NativeArrayOptions.UninitializedMemory);
		NativeHashMapData.GetKeyArray(m_Buffer, result);
		return result;
	}

	public unsafe NativeArray<TValue> GetValueArray(Allocator allocator)
	{
		NativeArray<TValue> result = new NativeArray<TValue>(Length, allocator, NativeArrayOptions.UninitializedMemory);
		NativeHashMapData.GetValueArray(m_Buffer, result);
		return result;
	}
}
