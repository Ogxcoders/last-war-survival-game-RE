using System;
using System.Collections.Generic;

namespace Unity.Collections;

internal sealed class NativeHashMapDebuggerTypeProxy<TKey, TValue> where TKey : struct, IEquatable<TKey> where TValue : struct
{
	private NativeHashMap<TKey, TValue> m_target;

	public List<Pair<TKey, TValue>> Items
	{
		get
		{
			List<Pair<TKey, TValue>> list = new List<Pair<TKey, TValue>>();
			using NativeArray<TKey> nativeArray = m_target.GetKeyArray(Allocator.Temp);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				if (m_target.TryGetValue(nativeArray[i], out var item))
				{
					list.Add(new Pair<TKey, TValue>(nativeArray[i], item));
				}
			}
			return list;
		}
	}

	public NativeHashMapDebuggerTypeProxy(NativeHashMap<TKey, TValue> target)
	{
		m_target = target;
	}
}
