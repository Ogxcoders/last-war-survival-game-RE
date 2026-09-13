namespace System.Diagnostics.Tracing;

public class EventCounter : DiagnosticCounter
{
	public EventCounter(string name, EventSource eventSource)
		: base(name, eventSource)
	{
	}

	public void WriteMetric(float value)
	{
	}

	public void WriteMetric(double value)
	{
	}
}
