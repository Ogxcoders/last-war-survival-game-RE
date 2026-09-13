using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

internal static class StyleCache
{
	private static Dictionary<long, VisualElementStylesData> s_StyleDataCache = new Dictionary<long, VisualElementStylesData>();

	private static Dictionary<int, InheritedStylesData> s_InheritedStyleDataCache = new Dictionary<int, InheritedStylesData>();

	private static Dictionary<int, StyleVariableContext> s_StyleVariableContextCache = new Dictionary<int, StyleVariableContext>();

	public static bool TryGetValue(long hash, out VisualElementStylesData data)
	{
		return s_StyleDataCache.TryGetValue(hash, out data);
	}

	public static void SetValue(long hash, VisualElementStylesData data)
	{
		s_StyleDataCache[hash] = data;
	}

	public static bool TryGetValue(int hash, out InheritedStylesData data)
	{
		return s_InheritedStyleDataCache.TryGetValue(hash, out data);
	}

	public static void SetValue(int hash, InheritedStylesData data)
	{
		s_InheritedStyleDataCache[hash] = data;
	}

	public static bool TryGetValue(int hash, out StyleVariableContext data)
	{
		return s_StyleVariableContextCache.TryGetValue(hash, out data);
	}

	public static void SetValue(int hash, StyleVariableContext data)
	{
		s_StyleVariableContextCache[hash] = data;
	}

	public static void ClearStyleCache()
	{
		s_StyleDataCache.Clear();
		s_InheritedStyleDataCache.Clear();
		s_StyleVariableContextCache.Clear();
	}
}
