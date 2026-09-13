#define UNITY_ASSERTIONS
using System;
using Unity.Profiling;
using UnityEngine.Assertions;

namespace UnityEngine.UIElements;

internal class UIRAtlasAllocator : IDisposable
{
	private class Row
	{
		private static ObjectPool<Row> s_Pool = new ObjectPool<Row>();

		public int Cursor;

		public int offsetX { get; private set; }

		public int offsetY { get; private set; }

		public int width { get; private set; }

		public int height { get; private set; }

		public static Row Acquire(int offsetX, int offsetY, int width, int height)
		{
			Row row = s_Pool.Get();
			row.offsetX = offsetX;
			row.offsetY = offsetY;
			row.width = width;
			row.height = height;
			row.Cursor = 0;
			return row;
		}

		public void Release()
		{
			s_Pool.Release(this);
			offsetX = -1;
			offsetY = -1;
			width = -1;
			height = -1;
			Cursor = -1;
		}
	}

	private class AreaNode
	{
		private static ObjectPool<AreaNode> s_Pool = new ObjectPool<AreaNode>();

		public RectInt rect;

		public AreaNode previous;

		public AreaNode next;

		public static AreaNode Acquire(RectInt rect)
		{
			AreaNode areaNode = s_Pool.Get();
			areaNode.rect = rect;
			areaNode.previous = null;
			areaNode.next = null;
			return areaNode;
		}

		public void Release()
		{
			s_Pool.Release(this);
		}

		public void RemoveFromChain()
		{
			if (previous != null)
			{
				previous.next = next;
			}
			if (next != null)
			{
				next.previous = previous;
			}
			previous = null;
			next = null;
		}

		public void AddAfter(AreaNode previous)
		{
			Assert.IsNull(this.previous);
			Assert.IsNull(next);
			this.previous = previous;
			if (previous != null)
			{
				next = previous.next;
				previous.next = this;
			}
			if (next != null)
			{
				next.previous = this;
			}
		}
	}

	private AreaNode m_FirstUnpartitionedArea;

	private Row[] m_OpenRows;

	private int m_1SidePadding;

	private int m_2SidePadding;

	private static ProfilerMarker s_MarkerTryAllocate = new ProfilerMarker("UIRAtlasAllocator.TryAllocate");

	public int maxAtlasSize { get; }

	public int maxImageWidth { get; }

	public int maxImageHeight { get; }

	public int virtualWidth { get; private set; }

	public int virtualHeight { get; private set; }

	public int physicalWidth { get; private set; }

	public int physicalHeight { get; private set; }

	protected bool disposed { get; private set; }

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposed)
		{
			return;
		}
		if (disposing)
		{
			for (int i = 0; i < m_OpenRows.Length; i++)
			{
				m_OpenRows[i]?.Release();
			}
			m_OpenRows = null;
			AreaNode areaNode = m_FirstUnpartitionedArea;
			while (areaNode != null)
			{
				AreaNode next = areaNode.next;
				areaNode.Release();
				areaNode = next;
			}
			m_FirstUnpartitionedArea = null;
		}
		disposed = true;
	}

	private static int GetLog2OfNextPower(int n)
	{
		float f = Mathf.NextPowerOfTwo(n);
		float f2 = Mathf.Log(f, 2f);
		return Mathf.RoundToInt(f2);
	}

	public UIRAtlasAllocator(int initialAtlasSize, int maxAtlasSize, int sidePadding = 1)
	{
		Assert.IsTrue(initialAtlasSize > 0 && initialAtlasSize <= maxAtlasSize);
		Assert.IsTrue(initialAtlasSize == Mathf.NextPowerOfTwo(initialAtlasSize));
		Assert.IsTrue(maxAtlasSize == Mathf.NextPowerOfTwo(maxAtlasSize));
		m_1SidePadding = sidePadding;
		m_2SidePadding = sidePadding << 1;
		this.maxAtlasSize = maxAtlasSize;
		maxImageWidth = maxAtlasSize;
		maxImageHeight = ((initialAtlasSize == maxAtlasSize) ? (maxAtlasSize / 2 + m_2SidePadding) : (maxAtlasSize / 4 + m_2SidePadding));
		virtualWidth = initialAtlasSize;
		virtualHeight = initialAtlasSize;
		int num = GetLog2OfNextPower(maxAtlasSize) + 1;
		m_OpenRows = new Row[num];
		RectInt rect = new RectInt(0, 0, initialAtlasSize, initialAtlasSize);
		m_FirstUnpartitionedArea = AreaNode.Acquire(rect);
		BuildAreas();
	}

	public bool TryAllocate(int width, int height, out RectInt location)
	{
		using (s_MarkerTryAllocate.Auto())
		{
			location = default(RectInt);
			if (disposed)
			{
				return false;
			}
			if (width < 1 || height < 1)
			{
				return false;
			}
			if (width > maxImageWidth || height > maxImageHeight)
			{
				return false;
			}
			int log2OfNextPower = GetLog2OfNextPower(Mathf.Max(height - m_2SidePadding, 1));
			int rowHeight = (1 << log2OfNextPower) + m_2SidePadding;
			Row row = m_OpenRows[log2OfNextPower];
			if (row != null && row.width - row.Cursor < width)
			{
				row = null;
			}
			if (row == null)
			{
				for (AreaNode areaNode = m_FirstUnpartitionedArea; areaNode != null; areaNode = areaNode.next)
				{
					if (TryPartitionArea(areaNode, log2OfNextPower, rowHeight, width))
					{
						row = m_OpenRows[log2OfNextPower];
						break;
					}
				}
				if (row == null)
				{
					return false;
				}
			}
			location = new RectInt(row.offsetX + row.Cursor, row.offsetY, width, height);
			row.Cursor += width;
			Assert.IsTrue(row.Cursor <= row.width);
			physicalWidth = Mathf.NextPowerOfTwo(Mathf.Max(physicalWidth, location.xMax));
			physicalHeight = Mathf.NextPowerOfTwo(Mathf.Max(physicalHeight, location.yMax));
			return true;
		}
	}

	private bool TryPartitionArea(AreaNode areaNode, int rowIndex, int rowHeight, int minWidth)
	{
		RectInt rect = areaNode.rect;
		if (rect.height < rowHeight || rect.width < minWidth)
		{
			return false;
		}
		m_OpenRows[rowIndex]?.Release();
		Row row = Row.Acquire(rect.x, rect.y, rect.width, rowHeight);
		m_OpenRows[rowIndex] = row;
		rect.y += rowHeight;
		rect.height -= rowHeight;
		if (rect.height == 0)
		{
			if (areaNode == m_FirstUnpartitionedArea)
			{
				m_FirstUnpartitionedArea = areaNode.next;
			}
			areaNode.RemoveFromChain();
			areaNode.Release();
		}
		else
		{
			areaNode.rect = rect;
		}
		return true;
	}

	private void BuildAreas()
	{
		AreaNode previous = m_FirstUnpartitionedArea;
		while (virtualWidth < maxAtlasSize || virtualHeight < maxAtlasSize)
		{
			RectInt rect;
			if (virtualWidth > virtualHeight)
			{
				rect = new RectInt(0, virtualHeight, virtualWidth, virtualHeight);
				virtualHeight *= 2;
			}
			else
			{
				rect = new RectInt(virtualWidth, 0, virtualWidth, virtualHeight);
				virtualWidth *= 2;
			}
			AreaNode areaNode = AreaNode.Acquire(rect);
			areaNode.AddAfter(previous);
			previous = areaNode;
		}
	}
}
