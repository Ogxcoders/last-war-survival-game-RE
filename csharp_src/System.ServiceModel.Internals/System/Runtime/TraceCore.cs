using System.Globalization;
using System.Resources;
using System.Runtime.Diagnostics;
using System.Security;
using System.Threading;

namespace System.Runtime;

internal class TraceCore
{
	private static ResourceManager resourceManager;

	private static CultureInfo resourceCulture;

	[SecurityCritical]
	private static EventDescriptor[] eventDescriptors;

	private static object syncLock = new object();

	private static volatile bool eventDescriptorsCreated;

	private static ResourceManager ResourceManager
	{
		get
		{
			if (resourceManager == null)
			{
				resourceManager = new ResourceManager("System.Runtime.TraceCore", typeof(TraceCore).Assembly);
			}
			return resourceManager;
		}
	}

	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	private TraceCore()
	{
	}

	internal static bool AppDomainUnloadIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Informational))
		{
			return IsEtwEventEnabled(trace, 0);
		}
		return true;
	}

	internal static void AppDomainUnload(EtwDiagnosticTrace trace, string appdomainName, string processName, string processId)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(trace, 0))
		{
			WriteEtwEvent(trace, 0, null, appdomainName, processName, processId, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Informational))
		{
			string description = string.Format(Culture, ResourceManager.GetString("AppDomainUnload", Culture), appdomainName, processName, processId);
			WriteTraceSource(trace, 0, description, serializedPayload);
		}
	}

	internal static bool HandledExceptionIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Informational))
		{
			return IsEtwEventEnabled(trace, 1);
		}
		return true;
	}

	internal static void HandledException(EtwDiagnosticTrace trace, string param0, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 1))
		{
			WriteEtwEvent(trace, 1, null, param0, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Informational))
		{
			string description = string.Format(Culture, ResourceManager.GetString("HandledException", Culture), param0);
			WriteTraceSource(trace, 1, description, serializedPayload);
		}
	}

	internal static bool ShipAssertExceptionMessageIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Error))
		{
			return IsEtwEventEnabled(trace, 2);
		}
		return true;
	}

	internal static void ShipAssertExceptionMessage(EtwDiagnosticTrace trace, string param0)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(trace, 2))
		{
			WriteEtwEvent(trace, 2, null, param0, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Error))
		{
			string description = string.Format(Culture, ResourceManager.GetString("ShipAssertExceptionMessage", Culture), param0);
			WriteTraceSource(trace, 2, description, serializedPayload);
		}
	}

	internal static bool ThrowingExceptionIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Warning))
		{
			return IsEtwEventEnabled(trace, 3);
		}
		return true;
	}

	internal static void ThrowingException(EtwDiagnosticTrace trace, string param0, string param1, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 3))
		{
			WriteEtwEvent(trace, 3, null, param0, param1, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Warning))
		{
			string description = string.Format(Culture, ResourceManager.GetString("ThrowingException", Culture), param0, param1);
			WriteTraceSource(trace, 3, description, serializedPayload);
		}
	}

	internal static bool UnhandledExceptionIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Critical))
		{
			return IsEtwEventEnabled(trace, 4);
		}
		return true;
	}

	internal static void UnhandledException(EtwDiagnosticTrace trace, string param0, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 4))
		{
			WriteEtwEvent(trace, 4, null, param0, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Critical))
		{
			string description = string.Format(Culture, ResourceManager.GetString("UnhandledException", Culture), param0);
			WriteTraceSource(trace, 4, description, serializedPayload);
		}
	}

	internal static bool TraceCodeEventLogCriticalIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Critical))
		{
			return IsEtwEventEnabled(trace, 5);
		}
		return true;
	}

	internal static void TraceCodeEventLogCritical(EtwDiagnosticTrace trace, TraceRecord traceRecord)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, traceRecord, null);
		if (IsEtwEventEnabled(trace, 5))
		{
			WriteEtwEvent(trace, 5, null, serializedPayload.ExtendedData, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Critical))
		{
			string description = string.Format(Culture, ResourceManager.GetString("TraceCodeEventLogCritical", Culture));
			WriteTraceSource(trace, 5, description, serializedPayload);
		}
	}

	internal static bool TraceCodeEventLogErrorIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Error))
		{
			return IsEtwEventEnabled(trace, 6);
		}
		return true;
	}

	internal static void TraceCodeEventLogError(EtwDiagnosticTrace trace, TraceRecord traceRecord)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, traceRecord, null);
		if (IsEtwEventEnabled(trace, 6))
		{
			WriteEtwEvent(trace, 6, null, serializedPayload.ExtendedData, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Error))
		{
			string description = string.Format(Culture, ResourceManager.GetString("TraceCodeEventLogError", Culture));
			WriteTraceSource(trace, 6, description, serializedPayload);
		}
	}

	internal static bool TraceCodeEventLogInfoIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Informational))
		{
			return IsEtwEventEnabled(trace, 7);
		}
		return true;
	}

	internal static void TraceCodeEventLogInfo(EtwDiagnosticTrace trace, TraceRecord traceRecord)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, traceRecord, null);
		if (IsEtwEventEnabled(trace, 7))
		{
			WriteEtwEvent(trace, 7, null, serializedPayload.ExtendedData, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Informational))
		{
			string description = string.Format(Culture, ResourceManager.GetString("TraceCodeEventLogInfo", Culture));
			WriteTraceSource(trace, 7, description, serializedPayload);
		}
	}

	internal static bool TraceCodeEventLogVerboseIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Verbose))
		{
			return IsEtwEventEnabled(trace, 8);
		}
		return true;
	}

	internal static void TraceCodeEventLogVerbose(EtwDiagnosticTrace trace, TraceRecord traceRecord)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, traceRecord, null);
		if (IsEtwEventEnabled(trace, 8))
		{
			WriteEtwEvent(trace, 8, null, serializedPayload.ExtendedData, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Verbose))
		{
			string description = string.Format(Culture, ResourceManager.GetString("TraceCodeEventLogVerbose", Culture));
			WriteTraceSource(trace, 8, description, serializedPayload);
		}
	}

	internal static bool TraceCodeEventLogWarningIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Warning))
		{
			return IsEtwEventEnabled(trace, 9);
		}
		return true;
	}

	internal static void TraceCodeEventLogWarning(EtwDiagnosticTrace trace, TraceRecord traceRecord)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, traceRecord, null);
		if (IsEtwEventEnabled(trace, 9))
		{
			WriteEtwEvent(trace, 9, null, serializedPayload.ExtendedData, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Warning))
		{
			string description = string.Format(Culture, ResourceManager.GetString("TraceCodeEventLogWarning", Culture));
			WriteTraceSource(trace, 9, description, serializedPayload);
		}
	}

	internal static bool HandledExceptionWarningIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Warning))
		{
			return IsEtwEventEnabled(trace, 10);
		}
		return true;
	}

	internal static void HandledExceptionWarning(EtwDiagnosticTrace trace, string param0, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 10))
		{
			WriteEtwEvent(trace, 10, null, param0, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Warning))
		{
			string description = string.Format(Culture, ResourceManager.GetString("HandledExceptionWarning", Culture), param0);
			WriteTraceSource(trace, 10, description, serializedPayload);
		}
	}

	internal static bool BufferPoolAllocationIsEnabled(EtwDiagnosticTrace trace)
	{
		return IsEtwEventEnabled(trace, 11);
	}

	internal static void BufferPoolAllocation(EtwDiagnosticTrace trace, int Size)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(trace, 11))
		{
			WriteEtwEvent(trace, 11, null, Size, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool BufferPoolChangeQuotaIsEnabled(EtwDiagnosticTrace trace)
	{
		return IsEtwEventEnabled(trace, 12);
	}

	internal static void BufferPoolChangeQuota(EtwDiagnosticTrace trace, int PoolSize, int Delta)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(trace, 12))
		{
			WriteEtwEvent(trace, 12, null, PoolSize, Delta, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool ActionItemScheduledIsEnabled(EtwDiagnosticTrace trace)
	{
		return IsEtwEventEnabled(trace, 13);
	}

	internal static void ActionItemScheduled(EtwDiagnosticTrace trace, EventTraceActivity eventTraceActivity)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(trace, 13))
		{
			WriteEtwEvent(trace, 13, eventTraceActivity, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool ActionItemCallbackInvokedIsEnabled(EtwDiagnosticTrace trace)
	{
		return IsEtwEventEnabled(trace, 14);
	}

	internal static void ActionItemCallbackInvoked(EtwDiagnosticTrace trace, EventTraceActivity eventTraceActivity)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(trace, 14))
		{
			WriteEtwEvent(trace, 14, eventTraceActivity, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool HandledExceptionErrorIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Error))
		{
			return IsEtwEventEnabled(trace, 15);
		}
		return true;
	}

	internal static void HandledExceptionError(EtwDiagnosticTrace trace, string param0, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 15))
		{
			WriteEtwEvent(trace, 15, null, param0, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Error))
		{
			string description = string.Format(Culture, ResourceManager.GetString("HandledExceptionError", Culture), param0);
			WriteTraceSource(trace, 15, description, serializedPayload);
		}
	}

	internal static bool HandledExceptionVerboseIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Verbose))
		{
			return IsEtwEventEnabled(trace, 16);
		}
		return true;
	}

	internal static void HandledExceptionVerbose(EtwDiagnosticTrace trace, string param0, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 16))
		{
			WriteEtwEvent(trace, 16, null, param0, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Verbose))
		{
			string description = string.Format(Culture, ResourceManager.GetString("HandledExceptionVerbose", Culture), param0);
			WriteTraceSource(trace, 16, description, serializedPayload);
		}
	}

	internal static bool EtwUnhandledExceptionIsEnabled(EtwDiagnosticTrace trace)
	{
		return IsEtwEventEnabled(trace, 17);
	}

	internal static void EtwUnhandledException(EtwDiagnosticTrace trace, string param0, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 17))
		{
			WriteEtwEvent(trace, 17, null, param0, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool ThrowingEtwExceptionIsEnabled(EtwDiagnosticTrace trace)
	{
		return IsEtwEventEnabled(trace, 18);
	}

	internal static void ThrowingEtwException(EtwDiagnosticTrace trace, string param0, string param1, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 18))
		{
			WriteEtwEvent(trace, 18, null, param0, param1, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool ThrowingEtwExceptionVerboseIsEnabled(EtwDiagnosticTrace trace)
	{
		return IsEtwEventEnabled(trace, 19);
	}

	internal static void ThrowingEtwExceptionVerbose(EtwDiagnosticTrace trace, string param0, string param1, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 19))
		{
			WriteEtwEvent(trace, 19, null, param0, param1, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool ThrowingExceptionVerboseIsEnabled(EtwDiagnosticTrace trace)
	{
		if (!trace.ShouldTrace(TraceEventLevel.Verbose))
		{
			return IsEtwEventEnabled(trace, 20);
		}
		return true;
	}

	internal static void ThrowingExceptionVerbose(EtwDiagnosticTrace trace, string param0, string param1, Exception exception)
	{
		TracePayload serializedPayload = trace.GetSerializedPayload(null, null, exception);
		if (IsEtwEventEnabled(trace, 20))
		{
			WriteEtwEvent(trace, 20, null, param0, param1, serializedPayload.SerializedException, serializedPayload.AppDomainFriendlyName);
		}
		if (trace.ShouldTraceToTraceSource(TraceEventLevel.Verbose))
		{
			string description = string.Format(Culture, ResourceManager.GetString("ThrowingExceptionVerbose", Culture), param0, param1);
			WriteTraceSource(trace, 20, description, serializedPayload);
		}
	}

	[SecuritySafeCritical]
	private static void CreateEventDescriptors()
	{
		eventDescriptors = new EventDescriptor[21]
		{
			new EventDescriptor(57393, 0, 19, 4, 0, 0, 1152921504606912512L),
			new EventDescriptor(57394, 0, 18, 4, 0, 0, 2305843009213759488L),
			new EventDescriptor(57395, 0, 18, 2, 0, 0, 2305843009213759488L),
			new EventDescriptor(57396, 0, 18, 3, 0, 0, 2305843009213759488L),
			new EventDescriptor(57397, 0, 17, 1, 0, 0, 4611686018427453440L),
			new EventDescriptor(57399, 0, 19, 1, 0, 0, 1152921504606912512L),
			new EventDescriptor(57400, 0, 19, 2, 0, 0, 1152921504606912512L),
			new EventDescriptor(57401, 0, 19, 4, 0, 0, 1152921504606912512L),
			new EventDescriptor(57402, 0, 19, 5, 0, 0, 1152921504606912512L),
			new EventDescriptor(57403, 0, 19, 3, 0, 0, 1152921504606912512L),
			new EventDescriptor(57404, 0, 18, 3, 0, 0, 2305843009213759488L),
			new EventDescriptor(131, 0, 19, 5, 12, 2509, 1152921504606912512L),
			new EventDescriptor(132, 0, 19, 5, 13, 2509, 1152921504606912512L),
			new EventDescriptor(133, 0, 19, 5, 1, 2593, 1152921504608944128L),
			new EventDescriptor(134, 0, 19, 5, 2, 2593, 1152921504608944128L),
			new EventDescriptor(57405, 0, 17, 2, 0, 0, 4611686018427453440L),
			new EventDescriptor(57406, 0, 18, 5, 0, 0, 2305843009213759488L),
			new EventDescriptor(57408, 0, 17, 1, 0, 0, 4611686018427453440L),
			new EventDescriptor(57410, 0, 18, 3, 0, 0, 2305843009213759488L),
			new EventDescriptor(57409, 0, 18, 5, 0, 0, 2305843009213759488L),
			new EventDescriptor(57407, 0, 18, 5, 0, 0, 2305843009213759488L)
		};
	}

	private static void EnsureEventDescriptors()
	{
		if (eventDescriptorsCreated)
		{
			return;
		}
		Monitor.Enter(syncLock);
		try
		{
			if (!eventDescriptorsCreated)
			{
				CreateEventDescriptors();
				eventDescriptorsCreated = true;
			}
		}
		finally
		{
			Monitor.Exit(syncLock);
		}
	}

	[SecuritySafeCritical]
	private static bool IsEtwEventEnabled(EtwDiagnosticTrace trace, int eventIndex)
	{
		if (trace.IsEtwProviderEnabled)
		{
			EnsureEventDescriptors();
			return trace.IsEtwEventEnabled(ref eventDescriptors[eventIndex], fullCheck: false);
		}
		return false;
	}

	[SecuritySafeCritical]
	private static bool WriteEtwEvent(EtwDiagnosticTrace trace, int eventIndex, EventTraceActivity eventParam0, string eventParam1, string eventParam2, string eventParam3, string eventParam4)
	{
		EnsureEventDescriptors();
		return trace.EtwProvider.WriteEvent(ref eventDescriptors[eventIndex], eventParam0, eventParam1, eventParam2, eventParam3, eventParam4);
	}

	[SecuritySafeCritical]
	private static bool WriteEtwEvent(EtwDiagnosticTrace trace, int eventIndex, EventTraceActivity eventParam0, string eventParam1, string eventParam2, string eventParam3)
	{
		EnsureEventDescriptors();
		return trace.EtwProvider.WriteEvent(ref eventDescriptors[eventIndex], eventParam0, eventParam1, eventParam2, eventParam3);
	}

	[SecuritySafeCritical]
	private static bool WriteEtwEvent(EtwDiagnosticTrace trace, int eventIndex, EventTraceActivity eventParam0, string eventParam1, string eventParam2)
	{
		EnsureEventDescriptors();
		return trace.EtwProvider.WriteEvent(ref eventDescriptors[eventIndex], eventParam0, eventParam1, eventParam2);
	}

	[SecuritySafeCritical]
	private static bool WriteEtwEvent(EtwDiagnosticTrace trace, int eventIndex, EventTraceActivity eventParam0, int eventParam1, string eventParam2)
	{
		EnsureEventDescriptors();
		return trace.EtwProvider.WriteEvent(ref eventDescriptors[eventIndex], eventParam0, eventParam1, eventParam2);
	}

	[SecuritySafeCritical]
	private static bool WriteEtwEvent(EtwDiagnosticTrace trace, int eventIndex, EventTraceActivity eventParam0, int eventParam1, int eventParam2, string eventParam3)
	{
		EnsureEventDescriptors();
		return trace.EtwProvider.WriteEvent(ref eventDescriptors[eventIndex], eventParam0, eventParam1, eventParam2, eventParam3);
	}

	[SecuritySafeCritical]
	private static bool WriteEtwEvent(EtwDiagnosticTrace trace, int eventIndex, EventTraceActivity eventParam0, string eventParam1)
	{
		EnsureEventDescriptors();
		return trace.EtwProvider.WriteEvent(ref eventDescriptors[eventIndex], eventParam0, eventParam1);
	}

	[SecuritySafeCritical]
	private static void WriteTraceSource(EtwDiagnosticTrace trace, int eventIndex, string description, TracePayload payload)
	{
		EnsureEventDescriptors();
		trace.WriteTraceSource(ref eventDescriptors[eventIndex], description, payload);
	}
}
