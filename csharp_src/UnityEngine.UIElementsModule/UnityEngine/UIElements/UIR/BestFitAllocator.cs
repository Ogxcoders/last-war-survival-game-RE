#define UNITY_ASSERTIONS
using System;

namespace UnityEngine.UIElements.UIR;

internal class BestFitAllocator
{
	private class Block : PoolItem
	{
		public uint start;

		public uint end;

		public Block prev;

		public Block next;

		public Block prevAvailable;

		public Block nextAvailable;

		public bool allocated;

		public uint size => end - start;
	}

	private Block m_FirstBlock;

	private Block m_FirstAvailableBlock;

	private Pool<Block> m_BlockPool = new Pool<Block>();

	private uint m_HighWatermark;

	public uint totalSize { get; }

	public uint highWatermark => m_HighWatermark;

	public BestFitAllocator(uint size)
	{
		totalSize = size;
		m_FirstBlock = (m_FirstAvailableBlock = m_BlockPool.Get());
		m_FirstAvailableBlock.end = size;
	}

	public Alloc Allocate(uint size)
	{
		Block block = BestFitFindAvailableBlock(size);
		if (block == null)
		{
			return default(Alloc);
		}
		Debug.Assert(block.size >= size);
		Debug.Assert(!block.allocated);
		if (size != block.size)
		{
			SplitBlock(block, size);
		}
		Debug.Assert(block.size == size);
		if (block.end > m_HighWatermark)
		{
			m_HighWatermark = block.end;
		}
		if (block == m_FirstAvailableBlock)
		{
			m_FirstAvailableBlock = m_FirstAvailableBlock.nextAvailable;
		}
		if (block.prevAvailable != null)
		{
			block.prevAvailable.nextAvailable = block.nextAvailable;
		}
		if (block.nextAvailable != null)
		{
			block.nextAvailable.prevAvailable = block.prevAvailable;
		}
		block.allocated = true;
		block.prevAvailable = (block.nextAvailable = null);
		return new Alloc
		{
			start = block.start,
			size = block.size,
			handle = block
		};
	}

	public void Free(Alloc alloc)
	{
		Block block = (Block)alloc.handle;
		if (!block.allocated)
		{
			Debug.Assert(condition: false, "Severe error: UIR allocation double-free");
			return;
		}
		Debug.Assert(block.allocated);
		Debug.Assert(block.start == alloc.start);
		Debug.Assert(block.size == alloc.size);
		if (block.end == m_HighWatermark)
		{
			if (block.prev != null)
			{
				m_HighWatermark = (block.prev.allocated ? block.prev.end : block.prev.start);
			}
			else
			{
				m_HighWatermark = 0u;
			}
		}
		block.allocated = false;
		Block block2 = m_FirstAvailableBlock;
		Block block3 = null;
		while (block2 != null && block2.start < block.start)
		{
			block3 = block2;
			block2 = block2.nextAvailable;
		}
		if (block3 == null)
		{
			Debug.Assert(block.prevAvailable == null);
			block.nextAvailable = m_FirstAvailableBlock;
			m_FirstAvailableBlock = block;
		}
		else
		{
			block.prevAvailable = block3;
			block.nextAvailable = block3.nextAvailable;
			block3.nextAvailable = block;
		}
		if (block.nextAvailable != null)
		{
			block.nextAvailable.prevAvailable = block;
		}
		if (block.prevAvailable == block.prev && block.prev != null)
		{
			block = CoalesceBlockWithPrevious(block);
		}
		if (block.nextAvailable == block.next && block.next != null)
		{
			block = CoalesceBlockWithPrevious(block.next);
		}
	}

	private Block CoalesceBlockWithPrevious(Block block)
	{
		Debug.Assert(block.prevAvailable.end == block.start);
		Debug.Assert(block.prev.nextAvailable == block);
		Block prev = block.prev;
		prev.next = block.next;
		if (block.next != null)
		{
			block.next.prev = prev;
		}
		prev.nextAvailable = block.nextAvailable;
		if (block.nextAvailable != null)
		{
			block.nextAvailable.prevAvailable = block.prevAvailable;
		}
		prev.end = block.end;
		m_BlockPool.Return(block);
		return prev;
	}

	internal HeapStatistics GatherStatistics()
	{
		HeapStatistics result = default(HeapStatistics);
		for (Block block = m_FirstBlock; block != null; block = block.next)
		{
			if (block.allocated)
			{
				result.numAllocs++;
				result.allocatedSize += block.size;
			}
			else
			{
				result.freeSize += block.size;
				result.availableBlocksCount++;
				result.largestAvailableBlock = Math.Max(result.largestAvailableBlock, block.size);
			}
			result.blockCount++;
		}
		result.totalSize = totalSize;
		result.highWatermark = m_HighWatermark;
		if (result.freeSize != 0)
		{
			result.fragmentation = (float)((double)(result.freeSize - result.largestAvailableBlock) / (double)result.freeSize) * 100f;
		}
		return result;
	}

	private Block BestFitFindAvailableBlock(uint size)
	{
		Block block = m_FirstAvailableBlock;
		Block result = null;
		uint num = uint.MaxValue;
		while (block != null)
		{
			if (block.size >= size && num > block.size)
			{
				result = block;
				num = block.size;
			}
			block = block.nextAvailable;
		}
		return result;
	}

	private void SplitBlock(Block block, uint size)
	{
		Debug.Assert(block.size > size);
		Block block2 = m_BlockPool.Get();
		block2.next = block.next;
		block2.nextAvailable = block.nextAvailable;
		block2.prev = block;
		block2.prevAvailable = block;
		block2.start = block.start + size;
		block2.end = block.end;
		if (block2.next != null)
		{
			block2.next.prev = block2;
		}
		if (block2.nextAvailable != null)
		{
			block2.nextAvailable.prevAvailable = block2;
		}
		block.next = block2;
		block.nextAvailable = block2;
		block.end = block2.start;
	}
}
