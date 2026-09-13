namespace System.Diagnostics.Tracing;

public class IncrementingEventCounter : DiagnosticCounter
{
	public TimeSpan DisplayRateTimeScale { get; set; }

	public IncrementingEventCounter(string name, EventSource eventSource)
		: base(name, eventSource)
	{
	}

	public void Increment(double increment = 1.0)
	{
	}
}
