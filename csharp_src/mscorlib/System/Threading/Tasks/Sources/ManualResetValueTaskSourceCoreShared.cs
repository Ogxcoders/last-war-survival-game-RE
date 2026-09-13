using System.Diagnostics;

namespace System.Threading.Tasks.Sources;

internal static class ManualResetValueTaskSourceCoreShared
{
	internal static readonly Action<object> s_sentinel = CompletionSentinel;

	[StackTraceHidden]
	internal static void ThrowInvalidOperationException()
	{
		throw new InvalidOperationException();
	}

	private static void CompletionSentinel(object _)
	{
		ThrowInvalidOperationException();
	}
}
