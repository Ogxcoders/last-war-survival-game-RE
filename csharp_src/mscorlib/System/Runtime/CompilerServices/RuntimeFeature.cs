namespace System.Runtime.CompilerServices;

public static class RuntimeFeature
{
	public const string PortablePdb = "PortablePdb";

	public const string DefaultImplementationsOfInterfaces = "DefaultImplementationsOfInterfaces";

	public static bool IsDynamicCodeSupported => true;

	public static bool IsDynamicCodeCompiled => true;

	public static bool IsSupported(string feature)
	{
		switch (feature)
		{
		case "PortablePdb":
		case "DefaultImplementationsOfInterfaces":
			return true;
		case "IsDynamicCodeSupported":
			return IsDynamicCodeSupported;
		case "IsDynamicCodeCompiled":
			return IsDynamicCodeCompiled;
		default:
			return false;
		}
	}
}
