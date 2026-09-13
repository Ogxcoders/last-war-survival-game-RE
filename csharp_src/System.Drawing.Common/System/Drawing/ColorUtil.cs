namespace System.Drawing;

internal static class ColorUtil
{
	public static Color FromKnownColor(KnownColor color)
	{
		return Color.FromKnownColor(color);
	}

	public static bool IsSystemColor(this Color color)
	{
		return color.IsSystemColor;
	}
}
