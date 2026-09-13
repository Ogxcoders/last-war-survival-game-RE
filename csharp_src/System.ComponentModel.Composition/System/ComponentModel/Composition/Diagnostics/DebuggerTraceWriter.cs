using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace System.ComponentModel.Composition.Diagnostics;

internal sealed class DebuggerTraceWriter : TraceWriter
{
	internal enum TraceEventType
	{
		Error = 2,
		Warning = 4,
		Information = 8
	}

	private static readonly string SourceName = "System.ComponentModel.Composition";

	public override bool CanWriteInformation => false;

	public override bool CanWriteWarning => Debugger.IsLogging();

	public override bool CanWriteError => Debugger.IsLogging();

	public override void WriteInformation(CompositionTraceId traceId, string format, params object[] arguments)
	{
		WriteEvent(TraceEventType.Information, traceId, format, arguments);
	}

	public override void WriteWarning(CompositionTraceId traceId, string format, params object[] arguments)
	{
		WriteEvent(TraceEventType.Warning, traceId, format, arguments);
	}

	public override void WriteError(CompositionTraceId traceId, string format, params object[] arguments)
	{
		WriteEvent(TraceEventType.Error, traceId, format, arguments);
	}

	private static void WriteEvent(TraceEventType eventType, CompositionTraceId traceId, string format, params object[] arguments)
	{
		if (Debugger.IsLogging())
		{
			string message = CreateLogMessage(eventType, traceId, format, arguments);
			Debugger.Log(0, null, message);
		}
	}

	internal static string CreateLogMessage(TraceEventType eventType, CompositionTraceId traceId, string format, params object[] arguments)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}: {2} : ", SourceName, eventType.ToString(), (int)traceId);
		if (arguments == null)
		{
			stringBuilder.Append(format);
		}
		else
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, format, arguments);
		}
		stringBuilder.AppendLine();
		return stringBuilder.ToString();
	}
}
