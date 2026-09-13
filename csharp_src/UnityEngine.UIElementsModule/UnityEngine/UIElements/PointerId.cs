using System.Collections.Generic;

namespace UnityEngine.UIElements;

public static class PointerId
{
	public static readonly int maxPointers = 32;

	public static readonly int invalidPointerId = -1;

	public static readonly int mousePointerId = 0;

	public static readonly int touchPointerIdBase = 1;

	public static readonly int touchPointerCount = 20;

	public static readonly int penPointerIdBase = touchPointerIdBase + touchPointerCount;

	public static readonly int penPointerCount = 2;

	internal static IEnumerable<int> hoveringPointers
	{
		get
		{
			yield return mousePointerId;
		}
	}
}
