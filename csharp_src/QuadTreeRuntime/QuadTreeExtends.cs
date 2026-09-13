using UnityEngine;

public static class QuadTreeExtends
{
	public static bool Contains(this Rect rect, Rect other)
	{
		if (other.xMin >= rect.xMin && other.xMax <= rect.xMax && other.yMin >= rect.yMin)
		{
			return other.yMax <= rect.yMax;
		}
		return false;
	}
}
