using System;

namespace MiniGame.Core;

internal static class StringUtils
{
	public static string Trim(this string s, int start, int length)
	{
		if (s == null)
		{
			throw new ArgumentNullException();
		}
		if (start < 0)
		{
			throw new ArgumentOutOfRangeException("start");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		int num = start + length - 1;
		if (num >= s.Length)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		while (start < num && char.IsWhiteSpace(s[start]))
		{
			start++;
		}
		while (num >= start && char.IsWhiteSpace(s[num]))
		{
			num--;
		}
		return s.Substring(start, num - start + 1);
	}
}
