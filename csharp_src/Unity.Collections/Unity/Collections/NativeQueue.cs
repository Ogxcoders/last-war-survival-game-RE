using System;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

[NativeContainer]
public struct NativeQueue<T> where T : struct
{
	[NativeContainer]
	[NativeContainerIsAtomicWriteOnly]
	public struct Concurrent
	{
		[NativeDisableUnsafePtrRestriction]
		internal unsafe NativeQueueData* m_Buffer;

		[NativeDisableUnsafePtrRestriction]
		internal unsafe NativeQueueBlockPoolData* m_QueuePool;

		[NativeSetThreadIndex]
		internal int m_ThreadIndex;

		public unsafe void Enqueue(T entry)
		{
			byte* ptr = NativeQueueData.AllocateWriteBlockMT<T>(m_Buffer, m_QueuePool, m_ThreadIndex);
			UnsafeUtility.WriteArrayElement(ptr + UnsafeUtility.SizeOf<NativeQueueBlockHeader>(), ((NativeQueueBlockHeader*)ptr)->itemsInBlock, entry);
			((NativeQueueBlockHeader*)ptr)->itemsInBlock++;
		}
	}

	[NativeDisableUnsafePtrRestriction]
	private unsafe NativeQueueData* m_Buffer;

	[NativeDisableUnsafePtrRestriction]
	private unsafe NativeQueueBlockPoolData* m_QueuePool;

	private Allocator m_AllocatorLabel;

	public unsafe int Count
	{
		get
		{
			int num = 0;
			for (NativeQueueBlockHeader* ptr = (NativeQueueBlockHeader*)m_Buffer->m_FirstBlock; ptr != null; ptr = (NativeQueueBlockHeader*)ptr->nextBlock)
			{
				num += ptr->itemsInBlock;
			}
			return num - m_Buffer->m_CurrentReadIndexInBlock;
		}
	}

	public unsafe static int PersistentMemoryBlockCount
	{
		get
		{
			return NativeQueueBlockPool.QueueBlockPool->MaxBlocks;
		}
		set
		{
			Interlocked.Exchange(ref NativeQueueBlockPool.QueueBlockPool->MaxBlocks, value);
		}
	}

	public static int MemoryBlockSize => 16384;

	public unsafe bool IsCreated => m_Buffer != null;

	public unsafe NativeQueue(Allocator label)
	{
		m_QueuePool = NativeQueueBlockPool.QueueBlockPool;
		m_AllocatorLabel = label;
		NativeQueueData.AllocateQueue<T>(label, out m_Buffer);
	}

	public unsafe T Peek()
	{
		byte* firstBlock = m_Buffer->m_FirstBlock;
		if (firstBlock == null)
		{
			throw new InvalidOperationException("Trying to peek from an empty queue");
		}
		return UnsafeUtility.ReadArrayElement<T>(firstBlock + UnsafeUtility.SizeOf<NativeQueueBlockHeader>(), m_Buffer->m_CurrentReadIndexInBlock);
	}

	public unsafe void Enqueue(T entry)
	{
		byte* ptr = NativeQueueData.AllocateWriteBlockMT<T>(m_Buffer, m_QueuePool, 0);
		UnsafeUtility.WriteArrayElement(ptr + UnsafeUtility.SizeOf<NativeQueueBlockHeader>(), ((NativeQueueBlockHeader*)ptr)->itemsInBlock, entry);
		((NativeQueueBlockHeader*)ptr)->itemsInBlock++;
	}

	public T Dequeue()
	{
		if (!TryDequeue(out var item))
		{
			throw new InvalidOperationException("Trying to dequeue from an empty queue");
		}
		return item;
	}

	public unsafe bool TryDequeue(out T item)
	{
		byte* firstBlock = m_Buffer->m_FirstBlock;
		if (firstBlock == null)
		{
			item = default(T);
			return false;
		}
		item = UnsafeUtility.ReadArrayElement<T>(firstBlock + UnsafeUtility.SizeOf<NativeQueueBlockHeader>(), m_Buffer->m_CurrentReadIndexInBlock);
		m_Buffer->m_CurrentReadIndexInBlock++;
		if (m_Buffer->m_CurrentReadIndexInBlock >= ((NativeQueueBlockHeader*)firstBlock)->itemsInBlock)
		{
			m_Buffer->m_CurrentReadIndexInBlock = 0;
			m_Buffer->m_FirstBlock = ((NativeQueueBlockHeader*)firstBlock)->nextBlock;
			if (m_Buffer->m_FirstBlock == null)
			{
				m_Buffer->m_LastBlock = IntPtr.Zero;
			}
			for (int i = 0; i < 128; i++)
			{
				if (m_Buffer->m_CurrentWriteBlockTLS[i * 16] == firstBlock)
				{
					m_Buffer->m_CurrentWriteBlockTLS[i * 16] = null;
				}
			}
			m_QueuePool->FreeBlock(firstBlock);
		}
		return true;
	}

	public unsafe void Clear()
	{
		NativeQueueBlockHeader* ptr = (NativeQueueBlockHeader*)m_Buffer->m_FirstBlock;
		while (ptr != null)
		{
			NativeQueueBlockHeader* nextBlock = (NativeQueueBlockHeader*)ptr->nextBlock;
			m_QueuePool->FreeBlock((byte*)ptr);
			ptr = nextBlock;
		}
		m_Buffer->m_FirstBlock = null;
		m_Buffer->m_LastBlock = IntPtr.Zero;
		m_Buffer->m_CurrentReadIndexInBlock = 0;
		for (int i = 0; i < 128; i++)
		{
			m_Buffer->m_CurrentWriteBlockTLS[i * 16] = null;
		}
	}

	public unsafe void Dispose()
	{
		NativeQueueData.DeallocateQueue(m_Buffer, m_QueuePool, m_AllocatorLabel);
		m_Buffer = null;
	}

	public unsafe Concurrent ToConcurrent()
	{
		Concurrent result = default(Concurrent);
		result.m_Buffer = m_Buffer;
		result.m_QueuePool = m_QueuePool;
		result.m_ThreadIndex = 0;
		return result;
	}
}
