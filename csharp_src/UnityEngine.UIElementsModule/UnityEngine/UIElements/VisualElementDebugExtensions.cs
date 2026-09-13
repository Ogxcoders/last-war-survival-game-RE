namespace UnityEngine.UIElements;

internal static class VisualElementDebugExtensions
{
	public static string GetDisplayName(this VisualElement ve, bool withHashCode = true)
	{
		if (ve == null)
		{
			return string.Empty;
		}
		string text = ve.GetType().Name;
		if (!string.IsNullOrEmpty(ve.name))
		{
			text = text + "#" + ve.name;
		}
		if (withHashCode)
		{
			text = text + " (" + ve.GetHashCode().ToString("x8") + ")";
		}
		return text;
	}
}
