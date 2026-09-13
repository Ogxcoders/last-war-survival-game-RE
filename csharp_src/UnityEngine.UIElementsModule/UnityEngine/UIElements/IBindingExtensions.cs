namespace UnityEngine.UIElements;

public static class IBindingExtensions
{
	public static bool IsBound(this IBindable control)
	{
		return control?.binding != null;
	}
}
