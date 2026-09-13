using System.Diagnostics.Tracing;
using System.Threading;

namespace System.Data;

[EventSource(Name = "System.Data.DataCommonEventSource")]
internal class DataCommonEventSource : EventSource
{
	internal static readonly DataCommonEventSource Log = new DataCommonEventSource();

	private static long s_nextScopeId = 0L;

	private const int TraceEventId = 1;

	private const int EnterScopeId = 2;

	private const int ExitScopeId = 3;

	[Event(1, Level = EventLevel.Informational)]
	internal void Trace(string message)
	{
		WriteEvent(1, message);
	}

	[NonEvent]
	internal void Trace<T0>(string format, T0 arg0)
	{
		if (Log.IsEnabled())
		{
			Trace(string.Format(format, arg0));
		}
	}

	[NonEvent]
	internal void Trace<T0, T1>(string format, T0 arg0, T1 arg1)
	{
		if (Log.IsEnabled())
		{
			Trace(string.Format(format, arg0, arg1));
		}
	}

	[NonEvent]
	internal void Trace<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
	{
		if (Log.IsEnabled())
		{
			Trace(string.Format(format, arg0, arg1, arg2));
		}
	}

	[NonEvent]
	internal void Trace<T0, T1, T2, T3>(string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
	{
		if (Log.IsEnabled())
		{
			Trace(string.Format(format, arg0, arg1, arg2, arg3));
		}
	}

	[NonEvent]
	internal void Trace<T0, T1, T2, T3, T4>(string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		if (Log.IsEnabled())
		{
			Trace(string.Format(format, arg0, arg1, arg2, arg3, arg4));
		}
	}

	[NonEvent]
	internal void Trace<T0, T1, T2, T3, T4, T5, T6>(string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
	{
		if (Log.IsEnabled())
		{
			Trace(string.Format(format, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
		}
	}

	[Event(2, Level = EventLevel.Verbose)]
	internal long EnterScope(string message)
	{
		long num = 0L;
		if (Log.IsEnabled())
		{
			num = Interlocked.Increment(ref s_nextScopeId);
			WriteEvent(2, num, message);
		}
		return num;
	}

	[NonEvent]
	internal long EnterScope<T1>(string format, T1 arg1)
	{
		if (!Log.IsEnabled())
		{
			return 0L;
		}
		return EnterScope(string.Format(format, arg1));
	}

	[NonEvent]
	internal long EnterScope<T1, T2>(string format, T1 arg1, T2 arg2)
	{
		if (!Log.IsEnabled())
		{
			return 0L;
		}
		return EnterScope(string.Format(format, arg1, arg2));
	}

	[NonEvent]
	internal long EnterScope<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
	{
		if (!Log.IsEnabled())
		{
			return 0L;
		}
		return EnterScope(string.Format(format, arg1, arg2, arg3));
	}

	[NonEvent]
	internal long EnterScope<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		if (!Log.IsEnabled())
		{
			return 0L;
		}
		return EnterScope(string.Format(format, arg1, arg2, arg3, arg4));
	}

	[Event(3, Level = EventLevel.Verbose)]
	internal void ExitScope(long scopeId)
	{
		WriteEvent(3, scopeId);
	}
}
