using System;
using System.Collections.Generic;

namespace Unity.Collections;

internal sealed class NativeMultiHashMapDebuggerTypeProxy<TKey, TValue> where TKey : struct, IEquatable<TKey>, IComparable<TKey> where TValue : struct
{
	private NativeMultiHashMap<TKey, TValue> m_target;

	public List<ListPair<TKey, List<TValue>>> Items
	{
		get
		{
			List<ListPair<TKey, List<TValue>>> list = new List<ListPair<TKey, List<TValue>>>();
			Tuple<NativeArray<TKey>, int> uniqueKeyArray = m_target.GetUniqueKeyArray(Allocator.Temp);
			using (uniqueKeyArray.Item1)
			{
				for (int i = 0; i < uniqueKeyArray.Item2; i++)
				{
					List<TValue> list2 = new List<TValue>();
					if (m_target.TryGetFirstValue(uniqueKeyArray.Item1[i], out var item, out var it))
					{
						do
						{
							list2.Add(item);
						}
						while (m_target.TryGetNextValue(out item, ref it));
					}
					list.Add(new ListPair<TKey, List<TValue>>(uniqueKeyArray.Item1[i], list2));
				}
				return list;
			}
		}
	}

	public NativeMultiHashMapDebuggerTypeProxy(NativeMultiHashMap<TKey, TValue> target)
	{
		m_target = target;
	}
}
