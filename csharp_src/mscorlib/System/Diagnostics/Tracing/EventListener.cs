using System.Collections.Generic;

namespace System.Diagnostics.Tracing;

public class EventListener : IDisposable
{
	public event EventHandler<EventSourceCreatedEventArgs> EventSourceCreated;

	public event EventHandler<EventWrittenEventArgs> EventWritten;

	public static int EventSourceIndex(EventSource eventSource)
	{
		return 0;
	}

	public void EnableEvents(EventSource eventSource, EventLevel level)
	{
	}

	public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword)
	{
	}

	public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword, IDictionary<string, string> arguments)
	{
	}

	public void DisableEvents(EventSource eventSource)
	{
	}

	protected internal virtual void OnEventSourceCreated(EventSource eventSource)
	{
	}

	protected internal virtual void OnEventWritten(EventWrittenEventArgs eventData)
	{
	}

	public virtual void Dispose()
	{
	}
}
