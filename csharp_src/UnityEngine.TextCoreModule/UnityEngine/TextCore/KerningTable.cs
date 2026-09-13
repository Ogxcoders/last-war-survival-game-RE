using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TextCore.LowLevel;

namespace UnityEngine.TextCore;

[Serializable]
internal class KerningTable
{
	public List<KerningPair> kerningPairs;

	public KerningTable()
	{
		kerningPairs = new List<KerningPair>();
	}

	public int AddGlyphPairAdjustmentRecord(uint first, GlyphValueRecord firstAdjustments, uint second, GlyphValueRecord secondAdjustments)
	{
		int num = kerningPairs.FindIndex((KerningPair item) => item.firstGlyph == first && item.secondGlyph == second);
		if (num == -1)
		{
			kerningPairs.Add(new KerningPair(first, firstAdjustments, second, secondAdjustments));
			return 0;
		}
		return -1;
	}

	public void RemoveKerningPair(int left, int right)
	{
		int num = kerningPairs.FindIndex((KerningPair item) => item.firstGlyph == left && item.secondGlyph == right);
		if (num != -1)
		{
			kerningPairs.RemoveAt(num);
		}
	}

	public void RemoveKerningPair(int index)
	{
		kerningPairs.RemoveAt(index);
	}

	public void SortKerningPairs()
	{
		if (kerningPairs.Count > 0)
		{
			kerningPairs = (from s in kerningPairs
				orderby s.firstGlyph, s.secondGlyph
				select s).ToList();
		}
	}
}
