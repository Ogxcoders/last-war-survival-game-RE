using System;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

internal struct NativeQueueData
{
	public unsafe byte* m_FirstBlock;

	public IntPtr m_LastBlock;

	public int m_ItemsPerBlock;

	public int m_CurrentReadIndexInBlock;

	public const int IntsPerCacheLine = 16;

	public unsafe byte** m_CurrentWriteBlockTLS;

	public unsafe static byte* AllocateWriteBlockMT<T>(NativeQueueData* data, NativeQueueBlockPoolData* pool, int threadIndex) where T : struct
	{
		byte* ptr = data->m_CurrentWriteBlockTLS[threadIndex * 16];
		if (ptr != null && ((NativeQueueBlockHeader*)ptr)->itemsInBlock == data->m_ItemsPerBlock)
		{
			ptr = null;
		}
		if (ptr == null)
		{
			ptr = pool->AllocateBlock();
			((NativeQueueBlockHeader*)ptr)->nextBlock = null;
			((NativeQueueBlockHeader*)ptr)->itemsInBlock = 0;
			NativeQueueBlockHeader* ptr2 = (NativeQueueBlockHeader*)(void*)Interlocked.Exchange(ref data->m_LastBlock, (IntPtr)ptr);
			if (ptr2 == null)
			{
				data->m_FirstBlock = ptr;
			}
			else
			{
				ptr2->nextBlock = ptr;
			}
			data->m_CurrentWriteBlockTLS[threadIndex * 16] = ptr;
		}
		return ptr;
	}

	public unsafe static void AllocateQueue<T>(Allocator label, out NativeQueueData* outBuf) where T : struct
	{
		NativeQueueData* ptr = (NativeQueueData*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<NativeQueueData>() + UnsafeUtility.SizeOf<IntPtr>() * 128 * 16, UnsafeUtility.AlignOf<NativeQueueData>(), label);
		ptr->m_CurrentWriteBlockTLS = (byte**)((byte*)ptr + UnsafeUtility.SizeOf<NativeQueueData>());
		ptr->m_FirstBlock = null;
		ptr->m_LastBlock = IntPtr.Zero;
		ptr->m_ItemsPerBlock = (16384 - UnsafeUtility.SizeOf<NativeQueueBlockHeader>()) / UnsafeUtility.SizeOf<T>();
		ptr->m_CurrentReadIndexInBlock = 0;
		for (int i = 0; i < 128; i++)
		{
			ptr->m_CurrentWriteBlockTLS[i * 16] = null;
		}
		outBuf = ptr;
	}

	public unsafe static void DeallocateQueue(NativeQueueData* data, NativeQueueBlockPoolData* pool, Allocator allocation)
	{
		NativeQueueBlockHeader* ptr = (NativeQueueBlockHeader*)data->m_FirstBlock;
		while (ptr != null)
		{
			NativeQueueBlockHeader* nextBlock = (NativeQueueBlockHeader*)ptr->nextBlock;
			pool->FreeBlock((byte*)ptr);
			ptr = nextBlock;
		}
		UnsafeUtility.Free(data, allocation);
	}
}
