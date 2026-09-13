using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections;

internal static class NativeQueueBlockPool
{
	private static NativeQueueBlockPoolData data;

	public unsafe static NativeQueueBlockPoolData* QueueBlockPool
	{
		get
		{
			if (data.allocatedBlocks == 0)
			{
				data.allocatedBlocks = (data.MaxBlocks = 256);
				data.allocLock = 0;
				byte* ptr = null;
				for (int i = 0; i < data.MaxBlocks; i++)
				{
					NativeQueueBlockHeader* ptr2 = (NativeQueueBlockHeader*)UnsafeUtility.Malloc(16384L, 16, Allocator.Persistent);
					ptr2->nextBlock = ptr;
					ptr = (byte*)ptr2;
				}
				data.firstBlock = (IntPtr)ptr;
				AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
			}
			return (NativeQueueBlockPoolData*)UnsafeUtility.AddressOf(ref data);
		}
	}

	private unsafe static void OnDomainUnload(object sender, EventArgs e)
	{
		while (data.firstBlock != IntPtr.Zero)
		{
			NativeQueueBlockHeader* ptr = (NativeQueueBlockHeader*)(void*)data.firstBlock;
			data.firstBlock = (IntPtr)ptr->nextBlock;
			UnsafeUtility.Free(ptr, Allocator.Persistent);
			data.allocatedBlocks--;
		}
	}
}
