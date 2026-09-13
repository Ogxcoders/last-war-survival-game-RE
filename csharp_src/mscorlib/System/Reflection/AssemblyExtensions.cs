namespace System.Reflection;

public static class AssemblyExtensions
{
	public static Type[] GetExportedTypes(Assembly assembly)
	{
		Requires.NotNull(assembly, "assembly");
		return assembly.GetExportedTypes();
	}

	public static Module[] GetModules(Assembly assembly)
	{
		Requires.NotNull(assembly, "assembly");
		return assembly.GetModules();
	}

	public static Type[] GetTypes(Assembly assembly)
	{
		Requires.NotNull(assembly, "assembly");
		return assembly.GetTypes();
	}
}
