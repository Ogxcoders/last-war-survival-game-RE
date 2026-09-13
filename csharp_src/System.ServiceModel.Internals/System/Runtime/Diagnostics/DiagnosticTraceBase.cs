using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Xml;

namespace System.Runtime.Diagnostics;

internal abstract class DiagnosticTraceBase
{
	protected const string DefaultTraceListenerName = "Default";

	protected const string TraceRecordVersion = "http://schemas.microsoft.com/2004/10/E2ETraceEvent/TraceRecord";

	protected static string AppDomainFriendlyName = AppDomain.CurrentDomain.FriendlyName;

	private const ushort TracingEventLogCategory = 4;

	private object thisLock;

	private bool tracingEnabled = true;

	private bool calledShutdown;

	private bool haveListeners;

	private SourceLevels level;

	protected string TraceSourceName;

	private TraceSource traceSource;

	[SecurityCritical]
	private string eventSourceName;

	protected DateTime LastFailure { get; set; }

	public TraceSource TraceSource
	{
		get
		{
			return traceSource;
		}
		set
		{
			SetTraceSource(value);
		}
	}

	public bool HaveListeners => haveListeners;

	public SourceLevels Level
	{
		get
		{
			if (TraceSource != null && TraceSource.Switch.Level != level)
			{
				level = TraceSource.Switch.Level;
			}
			return level;
		}
		[SecurityCritical]
		set
		{
			SetLevelThreadSafe(value);
		}
	}

	protected string EventSourceName
	{
		[SecuritySafeCritical]
		get
		{
			return eventSourceName;
		}
		[SecurityCritical]
		set
		{
			eventSourceName = value;
		}
	}

	public bool TracingEnabled
	{
		get
		{
			if (tracingEnabled)
			{
				return traceSource != null;
			}
			return false;
		}
	}

	protected static string ProcessName
	{
		[SecuritySafeCritical]
		get
		{
			string text = null;
			using Process process = Process.GetCurrentProcess();
			return process.ProcessName;
		}
	}

	protected static int ProcessId
	{
		[SecuritySafeCritical]
		get
		{
			int num = -1;
			using Process process = Process.GetCurrentProcess();
			return process.Id;
		}
	}

	protected bool CalledShutdown => calledShutdown;

	public static Guid ActivityId
	{
		[SecuritySafeCritical]
		get
		{
			object obj = Trace.CorrelationManager.ActivityId;
			if (obj != null)
			{
				return (Guid)obj;
			}
			return Guid.Empty;
		}
		[SecuritySafeCritical]
		set
		{
			Trace.CorrelationManager.ActivityId = value;
		}
	}

	public DiagnosticTraceBase(string traceSourceName)
	{
		thisLock = new object();
		TraceSourceName = traceSourceName;
		LastFailure = DateTime.MinValue;
	}

	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	private static void UnsafeRemoveDefaultTraceListener(TraceSource traceSource)
	{
		traceSource.Listeners.Remove("Default");
	}

	[SecuritySafeCritical]
	protected void SetTraceSource(TraceSource traceSource)
	{
		if (traceSource != null)
		{
			UnsafeRemoveDefaultTraceListener(traceSource);
			this.traceSource = traceSource;
			haveListeners = this.traceSource.Listeners.Count > 0;
		}
	}

	private SourceLevels FixLevel(SourceLevels level)
	{
		if ((level & ~SourceLevels.Information & SourceLevels.Verbose) != SourceLevels.Off)
		{
			level |= SourceLevels.Verbose;
		}
		else if ((level & ~SourceLevels.Warning & SourceLevels.Information) != SourceLevels.Off)
		{
			level |= SourceLevels.Information;
		}
		else if ((level & ~SourceLevels.Error & SourceLevels.Warning) != SourceLevels.Off)
		{
			level |= SourceLevels.Warning;
		}
		if ((level & ~SourceLevels.Critical & SourceLevels.Error) != SourceLevels.Off)
		{
			level |= SourceLevels.Error;
		}
		if ((level & SourceLevels.Critical) != SourceLevels.Off)
		{
			level |= SourceLevels.Critical;
		}
		if (level == SourceLevels.ActivityTracing)
		{
			level = SourceLevels.Off;
		}
		return level;
	}

	protected virtual void OnSetLevel(SourceLevels level)
	{
	}

	[SecurityCritical]
	private void SetLevel(SourceLevels level)
	{
		SourceLevels sourceLevels = FixLevel(level);
		this.level = sourceLevels;
		if (TraceSource != null)
		{
			haveListeners = TraceSource.Listeners.Count > 0;
			OnSetLevel(level);
			tracingEnabled = HaveListeners && level != SourceLevels.Off;
			TraceSource.Switch.Level = level;
		}
	}

	[SecurityCritical]
	private void SetLevelThreadSafe(SourceLevels level)
	{
		lock (thisLock)
		{
			SetLevel(level);
		}
	}

	public virtual bool ShouldTrace(TraceEventLevel level)
	{
		return ShouldTraceToTraceSource(level);
	}

	public bool ShouldTrace(TraceEventType type)
	{
		if (TracingEnabled && HaveListeners && TraceSource != null)
		{
			return ((uint)type & (uint)Level) != 0;
		}
		return false;
	}

	public bool ShouldTraceToTraceSource(TraceEventLevel level)
	{
		return ShouldTrace(TraceLevelHelper.GetTraceEventType(level));
	}

	public static string XmlEncode(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		int length = text.Length;
		StringBuilder stringBuilder = new StringBuilder(length + 8);
		for (int i = 0; i < length; i++)
		{
			char c = text[i];
			switch (c)
			{
			case '<':
				stringBuilder.Append("&lt;");
				break;
			case '>':
				stringBuilder.Append("&gt;");
				break;
			case '&':
				stringBuilder.Append("&amp;");
				break;
			default:
				stringBuilder.Append(c);
				break;
			}
		}
		return stringBuilder.ToString();
	}

	[SecuritySafeCritical]
	protected void AddDomainEventHandlersForCleanup()
	{
		AppDomain currentDomain = AppDomain.CurrentDomain;
		if (TraceSource != null)
		{
			haveListeners = TraceSource.Listeners.Count > 0;
		}
		tracingEnabled = haveListeners;
		if (TracingEnabled)
		{
			currentDomain.UnhandledException += UnhandledExceptionHandler;
			SetLevel(TraceSource.Switch.Level);
			currentDomain.DomainUnload += ExitOrUnloadEventHandler;
			currentDomain.ProcessExit += ExitOrUnloadEventHandler;
		}
	}

	private void ExitOrUnloadEventHandler(object sender, EventArgs e)
	{
		ShutdownTracing();
	}

	protected abstract void OnUnhandledException(Exception exception);

	protected void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
	{
		Exception exception = (Exception)args.ExceptionObject;
		OnUnhandledException(exception);
		ShutdownTracing();
	}

	protected static string CreateSourceString(object source)
	{
		if (source is ITraceSourceStringProvider traceSourceStringProvider)
		{
			return traceSourceStringProvider.GetSourceString();
		}
		return CreateDefaultSourceString(source);
	}

	internal static string CreateDefaultSourceString(object source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return string.Format(CultureInfo.CurrentCulture, "{0}/{1}", source.GetType().ToString(), source.GetHashCode());
	}

	protected static void AddExceptionToTraceString(XmlWriter xml, Exception exception)
	{
		xml.WriteElementString("ExceptionType", XmlEncode(exception.GetType().AssemblyQualifiedName));
		xml.WriteElementString("Message", XmlEncode(exception.Message));
		xml.WriteElementString("StackTrace", XmlEncode(StackTraceString(exception)));
		xml.WriteElementString("ExceptionString", XmlEncode(exception.ToString()));
		if (exception is Win32Exception ex)
		{
			xml.WriteElementString("NativeErrorCode", ex.NativeErrorCode.ToString("X", CultureInfo.InvariantCulture));
		}
		if (exception.Data != null && exception.Data.Count > 0)
		{
			xml.WriteStartElement("DataItems");
			foreach (object key in exception.Data.Keys)
			{
				xml.WriteStartElement("Data");
				xml.WriteElementString("Key", XmlEncode(key.ToString()));
				xml.WriteElementString("Value", XmlEncode(exception.Data[key].ToString()));
				xml.WriteEndElement();
			}
			xml.WriteEndElement();
		}
		if (exception.InnerException != null)
		{
			xml.WriteStartElement("InnerException");
			AddExceptionToTraceString(xml, exception.InnerException);
			xml.WriteEndElement();
		}
	}

	protected static string StackTraceString(Exception exception)
	{
		string text = exception.StackTrace;
		if (string.IsNullOrEmpty(text))
		{
			StackFrame[] frames = new StackTrace(fNeedFileInfo: false).GetFrames();
			int num = 0;
			bool flag = false;
			StackFrame[] array = frames;
			for (int i = 0; i < array.Length; i++)
			{
				string name = array[i].GetMethod().Name;
				switch (name)
				{
				case "StackTraceString":
				case "AddExceptionToTraceString":
				case "BuildTrace":
				case "TraceEvent":
				case "TraceException":
				case "GetAdditionalPayload":
					num++;
					break;
				default:
					if (name.StartsWith("ThrowHelper", StringComparison.Ordinal))
					{
						num++;
					}
					else
					{
						flag = true;
					}
					break;
				}
				if (flag)
				{
					break;
				}
			}
			text = new StackTrace(num, fNeedFileInfo: false).ToString();
		}
		return text;
	}

	[SecuritySafeCritical]
	protected void LogTraceFailure(string traceString, Exception exception)
	{
		TimeSpan timeSpan = TimeSpan.FromMinutes(10.0);
		try
		{
			lock (thisLock)
			{
				if (DateTime.UtcNow.Subtract(LastFailure) >= timeSpan)
				{
					LastFailure = DateTime.UtcNow;
					EventLogger eventLogger = EventLogger.UnsafeCreateEventLogger(eventSourceName, this);
					if (exception == null)
					{
						eventLogger.UnsafeLogEvent(TraceEventType.Error, 4, 3221291112u, false, traceString);
					}
					else
					{
						eventLogger.UnsafeLogEvent(TraceEventType.Error, 4, 3221291113u, false, traceString, exception.ToString());
					}
				}
			}
		}
		catch (Exception exception2)
		{
			if (Fx.IsFatal(exception2))
			{
				throw;
			}
		}
	}

	protected abstract void OnShutdownTracing();

	private void ShutdownTracing()
	{
		if (calledShutdown)
		{
			return;
		}
		calledShutdown = true;
		try
		{
			OnShutdownTracing();
		}
		catch (Exception exception)
		{
			if (Fx.IsFatal(exception))
			{
				throw;
			}
			LogTraceFailure(null, exception);
		}
	}

	protected static string LookupSeverity(TraceEventType type)
	{
		return type switch
		{
			TraceEventType.Critical => "Critical", 
			TraceEventType.Error => "Error", 
			TraceEventType.Warning => "Warning", 
			TraceEventType.Information => "Information", 
			TraceEventType.Verbose => "Verbose", 
			TraceEventType.Start => "Start", 
			TraceEventType.Stop => "Stop", 
			TraceEventType.Suspend => "Suspend", 
			TraceEventType.Transfer => "Transfer", 
			_ => type.ToString(), 
		};
	}

	public abstract bool IsEnabled();

	public abstract void TraceEventLogEvent(TraceEventType type, TraceRecord traceRecord);
}
