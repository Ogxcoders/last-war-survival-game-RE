using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.ServiceModel.Internals;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace System.Runtime.Diagnostics;

internal sealed class EtwDiagnosticTrace : DiagnosticTraceBase
{
	private static class TraceCodes
	{
		public const string AppDomainUnload = "AppDomainUnload";

		public const string TraceHandledException = "TraceHandledException";

		public const string ThrowingException = "ThrowingException";

		public const string UnhandledException = "UnhandledException";
	}

	private static class EventIdsWithMsdnTraceCode
	{
		public const int AppDomainUnload = 57393;

		public const int ThrowingExceptionWarning = 57396;

		public const int ThrowingExceptionVerbose = 57407;

		public const int HandledExceptionInfo = 57394;

		public const int HandledExceptionWarning = 57404;

		public const int HandledExceptionError = 57405;

		public const int HandledExceptionVerbose = 57406;

		public const int UnhandledException = 57397;
	}

	private static class LegacyTraceEventIds
	{
		public const int Diagnostics = 131072;

		public const int AppDomainUnload = 131073;

		public const int EventLog = 131074;

		public const int ThrowingException = 131075;

		public const int TraceHandledException = 131076;

		public const int UnhandledException = 131077;
	}

	private static class StringBuilderPool
	{
		private const int maxPooledStringBuilders = 64;

		private static readonly ConcurrentQueue<StringBuilder> freeStringBuilders = new ConcurrentQueue<StringBuilder>();

		public static StringBuilder Take()
		{
			StringBuilder result = null;
			if (freeStringBuilders.TryDequeue(out result))
			{
				return result;
			}
			return new StringBuilder();
		}

		public static void Return(StringBuilder sb)
		{
			if (freeStringBuilders.Count <= 64)
			{
				sb.Clear();
				freeStringBuilders.Enqueue(sb);
			}
		}
	}

	private const int WindowsVistaMajorNumber = 6;

	private const string EventSourceVersion = "4.0.0.0";

	private const ushort TracingEventLogCategory = 4;

	private const int MaxExceptionStringLength = 28672;

	private const int MaxExceptionDepth = 64;

	private const string DiagnosticTraceSource = "System.ServiceModel.Diagnostics";

	private const int XmlBracketsLength = 5;

	private const int XmlBracketsLengthForNullValue = 4;

	public static readonly Guid ImmutableDefaultEtwProviderId;

	[SecurityCritical]
	private static Guid defaultEtwProviderId;

	private static Hashtable etwProviderCache;

	private static bool isVistaOrGreater;

	private static Func<string> traceAnnotation;

	[SecurityCritical]
	private EtwProvider etwProvider;

	private Guid etwProviderId;

	[SecurityCritical]
	private static EventDescriptor transferEventDescriptor;

	public static Guid DefaultEtwProviderId
	{
		[SecuritySafeCritical]
		get
		{
			return defaultEtwProviderId;
		}
		[SecurityCritical]
		set
		{
			defaultEtwProviderId = value;
		}
	}

	public EtwProvider EtwProvider
	{
		[SecurityCritical]
		get
		{
			return etwProvider;
		}
	}

	public bool IsEtwProviderEnabled
	{
		[SecuritySafeCritical]
		get
		{
			if (EtwTracingEnabled)
			{
				return etwProvider.IsEnabled();
			}
			return false;
		}
	}

	public Action RefreshState
	{
		[SecuritySafeCritical]
		get
		{
			return EtwProvider.ControllerCallBack;
		}
		[SecuritySafeCritical]
		set
		{
			EtwProvider.ControllerCallBack = value;
		}
	}

	public bool IsEnd2EndActivityTracingEnabled
	{
		[SecuritySafeCritical]
		get
		{
			if (IsEtwProviderEnabled)
			{
				return EtwProvider.IsEnd2EndActivityTracingEnabled;
			}
			return false;
		}
	}

	private bool EtwTracingEnabled
	{
		[SecuritySafeCritical]
		get
		{
			return etwProvider != null;
		}
	}

	[SecurityCritical]
	static EtwDiagnosticTrace()
	{
		ImmutableDefaultEtwProviderId = new Guid("{c651f5f6-1c0d-492e-8ae1-b4efd7c9d503}");
		defaultEtwProviderId = ImmutableDefaultEtwProviderId;
		etwProviderCache = new Hashtable();
		isVistaOrGreater = Environment.OSVersion.Version.Major >= 6;
		transferEventDescriptor = new EventDescriptor(499, 0, 18, 0, 0, 0, 2305843009215397989L);
		if (!PartialTrustHelpers.HasEtwPermissions())
		{
			defaultEtwProviderId = Guid.Empty;
		}
	}

	[SecurityCritical]
	public EtwDiagnosticTrace(string traceSourceName, Guid etwProviderId)
		: base(traceSourceName)
	{
		try
		{
			TraceSourceName = traceSourceName;
			base.EventSourceName = TraceSourceName + " " + "4.0.0.0";
			CreateTraceSource();
		}
		catch (Exception ex)
		{
			if (Fx.IsFatal(ex))
			{
				throw;
			}
			new EventLogger(base.EventSourceName, null).LogEvent(TraceEventType.Error, 4, 3221291108u, false, ex.ToString());
		}
		try
		{
			CreateEtwProvider(etwProviderId);
		}
		catch (Exception ex2)
		{
			if (Fx.IsFatal(ex2))
			{
				throw;
			}
			etwProvider = null;
			new EventLogger(base.EventSourceName, null).LogEvent(TraceEventType.Error, 4, 3221291108u, false, ex2.ToString());
		}
		if (base.TracingEnabled || EtwTracingEnabled)
		{
			AddDomainEventHandlersForCleanup();
		}
	}

	[SecuritySafeCritical]
	public void SetEnd2EndActivityTracingEnabled(bool isEnd2EndTracingEnabled)
	{
		EtwProvider.SetEnd2EndActivityTracingEnabled(isEnd2EndTracingEnabled);
	}

	public void SetAnnotation(Func<string> annotation)
	{
		traceAnnotation = annotation;
	}

	public override bool ShouldTrace(TraceEventLevel level)
	{
		if (!base.ShouldTrace(level))
		{
			return ShouldTraceToEtw(level);
		}
		return true;
	}

	[SecuritySafeCritical]
	public bool ShouldTraceToEtw(TraceEventLevel level)
	{
		if (EtwProvider != null)
		{
			return EtwProvider.IsEnabled((byte)level, 0L);
		}
		return false;
	}

	[SecuritySafeCritical]
	public void Event(int eventId, TraceEventLevel traceEventLevel, TraceChannel channel, string description)
	{
		if (base.TracingEnabled)
		{
			EventDescriptor eventDescriptor = GetEventDescriptor(eventId, channel, traceEventLevel);
			Event(ref eventDescriptor, description);
		}
	}

	[SecurityCritical]
	public void Event(ref EventDescriptor eventDescriptor, string description)
	{
		if (base.TracingEnabled)
		{
			TracePayload serializedPayload = GetSerializedPayload(null, null, null);
			WriteTraceSource(ref eventDescriptor, description, serializedPayload);
		}
	}

	public void SetAndTraceTransfer(Guid newId, bool emitTransfer)
	{
		if (emitTransfer)
		{
			TraceTransfer(newId);
		}
		DiagnosticTraceBase.ActivityId = newId;
	}

	[SecuritySafeCritical]
	public void TraceTransfer(Guid newId)
	{
		Guid activityId = DiagnosticTraceBase.ActivityId;
		if (!(newId != activityId))
		{
			return;
		}
		try
		{
			_ = base.HaveListeners;
			if (IsEtwEventEnabled(ref transferEventDescriptor, fullCheck: false))
			{
				etwProvider.WriteTransferEvent(ref transferEventDescriptor, new EventTraceActivity(activityId), newId, (traceAnnotation == null) ? string.Empty : traceAnnotation(), DiagnosticTraceBase.AppDomainFriendlyName);
			}
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

	[SecurityCritical]
	public void WriteTraceSource(ref EventDescriptor eventDescriptor, string description, TracePayload payload)
	{
		if (!base.TracingEnabled)
		{
			return;
		}
		XPathNavigator xPathNavigator = null;
		try
		{
			GenerateLegacyTraceCode(ref eventDescriptor, out var msdnTraceCode, out var _);
			string xml = BuildTrace(ref eventDescriptor, description, payload, msdnTraceCode);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			xPathNavigator = xmlDocument.CreateNavigator();
			if (base.CalledShutdown)
			{
				base.TraceSource.Flush();
			}
		}
		catch (Exception exception)
		{
			if (Fx.IsFatal(exception))
			{
				throw;
			}
			LogTraceFailure((xPathNavigator == null) ? string.Empty : xPathNavigator.ToString(), exception);
		}
	}

	[SecurityCritical]
	private static string BuildTrace(ref EventDescriptor eventDescriptor, string description, TracePayload payload, string msdnTraceCode)
	{
		StringBuilder stringBuilder = StringBuilderPool.Take();
		try
		{
			using StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.CurrentCulture);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteStartElement("TraceRecord");
			xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/2004/10/E2ETraceEvent/TraceRecord");
			xmlTextWriter.WriteAttributeString("Severity", TraceLevelHelper.LookupSeverity((TraceEventLevel)eventDescriptor.Level, (TraceEventOpcode)eventDescriptor.Opcode));
			xmlTextWriter.WriteAttributeString("Channel", LookupChannel((TraceChannel)eventDescriptor.Channel));
			xmlTextWriter.WriteElementString("TraceIdentifier", msdnTraceCode);
			xmlTextWriter.WriteElementString("Description", description);
			xmlTextWriter.WriteElementString("AppDomain", payload.AppDomainFriendlyName);
			if (!string.IsNullOrEmpty(payload.EventSource))
			{
				xmlTextWriter.WriteElementString("Source", payload.EventSource);
			}
			if (!string.IsNullOrEmpty(payload.ExtendedData))
			{
				xmlTextWriter.WriteRaw(payload.ExtendedData);
			}
			if (!string.IsNullOrEmpty(payload.SerializedException))
			{
				xmlTextWriter.WriteRaw(payload.SerializedException);
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
			stringWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			StringBuilderPool.Return(stringBuilder);
		}
	}

	[SecurityCritical]
	private static void GenerateLegacyTraceCode(ref EventDescriptor eventDescriptor, out string msdnTraceCode, out int legacyEventId)
	{
		switch (eventDescriptor.EventId)
		{
		case 57393:
			msdnTraceCode = GenerateMsdnTraceCode("System.ServiceModel.Diagnostics", "AppDomainUnload");
			legacyEventId = 131073;
			break;
		case 57394:
		case 57404:
		case 57405:
		case 57406:
			msdnTraceCode = GenerateMsdnTraceCode("System.ServiceModel.Diagnostics", "TraceHandledException");
			legacyEventId = 131076;
			break;
		case 57396:
		case 57407:
			msdnTraceCode = GenerateMsdnTraceCode("System.ServiceModel.Diagnostics", "ThrowingException");
			legacyEventId = 131075;
			break;
		case 57397:
			msdnTraceCode = GenerateMsdnTraceCode("System.ServiceModel.Diagnostics", "UnhandledException");
			legacyEventId = 131077;
			break;
		default:
			msdnTraceCode = eventDescriptor.EventId.ToString(CultureInfo.InvariantCulture);
			legacyEventId = eventDescriptor.EventId;
			break;
		}
	}

	private static string GenerateMsdnTraceCode(string traceSource, string traceCodeString)
	{
		return string.Format(CultureInfo.InvariantCulture, "http://msdn.microsoft.com/{0}/library/{1}.{2}.aspx", CultureInfo.CurrentCulture.Name, traceSource, traceCodeString);
	}

	private static string LookupChannel(TraceChannel traceChannel)
	{
		return traceChannel switch
		{
			TraceChannel.Admin => "Admin", 
			TraceChannel.Analytic => "Analytic", 
			TraceChannel.Application => "Application", 
			TraceChannel.Debug => "Debug", 
			TraceChannel.Operational => "Operational", 
			TraceChannel.Perf => "Perf", 
			_ => traceChannel.ToString(), 
		};
	}

	public TracePayload GetSerializedPayload(object source, TraceRecord traceRecord, Exception exception)
	{
		return GetSerializedPayload(source, traceRecord, exception, getServiceReference: false);
	}

	public TracePayload GetSerializedPayload(object source, TraceRecord traceRecord, Exception exception, bool getServiceReference)
	{
		string eventSource = null;
		string extendedData = null;
		string serializedException = null;
		if (source != null)
		{
			eventSource = DiagnosticTraceBase.CreateSourceString(source);
		}
		if (traceRecord != null)
		{
			StringBuilder stringBuilder = StringBuilderPool.Take();
			try
			{
				using StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.CurrentCulture);
				using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
				xmlTextWriter.WriteStartElement("ExtendedData");
				traceRecord.WriteTo(xmlTextWriter);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.Flush();
				stringWriter.Flush();
				extendedData = stringBuilder.ToString();
			}
			finally
			{
				StringBuilderPool.Return(stringBuilder);
			}
		}
		if (exception != null)
		{
			serializedException = ExceptionToTraceString(exception, 28672);
		}
		if (getServiceReference && traceAnnotation != null)
		{
			return new TracePayload(serializedException, eventSource, DiagnosticTraceBase.AppDomainFriendlyName, extendedData, traceAnnotation());
		}
		return new TracePayload(serializedException, eventSource, DiagnosticTraceBase.AppDomainFriendlyName, extendedData, string.Empty);
	}

	[SecuritySafeCritical]
	public bool IsEtwEventEnabled(ref EventDescriptor eventDescriptor)
	{
		return IsEtwEventEnabled(ref eventDescriptor, fullCheck: true);
	}

	[SecuritySafeCritical]
	public bool IsEtwEventEnabled(ref EventDescriptor eventDescriptor, bool fullCheck)
	{
		if (fullCheck)
		{
			if (EtwTracingEnabled)
			{
				return etwProvider.IsEventEnabled(ref eventDescriptor);
			}
			return false;
		}
		if (EtwTracingEnabled)
		{
			return etwProvider.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords);
		}
		return false;
	}

	[SecuritySafeCritical]
	private void CreateTraceSource()
	{
		if (!string.IsNullOrEmpty(TraceSourceName))
		{
			SetTraceSource(new DiagnosticTraceSource(TraceSourceName));
		}
	}

	[SecurityCritical]
	private void CreateEtwProvider(Guid etwProviderId)
	{
		if (!(etwProviderId != Guid.Empty) || !isVistaOrGreater)
		{
			return;
		}
		etwProvider = (EtwProvider)etwProviderCache[etwProviderId];
		if (etwProvider == null)
		{
			lock (etwProviderCache)
			{
				etwProvider = (EtwProvider)etwProviderCache[etwProviderId];
				if (etwProvider == null)
				{
					etwProvider = new EtwProvider(etwProviderId);
					etwProviderCache.Add(etwProviderId, etwProvider);
				}
			}
		}
		this.etwProviderId = etwProviderId;
	}

	[SecurityCritical]
	private static EventDescriptor GetEventDescriptor(int eventId, TraceChannel channel, TraceEventLevel traceEventLevel)
	{
		long num = 0L;
		switch (channel)
		{
		case TraceChannel.Admin:
			num |= long.MinValue;
			break;
		case TraceChannel.Operational:
			num |= 0x4000000000000000L;
			break;
		case TraceChannel.Analytic:
			num |= 0x2000000000000000L;
			break;
		case TraceChannel.Debug:
			num |= 0x100000000000000L;
			break;
		case TraceChannel.Perf:
			num |= 0x800000000000000L;
			break;
		}
		return new EventDescriptor(eventId, 0, (byte)channel, (byte)traceEventLevel, 0, 0, num);
	}

	protected override void OnShutdownTracing()
	{
		ShutdownTraceSource();
		ShutdownEtwProvider();
	}

	private void ShutdownTraceSource()
	{
		try
		{
			if (TraceCore.AppDomainUnloadIsEnabled(this))
			{
				TraceCore.AppDomainUnload(this, AppDomain.CurrentDomain.FriendlyName, DiagnosticTraceBase.ProcessName, DiagnosticTraceBase.ProcessId.ToString(CultureInfo.CurrentCulture));
			}
			base.TraceSource.Flush();
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

	[SecuritySafeCritical]
	private void ShutdownEtwProvider()
	{
		try
		{
			if (etwProvider != null)
			{
				etwProvider.Dispose();
			}
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

	public override bool IsEnabled()
	{
		if (!TraceCore.TraceCodeEventLogCriticalIsEnabled(this) && !TraceCore.TraceCodeEventLogVerboseIsEnabled(this) && !TraceCore.TraceCodeEventLogInfoIsEnabled(this) && !TraceCore.TraceCodeEventLogWarningIsEnabled(this))
		{
			return TraceCore.TraceCodeEventLogErrorIsEnabled(this);
		}
		return true;
	}

	public override void TraceEventLogEvent(TraceEventType type, TraceRecord traceRecord)
	{
		switch (type)
		{
		case TraceEventType.Critical:
			if (TraceCore.TraceCodeEventLogCriticalIsEnabled(this))
			{
				TraceCore.TraceCodeEventLogCritical(this, traceRecord);
			}
			break;
		case TraceEventType.Verbose:
			if (TraceCore.TraceCodeEventLogVerboseIsEnabled(this))
			{
				TraceCore.TraceCodeEventLogVerbose(this, traceRecord);
			}
			break;
		case TraceEventType.Information:
			if (TraceCore.TraceCodeEventLogInfoIsEnabled(this))
			{
				TraceCore.TraceCodeEventLogInfo(this, traceRecord);
			}
			break;
		case TraceEventType.Warning:
			if (TraceCore.TraceCodeEventLogWarningIsEnabled(this))
			{
				TraceCore.TraceCodeEventLogWarning(this, traceRecord);
			}
			break;
		case TraceEventType.Error:
			if (TraceCore.TraceCodeEventLogErrorIsEnabled(this))
			{
				TraceCore.TraceCodeEventLogError(this, traceRecord);
			}
			break;
		}
	}

	protected override void OnUnhandledException(Exception exception)
	{
		if (TraceCore.UnhandledExceptionIsEnabled(this))
		{
			TraceCore.UnhandledException(this, (exception != null) ? exception.ToString() : string.Empty, exception);
		}
	}

	internal static string ExceptionToTraceString(Exception exception, int maxTraceStringLength)
	{
		StringBuilder stringBuilder = StringBuilderPool.Take();
		try
		{
			using StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.CurrentCulture);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			WriteExceptionToTraceString(xmlTextWriter, exception, maxTraceStringLength, 64);
			xmlTextWriter.Flush();
			stringWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			StringBuilderPool.Return(stringBuilder);
		}
	}

	private static void WriteExceptionToTraceString(XmlTextWriter xml, Exception exception, int remainingLength, int remainingAllowedRecursionDepth)
	{
		if (remainingAllowedRecursionDepth < 1 || !WriteStartElement(xml, "Exception", ref remainingLength))
		{
			return;
		}
		try
		{
			IList<Tuple<string, string>> list = new List<Tuple<string, string>>
			{
				new Tuple<string, string>("ExceptionType", DiagnosticTraceBase.XmlEncode(exception.GetType().AssemblyQualifiedName)),
				new Tuple<string, string>("Message", DiagnosticTraceBase.XmlEncode(exception.Message)),
				new Tuple<string, string>("StackTrace", DiagnosticTraceBase.XmlEncode(DiagnosticTraceBase.StackTraceString(exception))),
				new Tuple<string, string>("ExceptionString", DiagnosticTraceBase.XmlEncode(exception.ToString()))
			};
			if (exception is Win32Exception ex)
			{
				list.Add(new Tuple<string, string>("NativeErrorCode", ex.NativeErrorCode.ToString("X", CultureInfo.InvariantCulture)));
			}
			foreach (Tuple<string, string> item in list)
			{
				if (!WriteXmlElementString(xml, item.Item1, item.Item2, ref remainingLength))
				{
					return;
				}
			}
			if (exception.Data != null && exception.Data.Count > 0)
			{
				string exceptionData = GetExceptionData(exception);
				if (exceptionData.Length < remainingLength)
				{
					xml.WriteRaw(exceptionData);
					remainingLength -= exceptionData.Length;
				}
			}
			if (exception.InnerException != null)
			{
				string innerException = GetInnerException(exception, remainingLength, remainingAllowedRecursionDepth - 1);
				if (!string.IsNullOrEmpty(innerException) && innerException.Length < remainingLength)
				{
					xml.WriteRaw(innerException);
				}
			}
		}
		finally
		{
			xml.WriteEndElement();
		}
	}

	private static string GetInnerException(Exception exception, int remainingLength, int remainingAllowedRecursionDepth)
	{
		if (remainingAllowedRecursionDepth < 1)
		{
			return null;
		}
		StringBuilder stringBuilder = StringBuilderPool.Take();
		try
		{
			using StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.CurrentCulture);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			if (!WriteStartElement(xmlTextWriter, "InnerException", ref remainingLength))
			{
				return null;
			}
			WriteExceptionToTraceString(xmlTextWriter, exception.InnerException, remainingLength, remainingAllowedRecursionDepth);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
			stringWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			StringBuilderPool.Return(stringBuilder);
		}
	}

	private static string GetExceptionData(Exception exception)
	{
		StringBuilder stringBuilder = StringBuilderPool.Take();
		try
		{
			using StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.CurrentCulture);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteStartElement("DataItems");
			foreach (object key in exception.Data.Keys)
			{
				xmlTextWriter.WriteStartElement("Data");
				xmlTextWriter.WriteElementString("Key", DiagnosticTraceBase.XmlEncode(key.ToString()));
				if (exception.Data[key] == null)
				{
					xmlTextWriter.WriteElementString("Value", string.Empty);
				}
				else
				{
					xmlTextWriter.WriteElementString("Value", DiagnosticTraceBase.XmlEncode(exception.Data[key].ToString()));
				}
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
			stringWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			StringBuilderPool.Return(stringBuilder);
		}
	}

	private static bool WriteStartElement(XmlTextWriter xml, string localName, ref int remainingLength)
	{
		int num = localName.Length * 2 + 5;
		if (num <= remainingLength)
		{
			xml.WriteStartElement(localName);
			remainingLength -= num;
			return true;
		}
		return false;
	}

	private static bool WriteXmlElementString(XmlTextWriter xml, string localName, string value, ref int remainingLength)
	{
		int num = ((!string.IsNullOrEmpty(value) || LocalAppContextSwitches.IncludeNullExceptionMessageInETWTrace) ? (localName.Length * 2 + 5 + value.Length) : (localName.Length + 4));
		if (num <= remainingLength)
		{
			xml.WriteElementString(localName, value);
			remainingLength -= num;
			return true;
		}
		return false;
	}
}
