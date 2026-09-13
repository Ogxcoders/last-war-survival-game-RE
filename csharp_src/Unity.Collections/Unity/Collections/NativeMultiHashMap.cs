using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

[NativeContainer]
[DebuggerTypeProxy(typeof(NativeMultiHashMapDebuggerTypeProxy<, >))]
public struct NativeMultiHashMap<TKey, TValue> : IDisposable where TKey : struct, IEquatable<TKey> where TValue : struct
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

		public unsafe void Add(TKey key, TValue item)
		{
			NativeHashMapBase<TKey, TValue>.AddAtomicMulti(m_Buffer, key, item, m_ThreadIndex);
		}
	}

	[NativeDisableUnsafePtrRestriction]
	internal unsafe NativeHashMapData* m_Buffer;

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

	public unsafe bool IsCreated => m_Buffer != null;

	public unsafe NativeMultiHashMap(int capacity, Allocator label)
	{
		m_AllocatorLabel = label;
		NativeHashMapData.AllocateHashMap<TKey, TValue>(capacity, capacity * 2, label, out m_Buffer);
		Clear();
	}

	public unsafe void Clear()
	{
		NativeHashMapBase<TKey, TValue>.Clear(m_Buffer);
	}

	public unsafe void Add(TKey key, TValue item)
	{
		NativeHashMapBase<TKey, TValue>.TryAdd(m_Buffer, key, item, isMultiHashMap: true, m_AllocatorLabel);
	}

	public unsafe void Remove(TKey key)
	{
		NativeHashMapBase<TKey, TValue>.Remove(m_Buffer, key, isMultiHashMap: true);
	}

	public unsafe void Remove(NativeMultiHashMapIterator<TKey> it)
	{
		NativeHashMapBase<TKey, TValue>.Remove(m_Buffer, it);
	}

	public unsafe bool TryGetFirstValue(TKey key, out TValue item, out NativeMultiHashMapIterator<TKey> it)
	{
		return NativeHashMapBase<TKey, TValue>.TryGetFirstValueAtomic(m_Buffer, key, out item, out it);
	}

	public unsafe bool TryGetNextValue(out TValue item, ref NativeMultiHashMapIterator<TKey> it)
	{
		return NativeHashMapBase<TKey, TValue>.TryGetNextValueAtomic(m_Buffer, out item, ref it);
	}

	public unsafe bool SetValue(TValue item, NativeMultiHashMapIterator<TKey> it)
	{
		return NativeHashMapBase<TKey, TValue>.SetValue(m_Buffer, ref it, ref item);
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
