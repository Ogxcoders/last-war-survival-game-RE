using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Interop;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

namespace System.Runtime.Diagnostics;

internal sealed class EventLogger
{
	private const int MaxEventLogsInPT = 5;

	[SecurityCritical]
	private static int logCountForPT;

	private static bool canLogEvent = true;

	private DiagnosticTraceBase diagnosticTrace;

	[SecurityCritical]
	private string eventLogSourceName;

	private bool isInPartialTrust;

	private EventLogger()
	{
		isInPartialTrust = IsInPartialTrust();
	}

	[Obsolete("For System.Runtime.dll use only. Call FxTrace.EventLog instead")]
	public EventLogger(string eventLogSourceName, DiagnosticTraceBase diagnosticTrace)
	{
		try
		{
			this.diagnosticTrace = diagnosticTrace;
			if (canLogEvent)
			{
				SafeSetLogSourceName(eventLogSourceName);
			}
		}
		catch (SecurityException)
		{
			canLogEvent = false;
		}
	}

	[SecurityCritical]
	public static EventLogger UnsafeCreateEventLogger(string eventLogSourceName, DiagnosticTraceBase diagnosticTrace)
	{
		EventLogger eventLogger = new EventLogger();
		eventLogger.SetLogSourceName(eventLogSourceName, diagnosticTrace);
		return eventLogger;
	}

	[SecurityCritical]
	public void UnsafeLogEvent(TraceEventType type, ushort eventLogCategory, uint eventId, bool shouldTrace, params string[] values)
	{
		if (logCountForPT >= 5)
		{
			return;
		}
		try
		{
			int num = 0;
			string[] array = new string[values.Length + 2];
			for (int i = 0; i < values.Length; i++)
			{
				string text = values[i];
				num += (array[i] = (string.IsNullOrEmpty(text) ? string.Empty : NormalizeEventLogParameter(text))).Length + 1;
			}
			string text2 = NormalizeEventLogParameter(UnsafeGetProcessName());
			array[^2] = text2;
			num += text2.Length + 1;
			string text3 = UnsafeGetProcessId().ToString(CultureInfo.InvariantCulture);
			array[^1] = text3;
			num += text3.Length + 1;
			if (num > 25600)
			{
				int num2 = 25600 / array.Length - 1;
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j].Length > num2)
					{
						array[j] = array[j].Substring(0, num2);
					}
				}
			}
			SecurityIdentifier user = WindowsIdentity.GetCurrent().User;
			byte[] array2 = new byte[user.BinaryLength];
			user.GetBinaryForm(array2, 0);
			IntPtr[] array3 = new IntPtr[array.Length];
			GCHandle stringsRootHandle = default(GCHandle);
			GCHandle[] array4 = null;
			try
			{
				stringsRootHandle = GCHandle.Alloc(array3, GCHandleType.Pinned);
				array4 = new GCHandle[array.Length];
				for (int k = 0; k < array.Length; k++)
				{
					array4[k] = GCHandle.Alloc(array[k], GCHandleType.Pinned);
					array3[k] = array4[k].AddrOfPinnedObject();
				}
				UnsafeWriteEventLog(type, eventLogCategory, eventId, array, array2, stringsRootHandle);
			}
			finally
			{
				if (stringsRootHandle.AddrOfPinnedObject() != IntPtr.Zero)
				{
					stringsRootHandle.Free();
				}
				if (array4 != null)
				{
					GCHandle[] array5 = array4;
					foreach (GCHandle gCHandle in array5)
					{
						gCHandle.Free();
					}
				}
			}
			if (shouldTrace && diagnosticTrace != null && diagnosticTrace.IsEnabled())
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>(array.Length + 4);
				dictionary["CategoryID.Name"] = "EventLogCategory";
				dictionary["CategoryID.Value"] = eventLogCategory.ToString(CultureInfo.InvariantCulture);
				dictionary["InstanceID.Name"] = "EventId";
				dictionary["InstanceID.Value"] = eventId.ToString(CultureInfo.InvariantCulture);
				for (int m = 0; m < values.Length; m++)
				{
					dictionary.Add("Value" + m.ToString(CultureInfo.InvariantCulture), (values[m] == null) ? string.Empty : DiagnosticTraceBase.XmlEncode(values[m]));
				}
				diagnosticTrace.TraceEventLogEvent(type, new DictionaryTraceRecord(dictionary));
			}
		}
		catch (Exception exception)
		{
			if (Fx.IsFatal(exception))
			{
				throw;
			}
		}
		if (isInPartialTrust)
		{
			logCountForPT++;
		}
	}

	public void LogEvent(TraceEventType type, ushort eventLogCategory, uint eventId, bool shouldTrace, params string[] values)
	{
		if (!canLogEvent)
		{
			return;
		}
		try
		{
			SafeLogEvent(type, eventLogCategory, eventId, shouldTrace, values);
		}
		catch (SecurityException exception)
		{
			canLogEvent = false;
			if (shouldTrace)
			{
				Fx.Exception.TraceHandledException(exception, TraceEventType.Information);
			}
		}
	}

	public void LogEvent(TraceEventType type, ushort eventLogCategory, uint eventId, params string[] values)
	{
		LogEvent(type, eventLogCategory, eventId, shouldTrace: true, values);
	}

	private static EventLogEntryType EventLogEntryTypeFromEventType(TraceEventType type)
	{
		EventLogEntryType result = EventLogEntryType.Information;
		switch (type)
		{
		case TraceEventType.Critical:
		case TraceEventType.Error:
			result = EventLogEntryType.Error;
			break;
		case TraceEventType.Warning:
			result = EventLogEntryType.Warning;
			break;
		}
		return result;
	}

	[SecuritySafeCritical]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	private void SafeLogEvent(TraceEventType type, ushort eventLogCategory, uint eventId, bool shouldTrace, params string[] values)
	{
		UnsafeLogEvent(type, eventLogCategory, eventId, shouldTrace, values);
	}

	[SecuritySafeCritical]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	private void SafeSetLogSourceName(string eventLogSourceName)
	{
		this.eventLogSourceName = eventLogSourceName;
	}

	[SecurityCritical]
	private void SetLogSourceName(string eventLogSourceName, DiagnosticTraceBase diagnosticTrace)
	{
		this.eventLogSourceName = eventLogSourceName;
		this.diagnosticTrace = diagnosticTrace;
	}

	[SecuritySafeCritical]
	private bool IsInPartialTrust()
	{
		bool result = false;
		try
		{
			using Process process = Process.GetCurrentProcess();
			result = string.IsNullOrEmpty(process.ProcessName);
		}
		catch (SecurityException)
		{
			result = true;
		}
		return result;
	}

	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	private void UnsafeWriteEventLog(TraceEventType type, ushort eventLogCategory, uint eventId, string[] logValues, byte[] sidBA, GCHandle stringsRootHandle)
	{
		using SafeEventLogWriteHandle safeEventLogWriteHandle = SafeEventLogWriteHandle.RegisterEventSource(null, eventLogSourceName);
		if (safeEventLogWriteHandle != null)
		{
			UnsafeNativeMethods.ReportEvent(strings: new HandleRef(safeEventLogWriteHandle, stringsRootHandle.AddrOfPinnedObject()), hEventLog: safeEventLogWriteHandle, type: (ushort)EventLogEntryTypeFromEventType(type), category: eventLogCategory, eventID: eventId, userSID: sidBA, numStrings: (ushort)logValues.Length, dataLen: 0u, rawData: null);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	private string UnsafeGetProcessName()
	{
		string text = null;
		using Process process = Process.GetCurrentProcess();
		return process.ProcessName;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	private int UnsafeGetProcessId()
	{
		int num = -1;
		using Process process = Process.GetCurrentProcess();
		return process.Id;
	}

	internal static string NormalizeEventLogParameter(string eventLogParameter)
	{
		if (eventLogParameter.IndexOf('%') < 0)
		{
			return eventLogParameter;
		}
		StringBuilder stringBuilder = null;
		int length = eventLogParameter.Length;
		for (int i = 0; i < length; i++)
		{
			char c = eventLogParameter[i];
			if (c != '%')
			{
				stringBuilder?.Append(c);
				continue;
			}
			if (i + 1 >= length)
			{
				stringBuilder?.Append(c);
				continue;
			}
			if (eventLogParameter[i + 1] < '0' || eventLogParameter[i + 1] > '9')
			{
				stringBuilder?.Append(c);
				continue;
			}
			if (stringBuilder == null)
			{
				stringBuilder = new StringBuilder(length + 2);
				for (int j = 0; j < i; j++)
				{
					stringBuilder.Append(eventLogParameter[j]);
				}
			}
			stringBuilder.Append(c);
			stringBuilder.Append(' ');
		}
		if (stringBuilder == null)
		{
			return eventLogParameter;
		}
		return stringBuilder.ToString();
	}
}
