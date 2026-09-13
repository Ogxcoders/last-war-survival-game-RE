using System;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

internal struct NativeQueueBlockPoolData
{
	internal IntPtr firstBlock;

	internal int allocatedBlocks;

	internal int MaxBlocks;

	internal const int BlockSize = 16384;

	internal int allocLock;

	public unsafe byte* AllocateBlock()
	{
		while (Interlocked.CompareExchange(ref allocLock, 1, 0) != 0)
		{
		}
		byte* ptr = (byte*)(void*)firstBlock;
		byte* ptr2;
		do
		{
			ptr2 = ptr;
			if (ptr2 == null)
			{
				Interlocked.Exchange(ref allocLock, 0);
				Interlocked.Increment(ref allocatedBlocks);
				return (byte*)UnsafeUtility.Malloc(16384L, 16, Allocator.Persistent);
			}
			ptr = (byte*)(void*)Interlocked.CompareExchange(ref firstBlock, (IntPtr)((NativeQueueBlockHeader*)ptr2)->nextBlock, (IntPtr)ptr2);
		}
		while (ptr != ptr2);
		Interlocked.Exchange(ref allocLock, 0);
		return ptr2;
	}

	public unsafe void FreeBlock(byte* block)
	{
		if (allocatedBlocks > MaxBlocks)
		{
			if (Interlocked.Decrement(ref allocatedBlocks) + 1 > MaxBlocks)
			{
				UnsafeUtility.Free(block, Allocator.Persistent);
				return;
			}
			Interlocked.Increment(ref allocatedBlocks);
		}
		byte* ptr = (byte*)(void*)firstBlock;
		byte* ptr2;
		do
		{
			ptr2 = ptr;
			((NativeQueueBlockHeader*)block)->nextBlock = ptr;
			ptr = (byte*)(void*)Interlocked.CompareExchange(ref firstBlock, (IntPtr)block, (IntPtr)ptr);
		}
		while (ptr != ptr2);
	}
}
