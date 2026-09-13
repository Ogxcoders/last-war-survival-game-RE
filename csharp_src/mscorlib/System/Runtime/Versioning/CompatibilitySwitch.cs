namespace System.Runtime.Versioning;

public static class CompatibilitySwitch
{
	public static bool IsEnabled(string compatibilitySwitchName)
	{
		return false;
	}

	public static string GetValue(string compatibilitySwitchName)
	{
		return null;
	}

	internal static string GetValueInternal(string compatibilitySwitchName)
	{
		return null;
	}
}
