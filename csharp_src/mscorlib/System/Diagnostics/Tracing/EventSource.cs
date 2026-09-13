using System.Collections.Generic;

namespace System.Diagnostics.Tracing;

public class EventSource : IDisposable
{
	protected internal struct EventData
	{
		public IntPtr DataPointer { get; set; }

		public int Size { get; set; }

		internal int Reserved { get; set; }
	}

	public Exception ConstructionException => null;

	public static Guid CurrentThreadActivityId => Guid.Empty;

	public Guid Guid => Guid.Empty;

	public string Name { get; private set; }

	public EventSourceSettings Settings { get; private set; }

	public event EventHandler<EventCommandEventArgs> EventCommandExecuted
	{
		add
		{
			throw new NotImplementedException();
		}
		remove
		{
			throw new NotImplementedException();
		}
	}

	protected EventSource()
	{
		Name = GetType().Name;
	}

	protected EventSource(bool throwOnEventWriteErrors)
		: this()
	{
	}

	protected EventSource(EventSourceSettings settings)
		: this()
	{
		Settings = settings;
	}

	protected EventSource(EventSourceSettings settings, params string[] traits)
		: this(settings)
	{
	}

	public EventSource(string eventSourceName)
	{
		Name = eventSourceName;
	}

	public EventSource(string eventSourceName, EventSourceSettings config)
		: this(eventSourceName)
	{
		Settings = config;
	}

	public EventSource(string eventSourceName, EventSourceSettings config, params string[] traits)
		: this(eventSourceName, config)
	{
	}

	internal EventSource(Guid eventSourceGuid, string eventSourceName)
		: this(eventSourceName)
	{
	}

	~EventSource()
	{
		Dispose(disposing: false);
	}

	public bool IsEnabled()
	{
		return false;
	}

	public bool IsEnabled(EventLevel level, EventKeywords keywords)
	{
		return false;
	}

	public bool IsEnabled(EventLevel level, EventKeywords keywords, EventChannel channel)
	{
		return false;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public string GetTrait(string key)
	{
		return null;
	}

	public void Write(string eventName)
	{
	}

	public void Write(string eventName, EventSourceOptions options)
	{
	}

	public void Write<T>(string eventName, T data)
	{
	}

	public void Write<T>(string eventName, EventSourceOptions options, T data)
	{
	}

	[CLSCompliant(false)]
	public void Write<T>(string eventName, ref EventSourceOptions options, ref T data)
	{
	}

	public void Write<T>(string eventName, ref EventSourceOptions options, ref Guid activityId, ref Guid relatedActivityId, ref T data)
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	protected virtual void OnEventCommand(EventCommandEventArgs command)
	{
	}

	internal void ReportOutOfBandMessage(string msg, bool flush)
	{
	}

	protected void WriteEvent(int eventId)
	{
		WriteEvent(eventId, new object[0]);
	}

	protected void WriteEvent(int eventId, byte[] arg1)
	{
		WriteEvent(eventId, new object[1] { arg1 });
	}

	protected void WriteEvent(int eventId, int arg1)
	{
		WriteEvent(eventId, new object[1] { arg1 });
	}

	protected void WriteEvent(int eventId, string arg1)
	{
		WriteEvent(eventId, new object[1] { arg1 });
	}

	protected void WriteEvent(int eventId, int arg1, int arg2)
	{
		WriteEvent(eventId, new object[2] { arg1, arg2 });
	}

	protected void WriteEvent(int eventId, int arg1, int arg2, int arg3)
	{
		WriteEvent(eventId, new object[3] { arg1, arg2, arg3 });
	}

	protected void WriteEvent(int eventId, int arg1, string arg2)
	{
		WriteEvent(eventId, new object[2] { arg1, arg2 });
	}

	protected void WriteEvent(int eventId, long arg1)
	{
		WriteEvent(eventId, new object[1] { arg1 });
	}

	protected void WriteEvent(int eventId, long arg1, byte[] arg2)
	{
		WriteEvent(eventId, new object[2] { arg1, arg2 });
	}

	protected void WriteEvent(int eventId, long arg1, long arg2)
	{
		WriteEvent(eventId, new object[2] { arg1, arg2 });
	}

	protected void WriteEvent(int eventId, long arg1, long arg2, long arg3)
	{
		WriteEvent(eventId, new object[3] { arg1, arg2, arg3 });
	}

	protected void WriteEvent(int eventId, long arg1, string arg2)
	{
		WriteEvent(eventId, new object[2] { arg1, arg2 });
	}

	protected void WriteEvent(int eventId, params object[] args)
	{
	}

	protected void WriteEvent(int eventId, string arg1, int arg2)
	{
		WriteEvent(eventId, new object[2] { arg1, arg2 });
	}

	protected void WriteEvent(int eventId, string arg1, int arg2, int arg3)
	{
		WriteEvent(eventId, new object[3] { arg1, arg2, arg3 });
	}

	protected void WriteEvent(int eventId, string arg1, long arg2)
	{
		WriteEvent(eventId, new object[2] { arg1, arg2 });
	}

	protected void WriteEvent(int eventId, string arg1, string arg2)
	{
		WriteEvent(eventId, new object[2] { arg1, arg2 });
	}

	protected void WriteEvent(int eventId, string arg1, string arg2, string arg3)
	{
		WriteEvent(eventId, new object[3] { arg1, arg2, arg3 });
	}

	[CLSCompliant(false)]
	protected unsafe void WriteEventCore(int eventId, int eventDataCount, EventData* data)
	{
	}

	protected void WriteEventWithRelatedActivityId(int eventId, Guid relatedActivityId, params object[] args)
	{
	}

	[CLSCompliant(false)]
	protected unsafe void WriteEventWithRelatedActivityIdCore(int eventId, Guid* relatedActivityId, int eventDataCount, EventData* data)
	{
	}

	public static string GenerateManifest(Type eventSourceType, string assemblyPathToIncludeInManifest)
	{
		throw new NotImplementedException();
	}

	public static string GenerateManifest(Type eventSourceType, string assemblyPathToIncludeInManifest, EventManifestOptions flags)
	{
		throw new NotImplementedException();
	}

	public static Guid GetGuid(Type eventSourceType)
	{
		throw new NotImplementedException();
	}

	public static string GetName(Type eventSourceType)
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<EventSource> GetSources()
	{
		throw new NotImplementedException();
	}

	public static void SendCommand(EventSource eventSource, EventCommand command, IDictionary<string, string> commandArguments)
	{
		throw new NotImplementedException();
	}

	public static void SetCurrentThreadActivityId(Guid activityId)
	{
		throw new NotImplementedException();
	}

	public static void SetCurrentThreadActivityId(Guid activityId, out Guid oldActivityThatWillContinue)
	{
		throw new NotImplementedException();
	}
}
