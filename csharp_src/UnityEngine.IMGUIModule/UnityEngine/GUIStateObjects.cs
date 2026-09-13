using System;
using System.Collections.Generic;
using System.Security;

namespace UnityEngine;

internal class GUIStateObjects
{
	private static Dictionary<int, object> s_StateCache = new Dictionary<int, object>();

	[SecuritySafeCritical]
	internal static object GetStateObject(Type t, int controlID)
	{
		if (!s_StateCache.TryGetValue(controlID, out var value) || (object)value.GetType() != t)
		{
			value = Activator.CreateInstance(t);
			s_StateCache[controlID] = value;
		}
		return value;
	}

	internal static object QueryStateObject(Type t, int controlID)
	{
		object obj = s_StateCache[controlID];
		if (t.IsInstanceOfType(obj))
		{
			return obj;
		}
		return null;
	}

	internal static void Tests_ClearObjects()
	{
		s_StateCache.Clear();
	}
}
