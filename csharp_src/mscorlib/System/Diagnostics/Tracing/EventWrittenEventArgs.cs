using System.Collections.ObjectModel;

namespace System.Diagnostics.Tracing;

public class EventWrittenEventArgs : EventArgs
{
	public Guid ActivityId => EventSource.CurrentThreadActivityId;

	public EventChannel Channel => EventChannel.None;

	public int EventId { get; internal set; }

	public long OSThreadId { get; internal set; }

	public DateTime TimeStamp { get; internal set; }

	public string EventName { get; internal set; }

	public EventSource EventSource { get; private set; }

	public EventKeywords Keywords => EventKeywords.None;

	public EventLevel Level => EventLevel.LogAlways;

	public string Message { get; internal set; }

	public EventOpcode Opcode => EventOpcode.Info;

	public ReadOnlyCollection<object> Payload { get; internal set; }

	public ReadOnlyCollection<string> PayloadNames { get; internal set; }

	public Guid RelatedActivityId { get; internal set; }

	public EventTags Tags => EventTags.None;

	public EventTask Task => EventTask.None;

	public byte Version => 0;

	internal EventWrittenEventArgs(EventSource eventSource)
	{
		EventSource = eventSource;
	}
}
