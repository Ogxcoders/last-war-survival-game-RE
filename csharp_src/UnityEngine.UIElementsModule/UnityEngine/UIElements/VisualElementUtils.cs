using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal static class VisualElementUtils
{
	private static readonly HashSet<string> s_usedNames = new HashSet<string>();

	public static string GetUniqueName(string nameBase)
	{
		string text = nameBase;
		int num = 2;
		while (s_usedNames.Contains(text))
		{
			text = nameBase + num;
			num++;
		}
		s_usedNames.Add(text);
		return text;
	}
}
