using System.Runtime.Diagnostics;

namespace System.Runtime.Serialization;

internal static class FxTrace
{
	public static bool ShouldTraceError = true;

	public static bool ShouldTraceVerbose = true;

	public static EtwDiagnosticTrace Trace => Fx.Trace;

	public static ExceptionTrace Exception => new ExceptionTrace("System.Runtime.Serialization", Trace);

	public static bool IsEventEnabled(int index)
	{
		return false;
	}

	public static void UpdateEventDefinitions(EventDescriptor[] ed, ushort[] events)
	{
	}
}
