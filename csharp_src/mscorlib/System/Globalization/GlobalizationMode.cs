namespace System.Globalization;

internal static class GlobalizationMode
{
	private const string c_InvariantModeConfigSwitch = "System.Globalization.Invariant";

	internal static bool Invariant { get; } = GetGlobalizationInvariantMode();

	private static bool GetGlobalizationInvariantMode()
	{
		return false;
	}
}
