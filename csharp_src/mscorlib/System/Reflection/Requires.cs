namespace System.Reflection;

internal static class Requires
{
	internal static void NotNull(object obj, string name)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(name);
		}
	}
}
