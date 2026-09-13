namespace UnityEngine.UIElements;

public static class PointerType
{
	public static readonly string mouse = "mouse";

	public static readonly string touch = "touch";

	public static readonly string pen = "pen";

	public static readonly string unknown = "";

	internal static string GetPointerType(int pointerId)
	{
		if (pointerId == PointerId.mousePointerId)
		{
			return mouse;
		}
		return touch;
	}

	internal static bool IsDirectManipulationDevice(string pointerType)
	{
		return (object)pointerType == touch;
	}
}
