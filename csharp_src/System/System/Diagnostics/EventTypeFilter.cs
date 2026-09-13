namespace System.Diagnostics;

public class EventTypeFilter : TraceFilter
{
	private SourceLevels level;

	public SourceLevels EventType
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public EventTypeFilter(SourceLevels level)
	{
		this.level = level;
	}

	public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
	{
		return ((uint)eventType & (uint)level) != 0;
	}
}
