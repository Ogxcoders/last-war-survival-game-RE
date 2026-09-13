using Microsoft.Internal;

namespace System.ComponentModel.Composition.Diagnostics;

internal static class CompositionTraceSource
{
	private static readonly DebuggerTraceWriter Source = new DebuggerTraceWriter();

	public static bool CanWriteInformation => Source.CanWriteInformation;

	public static bool CanWriteWarning => Source.CanWriteWarning;

	public static bool CanWriteError => Source.CanWriteError;

	public static void WriteInformation(CompositionTraceId traceId, string format, params object[] arguments)
	{
		EnsureEnabled(CanWriteInformation);
		Source.WriteInformation(traceId, format, arguments);
	}

	public static void WriteWarning(CompositionTraceId traceId, string format, params object[] arguments)
	{
		EnsureEnabled(CanWriteWarning);
		Source.WriteWarning(traceId, format, arguments);
	}

	public static void WriteError(CompositionTraceId traceId, string format, params object[] arguments)
	{
		EnsureEnabled(CanWriteError);
		Source.WriteError(traceId, format, arguments);
	}

	private static void EnsureEnabled(bool condition)
	{
		Assumes.IsTrue(condition, "To avoid unnecessary work when a trace level has not been enabled, check CanWriteXXX before calling this method.");
	}
}
