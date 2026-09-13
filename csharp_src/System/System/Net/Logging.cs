using System.Diagnostics;

namespace System.Net;

internal static class Logging
{
	internal static bool On => false;

	internal static TraceSource Web => null;

	internal static TraceSource HttpListener => null;

	internal static TraceSource Sockets => null;

	[Conditional("TRACE")]
	internal static void Enter(TraceSource traceSource, object obj, string method, object paramObject)
	{
	}

	[Conditional("TRACE")]
	internal static void Enter(TraceSource traceSource, string msg)
	{
	}

	[Conditional("TRACE")]
	internal static void Enter(TraceSource traceSource, string msg, string parameters)
	{
	}

	[Conditional("TRACE")]
	internal static void Exception(TraceSource traceSource, object obj, string method, Exception e)
	{
	}

	[Conditional("TRACE")]
	internal static void Exit(TraceSource traceSource, object obj, string method, object retObject)
	{
	}

	[Conditional("TRACE")]
	internal static void Exit(TraceSource traceSource, string msg)
	{
	}

	[Conditional("TRACE")]
	internal static void Exit(TraceSource traceSource, string msg, string parameters)
	{
	}

	[Conditional("TRACE")]
	internal static void PrintInfo(TraceSource traceSource, object obj, string method, string msg)
	{
	}

	[Conditional("TRACE")]
	internal static void PrintInfo(TraceSource traceSource, object obj, string msg)
	{
	}

	[Conditional("TRACE")]
	internal static void PrintInfo(TraceSource traceSource, string msg)
	{
	}

	[Conditional("TRACE")]
	internal static void PrintWarning(TraceSource traceSource, object obj, string method, string msg)
	{
	}

	[Conditional("TRACE")]
	internal static void PrintWarning(TraceSource traceSource, string msg)
	{
	}

	[Conditional("TRACE")]
	internal static void PrintError(TraceSource traceSource, string msg)
	{
	}
}
