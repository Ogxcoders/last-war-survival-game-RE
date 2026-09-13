using System.Globalization;
using System.Resources;
using System.Runtime.Diagnostics;
using System.Security;
using System.Threading;

namespace System.Runtime.Serialization.Diagnostics.Application;

internal class TD
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
				resourceManager = new ResourceManager("System.Runtime.Serialization.Diagnostics.Application.TD", typeof(TD).Assembly);
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

	private TD()
	{
	}

	internal static bool ReaderQuotaExceededIsEnabled()
	{
		if (FxTrace.ShouldTraceError)
		{
			return IsEtwEventEnabled(0);
		}
		return false;
	}

	internal static void ReaderQuotaExceeded(string param0)
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(0))
		{
			WriteEtwEvent(0, null, param0, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCSerializeWithSurrogateStartIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(1);
		}
		return false;
	}

	internal static void DCSerializeWithSurrogateStart(string SurrogateType)
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(1))
		{
			WriteEtwEvent(1, null, SurrogateType, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCSerializeWithSurrogateStopIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(2);
		}
		return false;
	}

	internal static void DCSerializeWithSurrogateStop()
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(2))
		{
			WriteEtwEvent(2, null, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCDeserializeWithSurrogateStartIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(3);
		}
		return false;
	}

	internal static void DCDeserializeWithSurrogateStart(string SurrogateType)
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(3))
		{
			WriteEtwEvent(3, null, SurrogateType, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCDeserializeWithSurrogateStopIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(4);
		}
		return false;
	}

	internal static void DCDeserializeWithSurrogateStop()
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(4))
		{
			WriteEtwEvent(4, null, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool ImportKnownTypesStartIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(5);
		}
		return false;
	}

	internal static void ImportKnownTypesStart()
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(5))
		{
			WriteEtwEvent(5, null, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool ImportKnownTypesStopIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(6);
		}
		return false;
	}

	internal static void ImportKnownTypesStop()
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(6))
		{
			WriteEtwEvent(6, null, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCResolverResolveIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(7);
		}
		return false;
	}

	internal static void DCResolverResolve(string TypeName)
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(7))
		{
			WriteEtwEvent(7, null, TypeName, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCGenWriterStartIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(8);
		}
		return false;
	}

	internal static void DCGenWriterStart(string Kind, string TypeName)
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(8))
		{
			WriteEtwEvent(8, null, Kind, TypeName, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCGenWriterStopIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(9);
		}
		return false;
	}

	internal static void DCGenWriterStop()
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(9))
		{
			WriteEtwEvent(9, null, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCGenReaderStartIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(10);
		}
		return false;
	}

	internal static void DCGenReaderStart(string Kind, string TypeName)
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(10))
		{
			WriteEtwEvent(10, null, Kind, TypeName, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCGenReaderStopIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(11);
		}
		return false;
	}

	internal static void DCGenReaderStop()
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(11))
		{
			WriteEtwEvent(11, null, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCJsonGenReaderStartIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(12);
		}
		return false;
	}

	internal static void DCJsonGenReaderStart(string Kind, string TypeName)
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(12))
		{
			WriteEtwEvent(12, null, Kind, TypeName, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCJsonGenReaderStopIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(13);
		}
		return false;
	}

	internal static void DCJsonGenReaderStop()
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(13))
		{
			WriteEtwEvent(13, null, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCJsonGenWriterStartIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(14);
		}
		return false;
	}

	internal static void DCJsonGenWriterStart(string Kind, string TypeName)
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(14))
		{
			WriteEtwEvent(14, null, Kind, TypeName, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool DCJsonGenWriterStopIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(15);
		}
		return false;
	}

	internal static void DCJsonGenWriterStop()
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(15))
		{
			WriteEtwEvent(15, null, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool GenXmlSerializableStartIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(16);
		}
		return false;
	}

	internal static void GenXmlSerializableStart(string DCType)
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(16))
		{
			WriteEtwEvent(16, null, DCType, serializedPayload.AppDomainFriendlyName);
		}
	}

	internal static bool GenXmlSerializableStopIsEnabled()
	{
		if (FxTrace.ShouldTraceVerbose)
		{
			return IsEtwEventEnabled(17);
		}
		return false;
	}

	internal static void GenXmlSerializableStop()
	{
		TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
		if (IsEtwEventEnabled(17))
		{
			WriteEtwEvent(17, null, serializedPayload.AppDomainFriendlyName);
		}
	}

	[SecuritySafeCritical]
	private static void CreateEventDescriptors()
	{
		EventDescriptor[] ed = new EventDescriptor[18]
		{
			new EventDescriptor(1420, 0, 18, 2, 0, 2560, 2305843009217888256L),
			new EventDescriptor(5001, 0, 19, 5, 1, 2592, 1152921504606846978L),
			new EventDescriptor(5002, 0, 19, 5, 2, 2592, 1152921504606846978L),
			new EventDescriptor(5003, 0, 19, 5, 1, 2591, 1152921504606846978L),
			new EventDescriptor(5004, 0, 19, 5, 2, 2591, 1152921504606846978L),
			new EventDescriptor(5005, 0, 19, 5, 1, 2547, 1152921504606846978L),
			new EventDescriptor(5006, 0, 19, 5, 2, 2547, 1152921504606846978L),
			new EventDescriptor(5007, 0, 19, 5, 1, 2528, 1152921504606846978L),
			new EventDescriptor(5008, 0, 19, 5, 1, 2544, 1152921504606846978L),
			new EventDescriptor(5009, 0, 19, 5, 2, 2544, 1152921504606846978L),
			new EventDescriptor(5010, 0, 19, 5, 1, 2543, 1152921504606846978L),
			new EventDescriptor(5011, 0, 19, 5, 2, 2543, 1152921504606846978L),
			new EventDescriptor(5012, 0, 19, 5, 1, 2543, 1152921504606846978L),
			new EventDescriptor(5013, 0, 19, 5, 2, 2543, 1152921504606846978L),
			new EventDescriptor(5014, 0, 19, 5, 1, 2544, 1152921504606846978L),
			new EventDescriptor(5015, 0, 19, 5, 2, 2544, 1152921504606846978L),
			new EventDescriptor(5016, 0, 19, 5, 1, 2545, 1152921504606846978L),
			new EventDescriptor(5017, 0, 19, 5, 2, 2545, 1152921504606846978L)
		};
		ushort[] events = new ushort[0];
		FxTrace.UpdateEventDefinitions(ed, events);
		eventDescriptors = ed;
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

	private static bool IsEtwEventEnabled(int eventIndex)
	{
		if (FxTrace.Trace.IsEtwProviderEnabled)
		{
			EnsureEventDescriptors();
			return FxTrace.IsEventEnabled(eventIndex);
		}
		return false;
	}

	[SecuritySafeCritical]
	private static bool WriteEtwEvent(int eventIndex, EventTraceActivity eventParam0, string eventParam1, string eventParam2)
	{
		EnsureEventDescriptors();
		return FxTrace.Trace.EtwProvider.WriteEvent(ref eventDescriptors[eventIndex], eventParam0, eventParam1, eventParam2);
	}

	[SecuritySafeCritical]
	private static bool WriteEtwEvent(int eventIndex, EventTraceActivity eventParam0, string eventParam1)
	{
		EnsureEventDescriptors();
		return FxTrace.Trace.EtwProvider.WriteEvent(ref eventDescriptors[eventIndex], eventParam0, eventParam1);
	}

	[SecuritySafeCritical]
	private static bool WriteEtwEvent(int eventIndex, EventTraceActivity eventParam0, string eventParam1, string eventParam2, string eventParam3)
	{
		EnsureEventDescriptors();
		return FxTrace.Trace.EtwProvider.WriteEvent(ref eventDescriptors[eventIndex], eventParam0, eventParam1, eventParam2, eventParam3);
	}
}
