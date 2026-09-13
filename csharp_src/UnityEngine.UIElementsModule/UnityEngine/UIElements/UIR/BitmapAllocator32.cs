#define UNITY_ASSERTIONS
using System.Collections.Generic;

namespace UnityEngine.UIElements.UIR;

internal struct BitmapAllocator32
{
	private struct Page
	{
		public ushort x;

		public ushort y;

		public int freeSlots;
	}

	public const int kPageWidth = 32;

	private int m_PageHeight;

	private List<Page> m_Pages;

	private List<uint> m_AllocMap;

	private int m_EntryWidth;

	private int m_EntryHeight;

	public int entryWidth => m_EntryWidth;

	public int entryHeight => m_EntryHeight;

	public void Construct(int pageHeight, int entryWidth = 1, int entryHeight = 1)
	{
		m_PageHeight = pageHeight;
		m_Pages = new List<Page>(1);
		m_AllocMap = new List<uint>(m_PageHeight * m_Pages.Capacity);
		m_EntryWidth = entryWidth;
		m_EntryHeight = entryHeight;
	}

	public void ForceFirstAlloc(ushort firstPageX, ushort firstPageY)
	{
		m_AllocMap.Add(4294967294u);
		for (int i = 1; i < m_PageHeight; i++)
		{
			m_AllocMap.Add(uint.MaxValue);
		}
		m_Pages.Add(new Page
		{
			x = firstPageX,
			y = firstPageY,
			freeSlots = 32 * m_PageHeight - 1
		});
	}

	public BMPAlloc Allocate(BaseShaderInfoStorage storage)
	{
		int count = m_Pages.Count;
		for (int i = 0; i < count; i++)
		{
			Page value = m_Pages[i];
			if (value.freeSlots == 0)
			{
				continue;
			}
			int j = i * m_PageHeight;
			for (int num = j + m_PageHeight; j < num; j++)
			{
				uint num2 = m_AllocMap[j];
				if (num2 != 0)
				{
					byte b = CountTrailingZeroes(num2);
					m_AllocMap[j] = num2 & (uint)(~(1 << (int)b));
					value.freeSlots--;
					m_Pages[i] = value;
					return new BMPAlloc
					{
						page = i,
						pageLine = (ushort)(j - i * m_PageHeight),
						bitIndex = b,
						owned = 1
					};
				}
			}
		}
		if (storage == null || !storage.AllocateRect(32 * m_EntryWidth, m_PageHeight * m_EntryHeight, out var uvs))
		{
			return BMPAlloc.Invalid;
		}
		m_AllocMap.Capacity += m_PageHeight;
		m_AllocMap.Add(4294967294u);
		for (int k = 1; k < m_PageHeight; k++)
		{
			m_AllocMap.Add(uint.MaxValue);
		}
		m_Pages.Add(new Page
		{
			x = (ushort)uvs.xMin,
			y = (ushort)uvs.yMin,
			freeSlots = 32 * m_PageHeight - 1
		});
		return new BMPAlloc
		{
			page = m_Pages.Count - 1,
			owned = 1
		};
	}

	public void Free(BMPAlloc alloc)
	{
		Debug.Assert(alloc.owned == 1);
		int index = alloc.page * m_PageHeight + alloc.pageLine;
		m_AllocMap[index] |= (uint)(1 << (int)alloc.bitIndex);
		Page value = m_Pages[alloc.page];
		value.freeSlots++;
		m_Pages[alloc.page] = value;
	}

	internal void GetAllocPageAtlasLocation(int page, out ushort x, out ushort y)
	{
		Page page2 = m_Pages[page];
		x = page2.x;
		y = page2.y;
	}

	private static byte CountTrailingZeroes(uint val)
	{
		byte b = 0;
		if ((val & 0xFFFF) == 0)
		{
			val >>= 16;
			b = 16;
		}
		if ((val & 0xFF) == 0)
		{
			val >>= 8;
			b += 8;
		}
		if ((val & 0xF) == 0)
		{
			val >>= 4;
			b += 4;
		}
		if ((val & 3) == 0)
		{
			val >>= 2;
			b += 2;
		}
		if ((val & 1) == 0)
		{
			b++;
		}
		return b;
	}
}
