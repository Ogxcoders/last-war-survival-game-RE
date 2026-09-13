using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal static class CombineInstanceArrayPool
{
	private static readonly Dictionary<int, CombineInstance[]> s_Pool;

	public static void Init()
	{
		s_Pool.Clear();
	}

	static CombineInstanceArrayPool()
	{
		s_Pool = new Dictionary<int, CombineInstance[]>();
	}

	public static CombineInstance[] Get(List<CombineInstance> src)
	{
		int count = src.Count;
		if (!s_Pool.TryGetValue(count, out var value))
		{
			value = new CombineInstance[count];
			s_Pool.Add(count, value);
		}
		for (int i = 0; i < src.Count; i++)
		{
			value[i].mesh = src[i].mesh;
			value[i].transform = src[i].transform;
		}
		return value;
	}

	public static CombineInstance[] Get(List<CombineInstanceEx> src, int count)
	{
		if (!s_Pool.TryGetValue(count, out var value))
		{
			value = new CombineInstance[count];
			s_Pool.Add(count, value);
		}
		for (int i = 0; i < count; i++)
		{
			value[i].mesh = src[i].mesh;
			value[i].transform = src[i].transform;
		}
		return value;
	}
}
