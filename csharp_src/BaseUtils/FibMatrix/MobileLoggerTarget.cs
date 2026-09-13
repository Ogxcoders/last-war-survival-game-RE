using System;

namespace FibMatrix;

public class MobileLoggerTarget : ILoggerTarget
{
	private class AsyncReportErrorAction : IDisposable
	{
		private static readonly ObjectPool<AsyncReportErrorAction> _reportPool = new ObjectPool<AsyncReportErrorAction>(10, () => new AsyncReportErrorAction());

		public string message;

		public string stack;

		public static Action AllocateAsyncAction(string message, string stack)
		{
			AsyncReportErrorAction asyncReportErrorAction = _reportPool.Allocate();
			asyncReportErrorAction.message = message;
			asyncReportErrorAction.stack = stack;
			return asyncReportErrorAction.Report;
		}

		public void Report()
		{
			ReportError(message, stack);
			_reportPool.Recycle(this);
		}

		public void Dispose()
		{
		}
	}

	private const string TAG = "Unity";

	public LogTargetType targetType => LogTargetType.Runtime;

	private static void Log(LogLevel level, string tag, string message)
	{
	}

	private static void ReportError(string message, string stack)
	{
	}

	public void Debug(string message, object context, LogImportance importance)
	{
		Log(LogLevel.Debug, "Unity", message);
	}

	public void Info(string message, object context, LogImportance importance)
	{
		if ((targetType & LogTargetType.Runtime) != LogTargetType.None)
		{
			Log(LogLevel.Info, "Unity", message);
		}
	}

	public void Warning(string message, object context, LogImportance importance)
	{
		if ((targetType & LogTargetType.Runtime) != LogTargetType.None)
		{
			Log(LogLevel.Warning, "Unity", message);
		}
	}

	public void Error(string message, string stackTrace, object context, LogImportance importance)
	{
	}
}
