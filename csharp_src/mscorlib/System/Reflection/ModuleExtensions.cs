namespace System.Reflection;

public static class ModuleExtensions
{
	public static bool HasModuleVersionId(this Module module)
	{
		Requires.NotNull(module, "module");
		return true;
	}

	public static Guid GetModuleVersionId(this Module module)
	{
		Requires.NotNull(module, "module");
		return module.ModuleVersionId;
	}
}
