using System;
using System.Collections.Generic;

namespace FixRTLTMPro;

public static class GlyphTable
{
	private static readonly Dictionary<char, char> MapList;

	static GlyphTable()
	{
		string[] names = Enum.GetNames(typeof(ArabicIsolatedLetters));
		MapList = new Dictionary<char, char>(names.Length);
		string[] array = names;
		foreach (string value in array)
		{
			MapList.Add((char)(int)Enum.Parse(typeof(ArabicGeneralLetters), value), (char)(int)Enum.Parse(typeof(ArabicIsolatedLetters), value));
		}
	}

	public static char Convert(char toBeConverted)
	{
		if (!MapList.TryGetValue(toBeConverted, out var value))
		{
			return toBeConverted;
		}
		return value;
	}
}
