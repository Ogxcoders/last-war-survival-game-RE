namespace System.Diagnostics.Tracing;

public class PollingCounter : DiagnosticCounter
{
	public PollingCounter(string name, EventSource eventSource, Func<double> metricProvider)
		: base(name, eventSource)
	{
	}
}
