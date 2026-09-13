namespace System.Reflection;

public static class MethodInfoExtensions
{
	public static MethodInfo GetBaseDefinition(MethodInfo method)
	{
		Requires.NotNull(method, "method");
		return method.GetBaseDefinition();
	}
}
