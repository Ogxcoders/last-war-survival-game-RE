using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal static class ListExtensions
{
	public static bool SequenceEqualFast(this List<bool> self, List<bool> value)
	{
		if (self.Count != value.Count)
		{
			return false;
		}
		for (int i = 0; i < self.Count; i++)
		{
			if (self[i] != value[i])
			{
				return false;
			}
		}
		return true;
	}

	public static int CountFast(this List<bool> self)
	{
		int num = 0;
		for (int i = 0; i < self.Count; i++)
		{
			if (self[i])
			{
				num++;
			}
		}
		return num;
	}

	public static bool AnyFast<T>(this List<T> self) where T : UnityEngine.Object
	{
		for (int i = 0; i < self.Count; i++)
		{
			if ((bool)self[i])
			{
				return true;
			}
		}
		return false;
	}

	public static bool AnyFast<T>(this List<T> self, Predicate<T> predicate) where T : UnityEngine.Object
	{
		for (int i = 0; i < self.Count; i++)
		{
			if ((bool)self[i] && predicate(self[i]))
			{
				return true;
			}
		}
		return false;
	}
}
