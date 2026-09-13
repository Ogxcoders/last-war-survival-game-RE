using System;

namespace UnityEngine.UIElements.UIR;

internal class GPUBufferAllocator
{
	private BestFitAllocator m_Low;

	private BestFitAllocator m_High;

	public bool isEmpty => m_Low.highWatermark == 0 && m_High.highWatermark == 0;

	public GPUBufferAllocator(uint maxSize)
	{
		m_Low = new BestFitAllocator(maxSize);
		m_High = new BestFitAllocator(maxSize);
	}

	public Alloc Allocate(uint size, bool shortLived)
	{
		Alloc alloc;
		if (!shortLived)
		{
			alloc = m_Low.Allocate(size);
		}
		else
		{
			alloc = m_High.Allocate(size);
			alloc.start = m_High.totalSize - alloc.start - alloc.size;
		}
		alloc.shortLived = shortLived;
		if (HighLowCollide() && alloc.size != 0)
		{
			Free(alloc);
			return default(Alloc);
		}
		return alloc;
	}

	public void Free(Alloc alloc)
	{
		if (!alloc.shortLived)
		{
			m_Low.Free(alloc);
			return;
		}
		alloc.start = m_High.totalSize - alloc.start - alloc.size;
		m_High.Free(alloc);
	}

	public HeapStatistics GatherStatistics()
	{
		HeapStatistics result = new HeapStatistics
		{
			subAllocators = new HeapStatistics[2]
			{
				m_Low.GatherStatistics(),
				m_High.GatherStatistics()
			},
			largestAvailableBlock = uint.MaxValue
		};
		for (int i = 0; i < 2; i++)
		{
			result.numAllocs += result.subAllocators[i].numAllocs;
			result.totalSize = Math.Max(result.totalSize, result.subAllocators[i].totalSize);
			result.allocatedSize += result.subAllocators[i].allocatedSize;
			result.largestAvailableBlock = Math.Min(result.largestAvailableBlock, result.subAllocators[i].largestAvailableBlock);
			result.availableBlocksCount += result.subAllocators[i].availableBlocksCount;
			result.blockCount += result.subAllocators[i].blockCount;
			result.highWatermark = Math.Max(result.highWatermark, result.subAllocators[i].highWatermark);
			result.fragmentation = Math.Max(result.fragmentation, result.subAllocators[i].fragmentation);
		}
		result.freeSize = result.totalSize - result.allocatedSize;
		return result;
	}

	private bool HighLowCollide()
	{
		return m_Low.highWatermark + m_High.highWatermark > m_Low.totalSize;
	}
}
