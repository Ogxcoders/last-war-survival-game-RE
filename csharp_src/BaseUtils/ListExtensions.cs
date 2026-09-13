using System.Collections.Generic;

public static class ListExtensions
{
	public static T Pop<T>(this List<T> list)
	{
		T result = default(T);
		int num = list.Count - 1;
		if (num >= 0)
		{
			result = list[num];
			list.RemoveAt(num);
			return result;
		}
		return result;
	}
}
