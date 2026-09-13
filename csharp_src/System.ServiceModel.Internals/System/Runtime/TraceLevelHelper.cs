using System.Diagnostics;

namespace System.Runtime;

internal class TraceLevelHelper
{
	private static TraceEventType[] EtwLevelToTraceEventType = new TraceEventType[6]
	{
		TraceEventType.Critical,
		TraceEventType.Critical,
		TraceEventType.Error,
		TraceEventType.Warning,
		TraceEventType.Information,
		TraceEventType.Verbose
	};

	internal static TraceEventType GetTraceEventType(byte level, byte opcode)
	{
		return opcode switch
		{
			1 => TraceEventType.Start, 
			2 => TraceEventType.Stop, 
			8 => TraceEventType.Suspend, 
			7 => TraceEventType.Resume, 
			_ => EtwLevelToTraceEventType[level], 
		};
	}

	internal static TraceEventType GetTraceEventType(TraceEventLevel level)
	{
		return EtwLevelToTraceEventType[(int)level];
	}

	internal static TraceEventType GetTraceEventType(byte level)
	{
		return EtwLevelToTraceEventType[level];
	}

	internal static string LookupSeverity(TraceEventLevel level, TraceEventOpcode opcode)
	{
		return opcode switch
		{
			TraceEventOpcode.Start => "Start", 
			TraceEventOpcode.Stop => "Stop", 
			TraceEventOpcode.Suspend => "Suspend", 
			TraceEventOpcode.Resume => "Resume", 
			_ => level switch
			{
				TraceEventLevel.Critical => "Critical", 
				TraceEventLevel.Error => "Error", 
				TraceEventLevel.Warning => "Warning", 
				TraceEventLevel.Informational => "Information", 
				TraceEventLevel.Verbose => "Verbose", 
				_ => level.ToString(), 
			}, 
		};
	}
}
