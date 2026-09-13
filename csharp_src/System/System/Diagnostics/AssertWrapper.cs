namespace System.Diagnostics;

internal class AssertWrapper
{
	public static void ShowAssert(string stackTrace, StackFrame frame, string message, string detailMessage)
	{
		new DefaultTraceListener().Fail(message, detailMessage);
	}
}
