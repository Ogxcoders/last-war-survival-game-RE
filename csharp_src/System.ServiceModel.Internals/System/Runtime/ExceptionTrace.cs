using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Diagnostics;
using System.Security;

namespace System.Runtime;

internal class ExceptionTrace
{
	private const ushort FailFastEventLogCategory = 6;

	private string eventSourceName;

	private readonly EtwDiagnosticTrace diagnosticTrace;

	public ExceptionTrace(string eventSourceName, EtwDiagnosticTrace diagnosticTrace)
	{
		this.eventSourceName = eventSourceName;
		this.diagnosticTrace = diagnosticTrace;
	}

	public void AsInformation(Exception exception)
	{
		TraceCore.HandledException(diagnosticTrace, (exception != null) ? exception.ToString() : string.Empty, exception);
	}

	public void AsWarning(Exception exception)
	{
		TraceCore.HandledExceptionWarning(diagnosticTrace, (exception != null) ? exception.ToString() : string.Empty, exception);
	}

	public Exception AsError(Exception exception)
	{
		if (exception is AggregateException aggregateException)
		{
			return AsError<Exception>(aggregateException);
		}
		if (exception is TargetInvocationException { InnerException: not null } ex)
		{
			return AsError(ex.InnerException);
		}
		return TraceException(exception);
	}

	public Exception AsError(Exception exception, string eventSource)
	{
		if (exception is AggregateException aggregateException)
		{
			return AsError<Exception>(aggregateException, eventSource);
		}
		if (exception is TargetInvocationException { InnerException: not null } ex)
		{
			return AsError(ex.InnerException, eventSource);
		}
		return TraceException(exception, eventSource);
	}

	public Exception AsError(TargetInvocationException targetInvocationException, string eventSource)
	{
		if (Fx.IsFatal(targetInvocationException))
		{
			return targetInvocationException;
		}
		Exception innerException = targetInvocationException.InnerException;
		if (innerException != null)
		{
			return AsError(innerException, eventSource);
		}
		return TraceException((Exception)targetInvocationException, eventSource);
	}

	public Exception AsError<TPreferredException>(AggregateException aggregateException)
	{
		return AsError<TPreferredException>(aggregateException, eventSourceName);
	}

	public Exception AsError<TPreferredException>(AggregateException aggregateException, string eventSource)
	{
		if (Fx.IsFatal(aggregateException))
		{
			return aggregateException;
		}
		ReadOnlyCollection<Exception> innerExceptions = aggregateException.Flatten().InnerExceptions;
		if (innerExceptions.Count == 0)
		{
			return TraceException(aggregateException, eventSource);
		}
		Exception ex = null;
		foreach (Exception item in innerExceptions)
		{
			Exception ex2 = ((item is TargetInvocationException { InnerException: not null } ex3) ? ex3.InnerException : item);
			if (ex2 is TPreferredException && ex == null)
			{
				ex = ex2;
			}
			TraceException(ex2, eventSource);
		}
		if (ex == null)
		{
			ex = innerExceptions[0];
		}
		return ex;
	}

	public ArgumentException Argument(string paramName, string message)
	{
		return TraceException(new ArgumentException(message, paramName));
	}

	public ArgumentNullException ArgumentNull(string paramName)
	{
		return TraceException(new ArgumentNullException(paramName));
	}

	public ArgumentNullException ArgumentNull(string paramName, string message)
	{
		return TraceException(new ArgumentNullException(paramName, message));
	}

	public ArgumentException ArgumentNullOrEmpty(string paramName)
	{
		return Argument(paramName, InternalSR.ArgumentNullOrEmpty(paramName));
	}

	public ArgumentOutOfRangeException ArgumentOutOfRange(string paramName, object actualValue, string message)
	{
		return TraceException(new ArgumentOutOfRangeException(paramName, actualValue, message));
	}

	public ObjectDisposedException ObjectDisposed(string message)
	{
		return TraceException(new ObjectDisposedException(null, message));
	}

	public void TraceUnhandledException(Exception exception)
	{
		TraceCore.UnhandledException(diagnosticTrace, (exception != null) ? exception.ToString() : string.Empty, exception);
	}

	public void TraceHandledException(Exception exception, TraceEventType traceEventType)
	{
		switch (traceEventType)
		{
		case TraceEventType.Error:
			if (TraceCore.HandledExceptionErrorIsEnabled(diagnosticTrace))
			{
				TraceCore.HandledExceptionError(diagnosticTrace, (exception != null) ? exception.ToString() : string.Empty, exception);
			}
			break;
		case TraceEventType.Warning:
			if (TraceCore.HandledExceptionWarningIsEnabled(diagnosticTrace))
			{
				TraceCore.HandledExceptionWarning(diagnosticTrace, (exception != null) ? exception.ToString() : string.Empty, exception);
			}
			break;
		case TraceEventType.Verbose:
			if (TraceCore.HandledExceptionVerboseIsEnabled(diagnosticTrace))
			{
				TraceCore.HandledExceptionVerbose(diagnosticTrace, (exception != null) ? exception.ToString() : string.Empty, exception);
			}
			break;
		default:
			if (TraceCore.HandledExceptionIsEnabled(diagnosticTrace))
			{
				TraceCore.HandledException(diagnosticTrace, (exception != null) ? exception.ToString() : string.Empty, exception);
			}
			break;
		}
	}

	public void TraceEtwException(Exception exception, TraceEventType eventType)
	{
		switch (eventType)
		{
		case TraceEventType.Error:
		case TraceEventType.Warning:
			if (TraceCore.ThrowingEtwExceptionIsEnabled(diagnosticTrace))
			{
				TraceCore.ThrowingEtwException(diagnosticTrace, eventSourceName, (exception != null) ? exception.ToString() : string.Empty, exception);
			}
			break;
		case TraceEventType.Critical:
			if (TraceCore.EtwUnhandledExceptionIsEnabled(diagnosticTrace))
			{
				TraceCore.EtwUnhandledException(diagnosticTrace, (exception != null) ? exception.ToString() : string.Empty, exception);
			}
			break;
		default:
			if (TraceCore.ThrowingEtwExceptionVerboseIsEnabled(diagnosticTrace))
			{
				TraceCore.ThrowingEtwExceptionVerbose(diagnosticTrace, eventSourceName, (exception != null) ? exception.ToString() : string.Empty, exception);
			}
			break;
		}
	}

	private TException TraceException<TException>(TException exception) where TException : Exception
	{
		return TraceException(exception, eventSourceName);
	}

	[SecuritySafeCritical]
	private TException TraceException<TException>(TException exception, string eventSource) where TException : Exception
	{
		if (TraceCore.ThrowingExceptionIsEnabled(diagnosticTrace))
		{
			TraceCore.ThrowingException(diagnosticTrace, eventSource, (exception != null) ? exception.ToString() : string.Empty, exception);
		}
		BreakOnException(exception);
		return exception;
	}

	[SecuritySafeCritical]
	private void BreakOnException(Exception exception)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void TraceFailFast(string message)
	{
		EventLogger eventLogger = null;
		eventLogger = new EventLogger(eventSourceName, diagnosticTrace);
		TraceFailFast(message, eventLogger);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void TraceFailFast(string message, EventLogger logger)
	{
		if (logger == null)
		{
			return;
		}
		try
		{
			string text = null;
			try
			{
				text = new StackTrace().ToString();
			}
			catch (Exception ex)
			{
				text = ex.Message;
				if (Fx.IsFatal(ex))
				{
					throw;
				}
			}
			finally
			{
				logger.LogEvent(TraceEventType.Critical, 6, 3221291110u, message, text);
			}
		}
		catch (Exception ex2)
		{
			logger.LogEvent(TraceEventType.Critical, 6, 3221291111u, ex2.ToString());
			if (Fx.IsFatal(ex2))
			{
				throw;
			}
		}
	}
}
