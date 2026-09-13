namespace System.Diagnostics.Tracing;

public class IncrementingPollingCounter : DiagnosticCounter
{
	public TimeSpan DisplayRateTimeScale { get; set; }

	public IncrementingPollingCounter(string name, EventSource eventSource, Func<double> totalValueProvider)
		: base(name, eventSource)
	{
	}
}
