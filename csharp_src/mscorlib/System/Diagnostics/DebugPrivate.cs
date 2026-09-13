namespace System.Diagnostics;

internal static class DebugPrivate
{
	[Conditional("DEBUG")]
	public static void Assert(bool condition)
	{
	}

	[Conditional("DEBUG")]
	public static void Assert(bool condition, string message)
	{
	}

	[Conditional("DEBUG")]
	public static void Assert(bool condition, string message, string detailMessage)
	{
	}

	[Conditional("DEBUG")]
	public static void Assert(bool condition, string message, string detailMessageFormat, params object[] args)
	{
	}

	[Conditional("DEBUG")]
	public static void Fail(string message)
	{
	}

	[Conditional("DEBUG")]
	public static void Fail(string message, string detailMessage)
	{
	}
}
